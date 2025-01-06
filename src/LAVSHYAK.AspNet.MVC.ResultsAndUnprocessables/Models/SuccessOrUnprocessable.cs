using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class SuccessOrUnprocessable<TEnumError> : ISuccessOrUnprocessable where TEnumError : Enum
{
    public bool IsSuccess { get; private set; }
    public UnprocessableHttpRequestInfo<TEnumError>? UnprocessableInfo { get; private set; }
    public object? UnprocessableInfoObject { get=> UnprocessableInfo; }
    
    public TEnumError? Error { get; private set; }
    
    public static implicit operator SuccessOrUnprocessable<TEnumError>(TEnumError te)
    {
        var fieldInfo = EnumDescriptionTools.GetEnumFieldInfo(te);
        
        var unprocessableInfo = new UnprocessableHttpRequestInfo<TEnumError>()
        {
            Code = fieldInfo.ValueInt,
            Name = fieldInfo.Name,
            Description = fieldInfo.Description,
            Enum = te
        };
        
        var vou = new SuccessOrUnprocessable<TEnumError>
        {
            IsSuccess = false,
            UnprocessableInfo = unprocessableInfo,
            Error = te
        };

        return vou;
    }
    
    public static implicit operator SuccessOrUnprocessable<TEnumError>(ClassSuccessForSuccessOrUnprocessable classSuccessForSuccessOrUnprocessable)
    {
        var vou = new SuccessOrUnprocessable<TEnumError>
        {
            IsSuccess = true,
            Error = default
        };

        return vou;
    }
}