using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Algorithm
{
    public class PathLoader
    {
        public static PathTable LoadPathTable(string path)
        {
            List<Path> paths = new();
            
            var nums = File.ReadAllLines(path)
                .Select(line => line.Split(' ').Select(int.Parse).ToList())
                .ToList();

            if (nums == null)
                throw new FileLoadException(path);

            for (var i = 1; i <= nums.Count; i++)
                for (var j = 1; j <= nums[i-1].Count; j++)
                    if (nums[i-1][j-1] != 0)
                        paths.Add(new Path(i, j, nums[i-1][j-1]));

            return new PathTable(paths, nums.Count);
        } 
    }
}