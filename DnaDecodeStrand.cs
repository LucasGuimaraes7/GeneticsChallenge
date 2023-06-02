using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticsChallenge
{
    public static class DnaDecodeStrand
    {
        public static string DecodeStrand(string encodedStrand)
        {
            byte[] byteArray = Convert.FromBase64String(encodedStrand);
            string binaryStrand = string.Join("", byteArray.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            int strandLength = binaryStrand.Length / 2 * 2;
            binaryStrand = binaryStrand.Substring(0, strandLength);

            string dnaStrand = "";
            for (int i = 0; i < strandLength; i += 2)
            {
                string nucleotide = binaryStrand.Substring(i, 2);
                switch (nucleotide)
                {
                    case "00":
                        dnaStrand += "A";
                        break;
                    case "01":
                        dnaStrand += "C";
                        break;
                    case "10":
                        dnaStrand += "G";
                        break;
                    case "11":
                        dnaStrand += "T";
                        break;
                }
            }

            return dnaStrand;
        }
    }
}
