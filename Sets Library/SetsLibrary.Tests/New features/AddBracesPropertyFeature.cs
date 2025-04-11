using SetsLibrary;
using Xunit;
using System;
namespace SetsLibrary.Tests.New_features;

public class AddBracesPropertyFeature
{
    [Fact]
    public void AddBracesWithEmptyString_ShouldReturn_EmptySet()
    {
        //Arrange
        string expression = "";
        var config = new SetsConfigurations(",",addBraces: true);

        //Act
        var set = new TypedSet<int>(expression, config);

        //Assert
        Assert.Equal(set.BuildStringRepresentation(), "{}");
    }

    [Fact]
    public void AddBracesWithEmptyString_ShouldNotThrowException()
    {
        //Assert
        string expression = "";
        var config = new SetsConfigurations(",", addBraces: true);

        //Arrange 
        var ex = Record.Exception(() => new TypedSet<int>(expression, config));
        
            
        Assert.Null(ex);
    }

    [Fact]
    public void EmptyStringSet_ShouldThrowException()
    {
        //Assert
        string expression = "";
        var config = new SetsConfigurations(",", addBraces: false);

        //This should pass
        Assert.Throws<ArgumentException>(() => new TypedSet<int>(expression, config));
    }
}//class
