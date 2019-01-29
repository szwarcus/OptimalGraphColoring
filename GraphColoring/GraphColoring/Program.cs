using System;
using System.Collections.Generic;
using GraphColoring.Enums;
using GraphColoring.Factories;
using GraphColoring.Models;

namespace GraphColoring
{
    class Program
    {
        private static int _vertexCount = 100;

        static void Main(string[] args)
        {
            var edges = GraphFactory.CreateEdgesForRandomGraph(_vertexCount);

            RunAllPermutations(edges);

            Console.ReadKey();
        }

        private static void RunAllPermutations(List<Edge> edges)
        {
            var usedColors = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.NeighborCountColoredNeibourColorsCount, edges, _vertexCount);
            var usedColors2 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.NeighborCountColorsCountColoredNeibour, edges, _vertexCount);
            var usedColors3 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColoredNeibourNeighborCountColorsCount, edges, _vertexCount);
            var usedColors4 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColoredNeibourColorsCountNeighborCount, edges, _vertexCount);
            var usedColors5 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColorsCountColoredNeibourNeighborCount, edges, _vertexCount);
            var usedColors6 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColorsCountNeighborCountColoredNeibour, edges, _vertexCount);

            Console.WriteLine($"NeighborCountColoredNeibourColorsCount - Uzyte kolory: {usedColors}\n");
            Console.WriteLine($"NeighborCountColorsCountColoredNeibour - Uzyte kolory: {usedColors2}\n");
            Console.WriteLine($"ColoredNeibourNeighborCountColorsCount - Uzyte kolory: {usedColors3}\n");
            Console.WriteLine($"ColoredNeibourColorsCountNeighborCount - Uzyte kolory: {usedColors4}\n");
            Console.WriteLine($"ColorsCountColoredNeibourNeighborCount - Uzyte kolory: {usedColors5}\n");
            Console.WriteLine($"ColorsCountNeighborCountColoredNeibour - Uzyte kolory: {usedColors6}\n");
        }
    }
}