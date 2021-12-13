﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Spelunker.Core.Tests
{
  public class NodeTests
  {
    [Fact]
    public void Node_GivenNullName_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => _ = new Node(null));
    }

    [Fact]
    public void Node_GivenName_DoesNotThrowException()
    {
      var exception = Record.Exception(
        () => _ = new Node(string.Empty)
      );

      Assert.Null(exception);
    }

    [Fact]
    public void Node_GivenName_SetsPropertiesAsExpected()
    {
      var name = "qwerty";
      var sut = new Node(name);
      Assert.Equal(name, sut.Name);
    }

    [Fact]
    public void ToString_ReturnsNodeName()
    {
      var name = "qwerty";
      var sut = new Node(name);
      Assert.Equal(name, sut.ToString());
    }

    [Theory]
    [MemberData(nameof(NodeEqualityData))]
    public void Equals_GivenObject_ReturnsExpectedEquality(
      string name,
      object toCompare,
      bool expectedEquality
    )
    {
      var sut = new Node(name);
      Assert.Equal(expectedEquality, sut.Equals(toCompare));
    }

    [Theory]
    [MemberData(nameof(NodeEqualityData))]
    public void Equals_GivenNode_ReturnsExpectedEquality(
      string name,
      Node toCompare,
      bool expectedEquality
    )
    {
      var sut = new Node(name);
      Assert.Equal(expectedEquality, sut.Equals(toCompare));
    }

    public static IEnumerable<object[]> NodeEqualityData
    {
      get
      {
        yield return new object[]
        {
          "name",
          null,
          false
        };

        yield return new object[]
        {
          "name",
          new Node("othername"),
          false
        };

        yield return new object[]
        {
          "name",
          new Node("name"),
          true
        };

        yield return new object[]
        {
          "name",
          new Node("NAME"),
          false
        };
      }
    }
  }
}
