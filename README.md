# Bibliotekssystem-T9-Library System
Detta projekt är ett bibliotekssystem utvecklat med **ASP.NET Core MVC** och flera separata **Web API-tjänster**.  
Frontend-klienten fungerar som användarens gränssnitt där böcker kan visas, lån kan hanteras och delar av systemet är skyddade bakom inloggning.

## Live Demo / Huvudsida
**MVC-klienten (huvudwebbplatsen):**  
https://library-system.azurewebsites.net/

## Demo Accounts

Use the following accounts to log in and test the system:

### Admin Account
- **Email:** `terry@admin.com`
- **Password:** `Password123!`

### User Account
- **Email:** `andreas@user.com`
- **Password:** `Password123!`


## Projektöversikt

Systemet består av:

- En **MVC-klient** som fungerar som frontend
- Flera separata **Web API-tjänster**
- Varje tjänst har eget ansvar, egen logik och egen databas
- Kommunikation mellan klient och tjänster sker via **HTTP-anrop**

## Teknologier

- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Azure App Service

## Funktionalitet

- Visa böcker i biblioteket
- Hantera användare och inloggning
- Skapa och hantera lån
- Visa och hantera recensioner för böcker
- Skicka och hantera notifikationer i systemet
- Full CRUD i respektive webbtjänst
- Skyddade delar av webbplatsen för inloggade användare

## Gruppmedlemmar och tjänster

| Namn | Ansvarig tjänst | Publicerad API | Repo |
|------|------------------|----------------|------|
| Kim Sandblom | CatalogService | https://library-catalog-service.azurewebsites.net | https://github.com/Skimlyy/library-catalog-service |
| Angelinne Fors | AccountService | https://user-service-t9.azurewebsites.net | https://github.com/Angie-nin/Bibliotekssystem-T9-UserService |
| Zakarea Alammour | LoanService | https://loan-service.azurewebsites.net | https://github.com/Zakarea123/LoanService |
| Henrik Glahns(MoshuGosu) | ReviewService | https://book-review-service.azurewebsites.net | https://github.com/MoshuGosu/Review-service |
| Joakim Danling | NotificationService | https://notification-service-t9.azurewebsites.net | https://github.com/jockedanling/bibliotekssystem-notification-service |

## Arkitektur

Varje webbtjänst har en egen databas och ansvarar för sin egen data.  
Tjänsterna kommunicerar inte genom delade databaser utan genom API-anrop, vilket ger tydlig ansvarsfördelning och lös koppling mellan systemets delar.

## Deployment

Både MVC-klienten och webbtjänsterna är publicerade på Azure och tillgängliga via nätet.
