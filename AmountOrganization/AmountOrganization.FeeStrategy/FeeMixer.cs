using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmountOrganization.FeeStrategy
{
    public class FeeMixer : IMixer
    {
        public FeeMixer(uint feeRate = 10, uint inputSize = 69, uint outputSize = 33, uint sanityFeeRate = 2, ulong sanityFee = 1000)
        {
            DustThreshold = new[] { sanityFee, feeRate * inputSize, sanityFeeRate * inputSize }.Max();
            FeeRate = feeRate;
            InputSize = inputSize;
            OutputSize = outputSize;
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

            var remaining = myInputs.Sum();
            for (int i = 0; i < 50; i++)
            {
                var denomPlusFee = (ulong)Math.Pow(2, i) + OutputFee;
                ulong dustThresholdPlusFee = DustThreshold + OutputFee;

                if (denomPlusFee < dustThresholdPlusFee)
                {
                    continue;
                }

                if (denomPlusFee > remaining)
                {
                    if (remaining >= dustThresholdPlusFee)
                    {
                        yield return remaining;
                    }
                    else
                    {
                        Console.WriteLine($"WARNING! Dust happened: {remaining} sats");
                    }
                    break;
                }

                yield return denomPlusFee;

                remaining -= denomPlusFee;
            }
        }
    }
}
