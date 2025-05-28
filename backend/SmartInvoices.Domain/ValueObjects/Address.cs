using System;

namespace SmartInvoices.Domain.ValueObjects
{
    /// <summary>
    /// Reprezentuje adres jako obiekt wartościowy
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Ulica
        /// </summary>
        public string Street { get; private set; } = string.Empty;

        /// <summary>
        /// Numer budynku
        /// </summary>
        public string BuildingNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Numer mieszkania (opcjonalny)
        /// </summary>
        public string ApartmentNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Kod pocztowy
        /// </summary>
        public string PostalCode { get; private set; } = string.Empty;

        /// <summary>
        /// Miasto
        /// </summary>
        public string City { get; private set; } = string.Empty;

        /// <summary>
        /// Kraj
        /// </summary>
        public string Country { get; private set; } = string.Empty;

        /// <summary>
        /// Prywatny konstruktor dla EF Core
        /// </summary>
        private Address() { }

        /// <summary>
        /// Tworzy nowy adres
        /// </summary>
        public static Address Create(string street, string buildingNumber, string postalCode, string city, string country, string? apartmentNumber = null)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty", nameof(street));

            if (string.IsNullOrWhiteSpace(buildingNumber))
                throw new ArgumentException("Building number cannot be empty", nameof(buildingNumber));

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty", nameof(city));

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty", nameof(country));

            return new Address
            {
                Street = street,
                BuildingNumber = buildingNumber,
                ApartmentNumber = apartmentNumber ?? string.Empty,
                PostalCode = postalCode,
                City = city,
                Country = country
            };
        }

        /// <summary>
        /// Zwraca pełny adres jako string
        /// </summary>
        public override string ToString()
        {
            string address = $"{Street} {BuildingNumber}";

            if (!string.IsNullOrEmpty(ApartmentNumber))
            {
                address += $"/{ApartmentNumber}";
            }

            address += $", {PostalCode} {City}, {Country}";

            return address;
        }
    }
}
