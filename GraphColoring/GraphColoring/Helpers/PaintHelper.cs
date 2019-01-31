using System.Collections.Generic;
using System.Linq;
using GraphColoring.Models;

namespace GraphColoring.Helpers
{
    public static class PaintHelper
    {


        /// <summary>
        /// Koloruje wierzchołek
        /// </summary>
        /// <param name="vertex">Wierzchołek do pokolorowania</param>
        /// <returns>Kolor wierzchołka</returns>
        public static int PaintVertex(Vertex vertex)
        {
            int color = 1;

            while (vertex.Color == 0)
            {
                if (vertex.Neighbors.All(vNeigh => vNeigh.Color != color))
                {
                    vertex.Color = color;
                    return color;
                }

                color++;
            }

            return vertex.Color;
        }
    }
}