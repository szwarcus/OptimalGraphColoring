using System;
using System.Collections.Generic;
using GraphColoring.Enums;
using GraphColoring.Factories;
using GraphColoring.Models;

namespace GraphColoring
{
    class Program
    {
        private static int _vertexCount = 5;

        static void Main(string[] args)
        {
            //var edges = GraphFactory.CreateEdgesForRandomGraph(_vertexCount);
            var edges = GraphFactory.CreateEdgesForSampleGraph();

            RunAllPermutations(edges, _vertexCount);

            Console.ReadKey();
        }

        private static void RunAllPermutations(List<Edge> edges, int vertexCount)
        {
            var usedColors = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.NeighborCountColoredNeibourColorsCount, edges, vertexCount);
            var usedColors2 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.NeighborCountColorsCountColoredNeibour, edges, vertexCount);
            var usedColors3 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColoredNeibourNeighborCountColorsCount, edges, vertexCount);
            var usedColors4 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColoredNeibourColorsCountNeighborCount, edges, vertexCount);
            var usedColors5 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColorsCountColoredNeibourNeighborCount, edges, vertexCount);
            var usedColors6 = AlgorithmPermutationFactory.GetUsedColorsByPermutation(PermutationType.ColorsCountNeighborCountColoredNeibour, edges, vertexCount);

            Console.WriteLine($"NeighborCountColoredNeibourColorsCount - Uzyte kolory: {usedColors}");
            Console.WriteLine($"NeighborCountColorsCountColoredNeibour - Uzyte kolory: {usedColors2}");
            Console.WriteLine($"ColoredNeibourNeighborCountColorsCount - Uzyte kolory: {usedColors3}");
            Console.WriteLine($"ColoredNeibourColorsCountNeighborCount - Uzyte kolory: {usedColors4}");
            Console.WriteLine($"ColorsCountColoredNeibourNeighborCount - Uzyte kolory: {usedColors5}");
            Console.WriteLine($"ColorsCountNeighborCountColoredNeibour - Uzyte kolory: {usedColors6}");
        }
    }
}