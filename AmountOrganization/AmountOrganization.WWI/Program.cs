using System;
using System.Linq;
using AmountOrganization;
using AmountOrganization.WWI;

Console.WriteLine("Hello World!");

var inputs = 50;
var users = 40;

var randomAmounts = Sample.Amounts.GetRandomElements(inputs);
var groups = randomAmounts.RandomGroups(users).ToArray();

var outputCount = 0;
IMixer mixer = new WwiMixer();
var mix = mixer.CompleteMix(groups);
foreach (var userOutputs in mix)
{
    outputCount += userOutputs.Count();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{userOutputs.Sum()}\t");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"{string.Join(", ", userOutputs.OrderByDescending(x => x))}");
    Console.WriteLine();
}

foreach (var (value, count) in mix
    .SelectMany(x => x).GroupBy(x => x)
    .ToDictionary(x => x.Key, y => y.Count())
    .Select(x => (x.Key, x.Value))
    .OrderBy(x => x.Key))
{
    if (count == 1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    Console.WriteLine($"There are {count} occurrences of {value} BTC output.");
    Console.ForegroundColor = ConsoleColor.White;
}

Console.WriteLine();
Console.WriteLine($"Number of users:\t{users}");
Console.WriteLine($"Number of inputs:\t{inputs}");
Console.WriteLine($"Number of outputs:\t{outputCount}");

Console.ReadLine();
