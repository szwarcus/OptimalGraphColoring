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
        public static void NeighborCountColoredNeibourColorsCount(List<Vertex> graph)
        {
            Vertex vertex = null;
            var sortedByNeighbourCount = graph.OrderByDescending(x => x.Neighbors.Count).ToList();

            // koloruje wszystkie wierzchołki
            for (int i=0; i < graph.Count; i++)
            {
                vertex = sortedByNeighbourCount[i];
                if (vertex.Color != 0)
                {
                    continue;
                }

                var verticesInTie = new List<Vertex>();

                // szukam wierzchołki w remisie
                for (int j=i+1; j < graph.Count; j++)
                {
                    if (vertex.Neighbors.Count == sortedByNeighbourCount[j].Neighbors.Count)
                    {
                        if (sortedByNeighbourCount[j].Color == 0)
                        {
                            verticesInTie.Add(sortedByNeighbourCount[j]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // jesli nie ma remisu, to maluj i spojrz na kolejny wierzcholek
                if (verticesInTie.Count == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    continue;
                }

                // jesli byl remis to dodaj tez ten wierzcholek
                verticesInTie.Add(vertex);

                var coloredVertex = ColorByColoredNeighborCount(verticesInTie);
                
                // jesli wierzcholek pomalowany
                if (coloredVertex != null)
                {
                    // jesli pomalowany wierzcholek to inny wierzcholek niz ogladany i-ty wierzcholek, to trzeba obejrzec go jeszcze raz
                    if (coloredVertex != vertex)
                    {
                        i--;
                    }
                    continue;
                }

                coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

                // jesli wierzcholek pomalowany
                if (coloredVertex != null)
                {
                    // jesli pomalowany wierzcholek to inny wierzcholek niz ogladany i-ty wierzcholek, to trzeba obejrzec go jeszcze raz
                    if (coloredVertex != vertex)
                    {
                        i--;
                    }
                    continue;
                }

                PaintHelper.PaintVertex(vertex);
            }
        }

        /// <summary>
        /// Najpierw jest wykonywany algorytm zależny od ilości sąsiadów (im więcej tym większy priorytet).
        /// Następnie wykonywany jest algorytm zależny od ilości użytych barw w sąsiedztwie.
        /// Następnie wykonywany jest algorytm zależny od ilości pokolorowanych sąsiadów (im więcej tym większy priorytet).
        /// </summary>
        /// <param name="graph">Graf reprezentowany przez liczbę wszystkich wierzchołków</param>
        /// <returns>Pokolorowane wierzchołki oraz 1 jeśli wszystkie wierzchołki zostały pomalowane. 0 jeśli jakiś jest niepomalowany.</returns>
        public static void NeighborCountColorsCountColoredNeibour(List<Vertex> graph)
        {
            Vertex vertex = null;
            var sortedByNeighbourCount = graph.OrderByDescending(x => x.Neighbors.Count).ToList();

            // koloruje wszystkie wierzchołki
            for (int i = 0; i < graph.Count; i++)
            {
                vertex = sortedByNeighbourCount[i];
                if (vertex.Color != 0)
                {
                    continue;
                }

                var verticesInTie = new List<Vertex>();

                // szukam wierzchołki w remisie
                for (int j = i + 1; j < graph.Count; j++)
                {
                    if (vertex.Neighbors.Count == sortedByNeighbourCount[j].Neighbors.Count)
                    {
                        if (sortedByNeighbourCount[j].Color == 0)
                        {
                            verticesInTie.Add(sortedByNeighbourCount[j]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // jesli nie ma remisu, to maluj i spojrz na kolejny wierzcholek
                if (verticesInTie.Count == 0)
                {
                    PaintHelper.PaintVertex(vertex);
                    continue;
                }

                // jesli byl remis to dodaj tez ten wierzcholek
                verticesInTie.Add(vertex);

                var coloredVertex = ColorByDiffColorsInNeighborhood(verticesInTie);

                // jesli wierzcholek pomalowany
                if (coloredVertex != null)
                {
                    // jesli pomalowany wierzcholek to inny wierzcholek niz ogladany i-ty wierzcholek, to trzeba obejrzec go jeszcze raz
                    if (coloredVertex != vertex)
                    {
                        i--;
                    }
                    continue;
                }

                coloredVertex = ColorByColoredNeighborCount(verticesInTie);

                // jesli wierzcholek pomalowany
                if (coloredVertex != null)
                {
                    // jesli pomalowany wierzcholek to inny wierzcholek niz ogladany i-ty wierzcholek, to trzeba obejrzec go jeszcze raz
                    if (coloredVertex != vertex)
                    {
                        i--;
                    }
                    continue;
                }

                PaintHelper.PaintVertex(vertex);
            }
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

            vertices = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            if (vertices[0].Neighbors.Count == vertices[1].Neighbors.Count)
            {
                var countToCompare = vertices[0].Neighbors.Count;
                return vertices.Where(vertex => vertex.Neighbors.Count == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(vertices[0]);
            vertices.Remove(vertices[0]);
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
            vertices = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                       .ToList();

            // jest jakiś "remis"
            if (vertices[0].ColoredNeighbors == vertices[1].ColoredNeighbors)
            {
                var countToCompare = vertices[0].ColoredNeighbors;
                return vertices.Where(vertex => vertex.ColoredNeighbors == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(vertices[0]);
            vertices.Remove(vertices[0]);
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

            vertices = vertices.OrderByDescending(vertex => vertex.UsedColorsInNeighborhood.Count)
                               .ToList();

            // jeśli jest jakiś remis
            if (vertices[0].UsedColorsInNeighborhood.Count == vertices[1].UsedColorsInNeighborhood.Count)
            {
                var countToCompare = vertices[0].UsedColorsInNeighborhood.Count;
                return vertices.Where(vertex => vertex.UsedColorsInNeighborhood.Count == countToCompare).ToList();
            }

            PaintHelper.PaintVertex(vertices[0]);
            vertices.Remove(vertices[0]);
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

            vertices = vertices.OrderByDescending(vertex => vertex.Neighbors.Count).ToList();

            if (vertices[0].Neighbors.Count == vertices[1].Neighbors.Count)
            {
                var valueToCompare = vertices[0].Neighbors.Count;

                vertices = vertices.Where(vertex => vertex.Neighbors.Count == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = vertices[0];

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
            vertices = vertices.OrderByDescending(vertex => vertex.ColoredNeighbors)
                               .ToList();

            // nadal jest jakiś remis
            if (vertices[0].ColoredNeighbors == vertices[1].ColoredNeighbors)
            {
                var valueToCompare = vertices[0].ColoredNeighbors;

                vertices = vertices.Where(vertex => vertex.ColoredNeighbors == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = vertices[0];

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

            vertices = vertices.OrderByDescending(vertex => vertex.UsedColorsInNeighborhood.Count)
                               .ToList();

            if (vertices[0].UsedColorsInNeighborhood.Count == vertices[1].UsedColorsInNeighborhood.Count)
            {
                var valueToCompare = vertices[0].UsedColorsInNeighborhood.Count;

                vertices = vertices.Where(vertex => vertex.UsedColorsInNeighborhood.Count == valueToCompare).ToList();

                return coloredVertex;
            }

            coloredVertex = vertices[0];

            PaintHelper.PaintVertex(coloredVertex);

            return coloredVertex;
        }
        #endregion
    }
}