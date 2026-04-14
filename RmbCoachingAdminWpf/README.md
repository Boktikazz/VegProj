# RMB Coaching Admin WPF

Egyszerű .NET 8 WPF admin kliens a megadott RMBCoahcingBackend projekthez.

## Tudás
- admin bejelentkezés
- admin kurzuslista betöltése
- új kurzus létrehozása
- kurzus szerkesztése
- kurzus inaktiválása

## Fontos
A backend URL alapból ez:

```csharp
https://localhost:7145/
```

Ha nálad más porton fut, ezt itt írd át:

- `Helpers/AppConfig.cs`

## Indítás
1. Indítsd el a backendet.
2. Visual Studio-ban nyisd meg ezt a WPF projektet.
3. Állítsd be startup projectnek.
4. Futtasd.

## Admin jogosultság
A backend logikája alapján az első regisztrált user `Admin` szerepet kap.
Ha nincs még admin usered, előbb a backend `/api/auth/register` végpontján hozz létre egyet.
