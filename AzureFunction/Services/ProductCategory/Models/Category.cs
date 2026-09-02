using System.ComponentModel.DataAnnotations;

namespace AzureFunction.Services.ProductCategory.Models;

public class Category
{
	[Key]
	public Guid CategoryId { get; set; }
	public string CategoryName { get; set; } = default!;
	public int DisplayOrder { get; set; } = 99;
	public string? Description { get; set; }
	public int IsActive { get; set; } = 1;
}
