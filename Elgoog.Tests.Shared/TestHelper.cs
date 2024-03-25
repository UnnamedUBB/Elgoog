using System.Reflection;

namespace Elgoog.Tests.Shared;

public static class TestHelper
{
    public static object? InvokePrivateMethod(Type type, string methodName, params object?[]? parameters)
    {
        var instance = Activator.CreateInstance(type);
        var method =  type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        return method.Invoke(instance, parameters);
    }
}