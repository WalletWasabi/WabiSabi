using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization.WWI
{
    public class WwiMixer : IMixer
    {
        public IEnumerable<ulong> Mix(IEnumerable<ulong> myInputs, IEnumerable<ulong> othersInputs)
        {
            var remaining = myInputs.Sum();
            for (int i = 0; i < 50; i++)
            {
                var denom = (ulong)Math.Pow(2, i);
                if (denom > remaining)
                {
                    yield return remaining;
                    break;
                }

                yield return denom;

                remaining -= denom;
            }
        }
    }
}
