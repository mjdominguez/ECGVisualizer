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
    public partial class FormConfig : Form
    {
        public int selected;
        public double[][] valsCustom = new double[2][];
        public int R_peaks_offset;
        public int Segments_window_offset;
        public int P_T_searching_window;

        public int Defselected;
        public double[][] DefvalsCustom = new double[2][];
        public int DefR_peaks_offset = 40;
        public int DefSegments_window_offset = 10;
        public int DefP_T_searching_window = 10;

        bool change;
        public FormConfig()
        {
            InitializeComponent();
            for(int i=0; i<2; i++)
            {
                valsCustom[i] = new double[3];
            }
            selected = 1;
            valsCustom[0][0] = 120;
            valsCustom[0][1] = 80;
            valsCustom[0][2] = 350;
            valsCustom[1][0] = 200;
            valsCustom[1][1] = 120;
            valsCustom[1][2] = 430;
            change = false;
        }

        public FormConfig(double[] valsMin, double[] valsMax, int sel, int r_peaks, int window_off, int search)
        {
            InitializeComponent();
            for (int i = 0; i < 2; i++)
            {
                DefvalsCustom[i] = new double[3];
                valsCustom[i] = new double[3];
            }
            DefR_peaks_offset = r_peaks;
            DefSegments_window_offset = window_off;
            DefP_T_searching_window = search;
            Defselected = sel;
            DefvalsCustom[0][0] = valsMin[0];
            DefvalsCustom[0][1] = valsMin[1];
            DefvalsCustom[0][2] = valsMin[2];
            DefvalsCustom[1][0] = valsMax[0];
            DefvalsCustom[1][1] = valsMax[1];
            DefvalsCustom[1][2] = valsMax[2];
            undo();
            change = false;
        }

        private void restoreDefaults()
        {
            R_peaks_offset = 40;
            Segments_window_offset = 10;
            P_T_searching_window = 10;
            trackBar1.Value = 40;
            trackBar2.Value = 10;
            trackBar3.Value = 10;
            label12.Text = R_peaks_offset + "%";
            label13.Text = Segments_window_offset + "%";
            label14.Text = P_T_searching_window + "%";
            selected = 1;
            valsCustom[0][0] = 120;
            valsCustom[0][1] = 80;
            valsCustom[0][2] = 350;
            valsCustom[1][0] = 200;
            valsCustom[1][1] = 120;
            valsCustom[1][2] = 430;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = true;
            label8.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            selected = 2;
            textBox1.Text = valsCustom[0][0].ToString();
            textBox3.Text = valsCustom[0][1].ToString();
            textBox5.Text = valsCustom[0][2].ToString();
            textBox2.Text = valsCustom[1][0].ToString();
            textBox4.Text = valsCustom[1][1].ToString();
            textBox6.Text = valsCustom[1][2].ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            selected = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            selected = 1;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            R_peaks_offset = trackBar1.Value;
            label12.Text = R_peaks_offset + "%";
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            Segments_window_offset = trackBar2.Value;
            label13.Text = Segments_window_offset + "%";
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            P_T_searching_window = trackBar3.Value;
            label14.Text = P_T_searching_window + "%";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            restoreDefaults();
            change = true;
        }

        private void FormConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!change)
                restoreDefaults();
        }

        private void undo()
        {
            R_peaks_offset = DefR_peaks_offset;
            Segments_window_offset = DefSegments_window_offset;
            P_T_searching_window = DefP_T_searching_window;
            selected = Defselected;
            trackBar1.Value = R_peaks_offset;
            trackBar2.Value = Segments_window_offset;
            trackBar3.Value = P_T_searching_window;
            label12.Text = R_peaks_offset + "%";
            label13.Text = Segments_window_offset + "%";
            label14.Text = P_T_searching_window + "%";
            valsCustom[0][0] = DefvalsCustom[0][0];
            valsCustom[0][1] = DefvalsCustom[0][1];
            valsCustom[0][2] = DefvalsCustom[0][2];
            valsCustom[1][0] = DefvalsCustom[1][0];
            valsCustom[1][1] = DefvalsCustom[1][1];
            valsCustom[1][2] = DefvalsCustom[1][2];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            change = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            undo();
            change = true;
        }
    }
}
