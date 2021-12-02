using System;
using sut = Common.Utilities.Formatter;
using Common.Utilities.IO;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace Common.Utilities.Tests.Formatter
{
  public class RecordFormatterTests
  {
    [Fact]
    public void RecordFormatter_Constructor_SetsFileReader()
    {
      var reader = new Mock<IFileReader>();
      var expected = new string[] { "Record 1", "Record 2" };

      reader.Setup(r => r.ReadFile(It.IsAny<string>())).Returns("Record 1\nRecord 2");

      sut.IRecordFormatter sut = new sut.RecordFormatter(reader.Object);

      var actual = sut.FormatFile(string.Empty, "\n", false);

      Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecordFormatter_GivenNullFileReader_ThrowsNullReferenceExceptionOnFileRead()
    {
      sut.IRecordFormatter sut = new sut.RecordFormatter(null);
      Assert.Throws<NullReferenceException>(
        () => sut.FormatFile(string.Empty, string.Empty, false)
      );

      Assert.Throws<NullReferenceException>(
        () => sut.FormatFile(string.Empty, string.Empty, false, false)
      );
    }

    [Theory]
    [MemberData(nameof(SampleRecordsUnNormalizedLineEndings))]
    public void FormatRecord_GivenRecordsAndDefaultLineEndingNormalization_ReturnsExpectedRecords(
      string records,
      string recordDelimiter,
      bool removeBlankRecords,
      string[] expectedRecords
    )
    {
      sut.IRecordFormatter sut = new sut.RecordFormatter(null);

      var result = sut.FormatRecord(records, recordDelimiter, removeBlankRecords);

      Assert.Equal(expectedRecords, result);
    }

    [Theory]
    [MemberData(nameof(SampleSubRecordsUnNormalizedLineEndings))]
    public void FormatSubRecords_GivenRecordsAndDefaultLineEndingNormalization_ReturnsExpectedRecords(
      string[] records,
      string subRecordDelimiter,
      bool removeBlankRecords,
      IEnumerable<IEnumerable<string>> expectedRecords
    )
    {
      sut.IRecordFormatter sut = new sut.RecordFormatter(null);

      var result = sut.FormatSubRecords(records, subRecordDelimiter, removeBlankRecords);

      Assert.Equal(expectedRecords, result);
    }

    [Theory]
    [MemberData(nameof(SampleRecordsUnNormalizedLineEndings))]
    public void FormatFile_GivenFilesAndDefaultLineEndingNormalization_ReturnsExpectedRecords(
      string fileContents,
      string recordDelimiter,
      bool removeBlankRecords,
      IEnumerable<string> expectedRecords
      )
    {
      var reader = new Mock<IFileReader>();
      reader
        .Setup(r => r.ReadFile(It.IsAny<string>()))
        .Returns(fileContents);
      sut.IRecordFormatter sut = new sut.RecordFormatter(reader.Object);

      var result = sut.FormatFile(string.Empty, recordDelimiter, removeBlankRecords);

      Assert.Equal(expectedRecords, result);
    }

    [Theory]
    [MemberData(nameof(SampleRecords))]
    public void FormatRecord_GivenRecords_ReturnsExpectedRecords(
      string records,
      string recordDelimiter,
      bool removeBlankRecords,
      bool normalizeLineEndings,
      string[] expectedRecords
    )
    {
      sut.IRecordFormatter sut = new sut.RecordFormatter(null);

      var result = sut.FormatRecord(records, recordDelimiter, removeBlankRecords, normalizeLineEndings);

      Assert.Equal(expectedRecords, result);
    }

    [Theory]
    [MemberData(nameof(SampleSubRecords))]
    public void FormatSubRecords_GivenRecords_ReturnsExpectedRecords(
      string[] records,
      string subRecordDelimiter,
      bool removeBlankRecords,
      bool normalizeLineEndings,
      IEnumerable<IEnumerable<string>> expectedRecords
    )
    {
      sut.IRecordFormatter sut = new sut.RecordFormatter(null);

      var result = sut.FormatSubRecords(records, subRecordDelimiter, removeBlankRecords, normalizeLineEndings);

      Assert.Equal(expectedRecords, result);
    }

    [Theory]
    [MemberData(nameof(SampleRecords))]
    public void FormatFile_GivenFiles_ReturnsExpectedRecords(
      string fileContents,
      string recordDelimiter,
      bool removeBlankRecords,
      bool normalizeLineEndings,
      IEnumerable<string> expectedRecords
      )
    {
      var reader = new Mock<IFileReader>();
      reader
        .Setup(r => r.ReadFile(It.IsAny<string>()))
        .Returns(fileContents);

      sut.IRecordFormatter sut = new sut.RecordFormatter(reader.Object);

      var result = sut.FormatFile(string.Empty, recordDelimiter, removeBlankRecords, normalizeLineEndings);

      Assert.Equal(expectedRecords, result);
    }

    public static IEnumerable<object[]> SampleRecordsUnNormalizedLineEndings
    {
      get
      {
        yield return new object[]
        {
          string.Empty,
          "\n",
          true,
          new string[] {}
        };

        yield return new object[]
        {
          string.Empty,
          "\n",
          false,
          new string[] { string.Empty }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2",
          "\n",
          true,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n\n\n",
          "\n",
          true,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n",
          "\n",
          false,
          new string[] {
            "Record 1",
            "Record 2",
            string.Empty
          }
        };

        yield return new object[]
        {
          @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in",
          "\r\n\r\n",
          true,
          new string[]
          {
            @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm",
            @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929",
            @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm",
            @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in"
          }
        };
      }
    }

    public static IEnumerable<object[]> SampleSubRecordsUnNormalizedLineEndings
    {
      get
      {
        yield return new object[] {
          new string[] {},
          "\n",
          true,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty,},
          "\n",
          true,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty},
          "\n",
          false,
          new string[][] {
            new string[] { string.Empty },
            new string[] { string.Empty }
          }
        };

        yield return new object[] {
          new string[] {},
          "\n",
          false,
          new string[][] {}
        };

        yield return new object[] {
          new string[]
          {
            @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm",
            @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929",
            @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm",
            @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in"
          },
          " ",
          true,
          new string[][]
          {
            new string[]
            {
              "ecl:gry",
              "pid:860033327",
              "eyr:2020",
              "hcl:#fffffd\r\nbyr:1937",
              "iyr:2017",
              "cid:147",
              "hgt:183cm"
            },
            new string[]
            {
              "iyr:2013",
              "ecl:amb",
              "cid:350",
              "eyr:2023",
              "pid:028048884\r\nhcl:#cfa07d",
              "byr:1929"
            },
            new string[]
            {
              "hcl:#ae17e1",
              "iyr:2013\r\neyr:2024\r\necl:brn",
              "pid:760753108",
              "byr:1931\r\nhgt:179cm"
            },
            new string[]
            {
              "hcl:#cfa07d",
              "eyr:2025",
              "pid:166559648\r\niyr:2011",
              "ecl:brn",
              "hgt:59in"
            }
          }
        };
      }
    }

    public static IEnumerable<object[]> SampleRecords
    {
      get
      {
        yield return new object[]
        {
          string.Empty,
          "\n",
          true,
          true,
          new string[] {}
        };

        yield return new object[]
        {
          string.Empty,
          "\n",
          false,
          true,
          new string[] { string.Empty }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2",
          "\n",
          true,
          true,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n\n\n",
          "\n",
          true,
          true,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n",
          "\n",
          false,
          true,
          new string[] {
            "Record 1",
            "Record 2",
            string.Empty
          }
        };

        yield return new object[]
        {
          @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in",
          "\n\n",
          true,
          true,
          new string[]
          {
            "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd\nbyr:1937 iyr:2017 cid:147 hgt:183cm",
            "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884\nhcl:#cfa07d byr:1929",
            "hcl:#ae17e1 iyr:2013\neyr:2024\necl:brn pid:760753108 byr:1931\nhgt:179cm",
            "hcl:#cfa07d eyr:2025 pid:166559648\niyr:2011 ecl:brn hgt:59in"
          }
        };

        yield return new object[]
        {
          string.Empty,
          "\n",
          true,
          false,
          new string[] {}
        };

        yield return new object[]
        {
          string.Empty,
          "\n",
          false,
          false,
          new string[] { string.Empty }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2",
          "\n",
          true,
          false,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n\n\n",
          "\n",
          true,
          false,
          new string[] {
            "Record 1",
            "Record 2"
          }
        };

        yield return new object[]
        {
          "Record 1\nRecord 2\n",
          "\n",
          false,
          false,
          new string[] {
            "Record 1",
            "Record 2",
            string.Empty
          }
        };

        yield return new object[]
        {
          @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in",
          "\r\n\r\n",
          true,
          false,
          new string[]
          {
            @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm",
            @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929",
            @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm",
            @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in"
          }
        };
      }
    }

    public static IEnumerable<object[]> SampleSubRecords
    {
      get
      {
        yield return new object[] {
          new string[] {},
          "\n",
          true,
          true,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty,},
          "\n",
          true,
          true,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty},
          "\n",
          false,
          true,
          new string[][] {
            new string[] { string.Empty },
            new string[] { string.Empty }
          }
        };

        yield return new object[] {
          new string[] {},
          "\n",
          false,
          true,
          new string[][] {}
        };

        yield return new object[] {
          new string[]
          {
            @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm",
            @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929",
            @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm",
            @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in"
          },
          " ",
          true,
          true,
          new string[][]
          {
            new string[]
            {
              "ecl:gry",
              "pid:860033327",
              "eyr:2020",
              "hcl:#fffffd\nbyr:1937",
              "iyr:2017",
              "cid:147",
              "hgt:183cm"
            },
            new string[]
            {
              "iyr:2013",
              "ecl:amb",
              "cid:350",
              "eyr:2023",
              "pid:028048884\nhcl:#cfa07d",
              "byr:1929"
            },
            new string[]
            {
              "hcl:#ae17e1",
              "iyr:2013\neyr:2024\necl:brn",
              "pid:760753108",
              "byr:1931\nhgt:179cm"
            },
            new string[]
            {
              "hcl:#cfa07d",
              "eyr:2025",
              "pid:166559648\niyr:2011",
              "ecl:brn",
              "hgt:59in"
            }
          }
        };

        yield return new object[] {
          new string[] {},
          "\n",
          true,
          false,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty,},
          "\n",
          true,
          false,
          new string[][] {}
        };

        yield return new object[] {
          new string[] { string.Empty, string.Empty},
          "\n",
          false,
          false,
          new string[][] {
            new string[] { string.Empty },
            new string[] { string.Empty }
          }
        };

        yield return new object[] {
          new string[] {},
          "\n",
          false,
          false,
          new string[][] {}
        };

        yield return new object[] {
          new string[]
          {
            @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm",
            @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929",
            @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm",
            @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in"
          },
          " ",
          true,
          false,
          new string[][]
          {
            new string[]
            {
              "ecl:gry",
              "pid:860033327",
              "eyr:2020",
              "hcl:#fffffd\r\nbyr:1937",
              "iyr:2017",
              "cid:147",
              "hgt:183cm"
            },
            new string[]
            {
              "iyr:2013",
              "ecl:amb",
              "cid:350",
              "eyr:2023",
              "pid:028048884\r\nhcl:#cfa07d",
              "byr:1929"
            },
            new string[]
            {
              "hcl:#ae17e1",
              "iyr:2013\r\neyr:2024\r\necl:brn",
              "pid:760753108",
              "byr:1931\r\nhgt:179cm"
            },
            new string[]
            {
              "hcl:#cfa07d",
              "eyr:2025",
              "pid:166559648\r\niyr:2011",
              "ecl:brn",
              "hgt:59in"
            }
          }
        };
      }
    }
  }
}
