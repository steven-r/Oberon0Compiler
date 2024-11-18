namespace Oberon0.System.Tests;

using Oberon0System;
using Xunit;

public static partial class Oberon0SystemTests
{
    [Theory]
    [InlineData(0.0, false)]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(double.PositiveInfinity, true)]
    public static void CanCallIsInfinity(double value, bool expected)
    {
        // Act
        bool result = Oberon0System.IsInfinity(value);

        // Assert
        Assert.Equal(expected, result);
    }
}