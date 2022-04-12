using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels;

public class OrderViewModel
{
    [Required]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Phone { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }
}
