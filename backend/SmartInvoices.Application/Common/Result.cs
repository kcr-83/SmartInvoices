namespace SmartInvoices.Application.Common;

/// <summary>
/// Klasa reprezentująca wynik operacji, który może zakończyć się sukcesem lub niepowodzeniem.
/// </summary>
public class Result
{
    /// <summary>
    /// Określa, czy operacja zakończyła się sukcesem.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Lista błędów, które wystąpiły podczas operacji.
    /// </summary>
    public List<string> Errors { get; }

    /// <summary>
    /// Konstruktor klasy Result.
    /// </summary>
    /// <param name="isSuccess">Czy operacja zakończyła się sukcesem</param>
    /// <param name="errors">Lista błędów</param>
    protected Result(bool isSuccess, List<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Tworzy wynik sukcesu.
    /// </summary>
    public static Result Success()
    {
        return new Result(true, new List<string>());
    }

    /// <summary>
    /// Tworzy wynik niepowodzenia z pojedynczym błędem.
    /// </summary>
    /// <param name="error">Komunikat błędu</param>
    public static Result Failure(string error)
    {
        return new Result(false, new List<string> { error });
    }

    /// <summary>
    /// Tworzy wynik niepowodzenia z listą błędów.
    /// </summary>
    /// <param name="errors">Lista błędów</param>
    public static Result Failure(List<string> errors)
    {
        return new Result(false, errors);
    }
}
