namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Attributes;

/// <summary>
/// better don't use exceptions
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ThrowsUnprocessableHttpRequestEnumAttribute : Attribute
{
    public Type EnumType { get; }
    public ThrowsUnprocessableHttpRequestEnumAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}