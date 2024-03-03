using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmountOrganization
{
    public static class Sample
    {
        static Sample()
        {
            var path = "Sample.txt";
            Amounts = File.ReadAllLines(path).Select(x => decimal.Parse(x)).ToArray();
        }

        public static decimal[] Amounts { get; }
    }
}
