using System;
using System.Linq;
using AmountOrganization;
using AmountOrganization.DustStrategy;

var inputs = 50;
var users = 40;

var randomAmounts = Sample.Amounts.RandomElements(inputs);
var groups = randomAmounts.RandomGroups(users).ToArray();

IMixer mixer = new DustMixer();
var mix = mixer.CompleteMix(groups).ToArray();

Console.WriteLine($"Number of users:\t{users}");
Console.WriteLine($"Number of inputs:\t{inputs}");
Console.WriteLine($"Number of outputs:\t{mix.Sum(x => x.Count())}");

Console.WriteLine();

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
    Console.ForegroundColor = ConsoleColor.Gray;
}

Console.ReadLine();
