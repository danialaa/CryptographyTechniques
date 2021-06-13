using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class RC4
    {
        public string log = "";
        void print(int[] step, int k)
        {
       
            log += ("S(" + k + ") : "  );

            for (int i = 0; i < step.Length; i++)
            {
                log +=  step[i] + " ";
            }
            log += ("\n");
        }
        void print(int[] step)
        {

            for (int i = 0; i < step.Length; i++)
            {
                log+=(step[i] + " ");
            }
            log += ("\n");
        }
        int[] Step1(int[] T)
        {



            int[] S = new int[T.Length];
            int j = 0;
            for (int i = 0; i < T.Length; i++)
            {
                S[i] = i;
            }
            for (int i = 0; i < T.Length; i++)
            {
                j = (j + S[i] + T[i]) % 8;
              
                int temp = S[i];
                S[i] = S[j];
                S[j] = temp;
                print(S, i);
                log += (" J : " + j + "\n");
            }
            return S;
        }

        string XOR(string b1, string b2)
        {
            string result = "";
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] == b2[i])
                {
                    result += "0";
                }
                else
                {
                    result += "1";
                }
            }
            return result;
        }
        string GetIntBinaryString(int n)
        {
            char[] b = new char[3];
            int pos = 2;
            int i = 0;

            while (i < 3)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
        string[] Step2(int[] S, int[] Pt)
        {
            log += "\n";
            string[] cipher = new string[Pt.Length];
            int j = 0;
            int t = 0;
            int k = 0;
            int i = 0;
            for (int m = 0; m < Pt.Length; m++)
            {
                i = (i + 1) % 8;
                log+=(" i : " + i + "\n");
                j = (j + S[i]) % 8;
                log+=(" j : " + j + "\n");
                int temp = S[i];
                S[i] = S[j];
                S[j] = temp;
                print(S,(m));
                t = (S[i] + S[j]) % 8;
                log+=(" T : " + t + "\n");
                k = S[t];
                log+=(" K : " + k + "\n");
                string binary = GetIntBinaryString(k);
                log+=("Binary K :" + binary + "\n");
                string binary2 = GetIntBinaryString(Pt[m]);
                log+=("Binary Pt[" + (m + 1) + "] : " + binary2 + "\n");
                int res = k ^ Pt[m];
                cipher[m] = Convert.ToString(res) ;
                log+=("Cipher bit : " + GetIntBinaryString(res) + " , " + res +"\n");

            }
            log+=("\n");
            return cipher;
        }
        public string Encrypt(string[] pt, string[] key)
        {
            int[] Pt = new int[pt.Length];
            int[] Key = new int[key.Length];
            for(int i = 0; i <  pt.Length;i++)
            {
                Pt[i] = Convert.ToInt32(pt[i]);
            }
            for (int i = 0; i < key.Length; i++)
            {
               Key[i] = Convert.ToInt32(key[i]);
            }
            log = "";
            int[] T = new int[Pt.Length * 2];
            int e = 0;
            for (int f = 0; f < T.Length; f++)
            {
                if (e < Key.Length)
                {
                    T[f] = Key[e];
                }
                else
                {
                    f--;
                    e = -1;
                }


                e++;
            }
            print(T);
            log+=("\n Step 1 : \n");
            int[] S = Step1(T);
            print(S);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log +=("\n Step 2 : \n");

            string[] cipher = Step2(S, Pt);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += ("\n Cipher : ");
            string finalResult = "";
            for (int c = 0; c < 4; c++)
            {
                log+=(cipher[c]);
                finalResult += cipher[c];
            }
            return finalResult;
        }

    }
}


