using System.Globalization;

namespace Oberon0.System.Tests;

using Oberon0System;
using Xunit;

public static partial class Oberon0SystemTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(5274097)]
    [InlineData(-771204301)]
    public static void CanCallConvertToStringWithInt(int value)
    {
        // Act
        string result = Oberon0System.ConvertToString(value);

        // Assert
        Assert.Equal(value.ToString(), result);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.MinValue)]
    [InlineData(5274097.9283)]
    [InlineData(-771204301.211)]
    public static void CanCallConvertToStringWithReal(double value)
    {
        // Act
        string result = Oberon0System.ConvertToString(value);

        // Assert
        Assert.Equal(value.ToString(CultureInfo.InvariantCulture), result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void CanCallConvertToStringWithBoolean(bool value)
    {
        // Act
        string result = Oberon0System.ConvertToString(value);

        // Assert
        Assert.Equal(value.ToString(), result);
    }


    [Theory]
    [InlineData(0.0, "0", "G")]
    [InlineData(1.0, "1", "G")]
    [InlineData(10000000000000.00001, "10000000000000", "G")]
    [InlineData(0.1, "0.1", "G")]
    [InlineData(2.3E-06, "2.3E-06", "G")]
    [InlineData(2.3E06, "2300000", "G")]
    [InlineData(double.MinValue, "-1.7976931348623157E+308", "G")]
    public static void TestToStringRealFormat(double value, string expected, string format)
    {
        string result = Oberon0System.ConvertToString(value, format);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(false, "false", "true", "false")]
    [InlineData(true, "true", "true", "false")]
    [InlineData(false, "0", "1", "0")]
    [InlineData(true, "1", "1", "0")]
    public static void TestToStringBooleanFormat(bool value, string expected, string trueVal, string falseVal)
    {
        string result = Oberon0System.ConvertToString(value, trueVal, falseVal);
        Assert.Equal(expected, result);
    }
}