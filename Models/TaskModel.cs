
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace CarWorkshop.Models;

public class TaskModel
{
    public Guid Id { get; set; }

    public required Guid EmployeeId { get; set; }

    public required Guid TicketId { get; set; }

    public required string Description { get; set; }

    [Display(Name = "Work Time")]
    public required NpgsqlRange<DateTime> WorkTime { get; set; }

    [Display(Name = "Price Per Hour")]
    public required double PricePerHour { get; set; }

    [Display(Name = "Total Price")]
    public double TotalPrice => PricePerHour * (WorkTime.UpperBound - WorkTime.LowerBound).TotalHours;

    public required ApplicationUser Employee { get; set; }

    public required TicketModel Ticket { get; set; }

}

public class TaskInputModel
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid TicketId { get; set; }

    public required string Description { get; set; }

    public required TimeRange WorkTime { get; set; }

    public double PricePerHour { get; set; }

    public required ApplicationUser Employee { get; set; }

    public required TicketModel Ticket { get; set; }

}

public record TimeRange
{
    public DateTime UpperBound { get; set; }
    public DateTime LowerBound { get; set; }
}
