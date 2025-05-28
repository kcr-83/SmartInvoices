namespace SmartInvoices.Application.Common;

/// <summary>
/// Klasa reprezentująca wynik operacji z wartością, który może zakończyć się sukcesem lub niepowodzeniem.
/// </summary>
/// <typeparam name="T">Typ wartości wyniku</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Wartość wyniku.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Konstruktor klasy Result z wartością.
    /// </summary>
    /// <param name="value">Wartość wyniku</param>
    /// <param name="isSuccess">Czy operacja zakończyła się sukcesem</param>
    /// <param name="errors">Lista błędów</param>
    protected Result(T? value, bool isSuccess, List<string> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    /// <summary>
    /// Tworzy wynik sukcesu z wartością.
    /// </summary>
    /// <param name="value">Wartość wyniku</param>
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, new List<string>());
    }

    /// <summary>
    /// Tworzy wynik niepowodzenia z pojedynczym błędem.
    /// </summary>
    /// <param name="error">Komunikat błędu</param>
    public static new Result<T> Failure(string error)
    {
        return new Result<T>(default, false, new List<string> { error });
    }

    /// <summary>
    /// Tworzy wynik niepowodzenia z listą błędów.
    /// </summary>
    /// <param name="errors">Lista błędów</param>
    public static new Result<T> Failure(List<string> errors)
    {
        return new Result<T>(default, false, errors);
    }
}
