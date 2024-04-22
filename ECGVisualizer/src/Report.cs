using System;
using System.Collections.Generic;

namespace ECGVisualizer
{
    class Report
    {
        public static bool Tinversion(ref List<double> data, ref List<double> dots, double hertz)
        {
            bool derv;
            derv = ECGpeaks.NormalOInvertido(data);
            int bien = 0, mal = 0;
            bool inversion = false;
            foreach (double puntoT in dots)
            {
                int posPuntoT = (int)(puntoT * hertz);
                if ((derv && data[posPuntoT] >= 0) || (!derv && data[posPuntoT] <= 0))
                    bien++;
                else
                    mal++;
            }
            if (mal > bien)
                inversion = true;
            return inversion;
        }

        public static List<double> segmentsCalc(ref List<double> Pdots, ref List<double> Qdots, ref List<double> Rdots, ref List<double> Sdots, ref List<double> Tdots)
        {
            List<double> dev = new List<double>();

            dev.Add(pq(ref Pdots, ref Qdots));

            dev.Add(qr(ref Qdots, ref Rdots));

            dev.Add(rs(ref Rdots, ref Sdots));

            dev.Add(st(ref Sdots, ref Tdots));

            dev.Add(pr(ref Pdots, ref Rdots));

            dev.Add(qrs(ref Qdots, ref Rdots, ref Sdots));

            dev.Add(rr(ref Rdots));

            dev.Add(qt(ref Qdots, ref Tdots));

            dev.Add(qt_c(ref Qdots, ref Tdots, ref Rdots));

            return dev;
        }

        private static double pq(ref List<double> Pdots, ref List<double> Qdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Pdots.Count; a++)
            {
                if (a < Pdots.Count && a < Qdots.Count)
                    sum += Math.Abs((Qdots[a] - Pdots[a]));
            }
            return (sum / Pdots.Count);
        }
        private static double qr(ref List<double> Qdots, ref List<double> Rdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Qdots.Count; a++)
            {
                if (a < Rdots.Count && a < Qdots.Count)
                    sum += Math.Abs((Rdots[a] - Qdots[a]));

            }
            return (sum / Qdots.Count);
        }
        private static double pr(ref List<double> Pdots, ref List<double> Rdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Pdots.Count; a++)
            {
                if (a < Rdots.Count && a < Pdots.Count)
                    sum += Math.Abs((Rdots[a] - Pdots[a]));

            }
            return (sum / Pdots.Count);
        }
        private static double rs(ref List<double> Rdots, ref List<double> Sdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Rdots.Count; a++)
            {
                if (a < Rdots.Count && a < Sdots.Count)
                    sum += Math.Abs((Sdots[a] - Rdots[a]));
            }
            return (sum / Rdots.Count);
        }
        private static double st(ref List<double> Sdots, ref List<double> Tdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Sdots.Count; a++)
            {
                if (a < Tdots.Count && a < Sdots.Count)
                    sum += Math.Abs((Tdots[a] - Sdots[a]));
            }
            return (sum / Tdots.Count);
        }
        private static double qrs(ref List<double> Qdots, ref List<double> Rdots, ref List<double> Sdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Qdots.Count; a++)
            {
                if (a < Rdots.Count && a < Sdots.Count)
                    sum += Math.Abs((Sdots[a] - Qdots[a]));
            }
            return (sum / Qdots.Count);
        }
        private static double rr(ref List<double> Rdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Rdots.Count - 1; a++)
            {
                if (a < Rdots.Count - 1)
                    sum += Math.Abs((Rdots[a + 1] - Rdots[a]));
            }
            return (sum / Rdots.Count);
        }
        private static double qt(ref List<double> Qdots, ref List<double> Tdots)
        {
            double sum = 0.0;
            for (int a = 0; a < Qdots.Count; a++)
            {
                if (a < Qdots.Count && a < Tdots.Count)
                    sum += Math.Abs((Tdots[a] - Qdots[a]));
            }
            return (sum / Qdots.Count);
        }

        //the corrected QT interval (QTc) is calculated by dividing the QT interval by the square root of the preceeding R - R interval.
        private static double qt_c(ref List<double> Qdots, ref List<double> Tdots, ref List<double> Rdots)
        {
            double aux, sum = 0.0;
            for (int a = 1; a < Qdots.Count; a++)
            {
                if (a < Qdots.Count && a < Tdots.Count)
                {
                    aux = Math.Abs((Tdots[a] - Qdots[a]));
                    aux = aux / Math.Sqrt(Rdots[a] - Rdots[a-1]);
                    sum += aux;
                }
            }
            return (sum / (Qdots.Count-1));
        }

    }
}
