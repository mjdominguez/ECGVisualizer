using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ECGVisualizer
{
    public partial class Form1 : Form
    {
        Atribs atributes = new Atribs();
        bool peaksDetected = false, modification = false, manualF = false;
        int frecuenciaMuestreo;
        int ventanaAnchoR;
        int ventanaRetraso;
        //According to intervals
        int ventanaAnchoQ2;
        int ventanaAnchoS2;
        int ventanaAnchoP2;
        int ventanaAnchoT2;
        int ventanaAnchoQ2_2;
        int ventanaAnchoS2_2;
        int ventanaAnchoP2_2;
        int ventanaAnchoT2_2;
        int ventanaEnPosiciones;
        double umbralPicoP;
        double ancho;

        bool I = false, II = false, III = false, aVR = false, aVL = false, aVF = false, V1 = false, V2 = false, V3 = false, V4 = false, V5 = false, V6 = false;
        public Form1()
        {
            InitializeComponent();

            atributes.createLists();

            Dialogs.openFileDial(ref atributes);
            Dialogs.saveFileDial(ref atributes);
            Dialogs.configReport(ref atributes);

            this.ClientSize = new System.Drawing.Size(946, 372);
            CheckForIllegalCrossThreadCalls = false;
        }

        private void rESTORESIGNALSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.clearData(ref atributes);

            Tools.restoreData(ref atributes);

            atributes.herMod = atributes.her;

            atributes.blockSize = Math.Round((2 * atributes.herMod) * (1.0 / (double)atributes.herMod), 1);

            automaticPQRSTDetectionToolStripMenuItem.Enabled = false;
            generateReportToolStripMenuItem.Enabled = false;
            addLabelsAutomaticallyToolStripMenuItem.Enabled = true;
            filteringToolStripMenuItem.Enabled = true;

            drawECG();
        }  

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                atributes.T = true;
            else
                atributes.T = false;

            clearECG();
            drawECG();

            ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;

            drawPeaks();
        }

        private void drawPeaks()
        {
            if (checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                atributes.S = true;
            else
                atributes.S = false;

            clearECG();
            drawECG();

            double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;

            if (checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                atributes.R = true;
            else
                atributes.R = false;

            clearECG();
            drawECG();

            double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;

            if (checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                atributes.Q = true;
            else
                atributes.Q = false;

            clearECG();
            drawECG();

            double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;

            if (checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                atributes.P = true;
            else
                atributes.P = false;

            clearECG();
            drawECG();

            double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;

            if (checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<double> seg2 = Report.segmentsCalc(ref atributes.picosP2, ref atributes.picosQ2, ref atributes.picosR2, ref atributes.picosS2, ref atributes.picosT2);

            double mediaSegPQ = 1000 * seg2[0];
            double mediaSegQR = 1000 * seg2[1];
            double mediaSegRS = 1000 * seg2[2];
            double mediaSegST = 1000 * seg2[3];
            double mediaSegPR = 1000 * seg2[4];
            double mediaSegQRS = 1000 * seg2[5];
            double mediaSegRR = seg2[6];

            mediaSegRR = mediaSegRR * 1000;
            double mediaSegQT = 1000 * seg2[7];
            double mediaSegQT_c = 1000 * seg2[8];

            atributes.inversionT[0] = Report.Tinversion(ref atributes.data1f, ref atributes.picosT1, atributes.herMod);
            atributes.inversionT[1] = Report.Tinversion(ref atributes.data2f, ref atributes.picosT2, atributes.herMod);
            atributes.inversionT[2] = Report.Tinversion(ref atributes.data3f, ref atributes.picosT3, atributes.herMod);
            atributes.inversionT[3] = Report.Tinversion(ref atributes.data4f, ref atributes.picosT4, atributes.herMod);
            atributes.inversionT[4] = Report.Tinversion(ref atributes.data5f, ref atributes.picosT5, atributes.herMod);
            atributes.inversionT[5] = Report.Tinversion(ref atributes.data6f, ref atributes.picosT6, atributes.herMod);
            atributes.inversionT[6] = Report.Tinversion(ref atributes.data7f, ref atributes.picosT7, atributes.herMod);
            atributes.inversionT[7] = Report.Tinversion(ref atributes.data8f, ref atributes.picosT8, atributes.herMod);
            atributes.inversionT[8] = Report.Tinversion(ref atributes.data9f, ref atributes.picosT9, atributes.herMod);
            atributes.inversionT[9] = Report.Tinversion(ref atributes.data10f, ref atributes.picosT10, atributes.herMod);
            atributes.inversionT[10] = Report.Tinversion(ref atributes.data11f, ref atributes.picosT11, atributes.herMod);
            atributes.inversionT[11] = Report.Tinversion(ref atributes.data12f, ref atributes.picosT11, atributes.herMod);

            int inv1, inv2, inv3, inv4, inv5, inv6, inv7, inv8, inv9, inv10, inv11, inv12;
            inv1 = atributes.inversionT[0] ? 1 : 0;
            inv2 = atributes.inversionT[1] ? 1 : 0;
            inv3 = atributes.inversionT[2] ? 1 : 0;
            inv4 = atributes.inversionT[3] ? 1 : 0;
            inv5 = atributes.inversionT[4] ? 1 : 0;
            inv6 = atributes.inversionT[5] ? 1 : 0;
            inv7 = atributes.inversionT[6] ? 1 : 0;
            inv8 = atributes.inversionT[7] ? 1 : 0;
            inv9 = atributes.inversionT[8] ? 1 : 0;
            inv10 = atributes.inversionT[9] ? 1 : 0;
            inv11 = atributes.inversionT[10] ? 1 : 0;
            inv12 = atributes.inversionT[11] ? 1 : 0;

            bool nuevo = false;

            string text="";
            if (!File.Exists(atributes.reportFile))
            {
                string clientHeader = "ID" + ";";

                if (atributes.repAge)
                    clientHeader += "Age" + ";";
                if (atributes.repRace)
                    clientHeader += "Race" + ";";
                if (atributes.repGender)
                    clientHeader += "Gender" + ";";
                if (atributes.repHeight)
                    clientHeader += "Height" + ";";
                if (atributes.repWeight)
                    clientHeader += "Weight" + ";";
                if (atributes.repPQ)
                    clientHeader += "PQ interval" + ";";
                if (atributes.repQR)
                    clientHeader += "QR interval" + ";";
                if (atributes.repRS)
                    clientHeader += "RS interval" + ";";
                if (atributes.repST)
                    clientHeader += "ST interval" + ";";
                if (atributes.repPR)
                    clientHeader += "PR interval" + ";";
                if (atributes.repRR)
                    clientHeader += "RR interval" + ";";
                if (atributes.repQRS)
                    clientHeader += "QRS interval" + ";";
                if (atributes.repQT)
                    clientHeader += "QT interval" + ";";
                if (atributes.repQT_c)
                    clientHeader += "QT_c interval" + ";";
                if (atributes.repi1)
                    clientHeader += "T inv I" + ";";
                if (atributes.repi2)
                    clientHeader += "T inv II" + ";";
                if (atributes.repi3)
                    clientHeader += "T inv III" + ";";
                if (atributes.repaVR)
                    clientHeader += "T inv aVR" + ";";
                if (atributes.repaVL)
                    clientHeader += "T inv aVL" + ";";
                if (atributes.repaVF)
                    clientHeader += "T inv aVF" + ";";
                if (atributes.repV1)
                    clientHeader += "T inv V1" + ";";
                if (atributes.repV2)
                    clientHeader += "T inv V2" + ";";
                if (atributes.repV3)
                    clientHeader += "T inv V3" + ";";
                if (atributes.repV4)
                    clientHeader += "T inv V4" + ";";
                if (atributes.repV5)
                    clientHeader += "T inv V5" + ";";
                if (atributes.repV6)
                    clientHeader += "T inv V6" + ";";

                clientHeader += Environment.NewLine;

                File.WriteAllText(atributes.reportFile, clientHeader);

                nuevo = true;
            }

            string fileName2 = "";

            int found1 = atributes.fileName.LastIndexOf("\\");
            int found2 = atributes.fileName.IndexOf(".xml");
            fileName2 = atributes.fileName.Substring(found1+1,found2-found1-1);

            text = fileName2 + ";";

            if (atributes.repAge)
                text += atributes.age + ";";
            if (atributes.repRace)
                text += atributes.race + ";";
            if (atributes.repGender)
                text += atributes.gender + ";";
            if (atributes.repHeight)
                text += atributes.height + ";";
            if (atributes.repWeight)
                text += Convert.ToDouble(atributes.weight) / 10 + ";";
            if (atributes.repPQ)
                text += mediaSegPQ + ";";
            if (atributes.repQR)
                text += mediaSegQR + ";";
            if (atributes.repRS)
                text += mediaSegRS + ";";
            if (atributes.repST)
                text += mediaSegST + ";";
            if (atributes.repPR)
                text += mediaSegPR + ";";
            if (atributes.repRR)
                text += mediaSegRR + ";";
            if (atributes.repQRS)
                text += mediaSegQRS + ";";
            if (atributes.repQT)
                text += mediaSegQT + ";";
            if (atributes.repQT_c)
                text += mediaSegQT_c + ";";
            if (atributes.repi1)
                text += inv1 + ";";
            if (atributes.repi2)
                text += inv2 + ";";
            if (atributes.repi3)
                text += inv3 + ";";
            if (atributes.repaVR)
                text += inv4 + ";";
            if (atributes.repaVL)
                text += inv5 + ";";
            if (atributes.repaVF)
                text += inv6 + ";";
            if (atributes.repV1)
                text += inv7 + ";";
            if (atributes.repV2)
                text += inv8 + ";";
            if (atributes.repV3)
                text += inv9 + ";";
            if (atributes.repV4)
                text += inv10 + ";";
            if (atributes.repV5)
                text += inv11 + ";";
            if (atributes.repV6)
                text += inv12 + ";";

            text += Environment.NewLine;

            File.AppendAllText(atributes.reportFile, text);

            string mess;
            if (nuevo)
                mess = "New Report Generated";
            else
                mess = "Data Appended to Previous Report";

            MessageBox.Show(mess);

        }

        private void automaticPQRSTDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frecuenciaMuestreo = (int)atributes.herMod;
            ventanaAnchoR = (int)(1.0 / (atributes.bmpsFile / 60.0) * frecuenciaMuestreo);
            ventanaRetraso = (int)(ventanaAnchoR*0.3);

            //According to intervals
            ventanaAnchoQ2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : (int)((atributes.customSegmentsMax[1] / 2.0) * frecuenciaMuestreo);
            ventanaAnchoS2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : (int)((atributes.customSegmentsMax[1] / 2.0) * frecuenciaMuestreo);
            ventanaAnchoP2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMax[0] - (atributes.averageSegmentsMin[1] / 2.0)) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMax[0] - (atributes.athleteSegmentsMin[1] / 2.0)) * frecuenciaMuestreo) : (int)((atributes.customSegmentsMax[0] - (atributes.customSegmentsMin[1] / 2.0)) * frecuenciaMuestreo);
            
            ventanaAnchoT2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMax[2] - atributes.averageSegmentsMin[1]) * frecuenciaMuestreo) :
                                 atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMax[2] - atributes.athleteSegmentsMin[1]) * frecuenciaMuestreo) : 
                                                             (int)((atributes.customSegmentsMax[2] - atributes.customSegmentsMin[1]) * frecuenciaMuestreo);

            ventanaAnchoQ2_2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMin[1] / 2.0) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMin[1] / 2.0) * frecuenciaMuestreo): (int)((atributes.customSegmentsMin[1] / 2.0) * frecuenciaMuestreo);
            ventanaAnchoS2_2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMin[1] / 2.0) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMin[1] / 2.0) * frecuenciaMuestreo): (int)((atributes.customSegmentsMin[1] / 2.0) * frecuenciaMuestreo);
            ventanaAnchoP2_2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMin[0] - (atributes.averageSegmentsMax[1] / 2.0)) * frecuenciaMuestreo) : atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMin[0] - (atributes.athleteSegmentsMax[1] / 2.0)) * frecuenciaMuestreo) : (int)((atributes.customSegmentsMin[0] - (atributes.customSegmentsMax[1] / 2.0)) * frecuenciaMuestreo);
            
            ventanaAnchoT2_2 = atributes.typePeople == 0 ? (int)((atributes.averageSegmentsMin[2] - atributes.averageSegmentsMax[1]) * frecuenciaMuestreo) :
                                   atributes.typePeople == 1 ? (int)((atributes.athleteSegmentsMin[2] - atributes.athleteSegmentsMax[1]) * frecuenciaMuestreo) :
                                                               (int)((atributes.customSegmentsMin[2] - atributes.customSegmentsMax[1]) * frecuenciaMuestreo);

            ventanaEnPosiciones = (int)((atributes.ventanaComparacion / 100.0) * atributes.herMod);

            umbralPicoP = 0;

            ancho = atributes.blockSize >2?1: atributes.blockSize / 2.0;

            if (atributes.R)
            {
                atributes.picosR1 = ECGpeaks.DetectarPicosR2(atributes.data1f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot /100.0));
                atributes.picosR2 = ECGpeaks.DetectarPicosR2(atributes.data2f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR3 = ECGpeaks.DetectarPicosR2(atributes.data3f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR4 = ECGpeaks.DetectarPicosR2(atributes.data4f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR5 = ECGpeaks.DetectarPicosR2(atributes.data5f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR6 = ECGpeaks.DetectarPicosR2(atributes.data6f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR7 = ECGpeaks.DetectarPicosR2(atributes.data7f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR8 = ECGpeaks.DetectarPicosR2(atributes.data8f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR9 = ECGpeaks.DetectarPicosR2(atributes.data9f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR10 = ECGpeaks.DetectarPicosR2(atributes.data10f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR11 = ECGpeaks.DetectarPicosR2(atributes.data11f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));
                atributes.picosR12 = ECGpeaks.DetectarPicosR2(atributes.data12f, ventanaAnchoR, ventanaRetraso, atributes.herMod, (atributes.umbralRuidoPot / 100.0));

                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);

                if (atributes.Q)
                {
                    //DETECCION DE PICOS Q
                    atributes.picosQ1 = ECGpeaks.DetectarPicosQ2(atributes.data1f, atributes.picosR1, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime /100.0));
                    atributes.picosQ2 = ECGpeaks.DetectarPicosQ2(atributes.data2f, atributes.picosR2, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ3 = ECGpeaks.DetectarPicosQ2(atributes.data3f, atributes.picosR3, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ4 = ECGpeaks.DetectarPicosQ2(atributes.data4f, atributes.picosR4, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ5 = ECGpeaks.DetectarPicosQ2(atributes.data5f, atributes.picosR5, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ6 = ECGpeaks.DetectarPicosQ2(atributes.data6f, atributes.picosR6, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ7 = ECGpeaks.DetectarPicosQ2(atributes.data7f, atributes.picosR7, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ8 = ECGpeaks.DetectarPicosQ2(atributes.data8f, atributes.picosR8, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ9 = ECGpeaks.DetectarPicosQ2(atributes.data9f, atributes.picosR9, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ10 = ECGpeaks.DetectarPicosQ2(atributes.data10f, atributes.picosR10, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ11 = ECGpeaks.DetectarPicosQ2(atributes.data11f, atributes.picosR11, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosQ12 = ECGpeaks.DetectarPicosQ2(atributes.data12f, atributes.picosR12, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));

                    Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);

                    if (atributes.P)
                    {
                        //DETECCION PICOS P
                        atributes.picosP1 = ECGpeaks.DetectarPicosP1(atributes.data1f, atributes.picosQ1, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP2 = ECGpeaks.DetectarPicosP1(atributes.data2f, atributes.picosQ2, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP3 = ECGpeaks.DetectarPicosP1(atributes.data3f, atributes.picosQ3, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP4 = ECGpeaks.DetectarPicosP1(atributes.data4f, atributes.picosQ4, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP5 = ECGpeaks.DetectarPicosP1(atributes.data5f, atributes.picosQ5, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP6 = ECGpeaks.DetectarPicosP1(atributes.data6f, atributes.picosQ6, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP7 = ECGpeaks.DetectarPicosP1(atributes.data7f, atributes.picosQ7, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP8 = ECGpeaks.DetectarPicosP1(atributes.data8f, atributes.picosQ8, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP9 = ECGpeaks.DetectarPicosP1(atributes.data9f, atributes.picosQ9, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP10 = ECGpeaks.DetectarPicosP1(atributes.data10f, atributes.picosQ10, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP11 = ECGpeaks.DetectarPicosP1(atributes.data11f, atributes.picosQ11, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosP12 = ECGpeaks.DetectarPicosP1(atributes.data12f, atributes.picosQ12, ventanaAnchoP2, ventanaAnchoP2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);

                        Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                        Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
                    }
                }

                if (atributes.S)
                {
                    //DETECCION DE PICOS S
                    atributes.picosS1 = ECGpeaks.DetectarPicosS2(atributes.data1f, atributes.picosR1, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS2 = ECGpeaks.DetectarPicosS2(atributes.data2f, atributes.picosR2, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS3 = ECGpeaks.DetectarPicosS2(atributes.data3f, atributes.picosR3, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS4 = ECGpeaks.DetectarPicosS2(atributes.data4f, atributes.picosR4, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS5 = ECGpeaks.DetectarPicosS2(atributes.data5f, atributes.picosR5, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS6 = ECGpeaks.DetectarPicosS2(atributes.data6f, atributes.picosR6, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS7 = ECGpeaks.DetectarPicosS2(atributes.data7f, atributes.picosR7, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS8 = ECGpeaks.DetectarPicosS2(atributes.data8f, atributes.picosR8, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS9 = ECGpeaks.DetectarPicosS2(atributes.data9f, atributes.picosR9, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS10 = ECGpeaks.DetectarPicosS2(atributes.data10f, atributes.picosR10, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS11 = ECGpeaks.DetectarPicosS2(atributes.data11f, atributes.picosR11, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));
                    atributes.picosS12 = ECGpeaks.DetectarPicosS2(atributes.data12f, atributes.picosR12, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, atributes.herMod, (atributes.umbralRuidoTime / 100.0));

                    Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);

                    if (atributes.T)
                    {
                        //DETECCION PICOS T
                        atributes.picosT1 = ECGpeaks.DetectarPicosT1(atributes.data1f, atributes.picosS1, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT2 = ECGpeaks.DetectarPicosT1(atributes.data2f, atributes.picosS2, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT3 = ECGpeaks.DetectarPicosT1(atributes.data3f, atributes.picosS3, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT4 = ECGpeaks.DetectarPicosT1(atributes.data4f, atributes.picosS4, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT5 = ECGpeaks.DetectarPicosT1(atributes.data5f, atributes.picosS5, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT6 = ECGpeaks.DetectarPicosT1(atributes.data6f, atributes.picosS6, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT7 = ECGpeaks.DetectarPicosT1(atributes.data7f, atributes.picosS7, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT8 = ECGpeaks.DetectarPicosT1(atributes.data8f, atributes.picosS8, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT9 = ECGpeaks.DetectarPicosT1(atributes.data9f, atributes.picosS9, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT10 = ECGpeaks.DetectarPicosT1(atributes.data10f, atributes.picosS10, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT11 = ECGpeaks.DetectarPicosT1(atributes.data11f, atributes.picosS11, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        atributes.picosT12 = ECGpeaks.DetectarPicosT1(atributes.data12f, atributes.picosS12, ventanaAnchoT2, ventanaAnchoT2_2, atributes.herMod, (atributes.umbralRuidoTime / 100.0), ventanaEnPosiciones);

                        Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                        Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
                    }
                }

            }
            label18.Visible = true;
            label19.Visible = true;
            label20.Visible = true;
            label21.Visible = true;
            label22.Visible = true;

            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;

            peaksDetected = true;
            if(modification)
                groupBox12.Visible = true;

            generateReportToolStripMenuItem.Enabled = true;
            automaticPQRSTDetectionToolStripMenuItem.Enabled = false;
        }

        private void savePreviousFAF()
        {
            atributes.auxFAF1 = new List<double>(atributes.data1f);
            atributes.auxFAF2 = new List<double>(atributes.data2f);
            atributes.auxFAF3 = new List<double>(atributes.data3f);
            atributes.auxFAF4 = new List<double>(atributes.data4f);
            atributes.auxFAF5 = new List<double>(atributes.data5f);
            atributes.auxFAF6 = new List<double>(atributes.data6f);
            atributes.auxFAF7 = new List<double>(atributes.data7f);
            atributes.auxFAF8 = new List<double>(atributes.data8f);
            atributes.auxFAF9 = new List<double>(atributes.data9f);
            atributes.auxFAF10 = new List<double>(atributes.data10f);
            atributes.auxFAF11 = new List<double>(atributes.data11f);
            atributes.auxFAF12 = new List<double>(atributes.data12f);

            atributes.rawauxFAF1 = new List<double>(atributes.raw1f);
            atributes.rawauxFAF2 = new List<double>(atributes.raw2f);
            atributes.rawauxFAF3 = new List<double>(atributes.raw3f);
            atributes.rawauxFAF4 = new List<double>(atributes.raw4f);
            atributes.rawauxFAF5 = new List<double>(atributes.raw5f);
            atributes.rawauxFAF6 = new List<double>(atributes.raw6f);
            atributes.rawauxFAF7 = new List<double>(atributes.raw7f);
            atributes.rawauxFAF8 = new List<double>(atributes.raw8f);
            atributes.rawauxFAF9 = new List<double>(atributes.raw9f);
            atributes.rawauxFAF10 = new List<double>(atributes.raw10f);
            atributes.rawauxFAF11 = new List<double>(atributes.raw11f);
            atributes.rawauxFAF12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void restorePreviousFAF()
        {
            atributes.data1f = new List<double>(atributes.auxFAF1);
            atributes.data2f = new List<double>(atributes.auxFAF2);
            atributes.data3f = new List<double>(atributes.auxFAF3);
            atributes.data4f = new List<double>(atributes.auxFAF4);
            atributes.data5f = new List<double>(atributes.auxFAF5);
            atributes.data6f = new List<double>(atributes.auxFAF6);
            atributes.data7f = new List<double>(atributes.auxFAF7);
            atributes.data8f = new List<double>(atributes.auxFAF8);
            atributes.data9f = new List<double>(atributes.auxFAF9);
            atributes.data10f = new List<double>(atributes.auxFAF10);
            atributes.data11f = new List<double>(atributes.auxFAF11);
            atributes.data12f = new List<double>(atributes.auxFAF12);

            atributes.raw1f = new List<double>(atributes.rawauxFAF1);
            atributes.raw2f = new List<double>(atributes.rawauxFAF2);
            atributes.raw3f = new List<double>(atributes.rawauxFAF3);
            atributes.raw4f = new List<double>(atributes.rawauxFAF4);
            atributes.raw5f = new List<double>(atributes.rawauxFAF5);
            atributes.raw6f = new List<double>(atributes.rawauxFAF6);
            atributes.raw7f = new List<double>(atributes.rawauxFAF7);
            atributes.raw8f = new List<double>(atributes.rawauxFAF8);
            atributes.raw9f = new List<double>(atributes.rawauxFAF9);
            atributes.raw10f = new List<double>(atributes.rawauxFAF10);
            atributes.raw11f = new List<double>(atributes.rawauxFAF11);
            atributes.raw12f = new List<double>(atributes.rawauxFAF12);

            atributes.herMod = atributes.previousHertzs;
            atributes.firstFAF = true;
            button5.Enabled = false;
        }

        private void savePreviousMAF()
        {
            atributes.auxMAF1 = new List<double>(atributes.data1f);
            atributes.auxMAF2 = new List<double>(atributes.data2f);
            atributes.auxMAF3 = new List<double>(atributes.data3f);
            atributes.auxMAF4 = new List<double>(atributes.data4f);
            atributes.auxMAF5 = new List<double>(atributes.data5f);
            atributes.auxMAF6 = new List<double>(atributes.data6f);
            atributes.auxMAF7 = new List<double>(atributes.data7f);
            atributes.auxMAF8 = new List<double>(atributes.data8f);
            atributes.auxMAF9 = new List<double>(atributes.data9f);
            atributes.auxMAF10 = new List<double>(atributes.data10f);
            atributes.auxMAF11 = new List<double>(atributes.data11f);
            atributes.auxMAF12 = new List<double>(atributes.data12f);

            atributes.rawauxMAF1 = new List<double>(atributes.raw1f);
            atributes.rawauxMAF2 = new List<double>(atributes.raw2f);
            atributes.rawauxMAF3 = new List<double>(atributes.raw3f);
            atributes.rawauxMAF4 = new List<double>(atributes.raw4f);
            atributes.rawauxMAF5 = new List<double>(atributes.raw5f);
            atributes.rawauxMAF6 = new List<double>(atributes.raw6f);
            atributes.rawauxMAF7 = new List<double>(atributes.raw7f);
            atributes.rawauxMAF8 = new List<double>(atributes.raw8f);
            atributes.rawauxMAF9 = new List<double>(atributes.raw9f);
            atributes.rawauxMAF10 = new List<double>(atributes.raw10f);
            atributes.rawauxMAF11 = new List<double>(atributes.raw11f);
            atributes.rawauxMAF12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void savePreviousAM()
        {
            atributes.auxAM1 = new List<double>(atributes.data1f);
            atributes.auxAM2 = new List<double>(atributes.data2f);
            atributes.auxAM3 = new List<double>(atributes.data3f);
            atributes.auxAM4 = new List<double>(atributes.data4f);
            atributes.auxAM5 = new List<double>(atributes.data5f);
            atributes.auxAM6 = new List<double>(atributes.data6f);
            atributes.auxAM7 = new List<double>(atributes.data7f);
            atributes.auxAM8 = new List<double>(atributes.data8f);
            atributes.auxAM9 = new List<double>(atributes.data9f);
            atributes.auxAM10 = new List<double>(atributes.data10f);
            atributes.auxAM11 = new List<double>(atributes.data11f);
            atributes.auxAM12 = new List<double>(atributes.data12f);

            atributes.rawauxAM1 = new List<double>(atributes.raw1f);
            atributes.rawauxAM2 = new List<double>(atributes.raw2f);
            atributes.rawauxAM3 = new List<double>(atributes.raw3f);
            atributes.rawauxAM4 = new List<double>(atributes.raw4f);
            atributes.rawauxAM5 = new List<double>(atributes.raw5f);
            atributes.rawauxAM6 = new List<double>(atributes.raw6f);
            atributes.rawauxAM7 = new List<double>(atributes.raw7f);
            atributes.rawauxAM8 = new List<double>(atributes.raw8f);
            atributes.rawauxAM9 = new List<double>(atributes.raw9f);
            atributes.rawauxAM10 = new List<double>(atributes.raw10f);
            atributes.rawauxAM11 = new List<double>(atributes.raw11f);
            atributes.rawauxAM12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void savePreviousTM()
        {
            atributes.auxTM1 = new List<double>(atributes.data1f);
            atributes.auxTM2 = new List<double>(atributes.data2f);
            atributes.auxTM3 = new List<double>(atributes.data3f);
            atributes.auxTM4 = new List<double>(atributes.data4f);
            atributes.auxTM5 = new List<double>(atributes.data5f);
            atributes.auxTM6 = new List<double>(atributes.data6f);
            atributes.auxTM7 = new List<double>(atributes.data7f);
            atributes.auxTM8 = new List<double>(atributes.data8f);
            atributes.auxTM9 = new List<double>(atributes.data9f);
            atributes.auxTM10 = new List<double>(atributes.data10f);
            atributes.auxTM11 = new List<double>(atributes.data11f);
            atributes.auxTM12 = new List<double>(atributes.data12f);

            atributes.rawauxTM1 = new List<double>(atributes.raw1f);
            atributes.rawauxTM2 = new List<double>(atributes.raw2f);
            atributes.rawauxTM3 = new List<double>(atributes.raw3f);
            atributes.rawauxTM4 = new List<double>(atributes.raw4f);
            atributes.rawauxTM5 = new List<double>(atributes.raw5f);
            atributes.rawauxTM6 = new List<double>(atributes.raw6f);
            atributes.rawauxTM7 = new List<double>(atributes.raw7f);
            atributes.rawauxTM8 = new List<double>(atributes.raw8f);
            atributes.rawauxTM9 = new List<double>(atributes.raw9f);
            atributes.rawauxTM10 = new List<double>(atributes.raw10f);
            atributes.rawauxTM11 = new List<double>(atributes.raw11f);
            atributes.rawauxTM12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void restorePreviousMAF()
        {
            atributes.data1f = new List<double>(atributes.auxMAF1);
            atributes.data2f = new List<double>(atributes.auxMAF2);
            atributes.data3f = new List<double>(atributes.auxMAF3);
            atributes.data4f = new List<double>(atributes.auxMAF4);
            atributes.data5f = new List<double>(atributes.auxMAF5);
            atributes.data6f = new List<double>(atributes.auxMAF6);
            atributes.data7f = new List<double>(atributes.auxMAF7);
            atributes.data8f = new List<double>(atributes.auxMAF8);
            atributes.data9f = new List<double>(atributes.auxMAF9);
            atributes.data10f = new List<double>(atributes.auxMAF10);
            atributes.data11f = new List<double>(atributes.auxMAF11);
            atributes.data12f = new List<double>(atributes.auxMAF12);

            atributes.raw1f = new List<double>(atributes.rawauxMAF1);
            atributes.raw2f = new List<double>(atributes.rawauxMAF2);
            atributes.raw3f = new List<double>(atributes.rawauxMAF3);
            atributes.raw4f = new List<double>(atributes.rawauxMAF4);
            atributes.raw5f = new List<double>(atributes.rawauxMAF5);
            atributes.raw6f = new List<double>(atributes.rawauxMAF6);
            atributes.raw7f = new List<double>(atributes.rawauxMAF7);
            atributes.raw8f = new List<double>(atributes.rawauxMAF8);
            atributes.raw9f = new List<double>(atributes.rawauxMAF9);
            atributes.raw10f = new List<double>(atributes.rawauxMAF10);
            atributes.raw11f = new List<double>(atributes.rawauxMAF11);
            atributes.raw12f = new List<double>(atributes.rawauxMAF12);

            atributes.herMod = atributes.previousHertzs;
            atributes.firstMAF = true;
            button6.Enabled = false;
        }

        private void savePreviousMMF()
        {
            atributes.auxMMF1 = new List<double>(atributes.data1f);
            atributes.auxMMF2 = new List<double>(atributes.data2f);
            atributes.auxMMF3 = new List<double>(atributes.data3f);
            atributes.auxMMF4 = new List<double>(atributes.data4f);
            atributes.auxMMF5 = new List<double>(atributes.data5f);
            atributes.auxMMF6 = new List<double>(atributes.data6f);
            atributes.auxMMF7 = new List<double>(atributes.data7f);
            atributes.auxMMF8 = new List<double>(atributes.data8f);
            atributes.auxMMF9 = new List<double>(atributes.data9f);
            atributes.auxMMF10 = new List<double>(atributes.data10f);
            atributes.auxMMF11 = new List<double>(atributes.data11f);
            atributes.auxMMF12 = new List<double>(atributes.data12f);

            atributes.rawauxMMF1 = new List<double>(atributes.raw1f);
            atributes.rawauxMMF2 = new List<double>(atributes.raw2f);
            atributes.rawauxMMF3 = new List<double>(atributes.raw3f);
            atributes.rawauxMMF4 = new List<double>(atributes.raw4f);
            atributes.rawauxMMF5 = new List<double>(atributes.raw5f);
            atributes.rawauxMMF6 = new List<double>(atributes.raw6f);
            atributes.rawauxMMF7 = new List<double>(atributes.raw7f);
            atributes.rawauxMMF8 = new List<double>(atributes.raw8f);
            atributes.rawauxMMF9 = new List<double>(atributes.raw9f);
            atributes.rawauxMMF10 = new List<double>(atributes.raw10f);
            atributes.rawauxMMF11 = new List<double>(atributes.raw11f);
            atributes.rawauxMMF12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void restorePreviousMMF()
        {
            atributes.data1f = new List<double>(atributes.auxMMF1);
            atributes.data2f = new List<double>(atributes.auxMMF2);
            atributes.data3f = new List<double>(atributes.auxMMF3);
            atributes.data4f = new List<double>(atributes.auxMMF4);
            atributes.data5f = new List<double>(atributes.auxMMF5);
            atributes.data6f = new List<double>(atributes.auxMMF6);
            atributes.data7f = new List<double>(atributes.auxMMF7);
            atributes.data8f = new List<double>(atributes.auxMMF8);
            atributes.data9f = new List<double>(atributes.auxMMF9);
            atributes.data10f = new List<double>(atributes.auxMMF10);
            atributes.data11f = new List<double>(atributes.auxMMF11);
            atributes.data12f = new List<double>(atributes.auxMMF12);

            atributes.raw1f = new List<double>(atributes.rawauxMMF1);
            atributes.raw2f = new List<double>(atributes.rawauxMMF2);
            atributes.raw3f = new List<double>(atributes.rawauxMMF3);
            atributes.raw4f = new List<double>(atributes.rawauxMMF4);
            atributes.raw5f = new List<double>(atributes.rawauxMMF5);
            atributes.raw6f = new List<double>(atributes.rawauxMMF6);
            atributes.raw7f = new List<double>(atributes.rawauxMMF7);
            atributes.raw8f = new List<double>(atributes.rawauxMMF8);
            atributes.raw9f = new List<double>(atributes.rawauxMMF9);
            atributes.raw10f = new List<double>(atributes.rawauxMMF10);
            atributes.raw11f = new List<double>(atributes.rawauxMMF11);
            atributes.raw12f = new List<double>(atributes.rawauxMMF12);

            atributes.herMod = atributes.previousHertzs;
            button8.Enabled = false;
        }

        private void savePreviousBSF()
        {
            atributes.auxBSF1 = new List<double>(atributes.data1f);
            atributes.auxBSF2 = new List<double>(atributes.data2f);
            atributes.auxBSF3 = new List<double>(atributes.data3f);
            atributes.auxBSF4 = new List<double>(atributes.data4f);
            atributes.auxBSF5 = new List<double>(atributes.data5f);
            atributes.auxBSF6 = new List<double>(atributes.data6f);
            atributes.auxBSF7 = new List<double>(atributes.data7f);
            atributes.auxBSF8 = new List<double>(atributes.data8f);
            atributes.auxBSF9 = new List<double>(atributes.data9f);
            atributes.auxBSF10 = new List<double>(atributes.data10f);
            atributes.auxBSF11 = new List<double>(atributes.data11f);
            atributes.auxBSF12 = new List<double>(atributes.data12f);

            atributes.rawauxBSF1 = new List<double>(atributes.raw1f);
            atributes.rawauxBSF2 = new List<double>(atributes.raw2f);
            atributes.rawauxBSF3 = new List<double>(atributes.raw3f);
            atributes.rawauxBSF4 = new List<double>(atributes.raw4f);
            atributes.rawauxBSF5 = new List<double>(atributes.raw5f);
            atributes.rawauxBSF6 = new List<double>(atributes.raw6f);
            atributes.rawauxBSF7 = new List<double>(atributes.raw7f);
            atributes.rawauxBSF8 = new List<double>(atributes.raw8f);
            atributes.rawauxBSF9 = new List<double>(atributes.raw9f);
            atributes.rawauxBSF10 = new List<double>(atributes.raw10f);
            atributes.rawauxBSF11 = new List<double>(atributes.raw11f);
            atributes.rawauxBSF12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void restorePreviousBSF()
        {
            atributes.data1f = new List<double>(atributes.auxBSF1);
            atributes.data2f = new List<double>(atributes.auxBSF2);
            atributes.data3f = new List<double>(atributes.auxBSF3);
            atributes.data4f = new List<double>(atributes.auxBSF4);
            atributes.data5f = new List<double>(atributes.auxBSF5);
            atributes.data6f = new List<double>(atributes.auxBSF6);
            atributes.data7f = new List<double>(atributes.auxBSF7);
            atributes.data8f = new List<double>(atributes.auxBSF8);
            atributes.data9f = new List<double>(atributes.auxBSF9);
            atributes.data10f = new List<double>(atributes.auxBSF10);
            atributes.data11f = new List<double>(atributes.auxBSF11);
            atributes.data12f = new List<double>(atributes.auxBSF12);

            atributes.raw1f = new List<double>(atributes.rawauxBSF1);
            atributes.raw2f = new List<double>(atributes.rawauxBSF2);
            atributes.raw3f = new List<double>(atributes.rawauxBSF3);
            atributes.raw4f = new List<double>(atributes.rawauxBSF4);
            atributes.raw5f = new List<double>(atributes.rawauxBSF5);
            atributes.raw6f = new List<double>(atributes.rawauxBSF6);
            atributes.raw7f = new List<double>(atributes.rawauxBSF7);
            atributes.raw8f = new List<double>(atributes.rawauxBSF8);
            atributes.raw9f = new List<double>(atributes.rawauxBSF9);
            atributes.raw10f = new List<double>(atributes.rawauxBSF10);
            atributes.raw11f = new List<double>(atributes.rawauxBSF11);
            atributes.raw12f = new List<double>(atributes.rawauxBSF12);

            atributes.herMod = atributes.previousHertzs;
            button10.Enabled = false;
        }

        private void savePreviousSUB()
        {
            atributes.auxSUB1 = new List<double>(atributes.data1f);
            atributes.auxSUB2 = new List<double>(atributes.data2f);
            atributes.auxSUB3 = new List<double>(atributes.data3f);
            atributes.auxSUB4 = new List<double>(atributes.data4f);
            atributes.auxSUB5 = new List<double>(atributes.data5f);
            atributes.auxSUB6 = new List<double>(atributes.data6f);
            atributes.auxSUB7 = new List<double>(atributes.data7f);
            atributes.auxSUB8 = new List<double>(atributes.data8f);
            atributes.auxSUB9 = new List<double>(atributes.data9f);
            atributes.auxSUB10 = new List<double>(atributes.data10f);
            atributes.auxSUB11 = new List<double>(atributes.data11f);
            atributes.auxSUB12 = new List<double>(atributes.data12f);

            atributes.rawauxSUB1 = new List<double>(atributes.raw1f);
            atributes.rawauxSUB2 = new List<double>(atributes.raw2f);
            atributes.rawauxSUB3 = new List<double>(atributes.raw3f);
            atributes.rawauxSUB4 = new List<double>(atributes.raw4f);
            atributes.rawauxSUB5 = new List<double>(atributes.raw5f);
            atributes.rawauxSUB6 = new List<double>(atributes.raw6f);
            atributes.rawauxSUB7 = new List<double>(atributes.raw7f);
            atributes.rawauxSUB8 = new List<double>(atributes.raw8f);
            atributes.rawauxSUB9 = new List<double>(atributes.raw9f);
            atributes.rawauxSUB10 = new List<double>(atributes.raw10f);
            atributes.rawauxSUB11 = new List<double>(atributes.raw11f);
            atributes.rawauxSUB12 = new List<double>(atributes.raw12f);

            atributes.previousHertzs = atributes.herMod;
        }

        private void restorePreviousSUB()
        {
            atributes.data1f = new List<double>(atributes.auxSUB1);
            atributes.data2f = new List<double>(atributes.auxSUB2);
            atributes.data3f = new List<double>(atributes.auxSUB3);
            atributes.data4f = new List<double>(atributes.auxSUB4);
            atributes.data5f = new List<double>(atributes.auxSUB5);
            atributes.data6f = new List<double>(atributes.auxSUB6);
            atributes.data7f = new List<double>(atributes.auxSUB7);
            atributes.data8f = new List<double>(atributes.auxSUB8);
            atributes.data9f = new List<double>(atributes.auxSUB9);
            atributes.data10f = new List<double>(atributes.auxSUB10);
            atributes.data11f = new List<double>(atributes.auxSUB11);
            atributes.data12f = new List<double>(atributes.auxSUB12);

            atributes.raw1f = new List<double>(atributes.rawauxSUB1);
            atributes.raw2f = new List<double>(atributes.rawauxSUB2);
            atributes.raw3f = new List<double>(atributes.rawauxSUB3);
            atributes.raw4f = new List<double>(atributes.rawauxSUB4);
            atributes.raw5f = new List<double>(atributes.rawauxSUB5);
            atributes.raw6f = new List<double>(atributes.rawauxSUB6);
            atributes.raw7f = new List<double>(atributes.rawauxSUB7);
            atributes.raw8f = new List<double>(atributes.rawauxSUB8);
            atributes.raw9f = new List<double>(atributes.rawauxSUB9);
            atributes.raw10f = new List<double>(atributes.rawauxSUB10);
            atributes.raw11f = new List<double>(atributes.rawauxSUB11);
            atributes.raw12f = new List<double>(atributes.rawauxSUB12);

            atributes.herMod = atributes.previousHertzs;
            button13.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (atributes.firstFAF)
            {
                savePreviousFAF();
                atributes.firstFAF = false;
            }
            if(trackBar1.Value > 1)
            {
                atributes.data1f = Filters.FixedAverageFilter(atributes.data1f, trackBar1.Value);
                atributes.data2f = Filters.FixedAverageFilter(atributes.data2f, trackBar1.Value);
                atributes.data3f = Filters.FixedAverageFilter(atributes.data3f, trackBar1.Value);
                atributes.data4f = Filters.FixedAverageFilter(atributes.data4f, trackBar1.Value);
                atributes.data5f = Filters.FixedAverageFilter(atributes.data5f, trackBar1.Value);
                atributes.data6f = Filters.FixedAverageFilter(atributes.data6f, trackBar1.Value);
                atributes.data7f = Filters.FixedAverageFilter(atributes.data7f, trackBar1.Value);
                atributes.data8f = Filters.FixedAverageFilter(atributes.data8f, trackBar1.Value);
                atributes.data9f = Filters.FixedAverageFilter(atributes.data9f, trackBar1.Value);
                atributes.data10f = Filters.FixedAverageFilter(atributes.data10f, trackBar1.Value);
                atributes.data11f = Filters.FixedAverageFilter(atributes.data11f, trackBar1.Value);
                atributes.data12f = Filters.FixedAverageFilter(atributes.data12f, trackBar1.Value);

                atributes.raw1f = Filters.FixedAverageFilter(atributes.raw1f, trackBar1.Value);
                atributes.raw2f = Filters.FixedAverageFilter(atributes.raw2f, trackBar1.Value);
                atributes.raw3f = Filters.FixedAverageFilter(atributes.raw3f, trackBar1.Value);
                atributes.raw4f = Filters.FixedAverageFilter(atributes.raw4f, trackBar1.Value);
                atributes.raw5f = Filters.FixedAverageFilter(atributes.raw5f, trackBar1.Value);
                atributes.raw6f = Filters.FixedAverageFilter(atributes.raw6f, trackBar1.Value);
                atributes.raw7f = Filters.FixedAverageFilter(atributes.raw7f, trackBar1.Value);
                atributes.raw8f = Filters.FixedAverageFilter(atributes.raw8f, trackBar1.Value);
                atributes.raw9f = Filters.FixedAverageFilter(atributes.raw9f, trackBar1.Value);
                atributes.raw10f = Filters.FixedAverageFilter(atributes.raw10f, trackBar1.Value);
                atributes.raw11f = Filters.FixedAverageFilter(atributes.raw11f, trackBar1.Value);
                atributes.raw12f = Filters.FixedAverageFilter(atributes.raw12f, trackBar1.Value);

                atributes.finFAF1 = new List<double>(atributes.data1f);
                atributes.finFAF2 = new List<double>(atributes.data2f);
                atributes.finFAF3 = new List<double>(atributes.data3f);
                atributes.finFAF4 = new List<double>(atributes.data4f);
                atributes.finFAF5 = new List<double>(atributes.data5f);
                atributes.finFAF6 = new List<double>(atributes.data6f);
                atributes.finFAF7 = new List<double>(atributes.data7f);
                atributes.finFAF8 = new List<double>(atributes.data8f);
                atributes.finFAF9 = new List<double>(atributes.data9f);
                atributes.finFAF10 = new List<double>(atributes.data10f);
                atributes.finFAF11 = new List<double>(atributes.data11f);
                atributes.finFAF12 = new List<double>(atributes.data12f);

                atributes.rawfinFAF1 = new List<double>(atributes.raw1f);
                atributes.rawfinFAF2 = new List<double>(atributes.raw2f);
                atributes.rawfinFAF3 = new List<double>(atributes.raw3f);
                atributes.rawfinFAF4 = new List<double>(atributes.raw4f);
                atributes.rawfinFAF5 = new List<double>(atributes.raw5f);
                atributes.rawfinFAF6 = new List<double>(atributes.raw6f);
                atributes.rawfinFAF7 = new List<double>(atributes.raw7f);
                atributes.rawfinFAF8 = new List<double>(atributes.raw8f);
                atributes.rawfinFAF9 = new List<double>(atributes.raw9f);
                atributes.rawfinFAF10 = new List<double>(atributes.raw10f);
                atributes.rawfinFAF11 = new List<double>(atributes.raw11f);
                atributes.rawfinFAF12 = new List<double>(atributes.raw12f);

                atributes.herMod = atributes.herMod / trackBar1.Value;
                label14.Text = atributes.herMod.ToString();

                atributes.blockSize = Math.Round((2 * atributes.herMod) * (1.0 / (double)atributes.herMod), 1);
                drawECG();
                comboBox1.Items.Add("A");
                comboBox2.Items.Add("A");
                button5.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            restorePreviousFAF();
            label14.Text = atributes.herMod.ToString();

            atributes.blockSize = Math.Round((2 * atributes.herMod) * (1.0 / (double)atributes.herMod), 1);
            drawECG();
            comboBox1.Items.Remove("A");
            comboBox2.Items.Remove("A");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (atributes.firstMAF)
            {
                savePreviousMAF();
                atributes.firstMAF = false;
            }
            if (trackBar2.Value > 1)
            {
                atributes.data1f = Filters.MovingAverageFilter(atributes.data1f, trackBar2.Value);
                atributes.data2f = Filters.MovingAverageFilter(atributes.data2f, trackBar2.Value);
                atributes.data3f = Filters.MovingAverageFilter(atributes.data3f, trackBar2.Value);
                atributes.data4f = Filters.MovingAverageFilter(atributes.data4f, trackBar2.Value);
                atributes.data5f = Filters.MovingAverageFilter(atributes.data5f, trackBar2.Value);
                atributes.data6f = Filters.MovingAverageFilter(atributes.data6f, trackBar2.Value);
                atributes.data7f = Filters.MovingAverageFilter(atributes.data7f, trackBar2.Value);
                atributes.data8f = Filters.MovingAverageFilter(atributes.data8f, trackBar2.Value);
                atributes.data9f = Filters.MovingAverageFilter(atributes.data9f, trackBar2.Value);
                atributes.data10f = Filters.MovingAverageFilter(atributes.data10f, trackBar2.Value);
                atributes.data11f = Filters.MovingAverageFilter(atributes.data11f, trackBar2.Value);
                atributes.data12f = Filters.MovingAverageFilter(atributes.data12f, trackBar2.Value);

                atributes.raw1f = Filters.MovingAverageFilter(atributes.raw1f, trackBar2.Value);
                atributes.raw2f = Filters.MovingAverageFilter(atributes.raw2f, trackBar2.Value);
                atributes.raw3f = Filters.MovingAverageFilter(atributes.raw3f, trackBar2.Value);
                atributes.raw4f = Filters.MovingAverageFilter(atributes.raw4f, trackBar2.Value);
                atributes.raw5f = Filters.MovingAverageFilter(atributes.raw5f, trackBar2.Value);
                atributes.raw6f = Filters.MovingAverageFilter(atributes.raw6f, trackBar2.Value);
                atributes.raw7f = Filters.MovingAverageFilter(atributes.raw7f, trackBar2.Value);
                atributes.raw8f = Filters.MovingAverageFilter(atributes.raw8f, trackBar2.Value);
                atributes.raw9f = Filters.MovingAverageFilter(atributes.raw9f, trackBar2.Value);
                atributes.raw10f = Filters.MovingAverageFilter(atributes.raw10f, trackBar2.Value);
                atributes.raw11f = Filters.MovingAverageFilter(atributes.raw11f, trackBar2.Value);
                atributes.raw12f = Filters.MovingAverageFilter(atributes.raw12f, trackBar2.Value);

                atributes.finMAF1 = new List<double>(atributes.data1f);
                atributes.finMAF2 = new List<double>(atributes.data2f);
                atributes.finMAF3 = new List<double>(atributes.data3f);
                atributes.finMAF4 = new List<double>(atributes.data4f);
                atributes.finMAF5 = new List<double>(atributes.data5f);
                atributes.finMAF6 = new List<double>(atributes.data6f);
                atributes.finMAF7 = new List<double>(atributes.data7f);
                atributes.finMAF8 = new List<double>(atributes.data8f);
                atributes.finMAF9 = new List<double>(atributes.data9f);
                atributes.finMAF10 = new List<double>(atributes.data10f);
                atributes.finMAF11 = new List<double>(atributes.data11f);
                atributes.finMAF12 = new List<double>(atributes.data12f);

                atributes.rawfinMAF1 = new List<double>(atributes.raw1f);
                atributes.rawfinMAF2 = new List<double>(atributes.raw2f);
                atributes.rawfinMAF3 = new List<double>(atributes.raw3f);
                atributes.rawfinMAF4 = new List<double>(atributes.raw4f);
                atributes.rawfinMAF5 = new List<double>(atributes.raw5f);
                atributes.rawfinMAF6 = new List<double>(atributes.raw6f);
                atributes.rawfinMAF7 = new List<double>(atributes.raw7f);
                atributes.rawfinMAF8 = new List<double>(atributes.raw8f);
                atributes.rawfinMAF9 = new List<double>(atributes.raw9f);
                atributes.rawfinMAF10 = new List<double>(atributes.raw10f);
                atributes.rawfinMAF11 = new List<double>(atributes.raw11f);
                atributes.rawfinMAF12 = new List<double>(atributes.raw12f);

                drawECG();
                comboBox1.Items.Add("B");
                comboBox2.Items.Add("B");
                button6.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            restorePreviousMAF();
            drawECG();
            comboBox1.Items.Remove("B");
            comboBox2.Items.Remove("B");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (atributes.firstMMF)
            {
                savePreviousMMF();
                atributes.firstMMF = false;
            }
            if (trackBar3.Value > 1)
            {
                atributes.data1f = Filters.MovingAverageFilter(atributes.data1f, trackBar3.Value);
                atributes.data2f = Filters.MovingAverageFilter(atributes.data2f, trackBar3.Value);
                atributes.data3f = Filters.MovingAverageFilter(atributes.data3f, trackBar3.Value);
                atributes.data4f = Filters.MovingAverageFilter(atributes.data4f, trackBar3.Value);
                atributes.data5f = Filters.MovingAverageFilter(atributes.data5f, trackBar3.Value);
                atributes.data6f = Filters.MovingAverageFilter(atributes.data6f, trackBar3.Value);
                atributes.data7f = Filters.MovingAverageFilter(atributes.data7f, trackBar3.Value);
                atributes.data8f = Filters.MovingAverageFilter(atributes.data8f, trackBar3.Value);
                atributes.data9f = Filters.MovingAverageFilter(atributes.data9f, trackBar3.Value);
                atributes.data10f = Filters.MovingAverageFilter(atributes.data10f, trackBar3.Value);
                atributes.data11f = Filters.MovingAverageFilter(atributes.data11f, trackBar3.Value);
                atributes.data12f = Filters.MovingAverageFilter(atributes.data12f, trackBar3.Value);

                atributes.raw1f = Filters.MovingAverageFilter(atributes.raw1f, trackBar3.Value);
                atributes.raw2f = Filters.MovingAverageFilter(atributes.raw2f, trackBar3.Value);
                atributes.raw3f = Filters.MovingAverageFilter(atributes.raw3f, trackBar3.Value);
                atributes.raw4f = Filters.MovingAverageFilter(atributes.raw4f, trackBar3.Value);
                atributes.raw5f = Filters.MovingAverageFilter(atributes.raw5f, trackBar3.Value);
                atributes.raw6f = Filters.MovingAverageFilter(atributes.raw6f, trackBar3.Value);
                atributes.raw7f = Filters.MovingAverageFilter(atributes.raw7f, trackBar3.Value);
                atributes.raw8f = Filters.MovingAverageFilter(atributes.raw8f, trackBar3.Value);
                atributes.raw9f = Filters.MovingAverageFilter(atributes.raw9f, trackBar3.Value);
                atributes.raw10f = Filters.MovingAverageFilter(atributes.raw10f, trackBar3.Value);
                atributes.raw11f = Filters.MovingAverageFilter(atributes.raw11f, trackBar3.Value);
                atributes.raw12f = Filters.MovingAverageFilter(atributes.raw12f, trackBar3.Value);

                atributes.finMMF1 = new List<double>(atributes.data1f);
                atributes.finMMF2 = new List<double>(atributes.data2f);
                atributes.finMMF3 = new List<double>(atributes.data3f);
                atributes.finMMF4 = new List<double>(atributes.data4f);
                atributes.finMMF5 = new List<double>(atributes.data5f);
                atributes.finMMF6 = new List<double>(atributes.data6f);
                atributes.finMMF7 = new List<double>(atributes.data7f);
                atributes.finMMF8 = new List<double>(atributes.data8f);
                atributes.finMMF9 = new List<double>(atributes.data9f);
                atributes.finMMF10 = new List<double>(atributes.data10f);
                atributes.finMMF11 = new List<double>(atributes.data11f);
                atributes.finMMF12 = new List<double>(atributes.data12f);

                atributes.rawfinMMF1 = new List<double>(atributes.raw1f);
                atributes.rawfinMMF2 = new List<double>(atributes.raw2f);
                atributes.rawfinMMF3 = new List<double>(atributes.raw3f);
                atributes.rawfinMMF4 = new List<double>(atributes.raw4f);
                atributes.rawfinMMF5 = new List<double>(atributes.raw5f);
                atributes.rawfinMMF6 = new List<double>(atributes.raw6f);
                atributes.rawfinMMF7 = new List<double>(atributes.raw7f);
                atributes.rawfinMMF8 = new List<double>(atributes.raw8f);
                atributes.rawfinMMF9 = new List<double>(atributes.raw9f);
                atributes.rawfinMMF10 = new List<double>(atributes.raw10f);
                atributes.rawfinMMF11 = new List<double>(atributes.raw11f);
                atributes.rawfinMMF12 = new List<double>(atributes.raw12f);

                drawECG();
                comboBox1.Items.Add("C");
                comboBox2.Items.Add("C");
                button8.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            restorePreviousMMF();
            drawECG();
            comboBox1.Items.Remove("C");
            comboBox2.Items.Remove("C");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (atributes.firstBSF)
            {
                savePreviousBSF();
                atributes.firstBSF = false;
            }
            if (trackBar4.Value > 0)
            {
                atributes.data1f = Filters.BandRejectionFilter(atributes.data1f, atributes.herMod, trackBar4.Value, 10);
                atributes.data2f = Filters.BandRejectionFilter(atributes.data2f, atributes.herMod, trackBar4.Value, 10);
                atributes.data3f = Filters.BandRejectionFilter(atributes.data3f, atributes.herMod, trackBar4.Value, 10);
                atributes.data4f = Filters.BandRejectionFilter(atributes.data4f, atributes.herMod, trackBar4.Value, 10);
                atributes.data5f = Filters.BandRejectionFilter(atributes.data5f, atributes.herMod, trackBar4.Value, 10);
                atributes.data6f = Filters.BandRejectionFilter(atributes.data6f, atributes.herMod, trackBar4.Value, 10);
                atributes.data7f = Filters.BandRejectionFilter(atributes.data7f, atributes.herMod, trackBar4.Value, 10);
                atributes.data8f = Filters.BandRejectionFilter(atributes.data8f, atributes.herMod, trackBar4.Value, 10);
                atributes.data9f = Filters.BandRejectionFilter(atributes.data9f, atributes.herMod, trackBar4.Value, 10);
                atributes.data10f = Filters.BandRejectionFilter(atributes.data10f, atributes.herMod, trackBar4.Value, 10);
                atributes.data11f = Filters.BandRejectionFilter(atributes.data11f, atributes.herMod, trackBar4.Value, 10);
                atributes.data12f = Filters.BandRejectionFilter(atributes.data12f, atributes.herMod, trackBar4.Value, 10);

                atributes.raw1f = Filters.BandRejectionFilter(atributes.raw1f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw2f = Filters.BandRejectionFilter(atributes.raw2f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw3f = Filters.BandRejectionFilter(atributes.raw3f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw4f = Filters.BandRejectionFilter(atributes.raw4f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw5f = Filters.BandRejectionFilter(atributes.raw5f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw6f = Filters.BandRejectionFilter(atributes.raw6f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw7f = Filters.BandRejectionFilter(atributes.raw7f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw8f = Filters.BandRejectionFilter(atributes.raw8f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw9f = Filters.BandRejectionFilter(atributes.raw9f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw10f = Filters.BandRejectionFilter(atributes.raw10f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw11f = Filters.BandRejectionFilter(atributes.raw11f, atributes.herMod, trackBar4.Value, 10);
                atributes.raw12f = Filters.BandRejectionFilter(atributes.raw12f, atributes.herMod, trackBar4.Value, 10);

                atributes.finBSF1 = new List<double>(atributes.data1f);
                atributes.finBSF2 = new List<double>(atributes.data2f);
                atributes.finBSF3 = new List<double>(atributes.data3f);
                atributes.finBSF4 = new List<double>(atributes.data4f);
                atributes.finBSF5 = new List<double>(atributes.data5f);
                atributes.finBSF6 = new List<double>(atributes.data6f);
                atributes.finBSF7 = new List<double>(atributes.data7f);
                atributes.finBSF8 = new List<double>(atributes.data8f);
                atributes.finBSF9 = new List<double>(atributes.data9f);
                atributes.finBSF10 = new List<double>(atributes.data10f);
                atributes.finBSF11 = new List<double>(atributes.data11f);
                atributes.finBSF12 = new List<double>(atributes.data12f);

                atributes.rawfinBSF1 = new List<double>(atributes.raw1f);
                atributes.rawfinBSF2 = new List<double>(atributes.raw2f);
                atributes.rawfinBSF3 = new List<double>(atributes.raw3f);
                atributes.rawfinBSF4 = new List<double>(atributes.raw4f);
                atributes.rawfinBSF5 = new List<double>(atributes.raw5f);
                atributes.rawfinBSF6 = new List<double>(atributes.raw6f);
                atributes.rawfinBSF7 = new List<double>(atributes.raw7f);
                atributes.rawfinBSF8 = new List<double>(atributes.raw8f);
                atributes.rawfinBSF9 = new List<double>(atributes.raw9f);
                atributes.rawfinBSF10 = new List<double>(atributes.raw10f);
                atributes.rawfinBSF11 = new List<double>(atributes.raw11f);
                atributes.rawfinBSF12 = new List<double>(atributes.raw12f);

                drawECG();
                comboBox1.Items.Add("D");
                comboBox2.Items.Add("D");
                button10.Enabled = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            restorePreviousBSF();
            drawECG();
            comboBox1.Items.Remove("D");
            comboBox2.Items.Remove("D");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (atributes.firstSUB)
            {
                savePreviousSUB();
                atributes.firstSUB = false;
            }
            if (comboBox1.GetItemText(comboBox1.SelectedItem) != "" && comboBox2.GetItemText(comboBox2.SelectedItem) != "")
            {
                List<double> sub1_1 = new List<double>();
                List<double> sub1_2 = new List<double>();
                List<double> sub1_3 = new List<double>();
                List<double> sub1_4 = new List<double>();
                List<double> sub1_5 = new List<double>();
                List<double> sub1_6 = new List<double>(); 
                List<double> sub1_7 = new List<double>(); 
                List<double> sub1_8 = new List<double>(); 
                List<double> sub1_9 = new List<double>(); 
                List<double> sub1_10 = new List<double>(); 
                List<double> sub1_11 = new List<double>(); 
                List<double> sub1_12 = new List<double>();
                List<double> sub2_1 = new List<double>();
                List<double> sub2_2 = new List<double>();
                List<double> sub2_3 = new List<double>();
                List<double> sub2_4 = new List<double>();
                List<double> sub2_5 = new List<double>();
                List<double> sub2_6 = new List<double>();
                List<double> sub2_7 = new List<double>();
                List<double> sub2_8 = new List<double>();
                List<double> sub2_9 = new List<double>();
                List<double> sub2_10 = new List<double>();
                List<double> sub2_11 = new List<double>();
                List<double> sub2_12 = new List<double>();

                List<double> rawsub1_1 = new List<double>();
                List<double> rawsub1_2 = new List<double>();
                List<double> rawsub1_3 = new List<double>();
                List<double> rawsub1_4 = new List<double>();
                List<double> rawsub1_5 = new List<double>();
                List<double> rawsub1_6 = new List<double>();
                List<double> rawsub1_7 = new List<double>();
                List<double> rawsub1_8 = new List<double>();
                List<double> rawsub1_9 = new List<double>();
                List<double> rawsub1_10 = new List<double>();
                List<double> rawsub1_11 = new List<double>();
                List<double> rawsub1_12 = new List<double>();
                List<double> rawsub2_1 = new List<double>();
                List<double> rawsub2_2 = new List<double>();
                List<double> rawsub2_3 = new List<double>();
                List<double> rawsub2_4 = new List<double>();
                List<double> rawsub2_5 = new List<double>();
                List<double> rawsub2_6 = new List<double>();
                List<double> rawsub2_7 = new List<double>();
                List<double> rawsub2_8 = new List<double>();
                List<double> rawsub2_9 = new List<double>();
                List<double> rawsub2_10 = new List<double>();
                List<double> rawsub2_11 = new List<double>();
                List<double> rawsub2_12 = new List<double>();

                switch (comboBox1.GetItemText(comboBox1.SelectedItem))
                {
                    case "RAW":
                        sub1_1 = new List<double>(atributes.data1f);
                        sub1_2 = new List<double>(atributes.data2f);
                        sub1_3 = new List<double>(atributes.data3f);
                        sub1_4 = new List<double>(atributes.data4f);
                        sub1_5 = new List<double>(atributes.data5f);
                        sub1_6 = new List<double>(atributes.data6f);
                        sub1_7 = new List<double>(atributes.data7f);
                        sub1_8 = new List<double>(atributes.data8f);
                        sub1_9 = new List<double>(atributes.data9f);
                        sub1_10 = new List<double>(atributes.data10f);
                        sub1_11 = new List<double>(atributes.data11f);
                        sub1_12 = new List<double>(atributes.data12f);
                        rawsub1_1 = new List<double>(atributes.raw1f);
                        rawsub1_2 = new List<double>(atributes.raw2f);
                        rawsub1_3 = new List<double>(atributes.raw3f);
                        rawsub1_4 = new List<double>(atributes.raw4f);
                        rawsub1_5 = new List<double>(atributes.raw5f);
                        rawsub1_6 = new List<double>(atributes.raw6f);
                        rawsub1_7 = new List<double>(atributes.raw7f);
                        rawsub1_8 = new List<double>(atributes.raw8f);
                        rawsub1_9 = new List<double>(atributes.raw9f);
                        rawsub1_10 = new List<double>(atributes.raw10f);
                        rawsub1_11 = new List<double>(atributes.raw11f);
                        rawsub1_12 = new List<double>(atributes.raw12f);
                        break;
                    case "A":
                        sub1_1 = new List<double>(atributes.finFAF1);
                        sub1_2 = new List<double>(atributes.finFAF2);
                        sub1_3 = new List<double>(atributes.finFAF3);
                        sub1_4 = new List<double>(atributes.finFAF4);
                        sub1_5 = new List<double>(atributes.finFAF5);
                        sub1_6 = new List<double>(atributes.finFAF6);
                        sub1_7 = new List<double>(atributes.finFAF7);
                        sub1_8 = new List<double>(atributes.finFAF8);
                        sub1_9 = new List<double>(atributes.finFAF9);
                        sub1_10 = new List<double>(atributes.finFAF10);
                        sub1_11 = new List<double>(atributes.finFAF11);
                        sub1_12 = new List<double>(atributes.finFAF12);
                        rawsub1_1 = new List<double>(atributes.rawfinFAF1);
                        rawsub1_2 = new List<double>(atributes.rawfinFAF2);
                        rawsub1_3 = new List<double>(atributes.rawfinFAF3);
                        rawsub1_4 = new List<double>(atributes.rawfinFAF4);
                        rawsub1_5 = new List<double>(atributes.rawfinFAF5);
                        rawsub1_6 = new List<double>(atributes.rawfinFAF6);
                        rawsub1_7 = new List<double>(atributes.rawfinFAF7);
                        rawsub1_8 = new List<double>(atributes.rawfinFAF8);
                        rawsub1_9 = new List<double>(atributes.rawfinFAF9);
                        rawsub1_10 = new List<double>(atributes.rawfinFAF10);
                        rawsub1_11 = new List<double>(atributes.rawfinFAF11);
                        rawsub1_12 = new List<double>(atributes.rawfinFAF12);
                        break;
                    case "B":
                        sub1_1 = new List<double>(atributes.finMAF1);
                        sub1_2 = new List<double>(atributes.finMAF2);
                        sub1_3 = new List<double>(atributes.finMAF3);
                        sub1_4 = new List<double>(atributes.finMAF4);
                        sub1_5 = new List<double>(atributes.finMAF5);
                        sub1_6 = new List<double>(atributes.finMAF6);
                        sub1_7 = new List<double>(atributes.finMAF7);
                        sub1_8 = new List<double>(atributes.finMAF8);
                        sub1_9 = new List<double>(atributes.finMAF9);
                        sub1_10 = new List<double>(atributes.finMAF10);
                        sub1_11 = new List<double>(atributes.finMAF11);
                        sub1_12 = new List<double>(atributes.finMAF12);
                        rawsub1_1 = new List<double>(atributes.rawfinMAF1);
                        rawsub1_2 = new List<double>(atributes.rawfinMAF2);
                        rawsub1_3 = new List<double>(atributes.rawfinMAF3);
                        rawsub1_4 = new List<double>(atributes.rawfinMAF4);
                        rawsub1_5 = new List<double>(atributes.rawfinMAF5);
                        rawsub1_6 = new List<double>(atributes.rawfinMAF6);
                        rawsub1_7 = new List<double>(atributes.rawfinMAF7);
                        rawsub1_8 = new List<double>(atributes.rawfinMAF8);
                        rawsub1_9 = new List<double>(atributes.rawfinMAF9);
                        rawsub1_10 = new List<double>(atributes.rawfinMAF10);
                        rawsub1_11 = new List<double>(atributes.rawfinMAF11);
                        rawsub1_12 = new List<double>(atributes.rawfinMAF12);
                        break;
                    case "C":
                        sub1_1 = new List<double>(atributes.finMMF1);
                        sub1_2 = new List<double>(atributes.finMMF2);
                        sub1_3 = new List<double>(atributes.finMMF3);
                        sub1_4 = new List<double>(atributes.finMMF4);
                        sub1_5 = new List<double>(atributes.finMMF5);
                        sub1_6 = new List<double>(atributes.finMMF6);
                        sub1_7 = new List<double>(atributes.finMMF7);
                        sub1_8 = new List<double>(atributes.finMMF8);
                        sub1_9 = new List<double>(atributes.finMMF9);
                        sub1_10 = new List<double>(atributes.finMMF10);
                        sub1_11 = new List<double>(atributes.finMMF11);
                        sub1_12 = new List<double>(atributes.finMMF12);
                        rawsub1_1 = new List<double>(atributes.rawfinMMF1);
                        rawsub1_2 = new List<double>(atributes.rawfinMMF2);
                        rawsub1_3 = new List<double>(atributes.rawfinMMF3);
                        rawsub1_4 = new List<double>(atributes.rawfinMMF4);
                        rawsub1_5 = new List<double>(atributes.rawfinMMF5);
                        rawsub1_6 = new List<double>(atributes.rawfinMMF6);
                        rawsub1_7 = new List<double>(atributes.rawfinMMF7);
                        rawsub1_8 = new List<double>(atributes.rawfinMMF8);
                        rawsub1_9 = new List<double>(atributes.rawfinMMF9);
                        rawsub1_10 = new List<double>(atributes.rawfinMMF10);
                        rawsub1_11 = new List<double>(atributes.rawfinMMF11);
                        rawsub1_12 = new List<double>(atributes.rawfinMMF12);
                        break;
                    case "D":
                        sub1_1 = new List<double>(atributes.finBSF1);
                        sub1_2 = new List<double>(atributes.finBSF2);
                        sub1_3 = new List<double>(atributes.finBSF3);
                        sub1_4 = new List<double>(atributes.finBSF4);
                        sub1_5 = new List<double>(atributes.finBSF5);
                        sub1_6 = new List<double>(atributes.finBSF6);
                        sub1_7 = new List<double>(atributes.finBSF7);
                        sub1_8 = new List<double>(atributes.finBSF8);
                        sub1_9 = new List<double>(atributes.finBSF9);
                        sub1_10 = new List<double>(atributes.finBSF10);
                        sub1_11 = new List<double>(atributes.finBSF11);
                        sub1_12 = new List<double>(atributes.finBSF12);
                        rawsub1_1 = new List<double>(atributes.rawfinBSF1);
                        rawsub1_2 = new List<double>(atributes.rawfinBSF2);
                        rawsub1_3 = new List<double>(atributes.rawfinBSF3);
                        rawsub1_4 = new List<double>(atributes.rawfinBSF4);
                        rawsub1_5 = new List<double>(atributes.rawfinBSF5);
                        rawsub1_6 = new List<double>(atributes.rawfinBSF6);
                        rawsub1_7 = new List<double>(atributes.rawfinBSF7);
                        rawsub1_8 = new List<double>(atributes.rawfinBSF8);
                        rawsub1_9 = new List<double>(atributes.rawfinBSF9);
                        rawsub1_10 = new List<double>(atributes.rawfinBSF10);
                        rawsub1_11 = new List<double>(atributes.rawfinBSF11);
                        rawsub1_12 = new List<double>(atributes.rawfinBSF12);
                        break;
                }
                switch (comboBox2.GetItemText(comboBox2.SelectedItem))
                {
                    case "RAW":
                        sub2_1 = new List<double>(atributes.data1f);
                        sub2_2 = new List<double>(atributes.data2f);
                        sub2_3 = new List<double>(atributes.data3f);
                        sub2_4 = new List<double>(atributes.data4f);
                        sub2_5 = new List<double>(atributes.data5f);
                        sub2_6 = new List<double>(atributes.data6f);
                        sub2_7 = new List<double>(atributes.data7f);
                        sub2_8 = new List<double>(atributes.data8f);
                        sub2_9 = new List<double>(atributes.data9f);
                        sub2_10 = new List<double>(atributes.data10f);
                        sub2_11 = new List<double>(atributes.data11f);
                        sub2_12 = new List<double>(atributes.data12f);
                        rawsub2_1 = new List<double>(atributes.raw1f);
                        rawsub2_2 = new List<double>(atributes.raw2f);
                        rawsub2_3 = new List<double>(atributes.raw3f);
                        rawsub2_4 = new List<double>(atributes.raw4f);
                        rawsub2_5 = new List<double>(atributes.raw5f);
                        rawsub2_6 = new List<double>(atributes.raw6f);
                        rawsub2_7 = new List<double>(atributes.raw7f);
                        rawsub2_8 = new List<double>(atributes.raw8f);
                        rawsub2_9 = new List<double>(atributes.raw9f);
                        rawsub2_10 = new List<double>(atributes.raw10f);
                        rawsub2_11 = new List<double>(atributes.raw11f);
                        rawsub2_12 = new List<double>(atributes.raw12f);
                        break;
                    case "A":
                        sub2_1 = new List<double>(atributes.finFAF1);
                        sub2_2 = new List<double>(atributes.finFAF2);
                        sub2_3 = new List<double>(atributes.finFAF3);
                        sub2_4 = new List<double>(atributes.finFAF4);
                        sub2_5 = new List<double>(atributes.finFAF5);
                        sub2_6 = new List<double>(atributes.finFAF6);
                        sub2_7 = new List<double>(atributes.finFAF7);
                        sub2_8 = new List<double>(atributes.finFAF8);
                        sub2_9 = new List<double>(atributes.finFAF9);
                        sub2_10 = new List<double>(atributes.finFAF10);
                        sub2_11 = new List<double>(atributes.finFAF11);
                        sub2_12 = new List<double>(atributes.finFAF12);
                        rawsub2_1 = new List<double>(atributes.rawfinFAF1);
                        rawsub2_2 = new List<double>(atributes.rawfinFAF2);
                        rawsub2_3 = new List<double>(atributes.rawfinFAF3);
                        rawsub2_4 = new List<double>(atributes.rawfinFAF4);
                        rawsub2_5 = new List<double>(atributes.rawfinFAF5);
                        rawsub2_6 = new List<double>(atributes.rawfinFAF6);
                        rawsub2_7 = new List<double>(atributes.rawfinFAF7);
                        rawsub2_8 = new List<double>(atributes.rawfinFAF8);
                        rawsub2_9 = new List<double>(atributes.rawfinFAF9);
                        rawsub2_10 = new List<double>(atributes.rawfinFAF10);
                        rawsub2_11 = new List<double>(atributes.rawfinFAF11);
                        rawsub2_12 = new List<double>(atributes.rawfinFAF12);
                        break;
                    case "B":
                        sub2_1 = new List<double>(atributes.finMAF1);
                        sub2_2 = new List<double>(atributes.finMAF2);
                        sub2_3 = new List<double>(atributes.finMAF3);
                        sub2_4 = new List<double>(atributes.finMAF4);
                        sub2_5 = new List<double>(atributes.finMAF5);
                        sub2_6 = new List<double>(atributes.finMAF6);
                        sub2_7 = new List<double>(atributes.finMAF7);
                        sub2_8 = new List<double>(atributes.finMAF8);
                        sub2_9 = new List<double>(atributes.finMAF9);
                        sub2_10 = new List<double>(atributes.finMAF10);
                        sub2_11 = new List<double>(atributes.finMAF11);
                        sub2_12 = new List<double>(atributes.finMAF12);
                        rawsub2_1 = new List<double>(atributes.rawfinMAF1);
                        rawsub2_2 = new List<double>(atributes.rawfinMAF2);
                        rawsub2_3 = new List<double>(atributes.rawfinMAF3);
                        rawsub2_4 = new List<double>(atributes.rawfinMAF4);
                        rawsub2_5 = new List<double>(atributes.rawfinMAF5);
                        rawsub2_6 = new List<double>(atributes.rawfinMAF6);
                        rawsub2_7 = new List<double>(atributes.rawfinMAF7);
                        rawsub2_8 = new List<double>(atributes.rawfinMAF8);
                        rawsub2_9 = new List<double>(atributes.rawfinMAF9);
                        rawsub2_10 = new List<double>(atributes.rawfinMAF10);
                        rawsub2_11 = new List<double>(atributes.rawfinMAF11);
                        rawsub2_12 = new List<double>(atributes.rawfinMAF12);
                        break;
                    case "C":
                        sub2_1 = new List<double>(atributes.finMMF1);
                        sub2_2 = new List<double>(atributes.finMMF2);
                        sub2_3 = new List<double>(atributes.finMMF3);
                        sub2_4 = new List<double>(atributes.finMMF4);
                        sub2_5 = new List<double>(atributes.finMMF5);
                        sub2_6 = new List<double>(atributes.finMMF6);
                        sub2_7 = new List<double>(atributes.finMMF7);
                        sub2_8 = new List<double>(atributes.finMMF8);
                        sub2_9 = new List<double>(atributes.finMMF9);
                        sub2_10 = new List<double>(atributes.finMMF10);
                        sub2_11 = new List<double>(atributes.finMMF11);
                        sub2_12 = new List<double>(atributes.finMMF12);
                        rawsub2_1 = new List<double>(atributes.rawfinMMF1);
                        rawsub2_2 = new List<double>(atributes.rawfinMMF2);
                        rawsub2_3 = new List<double>(atributes.rawfinMMF3);
                        rawsub2_4 = new List<double>(atributes.rawfinMMF4);
                        rawsub2_5 = new List<double>(atributes.rawfinMMF5);
                        rawsub2_6 = new List<double>(atributes.rawfinMMF6);
                        rawsub2_7 = new List<double>(atributes.rawfinMMF7);
                        rawsub2_8 = new List<double>(atributes.rawfinMMF8);
                        rawsub2_9 = new List<double>(atributes.rawfinMMF9);
                        rawsub2_10 = new List<double>(atributes.rawfinMMF10);
                        rawsub2_11 = new List<double>(atributes.rawfinMMF11);
                        rawsub2_12 = new List<double>(atributes.rawfinMMF12);
                        break;
                    case "D":
                        sub2_1 = new List<double>(atributes.finBSF1);
                        sub2_2 = new List<double>(atributes.finBSF2);
                        sub2_3 = new List<double>(atributes.finBSF3);
                        sub2_4 = new List<double>(atributes.finBSF4);
                        sub2_5 = new List<double>(atributes.finBSF5);
                        sub2_6 = new List<double>(atributes.finBSF6);
                        sub2_7 = new List<double>(atributes.finBSF7);
                        sub2_8 = new List<double>(atributes.finBSF8);
                        sub2_9 = new List<double>(atributes.finBSF9);
                        sub2_10 = new List<double>(atributes.finBSF10);
                        sub2_11 = new List<double>(atributes.finBSF11);
                        sub2_12 = new List<double>(atributes.finBSF12);
                        rawsub2_1 = new List<double>(atributes.rawfinBSF1);
                        rawsub2_2 = new List<double>(atributes.rawfinBSF2);
                        rawsub2_3 = new List<double>(atributes.rawfinBSF3);
                        rawsub2_4 = new List<double>(atributes.rawfinBSF4);
                        rawsub2_5 = new List<double>(atributes.rawfinBSF5);
                        rawsub2_6 = new List<double>(atributes.rawfinBSF6);
                        rawsub2_7 = new List<double>(atributes.rawfinBSF7);
                        rawsub2_8 = new List<double>(atributes.rawfinBSF8);
                        rawsub2_9 = new List<double>(atributes.rawfinBSF9);
                        rawsub2_10 = new List<double>(atributes.rawfinBSF10);
                        rawsub2_11 = new List<double>(atributes.rawfinBSF11);
                        rawsub2_12 = new List<double>(atributes.rawfinBSF12);
                        break;
                }
                atributes.data1f = Filters.SubtractSignals(sub1_1, sub2_1);
                atributes.data2f = Filters.SubtractSignals(sub1_2, sub2_2);
                atributes.data3f = Filters.SubtractSignals(sub1_3, sub2_3);
                atributes.data4f = Filters.SubtractSignals(sub1_4, sub2_4);
                atributes.data5f = Filters.SubtractSignals(sub1_5, sub2_5);
                atributes.data6f = Filters.SubtractSignals(sub1_6, sub2_6);
                atributes.data7f = Filters.SubtractSignals(sub1_7, sub2_7);
                atributes.data8f = Filters.SubtractSignals(sub1_8, sub2_8);
                atributes.data9f = Filters.SubtractSignals(sub1_9, sub2_9);
                atributes.data10f = Filters.SubtractSignals(sub1_10, sub2_10);
                atributes.data11f = Filters.SubtractSignals(sub1_11, sub2_11);
                atributes.data12f = Filters.SubtractSignals(sub1_12, sub2_12);

                atributes.raw1f = Filters.SubtractSignals(rawsub1_1, rawsub2_1);
                atributes.raw2f = Filters.SubtractSignals(rawsub1_2, rawsub2_2);
                atributes.raw3f = Filters.SubtractSignals(rawsub1_3, rawsub2_3);
                atributes.raw4f = Filters.SubtractSignals(rawsub1_4, rawsub2_4);
                atributes.raw5f = Filters.SubtractSignals(rawsub1_5, rawsub2_5);
                atributes.raw6f = Filters.SubtractSignals(rawsub1_6, rawsub2_6);
                atributes.raw7f = Filters.SubtractSignals(rawsub1_7, rawsub2_7);
                atributes.raw8f = Filters.SubtractSignals(rawsub1_8, rawsub2_8);
                atributes.raw9f = Filters.SubtractSignals(rawsub1_9, rawsub2_9);
                atributes.raw10f = Filters.SubtractSignals(rawsub1_10, rawsub2_10);
                atributes.raw11f = Filters.SubtractSignals(rawsub1_11, rawsub2_11);
                atributes.raw12f = Filters.SubtractSignals(rawsub1_12, rawsub2_12);

                drawECG();
                button13.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                button12.Enabled = false;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            restorePreviousSUB();
            drawECG();
            button12.Enabled = true;
        }

        private void addLabelsAutomaticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int windowSize = atributes.herMod / 50;
            atributes.ventanaFiltroMedia = atributes.herMod / 100;

            atributes.data1f = Filters.FixedAverageFilter(atributes.data1f, atributes.ventanaFiltroMedia);
            atributes.data2f = Filters.FixedAverageFilter(atributes.data2f, atributes.ventanaFiltroMedia);
            atributes.data3f = Filters.FixedAverageFilter(atributes.data3f, atributes.ventanaFiltroMedia);
            atributes.data4f = Filters.FixedAverageFilter(atributes.data4f, atributes.ventanaFiltroMedia);
            atributes.data5f = Filters.FixedAverageFilter(atributes.data5f, atributes.ventanaFiltroMedia);
            atributes.data6f = Filters.FixedAverageFilter(atributes.data6f, atributes.ventanaFiltroMedia);
            atributes.data7f = Filters.FixedAverageFilter(atributes.data7f, atributes.ventanaFiltroMedia);
            atributes.data8f = Filters.FixedAverageFilter(atributes.data8f, atributes.ventanaFiltroMedia);
            atributes.data9f = Filters.FixedAverageFilter(atributes.data9f, atributes.ventanaFiltroMedia);
            atributes.data10f = Filters.FixedAverageFilter(atributes.data10f, atributes.ventanaFiltroMedia);
            atributes.data11f = Filters.FixedAverageFilter(atributes.data11f, atributes.ventanaFiltroMedia);
            atributes.data12f = Filters.FixedAverageFilter(atributes.data12f, atributes.ventanaFiltroMedia);

            atributes.raw1f = Filters.FixedAverageFilter(atributes.raw1f, atributes.ventanaFiltroMedia);
            atributes.raw2f = Filters.FixedAverageFilter(atributes.raw2f, atributes.ventanaFiltroMedia);
            atributes.raw3f = Filters.FixedAverageFilter(atributes.raw3f, atributes.ventanaFiltroMedia);
            atributes.raw4f = Filters.FixedAverageFilter(atributes.raw4f, atributes.ventanaFiltroMedia);
            atributes.raw5f = Filters.FixedAverageFilter(atributes.raw5f, atributes.ventanaFiltroMedia);
            atributes.raw6f = Filters.FixedAverageFilter(atributes.raw6f, atributes.ventanaFiltroMedia);
            atributes.raw7f = Filters.FixedAverageFilter(atributes.raw7f, atributes.ventanaFiltroMedia);
            atributes.raw8f = Filters.FixedAverageFilter(atributes.raw8f, atributes.ventanaFiltroMedia);
            atributes.raw9f = Filters.FixedAverageFilter(atributes.raw9f, atributes.ventanaFiltroMedia);
            atributes.raw10f = Filters.FixedAverageFilter(atributes.raw10f, atributes.ventanaFiltroMedia);
            atributes.raw11f = Filters.FixedAverageFilter(atributes.raw11f, atributes.ventanaFiltroMedia);
            atributes.raw12f = Filters.FixedAverageFilter(atributes.raw12f, atributes.ventanaFiltroMedia);

            atributes.herMod = atributes.herMod / atributes.ventanaFiltroMedia;
            label14.Text = atributes.herMod.ToString();

            List<double> filteredSignal1 = Filters.MovingAverageFilter(atributes.data1f, windowSize);
            List<double> filteredSignal2 = Filters.MovingAverageFilter(atributes.data2f, windowSize);
            List<double> filteredSignal3 = Filters.MovingAverageFilter(atributes.data3f, windowSize);
            List<double> filteredSignal4 = Filters.MovingAverageFilter(atributes.data4f, windowSize);
            List<double> filteredSignal5 = Filters.MovingAverageFilter(atributes.data5f, windowSize);
            List<double> filteredSignal6 = Filters.MovingAverageFilter(atributes.data6f, windowSize);
            List<double> filteredSignal7 = Filters.MovingAverageFilter(atributes.data7f, windowSize);
            List<double> filteredSignal8 = Filters.MovingAverageFilter(atributes.data8f, windowSize);
            List<double> filteredSignal9 = Filters.MovingAverageFilter(atributes.data9f, windowSize);
            List<double> filteredSignal10 = Filters.MovingAverageFilter(atributes.data10f, windowSize);
            List<double> filteredSignal11 = Filters.MovingAverageFilter(atributes.data11f, windowSize);
            List<double> filteredSignal12 = Filters.MovingAverageFilter(atributes.data12f, windowSize);

            List<double> rawfilteredSignal1 = Filters.MovingAverageFilter(atributes.raw1f, windowSize);
            List<double> rawfilteredSignal2 = Filters.MovingAverageFilter(atributes.raw2f, windowSize);
            List<double> rawfilteredSignal3 = Filters.MovingAverageFilter(atributes.raw3f, windowSize);
            List<double> rawfilteredSignal4 = Filters.MovingAverageFilter(atributes.raw4f, windowSize);
            List<double> rawfilteredSignal5 = Filters.MovingAverageFilter(atributes.raw5f, windowSize);
            List<double> rawfilteredSignal6 = Filters.MovingAverageFilter(atributes.raw6f, windowSize);
            List<double> rawfilteredSignal7 = Filters.MovingAverageFilter(atributes.raw7f, windowSize);
            List<double> rawfilteredSignal8 = Filters.MovingAverageFilter(atributes.raw8f, windowSize);
            List<double> rawfilteredSignal9 = Filters.MovingAverageFilter(atributes.raw9f, windowSize);
            List<double> rawfilteredSignal10 = Filters.MovingAverageFilter(atributes.raw10f, windowSize);
            List<double> rawfilteredSignal11 = Filters.MovingAverageFilter(atributes.raw11f, windowSize);
            List<double> rawfilteredSignal12 = Filters.MovingAverageFilter(atributes.raw12f, windowSize);

            List<double> ecgMedianFiltered1 = Filters.MedianFilter(filteredSignal1, windowSize);
            List<double> ecgMedianFiltered2 = Filters.MedianFilter(filteredSignal2, windowSize);
            List<double> ecgMedianFiltered3 = Filters.MedianFilter(filteredSignal3, windowSize);
            List<double> ecgMedianFiltered4 = Filters.MedianFilter(filteredSignal4, windowSize);
            List<double> ecgMedianFiltered5 = Filters.MedianFilter(filteredSignal5, windowSize);
            List<double> ecgMedianFiltered6 = Filters.MedianFilter(filteredSignal6, windowSize);
            List<double> ecgMedianFiltered7 = Filters.MedianFilter(filteredSignal7, windowSize);
            List<double> ecgMedianFiltered8 = Filters.MedianFilter(filteredSignal8, windowSize);
            List<double> ecgMedianFiltered9 = Filters.MedianFilter(filteredSignal9, windowSize);
            List<double> ecgMedianFiltered10 = Filters.MedianFilter(filteredSignal10, windowSize);
            List<double> ecgMedianFiltered11 = Filters.MedianFilter(filteredSignal11, windowSize);
            List<double> ecgMedianFiltered12 = Filters.MedianFilter(filteredSignal12, windowSize);

            List<double> rawecgMedianFiltered1 = Filters.MedianFilter(rawfilteredSignal1, windowSize);
            List<double> rawecgMedianFiltered2 = Filters.MedianFilter(rawfilteredSignal2, windowSize);
            List<double> rawecgMedianFiltered3 = Filters.MedianFilter(rawfilteredSignal3, windowSize);
            List<double> rawecgMedianFiltered4 = Filters.MedianFilter(rawfilteredSignal4, windowSize);
            List<double> rawecgMedianFiltered5 = Filters.MedianFilter(rawfilteredSignal5, windowSize);
            List<double> rawecgMedianFiltered6 = Filters.MedianFilter(rawfilteredSignal6, windowSize);
            List<double> rawecgMedianFiltered7 = Filters.MedianFilter(rawfilteredSignal7, windowSize);
            List<double> rawecgMedianFiltered8 = Filters.MedianFilter(rawfilteredSignal8, windowSize);
            List<double> rawecgMedianFiltered9 = Filters.MedianFilter(rawfilteredSignal9, windowSize);
            List<double> rawecgMedianFiltered10 = Filters.MedianFilter(rawfilteredSignal10, windowSize);
            List<double> rawecgMedianFiltered11 = Filters.MedianFilter(rawfilteredSignal11, windowSize);
            List<double> rawecgMedianFiltered12 = Filters.MedianFilter(rawfilteredSignal12, windowSize);

            atributes.data1f = Filters.SubtractSignals(atributes.data1f, ecgMedianFiltered1);
            atributes.data2f = Filters.SubtractSignals(atributes.data2f, ecgMedianFiltered2);
            atributes.data3f = Filters.SubtractSignals(atributes.data3f, ecgMedianFiltered3);
            atributes.data4f = Filters.SubtractSignals(atributes.data4f, ecgMedianFiltered4);
            atributes.data5f = Filters.SubtractSignals(atributes.data5f, ecgMedianFiltered5);
            atributes.data6f = Filters.SubtractSignals(atributes.data6f, ecgMedianFiltered6);
            atributes.data7f = Filters.SubtractSignals(atributes.data7f, ecgMedianFiltered7);
            atributes.data8f = Filters.SubtractSignals(atributes.data8f, ecgMedianFiltered8);
            atributes.data9f = Filters.SubtractSignals(atributes.data9f, ecgMedianFiltered9);
            atributes.data10f = Filters.SubtractSignals(atributes.data10f, ecgMedianFiltered10);
            atributes.data11f = Filters.SubtractSignals(atributes.data11f, ecgMedianFiltered11);
            atributes.data12f = Filters.SubtractSignals(atributes.data12f, ecgMedianFiltered12);

            atributes.raw1f = Filters.SubtractSignals(atributes.raw1f, rawecgMedianFiltered1);
            atributes.raw2f = Filters.SubtractSignals(atributes.raw2f, rawecgMedianFiltered2);
            atributes.raw3f = Filters.SubtractSignals(atributes.raw3f, rawecgMedianFiltered3);
            atributes.raw4f = Filters.SubtractSignals(atributes.raw4f, rawecgMedianFiltered4);
            atributes.raw5f = Filters.SubtractSignals(atributes.raw5f, rawecgMedianFiltered5);
            atributes.raw6f = Filters.SubtractSignals(atributes.raw6f, rawecgMedianFiltered6);
            atributes.raw7f = Filters.SubtractSignals(atributes.raw7f, rawecgMedianFiltered7);
            atributes.raw8f = Filters.SubtractSignals(atributes.raw8f, rawecgMedianFiltered8);
            atributes.raw9f = Filters.SubtractSignals(atributes.raw9f, rawecgMedianFiltered9);
            atributes.raw10f = Filters.SubtractSignals(atributes.raw10f, rawecgMedianFiltered10);
            atributes.raw11f = Filters.SubtractSignals(atributes.raw11f, rawecgMedianFiltered11);
            atributes.raw12f = Filters.SubtractSignals(atributes.raw12f, rawecgMedianFiltered12);

            atributes.data1f = Filters.BandRejectionFilter(atributes.data1f, 100, 55, 10);
            atributes.data2f = Filters.BandRejectionFilter(atributes.data2f, 100, 55, 10);
            atributes.data3f = Filters.BandRejectionFilter(atributes.data3f, 100, 55, 10);
            atributes.data4f = Filters.BandRejectionFilter(atributes.data4f, 100, 55, 10);
            atributes.data5f = Filters.BandRejectionFilter(atributes.data5f, 100, 55, 10);
            atributes.data6f = Filters.BandRejectionFilter(atributes.data6f, 100, 55, 10);
            atributes.data7f = Filters.BandRejectionFilter(atributes.data7f, 100, 55, 10);
            atributes.data8f = Filters.BandRejectionFilter(atributes.data8f, 100, 55, 10);
            atributes.data9f = Filters.BandRejectionFilter(atributes.data9f, 100, 55, 10);
            atributes.data10f = Filters.BandRejectionFilter(atributes.data10f, 100, 55, 10);
            atributes.data11f = Filters.BandRejectionFilter(atributes.data11f, 100, 55, 10);
            atributes.data12f = Filters.BandRejectionFilter(atributes.data12f, 100, 55, 10);

            atributes.raw1f = Filters.BandRejectionFilter(atributes.raw1f, 100, 55, 10);
            atributes.raw2f = Filters.BandRejectionFilter(atributes.raw2f, 100, 55, 10);
            atributes.raw3f = Filters.BandRejectionFilter(atributes.raw3f, 100, 55, 10);
            atributes.raw4f = Filters.BandRejectionFilter(atributes.raw4f, 100, 55, 10);
            atributes.raw5f = Filters.BandRejectionFilter(atributes.raw5f, 100, 55, 10);
            atributes.raw6f = Filters.BandRejectionFilter(atributes.raw6f, 100, 55, 10);
            atributes.raw7f = Filters.BandRejectionFilter(atributes.raw7f, 100, 55, 10);
            atributes.raw8f = Filters.BandRejectionFilter(atributes.raw8f, 100, 55, 10);
            atributes.raw9f = Filters.BandRejectionFilter(atributes.raw9f, 100, 55, 10);
            atributes.raw10f = Filters.BandRejectionFilter(atributes.raw10f, 100, 55, 10);
            atributes.raw11f = Filters.BandRejectionFilter(atributes.raw11f, 100, 55, 10);
            atributes.raw12f = Filters.BandRejectionFilter(atributes.raw12f, 100, 55, 10);

            atributes.blockSize = Math.Round((2 * atributes.herMod) * (1.0 / (double)atributes.herMod),1);

            drawECG();
            automaticPQRSTDetectionToolStripMenuItem.Enabled = true;
            addLabelsAutomaticallyToolStripMenuItem.Enabled = false;
            filteringToolStripMenuItem.Enabled = false;
        }

        public static IList<int> FindMaxPeaks(IList<double> values, int rangeOfPeaks)
        {
            List<int> peaks = new List<int>();
            double current;
            IEnumerable<double> range;

            int checksOnEachSide = rangeOfPeaks / 2;
            for (int i = 0; i < values.Count; i++)
            {
                current = values[i];
                range = values;

                if (i > checksOnEachSide)
                {
                    range = range.Skip(i - checksOnEachSide);
                }

                range = range.Take(rangeOfPeaks);
                if ((range.Count() > 0) && (current == range.Max()))
                {
                    peaks.Add(i);
                }
            }

            return peaks;
        }

        public static IList<int> FindMinPeaks(IList<double> values, int rangeOfPeaks)
        {
            List<int> peaks = new List<int>();
            double current;
            IEnumerable<double> range;

            int checksOnEachSide = rangeOfPeaks / 2;
            for (int i = 0; i < values.Count; i++)
            {
                current = values[i];
                range = values;

                if (i > checksOnEachSide)
                {
                    range = range.Skip(i - checksOnEachSide);
                }

                range = range.Take(rangeOfPeaks);
                if ((range.Count() > 0) && (current == range.Min()))
                {
                    peaks.Add(i);
                }
            }

            return peaks;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void asXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (atributes.lines1.Count == 0 && atributes.lines2.Count == 0 && atributes.lines3.Count == 0 && atributes.lines4.Count == 0 && atributes.lines5.Count == 0 && atributes.lines6.Count == 0 && atributes.lines7.Count == 0 && atributes.lines8.Count == 0 && atributes.lines9.Count == 0 && atributes.lines10.Count == 0 && atributes.lines11.Count == 0 && atributes.lines12.Count == 0)
            {
                MessageBox.Show("no marks recorded", "Error");
            }
            else
            {
                string s = (atributes.fileName.Replace(".XML", "")) + "_annotations.XML";
                Console.WriteLine(s);
                TextWriter txt = new StreamWriter(s);
                txt.Write("<?xml version=\"1.0\" encoding=\"ISO - 8859 - 1\" ?>\n");
                txt.Write("\t< CardiologyAnnotations >\n");
                if (atributes.lines1.Count > 0)
                {
                    txt.Write("\t\t< I >\n");
                    int i = 0;
                    foreach (StripLine sl in atributes.lines1)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot1[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ I >\n");
                }
                if (atributes.lines2.Count > 0)
                {
                    txt.Write("\t\t< II >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines2)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot2[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ II >\n");
                }
                if (atributes.lines3.Count > 0)
                {
                    txt.Write("\t\t< III >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines3)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot3[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ III >\n");
                }
                if (atributes.lines4.Count > 0)
                {
                    txt.Write("\t\t< aVR >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines4)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot4[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVR >\n");
                }
                if (atributes.lines5.Count > 0)
                {
                    txt.Write("\t\t< aVL >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines5)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot5[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVL >\n");
                }
                if (atributes.lines6.Count > 0)
                {
                    txt.Write("\t\t< aVF >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines6)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot6[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVF >\n");
                }
                if (atributes.lines7.Count > 0)
                {
                    txt.Write("\t\t< V1 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines7)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot7[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V1 >\n");
                }
                if (atributes.lines8.Count > 0)
                {
                    txt.Write("\t\t< V2 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines8)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot8[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V2 >\n");
                }
                if (atributes.lines9.Count > 0)
                {
                    txt.Write("\t\t< V3 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines9)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot9[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V3 >\n");
                }
                if (atributes.lines10.Count > 0)
                {
                    txt.Write("\t\t< V4 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines10)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot10[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V4 >\n");
                }
                if (atributes.lines11.Count > 0)
                {
                    txt.Write("\t\t< V5 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines11)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot11[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V5 >\n");
                }
                if (atributes.lines12.Count > 0)
                {
                    txt.Write("\t\t< V6 >\n");

                    int i = 0;
                    foreach (StripLine sl in atributes.lines12)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + atributes.annot12[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V6 >\n");
                }

                txt.Write("</ CardiologyAnnotations >");
                txt.Close();
            }
        }

        private void cargarECGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (atributes.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sr = new StreamReader(atributes.openFileDialog1.FileName);
                    //
                    atributes.fileName = atributes.openFileDialog1.FileName;
                    atributes.doc.Load(atributes.fileName);
                    if(atributes.archivoCargado)
                        eraseData();
                    basicData();
                    if (atributes.correcto)
                    {
                        ecgSummaryDataLoadFromFile();
                        clearECG();
                        ecgDataLoadFromFile();
                        addLabelsAutomaticallyToolStripMenuItem.Enabled = true;
                        filteringToolStripMenuItem.Enabled = true;
                        rESTORESIGNALSToolStripMenuItem.Enabled = true;
                        atributes.archivoCargado = true;
                        label18.Visible = false;
                        label19.Visible = false;
                        label20.Visible = false;
                        label21.Visible = false;
                        label22.Visible = false;

                        label14.Visible = true;
                        label15.Visible = true;
                        label16.Visible = true;
                        label17.Visible = true;
                        label23.Visible = true;
                        label24.Visible = true;

                        generateReportToolStripMenuItem.Enabled = false;
                        saveECGDataToolStripMenuItem.Enabled = true;
                        emulateNewECGToolStripMenuItem.Enabled = true;
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void modifyBasicData()
        {
            label6.Text = atributes.age_new;
            label7.Text = atributes.gender_new;
            label8.Text = atributes.race_new;
            label9.Text = atributes.height_new;
            label10.Text = atributes.weight_new;
        }

        private void basicData()
        {
            //Compruebo que es el ECG de General Electric
            try
            {
                atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/ClinicalInfo/DeviceInfo/Desc");

                string ecgtype = atributes.node.InnerText;

                if (ecgtype == "CardioSoft")
                {
                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Age");
                    atributes.age = atributes.node.InnerText;
                    atributes.age_new = atributes.node.InnerText;
                    label6.Text = atributes.age_new;
                    textBox1.Text = atributes.age_new;
                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Gender");
                    atributes.gender = atributes.node.InnerText;
                    atributes.gender_new = atributes.node.InnerText;
                    label7.Text = atributes.gender_new;
                    textBox2.Text = atributes.gender_new;
                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Race");
                    atributes.race = atributes.node.InnerText;
                    atributes.race_new = atributes.node.InnerText;
                    label8.Text = atributes.race_new;
                    textBox4.Text = atributes.race_new;
                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Height");
                    atributes.height = atributes.node.InnerText;
                    atributes.height_new = atributes.node.InnerText;
                    label9.Text = atributes.height_new;
                    textBox3.Text = atributes.height_new;
                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Weight");
                    atributes.weight = atributes.node.InnerText;
                    atributes.weight_new = atributes.node.InnerText;
                    label10.Text = atributes.weight_new;
                    textBox5.Text = atributes.weight_new;

                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/SampleRate");
                    atributes.hertz = atributes.node.InnerText;
                    atributes.hertz_new = atributes.node.InnerText;
                    label14.Text = atributes.hertz;
                    atributes.her = Convert.ToInt32(atributes.hertz);
                    atributes.herMod = atributes.her;

                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/ChannelSampleCountTotal");
                    atributes.secs = (Convert.ToInt32(atributes.node.InnerText) / atributes.herMod).ToString();
                    atributes.secs_new = atributes.secs;
                    label16.Text = atributes.secs_new;

                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/Resolution");
                    atributes.resolution = atributes.node.InnerText;
                    atributes.resolution_new = atributes.node.InnerText;
                    atributes.resol = Convert.ToInt32(atributes.resolution);
                    label24.Text = atributes.resolution;

                    groupBox1.Visible = true;

                    atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/RestingECGMeasurements/VentricularRate");

                    atributes.bmpsFile = Convert.ToDouble(atributes.node.InnerText);

                    atributes.correcto = true;
                }
                else
                {
                    atributes.correcto = false;
                    MessageBox.Show("Incorrect File");
                }
            }
            catch(Exception)
            {
                atributes.correcto = false;
                MessageBox.Show("Incorrect File");
            }

        }

        private void ecgSummaryDataLoadFromFile()
        {
            atributes.restingECGMeasurements = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/RestingECGMeasurements");
            atributes.vectorLoops = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/VectorLoops");

        }

        private void ecgDataLoadFromFile()
        {
            atributes.node = atributes.doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData");
            string[] result1 = atributes.node.ChildNodes[4].InnerText.Split(',');
            string[] result2 = atributes.node.ChildNodes[5].InnerText.Split(',');
            string[] result3 = atributes.node.ChildNodes[6].InnerText.Split(',');
            string[] result4 = atributes.node.ChildNodes[7].InnerText.Split(',');
            string[] result5 = atributes.node.ChildNodes[8].InnerText.Split(',');
            string[] result6 = atributes.node.ChildNodes[9].InnerText.Split(',');
            string[] result7 = atributes.node.ChildNodes[10].InnerText.Split(',');
            string[] result8 = atributes.node.ChildNodes[11].InnerText.Split(',');
            string[] result9 = atributes.node.ChildNodes[12].InnerText.Split(',');
            string[] result10 = atributes.node.ChildNodes[13].InnerText.Split(',');
            string[] result11 = atributes.node.ChildNodes[14].InnerText.Split(',');
            string[] result12 = atributes.node.ChildNodes[15].InnerText.Split(',');

            Tools.loadData(ref result1, ref atributes.raw1, ref atributes.raw1f, ref atributes.data1, ref atributes.data1f, ref atributes.maxr1, ref atributes.minr1, atributes.resol);
            Tools.loadData(ref result2, ref atributes.raw2, ref atributes.raw2f, ref atributes.data2, ref atributes.data2f, ref atributes.maxr2, ref atributes.minr2, atributes.resol);
            Tools.loadData(ref result3, ref atributes.raw3, ref atributes.raw3f, ref atributes.data3, ref atributes.data3f, ref atributes.maxr3, ref atributes.minr3, atributes.resol);
            Tools.loadData(ref result4, ref atributes.raw4, ref atributes.raw4f, ref atributes.data4, ref atributes.data4f, ref atributes.maxr4, ref atributes.minr4, atributes.resol);
            Tools.loadData(ref result5, ref atributes.raw5, ref atributes.raw5f, ref atributes.data5, ref atributes.data5f, ref atributes.maxr5, ref atributes.minr5, atributes.resol);
            Tools.loadData(ref result6, ref atributes.raw6, ref atributes.raw6f, ref atributes.data6, ref atributes.data6f, ref atributes.maxr6, ref atributes.minr6, atributes.resol);
            Tools.loadData(ref result7, ref atributes.raw7, ref atributes.raw7f, ref atributes.data7, ref atributes.data7f, ref atributes.maxr7, ref atributes.minr7, atributes.resol);
            Tools.loadData(ref result8, ref atributes.raw8, ref atributes.raw8f, ref atributes.data8, ref atributes.data8f, ref atributes.maxr8, ref atributes.minr8, atributes.resol);
            Tools.loadData(ref result9, ref atributes.raw9, ref atributes.raw9f, ref atributes.data9, ref atributes.data9f, ref atributes.maxr9, ref atributes.minr9, atributes.resol);
            Tools.loadData(ref result10, ref atributes.raw10, ref atributes.raw10f, ref atributes.data10, ref atributes.data10f, ref atributes.maxr10, ref atributes.minr10, atributes.resol);
            Tools.loadData(ref result11, ref atributes.raw11, ref atributes.raw11f, ref atributes.data11, ref atributes.data11f, ref atributes.maxr11, ref atributes.minr11, atributes.resol);
            Tools.loadData(ref result12, ref atributes.raw12, ref atributes.raw12f, ref atributes.data12, ref atributes.data12f, ref atributes.maxr12, ref atributes.minr12, atributes.resol);

            atributes.blockSize = Math.Round((2 * atributes.herMod) * (1.0 / (double)atributes.herMod), 1);
            drawECG();
        }

        private void eraseData()
        {
            Tools.erase(ref atributes);
        }

        private void drawECG()
        {
            clearECG();
            Tools.drawOnChart(ref chart1, ref atributes.data1f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart2, ref atributes.data2f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart3, ref atributes.data3f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart4, ref atributes.data4f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart5, ref atributes.data5f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart6, ref atributes.data6f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart7, ref atributes.data7f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart8, ref atributes.data8f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart9, ref atributes.data9f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart10, ref atributes.data10f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart11, ref atributes.data11f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);
            Tools.drawOnChart(ref chart12, ref atributes.data12f, atributes.herMod, Convert.ToDouble(atributes.secs_new), atributes.blockSize);

            groupBox2.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
        }
        private void clearECG()
        {
            Tools.clearOnChart(ref chart1);
            Tools.clearOnChart(ref chart2);
            Tools.clearOnChart(ref chart3);
            Tools.clearOnChart(ref chart4);
            Tools.clearOnChart(ref chart5);
            Tools.clearOnChart(ref chart6);
            Tools.clearOnChart(ref chart7);
            Tools.clearOnChart(ref chart8);
            Tools.clearOnChart(ref chart9);
            Tools.clearOnChart(ref chart10);
            Tools.clearOnChart(ref chart11);
            Tools.clearOnChart(ref chart12);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (atributes.blockSize >= 1)
            {
                atributes.blockSize -= 0.5;
                double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;
                drawECG();

                if (checkBox1.Visible && checkBox1.Checked)
                {
                    Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
                }
                if (checkBox2.Visible && checkBox2.Checked)
                {
                    Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
                }
                if (checkBox3.Visible && checkBox3.Checked)
                {
                    Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
                }
                if (checkBox4.Visible && checkBox4.Checked)
                {
                    Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
                }
                if (checkBox5.Visible && checkBox5.Checked)
                {
                    Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (atributes.blockSize <= 10)
            {
                atributes.blockSize += 0.5;
                double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;
                drawECG();
                if (checkBox1.Visible && checkBox1.Checked)
                {
                    Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                    Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
                }
                if (checkBox2.Visible && checkBox2.Checked)
                {
                    Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                    Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
                }
                if (checkBox3.Visible && checkBox3.Checked)
                {
                    Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                    Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
                }
                if (checkBox4.Visible && checkBox4.Checked)
                {
                    Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                    Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
                }
                if (checkBox5.Visible && checkBox5.Checked)
                {
                    Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                    Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            atributes.blockSize = 2;
            drawECG();
            double ancho = atributes.blockSize > 2 ? 1 : atributes.blockSize / 2.0;
            if (checkBox1.Visible && checkBox1.Checked)
            {
                Tools.pintarPicosP(ref chart1, ref atributes.lines1, ref atributes.picosP1, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart2, ref atributes.lines2, ref atributes.picosP2, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart3, ref atributes.lines3, ref atributes.picosP3, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart4, ref atributes.lines4, ref atributes.picosP4, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart5, ref atributes.lines5, ref atributes.picosP5, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart6, ref atributes.lines6, ref atributes.picosP6, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart7, ref atributes.lines7, ref atributes.picosP7, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart8, ref atributes.lines8, ref atributes.picosP8, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart9, ref atributes.lines9, ref atributes.picosP9, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart10, ref atributes.lines10, ref atributes.picosP10, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart11, ref atributes.lines11, ref atributes.picosP11, ancho, atributes.herMod);
                Tools.pintarPicosP(ref chart12, ref atributes.lines12, ref atributes.picosP12, ancho, atributes.herMod);
            }
            if (checkBox2.Visible && checkBox2.Checked)
            {
                Tools.pintarPicosQ(ref chart1, ref atributes.lines1, ref atributes.picosQ1, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart2, ref atributes.lines2, ref atributes.picosQ2, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart3, ref atributes.lines3, ref atributes.picosQ3, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart4, ref atributes.lines4, ref atributes.picosQ4, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart5, ref atributes.lines5, ref atributes.picosQ5, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart6, ref atributes.lines6, ref atributes.picosQ6, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart7, ref atributes.lines7, ref atributes.picosQ7, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart8, ref atributes.lines8, ref atributes.picosQ8, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart9, ref atributes.lines9, ref atributes.picosQ9, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart10, ref atributes.lines10, ref atributes.picosQ10, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart11, ref atributes.lines11, ref atributes.picosQ11, ancho, atributes.herMod);
                Tools.pintarPicosQ(ref chart12, ref atributes.lines12, ref atributes.picosQ12, ancho, atributes.herMod);
            }
            if (checkBox3.Visible && checkBox3.Checked)
            {
                Tools.pintarPicosR(ref chart1, ref atributes.lines1, ref atributes.picosR1, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart2, ref atributes.lines2, ref atributes.picosR2, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart3, ref atributes.lines3, ref atributes.picosR3, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart4, ref atributes.lines4, ref atributes.picosR4, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart5, ref atributes.lines5, ref atributes.picosR5, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart6, ref atributes.lines6, ref atributes.picosR6, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart7, ref atributes.lines7, ref atributes.picosR7, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart8, ref atributes.lines8, ref atributes.picosR8, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart9, ref atributes.lines9, ref atributes.picosR9, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart10, ref atributes.lines10, ref atributes.picosR10, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart11, ref atributes.lines11, ref atributes.picosR11, ancho, atributes.herMod);
                Tools.pintarPicosR(ref chart12, ref atributes.lines12, ref atributes.picosR12, ancho, atributes.herMod);
            }
            if (checkBox4.Visible && checkBox4.Checked)
            {
                Tools.pintarPicosS(ref chart1, ref atributes.lines1, ref atributes.picosS1, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart2, ref atributes.lines2, ref atributes.picosS2, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart3, ref atributes.lines3, ref atributes.picosS3, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart4, ref atributes.lines4, ref atributes.picosS4, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart5, ref atributes.lines5, ref atributes.picosS5, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart6, ref atributes.lines6, ref atributes.picosS6, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart7, ref atributes.lines7, ref atributes.picosS7, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart8, ref atributes.lines8, ref atributes.picosS8, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart9, ref atributes.lines9, ref atributes.picosS9, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart10, ref atributes.lines10, ref atributes.picosS10, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart11, ref atributes.lines11, ref atributes.picosS11, ancho, atributes.herMod);
                Tools.pintarPicosS(ref chart12, ref atributes.lines12, ref atributes.picosS12, ancho, atributes.herMod);
            }
            if (checkBox5.Visible && checkBox5.Checked)
            {
                Tools.pintarPicosT(ref chart1, ref atributes.lines1, ref atributes.picosT1, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart2, ref atributes.lines2, ref atributes.picosT2, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart3, ref atributes.lines3, ref atributes.picosT3, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart4, ref atributes.lines4, ref atributes.picosT4, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart5, ref atributes.lines5, ref atributes.picosT5, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart6, ref atributes.lines6, ref atributes.picosT6, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart7, ref atributes.lines7, ref atributes.picosT7, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart8, ref atributes.lines8, ref atributes.picosT8, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart9, ref atributes.lines9, ref atributes.picosT9, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart10, ref atributes.lines10, ref atributes.picosT10, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart11, ref atributes.lines11, ref atributes.picosT11, ancho, atributes.herMod);
                Tools.pintarPicosT(ref chart12, ref atributes.lines12, ref atributes.picosT12, ancho, atributes.herMod);
            }
        }

        private void setmark1_Click(object sender, EventArgs e)
        {
            int dev;
            StripLine stripline = new StripLine();
            stripline.Interval = 0;
            stripline.StripWidth = 3;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    stripline.IntervalOffset = atributes.pX_c1;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev!=-1)
                        atributes.annot1.Add(atributes.labels[dev]);
                    else
                        atributes.annot1.Add("empty");

                    stripline.BackColor = atributes.clabels[dev+1];
                    atributes.lines1.Add(stripline);
                    chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 1:
                    stripline.IntervalOffset = atributes.pX_c2;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot2.Add(atributes.labels[dev]);
                    else
                        atributes.annot2.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines2.Add(stripline);
                    chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 2:
                    stripline.IntervalOffset = atributes.pX_c3;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot3.Add(atributes.labels[dev]);
                    else
                        atributes.annot3.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines3.Add(stripline);
                    chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 3:
                    stripline.IntervalOffset = atributes.pX_c4;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot4.Add(atributes.labels[dev]);
                    else
                        atributes.annot4.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines4.Add(stripline);
                    chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 4:
                    stripline.IntervalOffset = atributes.pX_c5;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot5.Add(atributes.labels[dev]);
                    else
                        atributes.annot5.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines5.Add(stripline);
                    chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 5:
                    stripline.IntervalOffset = atributes.pX_c6;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot6.Add(atributes.labels[dev]);
                    else
                        atributes.annot6.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines6.Add(stripline);
                    chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 6:
                    stripline.IntervalOffset = atributes.pX_c7;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot7.Add(atributes.labels[dev]);
                    else
                        atributes.annot7.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines7.Add(stripline);
                    chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 7:
                    stripline.IntervalOffset = atributes.pX_c8;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot8.Add(atributes.labels[dev]);
                    else
                        atributes.annot8.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines8.Add(stripline);
                    chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 8:
                    stripline.IntervalOffset = atributes.pX_c9;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot9.Add(atributes.labels[dev]);
                    else
                        atributes.annot9.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines9.Add(stripline);
                    chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 9:
                    stripline.IntervalOffset = atributes.pX_c10;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot10.Add(atributes.labels[dev]);
                    else
                        atributes.annot10.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines10.Add(stripline);
                    chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 10:
                    stripline.IntervalOffset = atributes.pX_c11;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot11.Add(atributes.labels[dev]);
                    else
                        atributes.annot11.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines11.Add(stripline);
                    chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 11:
                    stripline.IntervalOffset = atributes.pX_c12;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        atributes.annot12.Add(atributes.labels[dev]);
                    else
                        atributes.annot12.Add("empty");

                    stripline.BackColor = atributes.clabels[dev + 1];

                    atributes.lines12.Add(stripline);
                    chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
            }
        }

        private void remmark_Click(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    if (atributes.lines1.Count > 0)
                    {
                        atributes.lines1.RemoveAt(atributes.lines1.Count - 1);
                        atributes.annot1.RemoveAt(atributes.annot1.Count - 1);
                        chart1.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines1)
                            chart1.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 1:
                    if (atributes.lines2.Count > 0)
                    {
                        atributes.lines2.RemoveAt(atributes.lines2.Count - 1);
                        atributes.annot2.RemoveAt(atributes.annot2.Count - 1);
                        chart2.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines2)
                            chart2.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 2:
                    if (atributes.lines3.Count > 0)
                    {
                        atributes.lines3.RemoveAt(atributes.lines3.Count - 1);
                        atributes.annot3.RemoveAt(atributes.annot3.Count - 1);
                        chart3.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines3)
                            chart3.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 3:
                    if (atributes.lines4.Count > 0)
                    {
                        atributes.lines4.RemoveAt(atributes.lines4.Count - 1);
                        atributes.annot4.RemoveAt(atributes.annot4.Count - 1);
                        chart4.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines4)
                            chart4.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 4:
                    if (atributes.lines5.Count > 0)
                    {
                        atributes.lines5.RemoveAt(atributes.lines5.Count - 1);
                        atributes.annot5.RemoveAt(atributes.annot5.Count - 1);
                        chart5.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines5)
                            chart5.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 5:
                    if (atributes.lines6.Count > 0)
                    {
                        atributes.lines6.RemoveAt(atributes.lines6.Count - 1);
                        atributes.annot6.RemoveAt(atributes.annot6.Count - 1);
                        chart6.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines6)
                            chart6.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 6:
                    if (atributes.lines7.Count > 0)
                    {
                        atributes.lines7.RemoveAt(atributes.lines7.Count - 1);
                        atributes.annot7.RemoveAt(atributes.annot7.Count - 1);
                        chart7.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines7)
                            chart7.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 7:
                    if (atributes.lines8.Count > 0)
                    {
                        atributes.lines8.RemoveAt(atributes.lines8.Count - 1);
                        atributes.annot8.RemoveAt(atributes.annot8.Count - 1);
                        chart8.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines8)
                            chart8.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 8:
                    if (atributes.lines9.Count > 0)
                    {
                        atributes.lines9.RemoveAt(atributes.lines9.Count - 1);
                        atributes.annot9.RemoveAt(atributes.annot9.Count - 1);
                        chart9.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines9)
                            chart9.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 9:
                    if (atributes.lines10.Count > 0)
                    {
                        atributes.lines10.RemoveAt(atributes.lines10.Count - 1);
                        atributes.annot10.RemoveAt(atributes.annot10.Count - 1);
                        chart10.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines10)
                            chart10.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 10:
                    if (atributes.lines11.Count > 0)
                    {
                        atributes.lines11.RemoveAt(atributes.lines11.Count - 1);
                        atributes.annot11.RemoveAt(atributes.annot11.Count - 1);
                        chart11.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines11)
                            chart11.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 11:
                    if (atributes.lines12.Count > 0)
                    {
                        atributes.lines12.RemoveAt(atributes.lines12.Count - 1);
                        atributes.annot12.RemoveAt(atributes.annot12.Count - 1);
                        chart12.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in atributes.lines12)
                            chart12.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
            }
        }

        private void eCGDeviceSupportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Actually, only GE CardioSoft 12SL is supported.\nMore devices will be probably included soon.", "Devices Supported");
        }

        private void filteringToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Fixed Window Filter (window: 1% of hertz sampling).\n" +
                            "2. Sliding Window Filter (window: 2% of hertz sampling).\n" +
                            "3. Median Filter (window: 2% of hertz sampling).\n" +
                            "4. [1. signal result] - [3. signal result].\n" +
                            "5. Band-stop Filter (50 and 60Hz).", "Automatic Filtering Summary");
        }

        private void patienTargetThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[][] d = new double[3][];
            d[0] = new double[6];
            d[1] = new double[6];
            d[2] = new double[6];
            d[0][0] = atributes.athleteSegmentsMin[0];
            d[0][1] = atributes.athleteSegmentsMax[0];
            d[0][2] = atributes.athleteSegmentsMin[1];
            d[0][3] = atributes.athleteSegmentsMax[1];
            d[0][4] = atributes.athleteSegmentsMin[2];
            d[0][5] = atributes.athleteSegmentsMax[2];
            d[1][0] = atributes.averageSegmentsMin[0];
            d[1][1] = atributes.averageSegmentsMax[0];
            d[1][2] = atributes.averageSegmentsMin[1];
            d[1][3] = atributes.averageSegmentsMax[1];
            d[1][4] = atributes.averageSegmentsMin[2];
            d[1][5] = atributes.averageSegmentsMax[2];
            d[2][0] = atributes.customSegmentsMin[0];
            d[2][1] = atributes.customSegmentsMax[0];
            d[2][2] = atributes.customSegmentsMin[1];
            d[2][3] = atributes.customSegmentsMax[1];
            d[2][4] = atributes.customSegmentsMin[2];
            d[2][5] = atributes.customSegmentsMax[2];

            FormThresholdsInfo ft = new FormThresholdsInfo(d);
            ft.Show();
        }

        private void automaticPQRSTDetectionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. R Peaks (max/min detection using interval given by heart rate).\n" +
                            "2. Q Peaks in [r_peak-qrs_max/2, r_peak-qrs_min/2).\n" +
                            "3. S Peaks in (r_peak+qrs_min/2, r_peak+qrs_max/2].\n" +
                            "4. P Peaks in [q_peak-(pr_max-qrs_min/2), q_peak-(pr_min-qrs_max/2)).\n" +
                            "5. T Peaks in (s_peak+(qt_min-qrs_max), s_peak+(qt_max-qrs_min)].", "PQRS Detection Summary");
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            atributes.aboutUS = new FormAboutUS();
            atributes.aboutUS.Show();
        }
        private void filteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!modification)
                if (filteringToolStripMenuItem.Checked)
                {
                    manualF = false;
                    filteringToolStripMenuItem.Checked = false;
                    addLabelsAutomaticallyToolStripMenuItem.Visible = true;
                    //Console.WriteLine(this.ClientSize.Height);
                    this.ClientSize = new System.Drawing.Size(946, 372); // = 477;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;
                    groupBox6.Visible = false;
                    groupBox7.Visible = false;
                    groupBox8.Visible = false;
                }
                else
                {
                    manualF = true;
                    filteringToolStripMenuItem.Checked = true;
                    addLabelsAutomaticallyToolStripMenuItem.Visible = false;
                    //Console.WriteLine(this.ClientSize.Height);
                    this.ClientSize = new System.Drawing.Size(946, 475); // = 470;
                    groupBox4.Visible = true;
                    groupBox5.Visible = true;
                    groupBox6.Visible = true;
                    groupBox7.Visible = true;
                    groupBox8.Visible = true;
                }
            else
                if (filteringToolStripMenuItem.Checked)
                {
                    manualF = false;
                    filteringToolStripMenuItem.Checked = false;
                    addLabelsAutomaticallyToolStripMenuItem.Visible = true;
                    //Console.WriteLine(this.ClientSize.Height);
                    this.ClientSize = new System.Drawing.Size(1205, 372); // = 477;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;
                    groupBox6.Visible = false;
                    groupBox7.Visible = false;
                    groupBox8.Visible = false;
                }
                else
                {
                    manualF = true;
                    filteringToolStripMenuItem.Checked = true;
                    addLabelsAutomaticallyToolStripMenuItem.Visible = false;
                    //Console.WriteLine(this.ClientSize.Height);
                    this.ClientSize = new System.Drawing.Size(1205, 475); // = 470;
                    groupBox4.Visible = true;
                    groupBox5.Visible = true;
                    groupBox6.Visible = true;
                    groupBox7.Visible = true;
                    groupBox8.Visible = true;
                }   
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label52.Text = trackBar3.Value.ToString();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label50.Text = trackBar2.Value.ToString();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label12.Text = trackBar1.Value.ToString();
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            label55.Text = trackBar4.Value.ToString();
        }

        private void addLabelsManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart1.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c1 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c1 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c1 + "," + atributes.pY_c1 + ")";
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart2.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c2 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c2 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c2 + "," + atributes.pY_c2 + ")";
        }

        private void chart3_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart3.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c3 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c3 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c3 + "," + atributes.pY_c3 + ")";
        }

        private void chart4_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart4.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c4 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c4 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c4 + "," + atributes.pY_c4 + ")";
        }

        private void chart5_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart5.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c5 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c5 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c5 + "," + atributes.pY_c5 + ")";
        }

        private void chart6_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart6.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c6 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c6 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c6 + "," + atributes.pY_c6 + ")";
        }

        private void chart7_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart7.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c7 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c7 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c7 + "," + atributes.pY_c7 + ")";
        }

        private void chart8_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart8.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c8 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c8 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c8 + "," + atributes.pY_c8 + ")";
        }

        private void chart9_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart9.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c9 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c9 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c9 + "," + atributes.pY_c9 + ")";
        }

        private void chart10_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart10.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c10 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c10 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c10 + "," + atributes.pY_c10 + ")";
        }

        private void chart11_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart11.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c11 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c11 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c11 + "," + atributes.pY_c11 + ")";
        }

        private void chart12_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart12.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            atributes.pX_c12 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            atributes.pY_c12 = chartArea.CursorY.Position;
            c1_point.Text = "(" + atributes.pX_c12 + "," + atributes.pY_c12 + ")";
        }
        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigReport frm = new FormConfigReport(atributes.repAge, atributes.repHeight, atributes.repWeight, atributes.repRace,
                                                        atributes.repGender, atributes.repPQ, atributes.repQR, atributes.repRS, atributes.repST,
                                                        atributes.repPR, atributes.repRR, atributes.repQT, atributes.repQT_c, atributes.repQRS,
                                                        atributes.repi1, atributes.repi2, atributes.repi3, atributes.repaVR, atributes.repaVL,
                                                        atributes.repaVF, atributes.repV1, atributes.repV2, atributes.repV3, atributes.repV4,
                                                        atributes.repV5, atributes.repV6);
            frm.ShowDialog();

            atributes.repAge = frm.age;
            atributes.repHeight = frm.height;
            atributes.repWeight = frm.weight;
            atributes.repRace = frm.race;
            atributes.repGender = frm.gender;
            atributes.repPQ = frm.segPQ;
            atributes.repQR = frm.segQR;
            atributes.repRS = frm.segRS;
            atributes.repST = frm.segST;
            atributes.repPR = frm.segPR;
            atributes.repRR = frm.segRR;
            atributes.repQT = frm.segQT;
            atributes.repQT_c = frm.segQTc;
            atributes.repQRS = frm.segQRS;
            atributes.repi1 = frm.i1;
            atributes.repi2 = frm.i2;
            atributes.repi3 = frm.i3;
            atributes.repaVR = frm.iaVR;
            atributes.repaVL = frm.iaVL;
            atributes.repaVF = frm.iaVF;
            atributes.repV1 = frm.iV1;
            atributes.repV2 = frm.iV2;
            atributes.repV3 = frm.iV3;
            atributes.repV4 = frm.iV4;
            atributes.repV5 = frm.iV5;
            atributes.repV6 = frm.iV6;
        }

        private void threshholdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormConfig frm = new FormConfig();
            FormConfig frm = new FormConfig(atributes.customSegmentsMin, atributes.customSegmentsMax, atributes.typePeople, atributes.umbralRuidoPot,
                                            atributes.umbralRuidoTime, atributes.ventanaComparacion);
            frm.ShowDialog();
            atributes.umbralRuidoPot = frm.R_peaks_offset;
            atributes.umbralRuidoTime = frm.Segments_window_offset;
            atributes.ventanaComparacion = frm.P_T_searching_window;
            atributes.typePeople = frm.selected;

            atributes.customSegmentsMin[0] = frm.valsCustom[0][0];
            atributes.customSegmentsMin[1] = frm.valsCustom[0][1];
            atributes.customSegmentsMin[2] = frm.valsCustom[0][2];

            atributes.customSegmentsMax[0] = frm.valsCustom[1][0];
            atributes.customSegmentsMax[1] = frm.valsCustom[1][1];
            atributes.customSegmentsMax[2] = frm.valsCustom[1][2];
        }

        private void saveECGDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (atributes.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    int i;
                    string day = DateTime.Now.Day.ToString();
                    string month = DateTime.Now.Month.ToString();
                    string year = DateTime.Now.Year.ToString();
                    string hour = DateTime.Now.Hour.ToString();
                    string min = DateTime.Now.Minute.ToString();
                    string sec = DateTime.Now.Second.ToString();
                    var sr = new StreamWriter(atributes.saveFileDialog1.FileName);
                    //
                    string name = atributes.saveFileDialog1.FileName;
                    //string s = (atributes.fileName.Replace(".XML", "")) + "_annotations.XML";
                    Console.WriteLine(name);
                    //TextWriter txt = new StreamWriter(name);
                    sr.Write("<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>\n");
                    sr.Write("<CardiologyXML>\n");
                    sr.Write("\t<ObservationType>RestECG</ObservationType>\n");

                    sr.Write("\t<ObservationDateTime>\n");
                    sr.Write("\t\t<Hour>"+hour+"</Hour>\n");
                    sr.Write("\t\t<Minute>"+min+"</Minute>\n");
                    sr.Write("\t\t<Second>"+sec+"</Second>\n");
                    sr.Write("\t\t<Day>"+day+"</Day>\n");
                    sr.Write("\t\t<Month>"+month+"</Month>\n");
                    sr.Write("\t\t<Year>"+year+"</Year>\n");
                    sr.Write("\t</ObservationDateTime>\n");

                    sr.Write("\t<UID>\n");
                    sr.Write("\t\t<DICOMStudyUID>1.2.840.113619.2.235.55583609418563173411694844</DICOMStudyUID>\n");
                    sr.Write("\t</UID>\n");

                    sr.Write("\t<ClinicalInfo>\n");
                    sr.Write("\t\t<ReasonForStudy></ReasonForStudy>\n");
                    sr.Write("\t\t<Technician>\n");
                    sr.Write("\t\t\t<FamilyName>Visualizer</FamilyName>\n");
                    sr.Write("\t\t\t<GivenName>ECG</GivenName>\n");
                    sr.Write("\t\t\t<PersonID></PersonID>\n");
                    sr.Write("\t\t</Technician>\n");
                    sr.Write("\t\t<ObservationComment></ObservationComment>\n");
                    sr.Write("\t\t<DeviceInfo>\n");
                    sr.Write("\t\t\t<Desc>CardioSoft</Desc>\n");
                    sr.Write("\t\t\t<SoftwareVer>V6.73</SoftwareVer>\n");
                    sr.Write("\t\t\t<AnalysisVer>12SL V21</AnalysisVer>\n");
                    sr.Write("\t\t</DeviceInfo>\n");
                    sr.Write("\t</ClinicalInfo>\n");

                    sr.Write("\t<PatientVisit>\n");
                    sr.Write("\t\t<PatientClass>O</PatientClass>\n");
                    sr.Write("\t\t<AssignedPatientLocation>\n");
                    sr.Write("\t\t\t<Facility>Universidad de Sevilla</Facility>\n");
                    sr.Write("\t\t\t<LocationNumber>0</LocationNumber>\n");
                    sr.Write("\t\t\t<LocationName>* 0 *</LocationName>\n");
                    sr.Write("\t\t</AssignedPatientLocation>\n");
                    sr.Write("\t\t<PatientRoom></PatientRoom>\n");
                    sr.Write("\t\t<AdmissionType>ROUT</AdmissionType>\n");
                    sr.Write("\t\t<OrderingProvider>\n");
                    sr.Write("\t\t\t<FamilyName></FamilyName>\n");
                    sr.Write("\t\t\t<GivenName></GivenName>\n");
                    sr.Write("\t\t\t<PersonID></PersonID>\n");
                    sr.Write("\t\t</OrderingProvider>\n");
                    sr.Write("\t\t<AttendingDoctor>\n");
                    sr.Write("\t\t\t<FamilyName>Visualizer</FamilyName>\n");
                    sr.Write("\t\t\t<GivenName>ECG</GivenName>\n");
                    sr.Write("\t\t\t<PersonID></PersonID>\n");
                    sr.Write("\t\t</AttendingDoctor>\n");
                    sr.Write("\t\t<ReferringDoctor>\n");
                    sr.Write("\t\t\t<FamilyName></FamilyName>\n");
                    sr.Write("\t\t\t<GivenName></GivenName>\n");
                    sr.Write("\t\t\t<PersonID></PersonID>\n");
                    sr.Write("\t\t</ReferringDoctor>\n");
                    sr.Write("\t\t<ServicingFacility>\n");
                    sr.Write("\t\t\t<Name>Universidad de Sevilla</Name>\n");
                    sr.Write("\t\t\t<Address>\n");
                    sr.Write("\t\t\t\t<Street1></Street1>\n");
                    sr.Write("\t\t\t\t<City></City>\n");
                    sr.Write("\t\t\t</Address>\n");
                    sr.Write("\t\t</ServicingFacility>\n");
                    sr.Write("\t\t<SysBP units=\"mmHg\"></SysBP>\n");
                    sr.Write("\t\t<DiaBP units=\"mmHg\"></DiaBP>\n");
                    sr.Write("\t\t<MedicalHistory>\n");
                    sr.Write("\t\t\t<MedicalHistoryText></MedicalHistoryText>\n");
                    sr.Write("\t\t</MedicalHistory>\n");
                    sr.Write("\t\t<OrderNumber></OrderNumber>\n");
                    sr.Write("\t\t<Medications>\n");
                    sr.Write("\t\t\t<Drug></Drug>\n");
                    sr.Write("\t\t\t<Dosage></Dosage>\n");
                    sr.Write("\t\t</Medications>\n");
                    sr.Write("\t\t<ExtraQuestions>\n");
                    sr.Write("\t\t\t<Label Type=\"Text\"></Label>\n");
                    sr.Write("\t\t\t<Content></Content>\n");
                    sr.Write("\t\t\t<Label Type=\"Text\"></Label>\n");
                    sr.Write("\t\t\t<Content></Content>\n");
                    sr.Write("\t\t</ExtraQuestions>\n");
                    sr.Write("\t</PatientVisit>\n");


                    sr.Write("\t<PatientInfo>\n");
                    sr.Write("\t\t<PID>000000000</PID>\n");
                    sr.Write("\t\t<Name>\n");
                    sr.Write("\t\t\t<FamilyName>Surname</FamilyName>\n");
                    sr.Write("\t\t\t<GivenName>Name</GivenName>\n");
                    sr.Write("\t\t</Name>\n");
                    sr.Write("\t\t<Age units=\"YEARS\">"+atributes.age_new+"</Age>\n");
                    sr.Write("\t\t<BirthDateTime>\n");
                    sr.Write("\t\t\t<Day>day</Day>\n");
                    sr.Write("\t\t\t<Month>month</Month>\n");
                    sr.Write("\t\t\t<Year>year</Year>\n");
                    sr.Write("\t\t</BirthDateTime>\n");
                    sr.Write("\t\t<Gender>"+atributes.gender_new+"</Gender>\n");
                    sr.Write("\t\t<Race>"+atributes.race_new+"</Race>\n");
                    sr.Write("\t\t<Height units=\"CENTIMETERS\">"+atributes.height_new+"</Height>\n");
                    sr.Write("\t\t<Weight units=\"KILOGRAMS\">"+atributes.weight_new+"</Weight>\n");
                    sr.Write("\t\t<PaceMaker>no</PaceMaker>\n");
                    sr.Write("\t</PatientInfo>\n");

                    sr.Write("\t<FilterSetting>\n");
                    sr.Write("\t\t<CubicSpline>No</CubicSpline>\n");
                    sr.Write("\t\t<Filter50Hz>Yes</Filter50Hz>\n");
                    sr.Write("\t\t<Filter60Hz>No</Filter60Hz>\n");
                    sr.Write("\t\t<LowPass units=\"Hz\">100</LowPass>\n");
                    sr.Write("\t\t<HighPass units=\"Hz\">0.01</HighPass>\n");
                    sr.Write("\t</FilterSetting>\n");

                    sr.Write("\t<Device-Type>2</Device-Type>\n");
                    
                    sr.Write("\t<Interpretation>\n");
                    sr.Write("\t</Interpretation>\n");


                    //extraigo de restingECGMeasurements

                    sr.Write("\t<RestingECGMeasurements>\n");

                    sr.Write("\t\t<DiagnosisVersion>"+ atributes.restingECGMeasurements.ChildNodes[0].InnerText + "</DiagnosisVersion>\n");
                    sr.Write("\t\t<VentricularRate units=\"BPM\">" + atributes.restingECGMeasurements.ChildNodes[1].InnerText + "</VentricularRate>\n");
                    sr.Write("\t\t<PQInterval units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[2].InnerText + "</PQInterval>\n");
                    sr.Write("\t\t<PDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[3].InnerText + "</PDuration>\n");
                    sr.Write("\t\t<QRSDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[4].InnerText + "</QRSDuration>\n");
                    sr.Write("\t\t<QTInterval units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[5].InnerText + "</QTInterval>\n");
                    sr.Write("\t\t<QTCInterval units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[6].InnerText + "</QTCInterval>\n");
                    sr.Write("\t\t<RRInterval units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[7].InnerText + "</RRInterval>\n");
                    sr.Write("\t\t<PPInterval units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[8].InnerText + "</PPInterval>\n");
                    sr.Write("\t\t<SokolovLVHIndex units=\"mV\">" + atributes.restingECGMeasurements.ChildNodes[9].InnerText + "</SokolovLVHIndex>\n");
                    sr.Write("\t\t<PAxis units=\"degrees\">" + atributes.restingECGMeasurements.ChildNodes[10].InnerText + "</PAxis>\n");
                    sr.Write("\t\t<RAxis units=\"degrees\">" + atributes.restingECGMeasurements.ChildNodes[11].InnerText + "</RAxis>\n");
                    sr.Write("\t\t<TAxis units=\"degrees\">" + atributes.restingECGMeasurements.ChildNodes[12].InnerText + "</TAxis>\n");
                    sr.Write("\t\t<QTDispersion units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[13].InnerText + "</QTDispersion>\n");
                    sr.Write("\t\t<QTDispersionBazett units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[14].InnerText + "</QTDispersionBazett>\n");
                    sr.Write("\t\t<QRSNum>" + atributes.restingECGMeasurements.ChildNodes[15].InnerText + "</QRSNum>\n");

                    sr.Write("\t\t<MeasurementTable Creation=\"12SL\">\n");
                    sr.Write("\t\t\t<LeadOrder>" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[0].InnerText + "</LeadOrder>\n");
                    sr.Write("\t\t\t<QDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[1].InnerText + "</QDuration>\n");
                    sr.Write("\t\t\t<RDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[2].InnerText + "</RDuration>\n");
                    sr.Write("\t\t\t<SDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[3].InnerText + "</SDuration>\n");
                    sr.Write("\t\t\t<RpDuration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[4].InnerText + "</RpDuration>\n");
                    sr.Write("\t\t\t<PAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[5].InnerText + "</PAmplitude>\n");
                    sr.Write("\t\t\t<QAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[6].InnerText + "</QAmplitude>\n");
                    sr.Write("\t\t\t<RAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[7].InnerText + "</RAmplitude>\n");
                    sr.Write("\t\t\t<SAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[8].InnerText + "</SAmplitude>\n");
                    sr.Write("\t\t\t<R1Amplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[9].InnerText + "</R1Amplitude>\n");
                    sr.Write("\t\t\t<S1Amplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[10].InnerText + "</S1Amplitude>\n");
                    sr.Write("\t\t\t<JAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[11].InnerText + "</JAmplitude>\n");
                    sr.Write("\t\t\t<JXAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[12].InnerText + "</JXAmplitude>\n");
                    sr.Write("\t\t\t<TAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[13].InnerText + "</TAmplitude>\n");
                    sr.Write("\t\t\t<JXSlope units=\"uV/s\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[14].InnerText + "</JXSlope>\n");
                    sr.Write("\t\t\t<R1Duration units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[15].InnerText + "</R1Duration>\n");
                    sr.Write("\t\t\t<P1Amplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[16].InnerText + "</P1Amplitude>\n");
                    sr.Write("\t\t\t<JXEAmplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[17].InnerText + "</JXEAmplitude>\n");
                    sr.Write("\t\t\t<T1Amplitude units=\"uV\">" + atributes.restingECGMeasurements.ChildNodes[16].ChildNodes[18].InnerText + "</T1Amplitude>\n");
                    sr.Write("\t\t</MeasurementTable>\n");

                    sr.Write("\t\t<POnset units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[17].InnerText + "</POnset>\n");
                    sr.Write("\t\t<POffset units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[18].InnerText + "</POffset>\n");
                    sr.Write("\t\t<QOnset units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[19].InnerText + "</QOnset>\n");
                    sr.Write("\t\t<QOffset units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[20].InnerText + "</QOffset>\n");
                    sr.Write("\t\t<TOffset units=\"ms\">" + atributes.restingECGMeasurements.ChildNodes[21].InnerText + "</TOffset>\n");

                    sr.Write("\t\t<MedianSamples>\n");
                    sr.Write("\t\t\t<NumberOfLeads>" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[0].InnerText + "</NumberOfLeads>\n");
                    //sr.Write("\t\t\t<SampleRate units=\"Hz\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[1].InnerText + "</SampleRate>\n");
                    sr.Write("\t\t\t<SampleRate units=\"Hz\">" + atributes.herMod.ToString() + "</SampleRate>\n");
                    sr.Write("\t\t\t<ChannelSampleCountTotal>" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[2].InnerText + "</ChannelSampleCountTotal>\n");
                    sr.Write("\t\t\t<Resolution units=\"uVperLsb\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[3].InnerText + "</Resolution>\n");
                    sr.Write("\t\t\t<FirstValid units=\"Sample\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[4].InnerText + "</FirstValid>\n");
                    sr.Write("\t\t\t<LastValid units=\"Sample\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[5].InnerText + "</LastValid>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"I\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[6].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"II\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[7].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"III\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[8].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"aVR\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[9].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"aVL\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[10].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"aVF\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[11].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V1\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[12].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V2\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[13].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V3\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[14].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V4\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[15].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V5\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[16].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t\t<WaveformData lead=\"V6\">" + atributes.restingECGMeasurements.ChildNodes[22].ChildNodes[17].InnerText + "</WaveformData>\n");
                    sr.Write("\t\t</MedianSamples>\n");

                    sr.Write("\t</RestingECGMeasurements>\n");

                    //extraigo de vectorLoops

                    sr.Write("\t<VectorLoops Creation=\"Inverse Dower\">\n");

                    sr.Write("\t\t<Resolution units=\"uVperLsb\">" + atributes.vectorLoops.ChildNodes[0].InnerText + "</Resolution>\n");
                    sr.Write("\t\t<ChannelSampleCountTotal>" + atributes.vectorLoops.ChildNodes[1].InnerText + "</ChannelSampleCountTotal>\n");
                    sr.Write("\t\t<POnset units=\"sample\">" + atributes.vectorLoops.ChildNodes[2].InnerText + "</POnset>\n");
                    sr.Write("\t\t<POffset units=\"sample\">" + atributes.vectorLoops.ChildNodes[3].InnerText + "</POffset>\n");
                    sr.Write("\t\t<QOnset units=\"sample\">" + atributes.vectorLoops.ChildNodes[4].InnerText + "</QOnset>\n");
                    sr.Write("\t\t<QOffset units=\"sample\">" + atributes.vectorLoops.ChildNodes[5].InnerText + "</QOffset>\n");
                    sr.Write("\t\t<TOffset units=\"sample\">" + atributes.vectorLoops.ChildNodes[6].InnerText + "</TOffset>\n");
                    sr.Write("\t\t<Frontal Lead=\"X\">" + atributes.vectorLoops.ChildNodes[7].InnerText + "</Frontal>\n");
                    sr.Write("\t\t<Frontal Lead=\"-Y\">" + atributes.vectorLoops.ChildNodes[8].InnerText + "</Frontal>\n");
                    sr.Write("\t\t<Horizontal Lead=\"X\">" + atributes.vectorLoops.ChildNodes[9].InnerText + "</Horizontal>\n");
                    sr.Write("\t\t<Horizontal Lead=\"Z\">" + atributes.vectorLoops.ChildNodes[10].InnerText + "</Horizontal>\n");
                    sr.Write("\t\t<Sagittal Lead=\"-Z\">" + atributes.vectorLoops.ChildNodes[11].InnerText + "</Sagittal>\n");
                    sr.Write("\t\t<Sagittal Lead=\"-Y\">" + atributes.vectorLoops.ChildNodes[12].InnerText + "</Sagittal>\n");

                    sr.Write("\t</VectorLoops>\n");

                    //guardamos datos modificados

                    sr.Write("\t<StripData>\n");

                    sr.Write("\t\t<NumberOfLeads>" + atributes.node.ChildNodes[0].InnerText + "</NumberOfLeads>\n");
                    //sr.Write("\t\t<SampleRate units=\"Hz\">" + atributes.node.ChildNodes[1].InnerText + "</SampleRate>\n");
                    sr.Write("\t\t<SampleRate units=\"Hz\">" + atributes.herMod.ToString() + "</SampleRate>\n");
                    //sr.Write("\t\t<ChannelSampleCountTotal>" + atributes.node.ChildNodes[2].InnerText + "</ChannelSampleCountTotal>\n");
                    sr.Write("\t\t<ChannelSampleCountTotal>" + atributes.raw1f.Count.ToString() + "</ChannelSampleCountTotal>\n");
                    sr.Write("\t\t<Resolution units=\"uVperLsb\">" + atributes.node.ChildNodes[3].InnerText + "</Resolution>\n");
                    
                    sr.Write("\t\t<WaveformData lead=\"I\">\t\t");
                    for (i = 0; i<atributes.raw1f.Count-1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw1f[i])+ ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw1f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"II\">\t\t");
                    for (i = 0; i < atributes.raw2f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw2f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw2f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"III\">\t\t");
                    for (i = 0; i < atributes.raw3f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw3f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw3f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"aVR\">\t\t");
                    for (i = 0; i < atributes.raw4f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw4f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw4f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"aVL\">\t\t");
                    for (i = 0; i < atributes.raw5f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw5f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw5f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"aVF\">\t\t");
                    for (i = 0; i < atributes.raw6f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw6f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw6f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V1\">\t\t");
                    for (i = 0; i < atributes.raw7f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw7f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw7f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V2\">\t\t");
                    for (i = 0; i < atributes.raw8f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw8f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw8f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V3\">\t\t");
                    for (i = 0; i < atributes.raw9f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw9f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw9f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V4\">\t\t");
                    for (i = 0; i < atributes.raw10f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw10f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw10f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V5\">\t\t");
                    for (i = 0; i < atributes.raw11f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw11f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw11f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t\t<WaveformData lead=\"V6\">\t\t");
                    for (i = 0; i < atributes.raw12f.Count - 1; i++)
                    {
                        sr.Write(Convert.ToInt32(atributes.raw12f[i]) + ",");
                    }
                    sr.Write(Convert.ToInt32(atributes.raw12f[i]));
                    sr.Write("</WaveformData>\n");

                    sr.Write("\t</StripData>\n");

                    sr.Write("\t<Export>\n");
                    sr.Write("\t</Export>\n");
                    sr.Write("\t<CSWeb>\n");
                    sr.Write("\t\t<a href=\"localhost/scripts/CardioSoftWeb/iscard.dll?GetExamination?PatNumber=53&amp;RecTyp=0&amp;RecComp=1&amp;RecNumber=12\"></a>\n");
                    sr.Write("\t</CSWeb>\n");

                    
                    sr.Write("</CardiologyXML>");
                    sr.Close();
                }
            }
            catch (SecurityException ex)
            {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        private void emulateNewECGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!manualF)
                if (emulateNewECGToolStripMenuItem.Checked)
                {
                    modification = false;
                    emulateNewECGToolStripMenuItem.Checked = false;
                    this.ClientSize = new System.Drawing.Size(946, 372);
                    groupBox9.Visible = false;
                    groupBox10.Visible = false;
                    groupBox11.Visible = false;
                    groupBox12.Visible = false;
                }
                else
                {
                    modification = true;
                    emulateNewECGToolStripMenuItem.Checked = true;
                    this.ClientSize = new System.Drawing.Size(1205, 372);
                    groupBox9.Visible = true;
                    groupBox10.Visible = true;
                    groupBox11.Visible = true;
                    if (peaksDetected)
                        groupBox12.Visible = true;
                }
            else
                if (emulateNewECGToolStripMenuItem.Checked)
                {
                    modification = false;
                    emulateNewECGToolStripMenuItem.Checked = false;
                    this.ClientSize = new System.Drawing.Size(946, 475);
                    groupBox9.Visible = false;
                    groupBox10.Visible = false;
                    groupBox11.Visible = false;
                    groupBox12.Visible = false;
                }
                else
                {
                    modification = true;
                    emulateNewECGToolStripMenuItem.Checked = true;
                    this.ClientSize = new System.Drawing.Size(1205, 475);
                    groupBox9.Visible = true;
                    groupBox10.Visible = true;
                    groupBox11.Visible = true;
                    if (peaksDetected)
                        groupBox12.Visible = true;
                }
        }

        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            label58.Text = (trackBar5.Value/100.0).ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (atributes.firstAM)
            {
                savePreviousAM();
                atributes.firstAM = false;
            }
            double factor = trackBar5.Value / 100.0;
            if (factor != 1.0)
            {
                atributes.data1f = Filters.AmplitudeModification(atributes.data1f, factor);
                atributes.data2f = Filters.AmplitudeModification(atributes.data2f, factor);
                atributes.data3f = Filters.AmplitudeModification(atributes.data3f, factor);
                atributes.data4f = Filters.AmplitudeModification(atributes.data4f, factor);
                atributes.data5f = Filters.AmplitudeModification(atributes.data5f, factor);
                atributes.data6f = Filters.AmplitudeModification(atributes.data6f, factor);
                atributes.data7f = Filters.AmplitudeModification(atributes.data7f, factor);
                atributes.data8f = Filters.AmplitudeModification(atributes.data8f, factor);
                atributes.data9f = Filters.AmplitudeModification(atributes.data9f, factor);
                atributes.data10f = Filters.AmplitudeModification(atributes.data10f, factor);
                atributes.data11f = Filters.AmplitudeModification(atributes.data11f, factor);
                atributes.data12f = Filters.AmplitudeModification(atributes.data12f, factor);

                atributes.raw1f = Filters.AmplitudeModification(atributes.raw1f, factor);
                atributes.raw2f = Filters.AmplitudeModification(atributes.raw2f, factor);
                atributes.raw3f = Filters.AmplitudeModification(atributes.raw3f, factor);
                atributes.raw4f = Filters.AmplitudeModification(atributes.raw4f, factor);
                atributes.raw5f = Filters.AmplitudeModification(atributes.raw5f, factor);
                atributes.raw6f = Filters.AmplitudeModification(atributes.raw6f, factor);
                atributes.raw7f = Filters.AmplitudeModification(atributes.raw7f, factor);
                atributes.raw8f = Filters.AmplitudeModification(atributes.raw8f, factor);
                atributes.raw9f = Filters.AmplitudeModification(atributes.raw9f, factor);
                atributes.raw10f = Filters.AmplitudeModification(atributes.raw10f, factor);
                atributes.raw11f = Filters.AmplitudeModification(atributes.raw11f, factor);
                atributes.raw12f = Filters.AmplitudeModification(atributes.raw12f, factor);

                atributes.finAM1 = new List<double>(atributes.data1f);
                atributes.finAM2 = new List<double>(atributes.data2f);
                atributes.finAM3 = new List<double>(atributes.data3f);
                atributes.finAM4 = new List<double>(atributes.data4f);
                atributes.finAM5 = new List<double>(atributes.data5f);
                atributes.finAM6 = new List<double>(atributes.data6f);
                atributes.finAM7 = new List<double>(atributes.data7f);
                atributes.finAM8 = new List<double>(atributes.data8f);
                atributes.finAM9 = new List<double>(atributes.data9f);
                atributes.finAM10 = new List<double>(atributes.data10f);
                atributes.finAM11 = new List<double>(atributes.data11f);
                atributes.finAM12 = new List<double>(atributes.data12f);

                atributes.rawfinAM1 = new List<double>(atributes.raw1f);
                atributes.rawfinAM2 = new List<double>(atributes.raw2f);
                atributes.rawfinAM3 = new List<double>(atributes.raw3f);
                atributes.rawfinAM4 = new List<double>(atributes.raw4f);
                atributes.rawfinAM5 = new List<double>(atributes.raw5f);
                atributes.rawfinAM6 = new List<double>(atributes.raw6f);
                atributes.rawfinAM7 = new List<double>(atributes.raw7f);
                atributes.rawfinAM8 = new List<double>(atributes.raw8f);
                atributes.rawfinAM9 = new List<double>(atributes.raw9f);
                atributes.rawfinAM10 = new List<double>(atributes.raw10f);
                atributes.rawfinAM11 = new List<double>(atributes.raw11f);
                atributes.rawfinAM12 = new List<double>(atributes.raw12f);

                drawECG();
                button14.Enabled = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            restorePreviousAM();
            drawECG();
        }

        private void restorePreviousAM()
        {
            atributes.data1f = new List<double>(atributes.auxAM1);
            atributes.data2f = new List<double>(atributes.auxAM2);
            atributes.data3f = new List<double>(atributes.auxAM3);
            atributes.data4f = new List<double>(atributes.auxAM4);
            atributes.data5f = new List<double>(atributes.auxAM5);
            atributes.data6f = new List<double>(atributes.auxAM6);
            atributes.data7f = new List<double>(atributes.auxAM7);
            atributes.data8f = new List<double>(atributes.auxAM8);
            atributes.data9f = new List<double>(atributes.auxAM9);
            atributes.data10f = new List<double>(atributes.auxAM10);
            atributes.data11f = new List<double>(atributes.auxAM11);
            atributes.data12f = new List<double>(atributes.auxAM12);

            atributes.raw1f = new List<double>(atributes.rawauxAM1);
            atributes.raw2f = new List<double>(atributes.rawauxAM2);
            atributes.raw3f = new List<double>(atributes.rawauxAM3);
            atributes.raw4f = new List<double>(atributes.rawauxAM4);
            atributes.raw5f = new List<double>(atributes.rawauxAM5);
            atributes.raw6f = new List<double>(atributes.rawauxAM6);
            atributes.raw7f = new List<double>(atributes.rawauxAM7);
            atributes.raw8f = new List<double>(atributes.rawauxAM8);
            atributes.raw9f = new List<double>(atributes.rawauxAM9);
            atributes.raw10f = new List<double>(atributes.rawauxAM10);
            atributes.raw11f = new List<double>(atributes.rawauxAM11);
            atributes.raw12f = new List<double>(atributes.rawauxAM12);

            atributes.herMod = atributes.previousHertzs;
            atributes.firstAM = true;
            button14.Enabled = false;
        }

        private void trackBar6_ValueChanged(object sender, EventArgs e)
        {
            label60.Text = (trackBar6.Value * 0.25).ToString();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (atributes.firstAM)
            {
                savePreviousTM();
                atributes.firstTM = false;
            }
            double factor = trackBar6.Value * 0.25;
            double secs = Convert.ToInt32(atributes.secs) / factor;
            atributes.secs_new = Convert.ToString(secs);
            label16.Text = atributes.secs_new;
            if (factor != 1.0)
            {
                atributes.data1f = Filters.TimingModification(atributes.data1f, factor);
                atributes.data2f = Filters.TimingModification(atributes.data2f, factor);
                atributes.data3f = Filters.TimingModification(atributes.data3f, factor);
                atributes.data4f = Filters.TimingModification(atributes.data4f, factor);
                atributes.data5f = Filters.TimingModification(atributes.data5f, factor);
                atributes.data6f = Filters.TimingModification(atributes.data6f, factor);
                atributes.data7f = Filters.TimingModification(atributes.data7f, factor);
                atributes.data8f = Filters.TimingModification(atributes.data8f, factor);
                atributes.data9f = Filters.TimingModification(atributes.data9f, factor);
                atributes.data10f = Filters.TimingModification(atributes.data10f, factor);
                atributes.data11f = Filters.TimingModification(atributes.data11f, factor);
                atributes.data12f = Filters.TimingModification(atributes.data12f, factor);

                atributes.raw1f = Filters.TimingModification(atributes.raw1f, factor);
                atributes.raw2f = Filters.TimingModification(atributes.raw2f, factor);
                atributes.raw3f = Filters.TimingModification(atributes.raw3f, factor);
                atributes.raw4f = Filters.TimingModification(atributes.raw4f, factor);
                atributes.raw5f = Filters.TimingModification(atributes.raw5f, factor);
                atributes.raw6f = Filters.TimingModification(atributes.raw6f, factor);
                atributes.raw7f = Filters.TimingModification(atributes.raw7f, factor);
                atributes.raw8f = Filters.TimingModification(atributes.raw8f, factor);
                atributes.raw9f = Filters.TimingModification(atributes.raw9f, factor);
                atributes.raw10f = Filters.TimingModification(atributes.raw10f, factor);
                atributes.raw11f = Filters.TimingModification(atributes.raw11f, factor);
                atributes.raw12f = Filters.TimingModification(atributes.raw12f, factor);

                atributes.finTM1 = new List<double>(atributes.data1f);
                atributes.finTM2 = new List<double>(atributes.data2f);
                atributes.finTM3 = new List<double>(atributes.data3f);
                atributes.finTM4 = new List<double>(atributes.data4f);
                atributes.finTM5 = new List<double>(atributes.data5f);
                atributes.finTM6 = new List<double>(atributes.data6f);
                atributes.finTM7 = new List<double>(atributes.data7f);
                atributes.finTM8 = new List<double>(atributes.data8f);
                atributes.finTM9 = new List<double>(atributes.data9f);
                atributes.finTM10 = new List<double>(atributes.data10f);
                atributes.finTM11 = new List<double>(atributes.data11f);
                atributes.finTM12 = new List<double>(atributes.data12f);

                atributes.rawfinTM1 = new List<double>(atributes.raw1f);
                atributes.rawfinTM2 = new List<double>(atributes.raw2f);
                atributes.rawfinTM3 = new List<double>(atributes.raw3f);
                atributes.rawfinTM4 = new List<double>(atributes.raw4f);
                atributes.rawfinTM5 = new List<double>(atributes.raw5f);
                atributes.rawfinTM6 = new List<double>(atributes.raw6f);
                atributes.rawfinTM7 = new List<double>(atributes.raw7f);
                atributes.rawfinTM8 = new List<double>(atributes.raw8f);
                atributes.rawfinTM9 = new List<double>(atributes.raw9f);
                atributes.rawfinTM10 = new List<double>(atributes.raw10f);
                atributes.rawfinTM11 = new List<double>(atributes.raw11f);
                atributes.rawfinTM12 = new List<double>(atributes.raw12f);

                //atributes.blockSize = atributes.blockSize * factor;

                drawECG();
                button16.Enabled = true;
            }
        }

        private void restorePreviousTM()
        {
            atributes.data1f = new List<double>(atributes.auxTM1);
            atributes.data2f = new List<double>(atributes.auxTM2);
            atributes.data3f = new List<double>(atributes.auxTM3);
            atributes.data4f = new List<double>(atributes.auxTM4);
            atributes.data5f = new List<double>(atributes.auxTM5);
            atributes.data6f = new List<double>(atributes.auxTM6);
            atributes.data7f = new List<double>(atributes.auxTM7);
            atributes.data8f = new List<double>(atributes.auxTM8);
            atributes.data9f = new List<double>(atributes.auxTM9);
            atributes.data10f = new List<double>(atributes.auxTM10);
            atributes.data11f = new List<double>(atributes.auxTM11);
            atributes.data12f = new List<double>(atributes.auxTM12);

            atributes.raw1f = new List<double>(atributes.rawauxTM1);
            atributes.raw2f = new List<double>(atributes.rawauxTM2);
            atributes.raw3f = new List<double>(atributes.rawauxTM3);
            atributes.raw4f = new List<double>(atributes.rawauxTM4);
            atributes.raw5f = new List<double>(atributes.rawauxTM5);
            atributes.raw6f = new List<double>(atributes.rawauxTM6);
            atributes.raw7f = new List<double>(atributes.rawauxTM7);
            atributes.raw8f = new List<double>(atributes.rawauxTM8);
            atributes.raw9f = new List<double>(atributes.rawauxTM9);
            atributes.raw10f = new List<double>(atributes.rawauxTM10);
            atributes.raw11f = new List<double>(atributes.rawauxTM11);
            atributes.raw12f = new List<double>(atributes.rawauxTM12);

            atributes.herMod = atributes.previousHertzs;
            atributes.firstTM = true;
            button16.Enabled = false;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            restorePreviousTM();
            drawECG();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            atributes.age_new = textBox1.Text;
            atributes.gender_new = textBox2.Text;
            atributes.height_new = textBox3.Text;
            atributes.race_new = textBox4.Text;
            atributes.weight_new = textBox5.Text;
            modifyBasicData();
            button19.Enabled = true;
        }

        private void groupBox12_VisibleChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = 5;
            comboBox4.SelectedIndex = 5;
            comboBox5.SelectedIndex = 5;
            comboBox6.SelectedIndex = 5;
            comboBox7.SelectedIndex = 5;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != 5)
                modifyPs(Convert.ToDouble(comboBox3.SelectedItem.ToString()));
            if (comboBox4.SelectedIndex != 5)
                modifyQs(Convert.ToDouble(comboBox4.SelectedItem.ToString()));
            if (comboBox5.SelectedIndex != 5)
                modifyRs(Convert.ToDouble(comboBox5.SelectedItem.ToString()));
            if (comboBox6.SelectedIndex != 5)
                modifySs(Convert.ToDouble(comboBox6.SelectedItem.ToString()));
            if (comboBox7.SelectedIndex != 5)
                modifyTs(Convert.ToDouble(comboBox7.SelectedItem.ToString()));

            comboBox3.SelectedIndex = 5;
            comboBox4.SelectedIndex = 5;
            comboBox5.SelectedIndex = 5;
            comboBox6.SelectedIndex = 5;
            comboBox7.SelectedIndex = 5;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            atributes.age_new = atributes.age;
            atributes.gender_new = atributes.gender;
            atributes.height_new = atributes.height;
            atributes.race_new = atributes.race;
            atributes.weight_new = atributes.weight;

            textBox1.Text = atributes.age;
            textBox2.Text = atributes.gender;
            textBox3.Text = atributes.height;
            textBox4.Text = atributes.race;
            textBox5.Text = atributes.weight;

            modifyBasicData();
            button19.Enabled = false;
        }

        private void modifyPs(double mod)
        {
            int posCentral;
            int posIni;
            int posFin;
            //double mmod = mod / 10;
            if (I)
                foreach (double d in atributes.picosP1)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    //Console.WriteLine(posCentral);
                    //Console.WriteLine(mod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i<=posFin; i++)
                    {
                        atributes.data1f[i] *= mod;
                        atributes.raw1f[i] *= mod;
                    }
                }
            if (II)
                foreach (double d in atributes.picosP2)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data2f[i] *= mod;
                        atributes.raw2f[i] *= mod;
                    }
                }
            if (III)
                foreach (double d in atributes.picosP3)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data3f[i] *= mod;
                        atributes.raw3f[i] *= mod;
                    }
                }
            if (aVR)
                foreach (double d in atributes.picosP4)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data4f[i] *= mod;
                        atributes.raw4f[i] *= mod;
                    }
                }
            if (aVL)
                foreach (double d in atributes.picosP5)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data5f[i] *= mod;
                        atributes.raw5f[i] *= mod;
                    }
                }
            if (aVF)
                foreach (double d in atributes.picosP6)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data6f[i] *= mod;
                        atributes.raw6f[i] *= mod;
                    }
                }
            if (V1)
                foreach (double d in atributes.picosP7)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data7f[i] *= mod;
                        atributes.raw7f[i] *= mod;
                    }
                }
            if (V2)
                foreach (double d in atributes.picosP8)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data8f[i] *= mod;
                        atributes.raw8f[i] *= mod;
                    }
                }
            if (V3)
                foreach (double d in atributes.picosP9)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data9f[i] *= mod;
                        atributes.raw9f[i] *= mod;
                    }
                }
            if (V4)
                foreach (double d in atributes.picosP10)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data10f[i] *= mod;
                        atributes.raw10f[i] *= mod;
                    }
                }
            if (V5)
                foreach (double d in atributes.picosP11)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data11f[i] *= mod;
                        atributes.raw11f[i] *= mod;
                    }
                }
            if (V6)
                foreach (double d in atributes.picosP12)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoP2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoP2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoP2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data12f[i] *= mod;
                        atributes.raw12f[i] *= mod;
                    }
                }

            drawECG();
            drawPeaks();
        }
        private void modifyQs(double mod)
        {
            int posCentral;
            int posIni;
            int posFin;
            //double mmod = mod / 10;
            if (I)
                foreach (double d in atributes.picosQ1)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    //Console.WriteLine(posCentral);
                    //Console.WriteLine(mod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data1f[i] *= mod;
                        atributes.raw1f[i] *= mod;
                    }
                }
            if (II)
                foreach (double d in atributes.picosQ2)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data2f[i] *= mod;
                        atributes.raw2f[i] *= mod;
                    }
                }
            if (III)
                foreach (double d in atributes.picosQ3)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data3f[i] *= mod;
                        atributes.raw3f[i] *= mod;
                    }
                }
            if (aVR)
                foreach (double d in atributes.picosQ4)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data4f[i] *= mod;
                        atributes.raw4f[i] *= mod;
                    }
                }
            if (aVL)
                foreach (double d in atributes.picosQ5)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data5f[i] *= mod;
                        atributes.raw5f[i] *= mod;
                    }
                }
            if (aVF)
                foreach (double d in atributes.picosQ6)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data6f[i] *= mod;
                        atributes.raw6f[i] *= mod;
                    }
                }
            if (V1)
                foreach (double d in atributes.picosQ7)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data7f[i] *= mod;
                        atributes.raw7f[i] *= mod;
                    }
                }
            if (V2)
                foreach (double d in atributes.picosQ8)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data8f[i] *= mod;
                        atributes.raw8f[i] *= mod;
                    }
                }
            if (V3)
                foreach (double d in atributes.picosQ9)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data9f[i] *= mod;
                        atributes.raw9f[i] *= mod;
                    }
                }
            if (V4)
                foreach (double d in atributes.picosQ10)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data10f[i] *= mod;
                        atributes.raw10f[i] *= mod;
                    }
                }
            if (V5)
                foreach (double d in atributes.picosQ11)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data11f[i] *= mod;
                        atributes.raw11f[i] *= mod;
                    }
                }
            if (V6)
                foreach (double d in atributes.picosQ12)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoQ2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoQ2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoQ2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data12f[i] *= mod;
                        atributes.raw12f[i] *= mod;
                    }
                }

            drawECG();
            drawPeaks();
        }
        private void modifyRs(double mod)
        {
            int posCentral;
            int posIni;
            int posFin;
            //double mmod = mod / 10;
            if (I)
                foreach (double d in atributes.picosR1)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    //Console.WriteLine(posCentral);
                    //Console.WriteLine(mod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data1f[i] *= mod;
                        atributes.raw1f[i] *= mod;
                    }
                }
            if (II)
                foreach (double d in atributes.picosR2)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data2f[i] *= mod;
                        atributes.raw2f[i] *= mod;
                    }
                }
            if (III)
                foreach (double d in atributes.picosR3)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data3f[i] *= mod;
                        atributes.raw3f[i] *= mod;
                    }
                }
            if (aVR)
                foreach (double d in atributes.picosR4)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data4f[i] *= mod;
                        atributes.raw4f[i] *= mod;
                    }
                }
            if (aVL)
                foreach (double d in atributes.picosR5)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data5f[i] *= mod;
                        atributes.raw5f[i] *= mod;
                    }
                }
            if (aVF)
                foreach (double d in atributes.picosR6)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data6f[i] *= mod;
                        atributes.raw6f[i] *= mod;
                    }
                }
            if (V1)
                foreach (double d in atributes.picosR7)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data7f[i] *= mod;
                        atributes.raw7f[i] *= mod;
                    }
                }
            if (V2)
                foreach (double d in atributes.picosR8)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data8f[i] *= mod;
                        atributes.raw8f[i] *= mod;
                    }
                }
            if (V3)
                foreach (double d in atributes.picosR9)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data9f[i] *= mod;
                        atributes.raw9f[i] *= mod;
                    }
                }
            if (V4)
                foreach (double d in atributes.picosR10)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data10f[i] *= mod;
                        atributes.raw10f[i] *= mod;
                    }
                }
            if (V5)
                foreach (double d in atributes.picosR11)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data11f[i] *= mod;
                        atributes.raw11f[i] *= mod;
                    }
                }
            if (V6)
                foreach (double d in atributes.picosR12)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoR / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoR / 4);
                    else posFin = posCentral + ((ventanaAnchoR / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data12f[i] *= mod;
                        atributes.raw12f[i] *= mod;
                    }
                }

            drawECG();
            drawPeaks();
        }
        private void modifySs(double mod)
        {
            int posCentral;
            int posIni;
            int posFin;
            //double mmod = mod / 10;
            if (I)
                foreach (double d in atributes.picosS1)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    //Console.WriteLine(posCentral);
                    //Console.WriteLine(mod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data1f[i] *= mod;
                        atributes.raw1f[i] *= mod;
                    }
                }
            if (II)
                foreach (double d in atributes.picosS2)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data2f[i] *= mod;
                        atributes.raw2f[i] *= mod;
                    }
            }
            if (III)
                foreach (double d in atributes.picosS3)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data3f[i] *= mod;
                        atributes.raw3f[i] *= mod;
                    }
                }
            if (aVR)
                foreach (double d in atributes.picosS4)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data4f[i] *= mod;
                        atributes.raw4f[i] *= mod;
                    }
                }
            if (aVL)
                foreach (double d in atributes.picosS5)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data5f[i] *= mod;
                        atributes.raw5f[i] *= mod;
                    }
                }
            if (aVF)
                foreach (double d in atributes.picosS6)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data6f[i] *= mod;
                        atributes.raw6f[i] *= mod;
                    }
                }
            if (V1)
                foreach (double d in atributes.picosS7)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data7f[i] *= mod;
                        atributes.raw7f[i] *= mod;
                    }
                }
            if (V2)
                foreach (double d in atributes.picosS8)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data8f[i] *= mod;
                        atributes.raw8f[i] *= mod;
                    }
                }
            if (V3)
                foreach (double d in atributes.picosS9)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data9f[i] *= mod;
                        atributes.raw9f[i] *= mod;
                    }
                }
            if (V4)
                foreach (double d in atributes.picosS10)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data10f[i] *= mod;
                        atributes.raw10f[i] *= mod;
                    }
                }
            if (V5)
                foreach (double d in atributes.picosS11)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data11f[i] *= mod;
                        atributes.raw11f[i] *= mod;
                    }
                }
            if (V6)
                foreach (double d in atributes.picosS12)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    posIni = posCentral - (ventanaAnchoS2_2 / 4);
                    if (mod > 0) posFin = posCentral + (ventanaAnchoS2_2 / 4);
                    else posFin = posCentral + ((ventanaAnchoS2_2 / 4) * 2);
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data12f[i] *= mod;
                        atributes.raw12f[i] *= mod;
                    }
                }

            drawECG();
            drawPeaks();
        }

        private void modifyTs(double mod)
        {
            int posCentral;
            int posIni;
            int posFin;
            //double mmod = mod / 10;
            if(I)
                foreach (double d in atributes.picosT1)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    //Console.WriteLine(posCentral);
                    //Console.WriteLine(mod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data1f[i] *= mod;
                        atributes.raw1f[i] *= mod;
                    }
                }
            if(II)
                foreach (double d in atributes.picosT2)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data2f[i] *= mod;
                        atributes.raw2f[i] *= mod;
                    }
                }
            if(III)
                foreach (double d in atributes.picosT3)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data3f[i] *= mod;
                        atributes.raw3f[i] *= mod;
                    }
                }
            if(aVR)
                foreach (double d in atributes.picosT4)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data4f[i] *= mod;
                        atributes.raw4f[i] *= mod;
                    }
                }
            if(aVL)
                foreach (double d in atributes.picosT5)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data5f[i] *= mod;
                        atributes.raw5f[i] *= mod;
                    }
                }
            if(aVF)
                foreach (double d in atributes.picosT6)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data6f[i] *= mod;
                        atributes.raw6f[i] *= mod;
                    }
                }
            if(V1)
                foreach (double d in atributes.picosT7)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data7f[i] *= mod;
                        atributes.raw7f[i] *= mod;
                    }
                }
            if(V2)
                foreach (double d in atributes.picosT8)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data8f[i] *= mod;
                        atributes.raw8f[i] *= mod;
                    }
                }
            if(V3)
                foreach (double d in atributes.picosT9)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    { 
                        atributes.data9f[i] *= mod;
                        atributes.raw9f[i] *= mod;
                    }
                }
            if(V4)
                foreach (double d in atributes.picosT10)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data10f[i] *= mod;
                        atributes.raw10f[i] *= mod;
                    }
                }
            if(V5)
                foreach (double d in atributes.picosT11)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);
                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data11f[i] *= mod;
                        atributes.raw11f[i] *= mod;
                    }
                }
            if(V6)
                foreach (double d in atributes.picosT12)
                {
                    posCentral = Convert.ToInt32(d * atributes.herMod);

                    if (mod > 0)
                    {
                        posIni = posCentral - (ventanaAnchoT2_2 / 4);
                        posFin = posCentral + (ventanaAnchoT2_2 / 4);
                    }
                    else
                    {
                        posIni = posCentral - ((ventanaAnchoT2_2 / 4) * 4);
                        posFin = posCentral + ((ventanaAnchoT2_2 / 4) * 4);
                    }
                    for (int i = posIni; i <= posFin; i++)
                    {
                        atributes.data12f[i] *= mod;
                        atributes.raw12f[i] *= mod;
                    }
                }

            drawECG();
            drawPeaks();
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            I = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            II = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            III = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            aVR = checkBox9.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            aVL = checkBox10.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            aVF = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            V1 = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            V2 = checkBox13.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            V3 = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            V4 = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            V5 = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            V6 = checkBox17.Checked;
        }
    }
}
