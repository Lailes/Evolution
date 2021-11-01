using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class PathTable
    {
        private readonly IList<Path> _paths;
        private readonly int _cityCount;
        
        public PathTable(IList<Path> paths, int cityCount) => (_paths, _cityCount) = (paths, cityCount);

        public Path this[int from, int to] => _paths.FirstOrDefault(path => path.From == from && path.To == to)
                                              ?? throw new Exception("NO WAAAY, NO FUCKING WAAAAAAY!!!");
        
        public Func<IList<int>, double> FitnessFunction =>
            ints => ints.Select((t, i) => this[i + 1, t]?.Length ?? 0.0).Sum();
        
        public int CityCount => _cityCount;


        public Path GetBestPath(int from, IList<int> visited) =>
            _paths
                .Where(path => path.From == from && !visited.Contains(path.To))
                .OrderBy(path => path.Length)
                .FirstOrDefault()
                ?? throw new Exception("NO WAAAY, NO FUCKING WAAAAAAY!!!");

    }
    
    public record Path(int From, int To, double Length);
}