using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace SevenSegmentDismay.Core.Tests
{
  public class DisplayTestCycleTests
  {
    [Fact]
    public void DisplayTestCycle_GivenNullResults_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new DisplayTestCycle(null)
      );
    }

    [Theory]
    [InlineData("")]
    [InlineData("sdfa asdf afsd eaws awe gawre asdf awer sDf aew | a a a a")]
    [InlineData("a a a a a a a a a a | a a a")]
    [InlineData("a a a a a a a a a | a a a a")]
    [InlineData("a a a a a a a a a a a | a a a a")]
    [InlineData("a a a a a a a a a a | a a a a a")]
    [InlineData("a a a a a a a a a q | a a a a")]
    [InlineData("a a a a a a a a a a | q a a a")]
    [InlineData("a a a a a a a a a a")]
    [InlineData("| a a a a")]
    [InlineData("1 2 3 4 5 6 7 8 9 0 | 1 2 3 4")]
    public void DisplayTestCycle_GivenInvalidResultString_ThrowsArgumentNullException(string invalidResults)
    {
      Assert.Throws<ArgumentException>(
        () => _ = new DisplayTestCycle(invalidResults)
      );
    }

    [Theory]
    [InlineData("be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe")]
    [InlineData("edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc")]
    [InlineData("fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg")]
    [InlineData("fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb")]
    [InlineData("aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea")]
    [InlineData("fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb")]
    [InlineData("dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe")]
    [InlineData("bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef")]
    [InlineData("egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb")]
    [InlineData("gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce")]
    public void DisplayTestCycle_GivenValidResultString_DoesNotThrowException(string results)
    {
      var exception = Record.Exception(
        () => _ = new DisplayTestCycle(results)
      );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(SampleDisplayTestResults))]
    public void DisplayTestCycle_GivenValidResultString_SetsPropertiesAsExpected(
      string results,
      IList<Display> expectedTestResults,
      IList<Display> expectedDisplayReading)
    {
      var sut = new DisplayTestCycle(results);

      Assert.Equal(expectedTestResults.ToList(), sut.TestResults.ToList());
      Assert.Equal(expectedDisplayReading.ToList(), sut.DisplayReading.ToList());
    }

    public static IEnumerable<object[]> SampleDisplayTestResults
    {
      get
      {
        yield return new object[] {
          "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
          new Display[] {
            new Display("be"),
            new Display("cfbegad"),
            new Display("cbdgef"),
            new Display("fgaecd"),
            new Display("cgeb"),
            new Display("fdcge"),
            new Display("agebfd"),
            new Display("fecdb"),
            new Display("fabcd"),
            new Display("edb"),
          },
          new Display[] {
            new Display("fdgacbe"),
            new Display("cefdb"),
            new Display("cefbgd"),
            new Display("gcbe")
          }
        };
        /*
        yield return new object[] {
          "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
          new Display[] {
            new Display("edbfga"),
            new Display("begcd"),
            new Display("cbg"),
            new Display("gc"),
            new Display("gcadebf"),
            new Display("fbgde"),
            new Display("acbgfd"),
            new Display("abcde"),
            new Display("gfcbed"),
            new Display("gfec"),
          },
          new Display[] {
            new Display("fcgedb"),
            new Display("cgb"),
            new Display("dgebacf"),
            new Display("gc")
          }
        };

        yield return new object[] {
          "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
          new Display[] {
            new Display("fgaebd"),
            new Display("cg"),
            new Display("bdaec"),
            new Display("gdafb"),
            new Display("agbcfd"),
            new Display("gdcbef"),
            new Display("bgcad"),
            new Display("gfac"),
            new Display("gcb"),
            new Display("cdgabef"),
          },
          new Display[] {
            new Display("cg"),
            new Display("cg"),
            new Display("fdcagb"),
            new Display("cbg")
          }
        };

        yield return new object[] {
          "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
          new Display[] {
            new Display("fbegcd"),
            new Display("cbd"),
            new Display("adcefb"),
            new Display("dageb"),
            new Display("afcb"),
            new Display("bc"),
            new Display("aefdc"),
            new Display("ecdab"),
            new Display("fgdeca"),
            new Display("fcdbega"),
          },
          new Display[] {
            new Display("efabcd"),
            new Display("cedba"),
            new Display("gadfec"),
            new Display("cb")
          }
        };

        yield return new object[] {
          "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
          new Display[] {
            new Display("aecbfdg"),
            new Display("fbg"),
            new Display("gf"),
            new Display("bafeg"),
            new Display("dbefa"),
            new Display("fcge"),
            new Display("gcbea"),
            new Display("fcaegb"),
            new Display("dgceab"),
            new Display("fcbdga"),
          },
          new Display[] {
            new Display("gecf"),
            new Display("egdcabf"),
            new Display("bgf"),
            new Display("bfgea")
          }
        };

        yield return new object[] {
          "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
          new Display[] {
            new Display("fgeab"),
            new Display("ca"),
            new Display("afcebg"),
            new Display("bdacfeg"),
            new Display("cfaedg"),
            new Display("gcfdb"),
            new Display("baec"),
            new Display("bfadeg"),
            new Display("bafgc"),
            new Display("acf"),
          },
          new Display[] {
            new Display("gebdcfa"),
            new Display("ecba"),
            new Display("ca"),
            new Display("fadegcb")
          }
        };

        yield return new object[] {
          "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
          new Display[] {
            new Display("dbcfg"),
            new Display("fgd"),
            new Display("bdegcaf"),
            new Display("fgec"),
            new Display("aegbdf"),
            new Display("ecdfab"),
            new Display("fbedc"),
            new Display("dacgb"),
            new Display("gdcebf"),
            new Display("gf"),
          },
          new Display[] {
            new Display("cefg"),
            new Display("dcbef"),
            new Display("fcge"),
            new Display("gbcadfe")
          }
        };

        yield return new object[] {
          "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
          new Display[] {
            new Display("bdfegc"),
            new Display("cbegaf"),
            new Display("gecbf"),
            new Display("dfcage"),
            new Display("bdacg"),
            new Display("ed"),
            new Display("bedf"),
            new Display("ced"),
            new Display("adcbefg"),
            new Display("gebcd"),
          },
          new Display[] {
            new Display("ed"),
            new Display("bcgafe"),
            new Display("cdgba"),
            new Display("cbgef")
          }
        };

        yield return new object[] {
          "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
          new Display[] {
            new Display("egadfb"),
            new Display("cdbfeg"),
            new Display("cegd"),
            new Display("fecab"),
            new Display("cgb"),
            new Display("gbdefca"),
            new Display("cg"),
            new Display("fgcdab"),
            new Display("egfdb"),
            new Display("bfceg"),
          },
          new Display[] {
            new Display("gbdfcae"),
            new Display("bgc"),
            new Display("cg"),
            new Display("cgb")
          }
        };

        yield return new object[] {
          "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
          new Display[] {
            new Display("gcafb"),
            new Display("gcf"),
            new Display("dcaebfg"),
            new Display("ecagb"),
            new Display("gf"),
            new Display("abcdeg"),
            new Display("gaef"),
            new Display("cafbge"),
            new Display("fdbac"),
            new Display("fegbdc"),
          },
          new Display[] {
            new Display("fgae"),
            new Display("cfgab"),
            new Display("fg"),
            new Display("bagce")
          }
        };*/
      }
    }
  }
}
