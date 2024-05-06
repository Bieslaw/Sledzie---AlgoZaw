using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sledzie
{
    public class Algorithms
    {
        static Graph ConstructGraphFromFile(string path)
        {
            var graph = new Graph();
            const int BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var lineContent = line.Split(' ');
                    var index = int.Parse(lineContent.First());
                    var shares = int.Parse(lineContent.Last());
                    graph.vertices.Add(new Vertex(index, shares));
                }
            }
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var lineContent = line.Split(' ');
                    var vertex = graph.vertices[int.Parse(lineContent.First())];
                    if (lineContent.Length > 2) // pierwsza linijka jest inna - nie zawiera informacji o śledzonym wierzchołku
                    {
                        var followedVertex = graph.vertices[int.Parse(lineContent[1])];
                        followedVertex.children.Add(vertex);
                        followedVertex.neighbors.Add(vertex);
                        vertex.neighbors.Add(followedVertex);
                    }
                }
            }
            return graph;
        }

        public static List<Vertex> MostSharesNoFollowing(string filePath)
        {
            var G = ConstructGraphFromFile(filePath);
            var connectedComponents = DFSDisconnected(G);
            var result = new List<Vertex>();
            result.AddRange(AlgorithmForTrees(connectedComponents[0]));
            foreach(var component in connectedComponents.Skip(1)) 
            {
                var resPresent = new List<Vertex>();
                var resAbsent = new List<Vertex>();
                var v = component.vertexOnCycle;
                var vNeighbors = new List<Vertex>(v.neighbors);
                component.removeVertex(v);
                var components1 = DFSDisconnected(component);
                foreach (var c in components1)
                {
                    resAbsent.AddRange(AlgorithmForTrees(c));
                }
                vNeighbors.ForEach(x => component.removeVertex(x));
                var components2 = DFSDisconnected(component);
                foreach (var c in components2)
                {
                    resPresent.AddRange(AlgorithmForTrees(c));
                }
                resPresent.Add(v);
                if (resPresent.Sum(x => x.shares) > resAbsent.Sum(x => x.shares))
                {
                    result.AddRange(resPresent);
                }
                else 
                {
                    result.AddRange(resAbsent);
                }
            }
            return result;
        }

        static List<Vertex> AlgorithmForTrees(Graph G) 
        {
            var root = G.GetRoot();
            var levels = ConstructLevels(root);
            var present = new Dictionary<Vertex,  int>();
            var absent = new Dictionary<Vertex, int>();
            var presentChildren = new Dictionary<Vertex, List<Vertex>>();
            var absentChildren = new Dictionary<Vertex, List<Vertex>>();
            foreach ( var v in G.vertices ) 
            {
                present[v] = 0;
                absent[v] = 0;
                presentChildren[v] = new List<Vertex>();
                absentChildren[v] = new List<Vertex>();
            }
            for(int i = levels.Length - 1; i >=0; i--) 
            { 
                foreach(var v in levels[i]) 
                {
                    present[v] = v.shares;
                    foreach (var u in v.children)
                    {
                        present[v] += absent[u];
                        if (present[u] > absent[u])
                        {
                            presentChildren[v].Add(u);
                            absent[v] += present[u];
                        }
                        else
                        {
                            absentChildren[v].Add(u);
                            absent[v] += absent[u];
                        }
                    }
                    
                }
            }
            var result = new List<Vertex>();
            CreateResult(root, present[root] > absent[root], result, presentChildren, absentChildren);
            return result;
        }
        static void CreateResult(Vertex v, bool isPresent, List<Vertex> result, 
            Dictionary<Vertex, List<Vertex>> presentChildren, Dictionary<Vertex, List<Vertex>> absentChildren)
        {
            if (isPresent)
            {
                result.Add(v);
                foreach (var u in v.children)
                {
                    CreateResult(u, false, result, presentChildren, absentChildren);
                }
            }
            else 
            {
                foreach (var u in absentChildren[v])
                {
                    CreateResult(u, false, result, presentChildren, absentChildren);
                }
                foreach (var u in presentChildren[v])
                {
                    CreateResult(u, true, result, presentChildren, absentChildren);
                }
            }
        }
        static List<Graph> DFSDisconnected(Graph G)
        {
            var connectedGraphs = new List<Graph>();
            var visited = new HashSet<Vertex>();
            foreach (var vertex in G.vertices)
            {
                if (!visited.Contains(vertex))
                { 
                    var connectedComponent = DFSConnected(vertex);
                    int vertexDegreesSum = 0;
                    foreach (var v in connectedComponent.vertices)
                    {
                        visited.Add(v);
                        vertexDegreesSum += v.neighbors.Count;
                    }
                    if (vertexDegreesSum < 2 * connectedComponent.vertices.Count) // drzewo
                    {
                        connectedGraphs.Insert(0, connectedComponent); // chcemy drzewo na początku
                    }
                    else
                    {
                        connectedGraphs.Add(connectedComponent);
                    }
                }
            }
            return connectedGraphs;
        }
        
        static Graph DFSConnected(Vertex vertex) 
        {
            var graph = new Graph(); 
            var stack = new Stack<(Vertex v,Vertex? parentV)>();
            var visited = new HashSet<Vertex>() { vertex };
            stack.Push((vertex, null));
            while (stack.Count > 0) 
            { 
                var vertexParentPair = stack.Pop();
                var currentVertex = vertexParentPair.v;
                graph.vertices.Add(currentVertex);
                
                foreach (var neighbor in currentVertex.neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        stack.Push((neighbor, currentVertex));
                        visited.Add(neighbor);
                    }
                    else if (neighbor != vertexParentPair.parentV)
                    {
                        graph.vertexOnCycle = neighbor;
                    }
                }
            }
            return graph;
        }
        static List<Vertex>[] ConstructLevels(Vertex root)
        {
            var levels = new List<List<Vertex>> { new List<Vertex> { root } };
            var queue = new Queue<Vertex>();
            var visited = new HashSet<Vertex>
            {
                root
            };
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();

                var newLevelFound = false;
                foreach (var neighbor in currentVertex.neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (!newLevelFound)
                        {
                            newLevelFound = true;
                            levels.Add(new List<Vertex>());
                        }
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        levels.Last().Add(neighbor);
                    }
                }
            }
            return levels.ToArray();
        }
    }
}
