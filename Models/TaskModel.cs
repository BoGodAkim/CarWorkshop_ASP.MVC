
using NpgsqlTypes;

namespace CarWorkshop.Models;

public class TaskModel
{
    public Guid Id { get; set; }

    public required Guid EmployeeId { get; set; }

    public required Guid TicketId { get; set; }

    public required string Description { get; set; }

    public required NpgsqlRange<DateTime> WorkTime { get; set; }

    public required double PricePerHour { get; set; }

    public double TotalPrice => PricePerHour * (WorkTime.UpperBound - WorkTime.LowerBound).TotalHours;

    public required ApplicationUser Employee { get; set; }

    public required TicketModel Ticket { get; set; }

}
