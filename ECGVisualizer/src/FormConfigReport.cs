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
    public partial class FormConfigReport : Form
    {
        public bool age, height, weight, race, gender;
        public bool segPQ, segQR, segRS, segST, segPR, segRR, segQT, segQTc, segQRS;
        public bool i1, i2, i3, iaVR, iaVL, iaVF, iV1, iV2, iV3, iV4, iV5, iV6;

        public bool Defage, Defheight, Defweight, Defrace, Defgender;
        public bool DefsegPQ, DefsegQR, DefsegRS, DefsegST, DefsegPR, DefsegRR, DefsegQT, DefsegQTc, DefsegQRS;
        public bool Defi1, Defi2, Defi3, DefiaVR, DefiaVL, DefiaVF, DefiV1, DefiV2, DefiV3, DefiV4, DefiV5, DefiV6;

        bool change;

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            segQRS = checkBox13.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            age = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            height = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            weight = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            race = checkBox4.Checked;
        }

        private void resetDefaults()
        {
            age = true;
            height = true;
            weight = true;
            race = true;
            gender = false;
            segPQ = false;
            segQR = false;
            segRS = false;
            segST = false;
            segPR = false;
            segRR = true;
            segQT = true;
            segQTc = true;
            segQRS = true;
            i1 = true;
            i2 = true;
            i3 = true;
            iaVR = true;
            iaVL = true;
            iaVF = true;
            iV1 = true;
            iV2 = true;
            iV3 = true;
            iV4 = true;
            iV5 = true;
            iV6 = true;
            change = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetDefaults();
        }

        private void undoChanges()
        {
            age = Defage;
            height = Defheight;
            weight = Defweight;
            race = Defrace;
            gender = Defgender;
            segPQ = DefsegPQ;
            segQR = DefsegQR;
            segRS = DefsegRS;
            segST = DefsegST;
            segPR = DefsegPR;
            segRR = DefsegRR;
            segQT = DefsegQT;
            segQTc = DefsegQTc;
            segQRS = DefsegQRS;
            i1 = Defi1;
            i2 = Defi2;
            i3 = Defi3;
            iaVR = DefiaVR;
            iaVL = DefiaVL;
            iaVF = DefiaVF;
            iV1 = DefiV1;
            iV2 = DefiV2;
            iV3 = DefiV3;
            iV4 = DefiV4;
            iV5 = DefiV5;
            iV6 = DefiV6;

            checkBox1.Checked = Defage;
            checkBox2.Checked = Defheight;
            checkBox3.Checked = Defweight;
            checkBox4.Checked = Defrace;
            checkBox5.Checked = DefsegPQ;
            checkBox6.Checked = DefsegQR;
            checkBox7.Checked = DefsegRS;
            checkBox8.Checked = DefsegST;
            checkBox9.Checked = DefsegPR;
            checkBox10.Checked = DefsegRR;
            checkBox11.Checked = DefsegQT;
            checkBox12.Checked = DefsegQTc;
            checkBox13.Checked = DefsegQRS;
            checkBox14.Checked = Defi1;
            checkBox15.Checked = Defi2;
            checkBox16.Checked = Defi3;
            checkBox17.Checked = DefiaVR;
            checkBox18.Checked = DefiaVL;
            checkBox19.Checked = DefiaVF;
            checkBox20.Checked = DefiV1;
            checkBox21.Checked = DefiV2;
            checkBox22.Checked = DefiV3;
            checkBox23.Checked = DefiV4;
            checkBox24.Checked = DefiV5;
            checkBox25.Checked = DefiV6;
            checkBox26.Checked = Defgender;
        }

        private void FormConfigReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!change)
                undoChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            change = true;
            this.Close();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            i1 = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            i2 = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            i3 = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            iaVR = checkBox17.Checked;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            iaVL = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            iaVF = checkBox19.Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            iV1 = checkBox20.Checked;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            iV2 = checkBox21.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            iV3 = checkBox22.Checked;
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            iV4 = checkBox23.Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            iV5 = checkBox24.Checked;
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            iV6 = checkBox25.Checked;
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            gender = checkBox26.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            segQTc = checkBox12.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            segQT = checkBox11.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            segRR = checkBox10.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            segPR = checkBox9.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            segST = checkBox8.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            segRS = checkBox7.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            segQR = checkBox6.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            segPQ = checkBox5.Checked;
        }

        public FormConfigReport()
        {
            InitializeComponent();
        }

        public FormConfigReport(bool ag, bool hei, bool wei, bool rac, bool gen, bool pq, bool qr, bool rs, bool st, bool pr, bool rr, bool qt, bool qtc, bool qrs, bool i1, bool i2, bool i3, bool aVR, bool aVL, bool aVF, bool v1, bool v2, bool v3, bool v4, bool v5, bool v6)
        {
            InitializeComponent();
            this.Defage = ag;
            this.Defheight = hei;
            this.Defweight = wei;
            this.Defrace = rac;
            this.Defgender = gen;
            this.DefsegPQ = pq;
            this.DefsegQR = qr;
            this.DefsegRS = rs;
            this.DefsegST = st;
            this.DefsegPR = pr;
            this.DefsegRR = rr;
            this.DefsegQT = qt;
            this.DefsegQTc = qtc;
            this.DefsegQRS = qrs;
            this.Defi1 = i1;
            this.Defi2 = i2;
            this.Defi3 = i3;
            this.DefiaVR = aVR;
            this.DefiaVL = aVL;
            this.DefiaVF = aVF;
            this.DefiV1 = v1;
            this.DefiV2 = v2;
            this.DefiV3 = v3;
            this.DefiV4 = v4;
            this.DefiV5 = v5;
            this.DefiV6 = v6;

            undoChanges();

            change = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            undoChanges();
        }
    }
}
