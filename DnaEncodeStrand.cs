using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticsChallenge
{
    public static class DnaEncodeStrand
    {
        public static string EncodeStrand(string strand)
        {
            var binaryStrand = "";
            foreach (char c in strand)
            {
                switch (c)
                {
                    case 'A':
                        binaryStrand += "00";
                        break;
                    case 'C':
                        binaryStrand += "01";
                        break;
                    case 'G':
                        binaryStrand += "10";
                        break;
                    case 'T':
                        binaryStrand += "11";
                        break;
                    default:
                        break;
                }
            }

            var byteArray = new byte[binaryStrand.Length / 8];
            for (int i = 0; i < binaryStrand.Length; i += 8)
            {
                byteArray[i / 8] = Convert.ToByte(binaryStrand.Substring(i, 8), 2);
            }

            var base64Strand = Convert.ToBase64String(byteArray);

            return base64Strand;
        }
    }
}
