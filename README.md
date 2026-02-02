# For community: 
You can change the namespace, repository name and project names. The owner in the LICENSE can be changed towards the community.
# LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables
Currently for .NET 8.\
Either analog for endpoints. Auto swagger documentation.
Returns 200 + model in body if succes. 422 + error info in body if unprocessable.

AddControllers setup example on configuring builder:
```csharp
services.AddControllers(options =>
		{
			options.OutputFormatters.RemoveType<StringOutputFormatter>(); // suddenly it won't work without this line
			options.OutputFormatters.Insert(0, new SuccessOrUnprocessableOutputFormatter());
			options.OutputFormatters.Insert(0, new ResultOrUnprocessableOutputFormatter());
		});
```

swagger setup example on configuring builder:
```csharp
services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API V1", Version = "v1" });

				var basePath = Directory.GetCurrentDirectory();

				options.IncludeXmlComments(Path.Combine(basePath, "WebApi.xml"), true); //for documentation of models, etc.
				
				options.OperationFilter<ResultOrUnprocessableOperationFilter>();
				options.OperationFilter<SuccessOrUnprocessableOperationFilter>();
				options.OperationFilter<FileResultOperationFilter>();
			});
```

your .csproj
```xml
<ItemGroup> <Using Include="LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Globals"> <Static>True</Static> </Using> </ItemGroup>
```

Controller example:
```csharp
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
```
Generated docs example:\
<img width="803" height="879" alt="изображение" src="https://github.com/user-attachments/assets/23fde683-78ff-46ab-8346-279397108a06" />
