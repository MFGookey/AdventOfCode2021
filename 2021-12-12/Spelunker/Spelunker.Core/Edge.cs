using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Spelunker.Core
{
  public class Edge
  {
    public IReadOnlyList<Node> ConnectedNodes
    {
      get;
      private set;
    }

    public Edge(Node toConnect)
    {
      if (toConnect == null)
      {
        throw new ArgumentNullException(nameof(toConnect), "Node may not be null");
      }

      ConnectedNodes = new List<Node> { toConnect };
    }

    public Edge(Node left, Node right)
    {
      if (left == null)
      {
        throw new ArgumentNullException(nameof(left), "Node may not be null");
      }

      if (right == null)
      {
        throw new ArgumentNullException(nameof(right), "Node may not be null");
      }

      var connection = new List<Node> { left };

      if (left != right)
      {
        connection.Add(right);
      }

      ConnectedNodes = connection;
    }

    public bool Connects(Node toCheck)
    {
      return ConnectedNodes.Contains(toCheck);
    }

    public bool Connects(Node start, Node end)
    {
      return this.Connects(start) && this.Connects(end);
    }
  }
}
