using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;

namespace Spelunker.Core
{
  public class Node : IEquatable<Node>, IComparable<Node>, IComparable
  {
    public string Name
    {
      get;
      private set;
    }

    public static readonly Func<Node, IEnumerable<Node>, bool> CanVisitRule = new Func<Node, IEnumerable<Node>, bool>(
        (current, route) => current.CanVisit(route)
      );

    public static readonly Func<Node, IEnumerable<Node>, bool> CanRevisitRule = new Func<Node, IEnumerable<Node>, bool>(
        (current, route) => current.CanRevisit(route)
      );

    public Node(string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name), "Node name may not be null!");
      }

      Name = name;
    }

    public override string ToString()
    {
      return Name;
    }
    
    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is Node))
      {
        return false;
      }
      else
      {
        return this.Equals((Node)obj);
      }
    }

    public bool Equals([AllowNull] Node other)
    {
      if (other == null)
      {
        return false;
      }

      return this.ToString().Equals(other.ToString(), StringComparison.InvariantCulture);
    }

    public int CompareTo([AllowNull] Node other)
    {
      if (other == null)
      {
        return this.ToString().CompareTo((string)null);
      }

      return this.ToString().CompareTo(other.ToString());
    }

    public int CompareTo(object obj)
    {
      if (!(obj is Node))
      {
        return this.ToString().CompareTo((string)null);
      }

      return this.CompareTo((Node)obj);
    }

    public override int GetHashCode()
    {
      return this.ToString().GetHashCode();
    }

    public bool CanVisit(IEnumerable<Node> visitedNodes)
    {
      if (this.Name.Equals(this.Name.ToLowerInvariant()))
      {
        return visitedNodes.Contains(this) == false;
      }

      return true;
    }

    public bool CanRevisit(IEnumerable<Node> visitedNodes)
    {
      return
        (
          this.CanVisit(visitedNodes)
          || (
          // to get here means we've visited this node once before
          // so now all we need to do is ensure that no lower case node has been visited twice
            false == visitedNodes
              .Where(n => n.Name.Equals(n.Name.ToLowerInvariant()))
              .Select(n => n.Name)
              .GroupBy(n => n)
              .Select(g => g.Count())
              .Where(n => n > 1)
              .Any()
            && this.Name.Equals("start") == false
            && this.Name.Equals("end") == false
          )
        );
    }
  }
}
