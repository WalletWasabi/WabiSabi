using System;
using System.Collections.Generic;
using System.Linq;
using AmountOrganization;
using AmountOrganization.DescPow2;

var inputCount = 100;
var remixRatio = 0.3;

var userCount = inputCount; // Only for the premix.
var preRandomAmounts = Sample.Amounts.RandomElements(inputCount);
var preGroups = preRandomAmounts.RandomGroups(userCount);

var preMixer = new DescPow2Mixer();
var preMix = (preMixer as IMixer).CompleteMix(preGroups);

var remixCount = (int)(inputCount * remixRatio);
var randomAmounts = Sample.Amounts.RandomElements(inputCount - remixCount).Concat(preMix.SelectMany(x => x).RandomElements(remixCount));
IEnumerable<decimal>[] inputGroups;
decimal[][] outputGroups;
DescPow2Mixer mixer;
int outputCount;
while (true)
{
    inputGroups = randomAmounts.RandomGroups(userCount).ToArray();
    mixer = new DescPow2Mixer();
    outputGroups = (mixer as IMixer).CompleteMix(inputGroups).Select(x => x.ToArray()).ToArray();

    outputCount = outputGroups.Sum(x => x.Length);

    // Output count must be maximum 1.5x as much as the input count.
    // This simulates many mixes.
    if (inputCount < outputCount && inputCount.Almost(outputCount, inputCount / 2))
    {
        break;
    }

    if (inputCount > outputCount)
    {
        userCount++;
    }
    else
    {
        userCount--;
    }
}

if (inputGroups.SelectMany(x => x).Sum() <= outputGroups.SelectMany(x => x).Sum())
{
    throw new InvalidOperationException("Bug. Transaction doesn't pay fees.");
}

var inputAmount = inputGroups.SelectMany(x => x).Sum();
var outputAmount = outputGroups.SelectMany(x => x).Sum();
var fee = inputAmount - outputAmount;
var size = inputCount * mixer.InputSize + outputCount * mixer.OutputSize;
var feeRate = (fee / size).ToSats();

Console.WriteLine();

foreach (var (value, count, unique) in inputGroups
    .GetIndistinguishable()
    .OrderBy(x => x.value))
{
    if (count == 1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    var displayResult = count.ToString();
    if (count != unique)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        displayResult = $"{unique}/{count} unique/total";
    }
    Console.WriteLine($"There are {displayResult} occurrences of\t{value} BTC input.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

Console.WriteLine();

foreach (var (value, count, unique) in outputGroups
    .GetIndistinguishable()
    .OrderBy(x => x.value))
{
    if (count == 1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    var displayResult = count.ToString();
    if (count != unique)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        displayResult = $"{unique}/{count} unique/total";
    }
    Console.WriteLine($"There are {displayResult} occurrences of\t{value} BTC output.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

Console.WriteLine();
Console.WriteLine($"Number of users:\t{userCount}");
Console.WriteLine($"Number of inputs:\t{inputCount}");
Console.WriteLine($"Number of outputs:\t{outputCount}");
Console.WriteLine($"Avg consolidation:\t{inputCount / userCount}");
Console.WriteLine($"Total in:\t\t{inputAmount} BTC");
Console.WriteLine($"Fee paid:\t\t{fee} BTC");
Console.WriteLine($"Size:\t\t\t{size} vbyte");
Console.WriteLine($"Fee rate:\t\t{feeRate} sats/vbyte");
Console.WriteLine($"Average anonset:\t{Analyzer.AverageAnonsetGain(inputGroups, outputGroups):0.##}");
Console.WriteLine($"Average input anonset:\t{Analyzer.AverageAnonsetGain(inputGroups):0.##}");
Console.WriteLine($"Average output anonset:\t{Analyzer.AverageAnonsetGain(outputGroups):0.##}");
Console.WriteLine($"Blockspace efficiency:\t{Analyzer.BlockspaceEfficiency(inputGroups, outputGroups, size):0.##}");

Console.ReadLine();
