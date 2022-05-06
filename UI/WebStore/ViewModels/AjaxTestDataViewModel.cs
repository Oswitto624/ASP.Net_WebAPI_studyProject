namespace WebStore.ViewModels;

public class AjaxTestDataViewModel
{
    public int Id { get; init; }

    public string Message { get; init; } = null!;

    public DateTime ServerTime { get; init; } = DateTime.Now;
}
