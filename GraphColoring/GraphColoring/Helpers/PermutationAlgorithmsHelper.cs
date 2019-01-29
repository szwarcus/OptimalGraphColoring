using System.Collections.Generic;
using System.Linq;
using GraphColoring.Factories;
using GraphColoring.Models;

namespace GraphColoring.Helpers
{
    public static class PermutationAlgorithmsHelper
    {
        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int NeighborColoredNeibourColorsCount(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            var sortedGraphByNeighorsCount = graph.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            bool endLoop = false;

            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle samo sąsiadów
            for (int i = 0; i < sortedGraphByNeighorsCount.Count && !endLoop; i++)
            {
                var vertex = sortedGraphByNeighorsCount[i];
                if (vertex.Color == 0)
                {
                    if (i < sortedGraphByNeighorsCount.Count - 1 
                        && vertex.Neighbors.Count == sortedGraphByNeighorsCount[i+1].Neighbors.Count)
                    {
                        endLoop = true;
                    }
                    else
                    {
                        var usedColors = PaintHelper.PaintVertex(vertex);
                        maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                    }
                }
            }

            // pobieram listę wierzchołków jeszcze niepomalowanych i sortuję ich po ilości pokolorowanych sąsiadów
            var sortedGraphByColoredNeighborsCount = sortedGraphByNeighorsCount.Where(vertex => vertex.Color == 0).ToList();

            // liczymy liczbę pomalowanych sąsiadów
            for (int i = 0; i < sortedGraphByColoredNeighborsCount.Count; i++)
            {
                var vertex = sortedGraphByColoredNeighborsCount[i];
                vertex.Neighbors.ForEach(neigh => vertex.ColoredNeighbors += neigh.Color != 0 ? 1 : 0);
            };

            //listę wierzchołków sortuję ich po ilości pokolorowanych sąsiadów
            sortedGraphByColoredNeighborsCount = sortedGraphByColoredNeighborsCount.OrderByDescending(vertex => vertex.ColoredNeighbors)
                                                                                   .ToList();

            endLoop = false;
            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle samo pokolorowanych sąsiadów
            for (int i = 0; i < sortedGraphByColoredNeighborsCount.Count && !endLoop; i++)
            {
                var vertex = sortedGraphByColoredNeighborsCount[i];
                if (vertex.Color == 0)
                {
                    if (i < sortedGraphByNeighorsCount.Count - 1
                        && vertex.ColoredNeighbors == sortedGraphByNeighorsCount[i + 1].ColoredNeighbors)
                    {
                        endLoop = true;
                    }
                    else
                    {
                        var usedColors = PaintHelper.PaintVertex(vertex);
                        maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                    }
                }
            }

            var sortedGraphByUsedColorsInNeighborhood = sortedGraphByColoredNeighborsCount.Where(vertex => vertex.Color == 0).ToList();

            // liczymy wśród niepomalowanych wierzchołków liczbę użytych kolorów w ich sąsiedztwie
            for (int i=0; i < sortedGraphByUsedColorsInNeighborhood.Count; i++)
            {
                var vertex = sortedGraphByUsedColorsInNeighborhood[i];
                vertex.Neighbors.ForEach(neigh =>
                {
                    // jesli wierzchołek jest pomalowany i jego kolor nie znajduje się w dotychczasowych kolorach w sąsiedztwie
                    if (neigh.Color != 0 && !vertex.UsedColorsInNeighborhood.Any(x => x == neigh.Color))
                    {
                        vertex.UsedColorsInNeighborhood.Add(neigh.Color);
                    }
                });
            };

            sortedGraphByUsedColorsInNeighborhood = sortedGraphByUsedColorsInNeighborhood.OrderByDescending(vertex => vertex.UsedColorsInNeighborhood.Count)
                                                                                         .ToList();

            // pętla wykonywana do końca
            for (int i = 0; i < sortedGraphByColoredNeighborsCount.Count; i++)
            {
                var vertex = sortedGraphByColoredNeighborsCount[i];
                if (vertex.Color == 0)
                {
                    var usedColors = PaintHelper.PaintVertex(vertex);
                    maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                }
            }

            return maxUsedColors;
        }

        private static int ColorByNeighborCountToTie(List<Vertex> vertices)
        {
            int maxUsedColors = -1;



            return maxUsedColors;
        }
    }
}