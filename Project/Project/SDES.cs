using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SDES
    {

        public string log = "";
        string P10 = "3 5 2 7 4 10 1 9 8 6";
        string P8 = "6 3 7 4 8 5 10 9";
        string IP = "2 6 3 1 4 8 5 7";
        string EP = "4 1 2 3 2 3 4 1";
        string P4 = "2 4 3 1";
        int[,] S0 = { { 1, 0, 3, 2 }, { 3, 2, 1, 0 }, { 0, 2, 1, 3 }, { 3, 1, 3, 2 } };
        int[,] S1 = { { 0,1, 2, 3 }, { 2, 0, 1, 3 }, { 3, 0, 1, 0 }, { 2, 1, 0, 3 } };


        string Pof8(List<int> p, string key)
        {
            string res = "";
         
            for (int i = 0; i < p.Count; i++)
            {
                res += key[p[i] - 1];
            }
            return res;
        }
        string Pof(List<int> p , string key )
        {
            string res = "";
            int min = 999999;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i] < min)
                {
                    min = p[i];
                }
            }
          
            for(int i = 0; i < p.Count; i++)
            {
                res += key[p[i] - min];
            }
            return res;
        } 
        string CyclicShift(string bits , int shift )
        {
            string temp = "";
            for (int m = 0; m < shift; m++)
            {
                temp = "";
                char firstbit = bits[0];
                for (int i = 1; i < bits.Length ; i++)
                {
                    temp += bits[i];
                }
                temp += firstbit;
                bits = temp;
            }
            return bits;
        }
        List<int> generateP(string p )
        {
            List<int> pi = new List<int>();
            List<string> ps = p.Split(' ').ToList();
            for(int i =0; i < ps.Count;i++)
            {
                pi.Add(Convert.ToInt32(ps[i]));
            }
            return pi;
        }
        void Split(string bits , out string b1 , out string b2)
        {
            b1 = "";
            b2 = "";
          for(int i = 0; i < bits.Length; i++ )
            {
                if(i < (bits.Length /2))
                {
                    b1 += bits[i];
                }
                else
                {
                    b2 += bits[i];
                }
            }            
        }
        string XOR(string p1, string p2)
        {
            int x1 = Convert.ToInt32(p1, 2);
            int x2 = Convert.ToInt32(p2, 2);
            int result = x1 ^ x2;
            string res = Convert.ToString(result, 2);
            if (p1.Length > res.Length || p2.Length > res.Length)
            {
                res = res.PadLeft(p1.Length , '0');
            }
            return res;
        }
        string SBox( string Rkey , int[,] sbox)
        {
            string outer = "";
                string inner = "";
            outer += Rkey[0];
                outer+=Rkey[3];
            inner += Rkey[1];
                inner +=Rkey[2];
            int row = Convert.ToInt32(outer, 2);
            log += " Row  : " + row + "\n";
            int col = Convert.ToInt32(inner, 2);
            log += " Col  : " + col + "\n";
            string bin =  Convert.ToString(sbox[row, col],2);
           
           if(bin.Length < 2)
            {
               bin = bin.PadLeft(2, '0');
            }
           else if( bin.Length > 2)
            {
                int index = bin.Length - 2;
                bin = bin.Substring(index);
            }
            log += "Binary : " + bin + "\n";
            return bin;
        }

        string Round1(string Iptxt,string k1)
        {
            List<int> ep = generateP(EP);
            List<int> p4 = generateP(P4);
            string sR, sL;
            Split(Iptxt, out sL, out sR);
            log += "Left :"+ sL +"\n";
            log += "Right :" + sR + "\n";
            string rResult = Pof(ep, sR);
            log += "EP(L+R) :" + rResult + "\n";
            string XORres = XOR(rResult, k1);
            log += "EP(L+R) XOR Key ( 1 for enc , 2 for dec ):" + XORres + "\n";
            string s2L, s2R;
            Split(XORres, out s2L, out s2R);
            log += "Result Left :" + s2L + "\n";
            log += "Result Right :" + s2R + "\n";
            log += "Left with SBox 0 \n";
            string p1 = SBox(s2L, S0);
            log += "Right with SBox 1 \n";
            string p2 = SBox(s2R, S1);
            string boxres = p1 + p2;
            log += "BoxResult :"+boxres+ "\n";
            string bxOrp4 = Pof(p4, boxres);
            log += "BoxResult OR P4 :" + bxOrp4 + "\n"; 
            
            string left = XOR(bxOrp4, sL);
            log += "Result XOR Left :" + left + "\n";
            log += "Result before Swap :" + left+sR + "\n";
            string Round1 = sR;
            Round1 += left;
            log += "Result After Swap :" + Round1 + "\n";
            return Round1;
        }
        string Round2(string r1 ,string k2)
        {
            List<int> ep = generateP(EP);
            List<int> p4 = generateP(P4);
            string sR, sL;
            Split(r1, out sL, out sR);
            log += "Left :" + sL + "\n";
            log += "Right :" + sR + "\n";
            string rResult = Pof(ep, sR);
            log += "EP(L+R) :" + rResult + "\n";
            string XORres = XOR(rResult, k2);
            log += "EP(L+R) XOR Key ( 2 for Enc , 1 for Dec ):" + XORres + "\n";
            string s2L, s2R;
            Split(XORres, out s2L, out s2R);
            log += "Result Left :" + s2L + "\n";
            log += "Result Right :" + s2R + "\n";
            log += "Left with SBox 0 \n";
            string p1 = SBox(s2L, S0);
            log += "Right with SBox 1 \n";
            string p2 = SBox(s2R, S1);
   
            string boxres = p1 + p2;
            log += "BoxResult :" + boxres + "\n";
            string bxOrp4 = Pof(p4, boxres);
            log += "BoxResult OR P4 :" + bxOrp4 + "\n";
            string left = XOR(bxOrp4, sL);
            string Round2 = left;
            Round2 += sR;
            log += "Result XOR Left :" + left + "\n";
            log += "Result :" +Round2 + "\n";
            return Round2;
        }

        public string Encrypt(string Key , string PlainText)
        {
            log = "";
            string k1, k2, Iptxt;
            List<int> ep = generateP(EP);
            List<int> p4 = generateP(P4);
            List<int> ip = generateP(IP);
            log += "Key Generation : \n";
            KeyGeneration(Key , out k1 , out k2);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Intial Permutation : \n";
            IntialPermutation(PlainText, IP , out Iptxt);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Round 1  : \n";
            string R1 = Round1(Iptxt,k1);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Round 2  : \n";
            string R2 = Round2(R1, k2);
           List<int> ipinv = IPinverse(ip);
          string Cipher = Pof(ipinv, R2);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "IP-1(R2): " + Cipher + "\n";
            log += "Cipher: " + Cipher + "\n";
            return Cipher;
        }
        public string Decrypt(string Key, string CipherText)
        {
            string k1, k2, Iptxt;
            List<int> ep = generateP(EP);
            List<int> p4 = generateP(P4);
            List<int> ip = generateP(IP);
            log += "Key Generation : \n";
            KeyGeneration(Key, out k1, out k2);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Intial Permutation : \n";
            IntialPermutation(CipherText, IP, out Iptxt);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Round 1  : \n";
            string R1 = Round1(Iptxt, k2);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "Round 2  : \n";
            string R2 = Round2(R1, k1);
            List<int> ipinv = IPinverse(ip);
          
            string PlainText = Pof(ipinv, R2);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += "IP-1(R2): "+PlainText + "\n";
            log += "PlainText: " + PlainText + "\n";
            return PlainText;
        }
        List<int> IPinverse(List<int> IP)
        {

            List<int> ipinverse = new List<int>();
            int index = 1;
            int ct = 0;
            while (ct < IP.Count)
            {
                int oldindx = index;
                for (int i = 0; i < IP.Count; i++)
                {

                    if (index == IP[i])
                    {
                        ipinverse.Add(i+1);
                        index++;
                        ct++;
                    }
                 
                }
                if(oldindx == index)
                {
                    index++;
                }
            }
            return ipinverse;
        }
        public void IntialPermutation(string PlainText , string IP , out string iptxt)
        {
            List<int> ip = generateP(IP);
            log += "IP :"+ IP+"\n";
            string res = Pof(ip, PlainText);
            log += "IP(PlainText) :"+res+" \n";
            iptxt = res;
         
        }
       public void KeyGeneration(string Key , out string k1 , out string k2 )
        {

          List<int> p10 =  generateP(P10);
            List<int> p8 = generateP(P8);
     
            string res =  Pof(p10, Key);
            log += "P10(Key) :"+  res +"\n";
            string p1, p2;
            Split(res, out p1, out p2);
            log += "L :" + p1 + "\n";
            log += "R :" + p2 + "\n";
            p1 = CyclicShift(p1, 1);
             p2 = CyclicShift(p2, 1);
            log += "Shift(L) :" + p1 + "\n";
            log += "Shift(R) :" + p2 + "\n";
            string pt = p1 + p2;
            string res2 = Pof8(p8, pt);
            log += "P8(L+R) :" + res2 + "\n";
            k1 = res2;
            log += "K1 :" + res2 + "\n";
            p1 = CyclicShift(p1, 2);
            p2 = CyclicShift(p2, 2);
            log += "Shift2(L) :" + p1 + "\n";
            log += "Shift2(R) :" + p2 + "\n";
            pt = p1 + p2;
           

            res2 = Pof8(p8, pt);
            log += "P8(L+R):" + pt+ "\n";
            k2 = res2;
            log += "K2 :" + pt + "\n";
        }
    }
}
