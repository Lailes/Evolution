using System.IO;
using System.Linq;

namespace Algorithm
{
    public static class CoordinatesLoader
    {
        public static CoordinateTable LoadCoordinateTable(string path) =>
            new( File
                .ReadAllLines(path)
                .Select(line => line.Split(' ').Select(double.Parse).ToArray())
                .Select(args => new Coordinate( (int) args[0], args[1], args[2]))
                .ToList() );
    }
}