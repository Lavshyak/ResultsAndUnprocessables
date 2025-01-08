namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;

public interface IHaveCodeStatic
{
    public static abstract int CodeStatic { get; }
}

public interface IHaveCode
{
    public int Code { get; }
}

public interface ICreatableFromValueAndCode<TValue, TResult>
{
    public static abstract TResult CreateFromValue<TIHaveCodeStatic>(TValue value)
        where TIHaveCodeStatic : IHaveCodeStatic;
}

public interface IHaveValue<TValue>
{
    public TValue Value { get; }
}

public interface IHaveCodeAndValue<TValue> : IHaveValue<TValue>, IHaveCode
{
    
}

public interface IHaveCodeAndValueAndCreatableFromValue<TCode,TValue> : ICreatableFromValueAndCode<TValue, IHaveCodeAndValue<TValue>>,
    IHaveCodeAndValue<TValue>
    where TCode : IHaveCodeStatic
{
    
}

public class CodeAndValue<TValue> : IHaveCodeAndValue<TValue>
{
    public required int Code { get; init; }
    public required TValue Value { get; init;  }
}

public class Code200 : IHaveCodeStatic
{
    public static int CodeStatic => 200;
}

public class Code200<TValue> : Code200, IHaveCodeAndValue<TValue>, ICreatableFromValueAndCode<TValue, Code200<TValue>>
{
    public required TValue Value { get; init; }

    public Code200 CodeContainer { get; }
    public int Code { get; }
    public static Code200<TValue> CreateFromValue<TIHaveCodeStatic>(TValue value) where TIHaveCodeStatic : IHaveCodeStatic
    {
        return new Code200<TValue> { Value = value };
    }
}

public class Code422 : IHaveCode, IHaveCodeStatic
{
    public static int CodeStatic => 422;
    public int Code => CodeStatic;
}

public class Code422<TValue> : Code422, IHaveCodeAndValue<TValue>, ICreatableFromValueAndCode<TValue, Code422<TValue>>
{
    public required TValue Value { get; init; }

    public Code200 CodeContainer { get; }
    public int Code { get; }
    public static Code422<TValue> CreateFromValue<TIHaveCodeStatic>(TValue value) where TIHaveCodeStatic : IHaveCodeStatic
    {
        return new Code422<TValue> { Value = value };
    }
}

public class EndpointReturn<TCode1, TValue1, TCode2, TValue2>
    where TCode1 : class, IHaveCodeAndValue<TValue1>, ICreatableFromValueAndCode<TValue1, TCode1>, IHaveCodeStatic
    where TCode2 : class, IHaveCodeAndValue<TValue2>, ICreatableFromValueAndCode<TValue2, TCode2>, IHaveCodeStatic
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
            Code1 = TCode1.CreateFromValue<TCode1>(value1),
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
            Code2 = TCode2.CreateFromValue<TCode2>(value2),
        };
    }
}

public class EndpointReturn2<TCode1, TValue1, TCode2, TValue2>
    where TCode1 : class, IHaveCodeStatic
    where TCode2 : class, IHaveCodeStatic
{
    public required bool IsTCode1 { get; init; }
    public required bool IsTCode2 { get; init; }

    public required IHaveCodeAndValue<TValue1>? Code1 { get; init; }
    public required IHaveCodeAndValue<TValue2>? Code2 { get; init; }

    public static implicit operator EndpointReturn2<TCode1, TValue1, TCode2, TValue2>(TValue1 value1)
    {
        return new EndpointReturn2<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = new CodeAndValue<TValue1>()
            {
                Code = TCode1.CodeStatic,
                Value = value1
            },
            Code2 = null
        };
    }

    public static implicit operator EndpointReturn2<TCode1, TValue1, TCode2, TValue2>(TValue2 value2)
    {
        return new EndpointReturn2<TCode1, TValue1, TCode2, TValue2>()
        {
            IsTCode1 = true,
            IsTCode2 = false,
            Code1 = null,
            Code2 = new CodeAndValue<TValue2>()
            {
                Code = TCode2.CodeStatic,
                Value = value2
            },
        };
    }
}

public class Foo
{
    public EndpointReturn<Code200<int>, int, Code422<string>, string> Bar1()
    {
        return 4;
    }

    public EndpointReturn<Code200<int>, int, Code422<string>, string> Bar2()
    {
        return "hui";
    }
}

public class Foo2
{
    public EndpointReturn2<Code200, int, Code422, string> Bar1()
    {
        return 4;
    }

    public EndpointReturn2<Code200, int, Code422, string> Bar2()
    {
        return "hui";
    }
}