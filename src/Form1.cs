using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Collections;

namespace ECGVisualizer
{
    public partial class Form1 : Form
    {
        //"International Recommendations for Electrocardiographic Interpretation in Athletes" (2017) - Journal of the American College of Cardiology - https://www.sciencedirect.com/science/article/pii/S0735109717302024
        double[] athleteSegmentsMin = new double[3] { 0.20, 0.08, 0.32 }; //PR, QRS, QT min in secs
        double[] athleteSegmentsMax = new double[3] { 0.40, 0.16, 0.56 }; //PR, QRS, QT max in secs

        //https://www.nottingham.ac.uk/nursing/practice/resources/cardiology/function/normal_duration.php
        double[] averageSegmentsMin = new double[3] { 0.12, 0.08, 0.35 }; //PR, QRS, QT min in secs
        double[] averageSegmentsMax = new double[3] { 0.20, 0.12, 0.43 }; //PR, QRS, QT max in secs

        double[] customSegmentsMin = new double[3] { 0.12, 0.08, 0.35 }; //PR, QRS, QT min in secs
        double[] customSegmentsMax = new double[3] { 0.20, 0.12, 0.43 }; //PR, QRS, QT max in secs

        public int umbralRuidoPot;
        public int umbralRuidoTime;

        private OpenFileDialog openFileDialog1;
        XmlDocument doc;
        XmlNode node;
        string fileName;
        string age, gender, race, height, weight, hertz, secs;
        int her,herMod, previousHertzs;
        int ventanaFiltroMedia;
        int ventanaComparacion = 10;
        //double hertzFiltered;
        double blockSize;
        double pX_c1, pX_c2, pX_c3, pX_c4, pX_c5, pX_c6, pX_c7, pX_c8, pX_c9, pX_c10, pX_c11, pX_c12;
        double pY_c1, pY_c2, pY_c3, pY_c4, pY_c5, pY_c6, pY_c7, pY_c8, pY_c9, pY_c10, pY_c11, pY_c12;
        ArrayList lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines10, lines11, lines12;
        List<double> data1, data2, data3, data4, data5, data6, data7, data8, data9, data10, data11, data12;
        List<double> data1f, data2f, data3f, data4f, data5f, data6f, data7f, data8f, data9f, data10f, data11f, data12f;
        List<double> auxFAF1, auxFAF2, auxFAF3, auxFAF4, auxFAF5, auxFAF6, auxFAF7, auxFAF8, auxFAF9, auxFAF10, auxFAF11, auxFAF12;
        List<double> auxMAF1, auxMAF2, auxMAF3, auxMAF4, auxMAF5, auxMAF6, auxMAF7, auxMAF8, auxMAF9, auxMAF10, auxMAF11, auxMAF12;
        List<double> auxMMF1, auxMMF2, auxMMF3, auxMMF4, auxMMF5, auxMMF6, auxMMF7, auxMMF8, auxMMF9, auxMMF10, auxMMF11, auxMMF12;
        List<double> auxBSF1, auxBSF2, auxBSF3, auxBSF4, auxBSF5, auxBSF6, auxBSF7, auxBSF8, auxBSF9, auxBSF10, auxBSF11, auxBSF12;
        List<double> auxSUB1, auxSUB2, auxSUB3, auxSUB4, auxSUB5, auxSUB6, auxSUB7, auxSUB8, auxSUB9, auxSUB10, auxSUB11, auxSUB12;

        List<double> finFAF1, finFAF2, finFAF3, finFAF4, finFAF5, finFAF6, finFAF7, finFAF8, finFAF9, finFAF10, finFAF11, finFAF12;
        List<double> finMAF1, finMAF2, finMAF3, finMAF4, finMAF5, finMAF6, finMAF7, finMAF8, finMAF9, finMAF10, finMAF11, finMAF12;
        List<double> finMMF1, finMMF2, finMMF3, finMMF4, finMMF5, finMMF6, finMMF7, finMMF8, finMMF9, finMMF10, finMMF11, finMMF12;
        List<double> finBSF1, finBSF2, finBSF3, finBSF4, finBSF5, finBSF6, finBSF7, finBSF8, finBSF9, finBSF10, finBSF11, finBSF12;

        bool firstFAF = true, firstMAF = true, firstMMF = true, firstBSF = true, firstSUB=true;

        ArrayList annot1, annot2, annot3, annot4, annot5, annot6, annot7, annot8, annot9, annot10, annot11, annot12;

        string[] labels;
        Color[] clabels;

        bool archivoCargado;
        string resolution;
        int resol;

        List<double> picosP1, picosP2, picosP3, picosP4, picosP5, picosP6, picosP7, picosP8, picosP9, picosP10, picosP11, picosP12;
        List<double> picosQ1, picosQ2, picosQ3, picosQ4, picosQ5, picosQ6, picosQ7, picosQ8, picosQ9, picosQ10, picosQ11, picosQ12;
        List<double> picosR1, picosR2, picosR3, picosR4, picosR5, picosR6, picosR7, picosR8, picosR9, picosR10, picosR11, picosR12;
        List<double> picosS1, picosS2, picosS3, picosS4, picosS5, picosS6, picosS7, picosS8, picosS9, picosS10, picosS11, picosS12;
        List<double> picosT1, picosT2, picosT3, picosT4, picosT5, picosT6, picosT7, picosT8, picosT9, picosT10, picosT11, picosT12;

        double[][] segments;
        bool[] inversionT;

        string reportFile;
        double tiempo = 0.0;

        bool R = true, Q = true, S = true, T = true, P = true;

        double bmpsFile;
        bool correcto = false;

        int typePeople = 1; //0: average, 1: athlete, ...

        FormAboutUS aboutUS;

        bool repAge, repHeight, repWeight, repRace, repGender, repPQ, repQR, repRS, repST, repPR, repRR, repQT, repQT_c, repQRS, repi1, repi2, repi3, repaVR, repaVL, repaVF, repV1, repV2, repV3, repV4, repV5, repV6;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory(); ;//@"C:\";
            openFileDialog1.Title = "Please select an ECG recorded file.";
            doc = new XmlDocument();
            CheckForIllegalCrossThreadCalls = false;
            clabels = new Color[] {Color.Black, Color.Red, Color.Orange, Color.Green, Color.Blue, Color.Violet};

            auxFAF1 = new List<double>();
            auxFAF2 = new List<double>();
            auxFAF3 = new List<double>();
            auxFAF4 = new List<double>();
            auxFAF5 = new List<double>();
            auxFAF6 = new List<double>();
            auxFAF7 = new List<double>();
            auxFAF8 = new List<double>();
            auxFAF9 = new List<double>();
            auxFAF10 = new List<double>();
            auxFAF11 = new List<double>();
            auxFAF12 = new List<double>();

            auxMAF1 = new List<double>();
            auxMAF2 = new List<double>();
            auxMAF3 = new List<double>();
            auxMAF4 = new List<double>();
            auxMAF5 = new List<double>();
            auxMAF6 = new List<double>();
            auxMAF7 = new List<double>();
            auxMAF8 = new List<double>();
            auxMAF9 = new List<double>();
            auxMAF10 = new List<double>();
            auxMAF11 = new List<double>();
            auxMAF12 = new List<double>();

            auxMMF1 = new List<double>();
            auxMMF2 = new List<double>();
            auxMMF3 = new List<double>();
            auxMMF4 = new List<double>();
            auxMMF5 = new List<double>();
            auxMMF6 = new List<double>();
            auxMMF7 = new List<double>();
            auxMMF8 = new List<double>();
            auxMMF9 = new List<double>();
            auxMMF10 = new List<double>();
            auxMMF11 = new List<double>();
            auxMMF12 = new List<double>();

            auxBSF1 = new List<double>();
            auxBSF2 = new List<double>();
            auxBSF3 = new List<double>();
            auxBSF4 = new List<double>();
            auxBSF5 = new List<double>();
            auxBSF6 = new List<double>();
            auxBSF7 = new List<double>();
            auxBSF8 = new List<double>();
            auxBSF9 = new List<double>();
            auxBSF10 = new List<double>();
            auxBSF11 = new List<double>();
            auxBSF12 = new List<double>();

            auxSUB1 = new List<double>();
            auxSUB2 = new List<double>();
            auxSUB3 = new List<double>();
            auxSUB4 = new List<double>();
            auxSUB5 = new List<double>();
            auxSUB6 = new List<double>();
            auxSUB7 = new List<double>();
            auxSUB8 = new List<double>();
            auxSUB9 = new List<double>();
            auxSUB10 = new List<double>();
            auxSUB11 = new List<double>();
            auxSUB12 = new List<double>();

            finFAF1 = new List<double>();
            finFAF2 = new List<double>();
            finFAF3 = new List<double>();
            finFAF4 = new List<double>();
            finFAF5 = new List<double>();
            finFAF6 = new List<double>();
            finFAF7 = new List<double>();
            finFAF8 = new List<double>();
            finFAF9 = new List<double>();
            finFAF10 = new List<double>();
            finFAF11 = new List<double>();
            finFAF12 = new List<double>();

            finMAF1 = new List<double>();
            finMAF2 = new List<double>();
            finMAF3 = new List<double>();
            finMAF4 = new List<double>();
            finMAF5 = new List<double>();
            finMAF6 = new List<double>();
            finMAF7 = new List<double>();
            finMAF8 = new List<double>();
            finMAF9 = new List<double>();
            finMAF10 = new List<double>();
            finMAF11 = new List<double>();
            finMAF12 = new List<double>();

            finMMF1 = new List<double>();
            finMMF2 = new List<double>();
            finMMF3 = new List<double>();
            finMMF4 = new List<double>();
            finMMF5 = new List<double>();
            finMMF6 = new List<double>();
            finMMF7 = new List<double>();
            finMMF8 = new List<double>();
            finMMF9 = new List<double>();
            finMMF10 = new List<double>();
            finMMF11 = new List<double>();
            finMMF12 = new List<double>();

            finBSF1 = new List<double>();
            finBSF2 = new List<double>();
            finBSF3 = new List<double>();
            finBSF4 = new List<double>();
            finBSF5 = new List<double>();
            finBSF6 = new List<double>();
            finBSF7 = new List<double>();
            finBSF8 = new List<double>();
            finBSF9 = new List<double>();
            finBSF10 = new List<double>();
            finBSF11 = new List<double>();
            finBSF12 = new List<double>();

            labels = new string[] {"P","Q","R","S","T"};
            
            lines1 = new ArrayList();
            lines2 = new ArrayList();
            lines3 = new ArrayList();
            lines4 = new ArrayList();
            lines5 = new ArrayList();
            lines6 = new ArrayList();
            lines7 = new ArrayList();
            lines8 = new ArrayList();
            lines9 = new ArrayList();
            lines10 = new ArrayList();
            lines11 = new ArrayList();
            lines12 = new ArrayList();

            annot1 = new ArrayList();
            annot2 = new ArrayList();
            annot3 = new ArrayList();
            annot4 = new ArrayList();
            annot5 = new ArrayList();
            annot6 = new ArrayList();
            annot7 = new ArrayList();
            annot8 = new ArrayList();
            annot9 = new ArrayList();
            annot10 = new ArrayList();
            annot11 = new ArrayList();
            annot12 = new ArrayList();

            data1 = new List<double>();
            data2 = new List<double>();
            data3 = new List<double>();
            data4 = new List<double>();
            data5 = new List<double>();
            data6 = new List<double>();
            data7 = new List<double>();
            data8 = new List<double>();
            data9 = new List<double>();
            data10 = new List<double>();
            data11 = new List<double>();
            data12 = new List<double>();

            data1f = new List<double>();
            data2f = new List<double>();
            data3f = new List<double>();
            data4f = new List<double>();
            data5f = new List<double>();
            data6f = new List<double>();
            data7f = new List<double>();
            data8f = new List<double>();
            data9f = new List<double>();
            data10f = new List<double>();
            data11f = new List<double>();
            data12f = new List<double>();

            segments = new double[12][];
            for(int i=0; i<12; i++)
            {
                segments[i] = new double[6];
            }
            inversionT = new bool[12] {false, false, false, false, false, false, false, false, false, false, false, false };

            string CurrentDirectory = Directory.GetCurrentDirectory();

            reportFile = CurrentDirectory + "/report.csv";

            archivoCargado = false;

            umbralRuidoPot = 40;
            umbralRuidoTime = 10;

            repAge = true;
            repHeight = true;
            repWeight = true;
            repRace = true;
            repGender = false;
            repPQ = false;
            repQR = false;
            repRS = false;
            repST = false;
            repPR = false;
            repRR = true;
            repQT = true;
            repQT_c = true;
            repQRS = true;
            repi1 = true;
            repi2 = true;
            repi3 = true;
            repaVR = true;
            repaVL = true;
            repaVF = true;
            repV1 = true;
            repV2 = true;
            repV3 = true;
            repV4 = true;
            repV5 = true;
            repV6 = true;

            this.ClientSize = new System.Drawing.Size(946, 388);
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

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigReport frm = new FormConfigReport(repAge,repHeight,repWeight,repRace,repGender,repPQ,repQR,repRS,repST,repPR,repRR,repQT,repQT_c,repQRS,repi1,repi2,repi3,repaVR,repaVL,repaVF,repV1,repV2,repV3,repV4,repV5,repV6);
            frm.ShowDialog();

            repAge = frm.age;
            repHeight = frm.height;
            repWeight = frm.weight;
            repRace = frm.race;
            repGender = frm.gender;
            repPQ = frm.segPQ;
            repQR = frm.segQR;
            repRS = frm.segRS;
            repST = frm.segST;
            repPR = frm.segPR;
            repRR = frm.segRR;
            repQT = frm.segQT;
            repQT_c = frm.segQTc;
            repQRS = frm.segQRS;
            repi1 = frm.i1;
            repi2 = frm.i2;
            repi3 = frm.i3;
            repaVR = frm.iaVR;
            repaVL = frm.iaVL;
            repaVF = frm.iaVF;
            repV1 = frm.iV1;
            repV2 = frm.iV2;
            repV3 = frm.iV3;
            repV4 = frm.iV4;
            repV5 = frm.iV5;
            repV6 = frm.iV6;
        }

        private void rESTORESIGNALSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //eraseData();

            data1f.Clear();
            data2f.Clear();
            data3f.Clear();
            data4f.Clear();
            data5f.Clear();
            data6f.Clear();
            data7f.Clear();
            data8f.Clear();
            data9f.Clear();
            data10f.Clear();
            data11f.Clear();
            data12f.Clear();

            data1f = new List<double>(data1);
            data2f = new List<double>(data2);
            data3f = new List<double>(data3);
            data4f = new List<double>(data4);
            data5f = new List<double>(data5);
            data6f = new List<double>(data6);
            data7f = new List<double>(data7);
            data8f = new List<double>(data8);
            data9f = new List<double>(data9);
            data10f = new List<double>(data10);
            data11f = new List<double>(data11);
            data12f = new List<double>(data12);

            herMod = her;

            blockSize = Math.Round((2 * herMod) * (1.0 / (double)herMod), 1);

            automaticPQRSTDetectionToolStripMenuItem.Enabled = false;
            generateReportToolStripMenuItem.Enabled = false;
            addLabelsAutomaticallyToolStripMenuItem.Enabled = true;
            filteringToolStripMenuItem.Enabled = true;

            drawECG();
        }

        private void threshholdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormConfig frm = new FormConfig();
            FormConfig frm = new FormConfig(customSegmentsMin, customSegmentsMax,typePeople, umbralRuidoPot, umbralRuidoTime, ventanaComparacion);
            frm.ShowDialog();
            umbralRuidoPot = frm.R_peaks_offset;
            umbralRuidoTime = frm.Segments_window_offset;
            ventanaComparacion = frm.P_T_searching_window;
            typePeople = frm.selected;

            customSegmentsMin[0] = frm.valsCustom[0][0];
            customSegmentsMin[1] = frm.valsCustom[0][1];
            customSegmentsMin[2] = frm.valsCustom[0][2];

            customSegmentsMax[0] = frm.valsCustom[1][0];
            customSegmentsMax[1] = frm.valsCustom[1][1];
            customSegmentsMax[2] = frm.valsCustom[1][2];
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
            d[0][0] = athleteSegmentsMin[0];
            d[0][1] = athleteSegmentsMax[0];
            d[0][2] = athleteSegmentsMin[1];
            d[0][3] = athleteSegmentsMax[1];
            d[0][4] = athleteSegmentsMin[2];
            d[0][5] = athleteSegmentsMax[2];
            d[1][0] = averageSegmentsMin[0];
            d[1][1] = averageSegmentsMax[0];
            d[1][2] = averageSegmentsMin[1];
            d[1][3] = averageSegmentsMax[1];
            d[1][4] = averageSegmentsMin[2];
            d[1][5] = averageSegmentsMax[2];
            d[2][0] = customSegmentsMin[0];
            d[2][1] = customSegmentsMax[0];
            d[2][2] = customSegmentsMin[1];
            d[2][3] = customSegmentsMax[1];
            d[2][4] = customSegmentsMin[2];
            d[2][5] = customSegmentsMax[2];

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
            aboutUS = new FormAboutUS();
            aboutUS.Show();
        }

        private void rPeaksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string v = "";
            DialogResult d = InputBox("R Peaks", "Change R-Peaks Noise (between 0.0 to 1.0):", ref v, umbralRuidoPot);
            if (d == DialogResult.OK)
            {
                try
                {
                    int dd = Convert.ToInt32(v);
                    if (dd >= 0 && dd <= 100)
                    {
                        umbralRuidoPot = Convert.ToInt32(v);
                        rPeaksToolStripMenuItem.Text = "R Peaks:  " + dd + "%";
                    }
                    else
                    {
                        MessageBox.Show("Wrong value");
                    }
                }
                catch (Exception) { MessageBox.Show("Wrong value"); }
            }
        }

        private void segmentsTime0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string v = "";
            DialogResult d = InputBox("Segments Time", "Change Segments Time Noise (between 0.0 and 1.0):", ref v, umbralRuidoTime);
            if (d == DialogResult.OK)
            {
                try
                {
                    int dd = Convert.ToInt32(v);
                    if (dd >= 0 && dd <= 100)
                    {
                        umbralRuidoTime = Convert.ToInt32(v);
                        segmentsTime0ToolStripMenuItem.Text = "Segments Time:  " + dd + "%";
                    }
                    else
                    {
                        MessageBox.Show("Wrong value");
                    }
                }
                catch (Exception) { MessageBox.Show("Wrong value"); }
            }
        }

        private void pTComparisonWindow1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string v = "";
            DialogResult d = InputBox("P & T Comparison Window", "Change P && T Window Size for Comparison (between 1 and 100%):", ref v, ventanaComparacion);
            if (d == DialogResult.OK)
            {
                try
                {
                    int dd = Convert.ToInt32(v);
                    if (dd >= 1 && dd <= 100)
                    {
                        ventanaComparacion = Convert.ToInt32(v);
                        pTComparisonWindow1ToolStripMenuItem.Text = "P && T Comparison Window: " + dd + "%";
                    }
                    else
                    {
                        MessageBox.Show("Wrong value");
                    }
                }
                catch (Exception) { MessageBox.Show("Wrong value"); }
            }
        }

        private void ordinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            athleteToolStripMenuItem.Checked = false;
            ordinaryToolStripMenuItem.Checked = true;
            customToolStripMenuItem.Checked = false;

            typePeople = 0;
        }

        private void athleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            athleteToolStripMenuItem.Checked = true;
            ordinaryToolStripMenuItem.Checked = false;
            customToolStripMenuItem.Checked = false;

            typePeople = 1;
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string prMin="", prMax="", qrsMin="", qrsMax="", qtMin="", qtMax="";

            DialogResult d = InputBoxSegments("Change Segments Time (in secs)", ref prMin, ref prMax, ref qrsMin, ref qrsMax, ref qtMin, ref qtMax, umbralRuidoTime);
            if (d == DialogResult.OK)
            {
                try
                {
                    double d1 = Convert.ToDouble(prMin);
                    double d2 = Convert.ToDouble(prMax);
                    double d3 = Convert.ToDouble(qrsMin);
                    double d4 = Convert.ToDouble(qrsMax);
                    double d5 = Convert.ToDouble(qtMin);
                    double d6 = Convert.ToDouble(qtMax);
                    customSegmentsMin[0] = d1;
                    customSegmentsMax[0] = d2;
                    customSegmentsMin[1] = d3;
                    customSegmentsMax[1] = d4;
                    customSegmentsMin[2] = d5;
                    customSegmentsMax[2] = d6;
                    //segmentsTime0ToolStripMenuItem.Text = "Segments Time:  " + Math.Truncate(dd * 100) + "%";
                    typePeople = 2;

                    athleteToolStripMenuItem.Checked = false;
                    ordinaryToolStripMenuItem.Checked = false;
                    customToolStripMenuItem.Checked = true;
                }
                catch (Exception) { MessageBox.Show("Wrong value"); }
            }

            
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                T = true;
            else
                T = false;

            clearECG();
            drawECG();

            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;

            if (checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                S = true;
            else
                S = false;

            clearECG();
            drawECG();

            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;

            if (checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                R = true;
            else
                R = false;

            clearECG();
            drawECG();

            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;

            if (checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                Q = true;
            else
                Q = false;

            clearECG();
            drawECG();

            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;

            if (checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                P = true;
            else
                P = false;

            clearECG();
            drawECG();

            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;

            if (checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //List<double> seg1 = Report.segmentsCalc(ref picosP1, ref picosQ1, ref picosR1, ref picosS1, ref picosT1);
            List<double> seg2 = Report.segmentsCalc(ref picosP2, ref picosQ2, ref picosR2, ref picosS2, ref picosT2);
            //List<double> seg3 = Report.segmentsCalc(ref picosP3, ref picosQ3, ref picosR3, ref picosS3, ref picosT3);
            //List<double> seg4 = Report.segmentsCalc(ref picosP4, ref picosQ4, ref picosR4, ref picosS4, ref picosT4);
            //List<double> seg5 = Report.segmentsCalc(ref picosP5, ref picosQ5, ref picosR5, ref picosS5, ref picosT5);
            //List<double> seg6 = Report.segmentsCalc(ref picosP6, ref picosQ6, ref picosR6, ref picosS6, ref picosT6);
            //List<double> seg7 = Report.segmentsCalc(ref picosP7, ref picosQ7, ref picosR7, ref picosS7, ref picosT7);
            //List<double> seg8 = Report.segmentsCalc(ref picosP8, ref picosQ8, ref picosR8, ref picosS8, ref picosT8);
            //List<double> seg9 = Report.segmentsCalc(ref picosP9, ref picosQ9, ref picosR9, ref picosS9, ref picosT9);
            //List<double> seg10 = Report.segmentsCalc(ref picosP10, ref picosQ10, ref picosR10, ref picosS10, ref picosT10);
            //List<double> seg11 = Report.segmentsCalc(ref picosP11, ref picosQ11, ref picosR11, ref picosS11, ref picosT11);
            //List<double> seg12 = Report.segmentsCalc(ref picosP12, ref picosQ12, ref picosR12, ref picosS12, ref picosT12);

            double mediaSegPQ = 1000 * seg2[0];//;* (seg1[0] + seg2[0] + seg3[0] + seg4[0] + seg5[0] + seg6[0] + seg7[0] + seg8[0] + seg9[0] + seg10[0] + seg11[0] + seg12[0]) / 12;

            double mediaSegQR = 1000 * seg2[1];//(seg1[1] + seg2[1] + seg3[1] + seg4[1] + seg5[1] + seg6[1] + seg7[1] + seg8[1] + seg9[1] + seg10[1] + seg11[1] + seg12[1]) / 12;

            double mediaSegRS = 1000 * seg2[2];//(seg1[2] + seg2[2] + seg3[2] + seg4[2] + seg5[2] + seg6[2] + seg7[2] + seg8[2] + seg9[2] + seg10[2] + seg11[2] + seg12[2]) / 12;

            double mediaSegST = 1000 * seg2[3];//(seg1[3] + seg2[3] + seg3[3] + seg4[3] + seg5[3] + seg6[3] + seg7[3] + seg8[3] + seg9[3] + seg10[3] + seg11[3] + seg12[3]) / 12;

            double mediaSegPR = 1000 * seg2[4];
            
            double mediaSegQRS = 1000 * seg2[5];//(seg1[4] + seg2[4] + seg3[4] + seg4[4] + seg5[4] + seg6[4] + seg7[4] + seg8[4] + seg9[4] + seg10[4] + seg11[4] + seg12[4]) / 12;

            double mediaSegRR = seg2[6];// (seg1[5] + seg2[5] + seg3[5] + seg4[5] + seg5[5] + seg6[5] + seg7[5] + seg8[5] + seg9[5] + seg10[5] + seg11[5] + seg12[5]) / 12;

            double pulsacionesPmin = 60.0 / mediaSegRR;

            mediaSegRR = mediaSegRR * 1000;

            double mediaSegQT = 1000 * seg2[7];

            double mediaSegQT_c = 1000 * seg2[8];

            inversionT[0] = Report.Tinversion(ref data1f, ref picosT1, herMod);
            inversionT[1] = Report.Tinversion(ref data2f, ref picosT2, herMod);
            inversionT[2] = Report.Tinversion(ref data3f, ref picosT3, herMod);
            inversionT[3] = Report.Tinversion(ref data4f, ref picosT4, herMod);
            inversionT[4] = Report.Tinversion(ref data5f, ref picosT5, herMod);
            inversionT[5] = Report.Tinversion(ref data6f, ref picosT6, herMod);
            inversionT[6] = Report.Tinversion(ref data7f, ref picosT7, herMod);
            inversionT[7] = Report.Tinversion(ref data8f, ref picosT8, herMod);
            inversionT[8] = Report.Tinversion(ref data9f, ref picosT9, herMod);
            inversionT[9] = Report.Tinversion(ref data10f, ref picosT10, herMod);
            inversionT[10] = Report.Tinversion(ref data11f, ref picosT11, herMod);
            inversionT[11] = Report.Tinversion(ref data12f, ref picosT11, herMod);

            int inv1, inv2, inv3, inv4, inv5, inv6, inv7, inv8, inv9, inv10, inv11, inv12;
            inv1 = inversionT[0] ? 1 : 0;
            inv2 = inversionT[1] ? 1 : 0;
            inv3 = inversionT[2] ? 1 : 0;
            inv4 = inversionT[3] ? 1 : 0;
            inv5 = inversionT[4] ? 1 : 0;
            inv6 = inversionT[5] ? 1 : 0;
            inv7 = inversionT[6] ? 1 : 0;
            inv8 = inversionT[7] ? 1 : 0;
            inv9 = inversionT[8] ? 1 : 0;
            inv10 = inversionT[9] ? 1 : 0;
            inv11 = inversionT[10] ? 1 : 0;
            inv12 = inversionT[11] ? 1 : 0;

            bool nuevo = false;

            string text="";
            if (!File.Exists(reportFile))
            {
                string clientHeader = "ID" + ";";//+ "Gender" + ";"

                if (repAge)
                    clientHeader += "Age" + ";";
                if (repRace)
                    clientHeader += "Race" + ";";
                if (repGender)
                    clientHeader += "Gender" + ";";
                if (repHeight)
                    clientHeader += "Height" + ";";
                if (repWeight)
                    clientHeader += "Weight" + ";";
                if (repPQ)
                    clientHeader += "PQ interval" + ";";
                if (repQR)
                    clientHeader += "QR interval" + ";";
                if (repRS)
                    clientHeader += "RS interval" + ";";
                if (repST)
                    clientHeader += "ST interval" + ";";
                if (repPR)
                    clientHeader += "PR interval" + ";";
                if (repRR)
                    clientHeader += "RR interval" + ";";
                if (repQRS)
                    clientHeader += "QRS interval" + ";";
                if (repQT)
                    clientHeader += "QT interval" + ";";
                if (repQT_c)
                    clientHeader += "QT_c interval" + ";";
                if (repi1)
                    clientHeader += "T inv I" + ";";
                if (repi2)
                    clientHeader += "T inv II" + ";";
                if (repi3)
                    clientHeader += "T inv III" + ";";
                if (repaVR)
                    clientHeader += "T inv aVR" + ";";
                if (repaVL)
                    clientHeader += "T inv aVL" + ";";
                if (repaVF)
                    clientHeader += "T inv aVF" + ";";
                if (repV1)
                    clientHeader += "T inv V1" + ";";
                if (repV2)
                    clientHeader += "T inv V2" + ";";
                if (repV3)
                    clientHeader += "T inv V3" + ";";
                if (repV4)
                    clientHeader += "T inv V4" + ";";
                if (repV5)
                    clientHeader += "T inv V5" + ";";
                if (repV6)
                    clientHeader += "T inv V6" + ";";

                clientHeader += Environment.NewLine;

                File.WriteAllText(reportFile, clientHeader);

                nuevo = true;
            }

            string fileName2 = "";

            int found1 = fileName.LastIndexOf("\\");
            int found2 = fileName.IndexOf(".xml");
            fileName2 = fileName.Substring(found1+1,found2-found1-1);

            text = fileName2 + ";";// + mediaSegRS + ";" + mediaSegST + ";" + mediaSegRR + ";" + mediaSegQRS + ";" + mediaSegQT + ";" + mediaSegQT_c + ";" + inv1 + ";" + inv2 + ";" + inv3 + ";" + inv4 + ";" + inv5 + ";" + inv6 + ";" + inv7 + ";" + inv8 + ";" + inv9 + ";" + inv10 + ";" + inv11 + ";" + inv12 + ";" + Environment.NewLine;

            if (repAge)
                text += age + ";";
            if (repRace)
                text += race + ";";
            if (repGender)
                text += gender + ";";
            if (repHeight)
                text += height + ";";
            if (repWeight)
                text += Convert.ToDouble(weight) / 10 + ";";
            if (repPQ)
                text += mediaSegPQ + ";";
            if (repQR)
                text += mediaSegQR + ";";
            if (repRS)
                text += mediaSegRS + ";";
            if (repST)
                text += mediaSegST + ";";
            if (repPR)
                text += mediaSegPR + ";";
            if (repRR)
                text += mediaSegRR + ";";
            if (repQRS)
                text += mediaSegQRS + ";";
            if (repQT)
                text += mediaSegQT + ";";
            if (repQT_c)
                text += mediaSegQT_c + ";";
            if (repi1)
                text += inv1 + ";";
            if (repi2)
                text += inv2 + ";";
            if (repi3)
                text += inv3 + ";";
            if (repaVR)
                text += inv4 + ";";
            if (repaVL)
                text += inv5 + ";";
            if (repaVF)
                text += inv6 + ";";
            if (repV1)
                text += inv7 + ";";
            if (repV2)
                text += inv8 + ";";
            if (repV3)
                text += inv9 + ";";
            if (repV4)
                text += inv10 + ";";
            if (repV5)
                text += inv11 + ";";
            if (repV6)
                text += inv12 + ";";

            text += Environment.NewLine;


            File.AppendAllText(reportFile, text);

            string mess;
            if (nuevo)
                mess = "New Report Generated";
            else
                mess = "Data Appended to Previous Report";

            MessageBox.Show(mess);

        }

        private void filteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filteringToolStripMenuItem.Checked)
            {
                filteringToolStripMenuItem.Checked = false;
                addLabelsAutomaticallyToolStripMenuItem.Visible = true;
                Console.WriteLine(this.ClientSize.Height);
                this.ClientSize = new System.Drawing.Size(946, 388); // = 477;
                groupBox4.Visible = false;
                groupBox5.Visible = false;
                groupBox6.Visible = false;
                groupBox7.Visible = false;
            }
            else
            {
                filteringToolStripMenuItem.Checked = true;
                addLabelsAutomaticallyToolStripMenuItem.Visible = false;
                Console.WriteLine(this.ClientSize.Height);
                this.ClientSize = new System.Drawing.Size(946, 475); // = 470;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
                groupBox7.Visible = true;
            }
        }

        private void pintarPicosR(double ancho)
        {
            foreach (double a in picosR1)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0*ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset = a - ((ancho * 0.5)* (1.0 / herMod));
                lines1.Add(stripline);
                chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }

            foreach (double a in picosR2)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / herMod));
                lines2.Add(stripline);
                chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR3)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines3.Add(stripline);
                chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR4)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines4.Add(stripline);
                chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR5)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines5.Add(stripline);
                chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR6)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines6.Add(stripline);
                chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR7)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines7.Add(stripline);
                chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR8)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines8.Add(stripline);
                chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR9)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines9.Add(stripline);
                chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR10)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines10.Add(stripline);
                chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR11)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines11.Add(stripline);
                chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosR12)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Green;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines12.Add(stripline);
                chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        private void pintarPicosQ(double ancho)
        {
            foreach (double a in picosQ1)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines1.Add(stripline);
                chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ2)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines2.Add(stripline);
                chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ3)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines3.Add(stripline);
                chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ4)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines4.Add(stripline);
                chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ5)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines5.Add(stripline);
                chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ6)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines6.Add(stripline);
                chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ7)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines7.Add(stripline);
                chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ8)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines8.Add(stripline);
                chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ9)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines9.Add(stripline);
                chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ10)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines10.Add(stripline);
                chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ11)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines11.Add(stripline);
                chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosQ12)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Red;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines12.Add(stripline);
                chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        private void pintarPicosP(double ancho)
        {
            foreach (double a in picosP1)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines1.Add(stripline);
                chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP2)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines2.Add(stripline);
                chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP3)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines3.Add(stripline);
                chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP4)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines4.Add(stripline);
                chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP5)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines5.Add(stripline);
                chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP6)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines6.Add(stripline);
                chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP7)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines7.Add(stripline);
                chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP8)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines8.Add(stripline);
                chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP9)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines9.Add(stripline);
                chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP10)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines10.Add(stripline);
                chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP11)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines11.Add(stripline);
                chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosP12)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Orange;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines12.Add(stripline);
                chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        private void pintarPicosS(double ancho)
        {
            foreach (double a in picosS1)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines1.Add(stripline);
                chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS2)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines2.Add(stripline);
                chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS3)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines3.Add(stripline);
                chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS4)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines4.Add(stripline);
                chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS5)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines5.Add(stripline);
                chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS6)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines6.Add(stripline);
                chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS7)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines7.Add(stripline);
                chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS8)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines8.Add(stripline);
                chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS9)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines9.Add(stripline);
                chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS10)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines10.Add(stripline);
                chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS11)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines11.Add(stripline);
                chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosS12)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Blue;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines12.Add(stripline);
                chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        private void pintarPicosT(double ancho)
        {
            foreach (double a in picosT1)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines1.Add(stripline);
                chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT2)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines2.Add(stripline);
                chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT3)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines3.Add(stripline);
                chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT4)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines4.Add(stripline);
                chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT5)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines5.Add(stripline);
                chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT6)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines6.Add(stripline);
                chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT7)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines7.Add(stripline);
                chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT8)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines8.Add(stripline);
                chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT9)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines9.Add(stripline);
                chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT10)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines10.Add(stripline);
                chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT11)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines11.Add(stripline);
                chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
            foreach (double a in picosT12)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = Color.Violet;
                stripline.IntervalOffset =  a - ((ancho * 0.25) * (1.0 / her));
                lines12.Add(stripline);
                chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        private void automaticPQRSTDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int frecuenciaMuestreo = (int)herMod;
            int ventanaAnchoR = (int)(1.0 / (bmpsFile / 60.0) * frecuenciaMuestreo);
            int ventanaRetraso = (int)(ventanaAnchoR*0.3);

            //According to intervals
            int ventanaAnchoQ2 = typePeople == 0 ? (int)((averageSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : (int)((customSegmentsMax[1] / 2.0) * frecuenciaMuestreo);
            int ventanaAnchoS2 = typePeople == 0 ? (int)((averageSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMax[1] / 2.0) * frecuenciaMuestreo) : (int)((customSegmentsMax[1] / 2.0) * frecuenciaMuestreo);
            int ventanaAnchoP2 = typePeople == 0 ? (int)((averageSegmentsMax[0] - (averageSegmentsMin[1] / 2.0)) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMax[0] - (athleteSegmentsMin[1] / 2.0)) * frecuenciaMuestreo) : (int)((customSegmentsMax[0] - (customSegmentsMin[1] / 2.0)) * frecuenciaMuestreo);
            //int ventanaAnchoT2 = typePeople == 0 ? (int)((averageSegmentsMax[2] - averageSegmentsMin[1]) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMax[2] - athleteSegmentsMin[1]) * frecuenciaMuestreo): (int)((customSegmentsMax[2] - customSegmentsMin[1]) * frecuenciaMuestreo);

            int ventanaAnchoT2 = typePeople == 0 ? (int)((averageSegmentsMax[2] - averageSegmentsMin[1]) * frecuenciaMuestreo) : 
                                 typePeople == 1 ? (int)((athleteSegmentsMax[2] - athleteSegmentsMin[1]) * frecuenciaMuestreo) : 
                                                   (int)((customSegmentsMax[2] - customSegmentsMin[1]) * frecuenciaMuestreo);

            int ventanaAnchoQ2_2 = typePeople == 0 ? (int)((averageSegmentsMin[1] / 2.0) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMin[1] / 2.0) * frecuenciaMuestreo): (int)((customSegmentsMin[1] / 2.0) * frecuenciaMuestreo);
            int ventanaAnchoS2_2 = typePeople == 0 ? (int)((averageSegmentsMin[1] / 2.0) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMin[1] / 2.0) * frecuenciaMuestreo): (int)((customSegmentsMin[1] / 2.0) * frecuenciaMuestreo);
            int ventanaAnchoP2_2 = typePeople == 0 ? (int)((averageSegmentsMin[0] - (averageSegmentsMax[1] / 2.0)) * frecuenciaMuestreo) : typePeople == 1 ? (int)((athleteSegmentsMin[0] - (athleteSegmentsMax[1] / 2.0)) * frecuenciaMuestreo) : (int)((customSegmentsMin[0] - (customSegmentsMax[1] / 2.0)) * frecuenciaMuestreo);
            
            int ventanaAnchoT2_2 = typePeople == 0 ? (int)((averageSegmentsMin[2] - averageSegmentsMax[1]) * frecuenciaMuestreo) :
                                   typePeople == 1 ? (int)((athleteSegmentsMin[2] - athleteSegmentsMax[1]) * frecuenciaMuestreo) :
                                                     (int)((customSegmentsMin[2] - customSegmentsMax[1]) * frecuenciaMuestreo);

            int ventanaEnPosiciones = (int)((ventanaComparacion / 100.0) * herMod);

            //Prefixed
            //int ventanaAnchoQ = (int)(0.1 * frecuenciaMuestreo);
            //int ventanaAnchoS = (int)(0.1 * frecuenciaMuestreo);
            //int ventanaAnchoP = (int)(0.2 * frecuenciaMuestreo);
            //int ventanaAnchoT = (int)(0.08*frecuenciaMuestreo);// (int)(0.2 * frecuenciaMuestreo);//(int)(0.3 * frecuenciaMuestreo);



            double umbralPicoP = 0;
            //int ventanaPicoP = (int)(0.5 * frecuenciaMuestreo);

            double ancho = blockSize>2?1:blockSize / 2.0;

            if (R)
            {
                picosR1 = ECGpeaks.DetectarPicosR2(data1f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot/100.0));
                picosR2 = ECGpeaks.DetectarPicosR2(data2f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR3 = ECGpeaks.DetectarPicosR2(data3f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR4 = ECGpeaks.DetectarPicosR2(data4f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR5 = ECGpeaks.DetectarPicosR2(data5f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR6 = ECGpeaks.DetectarPicosR2(data6f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR7 = ECGpeaks.DetectarPicosR2(data7f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR8 = ECGpeaks.DetectarPicosR2(data8f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR9 = ECGpeaks.DetectarPicosR2(data9f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR10 = ECGpeaks.DetectarPicosR2(data10f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR11 = ECGpeaks.DetectarPicosR2(data11f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));
                picosR12 = ECGpeaks.DetectarPicosR2(data12f, ventanaAnchoR, ventanaRetraso, herMod, (umbralRuidoPot / 100.0));

                pintarPicosR(ancho);

                if (Q)
                {
                    //DETECCION DE PICOS Q
                    picosQ1 = ECGpeaks.DetectarPicosQ2(data1f, picosR1, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime/100.0));
                    picosQ2 = ECGpeaks.DetectarPicosQ2(data2f, picosR2, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ3 = ECGpeaks.DetectarPicosQ2(data3f, picosR3, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ4 = ECGpeaks.DetectarPicosQ2(data4f, picosR4, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ5 = ECGpeaks.DetectarPicosQ2(data5f, picosR5, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ6 = ECGpeaks.DetectarPicosQ2(data6f, picosR6, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ7 = ECGpeaks.DetectarPicosQ2(data7f, picosR7, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ8 = ECGpeaks.DetectarPicosQ2(data8f, picosR8, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ9 = ECGpeaks.DetectarPicosQ2(data9f, picosR9, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ10 = ECGpeaks.DetectarPicosQ2(data10f, picosR10, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ11 = ECGpeaks.DetectarPicosQ2(data11f, picosR11, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosQ12 = ECGpeaks.DetectarPicosQ2(data12f, picosR12, ventanaAnchoQ2, ventanaAnchoQ2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));

                    pintarPicosQ(ancho);

                    if (P)
                    {
                        //DETECCION PICOS P
                        picosP1 = ECGpeaks.DetectarPicosP1(data1f, picosQ1, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP2 = ECGpeaks.DetectarPicosP1(data2f, picosQ2, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP3 = ECGpeaks.DetectarPicosP1(data3f, picosQ3, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP4 = ECGpeaks.DetectarPicosP1(data4f, picosQ4, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP5 = ECGpeaks.DetectarPicosP1(data5f, picosQ5, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP6 = ECGpeaks.DetectarPicosP1(data6f, picosQ6, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP7 = ECGpeaks.DetectarPicosP1(data7f, picosQ7, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP8 = ECGpeaks.DetectarPicosP1(data8f, picosQ8, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP9 = ECGpeaks.DetectarPicosP1(data9f, picosQ9, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP10 = ECGpeaks.DetectarPicosP1(data10f, picosQ10, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP11 = ECGpeaks.DetectarPicosP1(data11f, picosQ11, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosP12 = ECGpeaks.DetectarPicosP1(data12f, picosQ12, ventanaAnchoP2, ventanaAnchoP2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);

                        pintarPicosP(ancho); 
                    }
                }

                if (S)
                {
                    //DETECCION DE PICOS S
                    picosS1 = ECGpeaks.DetectarPicosS2(data1f, picosR1, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS2 = ECGpeaks.DetectarPicosS2(data2f, picosR2, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS3 = ECGpeaks.DetectarPicosS2(data3f, picosR3, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS4 = ECGpeaks.DetectarPicosS2(data4f, picosR4, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS5 = ECGpeaks.DetectarPicosS2(data5f, picosR5, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS6 = ECGpeaks.DetectarPicosS2(data6f, picosR6, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS7 = ECGpeaks.DetectarPicosS2(data7f, picosR7, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS8 = ECGpeaks.DetectarPicosS2(data8f, picosR8, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS9 = ECGpeaks.DetectarPicosS2(data9f, picosR9, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS10 = ECGpeaks.DetectarPicosS2(data10f, picosR10, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS11 = ECGpeaks.DetectarPicosS2(data11f, picosR11, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));
                    picosS12 = ECGpeaks.DetectarPicosS2(data12f, picosR12, ventanaAnchoS2, ventanaAnchoS2_2, umbralPicoP, herMod, (umbralRuidoTime / 100.0));

                    pintarPicosS(ancho);

                    if (T)
                    {
                        //DETECCION PICOS T
                        picosT1 = ECGpeaks.DetectarPicosT1(data1f, picosS1, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT2 = ECGpeaks.DetectarPicosT1(data2f, picosS2, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT3 = ECGpeaks.DetectarPicosT1(data3f, picosS3, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT4 = ECGpeaks.DetectarPicosT1(data4f, picosS4, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT5 = ECGpeaks.DetectarPicosT1(data5f, picosS5, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT6 = ECGpeaks.DetectarPicosT1(data6f, picosS6, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT7 = ECGpeaks.DetectarPicosT1(data7f, picosS7, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT8 = ECGpeaks.DetectarPicosT1(data8f, picosS8, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT9 = ECGpeaks.DetectarPicosT1(data9f, picosS9, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT10 = ECGpeaks.DetectarPicosT1(data10f, picosS10, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT11 = ECGpeaks.DetectarPicosT1(data11f, picosS11, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);
                        picosT12 = ECGpeaks.DetectarPicosT1(data12f, picosS12, ventanaAnchoT2, ventanaAnchoT2_2, herMod, (umbralRuidoTime / 100.0), ventanaEnPosiciones);

                        pintarPicosT(ancho);
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

            generateReportToolStripMenuItem.Enabled = true;
            automaticPQRSTDetectionToolStripMenuItem.Enabled = false;
        }

        private void savePreviousFAF()
        {
            auxFAF1 = new List<double>(data1f);
            auxFAF2 = new List<double>(data2f);
            auxFAF3 = new List<double>(data3f);
            auxFAF4 = new List<double>(data4f);
            auxFAF5 = new List<double>(data5f);
            auxFAF6 = new List<double>(data6f);
            auxFAF7 = new List<double>(data7f);
            auxFAF8 = new List<double>(data8f);
            auxFAF9 = new List<double>(data9f);
            auxFAF10 = new List<double>(data10f);
            auxFAF11 = new List<double>(data11f);
            auxFAF12 = new List<double>(data12f);
            previousHertzs = herMod;
        }

        private void restorePreviousFAF()
        {
            data1f = new List<double>(auxFAF1);
            data2f = new List<double>(auxFAF2);
            data3f = new List<double>(auxFAF3);
            data4f = new List<double>(auxFAF4);
            data5f = new List<double>(auxFAF5);
            data6f = new List<double>(auxFAF6);
            data7f = new List<double>(auxFAF7);
            data8f = new List<double>(auxFAF8);
            data9f = new List<double>(auxFAF9);
            data10f = new List<double>(auxFAF10);
            data11f = new List<double>(auxFAF11);
            data12f = new List<double>(auxFAF12);
            herMod = previousHertzs;
            firstFAF = true;
            button5.Enabled = false;
        }

        private void savePreviousMAF()
        {
            auxMAF1 = new List<double>(data1f);
            auxMAF2 = new List<double>(data2f);
            auxMAF3 = new List<double>(data3f);
            auxMAF4 = new List<double>(data4f);
            auxMAF5 = new List<double>(data5f);
            auxMAF6 = new List<double>(data6f);
            auxMAF7 = new List<double>(data7f);
            auxMAF8 = new List<double>(data8f);
            auxMAF9 = new List<double>(data9f);
            auxMAF10 = new List<double>(data10f);
            auxMAF11 = new List<double>(data11f);
            auxMAF12 = new List<double>(data12f);
            previousHertzs = herMod;
        }

        private void restorePreviousMAF()
        {
            data1f = new List<double>(auxMAF1);
            data2f = new List<double>(auxMAF2);
            data3f = new List<double>(auxMAF3);
            data4f = new List<double>(auxMAF4);
            data5f = new List<double>(auxMAF5);
            data6f = new List<double>(auxMAF6);
            data7f = new List<double>(auxMAF7);
            data8f = new List<double>(auxMAF8);
            data9f = new List<double>(auxMAF9);
            data10f = new List<double>(auxMAF10);
            data11f = new List<double>(auxMAF11);
            data12f = new List<double>(auxMAF12);
            herMod = previousHertzs;
            firstMAF = true;
            button6.Enabled = false;
        }

        private void savePreviousMMF()
        {
            auxMMF1 = new List<double>(data1f);
            auxMMF2 = new List<double>(data2f);
            auxMMF3 = new List<double>(data3f);
            auxMMF4 = new List<double>(data4f);
            auxMMF5 = new List<double>(data5f);
            auxMMF6 = new List<double>(data6f);
            auxMMF7 = new List<double>(data7f);
            auxMMF8 = new List<double>(data8f);
            auxMMF9 = new List<double>(data9f);
            auxMMF10 = new List<double>(data10f);
            auxMMF11 = new List<double>(data11f);
            auxMMF12 = new List<double>(data12f);
            previousHertzs = herMod;
        }

        private void restorePreviousMMF()
        {
            data1f = new List<double>(auxMMF1);
            data2f = new List<double>(auxMMF2);
            data3f = new List<double>(auxMMF3);
            data4f = new List<double>(auxMMF4);
            data5f = new List<double>(auxMMF5);
            data6f = new List<double>(auxMMF6);
            data7f = new List<double>(auxMMF7);
            data8f = new List<double>(auxMMF8);
            data9f = new List<double>(auxMMF9);
            data10f = new List<double>(auxMMF10);
            data11f = new List<double>(auxMMF11);
            data12f = new List<double>(auxMMF12);
            
            herMod = previousHertzs;
            button8.Enabled = false;
        }

        private void savePreviousBSF()
        {
            auxBSF1 = new List<double>(data1f);
            auxBSF2 = new List<double>(data2f);
            auxBSF3 = new List<double>(data3f);
            auxBSF4 = new List<double>(data4f);
            auxBSF5 = new List<double>(data5f);
            auxBSF6 = new List<double>(data6f);
            auxBSF7 = new List<double>(data7f);
            auxBSF8 = new List<double>(data8f);
            auxBSF9 = new List<double>(data9f);
            auxBSF10 = new List<double>(data10f);
            auxBSF11 = new List<double>(data11f);
            auxBSF12 = new List<double>(data12f);
            previousHertzs = herMod;
        }

        private void restorePreviousBSF()
        {
            data1f = new List<double>(auxBSF1);
            data2f = new List<double>(auxBSF2);
            data3f = new List<double>(auxBSF3);
            data4f = new List<double>(auxBSF4);
            data5f = new List<double>(auxBSF5);
            data6f = new List<double>(auxBSF6);
            data7f = new List<double>(auxBSF7);
            data8f = new List<double>(auxBSF8);
            data9f = new List<double>(auxBSF9);
            data10f = new List<double>(auxBSF10);
            data11f = new List<double>(auxBSF11);
            data12f = new List<double>(auxBSF12);

            herMod = previousHertzs;
            button10.Enabled = false;
        }

        private void savePreviousSUB()
        {
            auxSUB1 = new List<double>(data1f);
            auxSUB2 = new List<double>(data2f);
            auxSUB3 = new List<double>(data3f);
            auxSUB4 = new List<double>(data4f);
            auxSUB5 = new List<double>(data5f);
            auxSUB6 = new List<double>(data6f);
            auxSUB7 = new List<double>(data7f);
            auxSUB8 = new List<double>(data8f);
            auxSUB9 = new List<double>(data9f);
            auxSUB10 = new List<double>(data10f);
            auxSUB11 = new List<double>(data11f);
            auxSUB12 = new List<double>(data12f);
            previousHertzs = herMod;
        }

        private void restorePreviousSUB()
        {
            data1f = new List<double>(auxSUB1);
            data2f = new List<double>(auxSUB2);
            data3f = new List<double>(auxSUB3);
            data4f = new List<double>(auxSUB4);
            data5f = new List<double>(auxSUB5);
            data6f = new List<double>(auxSUB6);
            data7f = new List<double>(auxSUB7);
            data8f = new List<double>(auxSUB8);
            data9f = new List<double>(auxSUB9);
            data10f = new List<double>(auxSUB10);
            data11f = new List<double>(auxSUB11);
            data12f = new List<double>(auxSUB12);

            herMod = previousHertzs;
            button13.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (firstFAF)
            {
                savePreviousFAF();
                firstFAF = false;
            }
            if(trackBar1.Value > 1)
            {
                data1f = Filters.FixedAverageFilter(data1f, trackBar1.Value);
                data2f = Filters.FixedAverageFilter(data2f, trackBar1.Value);
                data3f = Filters.FixedAverageFilter(data3f, trackBar1.Value);
                data4f = Filters.FixedAverageFilter(data4f, trackBar1.Value);
                data5f = Filters.FixedAverageFilter(data5f, trackBar1.Value);
                data6f = Filters.FixedAverageFilter(data6f, trackBar1.Value);
                data7f = Filters.FixedAverageFilter(data7f, trackBar1.Value);
                data8f = Filters.FixedAverageFilter(data8f, trackBar1.Value);
                data9f = Filters.FixedAverageFilter(data9f, trackBar1.Value);
                data10f = Filters.FixedAverageFilter(data10f, trackBar1.Value);
                data11f = Filters.FixedAverageFilter(data11f, trackBar1.Value);
                data12f = Filters.FixedAverageFilter(data12f, trackBar1.Value);

                finFAF1 = new List<double>(data1f);
                finFAF2 = new List<double>(data2f);
                finFAF3 = new List<double>(data3f);
                finFAF4 = new List<double>(data4f);
                finFAF5 = new List<double>(data5f);
                finFAF6 = new List<double>(data6f);
                finFAF7 = new List<double>(data7f);
                finFAF8 = new List<double>(data8f);
                finFAF9 = new List<double>(data9f);
                finFAF10 = new List<double>(data10f);
                finFAF11 = new List<double>(data11f);
                finFAF12 = new List<double>(data12f);

                herMod = herMod / trackBar1.Value;
                label14.Text = herMod.ToString();

                blockSize = Math.Round((2 * herMod) * (1.0 / (double)herMod), 1);
                drawECG();
                comboBox1.Items.Add("A");
                comboBox2.Items.Add("A");
                button5.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            restorePreviousFAF();
            label14.Text = herMod.ToString();

            blockSize = Math.Round((2 * herMod) * (1.0 / (double)herMod), 1);
            drawECG();
            comboBox1.Items.Remove("A");
            comboBox2.Items.Remove("A");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (firstMAF)
            {
                savePreviousMAF();
                firstMAF = false;
            }
            if (trackBar2.Value > 1)
            {
                data1f = Filters.MovingAverageFilter(data1f, trackBar2.Value);
                data2f = Filters.MovingAverageFilter(data2f, trackBar2.Value);
                data3f = Filters.MovingAverageFilter(data3f, trackBar2.Value);
                data4f = Filters.MovingAverageFilter(data4f, trackBar2.Value);
                data5f = Filters.MovingAverageFilter(data5f, trackBar2.Value);
                data6f = Filters.MovingAverageFilter(data6f, trackBar2.Value);
                data7f = Filters.MovingAverageFilter(data7f, trackBar2.Value);
                data8f = Filters.MovingAverageFilter(data8f, trackBar2.Value);
                data9f = Filters.MovingAverageFilter(data9f, trackBar2.Value);
                data10f = Filters.MovingAverageFilter(data10f, trackBar2.Value);
                data11f = Filters.MovingAverageFilter(data11f, trackBar2.Value);
                data12f = Filters.MovingAverageFilter(data12f, trackBar2.Value);

                finMAF1 = new List<double>(data1f);
                finMAF2 = new List<double>(data2f);
                finMAF3 = new List<double>(data3f);
                finMAF4 = new List<double>(data4f);
                finMAF5 = new List<double>(data5f);
                finMAF6 = new List<double>(data6f);
                finMAF7 = new List<double>(data7f);
                finMAF8 = new List<double>(data8f);
                finMAF9 = new List<double>(data9f);
                finMAF10 = new List<double>(data10f);
                finMAF11 = new List<double>(data11f);
                finMAF12 = new List<double>(data12f);

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
            if (firstMMF)
            {
                savePreviousMMF();
                firstMMF = false;
            }
            if (trackBar3.Value > 1)
            {
                data1f = Filters.MovingAverageFilter(data1f, trackBar3.Value);
                data2f = Filters.MovingAverageFilter(data2f, trackBar3.Value);
                data3f = Filters.MovingAverageFilter(data3f, trackBar3.Value);
                data4f = Filters.MovingAverageFilter(data4f, trackBar3.Value);
                data5f = Filters.MovingAverageFilter(data5f, trackBar3.Value);
                data6f = Filters.MovingAverageFilter(data6f, trackBar3.Value);
                data7f = Filters.MovingAverageFilter(data7f, trackBar3.Value);
                data8f = Filters.MovingAverageFilter(data8f, trackBar3.Value);
                data9f = Filters.MovingAverageFilter(data9f, trackBar3.Value);
                data10f = Filters.MovingAverageFilter(data10f, trackBar3.Value);
                data11f = Filters.MovingAverageFilter(data11f, trackBar3.Value);
                data12f = Filters.MovingAverageFilter(data12f, trackBar3.Value);

                finMMF1 = new List<double>(data1f);
                finMMF2 = new List<double>(data2f);
                finMMF3 = new List<double>(data3f);
                finMMF4 = new List<double>(data4f);
                finMMF5 = new List<double>(data5f);
                finMMF6 = new List<double>(data6f);
                finMMF7 = new List<double>(data7f);
                finMMF8 = new List<double>(data8f);
                finMMF9 = new List<double>(data9f);
                finMMF10 = new List<double>(data10f);
                finMMF11 = new List<double>(data11f);
                finMMF12 = new List<double>(data12f);

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
            if (firstBSF)
            {
                savePreviousBSF();
                firstBSF = false;
            }
            if (trackBar4.Value > 0)
            {
                data1f = Filters.BandRejectionFilter(data1f, herMod, trackBar4.Value, 10);
                data2f = Filters.BandRejectionFilter(data2f, herMod, trackBar4.Value, 10);
                data3f = Filters.BandRejectionFilter(data3f, herMod, trackBar4.Value, 10);
                data4f = Filters.BandRejectionFilter(data4f, herMod, trackBar4.Value, 10);
                data5f = Filters.BandRejectionFilter(data5f, herMod, trackBar4.Value, 10);
                data6f = Filters.BandRejectionFilter(data6f, herMod, trackBar4.Value, 10);
                data7f = Filters.BandRejectionFilter(data7f, herMod, trackBar4.Value, 10);
                data8f = Filters.BandRejectionFilter(data8f, herMod, trackBar4.Value, 10);
                data9f = Filters.BandRejectionFilter(data9f, herMod, trackBar4.Value, 10);
                data10f = Filters.BandRejectionFilter(data10f, herMod, trackBar4.Value, 10);
                data11f = Filters.BandRejectionFilter(data11f, herMod, trackBar4.Value, 10);
                data12f = Filters.BandRejectionFilter(data12f, herMod, trackBar4.Value, 10);

                finBSF1 = new List<double>(data1f);
                finBSF2 = new List<double>(data2f);
                finBSF3 = new List<double>(data3f);
                finBSF4 = new List<double>(data4f);
                finBSF5 = new List<double>(data5f);
                finBSF6 = new List<double>(data6f);
                finBSF7 = new List<double>(data7f);
                finBSF8 = new List<double>(data8f);
                finBSF9 = new List<double>(data9f);
                finBSF10 = new List<double>(data10f);
                finBSF11 = new List<double>(data11f);
                finBSF12 = new List<double>(data12f);

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
            if (firstSUB)
            {
                savePreviousSUB();
                firstSUB = false;
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
                switch (comboBox1.GetItemText(comboBox1.SelectedItem))
                {
                    case "RAW":
                        sub1_1 = new List<double>(data1f);
                        sub1_2 = new List<double>(data2f);
                        sub1_3 = new List<double>(data3f);
                        sub1_4 = new List<double>(data4f);
                        sub1_5 = new List<double>(data5f);
                        sub1_6 = new List<double>(data6f);
                        sub1_7 = new List<double>(data7f);
                        sub1_8 = new List<double>(data8f);
                        sub1_9 = new List<double>(data9f);
                        sub1_10 = new List<double>(data10f);
                        sub1_11 = new List<double>(data11f);
                        sub1_12 = new List<double>(data12f);
                        break;
                    case "A":
                        sub1_1 = new List<double>(finFAF1);
                        sub1_2 = new List<double>(finFAF2);
                        sub1_3 = new List<double>(finFAF3);
                        sub1_4 = new List<double>(finFAF4);
                        sub1_5 = new List<double>(finFAF5);
                        sub1_6 = new List<double>(finFAF6);
                        sub1_7 = new List<double>(finFAF7);
                        sub1_8 = new List<double>(finFAF8);
                        sub1_9 = new List<double>(finFAF9);
                        sub1_10 = new List<double>(finFAF10);
                        sub1_11 = new List<double>(finFAF11);
                        sub1_12 = new List<double>(finFAF12);
                        break;
                    case "B":
                        sub1_1 = new List<double>(finMAF1);
                        sub1_2 = new List<double>(finMAF2);
                        sub1_3 = new List<double>(finMAF3);
                        sub1_4 = new List<double>(finMAF4);
                        sub1_5 = new List<double>(finMAF5);
                        sub1_6 = new List<double>(finMAF6);
                        sub1_7 = new List<double>(finMAF7);
                        sub1_8 = new List<double>(finMAF8);
                        sub1_9 = new List<double>(finMAF9);
                        sub1_10 = new List<double>(finMAF10);
                        sub1_11 = new List<double>(finMAF11);
                        sub1_12 = new List<double>(finMAF12);
                        break;
                    case "C":
                        sub1_1 = new List<double>(finMMF1);
                        sub1_2 = new List<double>(finMMF2);
                        sub1_3 = new List<double>(finMMF3);
                        sub1_4 = new List<double>(finMMF4);
                        sub1_5 = new List<double>(finMMF5);
                        sub1_6 = new List<double>(finMMF6);
                        sub1_7 = new List<double>(finMMF7);
                        sub1_8 = new List<double>(finMMF8);
                        sub1_9 = new List<double>(finMMF9);
                        sub1_10 = new List<double>(finMMF10);
                        sub1_11 = new List<double>(finMMF11);
                        sub1_12 = new List<double>(finMMF12);
                        break;
                    case "D":
                        sub1_1 = new List<double>(finBSF1);
                        sub1_2 = new List<double>(finBSF2);
                        sub1_3 = new List<double>(finBSF3);
                        sub1_4 = new List<double>(finBSF4);
                        sub1_5 = new List<double>(finBSF5);
                        sub1_6 = new List<double>(finBSF6);
                        sub1_7 = new List<double>(finBSF7);
                        sub1_8 = new List<double>(finBSF8);
                        sub1_9 = new List<double>(finBSF9);
                        sub1_10 = new List<double>(finBSF10);
                        sub1_11 = new List<double>(finBSF11);
                        sub1_12 = new List<double>(finBSF12);
                        break;
                }
                switch (comboBox2.GetItemText(comboBox2.SelectedItem))
                {
                    case "RAW":
                        sub2_1 = new List<double>(data1f);
                        sub2_2 = new List<double>(data2f);
                        sub2_3 = new List<double>(data3f);
                        sub2_4 = new List<double>(data4f);
                        sub2_5 = new List<double>(data5f);
                        sub2_6 = new List<double>(data6f);
                        sub2_7 = new List<double>(data7f);
                        sub2_8 = new List<double>(data8f);
                        sub2_9 = new List<double>(data9f);
                        sub2_10 = new List<double>(data10f);
                        sub2_11 = new List<double>(data11f);
                        sub2_12 = new List<double>(data12f);
                        break;
                    case "A":
                        sub2_1 = new List<double>(finFAF1);
                        sub2_2 = new List<double>(finFAF2);
                        sub2_3 = new List<double>(finFAF3);
                        sub2_4 = new List<double>(finFAF4);
                        sub2_5 = new List<double>(finFAF5);
                        sub2_6 = new List<double>(finFAF6);
                        sub2_7 = new List<double>(finFAF7);
                        sub2_8 = new List<double>(finFAF8);
                        sub2_9 = new List<double>(finFAF9);
                        sub2_10 = new List<double>(finFAF10);
                        sub2_11 = new List<double>(finFAF11);
                        sub2_12 = new List<double>(finFAF12);
                        break;
                    case "B":
                        sub2_1 = new List<double>(finMAF1);
                        sub2_2 = new List<double>(finMAF2);
                        sub2_3 = new List<double>(finMAF3);
                        sub2_4 = new List<double>(finMAF4);
                        sub2_5 = new List<double>(finMAF5);
                        sub2_6 = new List<double>(finMAF6);
                        sub2_7 = new List<double>(finMAF7);
                        sub2_8 = new List<double>(finMAF8);
                        sub2_9 = new List<double>(finMAF9);
                        sub2_10 = new List<double>(finMAF10);
                        sub2_11 = new List<double>(finMAF11);
                        sub2_12 = new List<double>(finMAF12);
                        break;
                    case "C":
                        sub2_1 = new List<double>(finMMF1);
                        sub2_2 = new List<double>(finMMF2);
                        sub2_3 = new List<double>(finMMF3);
                        sub2_4 = new List<double>(finMMF4);
                        sub2_5 = new List<double>(finMMF5);
                        sub2_6 = new List<double>(finMMF6);
                        sub2_7 = new List<double>(finMMF7);
                        sub2_8 = new List<double>(finMMF8);
                        sub2_9 = new List<double>(finMMF9);
                        sub2_10 = new List<double>(finMMF10);
                        sub2_11 = new List<double>(finMMF11);
                        sub2_12 = new List<double>(finMMF12);
                        break;
                    case "D":
                        sub2_1 = new List<double>(finBSF1);
                        sub2_2 = new List<double>(finBSF2);
                        sub2_3 = new List<double>(finBSF3);
                        sub2_4 = new List<double>(finBSF4);
                        sub2_5 = new List<double>(finBSF5);
                        sub2_6 = new List<double>(finBSF6);
                        sub2_7 = new List<double>(finBSF7);
                        sub2_8 = new List<double>(finBSF8);
                        sub2_9 = new List<double>(finBSF9);
                        sub2_10 = new List<double>(finBSF10);
                        sub2_11 = new List<double>(finBSF11);
                        sub2_12 = new List<double>(finBSF12);
                        break;
                }
                data1f = Filters.SubtractSignals(sub1_1, sub2_1);
                data2f = Filters.SubtractSignals(sub1_2, sub2_2);
                data3f = Filters.SubtractSignals(sub1_3, sub2_3);
                data4f = Filters.SubtractSignals(sub1_4, sub2_4);
                data5f = Filters.SubtractSignals(sub1_5, sub2_5);
                data6f = Filters.SubtractSignals(sub1_6, sub2_6);
                data7f = Filters.SubtractSignals(sub1_7, sub2_7);
                data8f = Filters.SubtractSignals(sub1_8, sub2_8);
                data9f = Filters.SubtractSignals(sub1_9, sub2_9);
                data10f = Filters.SubtractSignals(sub1_10, sub2_10);
                data11f = Filters.SubtractSignals(sub1_11, sub2_11);
                data12f = Filters.SubtractSignals(sub1_12, sub2_12);

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
            int windowSize = herMod / 50;
            ventanaFiltroMedia = herMod / 100;

            data1f = Filters.FixedAverageFilter(data1f, ventanaFiltroMedia);
            data2f = Filters.FixedAverageFilter(data2f, ventanaFiltroMedia);
            data3f = Filters.FixedAverageFilter(data3f, ventanaFiltroMedia);
            data4f = Filters.FixedAverageFilter(data4f, ventanaFiltroMedia);
            data5f = Filters.FixedAverageFilter(data5f, ventanaFiltroMedia);
            data6f = Filters.FixedAverageFilter(data6f, ventanaFiltroMedia);
            data7f = Filters.FixedAverageFilter(data7f, ventanaFiltroMedia);
            data8f = Filters.FixedAverageFilter(data8f, ventanaFiltroMedia);
            data9f = Filters.FixedAverageFilter(data9f, ventanaFiltroMedia);
            data10f = Filters.FixedAverageFilter(data10f, ventanaFiltroMedia);
            data11f = Filters.FixedAverageFilter(data11f, ventanaFiltroMedia);
            data12f = Filters.FixedAverageFilter(data12f, ventanaFiltroMedia);

            herMod = herMod / ventanaFiltroMedia;
            label14.Text = herMod.ToString();

            List<double> filteredSignal1 = Filters.MovingAverageFilter(data1f, windowSize);
            List<double> filteredSignal2 = Filters.MovingAverageFilter(data2f, windowSize);
            List<double> filteredSignal3 = Filters.MovingAverageFilter(data3f, windowSize);
            List<double> filteredSignal4 = Filters.MovingAverageFilter(data4f, windowSize);
            List<double> filteredSignal5 = Filters.MovingAverageFilter(data5f, windowSize);
            List<double> filteredSignal6 = Filters.MovingAverageFilter(data6f, windowSize);
            List<double> filteredSignal7 = Filters.MovingAverageFilter(data7f, windowSize);
            List<double> filteredSignal8 = Filters.MovingAverageFilter(data8f, windowSize);
            List<double> filteredSignal9 = Filters.MovingAverageFilter(data9f, windowSize);
            List<double> filteredSignal10 = Filters.MovingAverageFilter(data10f, windowSize);
            List<double> filteredSignal11 = Filters.MovingAverageFilter(data11f, windowSize);
            List<double> filteredSignal12 = Filters.MovingAverageFilter(data12f, windowSize);

            /*data1f = filteredSignal1;
            data2f = filteredSignal2;
            data3f = filteredSignal3;
            data4f = filteredSignal4;
            data5f = filteredSignal5;
            data6f = filteredSignal6;
            data7f = filteredSignal7;
            data8f = filteredSignal8;
            data9f = filteredSignal9;
            data10f = filteredSignal10;
            data11f = filteredSignal11;
            data12f = filteredSignal12;*/

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

            /*data1f = ecgMedianFiltered1;
            data2f = ecgMedianFiltered2;
            data3f = ecgMedianFiltered3;
            data4f = ecgMedianFiltered4;
            data5f = ecgMedianFiltered5;
            data6f = ecgMedianFiltered6;
            data7f = ecgMedianFiltered7;
            data8f = ecgMedianFiltered8;
            data9f = ecgMedianFiltered9;
            data10f = ecgMedianFiltered10;
            data11f = ecgMedianFiltered11;
            data12f = ecgMedianFiltered12;*/

            data1f = Filters.SubtractSignals(data1f, ecgMedianFiltered1);
            data2f = Filters.SubtractSignals(data2f, ecgMedianFiltered2);
            data3f = Filters.SubtractSignals(data3f, ecgMedianFiltered3);
            data4f = Filters.SubtractSignals(data4f, ecgMedianFiltered4);
            data5f = Filters.SubtractSignals(data5f, ecgMedianFiltered5);
            data6f = Filters.SubtractSignals(data6f, ecgMedianFiltered6);
            data7f = Filters.SubtractSignals(data7f, ecgMedianFiltered7);
            data8f = Filters.SubtractSignals(data8f, ecgMedianFiltered8);
            data9f = Filters.SubtractSignals(data9f, ecgMedianFiltered9);
            data10f = Filters.SubtractSignals(data10f, ecgMedianFiltered10);
            data11f = Filters.SubtractSignals(data11f, ecgMedianFiltered11);
            data12f = Filters.SubtractSignals(data12f, ecgMedianFiltered12);

            data1f = Filters.BandRejectionFilter(data1f, 100, 55, 10);
            data2f = Filters.BandRejectionFilter(data2f, 100, 55, 10);
            data3f = Filters.BandRejectionFilter(data3f, 100, 55, 10);
            data4f = Filters.BandRejectionFilter(data4f, 100, 55, 10);
            data5f = Filters.BandRejectionFilter(data5f, 100, 55, 10);
            data6f = Filters.BandRejectionFilter(data6f, 100, 55, 10);
            data7f = Filters.BandRejectionFilter(data7f, 100, 55, 10);
            data8f = Filters.BandRejectionFilter(data8f, 100, 55, 10);
            data9f = Filters.BandRejectionFilter(data9f, 100, 55, 10);
            data10f = Filters.BandRejectionFilter(data10f, 100, 55, 10);
            data11f = Filters.BandRejectionFilter(data11f, 100, 55, 10);
            data12f = Filters.BandRejectionFilter(data12f, 100, 55, 10);

            //blockSize = blockSize / 5;
            blockSize = Math.Round((2 * herMod) * (1.0 / (double)herMod),1);
            //blockSize = 2 * 60 * (1.0/her);
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
            if (lines1.Count == 0 && lines2.Count == 0 && lines3.Count == 0 && lines4.Count == 0 && lines5.Count == 0 && lines6.Count == 0 && lines7.Count == 0 && lines8.Count == 0 && lines9.Count == 0 && lines10.Count == 0 && lines11.Count == 0 && lines12.Count == 0)
            {
                MessageBox.Show("no marks recorded", "Error");
            }
            else
            {
                string s = (fileName.Replace(".XML", "")) + "_annotations.XML";
                Console.WriteLine(s);
                TextWriter txt = new StreamWriter(s);
                txt.Write("<?xml version=\"1.0\" encoding=\"ISO - 8859 - 1\" ?>\n");
                txt.Write("\t< CardiologyAnnotations >\n");
                if (lines1.Count > 0)
                {
                    txt.Write("\t\t< I >\n");
                    int i = 0;
                    foreach (StripLine sl in lines1)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot1[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ I >\n");
                }
                if (lines2.Count > 0)
                {
                    txt.Write("\t\t< II >\n");

                    int i = 0;
                    foreach (StripLine sl in lines2)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot2[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ II >\n");
                }
                if (lines3.Count > 0)
                {
                    txt.Write("\t\t< III >\n");

                    int i = 0;
                    foreach (StripLine sl in lines3)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot3[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ III >\n");
                }
                if (lines4.Count > 0)
                {
                    txt.Write("\t\t< aVR >\n");

                    int i = 0;
                    foreach (StripLine sl in lines4)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot4[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVR >\n");
                }
                if (lines5.Count > 0)
                {
                    txt.Write("\t\t< aVL >\n");

                    int i = 0;
                    foreach (StripLine sl in lines5)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot5[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVL >\n");
                }
                if (lines6.Count > 0)
                {
                    txt.Write("\t\t< aVF >\n");

                    int i = 0;
                    foreach (StripLine sl in lines6)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot6[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ aVF >\n");
                }
                if (lines7.Count > 0)
                {
                    txt.Write("\t\t< V1 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines7)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot7[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V1 >\n");
                }
                if (lines8.Count > 0)
                {
                    txt.Write("\t\t< V2 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines8)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot8[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V2 >\n");
                }
                if (lines9.Count > 0)
                {
                    txt.Write("\t\t< V3 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines9)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot9[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V3 >\n");
                }
                if (lines10.Count > 0)
                {
                    txt.Write("\t\t< V4 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines10)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot10[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V4 >\n");
                }
                if (lines11.Count > 0)
                {
                    txt.Write("\t\t< V5 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines11)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot11[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V5 >\n");
                }
                if (lines12.Count > 0)
                {
                    txt.Write("\t\t< V6 >\n");

                    int i = 0;
                    foreach (StripLine sl in lines12)
                    {
                        txt.Write("\t\t\t< mark >\n");

                        txt.Write("\t\t\t\t< time >" + sl.IntervalOffset + "</ time >\n");
                        txt.Write("\t\t\t\t< annot >" + annot12[i] + "</ annot >\n");

                        txt.Write("\t\t\t</ mark >\n");
                        i++;
                    }

                    txt.Write("\t\t</ V6 >\n");
                }

                txt.Write("</ CardiologyAnnotations >");
                txt.Close();
            }
        }

        private void addLabelsManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
        }

        private void cargarECGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sr = new StreamReader(openFileDialog1.FileName);
                    //
                    fileName = openFileDialog1.FileName;
                    doc.Load(fileName);
                    if(archivoCargado)
                        eraseData();
                    basicData();
                    if (correcto)
                    {
                        clearECG();
                        ecgDataLoadFromFile();
                        addLabelsAutomaticallyToolStripMenuItem.Enabled = true;
                        filteringToolStripMenuItem.Enabled = true;
                        rESTORESIGNALSToolStripMenuItem.Enabled = true;
                        archivoCargado = true;
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
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void basicData()
        {
            //Compruebo que es el ECG de General Electric
            try
            {
                node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/ClinicalInfo/DeviceInfo/Desc");

                string ecgtype = node.InnerText;

                if (ecgtype == "CardioSoft")
                {

                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Age");
                    age = node.InnerText;
                    label6.Text = age;
                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Gender");
                    gender = node.InnerText;
                    label7.Text = gender;
                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Race");
                    race = node.InnerText;
                    label8.Text = race;
                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Height");
                    height = node.InnerText;
                    label9.Text = height;
                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/PatientInfo/Weight");
                    weight = node.InnerText;
                    label10.Text = weight;

                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/SampleRate");
                    hertz = node.InnerText;
                    label14.Text = hertz;
                    her = Convert.ToInt32(hertz);
                    herMod = her;

                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/ChannelSampleCountTotal");
                    secs = (Convert.ToInt32(node.InnerText) / herMod).ToString();
                    label16.Text = secs;

                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData/Resolution");
                    resolution = node.InnerText;
                    resol = Convert.ToInt32(resolution);
                    label24.Text = resolution;

                    groupBox1.Visible = true;

                    node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/RestingECGMeasurements/VentricularRate");

                    bmpsFile = Convert.ToDouble(node.InnerText);

                    correcto = true;
                }
                else
                {
                    correcto = false;
                    MessageBox.Show("Incorrect File");
                }
            }
            catch(Exception)
            {
                correcto = false;
                MessageBox.Show("Incorrect File");
            }

        }

        private void ecgDataLoadFromFile()
        {
            
            node = doc.DocumentElement.SelectSingleNode("/CardiologyXML/StripData");
            string[] result1 = node.ChildNodes[4].InnerText.Split(',');
            string[] result2 = node.ChildNodes[5].InnerText.Split(',');
            string[] result3 = node.ChildNodes[6].InnerText.Split(',');
            string[] result4 = node.ChildNodes[7].InnerText.Split(',');
            string[] result5 = node.ChildNodes[8].InnerText.Split(',');
            string[] result6 = node.ChildNodes[9].InnerText.Split(',');
            string[] result7 = node.ChildNodes[10].InnerText.Split(',');
            string[] result8 = node.ChildNodes[11].InnerText.Split(',');
            string[] result9 = node.ChildNodes[12].InnerText.Split(',');
            string[] result10 = node.ChildNodes[13].InnerText.Split(',');
            string[] result11 = node.ChildNodes[14].InnerText.Split(',');
            string[] result12 = node.ChildNodes[15].InnerText.Split(',');

            

            foreach (string s in result1)
            {
                data1.Add(Double.Parse(s)*resol/1000);
                data1f.Add(Double.Parse(s) * resol / 1000);

            }
            foreach (string s in result2)
            {
                data2.Add(Double.Parse(s) * resol / 1000);
                data2f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result3)
            {
                data3.Add(Double.Parse(s) * resol / 1000);
                data3f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result4)
            {
                data4.Add(Double.Parse(s) * resol / 1000);
                data4f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result5)
            {
                data5.Add(Double.Parse(s) * resol / 1000);
                data5f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result6)
            {
                data6.Add(Double.Parse(s) * resol / 1000);
                data6f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result7)
            {
                data7.Add(Double.Parse(s) * resol / 1000);
                data7f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result8)
            {
                data8.Add(Double.Parse(s) * resol / 1000);
                data8f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result9)
            {
                data9.Add(Double.Parse(s) * resol / 1000);
                data9f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result10)
            {
                data10.Add(Double.Parse(s) * resol / 1000);
                data10f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result11)
            {
                data11.Add(Double.Parse(s) * resol / 1000);
                data11f.Add(Double.Parse(s) * resol / 1000);
            }
            foreach (string s in result12)
            {
                data12.Add(Double.Parse(s) * resol / 1000);
                data12f.Add(Double.Parse(s) * resol / 1000);
            }
            blockSize = Math.Round((2 * herMod) * (1.0 / (double)herMod), 1);
            drawECG();
        }

        private void eraseData()
        {
            data1.Clear();
            data2.Clear();
            data3.Clear();
            data4.Clear();
            data5.Clear();
            data6.Clear();
            data7.Clear();
            data8.Clear();
            data9.Clear();
            data10.Clear();
            data11.Clear();
            data12.Clear();

            data1f.Clear();
            data2f.Clear();
            data3f.Clear();
            data4f.Clear();
            data5f.Clear();
            data6f.Clear();
            data7f.Clear();
            data8f.Clear();
            data9f.Clear();
            data10f.Clear();
            data11f.Clear();
            data12f.Clear();

            auxFAF1.Clear();
            auxFAF2.Clear();
            auxFAF3.Clear();
            auxFAF4.Clear();
            auxFAF5.Clear();
            auxFAF6.Clear();
            auxFAF7.Clear();
            auxFAF8.Clear();
            auxFAF9.Clear();
            auxFAF10.Clear();
            auxFAF11.Clear();
            auxFAF12.Clear();

            auxMAF1.Clear();
            auxMAF2.Clear();
            auxMAF3.Clear();
            auxMAF4.Clear();
            auxMAF5.Clear();
            auxMAF6.Clear();
            auxMAF7.Clear();
            auxMAF8.Clear();
            auxMAF9.Clear();
            auxMAF10.Clear();
            auxMAF11.Clear();
            auxMAF12.Clear();

            auxMMF1.Clear();
            auxMMF2.Clear();
            auxMMF3.Clear();
            auxMMF4.Clear();
            auxMMF5.Clear();
            auxMMF6.Clear();
            auxMMF7.Clear();
            auxMMF8.Clear();
            auxMMF9.Clear();
            auxMMF10.Clear();
            auxMMF11.Clear();
            auxMMF12.Clear();

            auxBSF1.Clear();
            auxBSF2.Clear();
            auxBSF3.Clear();
            auxBSF4.Clear();
            auxBSF5.Clear();
            auxBSF6.Clear();
            auxBSF7.Clear();
            auxBSF8.Clear();
            auxBSF9.Clear();
            auxBSF10.Clear();
            auxBSF11.Clear();
            auxBSF12.Clear();

            auxSUB1.Clear();
            auxSUB2.Clear();
            auxSUB3.Clear();
            auxSUB4.Clear();
            auxSUB5.Clear();
            auxSUB6.Clear();
            auxSUB7.Clear();
            auxSUB8.Clear();
            auxSUB9.Clear();
            auxSUB10.Clear();
            auxSUB11.Clear();
            auxSUB12.Clear();




            finFAF1.Clear();
            finFAF2.Clear();
            finFAF3.Clear();
            finFAF4.Clear();
            finFAF5.Clear();
            finFAF6.Clear();
            finFAF7.Clear();
            finFAF8.Clear();
            finFAF9.Clear();
            finFAF10.Clear();
            finFAF11.Clear();
            finFAF12.Clear();

            finMAF1.Clear();
            finMAF2.Clear();
            finMAF3.Clear();
            finMAF4.Clear();
            finMAF5.Clear();
            finMAF6.Clear();
            finMAF7.Clear();
            finMAF8.Clear();
            finMAF9.Clear();
            finMAF10.Clear();
            finMAF11.Clear();
            finMAF12.Clear();

            finMMF1.Clear();
            finMMF2.Clear();
            finMMF3.Clear();
            finMMF4.Clear();
            finMMF5.Clear();
            finMMF6.Clear();
            finMMF7.Clear();
            finMMF8.Clear();
            finMMF9.Clear();
            finMMF10.Clear();
            finMMF11.Clear();
            finMMF12.Clear();

            finBSF1.Clear();
            finBSF2.Clear();
            finBSF3.Clear();
            finBSF4.Clear();
            finBSF5.Clear();
            finBSF6.Clear();
            finBSF7.Clear();
            finBSF8.Clear();
            finBSF9.Clear();
            finBSF10.Clear();
            finBSF11.Clear();
            finBSF12.Clear();

            lines1.Clear();
            lines2.Clear();
            lines3.Clear();
            lines4.Clear();
            lines5.Clear();
            lines6.Clear();
            lines7.Clear();
            lines8.Clear();
            lines9.Clear();
            lines10.Clear();
            lines11.Clear();
            lines12.Clear();

            annot1.Clear();
            annot2.Clear();
            annot3.Clear();
            annot4.Clear();
            annot5.Clear();
            annot6.Clear();
            annot7.Clear();
            annot8.Clear();
            annot9.Clear();
            annot10.Clear();
            annot11.Clear();
            annot12.Clear();

            if(picosP1!=null)
                picosP1.Clear();
            if (picosP2 != null) 
                picosP2.Clear();
            if (picosP3 != null) 
                picosP3.Clear();
            if (picosP4 != null) 
                picosP4.Clear();
            if (picosP5 != null) 
                picosP5.Clear();
            if (picosP6 != null) 
                picosP6.Clear();
            if (picosP7 != null) 
                picosP7.Clear();
            if (picosP8 != null) 
                picosP8.Clear();
            if (picosP9 != null) 
                picosP9.Clear();
            if (picosP10 != null) 
                picosP10.Clear();
            if (picosP11 != null) 
                picosP11.Clear();
            if (picosP12 != null) 
                picosP12.Clear();

            if(picosQ1!=null) 
                picosQ1.Clear();
            if (picosQ2 != null) 
                picosQ2.Clear();
            if (picosQ3 != null) 
                picosQ3.Clear();
            if (picosQ4 != null) 
                picosQ4.Clear();
            if (picosQ5 != null) 
                picosQ5.Clear();
            if (picosQ6 != null) 
                picosQ6.Clear();
            if (picosQ7 != null) 
                picosQ7.Clear();
            if (picosQ8 != null) 
                picosQ8.Clear();
            if (picosQ9 != null) 
                picosQ9.Clear();
            if (picosQ10 != null) 
                picosQ10.Clear();
            if (picosQ11 != null) 
                picosQ11.Clear();
            if (picosQ12 != null) 
                picosQ12.Clear();

            if(picosR1!=null)
                picosR1.Clear();
            if (picosR2 != null) 
                picosR2.Clear();
            if (picosR3 != null) 
                picosR3.Clear();
            if (picosR4 != null) 
                picosR4.Clear();
            if (picosR5 != null) 
                picosR5.Clear();
            if (picosR6 != null) 
                picosR6.Clear();
            if (picosR7 != null) 
                picosR7.Clear();
            if (picosR8 != null) 
                picosR8.Clear();
            if (picosR9 != null) 
                picosR9.Clear();
            if (picosR10 != null) 
                picosR10.Clear();
            if (picosR11 != null) 
                picosR11.Clear();
            if (picosR12 != null) 
                picosR12.Clear();

            if(picosS1!=null)
                picosS1.Clear();
            if (picosS2 != null) 
                picosS2.Clear();
            if (picosS3 != null) 
                picosS3.Clear();
            if (picosS4 != null) 
                picosS4.Clear();
            if (picosS5 != null) 
                picosS5.Clear();
            if (picosS6 != null) 
                picosS6.Clear();
            if (picosS7 != null) 
                picosS7.Clear();
            if (picosS8 != null) 
                picosS8.Clear();
            if (picosS9 != null) 
                picosS9.Clear();
            if (picosS10 != null) 
                picosS10.Clear();
            if (picosS11 != null) 
                picosS11.Clear();
            if (picosS12 != null) 
                picosS12.Clear();

            if(picosT1!=null)
                picosT1.Clear();
            if (picosT2 != null) 
                picosT2.Clear();
            if (picosT3 != null) 
                picosT3.Clear();
            if (picosT4 != null) 
                picosT4.Clear();
            if (picosT5 != null) 
                picosT5.Clear();
            if (picosT6 != null) 
                picosT6.Clear();
            if (picosT7 != null) 
                picosT7.Clear();
            if (picosT8 != null) 
                picosT8.Clear();
            if (picosT9 != null) 
                picosT9.Clear();
            if (picosT10 != null) 
                picosT10.Clear();
            if (picosT11 != null) 
                picosT11.Clear();
            if (picosT12 != null) 
                picosT12.Clear();

            age = "";
            gender = "";
            race = "";
            height = "";
            weight = "";
            hertz = "";
            secs = "";
            her = 0;
            herMod = 0;
            ventanaFiltroMedia = 0;
            //hertzFiltered = 0;

            pX_c1 = 0;
            pX_c2 = 0;
            pX_c3 = 0;
            pX_c4 = 0;
            pX_c5 = 0;
            pX_c6 = 0;
            pX_c7 = 0;
            pX_c8 = 0;
            pX_c9 = 0;
            pX_c10 = 0;
            pX_c11 = 0;
            pX_c12 = 0;
            pY_c1 = 0;
            pY_c2 = 0;
            pY_c3 = 0;
            pY_c4 = 0;
            pY_c5 = 0;
            pY_c6 = 0;
            pY_c7 = 0;
            pY_c8 = 0;
            pY_c9 = 0;
            pY_c10 = 0;
            pY_c11 = 0;
            pY_c12 = 0;

            segments = new double[12][];
            for (int i = 0; i < 12; i++)
            {
                segments[i] = new double[4];
            }
            inversionT = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

            string CurrentDirectory = Directory.GetCurrentDirectory();

            reportFile = CurrentDirectory + "/report.csv";

            

        }

        private void drawECG()
        {
            clearECG();
            tiempo = 0.0;
            foreach (double d in data1f)
            {
                chart1.Series[0].Points.AddXY(tiempo,d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data2f)
            {
                chart2.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data3f)
            {
                chart3.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data4f)
            {
                chart4.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data5f)
            {
                chart5.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data6f)
            {
                chart6.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data7f)
            {
                chart7.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data8f)
            {
                chart8.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data9f)
            {
                chart9.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data10f)
            {
                chart10.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data11f)
            {
                chart11.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            tiempo = 0.0;
            foreach (double d in data12f)
            {
                chart12.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double)herMod;
            }
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data1f.Count;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data2f.Count;
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data3f.Count;
            chart4.ChartAreas[0].AxisX.Minimum = 0;
            chart4.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data4f.Count;
            chart5.ChartAreas[0].AxisX.Minimum = 0;
            chart5.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data5f.Count;
            chart6.ChartAreas[0].AxisX.Minimum = 0;
            chart6.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data6f.Count;
            chart7.ChartAreas[0].AxisX.Minimum = 0;
            chart7.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data7f.Count;
            chart8.ChartAreas[0].AxisX.Minimum = 0;
            chart8.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data8f.Count;
            chart9.ChartAreas[0].AxisX.Minimum = 0;
            chart9.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data9f.Count;
            chart10.ChartAreas[0].AxisX.Minimum = 0;
            chart10.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data10f.Count;
            chart11.ChartAreas[0].AxisX.Minimum = 0;
            chart11.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data11f.Count;
            chart12.ChartAreas[0].AxisX.Minimum = 0;
            chart12.ChartAreas[0].AxisX.Maximum = Math.Round(Convert.ToDouble(secs) * (double)herMod * 1.0 / (double)herMod, 1);//data12f.Count;

            // enable autoscroll
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart2.ChartAreas[0].CursorX.AutoScroll = true;
            chart3.ChartAreas[0].CursorX.AutoScroll = true;
            chart4.ChartAreas[0].CursorX.AutoScroll = true;
            chart5.ChartAreas[0].CursorX.AutoScroll = true;
            chart6.ChartAreas[0].CursorX.AutoScroll = true;
            chart7.ChartAreas[0].CursorX.AutoScroll = true;
            chart8.ChartAreas[0].CursorX.AutoScroll = true;
            chart9.ChartAreas[0].CursorX.AutoScroll = true;
            chart10.ChartAreas[0].CursorX.AutoScroll = true;
            chart11.ChartAreas[0].CursorX.AutoScroll = true;
            chart12.ChartAreas[0].CursorX.AutoScroll = true;

            // let's zoom to [0,blockSize] (e.g. [0,100])
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart3.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart3.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart4.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart4.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart5.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart5.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart6.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart6.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart7.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart7.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart8.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart8.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart9.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart9.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart10.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart10.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart11.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart11.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart12.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart12.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart3.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart4.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart5.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart6.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart7.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart8.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart9.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart10.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart11.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);
            chart12.ChartAreas[0].AxisX.ScaleView.Zoom(0, blockSize);

            // disable zoom-reset button (only scrollbar's arrows are available)
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart2.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart3.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart4.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart5.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart6.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart7.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart8.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart9.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart10.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart11.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart12.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // set scrollbar small change to blockSize (e.g. 100)
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart2.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart3.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart4.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart5.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart6.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart7.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart8.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart9.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart10.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart11.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
            chart12.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;

            groupBox2.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;

            /*label25.Visible = true;
            label26.Visible = true;
            label27.Visible = true;
            label28.Visible = true;
            label29.Visible = true;
            label30.Visible = true;
            label31.Visible = true;
            label32.Visible = true;
            label33.Visible = true;
            label34.Visible = true;
            label35.Visible = true;
            label36.Visible = true;*/
        }
        private void clearECG()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            chart6.Series[0].Points.Clear();
            chart7.Series[0].Points.Clear();
            chart8.Series[0].Points.Clear();
            chart9.Series[0].Points.Clear();
            chart10.Series[0].Points.Clear();
            chart11.Series[0].Points.Clear();
            chart12.Series[0].Points.Clear();

            chart1.ChartAreas[0].AxisX.StripLines.Clear();
            chart2.ChartAreas[0].AxisX.StripLines.Clear();
            chart3.ChartAreas[0].AxisX.StripLines.Clear();
            chart4.ChartAreas[0].AxisX.StripLines.Clear();
            chart5.ChartAreas[0].AxisX.StripLines.Clear();
            chart6.ChartAreas[0].AxisX.StripLines.Clear();
            chart7.ChartAreas[0].AxisX.StripLines.Clear();
            chart8.ChartAreas[0].AxisX.StripLines.Clear();
            chart9.ChartAreas[0].AxisX.StripLines.Clear();
            chart10.ChartAreas[0].AxisX.StripLines.Clear();
            chart11.ChartAreas[0].AxisX.StripLines.Clear();
            chart12.ChartAreas[0].AxisX.StripLines.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (blockSize >= 1)
            {
                blockSize -= 0.5;
                double ancho = blockSize > 2 ? 1 : blockSize / 2.0;
                drawECG();

                if (checkBox1.Visible && checkBox1.Checked)
                    pintarPicosP(ancho);
                if(checkBox2.Visible && checkBox2.Checked)
                    pintarPicosQ(ancho);
                if(checkBox3.Visible && checkBox3.Checked)
                    pintarPicosR(ancho);
                if(checkBox4.Visible && checkBox4.Checked)
                    pintarPicosS(ancho);
                if(checkBox5.Visible && checkBox5.Checked)
                    pintarPicosT(ancho);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (blockSize <= 10)
            {
                blockSize += 0.5;
                double ancho = blockSize > 2 ? 1 : blockSize / 2.0;
                drawECG();
                if (checkBox1.Visible && checkBox1.Checked)
                    pintarPicosP(ancho);
                if (checkBox2.Visible && checkBox2.Checked)
                    pintarPicosQ(ancho);
                if (checkBox3.Visible && checkBox3.Checked)
                    pintarPicosR(ancho);
                if (checkBox4.Visible && checkBox4.Checked)
                    pintarPicosS(ancho);
                if (checkBox5.Visible && checkBox5.Checked)
                    pintarPicosT(ancho);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            blockSize = 2;
            drawECG();
            double ancho = blockSize > 2 ? 1 : blockSize / 2.0;
            if (checkBox1.Visible && checkBox1.Checked)
                pintarPicosP(ancho);
            if (checkBox2.Visible && checkBox2.Checked)
                pintarPicosQ(ancho);
            if (checkBox3.Visible && checkBox3.Checked)
                pintarPicosR(ancho);
            if (checkBox4.Visible && checkBox4.Checked)
                pintarPicosS(ancho);
            if (checkBox5.Visible && checkBox5.Checked)
                pintarPicosT(ancho);
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart1.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c1 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c1 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c1 + "," + pY_c1 + ")";
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart2.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c2 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c2 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c2 + "," + pY_c2 + ")";
        }

        private void chart3_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart3.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c3 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c3 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c3 + "," + pY_c3 + ")";
        }

        private void chart4_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart4.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c4 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c4 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c4 + "," + pY_c4 + ")";
        }

        private void chart5_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart5.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c5 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c5 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c5 + "," + pY_c5 + ")";
        }

        private void chart6_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart6.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c6 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c6 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c6 + "," + pY_c6 + ")";
        }

        private void chart7_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart7.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c7 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c7 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c7 + "," + pY_c7 + ")";
        }

        private void chart8_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart8.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c8 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c8 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c8 + "," + pY_c8 + ")";
        }

        private void chart9_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart9.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c9 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c9 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c9 + "," + pY_c9 + ")";
        }

        private void chart10_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart10.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c10 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c10 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c10 + "," + pY_c10 + ")";
        }

        private void chart11_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart11.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c11 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c11 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c11 + "," + pY_c11 + ")";
        }

        private void chart12_MouseClick(object sender, MouseEventArgs e)
        {
            var chartArea = chart12.ChartAreas[0];
            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            pX_c12 = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            pY_c12 = chartArea.CursorY.Position;
            c1_point.Text = "(" + pX_c12 + "," + pY_c12 + ")";
        }

        public static DialogResult InputBox(string title, string promptText, ref string value, double val)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;


            textBox.Text = Convert.ToString(val);

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static DialogResult InputBoxSegments(string title, ref string prMin, ref string prMax, ref string qrsMin, ref string qrsMax, ref string qtMin, ref string qtMax, double val)
        {
            Form form = new Form();
            //Label label = new Label();
            Label label1 = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            Label label4 = new Label();
            Label label5 = new Label();
            Label label6 = new Label();
            TextBox textBox1 = new TextBox();
            TextBox textBox2 = new TextBox();
            TextBox textBox3 = new TextBox();
            TextBox textBox4 = new TextBox();
            TextBox textBox5 = new TextBox();
            TextBox textBox6 = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            //label.Text = promptText;

            label1.Text = "PQ min:";
            label2.Text = "PQ max:";
            label3.Text = "QRS min:";
            label4.Text = "QRS max:";
            label5.Text = "QT min:";
            label6.Text = "QT max:";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            //label.SetBounds(10, 5, 200, 13);

            label1.SetBounds(10, 15, 55, 13);
            textBox1.SetBounds(65, 12, 50, 19);

            label2.SetBounds(150, 15, 55, 13);
            textBox2.SetBounds(205, 12, 50, 19);

            label3.SetBounds(10, 50, 55, 13);
            textBox3.SetBounds(65, 47, 50, 19);

            label4.SetBounds(150, 50, 55, 13);
            textBox4.SetBounds(205, 47, 50, 19);

            label5.SetBounds(10, 85, 55, 13);
            textBox5.SetBounds(65, 82, 50, 19);

            label6.SetBounds(150, 85, 55, 13);
            textBox6.SetBounds(205, 82, 50, 19);



            buttonOk.SetBounds(40, 120, 75, 23);
            buttonCancel.SetBounds(150, 120, 75, 23);

            //label.AutoSize = true;
            //textBox1.Anchor = textBox1.Anchor | AnchorStyles.Right;
            //buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            //buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;



            form.ClientSize = new Size(450, 160);
            form.Controls.AddRange(new Control[] { label1, label2, label3, label4, label5, label6, textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(250, textBox2.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            prMin = textBox1.Text;
            prMax = textBox2.Text;
            qrsMin = textBox3.Text;
            qrsMax = textBox4.Text;
            qtMin = textBox5.Text;
            qtMax = textBox6.Text;
            return dialogResult;
        }

        public static class CheckboxDialog
        {
            public static int ShowDialog(string text, string caption)
            {
                Form prompt = new Form();
                prompt.Width = 580;
                prompt.Height = 100;
                prompt.Text = caption;

                FlowLayoutPanel pnl = new FlowLayoutPanel();
                pnl.Dock = DockStyle.Fill;

                RadioButton r1 = new RadioButton();
                r1.Text = "P";
                r1.Location = new Point(10, 10);
                pnl.Controls.Add(r1);
                RadioButton r2 = new RadioButton();
                r2.Text = "Q";
                r2.Location = new Point(50, 10);
                pnl.Controls.Add(r2);
                RadioButton r3 = new RadioButton();
                r3.Text = "R";
                r3.Location = new Point(100, 10);
                pnl.Controls.Add(r3);
                RadioButton r4 = new RadioButton();
                r4.Text = "S";
                r4.Location = new Point(150, 10);
                pnl.Controls.Add(r4);
                RadioButton r5 = new RadioButton();
                r5.Text = "T";
                r5.Location = new Point(200, 10);
                pnl.Controls.Add(r5);

                Button ok = new Button() { Text = "OK" };
                ok.Click += (sender, e) => { prompt.Close(); };
                ok.Location = new Point(100, 50);
                pnl.Controls.Add(ok);

                prompt.Controls.Add(pnl);

                DialogResult res = prompt.ShowDialog();
                int r=-1;
                if (r1.Checked)
                    r = 0;
                else if (r2.Checked)
                    r = 1;
                else if (r3.Checked)
                    r = 2;
                else if (r4.Checked)
                    r = 3;
                else if (r5.Checked)
                    r = 4;

                return r;
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
                    stripline.IntervalOffset = pX_c1;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev!=-1)
                        annot1.Add(labels[dev]);
                    else
                        annot1.Add("empty");

                    stripline.BackColor = clabels[dev+1];
                    lines1.Add(stripline);
                    chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 1:
                    stripline.IntervalOffset = pX_c2;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot2.Add(labels[dev]);
                    else
                        annot2.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines2.Add(stripline);
                    chart2.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 2:
                    stripline.IntervalOffset = pX_c3;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot3.Add(labels[dev]);
                    else
                        annot3.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines3.Add(stripline);
                    chart3.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 3:
                    stripline.IntervalOffset = pX_c4;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot4.Add(labels[dev]);
                    else
                        annot4.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines4.Add(stripline);
                    chart4.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 4:
                    stripline.IntervalOffset = pX_c5;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot5.Add(labels[dev]);
                    else
                        annot5.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines5.Add(stripline);
                    chart5.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 5:
                    stripline.IntervalOffset = pX_c6;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot6.Add(labels[dev]);
                    else
                        annot6.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines6.Add(stripline);
                    chart6.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 6:
                    stripline.IntervalOffset = pX_c7;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot7.Add(labels[dev]);
                    else
                        annot7.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines7.Add(stripline);
                    chart7.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 7:
                    stripline.IntervalOffset = pX_c8;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot8.Add(labels[dev]);
                    else
                        annot8.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines8.Add(stripline);
                    chart8.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 8:
                    stripline.IntervalOffset = pX_c9;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot9.Add(labels[dev]);
                    else
                        annot9.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines9.Add(stripline);
                    chart9.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 9:
                    stripline.IntervalOffset = pX_c10;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot10.Add(labels[dev]);
                    else
                        annot10.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines10.Add(stripline);
                    chart10.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 10:
                    stripline.IntervalOffset = pX_c11;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot11.Add(labels[dev]);
                    else
                        annot11.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines11.Add(stripline);
                    chart11.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
                case 11:
                    stripline.IntervalOffset = pX_c12;

                    dev = CheckboxDialog.ShowDialog("Label", "Assign label to mark:");

                    if (dev != -1)
                        annot12.Add(labels[dev]);
                    else
                        annot12.Add("empty");

                    stripline.BackColor = clabels[dev + 1];

                    lines12.Add(stripline);
                    chart12.ChartAreas[0].AxisX.StripLines.Add(stripline);
                    break;
            }

        }

        private void remmark_Click(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    if (lines1.Count > 0)
                    {
                        lines1.RemoveAt(lines1.Count - 1);
                        annot1.RemoveAt(annot1.Count - 1);
                        chart1.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines1)
                            chart1.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 1:
                    if (lines2.Count > 0)
                    {
                        lines2.RemoveAt(lines2.Count - 1);
                        annot2.RemoveAt(annot2.Count - 1);
                        chart2.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines2)
                            chart2.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 2:
                    if (lines3.Count > 0)
                    {
                        lines3.RemoveAt(lines3.Count - 1);
                        annot3.RemoveAt(annot3.Count - 1);
                        chart3.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines3)
                            chart3.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 3:
                    if (lines4.Count > 0)
                    {
                        lines4.RemoveAt(lines4.Count - 1);
                        annot4.RemoveAt(annot4.Count - 1);
                        chart4.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines4)
                            chart4.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 4:
                    if (lines5.Count > 0)
                    {
                        lines5.RemoveAt(lines5.Count - 1);
                        annot5.RemoveAt(annot5.Count - 1);
                        chart5.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines5)
                            chart5.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 5:
                    if (lines6.Count > 0)
                    {
                        lines6.RemoveAt(lines6.Count - 1);
                        annot6.RemoveAt(annot6.Count - 1);
                        chart6.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines6)
                            chart6.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 6:
                    if (lines7.Count > 0)
                    {
                        lines7.RemoveAt(lines7.Count - 1);
                        annot7.RemoveAt(annot7.Count - 1);
                        chart7.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines7)
                            chart7.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 7:
                    if (lines8.Count > 0)
                    {
                        lines8.RemoveAt(lines8.Count - 1);
                        annot8.RemoveAt(annot8.Count - 1);
                        chart8.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines8)
                            chart8.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 8:
                    if (lines9.Count > 0)
                    {
                        lines9.RemoveAt(lines9.Count - 1);
                        annot9.RemoveAt(annot9.Count - 1);
                        chart9.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines9)
                            chart9.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 9:
                    if (lines10.Count > 0)
                    {
                        lines10.RemoveAt(lines10.Count - 1);
                        annot10.RemoveAt(annot10.Count - 1);
                        chart10.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines10)
                            chart10.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 10:
                    if (lines11.Count > 0)
                    {
                        lines11.RemoveAt(lines11.Count - 1);
                        annot11.RemoveAt(annot11.Count - 1);
                        chart11.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines11)
                            chart11.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
                case 11:
                    if (lines12.Count > 0)
                    {
                        lines12.RemoveAt(lines12.Count - 1);
                        annot12.RemoveAt(annot12.Count - 1);
                        chart12.ChartAreas[0].AxisX.StripLines.Clear();
                        foreach (StripLine s in lines12)
                            chart12.ChartAreas[0].AxisX.StripLines.Add(s);
                    }
                    break;
            }
            
        }

       
    }
}
