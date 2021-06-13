using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class PlayFair
    {
       
         string[,] Matrix = new string[5, 5];
         List<string> Alphabet = new List<string>();
         List<string> key = new List<string>();
        public string log = "";
         void FillAlpha()
        {
            char alpha = 'a';
            for(int i = 0; i < 26;i++)
            {
                Alphabet.Add(alpha.ToString());
                alpha++;
            }
        }

         bool Hasfound(string x)
        {
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if(Matrix[r,c] == x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

         Point GetLetter(string x)
        {
            Point pos = new Point(-1,-1);
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (Matrix[r, c] == x  || (Matrix[r,c] == "i" && x == "j") || (Matrix[r, c] == "j" && x == "i"))
                    {
                        pos.X = r;
                        pos.Y = c;
                    }
                }
            }
            return pos;
        }
         void ConstructMatrix(string Key)
        {
            int i = 0;
            int j = 0;
            for(int r = 0; r < 5;r++)
            {
                for (int c = 0; c < 5; c++)
                {


                    if (i < key.Count)
                    {
                        while (i < key.Count && (Hasfound(key[i].ToString()) || (key[i] == "i" && Hasfound("j")) || (key[i] == "j" && Hasfound("i"))))
                        {
                            key.RemoveAt(i);
                        }
                        if (i < key.Count)
                        {
                            Matrix[r, c] = key[i].ToString();
                            key.RemoveAt(i);
                            i--;
                        }
                        i++;
                    }
                    else
                    {
                        while (j < Alphabet.Count && (Hasfound(Alphabet[j].ToString()) || Alphabet[j] == "i" && Hasfound("j")) || (Alphabet[j] == "j" && Hasfound("i")))
                        {
                            Alphabet.RemoveAt(j);
                           
                        }
                        if (j < Alphabet.Count)
                        {
                            Matrix[r, c] = Alphabet[j].ToString();
                            Alphabet.RemoveAt(j);
                            Console.Write(Matrix[r, c] + ",");
                            j--;
                        }
                        j++;
                    }
                   
                
                        

                    
                }
            
            }
        
        }
        void FillKey(string Key)
        {
            for(int i = 0; i < Key.Length; i++)
            {
                key.Add(Key[i].ToString());
            }
        }
         List<string> decrpytText(List<string> Substrings)
        {
            List<string> Cipher = new List<string>();
            for (int i = 0; i < Substrings.Count; i++)
            {
                Point p1 = GetLetter(Substrings[i][0].ToString());
                Point p2 = GetLetter(Substrings[i][1].ToString());
                if (p1.X != p2.X && p1.Y != p2.Y)
                {
                    string x = Matrix[p1.X, p2.Y];
                    x += Matrix[p2.X, p1.Y];
                    Cipher.Add(x);
                }
                else if (p1.X == p2.X && p1.Y != p2.Y)
                {
                    int c1 = p1.Y - 1;
                    int c2 = p2.Y - 1;
                    if (c1 < 0)
                    {
                        c1 = 4;
                    }
                    if (c2 < 0)
                    {
                        c2 = 4;
                    }
                    string x = Matrix[p1.X, c1];
                    x += Matrix[p2.X, c2];
                    Cipher.Add(x);
                }
                else if (p1.Y == p2.Y && p1.X != p2.X)
                {
                    int r1 = p1.X - 1;
                    int r2 = p2.X - 1;
                    if (r1 < 0)
                    {
                        r1 = 4;
                    }
                    if (r2 < 0)
                    {
                        r2 = 4;
                    }
                    string x = Matrix[r1, p1.Y];
                    x += Matrix[r2, p2.Y];
                    Cipher.Add(x);
                }
            }
            return Cipher;
        }
         List<string> encrpytText(List<string> Substrings)
        {
           
            List<string> Cipher = new List<string>();
            for(int i = 0; i < Substrings.Count;i++)
            {
                Point p1 = GetLetter(Substrings[i][0].ToString());
                Point p2 = GetLetter(Substrings[i][1].ToString());
                if(p1.X != p2.X && p1.Y != p2.Y)
                {
                    string x = Matrix[p1.X, p2.Y];
                    x+= Matrix[p2.X, p1.Y];
                    Cipher.Add(x);
                }
                else if (p1.X == p2.X && p1.Y != p2.Y)
                {
                    int c1 = p1.Y + 1;
                    int c2 = p2.Y + 1;
                    if(c1 >= Matrix.GetLength(0))
                    {
                        c1 = 0;
                    }
                    if (c2 >= Matrix.GetLength(0))
                    {
                        c2 = 0;
                    }
                    string x = Matrix[p1.X, c1];
                    x += Matrix[p2.X, c2];
                    Cipher.Add(x);
                }
                else if (p1.Y == p2.Y && p1.X != p2.X)
                {
                    int r1 = p1.X + 1;
                    int r2 = p2.X + 1;
                    if (r1 >= Matrix.GetLength(0))
                    {
                        r1 = 0;
                    }
                    if (r2 >= Matrix.GetLength(0))
                    {
                        r2 = 0;
                    }
                    string x = Matrix[r1, p1.Y];
                    x += Matrix[r2,p2.Y];
                    Cipher.Add(x);
                }
            }
            return Cipher;
        }
      List<string> DivideText(string plainText)
        {
            List<string> substrings = new List<string>();
            for(int i = 0; i < plainText.Length; i+=2)
            {
               if(i+1 < plainText.Length)
                {
                    if(plainText[i] != plainText[i+1])
                    {
                        substrings.Add(plainText[i].ToString() + plainText[i + 1].ToString());
                    }
                    else
                    {
                        substrings.Add(plainText[i].ToString() +"x");
                        i--;
                    }
                }
               else if ( i+1 == plainText.Length)
                {
                    substrings.Add(plainText[i].ToString() + "x");
                }
            }
            return substrings;
        }
        public  string Encrypt(string Key , string PlainText)
        {
            log = "";
            Key = Key.ToLower();

            PlainText = PlainText.ToLower();
            log += "Key : " + Key + "\n";
            log += "Plain Text : " + PlainText + "\n";
            Key = Key.Replace(" ", String.Empty);
            PlainText = PlainText.Replace(" ", String.Empty);
            FillAlpha();
            FillKey(Key);
            ConstructMatrix(Key);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " Matrix : \n";
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    log += " " + Matrix[r, c] + " , ";
                }
                log += "\n";

            }
            List<string> div = DivideText(PlainText);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " divided PlainText : ";
            for (int i = 0; i < div.Count; i++)
            {
                log += " " + div[i].ToUpper() + " ";
            }
            log += "\n";

            List<string> cipher = encrpytText(div);
  
            string result = "";
            for (int i = 0; i < cipher.Count; i++)
            {
                result += cipher[i];
            }
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " Cipher Text : " + result + "\n";
            return result;
        }
        public string Decrypt(string Key, string CipherText)
        {
            log = "";
            Key = Key.ToLower();
        
            CipherText = CipherText.ToLower();
            log += "Key : " + Key + "\n";
            log += "Cipher Text : " + CipherText + "\n";
            Key = Key.Replace(" ", String.Empty);
            CipherText = CipherText.Replace(" ", String.Empty);
       
            FillAlpha();
            FillKey(Key);
            ConstructMatrix(Key);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " Matrix : \n";
            for (int r = 0; r < 5; r++)
            {
                for(int c = 0; c <  5; c++)
                {
                    if (Matrix[r, c] != "i" && Matrix[r, c] != "j")
                    {
                        log += " " + Matrix[r, c] + " , ";
                    }
                    else
                    {
                        log += " " + "(i/j)" + " , ";
                    }
                }
                log += "\n";

            }
            List<string> div = DivideText(CipherText);
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " divided Cipher : ";
            for (int i = 0; i < div.Count; i++)
            {
                log += " " + div[i].ToUpper() + " ";
            }
            log+= "\n";

            List<string> plain = decrpytText(div);
           
            string result = "";
            for(int i =0; i < plain.Count; i++)
            {
                if (plain[i][0] == 'i' || plain[i][0] == 'j')
                {
                    result += "(i/j)";
                    if (plain[i][1] == 'i' || plain[i][1] == 'j')
                    {
                        result += "(i/j)";
                    }
                    else
                    {
                        result += plain[i][1];
                    }
                }
                else
                {
                    if (plain[i][1] == 'i' || plain[i][1] == 'j')
                    {
                        result += plain[i][0];
                        result += "(i/j)";
                    }
                    else
                    {
                        result += plain[i];
                    }
                }
            }
            log += "\n xxxxxxxxxxxxxx \n \n";
            log += " Plain Text : " + result + "\n";
            return result;
           
        }
    }
}
