namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Experiments.EndpointReturnCodeWithValueOrCodeWithValue;

public interface IHaveCodeStatic
{
    public static abstract int CodeStatic { get; }
}

public interface IHaveCode
{
    public int Code { get; }
}

public interface IHaveValue<TValue>
{
    public TValue Value { get; }
}

public interface IHaveCodeAndValue<TValue> : IHaveValue<TValue>, IHaveCode
{
    
}

public class CodeAndValue<TCode, TValue> : IHaveCodeAndValue<TValue>
{
    public required int Code { get; init; }
    public required TValue Value { get; init;  }
}

public interface ICodeAndValueCreatable<TCode>
where TCode : IHaveCodeStatic
{
    public static CodeAndValue<TCode, TValue> With<TValue>(TValue value)
    {
        return new CodeAndValue<TCode, TValue>()
        {
            Code = TCode.CodeStatic,
            Value = value
        };
    }
}

public class Code200 : IHaveCodeStatic, ICodeAndValueCreatable<Code200>
{
    public static int CodeStatic => 200;

    public static CodeAndValue<Code200,TValue> With<TValue>(TValue value) => ICodeAndValueCreatable<Code200>.With(value);
}

public class Code422 : IHaveCodeStatic, ICodeAndValueCreatable<Code422>
{
    public static int CodeStatic => 422;
    
    public static CodeAndValue<Code422,TValue> With<TValue>(TValue value) => ICodeAndValueCreatable<Code422>.With(value);
}

public class EndpointReturn<TCode1, TValue1, TCode2, TValue2>
    where TCode1 : class, IHaveCodeStatic
    where TCode2 : class, IHaveCodeStatic
{
    public required bool IsTCode1 { get; init; }
    public required bool IsTCode2 { get; init; }

    public required IHaveCodeAndValue<TValue1>? Code1 { get; init; }
    public required IHaveCodeAndValue<TValue2>? Code2 { get; init; }

    public static implicit operator EndpointReturn<TCode1, TValue1, TCode2, TValue2>(TValue1 value1)
    {
        return new EndpointReturn<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = new CodeAndValue<TCode1, TValue1>()
            {
                Code = TCode1.CodeStatic,
                Value = value1
            },
            Code2 = null
        };
    }
    
    public static implicit operator EndpointReturn<TCode1, TValue1, TCode2, TValue2>(CodeAndValue<TCode1, TValue1> codeAndValue)
    {
        return new EndpointReturn<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = codeAndValue,
            Code2 = null
        };
    }

    public static implicit operator EndpointReturn<TCode1, TValue1, TCode2, TValue2>(TValue2 value2)
    {
        return new EndpointReturn<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = null,
            Code2 = new CodeAndValue<TCode2, TValue2>()
            {
                Code = TCode2.CodeStatic,
                Value = value2
            },
        };
    }
    
    public static implicit operator EndpointReturn<TCode1, TValue1, TCode2, TValue2>(CodeAndValue<TCode2, TValue2> codeAndValue)
    {
        return new EndpointReturn<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = null,
            Code2 = codeAndValue,
        };
    }
}

public class Empty
{
    private Empty() { }
    public static Empty Instance { get; } = new();
}

public class Foo
{
    public EndpointReturn<Code200, int, Code422, string> Bar1()
    {
        return 55;
    }

    public EndpointReturn<Code200, int, Code422, string> Bar2()
    {
        return "hui";
    }
    
    public EndpointReturn<Code200, int, Code422, string> Bar11()
    {
        return Code200.With(4);
    }
    
    public EndpointReturn<Code200, int, Code422, int> Bar333()
    {
        return Code422.With(8);
    }
    
    public EndpointReturn<Code200, Empty, Code422, int> Bar444()
    {
        return Code200.With(Empty.Instance);
    }
}