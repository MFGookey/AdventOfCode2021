using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Twinklepus.Core.Tests
{
  public class OctoGridTests
  {
    [Fact]
    public void OctoGrid_GivenNullEnergyLevels_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new OctoGrid(null)
      );
    }

    [Fact]
    public void OctoGrid_GivenEnergyLevelsContainingNull_ThrowsArgumentNullException()
    {
      var levels = new List<string>
      {
        "123",
        null,
        "456"
      };

      Assert.Throws<ArgumentNullException>(
        () => _ = new OctoGrid(levels)
      );
    }

    [Fact]
    public void OctoGrid_GivenNonRectangularEnergyLevels_ThrowsArgumentException()
    {
      var levels = new List<string>
      {
        "123",
        "7890",
        "456"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new OctoGrid(levels)
      );
    }

    [Fact]
    public void OctoGrid_GivenNonNumericEnergyLevels_ThrowsArgumentException()
    {
      var levels = new List<string>
      {
        "123",
        "789",
        "4s6"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new OctoGrid(levels)
      );
    }

    [Fact]
    public void OctoGrid_GivenValidEnergyLevels_DoesNotThrowExeception()
    {
      var levels = new List<string>
      {
        "123",
        "456",
        "789"
      };

      var exception = Record.Exception(
        () => _ = new OctoGrid(levels)
      );

      Assert.Null(exception);
    }

    [Fact]
    public void OctoGrid_GivenValidEnergyLevels_SetsLevelsAsExpected()
    {
      var levels = new List<string>
      {
        "123",
        "456",
        "789"
      };

      var sut = new OctoGrid(levels);
      Assert.Equal(0, sut.FlashCount);
      Assert.Equal(string.Join("\n", levels), sut.ToString());
    }

    [Theory]
    [MemberData(nameof(SingleStepGrids))]
    public void OctoGrid_Tick_UpdatesAsExpected(
      IEnumerable<string> startingGrid,
      IReadOnlyList<IEnumerable<string>> expectedStates,
      IReadOnlyList<Int64?> expectedFlashCounts
    )
    {
      var sut = new OctoGrid(startingGrid);
      Assert.Equal(string.Join("\n", startingGrid), sut.ToString());

      for(var i = 0; i < expectedStates.Count; i++)
      {
        sut.Tick();
        Assert.Equal(string.Join("\n", expectedStates[i]), sut.ToString());
        if (expectedFlashCounts[i].HasValue)
        {
          Assert.Equal(expectedFlashCounts[i].Value, sut.FlashCount);
        }
      }
    }

    [Theory]
    [MemberData(nameof(MultiStepGrids))]
    public void TickUntil_UpdatesAsExpected(
      IEnumerable<string> startingGrid,
      IReadOnlyDictionary<int, string[]> expectedStatesAtTick,
      IReadOnlyDictionary<int, Int64> expectedFlashCounts
    )
    {
      var sut = new OctoGrid(startingGrid);
      Assert.Equal(string.Join("\n", startingGrid), sut.ToString());
      Assert.Equal(0, sut.FlashCount);

      foreach (var kvp in expectedStatesAtTick)
      {
        sut.TickUntil(kvp.Key);
        Assert.Equal(string.Join("\n", kvp.Value), sut.ToString());
        if (expectedFlashCounts.ContainsKey(kvp.Key))
        {
          Assert.Equal(expectedFlashCounts[kvp.Key], sut.FlashCount);
        }
      }
    }

    [Theory]
    [MemberData(nameof(SynchronizedFlashGrids))]
    public void TickUntilSynchronized_StopsAtCorrectTickCount(
      IEnumerable<string> startingGrid,
      int expectedFirstSynchronizedFlash
    )
    {
      var sut = new OctoGrid(startingGrid);
      Assert.Equal(string.Join("\n", startingGrid), sut.ToString());
      Assert.Equal(0, sut.FlashCount);
      Assert.Null(sut.FirstSynchronizedFlash);

      sut.TickUntilSynchronized();
      Assert.NotNull(sut.FirstSynchronizedFlash);
      Assert.Equal(
        expectedFirstSynchronizedFlash,
        sut.FirstSynchronizedFlash.Value
      );
    }

    public static IEnumerable<object[]> SingleStepGrids
    {
      get
      {
        yield return new object[]
        {
          new string[]
          {
            "11111",
            "19991",
            "19191",
            "19991",
            "11111"
          },
          new List<string[]>
          {
            new string[]
            {
              "34543",
              "40004",
              "50005",
              "40004",
              "34543"
            },
            new string[]
            {
              "45654",
              "51115",
              "61116",
              "51115",
              "45654"
            }
          },
          new Int64?[]
          {
            9,
            null
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526"
          },
          new List<string[]>
          {
            new string[]
            {
              "6594254334",
              "3856965822",
              "6375667284",
              "7252447257",
              "7468496589",
              "5278635756",
              "3287952832",
              "7993992245",
              "5957959665",
              "6394862637"
            },

            new string[]
            {
              "8807476555",
              "5089087054",
              "8597889608",
              "8485769600",
              "8700908800",
              "6600088989",
              "6800005943",
              "0000007456",
              "9000000876",
              "8700006848"
            },

            new string[]
            {
              "0050900866",
              "8500800575",
              "9900000039",
              "9700000041",
              "9935080063",
              "7712300000",
              "7911250009",
              "2211130000",
              "0421125000",
              "0021119000"
            },

            new string[]
            {
              "2263031977",
              "0923031697",
              "0032221150",
              "0041111163",
              "0076191174",
              "0053411122",
              "0042361120",
              "5532241122",
              "1532247211",
              "1132230211"
            },

            new string[]
            {
              "4484144000",
              "2044144000",
              "2253333493",
              "1152333274",
              "1187303285",
              "1164633233",
              "1153472231",
              "6643352233",
              "2643358322",
              "2243341322"
            },

            new string[]
            {
              "5595255111",
              "3155255222",
              "3364444605",
              "2263444496",
              "2298414396",
              "2275744344",
              "2264583342",
              "7754463344",
              "3754469433",
              "3354452433"
            },

            new string[]
            {
              "6707366222",
              "4377366333",
              "4475555827",
              "3496655709",
              "3500625609",
              "3509955566",
              "3486694453",
              "8865585555",
              "4865580644",
              "4465574644"
            },

            new string[]
            {
              "7818477333",
              "5488477444",
              "5697666949",
              "4608766830",
              "4734946730",
              "4740097688",
              "6900007564",
              "0000009666",
              "8000004755",
              "6800007755"
            },

            new string[]
            {
              "9060000644",
              "7800000976",
              "6900000080",
              "5840000082",
              "5858000093",
              "6962400000",
              "8021250009",
              "2221130009",
              "9111128097",
              "7911119976"
            },

            new string[]
            {
              "0481112976",
              "0031112009",
              "0041112504",
              "0081111406",
              "0099111306",
              "0093511233",
              "0442361130",
              "5532252350",
              "0532250600",
              "0032240000"
            }
          },
          new Int64?[]
          {
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            204
          }
        };
      }
    }



    public static IEnumerable<object[]> MultiStepGrids
    {
      get
      {
        yield return new object[]
        {
          new string[]
          {
            "11111",
            "19991",
            "19191",
            "19991",
            "11111"
          },
          new Dictionary<int, string[]>
          {
            {
              1,
              new string[]
              {
                "34543",
                "40004",
                "50005",
                "40004",
                "34543"
              }
            },
            {
              2,
              new string[]
              {
                "45654",
                "51115",
                "61116",
                "51115",
                "45654"
              }
            }
          },
          new Dictionary<int, Int64>
          {
            { 1, 9 }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526"
          },
          new Dictionary<int, string[]>
          {
            {
              1,
              new string[]
              {
                "6594254334",
                "3856965822",
                "6375667284",
                "7252447257",
                "7468496589",
                "5278635756",
                "3287952832",
                "7993992245",
                "5957959665",
                "6394862637"
              }
            },
            {
              2,
              new string[]
              {
                "8807476555",
                "5089087054",
                "8597889608",
                "8485769600",
                "8700908800",
                "6600088989",
                "6800005943",
                "0000007456",
                "9000000876",
                "8700006848"
              }
            },
            {
              3,
              new string[]
              {
                "0050900866",
                "8500800575",
                "9900000039",
                "9700000041",
                "9935080063",
                "7712300000",
                "7911250009",
                "2211130000",
                "0421125000",
                "0021119000"
              }
            },
            {
              4,
              new string[]
              {
                "2263031977",
                "0923031697",
                "0032221150",
                "0041111163",
                "0076191174",
                "0053411122",
                "0042361120",
                "5532241122",
                "1532247211",
                "1132230211"
              }
            },
            {
              5,
              new string[]
              {
                "4484144000",
                "2044144000",
                "2253333493",
                "1152333274",
                "1187303285",
                "1164633233",
                "1153472231",
                "6643352233",
                "2643358322",
                "2243341322"
              }
            },
            {
              6,
              new string[]
              {
                "5595255111",
                "3155255222",
                "3364444605",
                "2263444496",
                "2298414396",
                "2275744344",
                "2264583342",
                "7754463344",
                "3754469433",
                "3354452433"
              }
            },
            {
              7,
              new string[]
              {
                "6707366222",
                "4377366333",
                "4475555827",
                "3496655709",
                "3500625609",
                "3509955566",
                "3486694453",
                "8865585555",
                "4865580644",
                "4465574644"
              }
            },
            {
              8,
              new string[]
              {
                "7818477333",
                "5488477444",
                "5697666949",
                "4608766830",
                "4734946730",
                "4740097688",
                "6900007564",
                "0000009666",
                "8000004755",
                "6800007755"
              }
            },
            {
              9,
              new string[]
              {
                "9060000644",
                "7800000976",
                "6900000080",
                "5840000082",
                "5858000093",
                "6962400000",
                "8021250009",
                "2221130009",
                "9111128097",
                "7911119976"
              }
            },
            {
              10,
              new string[]
              {
                "0481112976",
                "0031112009",
                "0041112504",
                "0081111406",
                "0099111306",
                "0093511233",
                "0442361130",
                "5532252350",
                "0532250600",
                "0032240000"
              }
            },
            {
              20,
              new string[]
              {
                "3936556452",
                "5686556806",
                "4496555690",
                "4448655580",
                "4456865570",
                "5680086577",
                "7000009896",
                "0000000344",
                "6000000364",
                "4600009543"
              }
            },
            {
              30,
              new string[]
              {
                "0643334118",
                "4253334611",
                "3374333458",
                "2225333337",
                "2229333338",
                "2276733333",
                "2754574565",
                "5544458511",
                "9444447111",
                "7944446119"
              }
            },
            {
              40,
              new string[]
              {
                "6211111981",
                "0421111119",
                "0042111115",
                "0003111115",
                "0003111116",
                "0065611111",
                "0532351111",
                "3322234597",
                "2222222976",
                "2222222762"
              }
            },
            {
              50,
              new string[]
              {
                "9655556447",
                "4865556805",
                "4486555690",
                "4458655580",
                "4574865570",
                "5700086566",
                "6000009887",
                "8000000533",
                "6800000633",
                "5680000538"
              }
            },
            {
              60,
              new string[]
              {
                "2533334200",
                "2743334640",
                "2264333458",
                "2225333337",
                "2225333338",
                "2287833333",
                "3854573455",
                "1854458611",
                "1175447111",
                "1115446111"
              }
            },
            {
              70,
              new string[]
              {
                "8211111164",
                "0421111166",
                "0042111114",
                "0004211115",
                "0000211116",
                "0065611111",
                "0532351111",
                "7322235117",
                "5722223475",
                "4572222754"
              }
            },
            {
              80,
              new string[]
              {
                "1755555697",
                "5965555609",
                "4486555680",
                "4458655580",
                "4570865570",
                "5700086566",
                "7000008666",
                "0000000990",
                "0000000800",
                "0000000000"
              }
            },
            {
              90,
              new string[]
              {
                "7433333522",
                "2643333522",
                "2264333458",
                "2226433337",
                "2222433338",
                "2287833333",
                "2854573333",
                "4854458333",
                "3387779333",
                "3333333333"
              }
            },
            {
              100,
              new string[]
              {
                "0397666866",
                "0749766918",
                "0053976933",
                "0004297822",
                "0004229892",
                "0053222877",
                "0532222966",
                "9322228966",
                "7922286866",
                "6789998766"
              }
            },
            {
              193,
              new string[]
              {
                "5877777777",
                "8877777777",
                "7777777777",
                "7777777777",
                "7777777777",
                "7777777777",
                "7777777777",
                "7777777777",
                "7777777777",
                "7777777777"
              }
            },
            {
              194,
              new string[]
              {
                "6988888888",
                "9988888888",
                "8888888888",
                "8888888888",
                "8888888888",
                "8888888888",
                "8888888888",
                "8888888888",
                "8888888888",
                "8888888888"
              }
            },
            {
              195,
              new string[]
              {
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000",
                "0000000000"
              }
            }
          },
          new Dictionary<int, Int64>
          {
            { 10, 204 },
            { 100, 1656 }
          }
        };
      }
    }

    public static IEnumerable<object[]> SynchronizedFlashGrids
    {
      get
      {
        yield return new object[]
        {
          new string[]
          {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526"
          },
          195
        };
      }
    }
  }
}
