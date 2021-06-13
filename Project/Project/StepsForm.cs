using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class StepsForm : Form
    {
        private RichTextBox stepsBox;

        public StepsForm()
        {
            InitializeComponent();

            stepsBox = stepsRichTextBox;
        }

        public void addTextToBox(string text, bool isHeader)
        {
            stepsBox.SelectionStart = stepsBox.TextLength;
            stepsBox.SelectionLength = text.Length;
            
            if(isHeader)
            {
                stepsBox.SelectionColor = Color.MidnightBlue;
            }
            else
            {
                stepsBox.SelectionColor = Color.Black;
            }

            stepsBox.AppendText(text);
        }
    }
}
