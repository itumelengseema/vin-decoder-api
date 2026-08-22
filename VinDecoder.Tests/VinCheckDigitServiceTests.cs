using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinCheckDigitServiceTests
{
    private readonly VinCheckDigitService _service;

    public VinCheckDigitServiceTests()
    {
        _service = new VinCheckDigitService();
    }

    #region Check Digit Validation Tests
    [Fact]
    public void IsValid_WithValidCheckDigit_ShouldReturnTrue()
    {
        // Arrange
        string validVin = "MALAF51CYNM186853";

        // Act
        bool result = _service.IsValid(validVin);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithInvalidCheckDigit_ShouldReturnFalse()
    {
        // Arrange
        string invalidVin = "MALAF51ZYNM186853"; // Changed valid check digit 'C' to invalid 'Z'

        // Act
        bool result = _service.IsValid(invalidVin);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("1GBWK542Z6Z9ABCDE")] // Different valid VINs
    [InlineData("JTEBTV326D5046374")]
    public void IsValid_WithVariousValidVins_ShouldReturnTrue(string validVin)
    {
        // Act
        bool result = _service.IsValid(validVin);

        // Assert
        Assert.True(result);
    }
    #endregion

    #region Check Digit Calculation Tests
    [Fact]
    public void CalculateCheckDigit_ShouldReturnValidDigit()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.True(char.IsLetterOrDigit(result) || result == 'X');
    }

    [Fact]
    public void CalculateCheckDigit_WithRemainder10_ShouldReturnX()
    {
        // Arrange - VIN that produces remainder 10 (which becomes 'X')
        string vin = "3G5DA0ZZZZZZZZZZZ";

        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.Equal('X', result);
    }

    [Theory]
    [InlineData("MALAF51CYNM186853", 'C')]
    public void CalculateCheckDigit_WithKnownVin_ShouldReturnExpectedCheckDigit(string vin, char expectedCheckDigit)
    {
        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.Equal(expectedCheckDigit, result);
    }

    [Fact]
    public void CalculateCheckDigit_ShouldReturnDigitOrX()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.True(
            char.IsDigit(result) || result == 'X',
            "Check digit must be a digit (0-9) or 'X'"
        );
    }
    #endregion

    #region Check Digit Validation Requirements Tests
    [Theory]
    [InlineData("1MALAF51CYNM18685", true)]  // Starts with 1 - requires validation
    [InlineData("2MALAF51CYNM18685", true)]  // Starts with 2 - requires validation
    [InlineData("3MALAF51CYNM18685", true)]  // Starts with 3 - requires validation
    [InlineData("4MALAF51CYNM18685", true)]  // Starts with 4 - requires validation
    [InlineData("5MALAF51CYNM18685", true)]  // Starts with 5 - requires validation
    [InlineData("6MALAF51CYNM18685", false)] // Starts with 6 - doesn't require validation
    [InlineData("7MALAF51CYNM18685", false)] // Starts with 7 - doesn't require validation
    [InlineData("8MALAF51CYNM18685", false)] // Starts with 8 - doesn't require validation
    [InlineData("9MALAF51CYNM18685", false)] // Starts with 9 - doesn't require validation
    [InlineData("JMALAF51CYNM18685", false)] // Starts with J - doesn't require validation
    [InlineData("WMALAF51CYNM18685", false)] // Starts with W - doesn't require validation
    public void RequiresCheckDigitValidation_WithVariousFirstCharacters_ShouldReturnCorrectly(string vin, bool expected)
    {
        // Act
        bool result = _service.RequiresCheckDigitValidation(vin);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RequiresCheckDigitValidation_WithDigit1_ShouldReturnTrue()
    {
        // Arrange
        string vin = "1MALAF51CYNM18685";

        // Act
        bool result = _service.RequiresCheckDigitValidation(vin);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void RequiresCheckDigitValidation_WithLetterG_ShouldReturnFalse()
    {
        // Arrange
        string vin = "GMALAF51CYNM18685";

        // Act
        bool result = _service.RequiresCheckDigitValidation(vin);

        // Assert
        Assert.False(result);
    }
    #endregion

    #region Consistency Tests
    [Fact]
    public void CalculateCheckDigit_WithSameVin_ShouldReturnConsistentResults()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        char result1 = _service.CalculateCheckDigit(vin);
        char result2 = _service.CalculateCheckDigit(vin);
        char result3 = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
    }

    [Fact]
    public void IsValid_WithSameVin_ShouldReturnConsistentResults()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        bool result1 = _service.IsValid(vin);
        bool result2 = _service.IsValid(vin);
        bool result3 = _service.IsValid(vin);

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
        Assert.True(result1);
    }
    #endregion

    #region Character Mapping Tests
    [Fact]
    public void CalculateCheckDigit_WithLettersAndNumbers_ShouldHandleCorrectly()
    {
        // Arrange VIN with mix of letters and numbers
        string vin = "1HGCV41JXMN109186";

        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.True(char.IsDigit(result) || result == 'X');
    }

    [Theory]
    [InlineData("A1MALAF51CYNM1868")] // A is first letter (rare)
    [InlineData("Z1MALAF51CYNM1868")] // Z is first letter (rare)
    public void CalculateCheckDigit_WithVariousFirstCharacters_ShouldCalculateCheckDigit(string vin)
    {
        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert
        Assert.True(char.IsDigit(result) || result == 'X');
    }
    #endregion

    #region Edge Cases
    [Fact]
    public void IsValid_CheckDigitAtPosition8_ShouldBeValidated()
    {
        // Arrange
        // Check digit is at position 8 (0-indexed)
        string vin = "MALAF51CYNM186853";

        // Act
        char checkDigitFromVin = vin[8];
        bool isValid = _service.IsValid(vin);

        // Assert
        Assert.Equal('C', checkDigitFromVin);
        Assert.True(isValid);
    }

    [Fact]
    public void CalculateCheckDigit_ShouldUseCorrectWeights()
    {
        // Arrange
        // Weights: 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2
        string vin = "MALAF51CYNM186853";

        // Act
        char result = _service.CalculateCheckDigit(vin);

        // Assert - Verify it returns the correct check digit
        Assert.Equal('C', result);
    }
    #endregion
}