namespace AzureFunction.ExceptionsQueue;

public class ExceptionMessage
{
	public Guid CategoryId { get; set; }
	public string CategoryName { get; set; } = default!;
	public int DisplayOrder { get; set; } = 99;
	public string? Description { get; set; }
	public int IsActive { get; set; } = 1;
	public ExceptionServiceMsg ExceptionMsg { get; set; }=default!;	
}

public class ExceptionServiceMsg
{
	public string ExceptionType { get; set; } = default!;
	public string ExceptionMessageText { get; set; } = default!;
	public string? StackTrace { get; set; } = default!;
	public DateTime OccurredAt { get; set; }
}
