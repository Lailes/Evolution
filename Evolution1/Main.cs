using System;

using ConsoleEvolution;
using static ConsoleEvolution.FileFunctions;


(double Start, double End) range = (-20, -2.3);

TargetFunction function = x => Math.Cos(2 * x) / (x * x);


var enableFileLog = PrepareInitialFile(function, range, args[0]);


var algorithm = new Algorithm
    (
        targetFunction: function,
        logingFunction: info => LogInfo(info, enableFileLog, args[0]),
        range: range,
        populationDeviationStopFactor: 0.001,
        initialPopulationSize: 250,
        bornPropability: 0.5,
        mutationPropability: 0.001
    );


var result = await algorithm.SolveAsync();

System.Console.WriteLine($"\n\tResult: {result}");
