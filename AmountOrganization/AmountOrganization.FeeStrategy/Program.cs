using System;
using System.Linq;
using AmountOrganization;
using AmountOrganization.FeeStrategy;

var inputCount = 50;
var userCount = 40;

var randomAmounts = Sample.Amounts.RandomElements(inputCount);
var groups = randomAmounts.RandomGroups(userCount).ToArray();

var mixer = new FeeMixer();
var mix = (mixer as IMixer).CompleteMix(groups).ToArray();

var outputCount = mix.Sum(x => x.Count());
var inputAmount = groups.SelectMany(x => x).Sum();
var outputAmount = mix.SelectMany(x => x).Sum();
var fee = inputAmount - outputAmount;
var size = inputCount * mixer.InputSize + outputCount * mixer.OutputSize;
var feeRate = (fee / size).ToSats();

Console.WriteLine($"Number of users:\t{userCount}");
Console.WriteLine($"Number of inputs:\t{inputCount}");
Console.WriteLine($"Number of outputs:\t{outputCount}");
Console.WriteLine($"Total in:\t\t{inputAmount} BTC");
Console.WriteLine($"Fee paid:\t\t{fee} BTC");
Console.WriteLine($"Size:\t\t\t{size} vbyte");
Console.WriteLine($"Fee rate:\t\t{feeRate} sats/vbyte");

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
