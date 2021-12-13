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

    public IEnumerable<string> Traverse(string start, string end)
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

      var queue = new Queue<Node>();
      queue.Enqueue(startNode);

      return Traverse(
        startNode,
        endNode,
        queue
      )
      .Distinct();

    }

    private IEnumerable<string> Traverse(
      Node current,
      Node destination,
      Queue<Node> route
    )
    {
      var returnList = new List<string>();

      // right now current is the last entry on the stack
      if (current.Equals(destination))
      {
        returnList.Add(string.Join(",", route.ToList()));
      }
      else
      {
        // get the list of nodes connected to this one that we can visit
        var nextRound = _edges
          .Where(e => e.Connects(current))
          .SelectMany(e => e.ConnectedNodes.Where(n => n.Equals(current) == false))
          .Where(n => n.CanVisit(route));

        

        foreach (var node in nextRound)
        {
          var newQueue = new Queue<Node>(route.ToList());
          newQueue.Enqueue(node);
          returnList = returnList.Concat(Traverse(node, destination, newQueue)).ToList();
        }
      }

      return returnList;
    }
  }
}
