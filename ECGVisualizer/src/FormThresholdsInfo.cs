using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECGVisualizer
{
    public partial class FormThresholdsInfo : Form
    {
        public FormThresholdsInfo()
        {
            InitializeComponent();
        }

        public FormThresholdsInfo(double[][] segms)
        {
            InitializeComponent();

            //Athlete
            label7.Text  = "[" + segms[0][0].ToString("N2") + " - " + segms[0][1].ToString("N2") + "]";
            label12.Text = "[" + segms[0][2].ToString("N2") + " - " + segms[0][3].ToString("N2") + "]";
            label10.Text = "[" + segms[0][4].ToString("N2") + " - " + segms[0][5].ToString("N2") + "]";
            //Common
            label8.Text  = "[" + segms[1][0].ToString("N2") + " - " + segms[1][1].ToString("N2") + "]";
            label13.Text = "[" + segms[1][2].ToString("N2") + " - " + segms[1][3].ToString("N2") + "]";
            label15.Text = "[" + segms[1][4].ToString("N2") + " - " + segms[1][5].ToString("N2") + "]";
            //Custom
            label9.Text  = "[" + segms[2][0].ToString("N2") + " - " + segms[2][1].ToString("N2") + "]";
            label14.Text = "[" + segms[2][2].ToString("N2") + " - " + segms[2][2].ToString("N2") + "]";
            label11.Text = "[" + segms[2][4].ToString("N2") + " - " + segms[2][4].ToString("N2") + "]";

            pictureBox1.Image = Properties.Resources.ECGwave;
        }

        private void FormThresholdsInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.pictureBox1.Dispose();
        }
    }
}
