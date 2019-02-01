using System;
using System.Collections.Generic;
using System.Linq;
using GraphColoring.Models;

namespace GraphColoring.Factories
{
    public static class GraphFactory
    {
        public static List<Vertex> Create(List<Edge> edges, int verticesCount = 100)
        {
            var result = new List<Vertex>();

            for (int i=0; i < verticesCount; i++)
            {
                result.Add( new Vertex( i ));
            }

            edges.ForEach(edge =>
            {
                var vertex1 = result.FirstOrDefault(v => v.Name == edge.Vertex1Name);
                var vertex2 = result.FirstOrDefault(v => v.Name == edge.Vertex2Name);

                if (vertex1 != null && vertex2 != null)
                {
                    vertex1.Neighbors.Add(vertex2);
                    vertex2.Neighbors.Add(vertex1);
                }
            });

            return result;
        }

        /// <summary>
        /// Just for tests (full graph)
        /// </summary>
        /// <param name="verticesCount"></param>
        /// <returns>List of edges</returns>
        public static List<Edge> CreateEdgesForRandomGraph(int verticesCount = 100)
        {
            var result = new List<Edge>();

            for (int i=1; i <= verticesCount; i++)
            {
                for (int j=i+1; j <= verticesCount; j++)
                {
                    result.Add(new Edge
                    {
                        Vertex1Name = i,
                        Vertex2Name = j
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Generowanie losowego grafu
        /// </summary>
        /// <param name="verticesCount">Ilość wierzchołków w grafie</param>
        /// <param name="groupCount">Ilość grup w grafie</param>
        /// <param name="connectionsInGroup">Ilość połączeń w grupie wychodzące z każdego wierzchołka w grupie</param>
        /// <param name="connectionsBetweenGroups"></param>
        /// <returns>Liste krawędzi losowych</returns>
        public static List<Edge> CreateRandomGraph(int verticesCount, int groupCount, int connectionsInGroup, int connectionsBetweenGroups)
        {
            List<Vertex> vertices = new List<Vertex>();
            var result = new List<Edge>();
            for (int i = 0; i < verticesCount; i++)
            {
                vertices.Add(new Vertex(i));
                
            }
            Random random = new Random();
            int interval = (verticesCount / groupCount);
            int random1 = 0;
            int random2 = 0;
            for (int i = 0; i < groupCount; i++)
            {
                Console.WriteLine("Group "+i);
                for (int j = 0; j < interval; j++)
                {
                    random2 = i * interval + j;
                    while (vertices[random2].Neighbors.Count < connectionsInGroup)
                    {
                        random1 = random.Next(i * interval, i * interval + interval);

                        if (!vertices[random2].Neighbors.Contains(vertices[random1]) && (random2) != random1)
                        {
                            result.Add(new Edge
                            {
                                Vertex1Name = random2,
                                Vertex2Name = random1
                            });
                            vertices[random2].Neighbors.Add(vertices.First(x => x.Name == random1));
                            vertices[random1].Neighbors.Add(vertices.First(x => x.Name == random2));
                            Console.WriteLine("Connecting vertice " + vertices[random2].Name + "  =>  " + vertices[random1].Name);
                        }

                    }
                }
            }
            List<int> groupUsed = new List<int>();
            int group1 = random.Next(0, groupCount);

            List<Tuple<int,int>> connectedVectors = new List<Tuple<int,int>>();
            while (groupUsed.Count != groupCount)
            {
                int group2 = random.Next(0, groupCount);
                if (group1 != group2 && !groupUsed.Contains(group1))
                {
                    Console.WriteLine("Connecting group " + group1 + " with group " + group2);
               
                    groupUsed.Add(group1);

                    for (int j = 0; j < connectionsBetweenGroups; j++)
                    {
                        random1 = random.Next(group1 * interval, group1 * interval + interval);
                        random2 = random.Next(group2 * interval, group2 * interval + interval);
                        bool isRepeated=connectedVectors.Any(v => (v.Item1 == random1 && v.Item2==random2)|| (v.Item1 == random2 && v.Item2 == random1));
                        if(isRepeated)
                        {
                            Console.WriteLine("Skipping this attempt " +random1+" => " +random2 +". Repeat detected!");
                            j--;
                            continue;
                        }
                        Console.WriteLine("Connecting vertice " + vertices[random1].Name + "  =>  " + vertices[random2].Name);
                        result.Add(new Edge
                        {
                            Vertex1Name = random1,
                            Vertex2Name = random2
                        });
                        connectedVectors.Add(new Tuple<int, int>(random1, random2));
                        vertices[random1].Neighbors.Add(vertices.First(x => x.Name == random2));
                        vertices[random2].Neighbors.Add(vertices.First(x => x.Name == random1));
                    }
                }
                group1 = group2;
            }
            //Check if any left empty
            var emptyVertices = vertices.FindAll(vertex => vertex.Neighbors.Count == 0);
            if (emptyVertices.Count != 0)
            {
                foreach (var vertex in emptyVertices)
                {
                    var neighbour = vertices.FirstOrDefault(x => x.Neighbors.Count > 5);
                    result.Add(new Edge
                    {
                        Vertex1Name = vertex.Name,
                        Vertex2Name = neighbour.Name
                    });
                    vertices[vertex.Name].Neighbors.Add(vertices.First(x => x.Name == random2));
                    vertices[neighbour.Name].Neighbors.Add(vertices.First(x => x.Name == vertex.Name));
                }
            }
            return result;
        }

        /// <summary>
        /// Just for tests 
        /// </summary>
        /// <param name="verticesCount"></param>
        /// <returns>List of edges</returns>
        public static List<Edge> CreateEdgesForSampleGraph()
        {
            var result = new List<Edge>();

            result.Add(new Edge { Vertex1Name = 1, Vertex2Name = 3 });
            result.Add(new Edge { Vertex1Name = 1, Vertex2Name = 5 });
            result.Add(new Edge { Vertex1Name = 2, Vertex2Name = 3 });
            result.Add(new Edge { Vertex1Name = 2, Vertex2Name = 5 });
            result.Add(new Edge { Vertex1Name = 3, Vertex2Name = 4 });
            result.Add(new Edge { Vertex1Name = 4, Vertex2Name = 5 });

            return result;
        }

        /// <summary>
        /// Depth first search
        /// </summary>
        /// <param name="graph">List of vertices as a graph</param>
        /// <returns></returns>
        public static List<int> DFS(List<Vertex> graph)
        {
            var visited = new List<int>();
            graph.OrderBy(x => x.Name);
            int start = graph.FirstOrDefault().Name;
            if (graph.FirstOrDefault(vertice => vertice.Name == start) == null)
                return visited;

            var stack = new Stack<int>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in graph.FirstOrDefault(n => n.Name == vertex).Neighbors)
                    if (!visited.Contains(neighbor.Name))
                        stack.Push(neighbor.Name);
            }

            return visited;
        }
    }
}