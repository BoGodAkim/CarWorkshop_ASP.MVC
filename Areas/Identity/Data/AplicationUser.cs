using System;
using Microsoft.AspNetCore.Identity;


namespace CarWorkshop.Areas.Identity.Data;


public class ApplicationUser : IdentityUser<Guid>
{
    public enum UserType
    {
        Admin,
        Employee,
    }

    public UserType Type { get; set; }

    [PersonalData]
    public required string FirstName { get; set; }

    [PersonalData]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [PersonalData]
    public double? HourlyRate { get; set; }

}

