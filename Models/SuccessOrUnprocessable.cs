using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class SuccessOrUnprocessable<TEnumError> : ISuccessOrUnprocessable where TEnumError : Enum
{
    public bool IsSuccess { get; private set; }
    public UnprocessableHttpRequestInfo? UnprocessableInfo { get; private set; }

    public object? UnprocessableInfoObject
    {
        get => UnprocessableInfo;
    }

    public static implicit operator SuccessOrUnprocessable<TEnumError>(TEnumError enumError)
    {
        var unprocessableInfo = new UnprocessableHttpRequestInfo()
        {
            Code = Convert.ToInt32(enumError),
            Description = EnumDescriptionTools.GetDescription(enumError)
        };

        var successOrUnprocessable = new SuccessOrUnprocessable<TEnumError>
        {
            IsSuccess = false,
            UnprocessableInfo = unprocessableInfo
        };

        return successOrUnprocessable;
    }

    public static implicit operator SuccessOrUnprocessable<TEnumError>(
        ClassSuccessForSuccessOrUnprocessable classSuccessForSuccessOrUnprocessable)
    {
        var successOrUnprocessable = new SuccessOrUnprocessable<TEnumError>
        {
            IsSuccess = true
        };

        return successOrUnprocessable;
    }
}