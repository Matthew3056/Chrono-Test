using Microsoft.EntityFrameworkCore;
using ChronoTrial.Data;
using ChronoTrial.Models;

namespace ChronoTrial.Services;

/// <summary>
/// Gesimuleerde betaalservice - werkt zonder echte Mollie account.
/// Voor productie: vervang door echte Mollie integratie.
/// </summary>
public class PaymentService
{
    private readonly ApplicationDbContext _db;

    public PaymentService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Maak een nep-betaling aan, sla 'm op als "pending" in de database en geef het order ID terug
    public async Task<string> CreateOrderAsync(string userId)
    {
        var orderId = $"CT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        _db.Purchases.Add(new Purchase
        {
            UserId = userId,
            OrderId = orderId,
            Status = "pending",
            Amount = 4.99m,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        return orderId;
    }

    // Simuleer een geslaagde betaling (demo modus): markeer de purchase als voltooid
    // en geef de gebruiker toegang tot de game. Beide wijzigingen gaan in dezelfde
    // transactie naar de database, zodat de aankoophistorie en de toegang altijd
    // synchroon blijven.
    public async Task<bool> CompletePaymentAsync(string orderId, string userId)
    {
        if (!int.TryParse(userId, out var parsedUserId))
            return false;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == parsedUserId);
        if (user == null)
            return false;

        var purchase = await _db.Purchases.FirstOrDefaultAsync(p => p.OrderId == orderId && p.UserId == userId);
        if (purchase == null)
            return false;

        purchase.Status = "completed";
        purchase.CompletedAt = DateTime.UtcNow;
        user.Purchased = true;

        await _db.SaveChangesAsync();
        return true;
    }

    // Heeft deze gebruiker de game al gekocht? Wordt gebruikt door zowel de
    // koop- als de speelpagina om te bepalen wat ze te zien krijgen.
    public async Task<bool> HasPurchasedAsync(string userId)
    {
        if (!int.TryParse(userId, out var parsedUserId))
            return false;

        return await _db.Users.AnyAsync(u => u.Id == parsedUserId && u.Purchased);
    }
}
