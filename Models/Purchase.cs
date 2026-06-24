using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoTrial.Models;

/// <summary>
/// Eén aankoop-/betaalpoging van een gebruiker. Wordt aangemaakt zodra de
/// betaling gestart wordt (status "pending") en bijgewerkt naar "completed"
/// zodra de betaling geslaagd is. Dit is de database-koppeling voor de
/// betaalfunctie: hier staat zwart-op-wit wie wat gekocht heeft en wanneer.
/// </summary>
[Table("aankopen")]
public class Purchase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [Column("order_id")]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    public string Status { get; set; } = "pending"; // pending | completed | cancelled

    [Column("amount")]
    public decimal Amount { get; set; } = 4.99m;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }
}
