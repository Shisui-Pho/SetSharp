using Xunit;
using SetSharp;
namespace SetSharp.Tests.SetParser;
public class TokenizerTests
{
    private static SetsConfigurations DefaultConfig()
    {
        return new SetsConfigurations(",", "|");
    }
    [Fact]
    public void Tokenize_SingleElement_ReturnsElement()
    {
        var tokenizer = new Tokenizer("A", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();
        Assert.Equal(TokenType.ELEMENT, tokens[0].TokenType);
        Assert.Equal("A", tokens[0].Value);
        Assert.Equal(TokenType.END_OF_LINE, tokens[1].TokenType);
    }


    [Fact]
    public void Tokenize_MultipleElements_ReturnsSeparators()
    {
        var tokenizer = new Tokenizer("A|B|C", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Collection(tokens,
            t => Assert.Equal(TokenType.ELEMENT, t.TokenType),
            t => Assert.Equal(TokenType.ROW_TERMINATOR, t.TokenType),
            t => Assert.Equal(TokenType.ELEMENT, t.TokenType),
            t => Assert.Equal(TokenType.ROW_TERMINATOR, t.TokenType),
            t => Assert.Equal(TokenType.ELEMENT, t.TokenType),
            t => Assert.Equal(TokenType.END_OF_LINE, t.TokenType)
        );
    }


    [Fact]
    public void Tokenize_SetWithBraces_ReturnsBraceTokens()
    {
        var tokenizer = new Tokenizer("{A}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Collection(tokens,
            t => Assert.Equal(TokenType.OPENING_BRACE, t.TokenType),
            t => Assert.Equal(TokenType.ELEMENT, t.TokenType),
            t => Assert.Equal(TokenType.CLOSING_BRACE, t.TokenType),
            t => Assert.Equal(TokenType.END_OF_LINE, t.TokenType)
        );
    }


    [Fact]
    public void Tokenize_NestedSet_ReturnsCorrectStructure()
    {
        var tokenizer = new Tokenizer("{A|{B|C}}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(
            new[]
            {
                TokenType.OPENING_BRACE,
                TokenType.ELEMENT,
                TokenType.ROW_TERMINATOR,
                TokenType.OPENING_BRACE,
                TokenType.ELEMENT,
                TokenType.ROW_TERMINATOR,
                TokenType.ELEMENT,
                TokenType.CLOSING_BRACE,
                TokenType.CLOSING_BRACE,
                TokenType.END_OF_LINE
            },
            tokens.Select(x => x.TokenType)
        );
    }


    [Fact]
    public void Tokenize_EmptyExpression_ReturnsOnlyEnd()
    {
        var tokenizer = new Tokenizer("", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Single(tokens);
        Assert.Equal(TokenType.END_OF_LINE, tokens[0].TokenType);
    }


    [Fact]
    public void Tokenize_WhitespaceElement_PreservesElement()
    {
        var tokenizer = new Tokenizer(" Hello ", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.ELEMENT, tokens[0].TokenType);
        Assert.Equal("Hello", tokens[0].Value);
    }


    [Fact]
    public void Tokenize_EscapedOpeningBrace_IsElement()
    {
        var tokenizer = new Tokenizer(@"\{Hello\}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.ELEMENT, tokens[0].TokenType);
        Assert.Equal("{Hello}", tokens[0].Value);
    }


    [Fact]
    public void Tokenize_EscapedSlash_ReturnsSlash()
    {
        var tokenizer = new Tokenizer(@"Hello\\World", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(@"Hello\World", tokens[0].Value);
    }


    [Fact]
    public void Tokenize_EscapedRowTerminator_DoesNotSplit()
    {
        var tokenizer = new Tokenizer(@"Hello\|World|Next", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("Hello|World", tokens[0].Value);
        Assert.Equal(TokenType.ROW_TERMINATOR, tokens[1].TokenType);
        Assert.Equal("Next", tokens[2].Value);
    }


    [Fact]
    public void Tokenize_CustomMultiCharacterRowTerminator()
    {
        var config = new SetsConfigurations(",", "::");

        var tokenizer = new Tokenizer("A::B::C", config);

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(6, tokens.Length);
    }


    [Fact]
    public void Tokenize_LastElementWithoutSeparator()
    {
        var tokenizer = new Tokenizer("A|B|C", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("C", tokens[4].Value);
    }


    [Fact]
    public void Tokenize_BracesAroundMultipleElements()
    {
        var tokenizer = new Tokenizer("{A|B|C}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.OPENING_BRACE, tokens[0].TokenType);
        Assert.Equal(TokenType.CLOSING_BRACE, tokens[^2].TokenType);
    }


    [Fact]
    public void Tokenize_DeeplyNestedSets()
    {
        var tokenizer = new Tokenizer("{{{A}}}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(3,
            tokens.Count(x => x.TokenType == TokenType.OPENING_BRACE));

        Assert.Equal(3,
            tokens.Count(x => x.TokenType == TokenType.CLOSING_BRACE));
    }


    [Fact]
    public void Tokenize_MixedNestedAndElements()
    {
        var tokenizer = new Tokenizer("{A|{B}|C}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Contains(tokens,
            x => x.Value == "A");

        Assert.Contains(tokens,
            x => x.Value == "B");

        Assert.Contains(tokens,
            x => x.Value == "C");
    }


    [Fact]
    public void Tokenize_OnlyBraces()
    {
        var tokenizer = new Tokenizer("{}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.OPENING_BRACE, tokens[0].TokenType);
        Assert.Equal(TokenType.CLOSING_BRACE, tokens[1].TokenType);
    }


    [Fact]
    public void Tokenize_NumberElements()
    {
        var tokenizer = new Tokenizer("1|2|3", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("1", tokens[0].Value);
        Assert.Equal("2", tokens[2].Value);
        Assert.Equal("3", tokens[4].Value);
    }


    [Fact]
    public void Tokenize_SpecialCharactersInsideElement()
    {
        var tokenizer = new Tokenizer("Hello World!", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("Hello World!", tokens[0].Value);
    }


    [Fact]
    public void Tokenize_EmptyElements()
    {
        var tokenizer = new Tokenizer("A||B", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.ROW_TERMINATOR,
            tokens[1].TokenType);

        Assert.Equal(TokenType.ROW_TERMINATOR,
            tokens[2].TokenType);
    }


    [Fact]
    public void Tokenize_EscapeAtEndThrows()
    {
        var tokenizer = new Tokenizer(@"\", DefaultConfig());

        Assert.Throws<SetsException>(() =>
            tokenizer.Tokenize());
    }


    [Fact]
    public void Tokenize_ElementBeforeNestedSetFlushes()
    {
        var tokenizer = new Tokenizer("A{B}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("A", tokens[0].Value);
        Assert.Equal(TokenType.OPENING_BRACE, tokens[1].TokenType);
    }


    [Fact]
    public void Tokenize_RowTerminatorInsideNestedSet()
    {
        var tokenizer = new Tokenizer("{A|{B|C}|D}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(4,
            tokens.Count(x => x.TokenType == TokenType.ELEMENT));
    }


    [Fact]
    public void Tokenize_EndTokenAlwaysExists()
    {
        var tokenizer = new Tokenizer("{A}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(
            TokenType.END_OF_LINE,
            tokens.Last().TokenType);
    }
    [Fact]
    public void Tokenize_MultipleEscapedStructuralCharacters_RemainElement()
    {
        var tokenizer = new Tokenizer(@"\{\}\\", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.ELEMENT, tokens[0].TokenType);
        Assert.Equal(@"{}\", tokens[0].Value);
        Assert.Equal(TokenType.END_OF_LINE, tokens[1].TokenType);
    }
    [Fact]
    public void Tokenize_RowTerminatorAtStart_ReturnsSeparator()
    {
        var tokenizer = new Tokenizer("|A", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(TokenType.ROW_TERMINATOR, tokens[0].TokenType);
        Assert.Equal(TokenType.ELEMENT, tokens[1].TokenType);
        Assert.Equal("A", tokens[1].Value);
    }
    [Fact]
    public void Tokenize_NestedEmptySets_ReturnsOnlyBraces()
    {
        var tokenizer = new Tokenizer("{{}}", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal(4,
            tokens.Count(x => x.TokenType == TokenType.OPENING_BRACE ||
                              x.TokenType == TokenType.CLOSING_BRACE));

        Assert.DoesNotContain(tokens,
            x => x.TokenType == TokenType.ELEMENT);
    }
    [Fact]
    public void Tokenize_EscapedRowTerminatorFollowedByRealSeparator_WorksCorrectly()
    {
        var tokenizer = new Tokenizer(@"A\|B|C", DefaultConfig());

        var tokens = tokenizer.Tokenize().ToArray();

        Assert.Equal("A|B", tokens[0].Value);

        Assert.Equal(TokenType.ROW_TERMINATOR,
            tokens[1].TokenType);

        Assert.Equal("C", tokens[2].Value);
    }
}
