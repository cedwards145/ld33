using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGameLibrary.Pathfinding
{
    public interface Node
    {
        Node[] GetNeighbours();
        int HeuristicDistance(Node other);
        int distanceBetween(Node other);
    }
}
