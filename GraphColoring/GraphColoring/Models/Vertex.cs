using System.Collections.Generic;

namespace GraphColoring.Models
{
    public class Vertex
    {
        public int Name { get; set; }
        public int Color { get; set; } = 0;
        public List<int> UsedColorsInNeighborhood = new List<int>();
        public int ColoredNeighbors = 0; 
        public List<Vertex> Neighbors { get; set; } = new List<Vertex>();
        public Vertex(int _name)
        {
            Name = _name;
        }
    }
}