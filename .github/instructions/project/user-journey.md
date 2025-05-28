# Podróże Użytkownika w Systemie Zarządzania Fakturami

Poniższe diagramy przedstawiają typowe ścieżki użytkowników podczas korzystania z Systemu Zarządzania Fakturami. Diagramy ilustrują poziom satysfakcji użytkownika na każdym etapie.

## Standardowy Użytkownik

### Przeglądanie i Zarządzanie Fakturami

```mermaid
journey
    title Przeglądanie i Zarządzanie Fakturami
    section Logowanie
      Otworzenie strony logowania: 3: Użytkownik
      Wprowadzenie danych logowania: 3: Użytkownik
      Pomyślne zalogowanie: 4: Użytkownik
    section Przeglądanie Faktur
      Przejście do listy faktur: 4: Użytkownik
      Filtrowanie faktur wg daty: 5: Użytkownik
      Sortowanie faktur wg kwoty: 5: Użytkownik
      Znalezienie poszukiwanej faktury: 5: Użytkownik
    section Szczegóły Faktury
      Otwarcie szczegółów faktury: 4: Użytkownik
      Przeglądanie pozycji faktury: 4: Użytkownik
      Eksport faktury do PDF: 5: Użytkownik
```

### Składanie Wniosku o Zmianę Pozycji Faktury

```mermaid
journey
    title Składanie Wniosku o Zmianę Pozycji Faktury
    section Wybór Faktury
      Przejście do listy faktur: 4: Użytkownik
      Wyszukanie faktury do modyfikacji: 3: Użytkownik
      Otwarcie szczegółów faktury: 4: Użytkownik
    section Tworzenie Wniosku o Zmianę
      Zaznaczenie pozycji do zmiany: 4: Użytkownik
      Określenie rodzaju zmiany: 3: Użytkownik
      Podanie uzasadnienia: 3: Użytkownik
      Złożenie wniosku o zmianę: 4: Użytkownik
    section Śledzenie Wniosku
      Przejście do listy wniosków: 4: Użytkownik
      Sprawdzenie statusu wniosku: 3: Użytkownik
      Otrzymanie powiadomienia o rozpatrzeniu: 5: Użytkownik
```

### Składanie Wniosku o Zwrot

```mermaid
journey
    title Składanie Wniosku o Zwrot
    section Wybór Faktury
      Przejście do listy faktur: 4: Użytkownik
      Wyszukanie faktury do zwrotu: 3: Użytkownik
      Otwarcie szczegółów faktury: 4: Użytkownik
    section Tworzenie Wniosku o Zwrot
      Wybór opcji "Złóż wniosek o zwrot": 4: Użytkownik
      Podanie powodu zwrotu: 3: Użytkownik
      Dołączenie dokumentacji: 2: Użytkownik
      Złożenie wniosku o zwrot: 4: Użytkownik
    section Śledzenie Wniosku
      Przejście do listy wniosków o zwrot: 4: Użytkownik
      Sprawdzenie statusu wniosku: 3: Użytkownik
      Otrzymanie powiadomienia o rozpatrzeniu: 5: Użytkownik
```

## Administrator

### Zarządzanie Użytkownikami

```mermaid
journey
    title Zarządzanie Użytkownikami
    section Logowanie Administratora
      Otworzenie strony logowania: 3: Administrator
      Wprowadzenie danych logowania: 3: Administrator
      Pomyślne zalogowanie: 4: Administrator
    section Zarządzanie Kontami
      Przejście do panelu administracyjnego: 4: Administrator
      Przeglądanie listy użytkowników: 4: Administrator
      Dodanie nowego użytkownika: 3: Administrator
      Modyfikacja uprawnień użytkownika: 3: Administrator
      Dezaktywacja konta użytkownika: 3: Administrator
```

### Zarządzanie Wnioskami

```mermaid
journey
    title Zarządzanie Wnioskami
    section Przeglądanie Wniosków
      Przejście do panelu administracyjnego: 4: Administrator
      Filtrowanie wniosków wg typu: 5: Administrator
      Filtrowanie wniosków wg statusu: 5: Administrator
    section Obsługa Wniosku o Zmianę
      Otwarcie szczegółów wniosku o zmianę: 4: Administrator
      Analiza uzasadnienia zmiany: 3: Administrator
      Weryfikacja danych faktury: 2: Administrator
      Akceptacja lub odrzucenie wniosku: 4: Administrator
      Dodanie komentarza uzasadniającego: 4: Administrator
    section Obsługa Wniosku o Zwrot
      Otwarcie szczegółów wniosku o zwrot: 4: Administrator
      Analiza powodu zwrotu: 3: Administrator
      Przeglądanie załączonej dokumentacji: 3: Administrator
      Akceptacja lub odrzucenie wniosku: 4: Administrator
      Dodanie komentarza uzasadniającego: 4: Administrator
```

## Objaśnienie Poziomu Satysfakcji

Skala 1-5 oznacza poziom satysfakcji użytkownika na danym etapie:
1. Bardzo niezadowolony
2. Niezadowolony
3. Neutralny
4. Zadowolony
5. Bardzo zadowolony

Diagramy pokazują, w których momentach procesu użytkownicy doświadczają najwyższego poziomu satysfakcji, a gdzie mogą pojawiać się trudności lub frustracje wymagające uwagi w projektowaniu interfejsu.
