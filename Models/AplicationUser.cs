using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CarWorkshop.Models;


public class ApplicationUser : IdentityUser<Guid>
{
    public enum UserType
    {
        Admin,
        Employee,
    }

    public UserType Type { get; set; }

    [PersonalData]
    [Display(Name = "First Name")]
    public required string FirstName { get; set; }

    [PersonalData]
    [Display(Name = "Last Name")]
    public required string LastName { get; set; }

    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";

    [PersonalData]
    [Display(Name = "Hourly Rate")]
    public double? HourlyRate { get; set; }

    public required ICollection<TaskModel> Tasks { get; set; }

}

