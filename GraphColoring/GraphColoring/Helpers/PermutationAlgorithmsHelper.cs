using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int NeighborCountColoredNeibourColorsCount(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var verticesInTie = ColorByNeighborCountToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje jeden z wierzchołków zależny od ilości pokolorowanych sąsiadów
            var coloredVertex = ColorByColoredNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return NeighborCountColoredNeibourColorsCount(graph);
            }

            // koloruje wierzchołki, które były jeszcze w remisie
            coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return NeighborCountColoredNeibourColorsCount(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i=0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return NeighborCountColoredNeibourColorsCount(graph);
                }
            }

            return 0;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int NeighborCountColorsCountColoredNeibour(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var verticesInTie = ColorByNeighborCountToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje wierzchołki w remisie
            var coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return NeighborCountColorsCountColoredNeibour(graph);
            }

            // koloruje wierzchołki, które były jeszcze w remisie
            coloredVertex = ColorByColoredNeighborCount(verticesInTie);

            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return NeighborCountColorsCountColoredNeibour(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i = 0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return NeighborCountColorsCountColoredNeibour(graph);
                }
            }

            return 0;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet). 
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int ColoredNeibourNeighborCountColorsCount(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości sąsiadów do momentu "remisu"
            var verticesInTie = ColorByColoredNeighborCountToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje graf algorytmem zależnym od ilości sąsiadów
            var coloredVertex = ColorByNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColoredNeibourNeighborCountColorsCount(graph);
            }

            // koloruje wierzchołki w remisie algorytmem zależnym od ilości różnych kolorów w sąsiedztwie
            coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColoredNeibourNeighborCountColorsCount(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i = 0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return ColoredNeibourNeighborCountColorsCount(graph);
                }
            }

            return 0;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet). 
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int ColoredNeibourColorsCountNeighborCount(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości pokolorowanych sąsiadów do momentu "remisu"
            var verticesInTie = ColorByColoredNeighborCountToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColoredNeibourColorsCountNeighborCount(graph);
            }

            // koloruje graf algorytmem zależnym od ilości sąsiadów "do końca"
            coloredVertex = ColorByNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColoredNeibourColorsCountNeighborCount(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i = 0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return ColoredNeibourColorsCountNeighborCount(graph);
                }
            }

            return 0;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet). 
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów.
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int ColorsCountColoredNeibourNeighborCount(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var verticesInTie = ColorByDiffColorsInNeighborhoodToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje wierzchołki w remisie algorytmem zależnym od ilości pokolorowanych sąsiadów
            var coloredVertex = ColorByColoredNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColorsCountColoredNeibourNeighborCount(graph);
            }

            // koloruje graf algorytmem zależnym od ilości sąsiadów "do końca"
            coloredVertex = ColorByNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColorsCountColoredNeibourNeighborCount(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i = 0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return ColorsCountColoredNeibourNeighborCount(graph);
                }
            }

            return 0;
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet). 
        /// Następnie wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów.
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static int ColorsCountNeighborCountColoredNeibour(List<Vertex> graph)
        {
            // koloruje graf algorytmem zależnym od ilości użytych barw w sąsiedztwie "do remisu"
            var verticesInTie = ColorByDiffColorsInNeighborhoodToTie(graph);

            // nie było momentu remisu i kończę
            if (verticesInTie.Count == 0)
            {
                return 1;
            }

            // koloruje wierzchołki w remisie algorytmem zależnym od ilości sąsiadów
            var coloredVertex = ColorByNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColorsCountNeighborCountColoredNeibour(graph);
            }

            // koloruje wierzchołki w remisie algorytmem zależnym od ilości pokolorowanych sąsiadów
            coloredVertex = ColorByColoredNeighborCount(verticesInTie);

            // udało się pomalować jeden wierzchołek i rekurencyjnie robimy tą funkcję jeszcze raz
            if (coloredVertex != null)
            {
                graph.Remove(coloredVertex);
                return ColorsCountNeighborCountColoredNeibour(graph);
            }

            // nie udało się pomalować ani jednego wierzchołka, więc koloruję pierwszego niepomalowanego
            for (int i = 0; i < verticesInTie.Count; i++)
            {
                var vertex = verticesInTie[i];
                if (vertex.Color == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    graph.Remove(vertex);
                    return ColorsCountNeighborCountColoredNeibour(graph);
                }
            }

            return 0;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet). 
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę sąsiadów.
        /// </summary>
        /// <param name="vertices">Lista wierzchołków</param>
        /// <returns>Zwraca listę wierzchołków w remisie.</returns>
        private static List<Vertex> ColorByNeighborCountToTie(List<Vertex> vertices)
        {
            if (vertices.Count == 0)
            {
                return vertices;
            }

            if (vertices.Count == 1)
            {
                PaintHelper.PaintVertex(vertices[0]);
                vertices.RemoveAt(0);
                return vertices;
            }

            var sortedGraphByNeighborsCount = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            if (sortedGraphByNeighborsCount[0].Neighbors.Count == sortedGraphByNeighborsCount[1].Neighbors.Count)
            {
                var countToCompare = sortedGraphByNeighborsCount[0].Neighbors.Count;
                return vertices.Where(vertex => vertex.Neighbors.Count == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(sortedGraphByNeighborsCount[0]);
            vertices.Remove(sortedGraphByNeighborsCount[0]);
            return ColorByNeighborCountToTie(vertices);
        }

        /// <summary>
        /// Algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę pokolorowanych sąsiadów.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>Zwraca listę wierzchołków w remisie.</returns>
        private static List<Vertex> ColorByColoredNeighborCountToTie(List<Vertex> vertices)
        {
            if (vertices.Count == 0)
            {
                return vertices;
            }

            if (vertices.Count == 1)
            {
                PaintHelper.PaintVertex(vertices[0]);
                vertices.RemoveAt(0);
                return vertices;
            }

            // liczymy liczbę pomalowanych sąsiadów
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh => vertex.ColoredNeighbors += neigh.Color != 0 ? 1 : 0);
            });

            //listę wierzchołków sortuję ich po ilości pokolorowanych sąsiadów
            var sortedGraphByColoredNeighborsCount = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                                                             .ToList();

            // jest jakiś "remis"
            if (sortedGraphByColoredNeighborsCount[0].ColoredNeighbors == sortedGraphByColoredNeighborsCount[1].ColoredNeighbors)
            {
                var countToCompare = sortedGraphByColoredNeighborsCount[0].ColoredNeighbors;
                return vertices.Where(vertex => vertex.ColoredNeighbors == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(sortedGraphByColoredNeighborsCount[0]);
            vertices.Remove(sortedGraphByColoredNeighborsCount[0]);
            return ColorByColoredNeighborCountToTie(vertices);
        }

        /// <summary>
        /// Algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet).
        /// Liczony do momentu "remisu", czyli gdy więcej niż jeden wierzchołek ma taką samą liczbę różnych 
        /// kolorów wśród sąsiadów.
        /// </summary>
        /// <param name="vertices">Lista wierzchołków</param>
        /// <returns>Zwraca listę wierzchołków w remisie.</returns>
        private static List<Vertex> ColorByDiffColorsInNeighborhoodToTie(List<Vertex> vertices)
        {
            if (vertices.Count == 0)
            {
                return vertices;
            }

            if (vertices.Count == 1)
            {
                PaintHelper.PaintVertex(vertices[0]);
                vertices.RemoveAt(0);
                return vertices;
            }

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

            // jeśli jest jakiś remis
            if (sortedGraphByUsedColorsInNeighborhood[0].UsedColorsInNeighborhood.Count == sortedGraphByUsedColorsInNeighborhood[1].UsedColorsInNeighborhood.Count)
            {
                var countToCompare = sortedGraphByUsedColorsInNeighborhood[0].UsedColorsInNeighborhood.Count;
                return vertices.Where(vertex => vertex.UsedColorsInNeighborhood.Count == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(sortedGraphByUsedColorsInNeighborhood[0]);
            vertices.Remove(sortedGraphByUsedColorsInNeighborhood[0]);
            return ColorByDiffColorsInNeighborhoodToTie(vertices);
        }

        /// <summary>
        /// Algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet). 
        /// </summary>
        /// <param name="vertices">Lista wierzchołków w remisie</param>
        /// <returns>Zwraca pokolorowany wierzchołek lub null, gdy nie pokolorowany.</returns>
        private static Vertex ColorByNeighborCount(List<Vertex> vertices)
        {
            Vertex coloredVertex = null;

            if (vertices.Count < 2)
            {
                throw new Exception("Nieprawdidłowa lista sąsiadów");
            }

            var sortedGraphByNeighorsCount = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            if (sortedGraphByNeighorsCount[0].Neighbors.Count == sortedGraphByNeighorsCount[1].Neighbors.Count)
            {
                var valueToCompare = sortedGraphByNeighorsCount[0].Neighbors.Count;

                vertices = vertices.Where(vertex => vertex.Neighbors.Count == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = sortedGraphByNeighorsCount[0];

            PaintHelper.PaintVertex(coloredVertex);

            return coloredVertex;
        }

        /// <summary>
        /// Algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="vertices">Lista wierzchołków w remisie</param>
        /// <returns>Zwraca pokolorowany wierzchołek lub null, gdy nie pokolorowany.</returns>
        private static Vertex ColorByColoredNeighborCount(List<Vertex> vertices)
        {
            Vertex coloredVertex = null;

            if (vertices.Count < 2)
            {
                throw new Exception("Nieprawdidłowa lista sąsiadów");
            }

            // liczymy liczbę pomalowanych sąsiadów
            vertices.ForEach(vertex =>
            {
                vertex.Neighbors.ForEach(neigh => vertex.ColoredNeighbors += neigh.Color != 0 ? 1 : 0);
            });

            //listę wierzchołków sortuję ich po ilości pokolorowanych sąsiadów
            var sortedGraphByColoredNeighborsCount = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                                                             .ToList();

            // nadal jest jakiś remis
            if (sortedGraphByColoredNeighborsCount[0].ColoredNeighbors == sortedGraphByColoredNeighborsCount[1].ColoredNeighbors)
            {
                var valueToCompare = sortedGraphByColoredNeighborsCount[0].ColoredNeighbors;

                vertices = vertices.Where(vertex => vertex.ColoredNeighbors == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = sortedGraphByColoredNeighborsCount[0];

            PaintHelper.PaintVertex(coloredVertex);

            return coloredVertex;
        }

        /// <summary>
        /// Algorytm zależny od ilości użytych barw w sąsiedztwie (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="vertices">Lista wierzchołków w remisie</param>
        /// <returns>Zwraca pokolorowany wierzchołek lub null, gdy nie pokolorowany.</returns>
        private static Vertex ColorByDiffColorsInNeighborhood(List<Vertex> vertices)
        {
            Vertex coloredVertex = null;


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

            if (sortedGraphByUsedColorsInNeighborhood[0].UsedColorsInNeighborhood.Count == sortedGraphByUsedColorsInNeighborhood[1].UsedColorsInNeighborhood.Count)
            {
                var valueToCompare = sortedGraphByUsedColorsInNeighborhood[0].UsedColorsInNeighborhood.Count;

                vertices = vertices.Where(vertex => vertex.UsedColorsInNeighborhood.Count == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = sortedGraphByUsedColorsInNeighborhood[0];

            PaintHelper.PaintVertex(coloredVertex);

            return coloredVertex;
        }
        #endregion
    }
}