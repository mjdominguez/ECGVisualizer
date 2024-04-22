using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Xml.Schema;

namespace ECGVisualizer
{
    public static class Tools
    {
        public static void clearData(ref Atribs atr)
        {
            atr.data1f.Clear();
            atr.data2f.Clear();
            atr.data3f.Clear();
            atr.data4f.Clear();
            atr.data5f.Clear();
            atr.data6f.Clear();
            atr.data7f.Clear();
            atr.data8f.Clear();
            atr.data9f.Clear();
            atr.data10f.Clear();
            atr.data11f.Clear();
            atr.data12f.Clear();

            atr.raw1f.Clear();
            atr.raw2f.Clear();
            atr.raw3f.Clear();
            atr.raw4f.Clear();
            atr.raw5f.Clear();
            atr.raw6f.Clear();
            atr.raw7f.Clear();
            atr.raw8f.Clear();
            atr.raw9f.Clear();
            atr.raw10f.Clear();
            atr.raw11f.Clear();
            atr.raw12f.Clear();
        }

        public static void loadData(ref string[] res, ref List<double> raw, ref List<double> raw2, ref List<double> orig, ref List<double> work, ref double max, ref double min, int resol)
        {
            double aux, aux2;
            foreach (string s in res)
            {
                aux = Double.Parse(s);
                aux2 = aux * resol / 1000.0;
                raw.Add(aux);
                raw2.Add(aux);
                orig.Add(aux2);
                work.Add(aux2);
            }
            calculateRangeRaw(ref raw, ref max, ref min);
        }

        public static void calculateRangeRaw(ref List<double> raw, ref double max, ref double min)
        {
            max = -1000;
            min = 1000;
            foreach (double s in raw)
            {
                if(s>max)
                {
                    max = s;
                }
                else if(s<min)
                {
                    min = s;
                }
            }
        }

        public static void erase(ref Atribs atributes)
        {
            atributes.data1.Clear();
            atributes.data2.Clear();
            atributes.data3.Clear();
            atributes.data4.Clear();
            atributes.data5.Clear();
            atributes.data6.Clear();
            atributes.data7.Clear();
            atributes.data8.Clear();
            atributes.data9.Clear();
            atributes.data10.Clear();
            atributes.data11.Clear();
            atributes.data12.Clear();

            atributes.data1f.Clear();
            atributes.data2f.Clear();
            atributes.data3f.Clear();
            atributes.data4f.Clear();
            atributes.data5f.Clear();
            atributes.data6f.Clear();
            atributes.data7f.Clear();
            atributes.data8f.Clear();
            atributes.data9f.Clear();
            atributes.data10f.Clear();
            atributes.data11f.Clear();
            atributes.data12f.Clear();

            atributes.raw1.Clear();
            atributes.raw2.Clear();
            atributes.raw3.Clear();
            atributes.raw4.Clear();
            atributes.raw5.Clear();
            atributes.raw6.Clear();
            atributes.raw7.Clear();
            atributes.raw8.Clear();
            atributes.raw9.Clear();
            atributes.raw10.Clear();
            atributes.raw11.Clear();
            atributes.raw12.Clear();

            atributes.raw1f.Clear();
            atributes.raw2f.Clear();
            atributes.raw3f.Clear();
            atributes.raw4f.Clear();
            atributes.raw5f.Clear();
            atributes.raw6f.Clear();
            atributes.raw7f.Clear();
            atributes.raw8f.Clear();
            atributes.raw9f.Clear();
            atributes.raw10f.Clear();
            atributes.raw11f.Clear();
            atributes.raw12f.Clear();

            atributes.auxFAF1.Clear();
            atributes.auxFAF2.Clear();
            atributes.auxFAF3.Clear();
            atributes.auxFAF4.Clear();
            atributes.auxFAF5.Clear();
            atributes.auxFAF6.Clear();
            atributes.auxFAF7.Clear();
            atributes.auxFAF8.Clear();
            atributes.auxFAF9.Clear();
            atributes.auxFAF10.Clear();
            atributes.auxFAF11.Clear();
            atributes.auxFAF12.Clear();

            atributes.auxMAF1.Clear();
            atributes.auxMAF2.Clear();
            atributes.auxMAF3.Clear();
            atributes.auxMAF4.Clear();
            atributes.auxMAF5.Clear();
            atributes.auxMAF6.Clear();
            atributes.auxMAF7.Clear();
            atributes.auxMAF8.Clear();
            atributes.auxMAF9.Clear();
            atributes.auxMAF10.Clear();
            atributes.auxMAF11.Clear();
            atributes.auxMAF12.Clear();

            atributes.auxMMF1.Clear();
            atributes.auxMMF2.Clear();
            atributes.auxMMF3.Clear();
            atributes.auxMMF4.Clear();
            atributes.auxMMF5.Clear();
            atributes.auxMMF6.Clear();
            atributes.auxMMF7.Clear();
            atributes.auxMMF8.Clear();
            atributes.auxMMF9.Clear();
            atributes.auxMMF10.Clear();
            atributes.auxMMF11.Clear();
            atributes.auxMMF12.Clear();

            atributes.auxBSF1.Clear();
            atributes.auxBSF2.Clear();
            atributes.auxBSF3.Clear();
            atributes.auxBSF4.Clear();
            atributes.auxBSF5.Clear();
            atributes.auxBSF6.Clear();
            atributes.auxBSF7.Clear();
            atributes.auxBSF8.Clear();
            atributes.auxBSF9.Clear();
            atributes.auxBSF10.Clear();
            atributes.auxBSF11.Clear();
            atributes.auxBSF12.Clear();

            atributes.auxSUB1.Clear();
            atributes.auxSUB2.Clear();
            atributes.auxSUB3.Clear();
            atributes.auxSUB4.Clear();
            atributes.auxSUB5.Clear();
            atributes.auxSUB6.Clear();
            atributes.auxSUB7.Clear();
            atributes.auxSUB8.Clear();
            atributes.auxSUB9.Clear();
            atributes.auxSUB10.Clear();
            atributes.auxSUB11.Clear();
            atributes.auxSUB12.Clear();

            atributes.auxAM1.Clear();
            atributes.auxAM2.Clear();
            atributes.auxAM3.Clear();
            atributes.auxAM4.Clear();
            atributes.auxAM5.Clear();
            atributes.auxAM6.Clear();
            atributes.auxAM7.Clear();
            atributes.auxAM8.Clear();
            atributes.auxAM9.Clear();
            atributes.auxAM10.Clear();
            atributes.auxAM11.Clear();
            atributes.auxAM12.Clear();

            atributes.finFAF1.Clear();
            atributes.finFAF2.Clear();
            atributes.finFAF3.Clear();
            atributes.finFAF4.Clear();
            atributes.finFAF5.Clear();
            atributes.finFAF6.Clear();
            atributes.finFAF7.Clear();
            atributes.finFAF8.Clear();
            atributes.finFAF9.Clear();
            atributes.finFAF10.Clear();
            atributes.finFAF11.Clear();
            atributes.finFAF12.Clear();

            atributes.finMAF1.Clear();
            atributes.finMAF2.Clear();
            atributes.finMAF3.Clear();
            atributes.finMAF4.Clear();
            atributes.finMAF5.Clear();
            atributes.finMAF6.Clear();
            atributes.finMAF7.Clear();
            atributes.finMAF8.Clear();
            atributes.finMAF9.Clear();
            atributes.finMAF10.Clear();
            atributes.finMAF11.Clear();
            atributes.finMAF12.Clear();

            atributes.finMMF1.Clear();
            atributes.finMMF2.Clear();
            atributes.finMMF3.Clear();
            atributes.finMMF4.Clear();
            atributes.finMMF5.Clear();
            atributes.finMMF6.Clear();
            atributes.finMMF7.Clear();
            atributes.finMMF8.Clear();
            atributes.finMMF9.Clear();
            atributes.finMMF10.Clear();
            atributes.finMMF11.Clear();
            atributes.finMMF12.Clear();

            atributes.finBSF1.Clear();
            atributes.finBSF2.Clear();
            atributes.finBSF3.Clear();
            atributes.finBSF4.Clear();
            atributes.finBSF5.Clear();
            atributes.finBSF6.Clear();
            atributes.finBSF7.Clear();
            atributes.finBSF8.Clear();
            atributes.finBSF9.Clear();
            atributes.finBSF10.Clear();
            atributes.finBSF11.Clear();
            atributes.finBSF12.Clear();

            atributes.finAM1.Clear();
            atributes.finAM2.Clear();
            atributes.finAM3.Clear();
            atributes.finAM4.Clear();
            atributes.finAM5.Clear();
            atributes.finAM6.Clear();
            atributes.finAM7.Clear();
            atributes.finAM8.Clear();
            atributes.finAM9.Clear();
            atributes.finAM10.Clear();
            atributes.finAM11.Clear();
            atributes.finAM12.Clear();

            atributes.lines1.Clear();
            atributes.lines2.Clear();
            atributes.lines3.Clear();
            atributes.lines4.Clear();
            atributes.lines5.Clear();
            atributes.lines6.Clear();
            atributes.lines7.Clear();
            atributes.lines8.Clear();
            atributes.lines9.Clear();
            atributes.lines10.Clear();
            atributes.lines11.Clear();
            atributes.lines12.Clear();

            atributes.annot1.Clear();
            atributes.annot2.Clear();
            atributes.annot3.Clear();
            atributes.annot4.Clear();
            atributes.annot5.Clear();
            atributes.annot6.Clear();
            atributes.annot7.Clear();
            atributes.annot8.Clear();
            atributes.annot9.Clear();
            atributes.annot10.Clear();
            atributes.annot11.Clear();
            atributes.annot12.Clear();

            if (atributes.picosP1 != null)
                atributes.picosP1.Clear();
            if (atributes.picosP2 != null)
                atributes.picosP2.Clear();
            if (atributes.picosP3 != null)
                atributes.picosP3.Clear();
            if (atributes.picosP4 != null)
                atributes.picosP4.Clear();
            if (atributes.picosP5 != null)
                atributes.picosP5.Clear();
            if (atributes.picosP6 != null)
                atributes.picosP6.Clear();
            if (atributes.picosP7 != null)
                atributes.picosP7.Clear();
            if (atributes.picosP8 != null)
                atributes.picosP8.Clear();
            if (atributes.picosP9 != null)
                atributes.picosP9.Clear();
            if (atributes.picosP10 != null)
                atributes.picosP10.Clear();
            if (atributes.picosP11 != null)
                atributes.picosP11.Clear();
            if (atributes.picosP12 != null)
                atributes.picosP12.Clear();

            if (atributes.picosQ1 != null)
                atributes.picosQ1.Clear();
            if (atributes.picosQ2 != null)
                atributes.picosQ2.Clear();
            if (atributes.picosQ3 != null)
                atributes.picosQ3.Clear();
            if (atributes.picosQ4 != null)
                atributes.picosQ4.Clear();
            if (atributes.picosQ5 != null)
                atributes.picosQ5.Clear();
            if (atributes.picosQ6 != null)
                atributes.picosQ6.Clear();
            if (atributes.picosQ7 != null)
                atributes.picosQ7.Clear();
            if (atributes.picosQ8 != null)
                atributes.picosQ8.Clear();
            if (atributes.picosQ9 != null)
                atributes.picosQ9.Clear();
            if (atributes.picosQ10 != null)
                atributes.picosQ10.Clear();
            if (atributes.picosQ11 != null)
                atributes.picosQ11.Clear();
            if (atributes.picosQ12 != null)
                atributes.picosQ12.Clear();

            if (atributes.picosR1 != null)
                atributes.picosR1.Clear();
            if (atributes.picosR2 != null)
                atributes.picosR2.Clear();
            if (atributes.picosR3 != null)
                atributes.picosR3.Clear();
            if (atributes.picosR4 != null)
                atributes.picosR4.Clear();
            if (atributes.picosR5 != null)
                atributes.picosR5.Clear();
            if (atributes.picosR6 != null)
                atributes.picosR6.Clear();
            if (atributes.picosR7 != null)
                atributes.picosR7.Clear();
            if (atributes.picosR8 != null)
                atributes.picosR8.Clear();
            if (atributes.picosR9 != null)
                atributes.picosR9.Clear();
            if (atributes.picosR10 != null)
                atributes.picosR10.Clear();
            if (atributes.picosR11 != null)
                atributes.picosR11.Clear();
            if (atributes.picosR12 != null)
                atributes.picosR12.Clear();

            if (atributes.picosS1 != null)
                atributes.picosS1.Clear();
            if (atributes.picosS2 != null)
                atributes.picosS2.Clear();
            if (atributes.picosS3 != null)
                atributes.picosS3.Clear();
            if (atributes.picosS4 != null)
                atributes.picosS4.Clear();
            if (atributes.picosS5 != null)
                atributes.picosS5.Clear();
            if (atributes.picosS6 != null)
                atributes.picosS6.Clear();
            if (atributes.picosS7 != null)
                atributes.picosS7.Clear();
            if (atributes.picosS8 != null)
                atributes.picosS8.Clear();
            if (atributes.picosS9 != null)
                atributes.picosS9.Clear();
            if (atributes.picosS10 != null)
                atributes.picosS10.Clear();
            if (atributes.picosS11 != null)
                atributes.picosS11.Clear();
            if (atributes.picosS12 != null)
                atributes.picosS12.Clear();

            if (atributes.picosT1 != null)
                atributes.picosT1.Clear();
            if (atributes.picosT2 != null)
                atributes.picosT2.Clear();
            if (atributes.picosT3 != null)
                atributes.picosT3.Clear();
            if (atributes.picosT4 != null)
                atributes.picosT4.Clear();
            if (atributes.picosT5 != null)
                atributes.picosT5.Clear();
            if (atributes.picosT6 != null)
                atributes.picosT6.Clear();
            if (atributes.picosT7 != null)
                atributes.picosT7.Clear();
            if (atributes.picosT8 != null)
                atributes.picosT8.Clear();
            if (atributes.picosT9 != null)
                atributes.picosT9.Clear();
            if (atributes.picosT10 != null)
                atributes.picosT10.Clear();
            if (atributes.picosT11 != null)
                atributes.picosT11.Clear();
            if (atributes.picosT12 != null)
                atributes.picosT12.Clear();

            atributes.age = "";
            atributes.gender = "";
            atributes.race = "";
            atributes.height = "";
            atributes.weight = "";
            atributes.hertz = "";
            atributes.secs = "";
            atributes.her = 0;
            atributes.herMod = 0;
            atributes.ventanaFiltroMedia = 0;

            atributes.pX_c1 = 0;
            atributes.pX_c2 = 0;
            atributes.pX_c3 = 0;
            atributes.pX_c4 = 0;
            atributes.pX_c5 = 0;
            atributes.pX_c6 = 0;
            atributes.pX_c7 = 0;
            atributes.pX_c8 = 0;
            atributes.pX_c9 = 0;
            atributes.pX_c10 = 0;
            atributes.pX_c11 = 0;
            atributes.pX_c12 = 0;
            atributes.pY_c1 = 0;
            atributes.pY_c2 = 0;
            atributes.pY_c3 = 0;
            atributes.pY_c4 = 0;
            atributes.pY_c5 = 0;
            atributes.pY_c6 = 0;
            atributes.pY_c7 = 0;
            atributes.pY_c8 = 0;
            atributes.pY_c9 = 0;
            atributes.pY_c10 = 0;
            atributes.pY_c11 = 0;
            atributes.pY_c12 = 0;

            atributes.segments = new double[12][];
            for (int i = 0; i < 12; i++)
            {
                atributes.segments[i] = new double[4];
            }
            atributes.inversionT = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

            atributes.reportFile = Directory.GetCurrentDirectory() + "/report.csv";
        }

        public static void restoreData(ref Atribs atr)
        {
            atr.data1f = new List<double>(atr.data1);
            atr.data2f = new List<double>(atr.data2);
            atr.data3f = new List<double>(atr.data3);
            atr.data4f = new List<double>(atr.data4);
            atr.data5f = new List<double>(atr.data5);
            atr.data6f = new List<double>(atr.data6);
            atr.data7f = new List<double>(atr.data7);
            atr.data8f = new List<double>(atr.data8);
            atr.data9f = new List<double>(atr.data9);
            atr.data10f = new List<double>(atr.data10);
            atr.data11f = new List<double>(atr.data11);
            atr.data12f = new List<double>(atr.data12);

            atr.raw1f = new List<double>(atr.raw1);
            atr.raw2f = new List<double>(atr.raw2);
            atr.raw3f = new List<double>(atr.raw3);
            atr.raw4f = new List<double>(atr.raw4);
            atr.raw5f = new List<double>(atr.raw5);
            atr.raw6f = new List<double>(atr.raw6);
            atr.raw7f = new List<double>(atr.raw7);
            atr.raw8f = new List<double>(atr.raw8);
            atr.raw9f = new List<double>(atr.raw9);
            atr.raw10f = new List<double>(atr.raw10);
            atr.raw11f = new List<double>(atr.raw11);
            atr.raw12f = new List<double>(atr.raw12);
        }

        public static void drawOnChart(ref Chart ch, ref List<double> ptos, int her, double secs, double block)
        {
            double tiempo = 0.0;
            foreach (double d in ptos)
            {
                ch.Series[0].Points.AddXY(tiempo, d);
                tiempo += 1.0 / (double) her;
            }
            ch.ChartAreas[0].AxisX.Minimum = 0;
            ch.ChartAreas[0].AxisX.Maximum = Math.Round((secs * her * 1.0) / her, 1);

            // enable autoscroll
            ch.ChartAreas[0].CursorX.AutoScroll = true;

            // let's zoom to [0,blockSize] (e.g. [0,100])
            ch.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            ch.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;

            ch.ChartAreas[0].AxisX.ScaleView.Zoom(0, block);

            ch.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // set scrollbar small change to blockSize (e.g. 100)
            ch.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = block;
        }

        public static void clearOnChart(ref Chart ch)
        {
            ch.Series[0].Points.Clear();

            ch.ChartAreas[0].AxisX.StripLines.Clear();
        }

        public static void pintarPicos(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod, Color c)
        {
            foreach (double a in picos)
            {
                StripLine stripline = new StripLine();
                stripline.Interval = 0;
                stripline.StripWidth = (1.0 * ancho) / (double)herMod;
                stripline.BackColor = c;
                stripline.IntervalOffset = a - ((ancho * 0.25) * (1.0 / herMod));
                lines.Add(stripline);
                ch.ChartAreas[0].AxisX.StripLines.Add(stripline);
            }
        }

        public static void pintarPicosR(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod)
        {
            pintarPicos(ref ch, ref lines, ref picos, ancho, herMod, Color.Green);
        }

        public static void pintarPicosQ(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod)
        {
            pintarPicos(ref ch, ref lines, ref picos, ancho, herMod, Color.Red);
        }

        public static void pintarPicosP(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod)
        {
            pintarPicos(ref ch, ref lines, ref picos, ancho, herMod, Color.Orange);
        }

        public static void pintarPicosS(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod)
        {
            pintarPicos(ref ch, ref lines, ref picos, ancho, herMod, Color.Blue);
        }

        public static void pintarPicosT(ref Chart ch, ref ArrayList lines, ref List<double> picos, double ancho, int herMod)
        {
            pintarPicos(ref ch, ref lines, ref picos, ancho, herMod, Color.Violet);
        }
    }
}
