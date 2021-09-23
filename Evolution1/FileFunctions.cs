using System.IO;

namespace ConsoleEvolution
{
    public static class FileFunctions
    {
        public static void LogInfo(LogInfo logInfo, bool enableFileLog, string directory)
        {
            System.Console.WriteLine(logInfo.Message);

            if (!enableFileLog)
                return;

            using var fileStream = new StreamWriter(File.Open($"{directory}\\Population-{logInfo.PopulationNumber}.tsv", FileMode.OpenOrCreate));
            foreach (var position in logInfo.Positions)
                fileStream.WriteLine($"{position.X}\t{position.Y}");
        }

        public static bool PrepareInitialFile(TargetFunction targetFunction, (double Start, double End) range, string directory)
        {
            if (!Directory.Exists(directory))
                return false;

            Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
            var step = (range.End - range.Start) / ushort.MaxValue;
            using var file = new StreamWriter(File.Open($"{directory}\\initial.tsv", FileMode.OpenOrCreate));
            for (var i = 0; i < ushort.MaxValue; i++)
            {
                var x = range.Start + i * step;
                var y = targetFunction(x);
                file.WriteLine($"{i}\t{x}\t{y}");
            }
            return true;
        }
    }
}
