using SetsLibrary;
using Xunit;
using System;
using SetsLibrary.Utility;
using SetsLibrary.Extensions;
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
        Assert.Throws<MissingBraceException>(() => new TypedSet<int>(expression, config));
    }
    [Fact]
    public void BuildSetTree_SimpleSet_ParsesCorrectly()
    {
        // Arrange
        var config = new SetsConfigurations(",", addBraces: true);
        string input = "1,2,3";

        // Act
        var result = new TypedSet<int>(input, config);

        // Assert
        Assert.Equal(3, result.Cardinality);
        Assert.Contains(1, result.EnumerateRootElements());
        Assert.Contains(2, result.EnumerateRootElements());
        Assert.Contains(3, result.EnumerateRootElements());
    }
    [Fact]
    public void BuildSetTree_WithBraces_RemovesBracesCorrectly()
    {
        var config = new SetsConfigurations(",", addBraces: false);
        string input = "{1,2,3}";
        var result = SetTreeBuilder<int>.BuildSetTree(input, config);

        Assert.Equal(3, result.CountRootElements);
    }
    //[Fact]
    //public void BuildSetTree_WithBraces_CreatesSubset()
    //{
    //    var config = new SetsConfigurations(",", addBraces: true);
    //    string input = "{2,1,2,3,1}";
    //    var set = new TypedSet<int>(input, config);

    //    var result = set.


    //    Assert.Equal(0, result.CountRootElements);
    //    Assert.Equal(1, result.CountSubsets);
    //    Assert.Equal(1, result.Count);
    //    Assert.Empty(result.RootElements);
    //    Assert.Empty(result.GetRootElementsEnumerator());
    //    Assert.NotEmpty(result.GetSubsetsEnumerator());
    //    Assert.Equal("{1,2,3}", result.GetSubsetsEnumerator().First().ToString());
    //}
}//class
