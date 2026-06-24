using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChronoTrial.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // gebruiker en leaderboard bestaan al in de Supabase-database
            // (buiten EF migraties om aangemaakt). We maken ze hier alleen
            // aan als ze nog niet bestaan, zodat deze migratie ook op een
            // verse database werkt zonder de bestaande data aan te raken.
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS gebruiker (
                    id SERIAL PRIMARY KEY,
                    username text NOT NULL,
                    email text NOT NULL,
                    wachtwoord text NOT NULL,
                    purchased boolean NOT NULL DEFAULT false
                );
            ");

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IF NOT EXISTS ix_gebruiker_username ON gebruiker (username);
            ");

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IF NOT EXISTS ix_gebruiker_email ON gebruiker (email);
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS leaderboard (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""Username"" text NOT NULL,
                    ""UserId"" text NOT NULL,
                    ""Time"" text NOT NULL,
                    ""Date"" timestamp with time zone NOT NULL,
                    created_at timestamp with time zone NOT NULL DEFAULT now()
                );
            ");

            // Nieuw: tabel voor aankopen/betalingen. Elke poging om de game
            // te kopen (StartPayment) en elke voltooide betaling
            // (SimulatePaid) wordt hier vastgelegd.
            migrationBuilder.CreateTable(
                name: "aankopen",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    order_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "pending"),
                    amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 4.99m),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aankopen", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aankopen_user_id",
                table: "aankopen",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_aankopen_order_id",
                table: "aankopen",
                column: "order_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aankopen");

            // gebruiker en leaderboard bewust niet droppen: die bestonden al
            // voor deze migratie en bevatten echte gebruikersdata.
        }
    }
}
