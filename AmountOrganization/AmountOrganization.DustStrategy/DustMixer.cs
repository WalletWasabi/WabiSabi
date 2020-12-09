using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization.DustStrategy
{
    public class DustMixer : IMixer
    {
        public DustMixer(uint currentFeeRate = 10, uint inputSpendSize = 69, uint sanityFeeRate = 2, ulong sanityFee = 1000)
        {
            DustThreshold = new[] { sanityFee, currentFeeRate * inputSpendSize, sanityFeeRate * inputSpendSize }.Max();
        }

        public ulong DustThreshold { get; }

        public IEnumerable<ulong> Mix(IEnumerable<ulong> myInputs, IEnumerable<ulong> othersInputs)
        {
            var remaining = myInputs.Sum();
            for (int i = 0; i < 50; i++)
            {
                var denom = (ulong)Math.Pow(2, i);

                if (denom < DustThreshold)
                {
                    continue;
                }

                if (denom > remaining)
                {
                    if (remaining >= DustThreshold)
                    {
                        yield return remaining;
                    }
                    else
                    {
                        Console.WriteLine($"WARNING! Dust happened: {remaining} sats");
                    }
                    break;
                }

                yield return denom;

                remaining -= denom;
            }
        }
    }
}
