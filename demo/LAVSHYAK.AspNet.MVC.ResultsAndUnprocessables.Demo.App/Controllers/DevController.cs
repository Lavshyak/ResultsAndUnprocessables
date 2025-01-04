using System.ComponentModel;
using System.Net.Mime;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Demo.App.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DevController : ControllerBase
{
    public enum SomeErrors
    {
        [Description("A error")] A,
        B
    }

    [HttpGet]
    public ResultOrUnprocessable<FileResult, SomeErrors> GetFileOrError()
    {
        return File([2, 3, 5, 6, 2], MediaTypeNames.Application.Octet);
    }

    [HttpGet]
    public ResultOrUnprocessable<string, SomeErrors> GetError()
    {
        return SomeErrors.A;
    }

    [HttpGet]
    public ResultOrUnprocessable<string, SomeErrors> GetString()
    {
        return "xdd";
    }

    [HttpGet]
    public async Task<ResultOrUnprocessable<string, SomeErrors>> GetErrorA()
    {
        await Task.Delay(10);
        return SomeErrors.A;
    }

    [HttpGet]
    public async Task<ResultOrUnprocessable<string, SomeErrors>> GetStringA()
    {
        await Task.Delay(10);
        return "xdd";
    }

    [HttpGet]
    public async Task<SuccessOrUnprocessable<SomeErrors>> GetSError()
    {
        await Task.Delay(10);
        return SomeErrors.A;
    }

    [HttpGet]
    public async Task<SuccessOrUnprocessable<SomeErrors>> GetSSuccess()
    {
        await Task.Delay(10);
        return SUCCESS;
    }
}