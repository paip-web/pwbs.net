﻿namespace PWBS.NetFrameworkUtils;

public class AssertionException: Exception
{
    public AssertionException(string message): base(message)
    {
    }
}