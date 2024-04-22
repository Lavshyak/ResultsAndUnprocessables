using System.Net.Mime;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.ExceptionHandlers;

/// <summary>
/// better don't use exceptions
/// </summary>
public class BadRequestExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
		CancellationToken cancellationToken)
	{
		if (exception is not UnprocessableHttpRequestException unprocessableHttpRequestException)
		{
			return false;
		}

		httpContext.Response.StatusCode = unprocessableHttpRequestException.StatusCode;

		httpContext.Response.ContentType = MediaTypeNames.Application.Json;
		await httpContext.Response.WriteAsync(exception.Message, cancellationToken);

		return true;
	}
}