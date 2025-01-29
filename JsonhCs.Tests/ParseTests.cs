﻿using System.Text.Json;

namespace JsonhCs.Tests;

public class ParseTests {
    [Fact]
    public void EscapeSequenceTest() {
        string Jsonh = """
            "\U0001F47D and \uD83D\uDC7D"
            """;
        string Element = new JsonhReader(Jsonh).ParseElement<string>().Value!;

        Assert.Equal("👽 and 👽", Element);
    }
    [Fact]
    public void MultiQuotedStringTest() {
        string Jsonh = """""  
                """"
                  Hello! Here's a quote: ". Now a double quote: "". And a triple quote! """. Escape: \\\U0001F47D.
                 """"
            """"";
        string Element = new JsonhReader(Jsonh).ParseElement<string>().Value!;

        Assert.Equal(" Hello! Here's a quote: \". Now a double quote: \"\". And a triple quote! \"\"\". Escape: \\👽.", Element);
    }
    [Fact]
    public void ArrayTest() {
        string Jsonh = """""  
                [
                  1, 2,
                  3
                  4 5,6
                ]
            """"";
        JsonElement Element = new JsonhReader(Jsonh).ParseElement().Value;

        Assert.Equal(5, Element.GetArrayLength());
        Assert.Equal(1, Element[0].Deserialize<int>(GlobalJsonOptions.Mini));
        Assert.Equal(2, Element[1].Deserialize<int>(GlobalJsonOptions.Mini));
        Assert.Equal(3, Element[2].Deserialize<int>(GlobalJsonOptions.Mini));
        Assert.Equal("4 5", Element[3].Deserialize<string>(GlobalJsonOptions.Mini));
        Assert.Equal(6, Element[4].Deserialize<int>(GlobalJsonOptions.Mini));
    }
}