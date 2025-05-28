// Dodaj tę konfigurację w miejscu rejestracji kontekstu bazy danych
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); // Włączenie logowania danych wrażliwych
});
