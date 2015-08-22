using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGameLibrary.Pathfinding.AStar
{
    public static class AStarPathfinder
    {
        
        public static Path findPath(Node startNode, Node destination)
        {
            List<Node> closedList = new List<Node>();
            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            
            Dictionary<Node, int> gScore = new Dictionary<Node,int>();
            gScore[startNode] = 0;

            Dictionary<Node, int> fScore = new Dictionary<Node,int>();
            fScore[startNode] = startNode.HeuristicDistance(destination);

            Dictionary<Node, Node> prevNode = new Dictionary<Node, Node>();

            openList.Add(fScore[startNode], startNode);

            while (openList.Count > 0)
            {
                Node current = openList.RemoveMin();
                if (current.Equals(destination))
                {
                    return getPath(prevNode, destination);
                }
                else
                {
                    closedList.Add(current);
                    Node[] neighbours = current.GetNeighbours();
                    foreach (Node next in neighbours)
                    {
                        if (closedList.Contains(next))
                            continue;

                        int newGScore = gScore[current] + current.distanceBetween(next);

                        if (!openList.Contains(next) || newGScore < gScore[next])
                        {
                            prevNode[next] = current;
                            gScore[next] = newGScore;
                            fScore[next] = gScore[next] + next.HeuristicDistance(destination);
                            if (!openList.Contains(next))
                                openList.Add(fScore[next], next);
                        }
                    }
                }
            }

            return null;
        }

        private static Path getPath(Dictionary<Node, Node> moves, Node end)
        {
            Path path = new Path();

            path.AddNode(end);
            while (moves.ContainsKey(end))
            {
                end = moves[end];
                path.AddNode(end);
            }

            path.Reverse();

            return path;
        }
    }
}
