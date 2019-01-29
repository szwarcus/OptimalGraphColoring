using System.Collections.Generic;
using System.Linq;
using GraphColoring.Factories;
using GraphColoring.Models;

namespace GraphColoring.Helpers
{
    public static class PermutationAlgorithmsHelper
    {
        #region Public Methods
        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int NeighborCountColoredNeibourColorsCount(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var usedColorsByNeighborCount = ColorByNeighborCountToTie(graph);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCountToTie(notColoredVertices);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości różnych kolorów w sąsiedztwie "do końca"
            notColoredVertices = notColoredVertices.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhood(notColoredVertices);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            return maxUsedColors;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int NeighborCountColorsCountColoredNeibour(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var usedColorsByNeighborCount = ColorByNeighborCountToTie(graph);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości różnych kolorów w sąsiedztwie "do remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhoodToTie(notColoredVertices);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów "do końca"
            notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCount(notColoredVertices);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            return maxUsedColors;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet). 
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int ColoredNeibourNeighborCountColorsCount(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "remisu"
            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCountToTie(graph);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByNeighborCount = ColorByNeighborCountToTie(graph);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości różnych kolorów w sąsiedztwie "do końca"
            notColoredVertices = notColoredVertices.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhood(notColoredVertices);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            return maxUsedColors;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet). 
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int ColoredNeibourColorsCountNeighborCount(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "remisu"
            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCountToTie(graph);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhoodToTie(notColoredVertices);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości sąsiadów "do końca"
            notColoredVertices = notColoredVertices.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByNeighborCount = ColorByNeighborCount(notColoredVertices);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;
            
            return maxUsedColors;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet). 
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów.
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int ColorsCountColoredNeibourNeighborCount(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhoodToTie(graph);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCountToTie(notColoredVertices);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości sąsiadów "do końca"
            notColoredVertices = notColoredVertices.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByNeighborCount = ColorByNeighborCount(notColoredVertices);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;

            return maxUsedColors;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet). 
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów.
        /// </summary>
        /// <param name="edges">Krawędzie</param>
        /// <param name="vertexCount">Liczba wierzchołków</param>
        /// <returns>Liczba użytych kolorów</returns>
        public static int ColorsCountNeighborCountColoredNeibour(List<Edge> edges, int vertexCount = 100)
        {
            int maxUsedColors = -1;

            var graph = GraphFactory.Create(edges, vertexCount);

            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var usedColorsByColorsInNeighborhood = ColorByDiffColorsInNeighborhoodToTie(graph);
            maxUsedColors = usedColorsByColorsInNeighborhood > maxUsedColors ? usedColorsByColorsInNeighborhood : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości sąsiadów "do remisu"
            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByNeighborCount = ColorByNeighborCountToTie(notColoredVertices);
            maxUsedColors = usedColorsByNeighborCount > maxUsedColors ? usedColorsByNeighborCount : maxUsedColors;

            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "końca"
            notColoredVertices = notColoredVertices.Where(vertex => vertex.Color == 0).ToList();

            var usedColorsByColoredNeighborCount = ColorByColoredNeighborCount(notColoredVertices);
            maxUsedColors = usedColorsByColoredNeighborCount > maxUsedColors ? usedColorsByColoredNeighborCount : maxUsedColors;

            return maxUsedColors;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet). 
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę sąsiadów.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByNeighborCountToTie(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            var sortedGraphByNeighorsCount = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle samo sąsiadów
            for (int i = 0; i < sortedGraphByNeighorsCount.Count; i++)
            {
                var vertex = sortedGraphByNeighorsCount[i];

                // wierzchołek niepomalowany, więc można malować
                if (vertex.Color == 0)
                {
                    if (i < sortedGraphByNeighorsCount.Count - 1
                        && vertex.Neighbors.Count == sortedGraphByNeighorsCount[i + 1].Neighbors.Count)
                    {
                        return maxUsedColors;
                    }
                    else
                    {
                        var usedColors = PaintHelper.PaintVertex(vertex);
                        maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                    }
                }
            }

            return maxUsedColors;
        }

        /// <summary>
        /// Algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet). 
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByNeighborCount(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            var sortedGraphByNeighorsCount = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            sortedGraphByNeighorsCount.ForEach(vertex =>
            {
                if (vertex.Color == 0)
                {
                    var usedColors = PaintHelper.PaintVertex(vertex);
                    maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                }
            });

            return maxUsedColors;
        }

        /// <summary>
        /// Algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę pokolorowanych sąsiadów.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByColoredNeighborCountToTie(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            // liczymy liczbę pomalowanych sąsiadów
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh => vertex.ColoredNeighbors += neigh.Color != 0 ? 1 : 0);
            });

            //listę wierzchołków sortuję ich po ilości pokolorowanych sąsiadów
            var sortedGraphByColoredNeighborsCount = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                                                             .ToList();

            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle samo pokolorowanych sąsiadów
            for (int i = 0; i < sortedGraphByColoredNeighborsCount.Count; i++)
            {
                var vertex = sortedGraphByColoredNeighborsCount[i];
                if (vertex.Color == 0)
                {
                    if (i < sortedGraphByColoredNeighborsCount.Count - 1
                        && vertex.ColoredNeighbors == sortedGraphByColoredNeighborsCount[i + 1].ColoredNeighbors)
                    {
                        return maxUsedColors;
                    }
                    else
                    {
                        var usedColors = PaintHelper.PaintVertex(vertex);
                        maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                    }
                }
            }

            return maxUsedColors;
        }

        /// <summary>
        /// Algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByColoredNeighborCount(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            // liczymy liczbę pomalowanych sąsiadów
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh => vertex.ColoredNeighbors += neigh.Color != 0 ? 1 : 0);
            });

            //listę wierzchołków sortuję ich po ilości pokolorowanych sąsiadów
            var sortedGraphByColoredNeighborsCount = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                                                             .ToList();

            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle samo pokolorowanych sąsiadów
            sortedGraphByColoredNeighborsCount.ForEach(vertex =>
            {
                if (vertex.Color == 0)
                {
                    var usedColors = PaintHelper.PaintVertex(vertex);
                    maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                }
            });

            return maxUsedColors;
        }

        /// <summary>
        /// Algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet).
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę różnych 
        /// kolorów wśród sąsiadów.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByDiffColorsInNeighborhoodToTie(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            // liczymy wśród niepomalowanych wierzchołków liczbę użytych kolorów w ich sąsiedztwie
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh =>
                {
                    // jesli wierzchołek jest pomalowany i jego kolor nie znajduje się w dotychczasowych kolorach w sąsiedztwie
                    if (neigh.Color != 0 && !vertex.UsedColorsInNeighborhood.Any(x => x == neigh.Color))
                    {
                        vertex.UsedColorsInNeighborhood.Add(neigh.Color);
                    }
                });
            });

            var sortedGraphByUsedColorsInNeighborhood = vertices.OrderByDescending(vertex => vertex.UsedColorsInNeighborhood.Count)
                                                                .ToList();


            // pętla wykonywana do momentu, aż jakaś para wierzchołków będzie miała tyle barw w jego sąsiedztwie
            for (int i = 0; i < sortedGraphByUsedColorsInNeighborhood.Count; i++)
            {
                var vertex = sortedGraphByUsedColorsInNeighborhood[i];
                if (vertex.Color == 0)
                {
                    if (i < sortedGraphByUsedColorsInNeighborhood.Count - 1
                        && vertex.ColoredNeighbors == sortedGraphByUsedColorsInNeighborhood[i + 1].ColoredNeighbors)
                    {
                        return maxUsedColors;
                    }
                    else
                    {
                        var usedColors = PaintHelper.PaintVertex(vertex);
                        maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                    }
                }
            }

            return maxUsedColors;
        }

        /// <summary>
        /// Algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca maksymalną liczbę użytych kolorów.</returns>
        private static int ColorByDiffColorsInNeighborhood(List<Vertex> vertices)
        {
            int maxUsedColors = -1;

            // liczymy wśród niepomalowanych wierzchołków liczbę użytych kolorów w ich sąsiedztwie
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh =>
                {
                    // jesli wierzchołek jest pomalowany i jego kolor nie znajduje się w dotychczasowych kolorach w sąsiedztwie
                    if (neigh.Color != 0 && !vertex.UsedColorsInNeighborhood.Any(x => x == neigh.Color))
                    {
                        vertex.UsedColorsInNeighborhood.Add(neigh.Color);
                    }
                });
            });

            var sortedGraphByUsedColorsInNeighborhood = vertices.OrderByDescending(vertex => vertex.UsedColorsInNeighborhood.Count)
                                                                .ToList();

            // pętla wykonywana do końca
            sortedGraphByUsedColorsInNeighborhood.ForEach(vertex =>
            {
                if (vertex.Color == 0)
                {
                    var usedColors = PaintHelper.PaintVertex(vertex);
                    maxUsedColors = usedColors > maxUsedColors ? usedColors : maxUsedColors;
                }
            });

            return maxUsedColors;
        }
        #endregion
    }
}