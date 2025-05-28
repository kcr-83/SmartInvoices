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
        public string Street { get; private set; }
        
        /// <summary>
        /// Numer budynku
        /// </summary>
        public string BuildingNumber { get; private set; }
        
        /// <summary>
        /// Numer lokalu (opcjonalnie)
        /// </summary>
        public string ApartmentNumber { get; private set; }
        
        /// <summary>
        /// Kod pocztowy
        /// </summary>
        public string PostalCode { get; private set; }
        
        /// <summary>
        /// Miasto
        /// </summary>
        public string City { get; private set; }
        
        /// <summary>
        /// Kraj
        /// </summary>
        public string Country { get; private set; }
        
        /// <summary>
        /// Prywatny konstruktor dla EF Core
        /// </summary>
        private Address() { }
        
        /// <summary>
        /// Tworzy nowy adres
        /// </summary>
        public Address(string street, string buildingNumber, string postalCode, string city, string country, string apartmentNumber = null)
        {
            Street = street;
            BuildingNumber = buildingNumber;
            PostalCode = postalCode;
            City = city;
            Country = country;
            ApartmentNumber = apartmentNumber;
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
