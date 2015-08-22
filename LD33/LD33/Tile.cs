using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameLibrary.Pathfinding;
using XNAGameLibrary.Pathfinding.AStar;

namespace LD33
{
    public class Tile : Node
    {
        public bool passable;
        public List<Tile> neighbours;
        public Point position;

        public Tile(Point reqPosition, bool passability)
        {
            position = reqPosition;
            passable = passability;
            neighbours = new List<Tile>();
        }

        public Node[] GetNeighbours()
        {
            return neighbours.ToArray();
        }

        public int HeuristicDistance(Node other)
        {
            if (other is Tile)
            {
                Tile otherTile = (Tile)other;

                return (int)(Math.Sqrt(Math.Pow(position.X - otherTile.position.X, 2) +
                                       Math.Pow(position.Y - otherTile.position.Y, 2)));
            }
            else
                return 0;
        }

        public int distanceBetween(Node other)
        {
            if (other is Tile)
                return 1;
            else
                return 0;
        }

        public override string ToString()
        {
            return position.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Tile)
                return ((Tile)obj).position.Equals(position);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
