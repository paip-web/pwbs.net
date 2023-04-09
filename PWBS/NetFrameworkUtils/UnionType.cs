namespace PWBS.NetFrameworkUtils;

public struct UnionType<T1>
{
    private readonly T1? _t1;
    public UnionType(T1 t1)
    {
        _t1 = t1;
    }
    public object? getValue()
    {
        return _t1;
    }
}

public struct UnionType<T1, T2>
{
    private readonly T1? _t1;
    private readonly T2? _t2;
    
    public UnionType(T1 t1)
    {
        _t1 = t1;
    }
    public UnionType(T2 t2)
    {
        _t2 = t2;
    }
    
    public object? getValue()
    {
        if (_t1 != null) return _t1;
        if (_t2 != null) return _t2;
        return null;
    }
}

public struct UnionType<T1, T2, T3>
{
    private readonly T1? _t1;
    private readonly T2? _t2;
    private readonly T3? _t3;
    
    public UnionType(T1 t1)
    {
        _t1 = t1;
    }
    public UnionType(T2 t2)
    {
        _t2 = t2;
    }
    public UnionType(T3 t3)
    {
        _t3 = t3;
    }
    
    public object? getValue()
    {
        if (_t1 != null) return _t1;
        if (_t2 != null) return _t2;
        if (_t3 != null) return _t3;
        return null;
    }
}

public struct UnionType<T1, T2, T3, T4>
{
    private readonly T1? _t1;
    private readonly T2? _t2;
    private readonly T3? _t3;
    private readonly T4? _t4;
    
    public UnionType(T1 t1)
    {
        _t1 = t1;
    }
    public UnionType(T2 t2)
    {
        _t2 = t2;
    }
    public UnionType(T3 t3)
    {
        _t3 = t3;
    }
    public UnionType(T4 t4)
    {
        _t4 = t4;
    }
    
    public object? getValue()
    {
        if (_t1 != null) return _t1;
        if (_t2 != null) return _t2;
        if (_t3 != null) return _t3;
        if (_t4 != null) return _t4;
        return null;
    }
}