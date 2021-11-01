using System;
using System.IO;
using Algorithm;

var path = PathLoader.LoadPathTable(args[0]);

var algorithm = new CoreAlgorithm(path, new ClassicRandom(), 100, 1.0, 0.2, 0.01);

var result = algorithm.FindPath();

var textOut = string.Join('-', result.ConvertToForward()) + "\n" + string.Join('-', result);
File.WriteAllText(args[1], textOut);
