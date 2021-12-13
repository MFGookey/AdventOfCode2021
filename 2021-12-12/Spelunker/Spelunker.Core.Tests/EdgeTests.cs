using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace Spelunker.Core.Tests
{
  public class EdgeTests
  {
    [Fact]
    public void Edge_GivenNullNode_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new Edge(null)
      );
    }

    [Theory]
    [MemberData(nameof(NodesWithNulls))]
    public void Edge_GivenNullNodes_ThrowsArgumentNullException(Node left, Node right)
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new Edge(left, right)
      );
    }

    [Fact]
    public void Edge_GivenNode_DoesNotThrowException()
    {
      var node = new Node("test");
      var exception = Record.Exception(() => _ = new Edge(node));
      Assert.Null(exception);
    }

    [Fact]
    public void Edge_GivenNodes_DoesNotThrowException()
    {
      var left = new Node("test");
      var right = new Node("test2");
      var exception = Record.Exception(() => _ = new Edge(left, right));
      Assert.Null(exception);
    }

    [Fact]
    public void Connects_GivenNode_ShowsConnection()
    {
      var node = new Node("test");
      var sut = new Edge(node);
      Assert.True(sut.Connects(node));
      Assert.Equal(new List<Node> { node }, sut.ConnectedNodes);
    }

    [Fact]
    public void Connects_GivenNodes_ShowsConnection()
    {
      var left = new Node("test");
      var right = new Node("test2");
      var sut = new Edge(left, right);
      
      Assert.True(sut.Connects(left));
      Assert.True(sut.Connects(right));

      Assert.True(sut.Connects(left, right));
      Assert.True(sut.Connects(right, left));

      Assert.Contains(left, sut.ConnectedNodes);
      Assert.Contains(right, sut.ConnectedNodes);
      Assert.Equal(2, sut.ConnectedNodes.Count());
    }

    public static IEnumerable<object[]> NodesWithNulls
    {
      get
      {
        yield return new object[]
        {
          new Node("left"),
          null
        };

        yield return new object[]
        {
          null,
          new Node("right")
        };

        yield return new object[]
        {
          null,
          null
        };
      }
    }
  }
}
