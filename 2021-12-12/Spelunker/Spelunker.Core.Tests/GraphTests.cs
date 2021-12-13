using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace Spelunker.Core.Tests
{
  public class GraphTests
  {
    [Fact]
    public void Graph_GivenNullRelationships_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new Graph(null)
      );
    }

    [Fact]
    public void Graph_GivenRelationshipsContainingNull_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new Graph(new List<string> { null })
      );
    }

    [Fact]
    public void Graph_GivenInvalidRelationship_ThrowsArgumentException()
    {
      var relationships = new List<string>
      {
        "a"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new Graph(relationships)
      );
    }

    [Theory]
    [MemberData(nameof(ValidGraphs))]
    public void Graph_GivenValidRelationships_DoesNotThrowException(IEnumerable<string> graph)
    {
      var exception = Record.Exception(() => _ = new Graph(graph));
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(ValidGraphsWithVisitRulePathCounts))]
    public void Traverse_GivenValidNodes_ReturnsExpectedVisitRuleCount(
      IEnumerable<string> graph,
      string startingNode,
      string endingNode,
      int expectedPathCount
    )
    {
      var sut = new Graph(graph);
      Assert.Equal(
        expectedPathCount,
        sut.Traverse(startingNode, endingNode, Node.CanVisitRule).Count()
      );
    }

    [Theory]
    [MemberData(nameof(ValidGraphsWithVisitRulePathResults))]
    public void Traverse_GivenValidNodes_ReturnsExpectedVisitRuleResults(
      IEnumerable<string> graph,
      string startingNode,
      string endingNode,
      IEnumerable<string> expectedPaths
    )
    {
      var sut = new Graph(graph);
      Assert.Equal(
        expectedPaths.OrderBy(s=>s),
        sut.Traverse(
          startingNode,
          endingNode,
          Node.CanVisitRule
        ).OrderBy(s=>s)
      );
    }

    [Theory]
    [MemberData(nameof(ValidGraphsWithRevisitRulePathCounts))]
    public void Traverse_GivenValidNodes_ReturnsExpectedRevisitRuleCount(
      IEnumerable<string> graph,
      string startingNode,
      string endingNode,
      int expectedPathCount
    )
    {
      var sut = new Graph(graph);
      Assert.Equal(
        expectedPathCount,
        sut.Traverse(startingNode, endingNode, Node.CanRevisitRule).Count()
      );
    }
    
    [Theory]
    [MemberData(nameof(ValidGraphsWithRevisitRulePathResults))]
    public void Traverse_GivenValidNodes_ReturnsExpectedRevisitRuleResults(
      IEnumerable<string> graph,
      string startingNode,
      string endingNode,
      IEnumerable<string> expectedPaths
    )
    {
      var sut = new Graph(graph);
      Assert.Equal(
        expectedPaths.OrderBy(s => s),
        sut.Traverse(
          startingNode,
          endingNode,
          Node.CanRevisitRule
        ).OrderBy(s => s)
      );
    }

    public static IEnumerable<object[]> ValidGraphs
    {
      get
      {
        yield return new object[]
        {
          new List<string>()
        };

        yield return new object[]
        {
          new List<string>
          {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "fs-end",
            "he-DX",
            "fs-he",
            "start-DX",
            "pj-DX",
            "end-zg",
            "zg-sl",
            "zg-pj",
            "pj-he",
            "RW-he",
            "fs-DX",
            "pj-RW",
            "zg-RW",
            "start-pj",
            "he-WI",
            "zg-he",
            "pj-fs",
            "start-RW"
          }
        };
      }
    } 

    public static IEnumerable<object[]> ValidGraphsWithVisitRulePathCounts
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end"
          },
          "start",
          "end",
          10
        };
        
        yield return new object[]
        {
          new List<string>
          {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc"
          },
          "start",
          "end",
          19
        };

        yield return new object[]
        {
          new List<string>
          {
            "fs-end",
            "he-DX",
            "fs-he",
            "start-DX",
            "pj-DX",
            "end-zg",
            "zg-sl",
            "zg-pj",
            "pj-he",
            "RW-he",
            "fs-DX",
            "pj-RW",
            "zg-RW",
            "start-pj",
            "he-WI",
            "zg-he",
            "pj-fs",
            "start-RW"
          },
          "start",
          "end",
          226
        };
      }
    }

    public static IEnumerable<object[]> ValidGraphsWithVisitRulePathResults
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end"
          },
          "start",
          "end",
          new List<string>
          {
            "start,A,b,A,c,A,end",
            "start,A,b,A,end",
            "start,A,b,end",
            "start,A,c,A,b,A,end",
            "start,A,c,A,b,end",
            "start,A,c,A,end",
            "start,A,end",
            "start,b,A,c,A,end",
            "start,b,A,end",
            "start,b,end"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc"
          },
          "start",
          "end",
          new List<string>
          {
            "start,HN,dc,HN,end",
            "start,HN,dc,HN,kj,HN,end",
            "start,HN,dc,end",
            "start,HN,dc,kj,HN,end",
            "start,HN,end",
            "start,HN,kj,HN,dc,HN,end",
            "start,HN,kj,HN,dc,end",
            "start,HN,kj,HN,end",
            "start,HN,kj,dc,HN,end",
            "start,HN,kj,dc,end",
            "start,dc,HN,end",
            "start,dc,HN,kj,HN,end",
            "start,dc,end",
            "start,dc,kj,HN,end",
            "start,kj,HN,dc,HN,end",
            "start,kj,HN,dc,end",
            "start,kj,HN,end",
            "start,kj,dc,HN,end",
            "start,kj,dc,end"
          }
        };
      }
    }

    public static IEnumerable<object[]> ValidGraphsWithRevisitRulePathCounts
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end"
          },
          "start",
          "end",
          36
        };

        yield return new object[]
        {
          new List<string>
          {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc"
          },
          "start",
          "end",
          103
        };

        yield return new object[]
        {
          new List<string>
          {
            "fs-end",
            "he-DX",
            "fs-he",
            "start-DX",
            "pj-DX",
            "end-zg",
            "zg-sl",
            "zg-pj",
            "pj-he",
            "RW-he",
            "fs-DX",
            "pj-RW",
            "zg-RW",
            "start-pj",
            "he-WI",
            "zg-he",
            "pj-fs",
            "start-RW"
          },
          "start",
          "end",
          3509
        };
      }
    }

    public static IEnumerable<object[]> ValidGraphsWithRevisitRulePathResults
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end"
          },
          "start",
          "end",
          new List<string>
          {
            "start,A,b,A,b,A,c,A,end",
            "start,A,b,A,b,A,end",
            "start,A,b,A,b,end",
            "start,A,b,A,c,A,b,A,end",
            "start,A,b,A,c,A,b,end",
            "start,A,b,A,c,A,c,A,end",
            "start,A,b,A,c,A,end",
            "start,A,b,A,end",
            "start,A,b,d,b,A,c,A,end",
            "start,A,b,d,b,A,end",
            "start,A,b,d,b,end",
            "start,A,b,end",
            "start,A,c,A,b,A,b,A,end",
            "start,A,c,A,b,A,b,end",
            "start,A,c,A,b,A,c,A,end",
            "start,A,c,A,b,A,end",
            "start,A,c,A,b,d,b,A,end",
            "start,A,c,A,b,d,b,end",
            "start,A,c,A,b,end",
            "start,A,c,A,c,A,b,A,end",
            "start,A,c,A,c,A,b,end",
            "start,A,c,A,c,A,end",
            "start,A,c,A,end",
            "start,A,end",
            "start,b,A,b,A,c,A,end",
            "start,b,A,b,A,end",
            "start,b,A,b,end",
            "start,b,A,c,A,b,A,end",
            "start,b,A,c,A,b,end",
            "start,b,A,c,A,c,A,end",
            "start,b,A,c,A,end",
            "start,b,A,end",
            "start,b,d,b,A,c,A,end",
            "start,b,d,b,A,end",
            "start,b,d,b,end",
            "start,b,end"
          }
        };
      }
    }
  }
}
