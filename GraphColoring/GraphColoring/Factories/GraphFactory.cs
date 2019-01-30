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

            for (int i=1; i <= verticesCount; i++)
            {
                result.Add(new Vertex { Name = i });
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
        /// Just for tests (full graph)
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
    }
}