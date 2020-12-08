using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization
{
    public interface IMixer
    {
        public IEnumerable<IEnumerable<decimal>> CompleteMix(IEnumerable<IEnumerable<decimal>> inputs)
            => CompleteMix(inputs.Select(x => x.Select(y => y.ToSats()))).Select(x => x.Select(y => y.ToBtc()));

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
                yield return Mix(currentUser, others);
            }
        }

        public IEnumerable<ulong> Mix(IEnumerable<ulong> myInputs, IEnumerable<ulong> othersInputs);
    }
}
