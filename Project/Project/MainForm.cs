using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class MainForm : Form
    {
        StepsForm stepsForm = null;
        bool isSAESHexa = false, isRSAEncrypting = true;
        char[] binaryCharacters = new char[] { '0', '1' };
        char[] decimalCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char[] hexaCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'A', 'b', 'B', 'c', 'C', 'd', 'D', 'e', 'E', 'f', 'F' };
        string[] saesOutput = null;
        string rsaOutput = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void saesButton_Click(object sender, EventArgs e)
        {
            saesGroupBox.Visible = true;
            mainGroupBox.Visible = false;
            backButton.Visible = true;
        }

        private void rsaButton_Click(object sender, EventArgs e)
        {
            rsaGroupBox.Visible = true;
            mainGroupBox.Visible = false;
            backButton.Visible = true;
        }

        private void playfairButton_Click(object sender, EventArgs e)
        {
            pfGroupBox.Visible = true;
            mainGroupBox.Visible = false;
            backButton.Visible = true;
        }

        private void rc4Button_Click(object sender, EventArgs e)
        {
            rc4GroupBox.Visible = true;
            mainGroupBox.Visible = false;
            backButton.Visible = true;
        }

        private void sdesButton_Click(object sender, EventArgs e)
        {
            sdesGroupBox.Visible = true;
            mainGroupBox.Visible = false;
            backButton.Visible = true;
        }

        private void saesCipherButton_Click(object sender, EventArgs e)
        {
            char[] saesAllowed = isSAESHexa ? hexaCharacters : binaryCharacters;
            string[] plaintext = new string[] { saesPlainTextBox1.Text, saesPlainTextBox2.Text, saesPlainTextBox3.Text, saesPlainTextBox4.Text };
            string[] key = new string[] { saesKeyTextBox1.Text, saesKeyTextBox2.Text, saesKeyTextBox3.Text, saesKeyTextBox4.Text };
            int baseString = isSAESHexa ? 16 : 2;
            int maxSize = isSAESHexa ? 1 : 4;
            int minSize = isSAESHexa ? 1 : 4;

            if (isStringArrayValid(plaintext, saesAllowed, minSize, maxSize) && isStringArrayValid(key, saesAllowed, minSize, maxSize))
            {
                saesErrorLabel.Visible = false;
                saesCipherLabel1.Visible = saesCipherLabel2.Visible = saesCipherLabel3.Visible = saesCipherLabel4.Visible = true;
                saesCiperLabel.Visible = true;

                if (saesStepsCheckBox.Checked)
                {
                    if (stepsForm != null)
                    {
                        stepsForm.Close();
                    }

                    stepsForm = new StepsForm();
                    stepsForm.Show();
                }

                string[] ciphertext = Cryptographer.saes(plaintext, key, baseString, saesStepsCheckBox.Checked, stepsForm);

                saesOutput = new string[4];

                saesOutput[0] = saesCipherLabel1.Text = ciphertext[0].ToUpper();
                saesOutput[1] = saesCipherLabel2.Text = ciphertext[1].ToUpper();
                saesOutput[2] = saesCipherLabel3.Text = ciphertext[2].ToUpper();
                saesOutput[3] = saesCipherLabel4.Text = ciphertext[3].ToUpper();
            }
            else
            {
                saesOutput = null;
                saesErrorLabel.Visible = true;
                saesCipherLabel1.Visible = saesCipherLabel2.Visible = saesCipherLabel3.Visible = saesCipherLabel4.Visible = false;
                saesCiperLabel.Visible = false;
            }
        }

        private void rsaComputeButton_Click(object sender, EventArgs e)
        {
            int mMin = isRSAEncrypting ? 1 : 0;
            int cMin = isRSAEncrypting ? 0 : 1;

            if (isStringValid(rsaPTextBox.Text, decimalCharacters, 1, 5) && isStringValid(rsaQTextBox.Text, decimalCharacters, 0, rsaQTextBox.Text.Length)
             && isStringValid(rsaETextBox.Text, decimalCharacters, 0, 5) && isStringValid(rsaMTextBox.Text, decimalCharacters, mMin, rsaMTextBox.Text.Length)
             && isStringValid(rsaCTextBox.Text, decimalCharacters, cMin, 5)
             && isPrime(Convert.ToInt32(rsaPTextBox.Text)) && isPrime(Convert.ToInt32(rsaQTextBox.Text)))
            {
                rsaErrorLabel.Visible = false;
                rsaOutputLabel.Visible = rsaOutValLabel.Visible = true;

                if (rsaStepsCheckBox.Checked)
                {
                    if (stepsForm != null)
                    {
                        stepsForm.Close();
                    }

                    stepsForm = new StepsForm();
                    stepsForm.Show();
                }

                string output = Cryptographer.rsa(rsaPTextBox.Text, rsaQTextBox.Text, rsaETextBox.Text, rsaMTextBox.Text, rsaCTextBox.Text, isRSAEncrypting, rsaStepsCheckBox.Checked, stepsForm);

                rsaOutValLabel.Text = rsaOutput = output;
            }
            else
            {
                rsaOutput = null;
                rsaErrorLabel.Visible = true;
                rsaOutputLabel.Visible = rsaOutValLabel.Visible = false;
            }
        }

        private void saesBinaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            saesHexaRadioButton.Checked = !saesBinaryRadioButton.Checked;
            isSAESHexa = saesHexaRadioButton.Checked;
            saesErrorLabel.Visible = false;
            saesPlainTextBox1.Text = saesPlainTextBox2.Text = saesPlainTextBox3.Text = saesPlainTextBox4.Text = "";
            saesKeyTextBox1.Text = saesKeyTextBox2.Text = saesKeyTextBox3.Text = saesKeyTextBox4.Text = "";

            if (saesOutput != null && !isSAESHexa)
            {
                saesOutput[0] = saesCipherLabel1.Text = Convert.ToString(Convert.ToUInt16(saesOutput[0], 16), 2).PadLeft(4, '0');
                saesOutput[1] = saesCipherLabel2.Text = Convert.ToString(Convert.ToUInt16(saesOutput[1], 16), 2).PadLeft(4, '0');
                saesOutput[2] = saesCipherLabel3.Text = Convert.ToString(Convert.ToUInt16(saesOutput[2], 16), 2).PadLeft(4, '0');
                saesOutput[3] = saesCipherLabel4.Text = Convert.ToString(Convert.ToUInt16(saesOutput[3], 16), 2).PadLeft(4, '0');
            }
        }

        private void saesHexaRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            saesBinaryRadioButton.Checked = !saesHexaRadioButton.Checked;
            isSAESHexa = saesHexaRadioButton.Checked;
            saesErrorLabel.Visible = false;
            saesPlainTextBox1.Text = saesPlainTextBox2.Text = saesPlainTextBox3.Text = saesPlainTextBox4.Text = "";
            saesKeyTextBox1.Text = saesKeyTextBox2.Text = saesKeyTextBox3.Text = saesKeyTextBox4.Text = "";

            if (saesOutput != null && isSAESHexa)
            {
                saesOutput[0] = saesCipherLabel1.Text = Convert.ToString(Convert.ToUInt16(saesOutput[0], 2), 16).ToUpper();
                saesOutput[1] = saesCipherLabel2.Text = Convert.ToString(Convert.ToUInt16(saesOutput[1], 2), 16).ToUpper();
                saesOutput[2] = saesCipherLabel3.Text = Convert.ToString(Convert.ToUInt16(saesOutput[2], 2), 16).ToUpper();
                saesOutput[3] = saesCipherLabel4.Text = Convert.ToString(Convert.ToUInt16(saesOutput[3], 2), 16).ToUpper();
            }
        }

        private void rsaEncRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rsaDecRadioButton.Checked = !rsaEncRadioButton.Checked;
            isRSAEncrypting = rsaEncRadioButton.Checked;
            rsaErrorLabel.Visible = false;
            rsaMTextBox.Text = rsaCTextBox.Text = "";

            if (rsaEncRadioButton.Checked)
            {
                rsaMsgLabel.Visible = rsaMTextBox.Visible = true;
                rsaCipherLabel.Visible = rsaCTextBox.Visible = false;
                rsaOutputLabel.Text = "Cipher:";
            }
            else
            {
                rsaMsgLabel.Visible = rsaMTextBox.Visible = false;
                rsaCipherLabel.Visible = rsaCTextBox.Visible = true;
                rsaOutputLabel.Text = "Message:";
            }
        }

        private void rsaDecRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rsaEncRadioButton.Checked = !rsaDecRadioButton.Checked;
            isRSAEncrypting = rsaEncRadioButton.Checked;
            rsaErrorLabel.Visible = false;
            rsaMTextBox.Text = rsaCTextBox.Text = "";

            if (rsaEncRadioButton.Checked)
            {
                rsaMsgLabel.Visible = rsaMTextBox.Visible = true;
                rsaCipherLabel.Visible = rsaCTextBox.Visible = false;
                rsaOutputLabel.Text = "Cipher:";
            }
            else
            {
                rsaMsgLabel.Visible = rsaMTextBox.Visible = false;
                rsaCipherLabel.Visible = rsaCTextBox.Visible = true;
                rsaOutputLabel.Text = "Message:";
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            mainGroupBox.Visible = true;
            saesGroupBox.Visible = false;
            rsaGroupBox.Visible = false;
            rc4GroupBox.Visible = false;
            pfGroupBox.Visible = false;
            sdesGroupBox.Visible = false;
            backButton.Visible = false;

            if (stepsForm != null)
            {
                stepsForm.Close();
                stepsForm = null;
            }

            resetSaes();
            resetRSA();
        }

        private void resetSaes()
        {
            saesOutput = null;
            saesErrorLabel.Visible = false;
            saesCipherLabel1.Visible = saesCipherLabel2.Visible = saesCipherLabel3.Visible = saesCipherLabel4.Visible = false;
            saesCiperLabel.Visible = false;
            saesBinaryRadioButton.Checked = true;
            saesHexaRadioButton.Checked = false;
            saesStepsCheckBox.Checked = false;
            isSAESHexa = false;
            saesPlainTextBox1.Text = saesPlainTextBox2.Text = saesPlainTextBox3.Text = saesPlainTextBox4.Text = "";
            saesKeyTextBox1.Text = saesKeyTextBox2.Text = saesKeyTextBox3.Text = saesKeyTextBox4.Text = "";
        }

        private void resetRSA()
        {
            rsaOutput = null;
            rsaErrorLabel.Visible = false;
            rsaOutputLabel.Visible = rsaOutValLabel.Visible = false;
            rsaCipherLabel.Visible = rsaCTextBox.Visible = false;
            rsaMsgLabel.Visible = rsaMTextBox.Visible = true;
            rsaEncRadioButton.Checked = true;
            rsaDecRadioButton.Checked = false;
            rsaStepsCheckBox.Checked = false;
            isRSAEncrypting = true;
            rsaPTextBox.Text = rsaQTextBox.Text = rsaETextBox.Text = rsaMTextBox.Text = rsaCTextBox.Text = "";
        }

        bool isStringArrayValid(string[] text, char[] allowedCharacters, int minLength, int maxLength)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!isStringValid(text[i], allowedCharacters, minLength, maxLength))
                {
                    return false;
                }
            }

            return true;
        }

        bool isStringValid(string text, char[] allowedCharacters, int minLength, int maxLength)
        {
            if (text.Length < minLength || text.Length > maxLength)
            {
                return false;
            }

            return text.All(c => allowedCharacters.Contains(c));
        }

        private void rc4EncryptButton_Click(object sender, EventArgs e)
        {
            RC4 rc4 = new RC4();

            string[] rc4Input = rc4PlainTextBox1.Text.Split(',');

            string[] rc4Key = rc4KeyTextBox1.Text.Split(',');

        

            string output = rc4.Encrypt(rc4Input, rc4Key);

            if (rc4StepsCheckBox.Checked)
            {
                if (stepsForm != null)
                {
                    stepsForm.Close();
                }

                stepsForm = new StepsForm();

                Cryptographer.addStep(rc4.log, false, rc4StepsCheckBox.Checked, stepsForm);

                stepsForm.Show();
            }

            rc4CipherLabel1.Text = output;
            rc4CipherLabel1.Visible = true;
            rc4CipherLabel.Visible = true;
        }

        private void pfComputeButton_Click(object sender, EventArgs e)
        {
            PlayFair pf = new PlayFair();
            string output;
            
            if (pfEncRadioButton.Checked)
            {
                output = pf.Encrypt(pfKeyTextBox.Text, pfInputTextBox.Text);
            }
            else
            {
                output = pf.Decrypt(pfKeyTextBox.Text, pfInputTextBox.Text);
            }

            if (pfStepsCheckBox.Checked)
            {
                if (stepsForm != null)
                {
                    stepsForm.Close();
                }

                stepsForm = new StepsForm();

                Cryptographer.addStep(pf.log, false, pfStepsCheckBox.Checked, stepsForm);

                stepsForm.Show();
            }

            pfOutValLabel.Text = output;
            pfOutValLabel.Visible = true;
            pfOutputLabel.Visible = true;
        }

        private void sdesComputeButton_Click(object sender, EventArgs e)
        {
            string input = sdesInputTextBox1.Text + sdesInputTextBox2.Text;
            string key = sdesKeyTextBox1.Text + sdesKeyTextBox2.Text;
            SDES sdes= new SDES();
            string output;

            if (sdesEncRadioButton.Checked)
            {
                output = sdes.Encrypt(key, input);
            }
            else
            {
                output = sdes.Decrypt(key, input);
            }

            if (sdesStepsCheckBox.Checked)
            {
                if (stepsForm != null)
                {
                    stepsForm.Close();
                }

                stepsForm = new StepsForm();

                Cryptographer.addStep(sdes.log, false, sdesStepsCheckBox.Checked, stepsForm);

                stepsForm.Show();
            }

            sdesOutputLabel1.Text = output;
            sdesOutputLabel1.Visible = true;
            sdesOutputLabel.Visible = true;
        }

        private void pfEncRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pfDecRadioButton.Checked = !pfEncRadioButton.Checked;

            if (pfEncRadioButton.Checked)
            {
                pfOutputLabel.Text = "Cipher:";
                pfInputLabel.Text = "Plaintext:";
            }
            else
            {
                pfOutputLabel.Text = "Plaintext:";
                pfInputLabel.Text = "Cipher:";
            }
        }

        private void pfDecRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pfEncRadioButton.Checked = !pfDecRadioButton.Checked;

            if (pfEncRadioButton.Checked)
            {
                pfOutputLabel.Text = "Cipher:";
                pfInputLabel.Text = "Message:";
            }
            else
            {
                pfOutputLabel.Text = "Message:";
                pfInputLabel.Text = "Cipher:";
            }
        }

        private void sdesEncRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            sdesDecRadioButton.Checked = !sdesEncRadioButton.Checked;

            if (sdesEncRadioButton.Checked)
            {
                sdesOutputLabel.Text = "Ciphertext:";
                sdesInputLabel.Text = "Plaintext:";
            }
            else
            {
                sdesOutputLabel.Text = "Plaintext:";
                sdesInputLabel.Text = "Cipher:";
            }
        }

        private void sdesDecRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            sdesEncRadioButton.Checked = !sdesDecRadioButton.Checked;

            if (sdesEncRadioButton.Checked)
            {
                sdesOutputLabel.Text = "Ciphertext:";
                sdesInputLabel.Text = "Plaintext:";
            }
            else
            {
                sdesOutputLabel.Text = "Plaintext:";
                sdesInputLabel.Text = "Cipher:";
            }
        }

        bool isPrime(int num)
        {
            for (int i = 2; i <= num / 2; i++)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}