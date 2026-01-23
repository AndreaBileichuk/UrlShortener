using UrlShortener.BLL.Services.UrlShortener;

namespace UrlShortener.Tests;

public class AlgorithmTests
{
    [Theory]
    [InlineData(1, "1")]           
    [InlineData(10, "a")]          
    [InlineData(61, "Z")]          
    [InlineData(62, "10")]         
    [InlineData(3844, "100")]      
    public void GenerateShortCode_ShouldReturnCorrectString(int id, string expected)
    {
        // Act
        var result = UrlShortenetAlgorithm.GenerateShortCode(id);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateShortCode_ShouldHandleZero()
    {
        // Act
        var result = UrlShortenetAlgorithm.GenerateShortCode(0);

        // Assert
        Assert.Equal("0", result);
    }
}