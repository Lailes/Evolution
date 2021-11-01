#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;using Algorithm;

namespace Algorithm
{
    public static class PathExtensions
    {

        public static IList<int> ConvertToForward(this IList<int> path)
        {
            var resultPath = new List<int> { 1 };
            
            while (resultPath.Count != path.Count && (resultPath[^1] != 1 || resultPath.Count == 1)) 
                resultPath.Add(path[resultPath[^1] - 1]);

            return resultPath;
        }

        public static IList<int> ConvertToNeighbourhood(this IList<int> path)
        {
            var result = new int[path.Count];

            for (var i = 0; i < path.Count - 1; i++) 
                result[path[i] - 1] = path[i + 1];

            result[path[^1] - 1] = 1;

            return result;
        }

        public static IList<int> Shuffle(this IList<int> path, IRandom random) =>
            path.OrderBy(_ => random.Next(0, path.Count * 10)).ToList();

        public static IList<int> GenerateForwardPath(int count, IRandom random) =>
            Enumerable
                .Repeat(1, 1)
                .Concat(Enumerable.Range(2, count - 1).ToList().Shuffle(random))
                .ToList();

        public static IList<Path> GetPathsNeighbour(this IEnumerable<int> path, PathTable pathTable) => 
            path.Select((t, i) => pathTable[i + 1, t]).ToList();


        public static Path? GetBestWay(Path? p1, Path? p2) =>
            (p1 is null, p2 is null) switch {
                (true,  false) => p2,
                (false,  true) => p1,
                (false, false) => p1.Length > p2.Length ? p2 : p1,
                (true,   true) => null
            };
    }
}