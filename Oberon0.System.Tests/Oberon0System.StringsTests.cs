using System.Globalization;

namespace Oberon0.System.Tests;

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
        string result = Oberon0System.Oberon0System.ConvertToString(value);

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
        string result = Oberon0System.Oberon0System.ConvertToString(value);

        // Assert
        Assert.Equal(value.ToString(CultureInfo.InvariantCulture), result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void CanCallConvertToStringWithBoolean(bool value)
    {
        // Act
        string result = Oberon0System.Oberon0System.ConvertToString(value);

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
        string result = Oberon0System.Oberon0System.ConvertToString(value, format);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(false, "false", "true", "false")]
    [InlineData(true, "true", "true", "false")]
    [InlineData(false, "0", "1", "0")]
    [InlineData(true, "1", "1", "0")]
    public static void TestToStringBooleanFormat(bool value, string expected, string trueVal, string falseVal)
    {
        string result = Oberon0System.Oberon0System.ConvertToString(value, trueVal, falseVal);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", 0, true)]
    [InlineData("  0 ", 0, true)]
    [InlineData(" 1.2 ", 1.2, true)]
    [InlineData(" 1 E2 ", 0, false)]
    [InlineData(" 1,2 ", 0, false)]
    public static void TestParseStringReal(string value, double expected, bool expectedReturn)
    {
        double result = 0;
        bool returnValue = Oberon0System.Oberon0System.ParseString(value, ref result);
        Assert.Equal(expectedReturn, returnValue);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", 0, true)]
    [InlineData("  0 ", 0, true)]
    [InlineData(" 12 ", 12, true)]
    [InlineData(" 1 E2 ", 0, false)]
    [InlineData(" 1,2 ", 0, false)]
    public static void TestParseStringInt(string value, int expected, bool expectedReturn)
    {
        int result = 0;
        bool returnValue = Oberon0System.Oberon0System.ParseString(value, ref result);
        Assert.Equal(expectedReturn, returnValue);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", false, false)]
    [InlineData("  true ", true, true)]
    [InlineData(" TRUE ", true, true)]
    [InlineData(" E2 ", false, false)]
    [InlineData(" 1,2 ", false, false)]
    [InlineData("False", false, true)]
    public static void TestParseStringBool(string value, bool expected, bool expectedReturn)
    {
        bool result = false;
        bool returnValue = Oberon0System.Oberon0System.ParseString(value, ref result);
        Assert.Equal(expectedReturn, returnValue);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hello World", "Hello", " Hi", " Hi World")]
    [InlineData("Hello World", "Hello ", "", "World")]
    [InlineData("Hello World Test", " World", " Unit", "Hello Unit Test")]
    [InlineData("Hello World", "NotFound", "Replaced", "Hello World")]
    public static void TestReplaceString(string value, string source, string target, string expected)
    {
        string result = Oberon0System.Oberon0System.ReplaceString(value, source, target);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("This is a Test", 1, 2, "Th")]
    [InlineData("This is a Test", 6, 2, "is")]
    [InlineData("This is a Test", 1, 14, "This is a Test")]
    public static void TestSubString(string value, int start, int len, string expected)
    {
        string result = Oberon0System.Oberon0System.SubString(value, start, len);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("This is a Test", 0, 2)]
    [InlineData("This is a Test", 6, 14)]
    [InlineData("This is a Test", 1, 20)]
    public static void TestSubStringIndexOobException(string value, int start, int len)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Oberon0System.Oberon0System.SubString(value, start, len));
    }

}