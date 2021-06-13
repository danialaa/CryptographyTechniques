using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class BitOperations
    {
        public static void swap(ref UInt16 a, ref UInt16 b)
        {
            UInt16 temp = a;
            a = b;
            b = temp;
        }

        public static UInt16 xor(UInt16 a, UInt16 b)
        {
            return (UInt16)(a ^ b);
        }

        public static UInt16 leftShift(UInt16 x, UInt16 amount)
        {
            UInt16 res = (UInt16)((x << amount) | (x >> 4 - amount));
            return (UInt16)(res & 0x_F);
        }

        public static UInt16 rightShift(UInt16 x, UInt16 amount)
        {
            UInt16 res = (UInt16)((x >> amount) | (x << 4 - amount));
            return (UInt16)(res & 0x_F);
        }

        public static void xor(ref UInt16[,] list, int destination, int r, UInt16[] vals)
        {
            if (destination <= list.GetLength(0) && r <= list.GetLength(0))
            {
                for (int c = 0; c < list.GetLength(1); c++)
                {
                    list[destination, c] = xor(list[r, c], vals[c]);
                }
            }
        }

        public static void xor(ref UInt16[,] list, int destination, int r1, int r2)
        {
            if (destination <= list.GetLength(0) && r1 <= list.GetLength(0) && r2 <= list.GetLength(0))
            {
                for (int c = 0; c < list.GetLength(1); c++)
                {
                    list[destination, c] = xor(list[r1, c], list[r2, c]);
                }
            }
        }

        public static void xor(ref UInt16[,] list, int r, UInt16[] vals)
        {
            if (r <= list.GetLength(0))
            {
                for (int c = 0; c < list.GetLength(1); c++)
                {
                    list[r, c] = xor(list[r, c], vals[c]);
                }
            }
        }

        public static UInt16[] xor(UInt16[,] list, int r, UInt16[] vals)
        {
            UInt16[] result = new UInt16[list.GetLength(1) > vals.Length ? list.GetLength(1) : vals.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (i < list.GetLength(1) && i < vals.Length)
                {
                    result[i] = xor(list[r, i], vals[i]);
                }
                else if (i < list.GetLength(1))
                {
                    result[i] = list[r, i];
                }
                else
                {
                    result[i] = vals[i];
                }
            }

            return result;
        }

        public static UInt16[] stringToHex(string[] strings, int stringBase)
        {
            UInt16[] hexas = new UInt16[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    hexas[i] = Convert.ToUInt16(strings[i], stringBase);
                }
                catch
                {
                    hexas[i] = 0;
                }
            }

            return hexas;
        }

        public static string hexToString(UInt16 hexa, int stringBase)
        {
            int size = 0;

            if (stringBase == 2)
            {
                size = 4;
            }
            else if (stringBase == 16)
            {
                size = 1;
            }

            string text = Convert.ToString(hexa, stringBase).PadLeft(size, '0').ToUpper();

            return text;
        }

        public static string[] hexToStrings(UInt16[] hexas, int stringBase)
        {
            string[] strings = new string[hexas.Length];

            for (int i = 0; i < hexas.Length; i++)
            {
                strings[i] = hexToString(hexas[i], stringBase);
            }

            return strings;
        }

        public static string hexToString(UInt16[] hexas, int stringBase)
        {
            string text = "";

            for (int i = 0; i < hexas.Length; i++)
            {
                text += hexToString(hexas[i], stringBase) + " ";
            }

            return text;
        }
    }
}
