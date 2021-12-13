using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Spelunker.Core
{
  public class Graph
  {
    private List<Node> _nodes;
    private List<Edge> _edges;

    public Graph(IEnumerable<string> relationships)
    {
      if (relationships == null || relationships.Any(r => r == null))
      {
        throw new ArgumentNullException(nameof(relationships), "Relationships may not be null");
      }

      if (relationships.Any(r => Regex.IsMatch(r, @"[^-]+-[^-]+") == false))
      {
        throw new ArgumentException("Relationships must be of the form {A}-{B} where A and B are node names.");
      }

      _nodes = new List<Node>();
      _edges = new List<Edge>();

      foreach (var r in relationships)
      {
        var nodesToAdd = r.Split("-");
        var left = new Node(nodesToAdd[0]);
        var right = new Node(nodesToAdd[1]);
        
        if (_nodes.Contains(left) == false)
        {
          _nodes.Add(left);
        }

        if (_nodes.Contains(right) == false)
        {
          _nodes.Add(right);
        }

        if (_edges.Any(e => e.Connects(left, right)) == false)
        {
          _edges.Add(new Edge(left, right));
        }
      }
    }

    public IEnumerable<string> Traverse(
      string start,
      string end,
      Func<Node, IEnumerable<Node>, bool> visitRule
    )
    {
      var startNode = new Node(start);
      if (_nodes.Contains(startNode) == false)
      {
        throw new ArgumentException($"Graph does not contain {start}", nameof(start));
      }

      var endNode = new Node(end);
      if (_nodes.Contains(endNode) == false)
      {
        throw new ArgumentException($"Graph does not contain {end}", nameof(end));
      }

      var route = new Stack<Node>();

      return Traverse(
        startNode,
        endNode,
        route,
        visitRule
      )
      .Distinct();

    }

    private IEnumerable<string> Traverse(
      Node current,
      Node destination,
      Stack<Node> route,
      Func<Node, IEnumerable<Node>, bool> visitRule
    )
    {
      var returnList = new List<string>();

      route.Push(current);


      // right now current is the last entry on the stack
      if (current.Equals(destination))
      {
        returnList.Add(string.Join(",", route.Reverse()));
      }
      else
      {
        // get the list of nodes connected to this one that we can visit
        var nextRound = _edges
          .Where(e => e.Connects(current))
          .SelectMany(e => e.ConnectedNodes.Where(n => n.Equals(current) == false))
          .Where(n => visitRule(n, route));

        foreach (var node in nextRound)
        {
          returnList = returnList.Concat(
            Traverse(node, destination, route, visitRule)
          ).ToList();
        }
      }

      _ = route.Pop();

      return returnList;
    }
  }
}
