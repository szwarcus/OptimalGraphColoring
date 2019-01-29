using System.Collections.Generic;
using GraphColoring.Enums;
using GraphColoring.Helpers;
using GraphColoring.Models;

namespace GraphColoring.Factories
{
    public static class AlgorithmPermutationFactory
    {
        public static int GetUsedColorsByPermutation(PermutationType permutationType, List<Edge> edges, int vertexCount = 100)
        {
            switch (permutationType)
            {
                case PermutationType.ColoredNeibourColorsCountNeighborCount:
                    return PermutationAlgorithmsHelper.ColoredNeibourColorsCountNeighborCount(edges, vertexCount);
                case PermutationType.ColoredNeibourNeighborCountColorsCount:
                    return PermutationAlgorithmsHelper.ColoredNeibourNeighborCountColorsCount(edges, vertexCount);
                case PermutationType.ColorsCountColoredNeibourNeighborCount:
                    return PermutationAlgorithmsHelper.ColorsCountColoredNeibourNeighborCount(edges, vertexCount);
                case PermutationType.ColorsCountNeighborCountColoredNeibour:
                    return PermutationAlgorithmsHelper.ColorsCountNeighborCountColoredNeibour(edges, vertexCount);
                case PermutationType.NeighborCountColoredNeibourColorsCount:
                    return PermutationAlgorithmsHelper.NeighborCountColoredNeibourColorsCount(edges, vertexCount);
                case PermutationType.NeighborCountColorsCountColoredNeibour:
                    return PermutationAlgorithmsHelper.NeighborCountColorsCountColoredNeibour(edges, vertexCount);
                default:
                    return -1;
            }
        }
    }
}