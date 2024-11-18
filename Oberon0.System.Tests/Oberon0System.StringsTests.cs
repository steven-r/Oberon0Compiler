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

}