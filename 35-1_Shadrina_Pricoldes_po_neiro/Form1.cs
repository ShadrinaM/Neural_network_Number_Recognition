using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _35_1_Shadrina_Pricoldes_po_neiro
{
    public partial class Pricoldes_form : Form
    {
        private double[] inputPixels = new double[15] { 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d };
        public Pricoldes_form()
        {
            InitializeComponent();
        }

        //Изменение цвета кнопки и содержимого inputPixels[]
        private void PaintPixel(Button button, int index)
        {
            if (button.BackColor == SystemColors.GradientActiveCaption)
            {
                button.BackColor = SystemColors.MenuHighlight;
                button.ForeColor = SystemColors.HighlightText;
                inputPixels[index - 1] = 1d;
            }
            else
            {
                button.BackColor = SystemColors.GradientActiveCaption;
                button.ForeColor = SystemColors.MenuHighlight;
                inputPixels[index - 1] = 0d;
            }
        }

        //Нажатие кнопочек-пикселей
        private void button1_Click(object sender, EventArgs e)
        {
            PaintPixel(button1, 1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            PaintPixel(button2, 2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            PaintPixel(button3, 3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PaintPixel(button4, 4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            PaintPixel(button5, 5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            PaintPixel(button6, 6); 
        }
        private void button7_Click(object sender, EventArgs e)
        {
            PaintPixel(button7, 7);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            PaintPixel(button8, 8);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            PaintPixel(button9, 9);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            PaintPixel(button10, 10);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            PaintPixel(button11, 11);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            PaintPixel(button12, 12);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            PaintPixel(button13, 13);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            PaintPixel(button14, 14);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            PaintPixel(button15, 15);
        }



        //Кнопка для проверки вектора значений
        private void button16_Click(object sender, EventArgs e)
        {
            string s = string.Empty;
            s += "\n   ";            
            for (int i=0; i<15; i++)
            { 
                s += inputPixels[i].ToString();
                s += " ";
                if ((i+1)%3 == 0 && i!=0)
                {
                    s += "\n   ";
                }
            }
            richTextBox1.Text = s;
        }
    }

    
}
