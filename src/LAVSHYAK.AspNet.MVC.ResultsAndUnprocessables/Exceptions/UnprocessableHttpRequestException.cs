using System.Text.Json;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.AspNetCore.Http;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Exceptions;

/// <summary>
/// better don't use exceptions
/// </summary>
public class UnprocessableHttpRequestException : BadHttpRequestException
{
	private UnprocessableHttpRequestException(string infoJson, UnprocessableHttpRequestInfo info,
		int statusCode = StatusCodes.Status422UnprocessableEntity) : base(infoJson, statusCode)
	{
		Info = info;
	}
	
	public UnprocessableHttpRequestInfo Info { get; }
	
	public static UnprocessableHttpRequestException FromEnum<TEnum>(TEnum value) where TEnum : Enum
	{
		var info = new UnprocessableHttpRequestInfo()
		{
			Code = Convert.ToInt32(value),
			Description = EnumDescriptionTools.GetDescription(value)
		};

		var json = JsonSerializer.Serialize(info);
		return new UnprocessableHttpRequestException(json, info);
	}
}