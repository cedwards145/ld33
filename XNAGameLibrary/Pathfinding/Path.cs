using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGameLibrary.Pathfinding
{
    public class Path
    {
        public List<Node> Nodes { get; protected set; }
        public int Count { get { return Nodes.Count; } }

        public Path()
        {
            Nodes = new List<Node>();
        }

        public Path(List<Node> reqNodes)
        {
            Nodes = new List<Node>(reqNodes);
        }

        public void AddNode(Node newNode)
        {
            Nodes.Add(newNode);
        }

        public void Reverse()
        {
            Nodes.Reverse();
        }

        public Node Dequeue()
        {
            if (Nodes.Count > 0)
            {
                Node node = Nodes[0];
                Nodes.RemoveAt(0);
                return node;
            }
            return null;
        }

        public override string ToString()
        {
            string temp = "";

            foreach (Node node in Nodes)
            {
                temp += node.ToString() + " => ";
            }

            return temp;
        }

        public Path Clone()
        {
            Path clone = new Path(Nodes);

            return clone;
        }
    }
}
