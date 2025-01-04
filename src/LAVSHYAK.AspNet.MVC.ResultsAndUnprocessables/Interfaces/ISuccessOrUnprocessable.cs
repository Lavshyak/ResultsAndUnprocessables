namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

public interface ISuccessOrUnprocessable
{
    public bool IsSuccess { get; }
    
    public object? UnprocessableInfoObject { get; }
}