# Prompty do Implementacji Projektu SmartInvoices

Ten dokument zawiera zestaw promptów pomocnych przy implementacji projektu SmartInvoices od podstaw. Prompty są pogrupowane według etapów rozwoju projektu i warstw architektury.

## Spis treści
- [Prompty do Implementacji Projektu SmartInvoices](#prompty-do-implementacji-projektu-smartinvoices)
  - [Spis treści](#spis-treści)
  - [1. Konfiguracja Projektu](#1-konfiguracja-projektu)
    - [1.1 Utworzenie Struktury Rozwiązania](#11-utworzenie-struktury-rozwiązania)
    - [1.2 Konfiguracja Warstwy Domenowej](#12-konfiguracja-warstwy-domenowej)
    - [1.3 Konfiguracja Warstwy Aplikacji](#13-konfiguracja-warstwy-aplikacji)
    - [1.4 Konfiguracja Warstwy Infrastruktury](#14-konfiguracja-warstwy-infrastruktury)
    - [1.5 Konfiguracja API](#15-konfiguracja-api)
  - [2. Implementacja Backendu](#2-implementacja-backendu)
    - [2.1 Warstwa Domenowa](#21-warstwa-domenowa)
    - [2.2 Warstwa Aplikacji](#22-warstwa-aplikacji)
    - [2.3 Warstwa Infrastruktury](#23-warstwa-infrastruktury)
    - [2.4 Warstwa WebAPI](#24-warstwa-webapi)
  - [3. Implementacja Frontendu](#3-implementacja-frontendu)
    - [3.1 Konfiguracja Podstawowa](#31-konfiguracja-podstawowa)
    - [3.2 Komponenty Core](#32-komponenty-core)
    - [3.3 Moduły Funkcjonalne](#33-moduły-funkcjonalne)
    - [3.4 Integracja z API](#34-integracja-z-api)
  - [4. Implementacja Funkcjonalności](#4-implementacja-funkcjonalności)
    - [4.1 Zarządzanie Fakturami](#41-zarządzanie-fakturami)
    - [4.2 Wnioski o Zmiany w Pozycjach Faktury](#42-wnioski-o-zmiany-w-pozycjach-faktury)
    - [4.3 Wnioski o Pełne Zwroty](#43-wnioski-o-pełne-zwroty)
    - [4.4 Panel Administracyjny](#44-panel-administracyjny)
  - [5. Testy](#5-testy)
    - [5.1 Testy Jednostkowe](#51-testy-jednostkowe)
    - [5.2 Testy Integracyjne](#52-testy-integracyjne)
    - [5.3 Testy UI](#53-testy-ui)
  - [6. Dokumentacja](#6-dokumentacja)
    - [6.1 Dokumentacja API](#61-dokumentacja-api)
    - [6.2 Dokumentacja Techniczna](#62-dokumentacja-techniczna)

## 1. Konfiguracja Projektu

### 1.1 Utworzenie Struktury Rozwiązania

```
Utwórz strukturę rozwiązania dla projektu SmartInvoices zgodnie z zasadami Clean Architecture. Potrzebuję następujące projekty:
- SmartInvoices.Domain (biblioteka klas .NET Core)
- SmartInvoices.Application (biblioteka klas .NET Core)
- SmartInvoices.Persistence (biblioteka klas .NET Core)
- SmartInvoices.Infrastructure (biblioteka klas .NET Core)
- SmartInvoices.Common (biblioteka klas .NET Core)
- SmartInvoices.WebApi (projekt WebAPI .NET Core)
- SmartInvoices.Service (projekt Worker Service .NET Core)

Projekty powinny mieć odpowiednie referencje zgodne z zasadami Clean Architecture, gdzie warstwy wewnętrzne nie zależą od warstw zewnętrznych.
```

### 1.2 Konfiguracja Warstwy Domenowej

```
W projekcie SmartInvoices.Domain utwórz podstawową strukturę folderów dla modeli domenowych zgodnie z Clean Architecture:
1. Entities - dla głównych encji biznesowych
2. ValueObjects - dla obiektów wartości
3. Exceptions - dla wyjątków domenowych
4. Events - dla zdarzeń domenowych
5. Interfaces - dla interfejsów repozytoriów
6. Enums - dla typów wyliczeniowych

Następnie utwórz podstawowe typy wyliczeniowe potrzebne w aplikacji, takie jak InvoiceStatus, RequestStatus i RequestType.
```

### 1.3 Konfiguracja Warstwy Aplikacji

```
Skonfiguruj warstwę aplikacji SmartInvoices.Application, która będzie służyć jako warstwa pośrednia między domeną a infrastrukturą:

1. Dodaj referencję do projektu SmartInvoices.Domain
2. Utwórz następującą strukturę folderów:
   - Common/Mediator - dla implementacji własnego mediatora
   - DTOs - dla obiektów transferu danych
   - Features - dla funkcjonalności CQRS, podzielone na podkatalogi według funkcjonalności
   - Interfaces - dla interfejsów usług infrastrukturalnych
   - Validators - dla logiki walidacji
   - Behaviors - dla zachowań potoków mediatorów

3. Zaimplementuj prostą wersję wzorca mediatora dla CQRS (bez użycia MediatR, które jest płatne)
```

### 1.4 Konfiguracja Warstwy Infrastruktury

```
Skonfiguruj warstwę infrastruktury SmartInvoices.Persistence i SmartInvoices.Infrastructure:

1. W projekcie SmartInvoices.Persistence:
   - Dodaj referencję do SmartInvoices.Domain i SmartInvoices.Application
   - Zaimplementuj DbContext dla Entity Framework Core
   - Utwórz bazową konfigurację dla mapowania encji
   - Utwórz strukturę folderów dla repozytoriów i konfiguracji encji
   - Przygotuj klasę DependencyInjection do rejestracji usług

2. W projekcie SmartInvoices.Infrastructure:
   - Dodaj referencję do SmartInvoices.Domain i SmartInvoices.Application
   - Utwórz strukturę dla implementacji usług zewnętrznych
   - Przygotuj klasę DependencyInjection do rejestracji usług
```

### 1.5 Konfiguracja API

```
Skonfiguruj projekt SmartInvoices.WebApi:

1. Dodaj referencje do wszystkich niezbędnych projektów (Domain, Application, Persistence, Infrastructure, Common)
2. Skonfiguruj Program.cs z odpowiednią konfiguracją usług i middleware:
   - Dodaj obsługę kontrolerów
   - Skonfiguruj Swagger
   - Skonfiguruj CORS
   - Dodaj obsługę wyjątków
   - Zintegruj zależności z innych warstw używając ich metod rozszerzających DependencyInjection
3. Utwórz bazową strukturę folderów dla kontrolerów
4. Zaimplementuj podstawowy model odpowiedzi API (ApiResponse)
```

## 2. Implementacja Backendu

### 2.1 Warstwa Domenowa

```
Zaimplementuj podstawowe encje domenowe dla systemu SmartInvoices:

1. Faktura (Invoice):
   - Właściwości: Id, Number, IssueDate, DueDate, Status, TotalAmount, Tax, Customer, Items
   - Metody: CalculateTotal, AddItem, RemoveItem

2. Pozycja faktury (InvoiceItem):
   - Właściwości: Id, InvoiceId, Description, Quantity, UnitPrice, TotalPrice, Tax
   - Metody: CalculatePrice

3. Wniosek o zmianę (ChangeRequest):
   - Właściwości: Id, RequestDate, Status, UserId, InvoiceId, ItemsToChange, Reason, Comments
   - Metody: Approve, Reject, AddItem, RemoveItem

4. Wniosek o zwrot (RefundRequest):
   - Właściwości: Id, RequestDate, Status, UserId, InvoiceId, Reason, Documents, Comments
   - Metody: Approve, Reject, AddDocument

5. Użytkownik (User):
   - Właściwości: Id, Email, FirstName, LastName, Role, IsActive
   - Metody: Activate, Deactivate, ChangeRole

Encje powinny implementować logikę biznesową zgodnie z zasadami DDD, zapewniając spójność danych i enkapsulację.
```

```
Zaimplementuj interfejsy repozytoriów dla podstawowych encji w projekcie SmartInvoices.Domain:

1. IInvoiceRepository:
   - GetAllAsync
   - GetByIdAsync
   - GetByNumberAsync
   - GetWithItemsAsync
   - AddAsync
   - UpdateAsync
   - DeleteAsync

2. IRequestRepository (generic dla wniosków o zmiany i zwroty):
   - GetAllAsync
   - GetByIdAsync
   - GetByUserIdAsync
   - GetByInvoiceIdAsync
   - GetPendingAsync
   - AddAsync
   - UpdateAsync

3. IUserRepository:
   - GetAllAsync
   - GetByIdAsync
   - GetByEmailAsync
   - AddAsync
   - UpdateAsync
   - DeleteAsync

Interfejsy powinny zawierać metody typowe dla repozytoriów, z odpowiednimi parametrami i typami zwracanymi.
```

### 2.2 Warstwa Aplikacji

```
Zaimplementuj DTOs (Data Transfer Objects) dla głównych encji w projekcie SmartInvoices.Application:

1. InvoiceDto - DTO dla faktury
2. InvoiceItemDto - DTO dla pozycji faktury
3. ChangeRequestDto - DTO dla wniosku o zmianę
4. RefundRequestDto - DTO dla wniosku o zwrot
5. UserDto - DTO dla użytkownika

DTOs powinny zawierać tylko te pola, które są potrzebne na interfejsie użytkownika, oraz metody mapujące z/do encji domenowych (zamiast AutoMappera, który jest płatny).
```

```
Zaimplementuj podstawową strukturę CQRS dla funkcjonalności związanych z fakturami w projekcie SmartInvoices.Application:

1. Zapytania (Queries):
   - GetInvoicesQuery i GetInvoicesQueryHandler - do pobierania listy faktur z filtrowaniem
   - GetInvoiceDetailQuery i GetInvoiceDetailQueryHandler - do pobierania szczegółów faktury

2. Komendy (Commands):
   - CreateInvoiceCommand i CreateInvoiceCommandHandler - do tworzenia nowej faktury
   - UpdateInvoiceCommand i UpdateInvoiceCommandHandler - do aktualizacji faktury

Użyj własnej implementacji mediatora zamiast biblioteki MediatR. Zaimplementuj również walidację komend za pomocą FluentValidation.
```

### 2.3 Warstwa Infrastruktury

```
Zaimplementuj warstwę persystencji w projekcie SmartInvoices.Persistence:

1. SmartInvoicesDbContext - główna klasa kontekstu dla Entity Framework Core
2. Konfiguracje encji (InvoiceConfiguration, InvoiceItemConfiguration, itd.)
3. Implementacje repozytoriów zdefiniowanych w warstwie domenowej
4. Migracje początkowe

Pamiętaj o prawidłowej konfiguracji relacji między encjami, kluczy głównych i obcych oraz indeksów.
```

```
Zaimplementuj usługi infrastrukturalne w projekcie SmartInvoices.Infrastructure:

1. EmailService - do wysyłania powiadomień email
2. PdfService - do generowania faktur w formacie PDF
3. FileStorage - do przechowywania załączników i dokumentów
4. JwtHandler - do obsługi tokenów uwierzytelniania
5. IdentityService - do zarządzania tożsamością użytkowników

Usługi powinny implementować interfejsy zdefiniowane w warstwie aplikacji.
```

### 2.4 Warstwa WebAPI

```
Zaimplementuj kontrolery API w projekcie SmartInvoices.WebApi:

1. InvoicesController - do zarządzania fakturami
   - GET /api/invoices - pobieranie listy faktur z filtrowaniem
   - GET /api/invoices/{id} - pobieranie szczegółów faktury
   - POST /api/invoices - tworzenie nowej faktury
   - PUT /api/invoices/{id} - aktualizacja faktury
   - DELETE /api/invoices/{id} - usuwanie faktury
   - GET /api/invoices/{id}/pdf - generowanie PDF

2. RequestsController - do zarządzania wnioskami
   - Endpointy dla wniosków o zmiany
   - Endpointy dla wniosków o zwroty

3. UsersController - do zarządzania użytkownikami
   - Endpointy dla administracji użytkownikami

4. AuthController - do uwierzytelniania
   - Endpointy dla logowania i rejestracji

Kontrolery powinny być jak najcieńsze, delegując logikę biznesową do warstwy aplikacji poprzez mediatr.
```

## 3. Implementacja Frontendu

### 3.1 Konfiguracja Podstawowa

```
Utwórz nowy projekt Angular dla aplikacji SmartInvoices:

1. Utwórz nowy projekt Angular używając Angular CLI:
   ng new smart-invoices --routing --style=scss

2. Dodaj niezbędne biblioteki:
   - Angular Material do komponentów UI
   - NgRx do zarządzania stanem aplikacji
   - RxJS do programowania reaktywnego

3. Skonfiguruj strukturę projektu zgodnie z Angular Style Guide:
   - core/ - dla usług i modeli podstawowych
   - shared/ - dla współdzielonych komponentów
   - features/ - dla modułów funkcjonalnych
   - admin/ - dla funkcji administracyjnych

4. Skonfiguruj routing dla głównych sekcji aplikacji
```

### 3.2 Komponenty Core

```
Zaimplementuj podstawowe modele i usługi w folderze core/:

1. Modele:
   - invoice.model.ts - interfejs dla faktury
   - line-item.model.ts - interfejs dla pozycji faktury
   - change-request.model.ts - interfejs dla wniosku o zmianę
   - refund-request.model.ts - interfejs dla wniosku o zwrot
   - user.model.ts - interfejs dla użytkownika

2. Usługi:
   - auth.service.ts - do uwierzytelniania
   - invoice.service.ts - do komunikacji z API dla faktur
   - request.service.ts - do komunikacji z API dla wniosków
   - user.service.ts - do komunikacji z API dla użytkowników

3. Guards:
   - auth.guard.ts - do ochrony tras wymagających uwierzytelniania
   - admin.guard.ts - do ochrony tras administracyjnych
```

### 3.3 Moduły Funkcjonalne

```
Zaimplementuj główne komponenty dla zarządzania fakturami:

1. InvoiceListComponent:
   - Wyświetlanie listy faktur z filtrowaniem i sortowaniem
   - Paginacja dla dużych zestawów danych
   - Opcje: podgląd szczegółów, eksport do PDF

2. InvoiceDetailComponent:
   - Wyświetlanie szczegółów faktury
   - Lista pozycji faktury
   - Przyciski akcji: złóż wniosek o zmianę, złóż wniosek o zwrot

3. InvoiceFilterComponent:
   - Formularze filtrowania według różnych kryteriów
   - Zapisywanie preferencji filtrowania
```

```
Zaimplementuj komponenty do obsługi wniosków:

1. ChangeRequestFormComponent:
   - Formularz do wyboru pozycji do zmiany
   - Pole opisu zmiany i uzasadnienia
   - Obsługa załączników

2. RefundRequestFormComponent:
   - Formularz wniosku o zwrot
   - Pole opisu przyczyny zwrotu
   - Obsługa załączników dokumentujących

3. RequestListComponent:
   - Lista złożonych wniosków z filtrowaniem
   - Wskaźniki statusu wniosku
   - Możliwość podglądu szczegółów wniosku
```

### 3.4 Integracja z API

```
Zaimplementuj integrację z API dla usługi faktur (invoice.service.ts):

1. Metody do pobierania danych:
   - getInvoices(filters) - pobieranie listy faktur z filtrowaniem
   - getInvoiceById(id) - pobieranie szczegółów faktury
   - downloadInvoicePdf(id) - pobieranie faktury w formacie PDF

2. Obsługa błędów:
   - Implementacja interceptora do obsługi błędów HTTP
   - Wyświetlanie odpowiednich komunikatów dla użytkownika

3. Transformacja danych:
   - Mapowanie odpowiedzi API na modele frontendu
   - Formatowanie dat i wartości pieniężnych
```

## 4. Implementacja Funkcjonalności

### 4.1 Zarządzanie Fakturami

```
Zaimplementuj funkcjonalność przeglądania faktur w aplikacji:

1. W warstwie backendowej:
   - Kompletne query i handler dla GetInvoicesQuery
   - Filtrowanie faktur według daty, kwoty i statusu
   - Paginacja wyników
   - Sortowanie według różnych kolumn

2. W warstwie frontendowej:
   - Komponent listy faktur z tabelą
   - Mechanizm paginacji i sortowania
   - Komponenty filtrów z walidacją
   - Zapamiętywanie ustawień filtrowania dla użytkownika
```

```
Zaimplementuj funkcjonalność szczegółów faktury i eksportu do PDF:

1. W warstwie backendowej:
   - Query i handler dla GetInvoiceDetailQuery
   - Usługa generowania PDF z faktury (PdfService)
   - Endpoint do pobierania faktury w formacie PDF

2. W warstwie frontendowej:
   - Komponent szczegółów faktury z sekcjami dla danych ogólnych i pozycji
   - Przycisk do eksportu PDF
   - Wyświetlanie podsumowań i sum
   - Przyciski akcji dla składania wniosków
```

### 4.2 Wnioski o Zmiany w Pozycjach Faktury

```
Zaimplementuj funkcjonalność wniosków o zmiany w pozycjach faktury:

1. W warstwie backendowej:
   - Command i handler dla CreateChangeRequestCommand
   - Walidacja wniosku (sprawdzenie czy faktura istnieje, czy pozycje są prawidłowe)
   - Logika biznesowa obsługi wniosku
   - Powiadomienia email o nowym wniosku

2. W warstwie frontendowej:
   - Formularz z wyborem pozycji do zmiany
   - Interfejs do określania rodzaju zmiany (ilość, cena, opis)
   - Pole uzasadnienia
   - Walidacja formularza i komunikaty błędów
```

```
Zaimplementuj funkcjonalność śledzenia statusu wniosków o zmiany:

1. W warstwie backendowej:
   - Query i handler dla GetChangeRequestsQuery
   - Query i handler dla GetChangeRequestDetailQuery
   - Filtrowanie wniosków według statusu i daty

2. W warstwie frontendowej:
   - Lista wniosków z informacjami o statusie
   - Komponent szczegółów wniosku
   - Wskaźniki wizualne statusu (np. kolorowe etykiety)
   - Powiadomienia o zmianie statusu wniosku
```

### 4.3 Wnioski o Pełne Zwroty

```
Zaimplementuj funkcjonalność wniosków o pełne zwroty:

1. W warstwie backendowej:
   - Command i handler dla CreateRefundRequestCommand
   - Walidacja wniosku (sprawdzenie czy faktura kwalifikuje się do zwrotu)
   - Logika biznesowa obsługi wniosku
   - Obsługa załączników i dokumentacji

2. W warstwie frontendowej:
   - Formularz wniosku o zwrot
   - Interfejs do uploadu dokumentów
   - Pole uzasadnienia
   - Walidacja formularza i komunikaty błędów
```

```
Zaimplementuj funkcjonalność śledzenia statusu wniosków o zwroty:

1. W warstwie backendowej:
   - Query i handler dla GetRefundRequestsQuery
   - Query i handler dla GetRefundRequestDetailQuery
   - Pobieranie załączonych dokumentów

2. W warstwie frontendowej:
   - Lista wniosków o zwrot z informacjami o statusie
   - Komponent szczegółów wniosku o zwrot
   - Wyświetlanie załączników
   - Powiadomienia o zmianie statusu wniosku
```

### 4.4 Panel Administracyjny

```
Zaimplementuj panel zarządzania użytkownikami dla administratorów:

1. W warstwie backendowej:
   - Query i handler dla GetUsersQuery
   - Command i handler dla CreateUserCommand, UpdateUserCommand, DeactivateUserCommand
   - Zarządzanie uprawnieniami

2. W warstwie frontendowej:
   - Komponent listy użytkowników z filtrowaniem i sortowaniem
   - Formularze dodawania i edycji użytkowników
   - Przyciski do aktywacji/deaktywacji kont
   - Formularz zarządzania uprawnieniami
```

```
Zaimplementuj panel zarządzania wnioskami dla administratorów:

1. W warstwie backendowej:
   - Query do pobierania wszystkich wniosków
   - Command i handler dla ApproveRequestCommand i RejectRequestCommand
   - Dodawanie komentarzy do wniosków

2. W warstwie frontendowej:
   - Komponent listy wszystkich wniosków z zaawansowanym filtrowaniem
   - Interfejs do zatwierdzania/odrzucania wniosków
   - Formularz dodawania komentarzy
   - Dashboard ze statystykami wniosków
```

## 5. Testy

### 5.1 Testy Jednostkowe

```
Napisz testy jednostkowe dla głównych encji domenowych:

1. Testy dla Invoice:
   - Test metody CalculateTotal
   - Test metod AddItem i RemoveItem
   - Test walidacji stanu encji

2. Testy dla ChangeRequest:
   - Test metod Approve i Reject
   - Test logiki biznesowej związanej ze zmianą pozycji

3. Testy dla RefundRequest:
   - Test metod Approve i Reject
   - Test walidacji załączników
```

```
Napisz testy jednostkowe dla handlerów komend i zapytań:

1. Testy dla GetInvoicesQueryHandler:
   - Test filtrowania faktur
   - Test paginacji wyników
   - Test sortowania wyników

2. Testy dla CreateChangeRequestCommandHandler:
   - Test poprawnego tworzenia wniosku
   - Test walidacji (np. nieprawidłowa faktura, nieprawidłowe pozycje)
   - Test obsługi błędów
```

### 5.2 Testy Integracyjne

```
Napisz testy integracyjne dla warstwy persystencji:

1. Testy dla repozytoriów:
   - Test zapisywania i odczytu encji
   - Test filtrowania i sortowania
   - Test relacji między encjami

2. Testy dla DbContext:
   - Test konfiguracji relacji
   - Test mapowania właściwości
   - Test kaskadowego usuwania
```

```
Napisz testy integracyjne dla kontrolerów API:

1. Testy dla InvoicesController:
   - Test pobierania listy faktur
   - Test pobierania szczegółów faktury
   - Test tworzenia i aktualizacji faktury

2. Testy dla RequestsController:
   - Test składania wniosków
   - Test pobierania statusu wniosków
   - Test aktualizacji statusu wniosku
```

### 5.3 Testy UI

```
Napisz testy dla komponentów Angular:

1. Testy dla InvoiceListComponent:
   - Test renderowania listy faktur
   - Test działania filtrów
   - Test paginacji i sortowania

2. Testy dla formularzy wniosków:
   - Test walidacji formularzy
   - Test submitowania formularzy
   - Test obsługi błędów
```

## 6. Dokumentacja

### 6.1 Dokumentacja API

```
Skonfiguruj i rozbuduj dokumentację API Swagger:

1. Dodaj szczegółowe opisy dla wszystkich endpointów
2. Zdefiniuj przykładowe odpowiedzi i zapytania
3. Pogrupuj endpointy według funkcjonalności
4. Dodaj autoryzację do dokumentacji Swagger
5. Wersjonuj API w dokumentacji
```

### 6.2 Dokumentacja Techniczna

```
Utwórz dokumentację ADR (Architecture Decision Record) dla kluczowych decyzji:

1. Wybór architektury Clean Architecture
2. Implementacja własnego mediatora zamiast MediatR
3. Strategia obsługi błędów i wyjątków
4. Podejście do walidacji danych wejściowych
5. Strategia testowania
```

```
Utwórz dokumentację dla zespołu deweloperskiego:

1. Instrukcje instalacji i konfiguracji środowiska
2. Opis struktury projektu i konwencji
3. Proces dodawania nowych funkcjonalności
4. Procedury testowania i wdrażania
5. Najlepsze praktyki specyficzne dla projektu
```
