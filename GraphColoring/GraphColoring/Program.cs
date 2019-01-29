using System;
using System.Threading;
using System.Threading.Tasks;
using GraphColoring.Factories;
using GraphColoring.Helpers;

namespace GraphColoring
{
    class Program
    {
        private static int _vertexCount = 100;

        static void Main(string[] args)
        {
            var edges = GraphFactory.CreateEdgesForRandomGraph(_vertexCount);

            var usedColors = PermutationAlgorithmsHelper.NeighborColoredNeibourColorsCount(edges, _vertexCount);

            Task.Run(() => { }).Wait();
            

            Console.WriteLine($"Uzyte kolory: {usedColors}\n");
            Console.ReadKey();
        }
    }
}