using ConsoleEvolution;

using System;
using System.IO;

if (args.Length == 0) return;


(double Start, double End) range = (-20, -2.3);

TargetFunction function = x => Math.Cos(2 * x) / (x * x);


PrepareInitialFile(function, range);


var algorithm = new Algorithm
    (
        function,
        LogInfo,
        range,
        populationDeviationStopFactor: 0.001,
        initialPopulationSize: 250
    );


var result = algorithm.Solve();

System.Console.WriteLine($"\n\tResult: {result}");


void LogInfo(LogInfo logInfo)
{
    System.Console.WriteLine(logInfo.Message);
    using var fileStream = new StreamWriter(File.Open($"{args[0]}\\Population-{logInfo.PopulationNumber}.tsv", FileMode.OpenOrCreate));
    foreach (var position in logInfo.Positions)
        fileStream.WriteLine($"{position.X}\t{position.Y}");
}

void PrepareInitialFile(TargetFunction targetFunction, (double Start, double End) range)
{
    Directory.Delete(args[0], true);
    Directory.CreateDirectory(args[0]);
    var step = (range.End - range.Start) / ushort.MaxValue;
    using var file = new StreamWriter(File.Open($"{args[0]}\\initial.tsv", FileMode.OpenOrCreate));
    for (var i = 0; i < ushort.MaxValue; i++)
    {
        var x = range.Start + i * step;
        var y = targetFunction(x);
        file.WriteLine($"{i}\t{x}\t{y}");
    }
}
