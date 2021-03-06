﻿using System;
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
            var isConnected= graph.FirstOrDefault(n => n.Neighbors.Count == 0);
            if(isConnected!=null)
            {
                Console.WriteLine("Graph is not connected please check parameters for creating graph");
                return -1;
            }
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

            var notColoredVertices = graph.Where(vertex => vertex.Color == 0).ToList();
            List<int> dfspath = GraphFactory.DFS(graph);
            if(dfspath.Count!=graph.Count)
            {
                Console.WriteLine("DFS nieudany, prosze o dobranie parametrow");
            }
            return graph.Max(vertex => vertex.Color);
        }
    }
}