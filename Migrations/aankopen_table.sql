-- Voer dit uit in de Supabase SQL editor (of via psql) om de tabel voor
-- aankopen/betalingen aan te maken. Dit is dezelfde tabel die de
-- EF-migratie "20260624095018_InitialCreate" aanmaakt — gebruik dit script
-- als je de database liever direct in Supabase beheert.

CREATE TABLE IF NOT EXISTS aankopen (
    id            SERIAL PRIMARY KEY,
    user_id       text NOT NULL,
    order_id      text NOT NULL,
    status        text NOT NULL DEFAULT 'pending',
    amount        numeric NOT NULL DEFAULT 4.99,
    created_at    timestamptz NOT NULL DEFAULT now(),
    completed_at  timestamptz NULL
);

CREATE INDEX IF NOT EXISTS ix_aankopen_user_id ON aankopen (user_id);
CREATE UNIQUE INDEX IF NOT EXISTS ix_aankopen_order_id ON aankopen (order_id);

-- Optioneel: koppel order_id/user_id losjes aan gebruiker.id voor
-- referentiële integriteit. Niet verplicht, want user_id wordt als tekst
-- opgeslagen (consistent met de rest van het project).
-- ALTER TABLE aankopen
--     ADD CONSTRAINT fk_aankopen_gebruiker
--     FOREIGN KEY (user_id) REFERENCES gebruiker (id);
