namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class UnprocessableHttpRequestInfo
{
    public required int Code { get; set; }
    public required string Description { get; set; }
}