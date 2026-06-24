# GameFiles

Zet hier het bestand dat spelers kunnen downloaden nadat ze Chrono Trials
gekocht hebben:

```
GameFiles/ChronoTrials.zip
```

Deze map staat **buiten** `wwwroot`, dus dit bestand is niet rechtstreeks
via een URL te benaderen. Downloads gaan via het endpoint
`GET /api/download/game` (zie `Program.cs`), dat eerst controleert of de
ingelogde gebruiker `Purchased = true` heeft in de database voordat het
bestand teruggegeven wordt. Zo kan niemand de download omzeilen door simpelweg
de bestandsnaam te raden.

Zolang `ChronoTrials.zip` hier niet staat, geeft de downloadknop op `/game`
een duidelijke 404-melding ("Het downloadbestand is nog niet beschikbaar").
De betaal- en toegangslogica werkt dan al wel correct.
