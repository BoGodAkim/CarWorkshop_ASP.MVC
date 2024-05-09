
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWorkshop.Models;

public class TicketModel
{
    public enum Status : uint
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Canceled = 3
    }

    public Guid Id { get; set; }

    [Display(Name = "Car Brand")]
    public required string CarBrand { get; set; }

    [Display(Name = "Car Model")]
    public required string CarModel { get; set; }

    [Display(Name = "Car Year")]
    public required int Year { get; set; }

    [Display(Name = "License Plate")]
    public required string LicensePlate { get; set; }

    public required string Description { get; set; }

    [Display(Name = "Paid Amount")]
    public required double? PaidAmount { get; set; }

    [Display(Name = "Ticket Status")]
    public required Status TicketStatus { get; set; }

    [Display(Name = "Estimation")]
    public required Estimation Estimation { get; set; }

    public required ICollection<PartModel> Parts { get; set; }

    public required ICollection<TaskModel> Tasks { get; set; }

}

[ComplexType]
public record Estimation
{
    [Display(Name = "Estimated Price")]
    public double? Price { get; set; }

    [Display(Name = "Estimation Description")]
    public string? Description { get; set; }

    [Display(Name = "Is Approved")]
    public bool IsApproved { get; set; } = false;
}    
