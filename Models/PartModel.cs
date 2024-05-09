
using System.ComponentModel.DataAnnotations;

namespace CarWorkshop.Models;

public class PartModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required double Price { get; set; }

    public required double Quantity { get; set; }

    public required Guid TicketId { get; set; }

    public required TicketModel Ticket { get; set; }

    [Display(Name = "Total Price")]
    public double TotalPrice => Price * Quantity;

}
