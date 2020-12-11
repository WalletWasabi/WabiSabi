using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization.DescPow2
{
    internal class DescPow2Mixer : IMixer
    {
        public IOrderedEnumerable<ulong> Denominations { get; }

        public DescPow2Mixer(uint feeRate = 10, uint inputSize = 69, uint outputSize = 33, uint sanityFeeRate = 2, ulong sanityFee = 1024)
        {
            DustThreshold = new[] { sanityFee, feeRate * inputSize, sanityFeeRate * inputSize }.Max();
            FeeRate = feeRate;
            InputSize = inputSize;
            OutputSize = outputSize;

            var denominations = new HashSet<ulong>();
            for (int i = 50; i > 0; i--)
            {
                var denom = (ulong)Math.Pow(2, i);

                denominations.Add(denom);
            }

            Denominations = denominations.OrderByDescending(x => x);
        }

        public ulong DustThreshold { get; }
        public ulong InputFee => InputSize * FeeRate;
        public ulong OutputFee => OutputSize * FeeRate;

        public uint FeeRate { get; }
        public uint InputSize { get; }
        public uint OutputSize { get; }

        public IEnumerable<IEnumerable<ulong>> CompleteMix(IEnumerable<IEnumerable<ulong>> inputs)
        {
            var inputArray = inputs.ToArray();

            for (int i = 0; i < inputArray.Length; i++)
            {
                var currentUser = inputArray[i];
                var others = new List<ulong>();
                for (int j = 0; j < inputArray.Length; j++)
                {
                    if (i != j)
                    {
                        others.AddRange(inputArray[j]);
                    }
                }
                yield return Mix(currentUser, others).Select(x => x - OutputFee);
            }
        }

        public IEnumerable<ulong> Mix(IEnumerable<ulong> myInputsParam, IEnumerable<ulong> othersInputsParam)
        {
            var myInputs = myInputsParam.Select(x => x - InputFee).ToArray();
            var othersInputs = othersInputsParam.Select(x => x - InputFee).ToArray();
            var largestOthersInputs = othersInputs.Max();

            var remaining = myInputs.Sum();
            ulong dustThresholdPlusFee = DustThreshold + OutputFee;

            foreach (var denomPlusFee in Denominations.Select(x => x + OutputFee))
            {
                if (denomPlusFee > largestOthersInputs)
                {
                    continue;
                }

                if (denomPlusFee < dustThresholdPlusFee || remaining < dustThresholdPlusFee)
                {
                    break;
                }

                if (denomPlusFee <= remaining)
                {
                    yield return denomPlusFee;
                    remaining -= denomPlusFee;
                }
            }

            if (remaining >= dustThresholdPlusFee)
            {
                yield return remaining;
            }
            else if (remaining == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"EUREKA! Perfect mix!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARNING! Dust happened: {remaining} sats");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
