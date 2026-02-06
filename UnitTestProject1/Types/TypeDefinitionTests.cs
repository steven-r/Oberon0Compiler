#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Tests.Types;

/// <summary>
///     Tests for TypeDefinition base class properties
/// </summary>
public class TypeDefinitionTests
{
    #region IsSimpleType Tests

    [Fact]
    public void IsSimpleType_ForIntegerType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);

        // Act & Assert
        Assert.True(type.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForRealType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Real, "REAL", false);

        // Act & Assert
        Assert.True(type.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForBooleanType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Bool, "BOOLEAN", false);

        // Act & Assert
        Assert.True(type.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForStringType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.String, "STRING", false);

        // Act & Assert
        Assert.True(type.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForVoidType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Void, "VOID", false);

        // Act & Assert
        Assert.True(type.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForArrayType_ReturnsFalse()
    {
        // Arrange
        var intType = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);
        var arrayType = new ArrayTypeDefinition(10, intType);

        // Act & Assert
        Assert.False(arrayType.IsSimpleType);
    }

    [Fact]
    public void IsSimpleType_ForRecordType_ReturnsFalse()
    {
        // Arrange
        var recordType = new RecordTypeDefinition { Name = "TestRecord" };

        // Act & Assert
        Assert.False(recordType.IsSimpleType);
    }

    #endregion

    #region IsComplexType Tests

    [Fact]
    public void IsComplexType_ForArrayType_ReturnsTrue()
    {
        // Arrange
        var intType = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);
        var arrayType = new ArrayTypeDefinition(10, intType);

        // Act & Assert
        Assert.True(arrayType.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForRecordType_ReturnsTrue()
    {
        // Arrange
        var recordType = new RecordTypeDefinition { Name = "TestRecord" };

        // Act & Assert
        Assert.True(recordType.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForIntegerType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);

        // Act & Assert
        Assert.False(type.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForRealType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Real, "REAL", false);

        // Act & Assert
        Assert.False(type.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForBooleanType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Bool, "BOOLEAN", false);

        // Act & Assert
        Assert.False(type.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForStringType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.String, "STRING", false);

        // Act & Assert
        Assert.False(type.IsComplexType);
    }

    [Fact]
    public void IsComplexType_ForVoidType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Void, "VOID", false);

        // Act & Assert
        Assert.False(type.IsComplexType);
    }

    #endregion

    #region IsAnyType Tests

    [Fact]
    public void IsAnyType_ForAnyType_ReturnsTrue()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Any, "ANY", true);

        // Act & Assert
        Assert.True(type.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForIntegerType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);

        // Act & Assert
        Assert.False(type.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForRealType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Real, "REAL", false);

        // Act & Assert
        Assert.False(type.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForBooleanType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.Bool, "BOOLEAN", false);

        // Act & Assert
        Assert.False(type.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForStringType_ReturnsFalse()
    {
        // Arrange
        var type = new SimpleTypeDefinition(BaseTypes.String, "STRING", false);

        // Act & Assert
        Assert.False(type.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForArrayType_ReturnsFalse()
    {
        // Arrange
        var intType = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", false);
        var arrayType = new ArrayTypeDefinition(10, intType);

        // Act & Assert
        Assert.False(arrayType.IsAnyType);
    }

    [Fact]
    public void IsAnyType_ForRecordType_ReturnsFalse()
    {
        // Arrange
        var recordType = new RecordTypeDefinition { Name = "TestRecord" };

        // Act & Assert
        Assert.False(recordType.IsAnyType);
    }

    #endregion
}
