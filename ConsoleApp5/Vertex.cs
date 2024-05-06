using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sledzie
{
    public class Vertex
    {
        public Vertex(int index, int shares)
        {
            this.index = index;
            this.shares = shares;
            neighbors = new List<Vertex>();
            children = new List<Vertex>();
        }
        public int index { get; set; }
        public int shares { get; set; }
        public List<Vertex> neighbors { get; set; }

        public List<Vertex> children { get; set; }
    }
}
