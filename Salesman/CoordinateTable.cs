using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class CoordinateTable
    {
        public IList<Coordinate> Coordinates { get; }

        public CoordinateTable(IList<Coordinate> coordinates) =>
            Coordinates = coordinates;

        public Coordinate this[int city] => Coordinates.FirstOrDefault(c => c.City == city);
    }
    
    public record Coordinate(int City, double X, double Y);
}