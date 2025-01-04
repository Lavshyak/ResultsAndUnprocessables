namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

public interface IResultOrUnprocessable : ISuccessOrUnprocessable
{
    public object? ResultObject { get; }
}