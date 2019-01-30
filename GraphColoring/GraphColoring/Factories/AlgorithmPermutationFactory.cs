using System.Collections.Generic;
using System.Linq;
using GraphColoring.Enums;
using GraphColoring.Helpers;
using GraphColoring.Models;

namespace GraphColoring.Factories
{
    public static class AlgorithmPermutationFactory
    {
        public static int GetUsedColorsByPermutation(PermutationType permutationType, List<Edge> edges, int vertexCount = 100)
        {
            var graph = GraphFactory.Create(edges, vertexCount);
            var cloneGraph = graph.ToList();

            switch (permutationType)
            {
                case PermutationType.ColoredNeibourColorsCountNeighborCount:
                    PermutationAlgorithmsHelper.ColoredNeibourColorsCountNeighborCount(cloneGraph);
                    break;
                case PermutationType.ColoredNeibourNeighborCountColorsCount:
                    PermutationAlgorithmsHelper.ColoredNeibourNeighborCountColorsCount(cloneGraph);
                    break;
                case PermutationType.ColorsCountColoredNeibourNeighborCount:
                    PermutationAlgorithmsHelper.ColorsCountColoredNeibourNeighborCount(cloneGraph);
                    break;
                case PermutationType.ColorsCountNeighborCountColoredNeibour:
                    PermutationAlgorithmsHelper.ColorsCountNeighborCountColoredNeibour(cloneGraph);
                    break;
                case PermutationType.NeighborCountColoredNeibourColorsCount:
                    PermutationAlgorithmsHelper.NeighborCountColoredNeibourColorsCount(cloneGraph);
                    break;
                case PermutationType.NeighborCountColorsCountColoredNeibour:
                    PermutationAlgorithmsHelper.NeighborCountColorsCountColoredNeibour(cloneGraph);
                    break;
                default:
                    break;
            }

            return graph.Max(vertex => vertex.Color);
        }
    }
}