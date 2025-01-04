using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class ResultOrUnprocessable<TOnSuccess, TEnumError> : IResultOrUnprocessable where TEnumError : Enum
{
    public bool IsSuccess { get; private set; }
    
    public TOnSuccess? Result { get; private set; }
    public UnprocessableHttpRequestInfo? UnprocessableInfo { get; private set; }

    public object? ResultObject => Result;
    public object? UnprocessableInfoObject => UnprocessableInfo;

    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TEnumError enumError)
    {
        var unprocessableInfo = new UnprocessableHttpRequestInfo()
        {
            Code = Convert.ToInt32(enumError),
            Description = EnumDescriptionTools.GetDescription(enumError)
        };

        var resultOrUnprocessable = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = default,
            IsSuccess = false,
            UnprocessableInfo = unprocessableInfo
        };

        return resultOrUnprocessable;
    }

    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TOnSuccess onSuccess)
    {
        var resultOrUnprocessable = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = onSuccess,
            IsSuccess = true,
        };

        return resultOrUnprocessable;
    }
}