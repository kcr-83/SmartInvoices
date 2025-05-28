using SmartInvoices.Application.DTOs;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów User na UserDto i odwrotnie.
/// </summary>
public static class UserMappings
{
    /// <summary>
    /// Mapuje obiekt User na UserDto.
    /// </summary>
    /// <param name="user">Użytkownik do zmapowania</param>
    /// <returns>Zmapowany obiekt UserDto</returns>
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.UserId,
            Username = user.FirstName + " " + user.LastName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            //Roles = user.Roles.ToList(),
            IsActive = user.IsActive
        };
    }

    /// <summary>
    /// Mapuje listę obiektów User na listę UserDto.
    /// </summary>
    /// <param name="users">Lista użytkowników do zmapowania</param>
    /// <returns>Zmapowana lista UserDto</returns>
    public static List<UserDto> ToDto(this IEnumerable<User> users)
    {
        return users?.Select(user => user.ToDto()).ToList() ?? new List<UserDto>();
    }
}
