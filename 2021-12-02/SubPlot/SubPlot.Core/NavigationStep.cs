using System;
using System.Collections.Generic;
using System.Text;

namespace SubPlot.Core
{
  public class NavigationStep
  {
    public int Magnitude
    {
      get; private set;
    }

    public NavigationDirections Direction
    {
      get; private set;
    }

    public NavigationStep(string command)
    {
      var subCommands = command.Split(" ");
      NavigationDirections parsedDirection;
      int parsedMagnitude;
      if (
        subCommands.Length != 2 // We specifically want Direction and magnitude only
        || int.TryParse(subCommands[0], out _) // Do not accept an integer for the direction
        || Enum.TryParse(subCommands[0], true, out parsedDirection) == false // The direction must be in the enum
        || int.TryParse(subCommands[1], out parsedMagnitude) == false // The magnitude must be an integer
      )
      {
        throw new FormatException($"Command \"{command}\" is not validly formatted.");
      }

      Magnitude = parsedMagnitude;
      Direction = parsedDirection;
    }

    public int ChangeInHorizontal()
    {
      switch (Direction)
      {
        case NavigationDirections.Forward:
            return Magnitude;
        default:
          return 0;
      }
    }

    public int ChangeInDepth()
    {
      switch (Direction)
      {
        case NavigationDirections.Up:
          return -Magnitude;
        case NavigationDirections.Down:
          return Magnitude;
        default:
          return 0;
      }
    }
  }
}
