using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.XPath;

namespace Project
{
    class Cryptographer
    {
        private static Dictionary<UInt16, UInt16> subnibTable = null;
        private static UInt16[,] multiplicationMatrix = null;

        private static void initializeMultMatrix()
        {
            if (multiplicationMatrix == null)
            {
                multiplicationMatrix = new UInt16[,] { { 1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15 },
                                                   { 2,  4,  6,  8, 10, 12, 14,  3,  1,  7,  5, 11,  9, 15, 13 },
                                                   { 3,  6,  5, 12, 15, 10,  9, 11,  8, 13, 14,  7,  4,  1,  2 },
                                                   { 4,  8, 12,  3,  7, 11, 15,  6,  2, 14, 10,  5,  1, 13,  9 },
                                                   { 5, 10, 15,  7,  2, 13,  8, 10, 11,  4,  1,  9, 12,  3,  6 },
                                                   { 6, 12, 10, 11, 13,  7,  1,  5,  3,  9, 15, 14,  8,  2,  4 },
                                                   { 7, 14,  9, 15,  8,  1,  6, 13, 10,  3,  4,  2,  5, 12, 11 },
                                                   { 8,  3, 11,  6, 14,  5, 13, 12,  4, 15,  7, 10,  2,  9,  1 },
                                                   { 9,  1,  8,  2, 11,  3, 10,  4, 13,  5, 12,  6, 15,  7, 14 },
                                                   { 10, 7, 13, 14,  4,  9,  3, 15,  5,  8,  2,  1, 11,  6, 12 },
                                                   { 11, 5, 14, 10,  1, 15,  4,  7, 12,  2,  9, 13,  6,  8,  3 },
                                                   { 12, 11, 7,  5,  9, 14,  2, 10,  6,  1, 13, 15,  3,  4,  8 },
                                                   { 13,  9, 4,  1, 12,  8,  5,  2, 15, 11,  6,  3, 14, 10,  7 },
                                                   { 14, 15, 1, 13,  3,  2, 12,  9,  7,  6,  8,  4, 10, 11,  5 },
                                                   { 15, 13, 2,  9,  6,  4, 11,  1, 14, 12,  3,  8,  7,  5, 10 }};
            }
        }

        private static void initializeSubNibTable()
        {
            if (subnibTable == null)
            {
                subnibTable = new Dictionary<UInt16, UInt16>();

                subnibTable.Add(0b_0000, 0b_1001);
                subnibTable.Add(0b_0001, 0b_0100);
                subnibTable.Add(0b_0010, 0b_1010);
                subnibTable.Add(0b_0011, 0b_1011);
                subnibTable.Add(0b_0100, 0b_1101);
                subnibTable.Add(0b_0101, 0b_0001);
                subnibTable.Add(0b_0110, 0b_1000);
                subnibTable.Add(0b_0111, 0b_0101);

                subnibTable.Add(0b_1000, 0b_0110);
                subnibTable.Add(0b_1001, 0b_0010);
                subnibTable.Add(0b_1010, 0b_0000);
                subnibTable.Add(0b_1011, 0b_0011);
                subnibTable.Add(0b_1100, 0b_1100);
                subnibTable.Add(0b_1101, 0b_1110);
                subnibTable.Add(0b_1110, 0b_1111);
                subnibTable.Add(0b_1111, 0b_0111);
            }
        }

        public static string rsa(string pVal, string qVal, string e, string mVal, string cVal, bool isEncrypting, bool showSteps, StepsForm form)
        {
            List<int> outputs = new List<int>();
            int p = Convert.ToInt32(pVal);
            int q = Convert.ToInt32(qVal);
            int m = 0;
            int c = 0;

            if (isEncrypting)
            {
                m = Convert.ToInt32(mVal);
            }
            else
            {
                c = Convert.ToInt32(cVal);
            }

            double n = p * q;
            outputs.Add((int)n);
            int phiN = (p - 1) * (q - 1);
            outputs.Add(phiN);
            int encryptionKey;

            if (String.IsNullOrEmpty(e))
            {
                encryptionKey = generateEncryptionKey(phiN);
            }
            else
            {
                encryptionKey = Convert.ToInt32(e);
            }

            outputs.Add(encryptionKey);
            double decryptionKey = getDecryptionKey(encryptionKey, phiN);
            outputs.Add((int)decryptionKey);

            if (isEncrypting)
            {
                c = (int)(powerMod(m, encryptionKey, n));
            }
            else
            {
                m = (int)(powerMod(c, decryptionKey, n));
            }

            outputs.Add(c);
            outputs.Add(m);
            addRSASteps(outputs, isEncrypting, showSteps, form);

            return isEncrypting ? Convert.ToString(c) : Convert.ToString(m);
        }

        private static double powerMod(double val, double power, double mod)
        {
            double res = 1;

            for (int i = 1; i <= power; i++)
            {
                res *= (val % mod);
                res %= mod;
            }

            return res % mod;
        }

        public static string[] saes(string[] plaintext, string[] key, int stringBase, bool showSteps, StepsForm form)
        {
            List<UInt16[]> outputs = new List<ushort[]>();
            UInt16[,] keys = new UInt16[3, 4];
            UInt16[] hexaPlaintext = BitOperations.stringToHex(plaintext, stringBase);
            initializeSubNibTable();
            initializeMultMatrix();

            keys = keyGeneration(BitOperations.stringToHex(key, stringBase));

            outputs.Add(new UInt16[] { keys[0, 0], keys[0, 1], keys[0, 2], keys[0, 3] });
            outputs.Add(new UInt16[] { keys[1, 0], keys[1, 1], keys[1, 2], keys[1, 3] });
            outputs.Add(new UInt16[] { keys[2, 0], keys[2, 1], keys[2, 2], keys[2, 3] });

            UInt16[] roundKey1 = substituteNibs(BitOperations.xor(keys, 0, hexaPlaintext));
            outputs.Add((UInt16[])roundKey1.Clone());
            BitOperations.swap(ref roundKey1[1], ref roundKey1[3]);
            outputs.Add((UInt16[])roundKey1.Clone());
            roundKey1 = mixColumns(roundKey1);
            outputs.Add((UInt16[])roundKey1.Clone());

            UInt16[] roundKey2 = substituteNibs(BitOperations.xor(keys, 1, roundKey1));
            outputs.Add((UInt16[])roundKey2.Clone());
            BitOperations.swap(ref roundKey2[1], ref roundKey2[3]);
            outputs.Add((UInt16[])roundKey2.Clone());

            UInt16[] ciphertext = BitOperations.xor(keys, 2, roundKey2);
            outputs.Add((UInt16[])ciphertext.Clone());

            addSAESSteps(outputs, stringBase, showSteps, form);

            return BitOperations.hexToStrings(ciphertext, stringBase);
        }

        private static double getDecryptionKey(int e, int phiN)
        {
            for (int i = 0; i <= phiN; i++)
            {
                int ed = e * i;

                if (ed % phiN == 1)
                {
                    return i;
                }
            }

            return 0;
        }

        private static int generateEncryptionKey(int phiN)
        {
            Random random = new Random();
            int e;

            while (true)
            {
                e = random.Next(2, phiN);

                if (getGCD(Math.Max(e, phiN), Math.Min(e, phiN)) == 1)
                {
                    break;
                }
            }

            return e;
        }

        private static int getGCD(int a, int b)
        {
            while (b > 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        private static UInt16[] mixColumns(UInt16[] key)
        {
            UInt16[] result = new UInt16[key.Length];

            result[0] = BitOperations.xor(key[0], multiplicationMatrix[key[1] > 0 ? key[1] - 1 : 0, 3]);
            result[1] = BitOperations.xor(key[1], multiplicationMatrix[key[0] > 0 ? key[0] - 1 : 0, 3]);
            result[2] = BitOperations.xor(key[2], multiplicationMatrix[key[3] > 0 ? key[3] - 1 : 0, 3]);
            result[3] = BitOperations.xor(key[3], multiplicationMatrix[key[2] > 0 ? key[2] - 1 : 0, 3]);

            return result;
        }

        private static UInt16[] substituteNibs(UInt16[] nibs)
        {
            UInt16[] newNibs = new UInt16[nibs.Length];

            for (int i = 0; i < nibs.Length; i++)
            {
                newNibs[i] = subnibTable[nibs[i]];
            }

            return newNibs;
        }

        private static UInt16[,] keyGeneration(UInt16[] key)
        {
            UInt16[,] words = new UInt16[6, 2];

            //w0 w1
            words[0, 0] = key[0];
            words[0, 1] = key[1];
            words[1, 0] = key[2];
            words[1, 1] = key[3];

            //w2
            BitOperations.xor(ref words, 2, 0, new UInt16[] { 0b_1000, 0b_0000 });
            UInt16[] w1 = new UInt16[] { words[1, 0], words[1, 1] };
            BitOperations.swap(ref w1[0], ref w1[1]);
            w1 = substituteNibs(w1);
            BitOperations.xor(ref words, 2, w1);

            //w3
            BitOperations.xor(ref words, 3, 2, 1);

            //w4
            BitOperations.xor(ref words, 4, 2, new UInt16[] { 0b_0011, 0b_0000 });
            UInt16[] w3 = new UInt16[] { words[3, 0], words[3, 1] };
            BitOperations.swap(ref w3[0], ref w3[1]);
            w3 = substituteNibs(w3);
            BitOperations.xor(ref words, 4, w3);

            //w5
            BitOperations.xor(ref words, 5, 4, 3);

            return new UInt16[,] { { words[0, 0], words[0, 1], words[1, 0], words[1, 1] },
                                   { words[2, 0], words[2, 1], words[3, 0], words[3, 1] },
                                   { words[4, 0], words[4, 1], words[5, 0], words[5, 1] }};
        }

        private static void addSAESSteps(List<UInt16[]> outputs, int stringBase, bool showSteps, StepsForm form) //0-1-2 keys, 3 R1 xor&sub, 4 R1 Shift, 5 R1 mix, 6 R2 xor&sub, 7 R2 shift, 8 cipher
        {
            if (showSteps)
            {
                addStep("1. Key Generation:" + "\r\n", true, showSteps, form);
                string output = BitOperations.hexToString(outputs[0], stringBase);
                addStep(" K0 = " + output + "\r\n", false, showSteps, form);

                output = BitOperations.hexToString(outputs[1], stringBase);
                addStep(" K1 = " + output + "\r\n", false, showSteps, form);

                output = BitOperations.hexToString(outputs[2], stringBase);
                addStep(" K2 = " + output + "\r\n", false, showSteps, form);

                addStep("\r\n" + "xxxxxxxxxxxxxxxxxxx" + "\r\n" + "\r\n", true, showSteps, form);
                addStep("2. Encryption:" + "\r\n", true, showSteps, form);
                addStep(" 2.1. Round 1:" + "\r\n", true, showSteps, form);

                output = BitOperations.hexToString(outputs[3], stringBase);
                addStep("   XOR & SubNib = " + output + "\r\n", false, showSteps, form);

                output = BitOperations.hexToString(outputs[4], stringBase);
                addStep("   Shifting = " + output + "\r\n", false, showSteps, form);

                output = BitOperations.hexToString(outputs[5], stringBase);
                addStep("   Mix Columns = " + output + "\r\n", false, showSteps, form);

                addStep("\r\n" + " 2.2. Round 2:" + "\r\n", true, showSteps, form);

                output = BitOperations.hexToString(outputs[6], stringBase);
                addStep("   XOR & SubNib = " + output + "\r\n", false, showSteps, form);

                output = BitOperations.hexToString(outputs[7], stringBase);
                addStep("   Shifting & SubNib = " + output + "\r\n", false, showSteps, form);

                addStep("\r\n" + "xxxxxxxxxxxxxxxxxxx" + "\r\n" + "\r\n", true, showSteps, form);
                addStep("3. CipherKey:" + "\r\n", true, showSteps, form);

                output = BitOperations.hexToString(outputs[8], stringBase);
                addStep("   " + output, false, showSteps, form);
            }
        }

        private static void addRSASteps(List<int> outputs, bool isEncrypting, bool showSteps, StepsForm form) //0 n, 1 phiN, 2 e, 3 d, 4 c, 5 m
        {
            if (showSteps)
            {
                string publicKey = "(";
                string privateKey = "(";
                string n;

                addStep("1. System Modulus:" + "\r\n", true, showSteps, form);
                string output = Convert.ToString(outputs[0]);
                addStep(" n = " + output + "\r\n", false, showSteps, form);
                n = output;

                addStep("2. Eular Function:" + "\r\n", true, showSteps, form);
                output = Convert.ToString(outputs[1]);
                addStep(" phi(N) = " + output + "\r\n", false, showSteps, form);

                addStep("3. Encryption Key:" + "\r\n", true, showSteps, form);
                output = Convert.ToString(outputs[2]);
                addStep(" e = " + output + "\r\n", false, showSteps, form);
                publicKey += output + ", " + n + ")";

                addStep("4. Decryption Key:" + "\r\n", true, showSteps, form);
                output = Convert.ToString(outputs[3]);
                addStep(" d = " + output + "\r\n", false, showSteps, form);
                privateKey += output + ", " + n + ")";

                addStep("\r\n" + "xxxxxxxxxxxxxxxxxxx" + "\r\n" + "\r\n", true, showSteps, form);

                addStep("5. Keys:" + "\r\n", true, showSteps, form);
                addStep("   Public Key = " + publicKey + "\r\n", false, showSteps, form);
                addStep("   Private Key = " + privateKey + "\r\n", false, showSteps, form);

                addStep("\r\n" + "xxxxxxxxxxxxxxxxxxx" + "\r\n" + "\r\n", true, showSteps, form);

                if (isEncrypting)
                {
                    addStep("6. Encryption:" + "\r\n", true, showSteps, form);
                    output = Convert.ToString(outputs[4]);
                    addStep(" C = " + output + "\r\n", false, showSteps, form);
                }
                else
                {
                    addStep("6. Decryption:" + "\r\n", true, showSteps, form);
                    output = Convert.ToString(outputs[5]);
                    addStep(" M = " + output + "\r\n", false, showSteps, form);
                }
            }
        }

        public static void addStep(string text, bool isHeader, bool showSteps, StepsForm form)
        {
            if (showSteps)
            {
                form.addTextToBox(text, isHeader);
            }
        }
    }
}
