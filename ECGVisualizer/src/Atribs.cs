using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace ECGVisualizer
{
    public class Atribs
    {
        //"International Recommendations for Electrocardiographic Interpretation in Athletes" (2017) - Journal of the American College of Cardiology - https://www.sciencedirect.com/science/article/pii/S0735109717302024
        public double[] athleteSegmentsMin = new double[3] { 0.20, 0.08, 0.32 }; //PR, QRS, QT min in secs
        public double[] athleteSegmentsMax = new double[3] { 0.40, 0.16, 0.56 }; //PR, QRS, QT max in secs

        //https://www.nottingham.ac.uk/nursing/practice/resources/cardiology/function/normal_duration.php
        public double[] averageSegmentsMin = new double[3] { 0.12, 0.08, 0.35 }; //PR, QRS, QT min in secs
        public double[] averageSegmentsMax = new double[3] { 0.20, 0.12, 0.43 }; //PR, QRS, QT max in secs

        public double[] customSegmentsMin = new double[3] { 0.12, 0.08, 0.35 }; //PR, QRS, QT min in secs
        public double[] customSegmentsMax = new double[3] { 0.20, 0.12, 0.43 }; //PR, QRS, QT max in secs

        public int umbralRuidoPot = 40;
        public int umbralRuidoTime = 10;

        public OpenFileDialog openFileDialog1;
        public SaveFileDialog saveFileDialog1;
        public XmlDocument doc;
        public XmlNode node, restingECGMeasurements, vectorLoops;
        public string fileName;
        public string age, gender, race, height, weight, hertz, secs, secs_new;
        public string age_new, gender_new, race_new, height_new, weight_new, hertz_new;
        public int her, herMod, previousHertzs;
        public int ventanaFiltroMedia;
        public int ventanaComparacion = 10;
        public double blockSize;
        public double pX_c1, pX_c2, pX_c3, pX_c4, pX_c5, pX_c6, pX_c7, pX_c8, pX_c9, pX_c10, pX_c11, pX_c12;
        public double pY_c1, pY_c2, pY_c3, pY_c4, pY_c5, pY_c6, pY_c7, pY_c8, pY_c9, pY_c10, pY_c11, pY_c12;
        public ArrayList lines1, lines2, lines3, lines4, lines5, lines6, lines7, lines8, lines9, lines10, lines11, lines12;

        public double maxr1, minr1, maxr2, minr2, maxr3, minr3, maxr4, minr4, maxr5, minr5, maxr6, minr6, maxr7, minr7, maxr8, minr8, maxr9, minr9, maxr10, minr10, maxr11, minr11, maxr12, minr12;
        public List<double> raw1, raw2, raw3, raw4, raw5, raw6, raw7, raw8, raw9, raw10, raw11, raw12;
        public List<double> raw1f, raw2f, raw3f, raw4f, raw5f, raw6f, raw7f, raw8f, raw9f, raw10f, raw11f, raw12f;
        public List<double> data1, data2, data3, data4, data5, data6, data7, data8, data9, data10, data11, data12;
        public List<double> data1f, data2f, data3f, data4f, data5f, data6f, data7f, data8f, data9f, data10f, data11f, data12f;
        
        public List<double> auxFAF1, auxFAF2, auxFAF3, auxFAF4, auxFAF5, auxFAF6, auxFAF7, auxFAF8, auxFAF9, auxFAF10, auxFAF11, auxFAF12;
        public List<double> auxMAF1, auxMAF2, auxMAF3, auxMAF4, auxMAF5, auxMAF6, auxMAF7, auxMAF8, auxMAF9, auxMAF10, auxMAF11, auxMAF12;
        public List<double> auxMMF1, auxMMF2, auxMMF3, auxMMF4, auxMMF5, auxMMF6, auxMMF7, auxMMF8, auxMMF9, auxMMF10, auxMMF11, auxMMF12;
        public List<double> auxBSF1, auxBSF2, auxBSF3, auxBSF4, auxBSF5, auxBSF6, auxBSF7, auxBSF8, auxBSF9, auxBSF10, auxBSF11, auxBSF12;
        public List<double> auxSUB1, auxSUB2, auxSUB3, auxSUB4, auxSUB5, auxSUB6, auxSUB7, auxSUB8, auxSUB9, auxSUB10, auxSUB11, auxSUB12;
        public List<double> auxAM1, auxAM2, auxAM3, auxAM4, auxAM5, auxAM6, auxAM7, auxAM8, auxAM9, auxAM10, auxAM11, auxAM12;
        public List<double> auxTM1, auxTM2, auxTM3, auxTM4, auxTM5, auxTM6, auxTM7, auxTM8, auxTM9, auxTM10, auxTM11, auxTM12;

        public List<double> rawauxFAF1, rawauxFAF2, rawauxFAF3, rawauxFAF4, rawauxFAF5, rawauxFAF6, rawauxFAF7, rawauxFAF8, rawauxFAF9, rawauxFAF10, rawauxFAF11, rawauxFAF12;
        public List<double> rawauxMAF1, rawauxMAF2, rawauxMAF3, rawauxMAF4, rawauxMAF5, rawauxMAF6, rawauxMAF7, rawauxMAF8, rawauxMAF9, rawauxMAF10, rawauxMAF11, rawauxMAF12;
        public List<double> rawauxMMF1, rawauxMMF2, rawauxMMF3, rawauxMMF4, rawauxMMF5, rawauxMMF6, rawauxMMF7, rawauxMMF8, rawauxMMF9, rawauxMMF10, rawauxMMF11, rawauxMMF12;
        public List<double> rawauxBSF1, rawauxBSF2, rawauxBSF3, rawauxBSF4, rawauxBSF5, rawauxBSF6, rawauxBSF7, rawauxBSF8, rawauxBSF9, rawauxBSF10, rawauxBSF11, rawauxBSF12;
        public List<double> rawauxSUB1, rawauxSUB2, rawauxSUB3, rawauxSUB4, rawauxSUB5, rawauxSUB6, rawauxSUB7, rawauxSUB8, rawauxSUB9, rawauxSUB10, rawauxSUB11, rawauxSUB12;
        public List<double> rawauxAM1, rawauxAM2, rawauxAM3, rawauxAM4, rawauxAM5, rawauxAM6, rawauxAM7, rawauxAM8, rawauxAM9, rawauxAM10, rawauxAM11, rawauxAM12;
        public List<double> rawauxTM1, rawauxTM2, rawauxTM3, rawauxTM4, rawauxTM5, rawauxTM6, rawauxTM7, rawauxTM8, rawauxTM9, rawauxTM10, rawauxTM11, rawauxTM12;


        public List<double> finFAF1, finFAF2, finFAF3, finFAF4, finFAF5, finFAF6, finFAF7, finFAF8, finFAF9, finFAF10, finFAF11, finFAF12;
        public List<double> finMAF1, finMAF2, finMAF3, finMAF4, finMAF5, finMAF6, finMAF7, finMAF8, finMAF9, finMAF10, finMAF11, finMAF12;
        public List<double> finMMF1, finMMF2, finMMF3, finMMF4, finMMF5, finMMF6, finMMF7, finMMF8, finMMF9, finMMF10, finMMF11, finMMF12;
        public List<double> finBSF1, finBSF2, finBSF3, finBSF4, finBSF5, finBSF6, finBSF7, finBSF8, finBSF9, finBSF10, finBSF11, finBSF12;
        public List<double> finAM1, finAM2, finAM3, finAM4, finAM5, finAM6, finAM7, finAM8, finAM9, finAM10, finAM11, finAM12;
        public List<double> finTM1, finTM2, finTM3, finTM4, finTM5, finTM6, finTM7, finTM8, finTM9, finTM10, finTM11, finTM12;

        public List<double> rawfinFAF1, rawfinFAF2, rawfinFAF3, rawfinFAF4, rawfinFAF5, rawfinFAF6, rawfinFAF7, rawfinFAF8, rawfinFAF9, rawfinFAF10, rawfinFAF11, rawfinFAF12;
        public List<double> rawfinMAF1, rawfinMAF2, rawfinMAF3, rawfinMAF4, rawfinMAF5, rawfinMAF6, rawfinMAF7, rawfinMAF8, rawfinMAF9, rawfinMAF10, rawfinMAF11, rawfinMAF12;
        public List<double> rawfinMMF1, rawfinMMF2, rawfinMMF3, rawfinMMF4, rawfinMMF5, rawfinMMF6, rawfinMMF7, rawfinMMF8, rawfinMMF9, rawfinMMF10, rawfinMMF11, rawfinMMF12;
        public List<double> rawfinBSF1, rawfinBSF2, rawfinBSF3, rawfinBSF4, rawfinBSF5, rawfinBSF6, rawfinBSF7, rawfinBSF8, rawfinBSF9, rawfinBSF10, rawfinBSF11, rawfinBSF12;
        public List<double> rawfinAM1, rawfinAM2, rawfinAM3, rawfinAM4, rawfinAM5, rawfinAM6, rawfinAM7, rawfinAM8, rawfinAM9, rawfinAM10, rawfinAM11, rawfinAM12;
        public List<double> rawfinTM1, rawfinTM2, rawfinTM3, rawfinTM4, rawfinTM5, rawfinTM6, rawfinTM7, rawfinTM8, rawfinTM9, rawfinTM10, rawfinTM11, rawfinTM12;


        public bool firstFAF = true, firstMAF = true, firstMMF = true, firstBSF = true, firstSUB = true, firstAM = true, firstTM = true;

        public ArrayList annot1, annot2, annot3, annot4, annot5, annot6, annot7, annot8, annot9, annot10, annot11, annot12;

        public string[] labels = new string[] { "P", "Q", "R", "S", "T" };
        public Color[] clabels = new Color[] {Color.Black, Color.Red, Color.Orange, Color.Green, Color.Blue, Color.Violet};

        public bool archivoCargado;
        public string resolution, resolution_new;
        public int resol;

        public List<double> picosP1, picosP2, picosP3, picosP4, picosP5, picosP6, picosP7, picosP8, picosP9, picosP10, picosP11, picosP12;
        public List<double> picosQ1, picosQ2, picosQ3, picosQ4, picosQ5, picosQ6, picosQ7, picosQ8, picosQ9, picosQ10, picosQ11, picosQ12;
        public List<double> picosR1, picosR2, picosR3, picosR4, picosR5, picosR6, picosR7, picosR8, picosR9, picosR10, picosR11, picosR12;
        public List<double> picosS1, picosS2, picosS3, picosS4, picosS5, picosS6, picosS7, picosS8, picosS9, picosS10, picosS11, picosS12;
        public List<double> picosT1, picosT2, picosT3, picosT4, picosT5, picosT6, picosT7, picosT8, picosT9, picosT10, picosT11, picosT12;

        public double[][] segments;
        public bool[] inversionT;

        public string reportFile;
        public double tiempo = 0.0, tiempo_new = 0.0;

        public bool R = true, Q = true, S = true, T = true, P = true;

        public double bmpsFile;
        public bool correcto = false;

        public int typePeople = 1; //0: average, 1: athlete, ...

        public FormAboutUS aboutUS;

        public bool repAge, repHeight, repWeight, repRace, repGender, repPQ, repQR, repRS, repST, repPR, repRR, repQT, repQT_c, repQRS, repi1, repi2, repi3, repaVR, repaVL, repaVF, repV1, repV2, repV3, repV4, repV5, repV6;

        public void createLists()
        {
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

            rawauxFAF1 = new List<double>();
            rawauxFAF2 = new List<double>();
            rawauxFAF3 = new List<double>();
            rawauxFAF4 = new List<double>();
            rawauxFAF5 = new List<double>();
            rawauxFAF6 = new List<double>();
            rawauxFAF7 = new List<double>();
            rawauxFAF8 = new List<double>();
            rawauxFAF9 = new List<double>();
            rawauxFAF10 = new List<double>();
            rawauxFAF11 = new List<double>();
            rawauxFAF12 = new List<double>();

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

            rawauxMAF1 = new List<double>();
            rawauxMAF2 = new List<double>();
            rawauxMAF3 = new List<double>();
            rawauxMAF4 = new List<double>();
            rawauxMAF5 = new List<double>();
            rawauxMAF6 = new List<double>();
            rawauxMAF7 = new List<double>();
            rawauxMAF8 = new List<double>();
            rawauxMAF9 = new List<double>();
            rawauxMAF10 = new List<double>();
            rawauxMAF11 = new List<double>();
            rawauxMAF12 = new List<double>();

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

            rawauxMMF1 = new List<double>();
            rawauxMMF2 = new List<double>();
            rawauxMMF3 = new List<double>();
            rawauxMMF4 = new List<double>();
            rawauxMMF5 = new List<double>();
            rawauxMMF6 = new List<double>();
            rawauxMMF7 = new List<double>();
            rawauxMMF8 = new List<double>();
            rawauxMMF9 = new List<double>();
            rawauxMMF10 = new List<double>();
            rawauxMMF11 = new List<double>();
            rawauxMMF12 = new List<double>();

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

            rawauxBSF1 = new List<double>();
            rawauxBSF2 = new List<double>();
            rawauxBSF3 = new List<double>();
            rawauxBSF4 = new List<double>();
            rawauxBSF5 = new List<double>();
            rawauxBSF6 = new List<double>();
            rawauxBSF7 = new List<double>();
            rawauxBSF8 = new List<double>();
            rawauxBSF9 = new List<double>();
            rawauxBSF10 = new List<double>();
            rawauxBSF11 = new List<double>();
            rawauxBSF12 = new List<double>();

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

            rawauxSUB1 = new List<double>();
            rawauxSUB2 = new List<double>();
            rawauxSUB3 = new List<double>();
            rawauxSUB4 = new List<double>();
            rawauxSUB5 = new List<double>();
            rawauxSUB6 = new List<double>();
            rawauxSUB7 = new List<double>();
            rawauxSUB8 = new List<double>();
            rawauxSUB9 = new List<double>();
            rawauxSUB10 = new List<double>();
            rawauxSUB11 = new List<double>();
            rawauxSUB12 = new List<double>();

            auxAM1 = new List<double>();
            auxAM2 = new List<double>();
            auxAM3 = new List<double>();
            auxAM4 = new List<double>();
            auxAM5 = new List<double>();
            auxAM6 = new List<double>();
            auxAM7 = new List<double>();
            auxAM8 = new List<double>();
            auxAM9 = new List<double>();
            auxAM10 = new List<double>();
            auxAM11 = new List<double>();
            auxAM12 = new List<double>();

            rawauxAM1 = new List<double>();
            rawauxAM2 = new List<double>();
            rawauxAM3 = new List<double>();
            rawauxAM4 = new List<double>();
            rawauxAM5 = new List<double>();
            rawauxAM6 = new List<double>();
            rawauxAM7 = new List<double>();
            rawauxAM8 = new List<double>();
            rawauxAM9 = new List<double>();
            rawauxAM10 = new List<double>();
            rawauxAM11 = new List<double>();
            rawauxAM12 = new List<double>();

            auxTM1 = new List<double>();
            auxTM2 = new List<double>();
            auxTM3 = new List<double>();
            auxTM4 = new List<double>();
            auxTM5 = new List<double>();
            auxTM6 = new List<double>();
            auxTM7 = new List<double>();
            auxTM8 = new List<double>();
            auxTM9 = new List<double>();
            auxTM10 = new List<double>();
            auxTM11 = new List<double>();
            auxTM12 = new List<double>();

            rawauxTM1 = new List<double>();
            rawauxTM2 = new List<double>();
            rawauxTM3 = new List<double>();
            rawauxTM4 = new List<double>();
            rawauxTM5 = new List<double>();
            rawauxTM6 = new List<double>();
            rawauxTM7 = new List<double>();
            rawauxTM8 = new List<double>();
            rawauxTM9 = new List<double>();
            rawauxTM10 = new List<double>();
            rawauxTM11 = new List<double>();
            rawauxTM12 = new List<double>();

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

            rawfinFAF1 = new List<double>();
            rawfinFAF2 = new List<double>();
            rawfinFAF3 = new List<double>();
            rawfinFAF4 = new List<double>();
            rawfinFAF5 = new List<double>();
            rawfinFAF6 = new List<double>();
            rawfinFAF7 = new List<double>();
            rawfinFAF8 = new List<double>();
            rawfinFAF9 = new List<double>();
            rawfinFAF10 = new List<double>();
            rawfinFAF11 = new List<double>();
            rawfinFAF12 = new List<double>();

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

            rawfinMAF1 = new List<double>();
            rawfinMAF2 = new List<double>();
            rawfinMAF3 = new List<double>();
            rawfinMAF4 = new List<double>();
            rawfinMAF5 = new List<double>();
            rawfinMAF6 = new List<double>();
            rawfinMAF7 = new List<double>();
            rawfinMAF8 = new List<double>();
            rawfinMAF9 = new List<double>();
            rawfinMAF10 = new List<double>();
            rawfinMAF11 = new List<double>();
            rawfinMAF12 = new List<double>();

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

            rawfinMMF1 = new List<double>();
            rawfinMMF2 = new List<double>();
            rawfinMMF3 = new List<double>();
            rawfinMMF4 = new List<double>();
            rawfinMMF5 = new List<double>();
            rawfinMMF6 = new List<double>();
            rawfinMMF7 = new List<double>();
            rawfinMMF8 = new List<double>();
            rawfinMMF9 = new List<double>();
            rawfinMMF10 = new List<double>();
            rawfinMMF11 = new List<double>();
            rawfinMMF12 = new List<double>();

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

            rawfinBSF1 = new List<double>();
            rawfinBSF2 = new List<double>();
            rawfinBSF3 = new List<double>();
            rawfinBSF4 = new List<double>();
            rawfinBSF5 = new List<double>();
            rawfinBSF6 = new List<double>();
            rawfinBSF7 = new List<double>();
            rawfinBSF8 = new List<double>();
            rawfinBSF9 = new List<double>();
            rawfinBSF10 = new List<double>();
            rawfinBSF11 = new List<double>();
            rawfinBSF12 = new List<double>();

            finAM1 = new List<double>();
            finAM2 = new List<double>();
            finAM3 = new List<double>();
            finAM4 = new List<double>();
            finAM5 = new List<double>();
            finAM6 = new List<double>();
            finAM7 = new List<double>();
            finAM8 = new List<double>();
            finAM9 = new List<double>();
            finAM10 = new List<double>();
            finAM11 = new List<double>();
            finAM12 = new List<double>();

            rawfinAM1 = new List<double>();
            rawfinAM2 = new List<double>();
            rawfinAM3 = new List<double>();
            rawfinAM4 = new List<double>();
            rawfinAM5 = new List<double>();
            rawfinAM6 = new List<double>();
            rawfinAM7 = new List<double>();
            rawfinAM8 = new List<double>();
            rawfinAM9 = new List<double>();
            rawfinAM10 = new List<double>();
            rawfinAM11 = new List<double>();
            rawfinAM12 = new List<double>();

            finTM1 = new List<double>();
            finTM2 = new List<double>();
            finTM3 = new List<double>();
            finTM4 = new List<double>();
            finTM5 = new List<double>();
            finTM6 = new List<double>();
            finTM7 = new List<double>();
            finTM8 = new List<double>();
            finTM9 = new List<double>();
            finTM10 = new List<double>();
            finTM11 = new List<double>();
            finTM12 = new List<double>();

            rawfinTM1 = new List<double>();
            rawfinTM2 = new List<double>();
            rawfinTM3 = new List<double>();
            rawfinTM4 = new List<double>();
            rawfinTM5 = new List<double>();
            rawfinTM6 = new List<double>();
            rawfinTM7 = new List<double>();
            rawfinTM8 = new List<double>();
            rawfinTM9 = new List<double>();
            rawfinTM10 = new List<double>();
            rawfinTM11 = new List<double>();
            rawfinTM12 = new List<double>();

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

            raw1 = new List<double>();
            raw2 = new List<double>();
            raw3 = new List<double>();
            raw4 = new List<double>();
            raw5 = new List<double>();
            raw6 = new List<double>();
            raw7 = new List<double>();
            raw8 = new List<double>();
            raw9 = new List<double>();
            raw10 = new List<double>();
            raw11 = new List<double>();
            raw12 = new List<double>();

            raw1f = new List<double>();
            raw2f = new List<double>();
            raw3f = new List<double>();
            raw4f = new List<double>();
            raw5f = new List<double>();
            raw6f = new List<double>();
            raw7f = new List<double>();
            raw8f = new List<double>();
            raw9f = new List<double>();
            raw10f = new List<double>();
            raw11f = new List<double>();
            raw12f = new List<double>();
        }
    }
}
