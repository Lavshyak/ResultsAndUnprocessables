using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class ResultOrUnprocessable<TOnSuccess, TEnumError> : IResultOrUnprocessable where TEnumError : Enum
{
    public bool IsSuccess { get; private set; }

    public TOnSuccess? Result { get; private set; }
    public UnprocessableHttpRequestInfo<TEnumError>? UnprocessableInfo { get; private set; }
    
    public TEnumError? Error { get; private set; }

    public object? ResultObject => Result;
    public object? UnprocessableInfoObject => UnprocessableInfo;

    public enum SomeErrors
    {
        A
    }

    public ResultOrUnprocessable<int, SomeErrors> A()
    {
        ResultOrUnprocessable<int, SomeErrors> c = null;
        return null;
    }

    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TEnumError te)
    {
        var fieldInfo = EnumDescriptionTools.GetEnumFieldInfo(te);
        
        var unprocessableInfo = new UnprocessableHttpRequestInfo<TEnumError>()
        {
            Code = fieldInfo.ValueInt,
            Name = fieldInfo.Name,
            Description = fieldInfo.Description,
            Enum = te
        };

        var roe = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = default,
            IsSuccess = false,
            UnprocessableInfo = unprocessableInfo,
            Error = te
        };

        return roe;
    }

    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TOnSuccess tr)
    {
        var roe = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = tr,
            IsSuccess = true
        };

        return roe;
    }
}