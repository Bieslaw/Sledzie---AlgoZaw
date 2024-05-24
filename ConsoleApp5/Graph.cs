using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sledzie
{
    public class Graph
    {
        public Graph() { vertices = new List<Vertex>(); }
        public List<Vertex> vertices { get; set; }

        public Vertex? vertexOnCycle { get; set; }

        public void removeVertex(Vertex vertex)
        {
            vertices.Remove(vertex);
            foreach (var v in vertex.neighbors)
            { 
                v.neighbors.Remove(vertex);
                if (v.children.Contains(vertex))
                { 
                    v.children.Remove(vertex);
                }
            }
            vertex.neighbors.Clear();
            vertex.children.Clear();
            
        }
        public Vertex GetRoot()
        {
            var verticesWithParents = new HashSet<Vertex>();
            foreach (var v in vertices)
            {
                foreach (var child in v.children)
                { 
                    verticesWithParents.Add(child);
                }
            }
            foreach (var v in vertices)
            {
                if (!verticesWithParents.Contains(v))
                    return v;
            }
            throw new Exception("Root not found!");
        }
    }
}
