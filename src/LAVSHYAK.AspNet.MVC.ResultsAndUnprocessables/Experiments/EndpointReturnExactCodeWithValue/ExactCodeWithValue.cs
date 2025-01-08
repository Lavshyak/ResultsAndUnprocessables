namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Experiments.EndpointReturnExactCodeWithValue;

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

public class Code200 : IHaveCode, IHaveCodeStatic, ICodeAndValueCreatable<Code200>
{
    public static int CodeStatic => 200;
    public int Code => CodeStatic;

    public static implicit operator Code200(Empty empty)
    {
        return new Code200();
    }
}

public class Code200<TValue> : Code200, IHaveCodeAndValue<TValue>
{
    public TValue Value { get; }

    public Code200(TValue value)
    {
        Value = value;
    }
    
    public static implicit operator Code200<TValue>(TValue value)
    {
        return new Code200<TValue>(value);
    }
}

public class Code422 : IHaveCode, IHaveCodeStatic, ICodeAndValueCreatable<Code422>
{
    public static int CodeStatic => 422;
    public int Code => CodeStatic;
    
    public static implicit operator Code422(Empty empty)
    {
        return new Code422();
    }
}

public class Code422<TValue> : Code422, IHaveCodeAndValue<TValue>
{
    public TValue Value { get; }

    public Code422(TValue value)
    {
        Value = value;
    }
    
    public static implicit operator Code422<TValue>(TValue value)
    {
        return new Code422<TValue>(value);
    }
}

public class Empty
{
    private Empty() { }
    public static Empty Instance { get; } = new();
}

public class Foo
{
    public Code200<int> Bar1()
    {
        return 55;
    }

    public Code422<string> Bar2()
    {
        return "hui";
    }
    
    public Code200 Bar11()
    {
        return new Code200();
    }
    
    public Code422 Bar22()
    {
        return new Code422();
    }
    
    public Code200 Bar111()
    {
        return Empty.Instance;
    }
    
    public Code422 Bar222()
    {
        return Empty.Instance;
    }
}