using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization
{
    public static class Analyzer
    {
        public static decimal AverageAnonsetGain(IEnumerable<IEnumerable<decimal>> inputs, IEnumerable<IEnumerable<decimal>> outputs)
            => AverageAnonsetGain(inputs.Select(x => x.Select(y => y.ToSats())), outputs.Select(x => x.Select(y => y.ToSats())));

        public static decimal AverageAnonsetGain(IEnumerable<IEnumerable<ulong>> inputs, IEnumerable<IEnumerable<ulong>> outputs)
        {
            var inputSum = inputs.SelectMany(x => x).Sum();
            var outputSum = inputs.SelectMany(x => x).Sum();
            var ratio = (decimal)inputSum / outputSum;
            return (AverageAnonsetGain(inputs) * ratio + AverageAnonsetGain(outputs)) / 2;
        }

        public static decimal AverageAnonsetGain(IEnumerable<IEnumerable<decimal>> valueGroups)
            => AverageAnonsetGain(valueGroups.Select(x => x.Select(y => y.ToSats())));

        public static decimal AverageAnonsetGain(IEnumerable<IEnumerable<ulong>> valueGroups)
        {
            var indistinguishables = valueGroups
                .SelectMany(x => x)
                .GetIndistinguishable(true);

            var anonsetWeighted = 0ul;

            foreach (var (value, count) in valueGroups
                .Select(x => x.GetIndistinguishable(true))
                .SelectMany(x => x))
            {
                // For example if 5 BTC outputs appears 10 times,
                // but only 9 of them are from unique users,
                // then 8 users gained anonymity set of 10,
                // and one user gained anonymity set of 9 for 2 coins.
                // So this should result in 5BTC * 8 coins * 10 anonset + 5BTC * 2coins * 9anonset = 490

                var total = indistinguishables.First(x => x.value == value).count;
                int redundant = count - 1;
                var anonset = total - redundant;

                anonsetWeighted += value * (ulong)anonset * (ulong)count;
            }

            ulong sum = valueGroups.SelectMany(x => x).Sum();
            return anonsetWeighted / (decimal)sum;
        }

        public static decimal AverageAnonsetGain(IEnumerable<decimal> values)
            => AverageAnonsetGain(values.Select(x => x.ToSats()));

        /// <summary>
        /// Calculates how much anonymity set an average amount gained in the coinjoin.
        /// </summary>
        /// <param name="txo"></param>
        public static decimal AverageAnonsetGain(IEnumerable<ulong> values)
        {
            var anonsetWeighted = 0ul;

            foreach (var (value, count) in values.GetIndistinguishable(true))
            {
                // For example if 5 BTC outputs appears 10 times, then this results in: 10 * 10 * 5,
                // because 10 people gained 10 anonymity set for 5 BTC.
                anonsetWeighted += value * (ulong)count * (ulong)count;
            }

            ulong sum = values.Sum();
            return anonsetWeighted / (decimal)sum;
        }
    }
}
