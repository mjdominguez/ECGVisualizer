using System;
using System.Collections.Generic;

namespace ECGVisualizer
{
    class ECGpeaks
    {
        public static bool NormalOInvertido(List<double> ecgSignal)
        {
            bool norm = true;
            int count = 0;
            double sum = 0.0;
            double max = ecgSignal[0], min = ecgSignal[0];

            foreach (double d in ecgSignal)
            {
                sum += d;
                count++;
                if (d > max)
                    max = d;
                else if (d < min)
                    min = d;
            }
            sum = sum / count;
            if (Math.Abs(max - sum) < Math.Abs(sum - min))
                norm = false;

            return norm;
        }

        public static bool TposOneg2(List<double> ecgSignal, List<double> puntosS, int ventanaMax, int ventanaMin, int hercios, double ruido)
        {
            bool pos = true;
            int count = 0;
            double sum = 0.0;
            double max = -500, min = 500;
            double segSTmin = (ventanaMin * 1.0) - (ventanaMin * ruido);
            double segSTmax = ((ventanaMax * 1.0) + (ventanaMax * ruido))*1.5;
            int numNegs = 0, numPos = 0;

            foreach (double puntoS in puntosS)
            {
                int ini = (int)((puntoS * hercios) + segSTmin) > (ecgSignal.Count - 1)? (ecgSignal.Count - 1): (int)((puntoS * hercios) + segSTmin);
                int fin = (int)(ini + segSTmax) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (int)(ini + segSTmax);
                min = ecgSignal[ini];
                max = ecgSignal[ini];
                for (int i = ini; i < fin && i < ecgSignal.Count; i++)
                {
                    sum += ecgSignal[i];
                    
                    if (ecgSignal[i] > max)
                        max = ecgSignal[i];
                    else if (ecgSignal[i] < min)
                        min = ecgSignal[i];
                }
                if (Math.Abs(max) < Math.Abs(min))
                    numNegs++;
                else
                    numPos++;
                count++;
            }

            if (numNegs > numPos)
                pos = false;

            return pos;
        }

        public static bool PposOneg(List<double> ecgSignal, List<double> puntosQ, int ventanaMax, int ventanaMin, double ruido)
        {
            bool pos = true;
            int count = 0;
            double sum = 0.0;
            double max = -500, min = 500;
            int segPQmin = (int) ((ventanaMin * 0.8) - (ventanaMin * ruido));
            int segPQmax = (int) (ventanaMax * 1.2 + (ventanaMax * ruido));

            foreach (int puntoQ in puntosQ)
            {
                int ini = (puntoQ - segPQmin)>=0? puntoQ - segPQmin:0;
                int fin = (ini - segPQmax)>=0? ini - segPQmax:0;
                min = ecgSignal[ini];
                max = ecgSignal[ini];
                for (int i = ini; i > fin && i > 0; i--)
                {
                    sum += ecgSignal[i];
                    count++;
                    if (ecgSignal[i] > max)
                        max = ecgSignal[i];
                    else if (ecgSignal[i] < min)
                        min = ecgSignal[i];
                }
            }
            sum = sum / count;
            if (Math.Abs(max - sum) < Math.Abs(sum - min))
                pos = false;

            return pos;
        }

        public static List<double> DetectarPicosR2(List<double> ecgSignal, int ventanaAncho, int retraso, int hercios, double ruido)
        {
            List<double> picosR = new List<double>();
            double max = ecgSignal[0];
            double min = ecgSignal[0];
            double primerMax = ecgSignal[0];
            double primerMin = ecgSignal[0];
            int indexMax = 0, indexMin = 0;
            bool nuevo = false;

            bool normal = NormalOInvertido(ecgSignal);

            if (normal)
            {
                for (int i = 1; i < ventanaAncho; i++)
                {
                    if (ecgSignal[i] > max)
                    {
                        max = ecgSignal[i];
                        indexMax = i;
                        nuevo = true;
                    }
                }
                if (nuevo)
                {
                    primerMax = max;
                    picosR.Add(((indexMax)/(double)hercios));
                    nuevo = false;
                }
                for (int i = indexMax + retraso; i < ecgSignal.Count; i++)
                {
                    max = ecgSignal[i];
                    indexMax = i;
                    int fin = ((i + ventanaAncho) < ecgSignal.Count) ? (i + ventanaAncho) : ecgSignal.Count - 1;
                    for (int j = i; j<fin; j++)
                    {
                        if (ecgSignal[j] > max && ecgSignal[j] > (primerMax * (1 - ruido)))
                        {
                            max = ecgSignal[j];
                            indexMax = j;
                            nuevo = true;
                        }
                    }
                    if (nuevo)
                    {
                        picosR.Add(((indexMax) / (double)hercios));
                        i = indexMax + retraso;
                        nuevo = false;
                    }
                    else
                        i = i + ventanaAncho;
                }
            }
            else
            {
                for (int i = 1; i < ventanaAncho; i++)
                {
                    if (ecgSignal[i] < min)
                    {
                        min = ecgSignal[i];
                        indexMin = i;
                        nuevo = true;
                    }
                }
                if (nuevo)
                {
                    primerMin = min;
                    picosR.Add(((indexMin) / (double)hercios));
                    nuevo = false;
                }
                for (int i = indexMin + retraso; i < ecgSignal.Count; i++)
                {
                    min = ecgSignal[i];
                    indexMin = i;
                    int fin = ((i + ventanaAncho) < ecgSignal.Count) ? (i + ventanaAncho) : ecgSignal.Count - 1;
                    for (int j = i; j < fin; j++)
                    {
                        if (ecgSignal[j] < min && ecgSignal[j] < primerMin * (1 - ruido))
                        {
                            min = ecgSignal[j];
                            indexMin = j;
                            nuevo = true;
                        }
                    }
                    if (nuevo)
                    {
                        picosR.Add(((indexMin) / (double)hercios));
                        i = indexMin + retraso;
                        nuevo = false;
                    }
                    else
                        i = i + ventanaAncho;
                }
            }

            return picosR;
        }

        public static List<double> DetectarPicosQ2(List<double> ecgSignal, List<double> picosR, int ventanaAnchoMax, int ventanaAnchoMin, double umbralPicoP, int hercios, double ruido)
        {
            List<double> picosQ = new List<double>();

            double min, max, anterior;
            int indexMin, indexMax;
            int ini, fin;
            int QRsegMax = (int)(ventanaAnchoMax*1.0 + (ventanaAnchoMax * ruido));
            int QRsegMin = (int)(ventanaAnchoMin*1.0 - (ventanaAnchoMin * ruido));

            bool normal = NormalOInvertido(ecgSignal);

            if (normal)
            {
                foreach (double picoR in picosR)
                {
                    indexMin = (int)(picoR*hercios)-1;
                    min = ecgSignal[indexMin];
                    anterior = ecgSignal[indexMin+1];
                    ini = indexMin;
                    fin = (ini - QRsegMax - 1)<0? 0:ini - QRsegMax - 1;
                    for (int i = ini; i >= fin && i >= 0; i--)
                    {
                        if (anterior>ecgSignal[i])
                        {
                            anterior = min;
                            min = ecgSignal[i];
                            indexMin = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    picosQ.Add(((indexMin) / (double)hercios));
                }
            }
            else
            {
                foreach (double picoR in picosR)
                {
                    indexMax = (int)(picoR * hercios) - 1;
                    max = ecgSignal[indexMax];
                    anterior = ecgSignal[indexMax+1];
                    ini = indexMax;
                    fin = (ini - QRsegMax - 1) < 0 ? 0:ini - QRsegMax - 1;
                    for (int i = ini; i >= fin && i >= 0; i--)
                    {
                        if (anterior < ecgSignal[i])
                        {
                            anterior = max;
                            max = ecgSignal[i];
                            indexMax = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    picosQ.Add(((indexMax) / (double)hercios));
                }
            }

            return picosQ;
        }

        public static List<double> DetectarPicosS2(List<double> ecgSignal, List<double> picosR, int ventanaAnchoMax, int ventanaAnchoMin, double umbralPicoP, int hercios, double ruido)
        {
            List<double> picosS = new List<double>();

            double min, max, anterior;
            int indexMin, indexMax;
            int ini, fin;

            bool normal = NormalOInvertido(ecgSignal);

            int RSsegMax = (int) ((ventanaAnchoMax*1.0) + (ventanaAnchoMax * ruido));
            int RSsegMin = (int) ((ventanaAnchoMin*1.0) - (ventanaAnchoMin * ruido));

            if (normal)
            {
                foreach (double picoR in picosR)
                {
                    indexMin = (int)(picoR * hercios) + 1;
                    ini = (indexMin+1) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (indexMin + 1);
                    min = ecgSignal[ini];
                    anterior = ecgSignal[ini-1];

                    fin = (ini + RSsegMax + 1) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (ini + RSsegMax + 1);

                    for (int i = ini; i <= fin && i < ecgSignal.Count; i++)
                    {
                        if (anterior > ecgSignal[i])
                        {
                            anterior = min;
                            min = ecgSignal[i];
                            indexMin = i;
                        }
                        else
                        {
                            break;
                        }

                    }
                    picosS.Add(((indexMin) / (double)hercios));
                }
            }
            else
            {
                foreach (double picoR in picosR)
                {
                    indexMax = (int)(picoR * hercios) + 1;
                    ini = (indexMax+1) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (indexMax + 1); ;
                    max = ecgSignal[ini];
                    anterior = ecgSignal[ini - 1];
                    fin = (ini + RSsegMax + 1) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (ini + RSsegMax + 1);
                    for (int i = ini; i <= fin && i < ecgSignal.Count; i++)
                    {
                        if (anterior < ecgSignal[i])
                        {
                            anterior = max;
                            max = ecgSignal[i];
                            indexMax = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    picosS.Add(((indexMax) / (double)hercios));
                }
            }

            return picosS;
        }

         public static List<double> DetectarPicosT1(List<double> ecgSignal, List<double> picosS, int ventanaAnchoMax, int ventanaAnchoMin, int hercios, double ruido, int window)
        {
            List<double> picosT = new List<double>();

            int segmentSTmin = (int)((ventanaAnchoMin * 1.0) - (ventanaAnchoMin * ruido));
            int segmentSTmax = (int)((ventanaAnchoMax * 1.0) + (ventanaAnchoMax * ruido));

            double min, max;
            int indexMin, indexMax;
            int ini, fin;

            bool normal = NormalOInvertido(ecgSignal);
            bool Tsignalpos = TposOneg2(ecgSignal, picosS, ventanaAnchoMax, ventanaAnchoMin, hercios, ruido);

            if (!Tsignalpos)
            {
                foreach (double picoS in picosS)
                {
                    indexMin = (((int)(picoS * hercios) + segmentSTmin)) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (int)((picoS * hercios) + segmentSTmin);
                    min = ecgSignal[indexMin];
                    ini = indexMin;
                    fin = (int)((picoS * hercios) + segmentSTmax) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : (int)((picoS * hercios) + segmentSTmax);
                    for (int i = ini; i <= fin && i < ecgSignal.Count; i++)
                    {
                        if (ecgSignal[i] < min)
                        {
                            min = ecgSignal[i];
                            indexMin = i;
                        }
                        else if (i > (ini + window) && higherWindow(ref ecgSignal, i, window, 1))
                            break;
                    }
                    picosT.Add(((indexMin) / (double)hercios));
                }
            }
            else
            {
                foreach (double picoS in picosS)
                {
                    indexMax = ((int)(picoS * hercios) + segmentSTmin) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : ((int)(picoS * hercios) + segmentSTmin);
                    max = ecgSignal[indexMax];
                    ini = indexMax;
                    fin = ((int)(picoS * hercios) + segmentSTmax) > (ecgSignal.Count - 1) ? (ecgSignal.Count - 1) : ((int)(picoS * hercios) + segmentSTmax);
                    for (int i = ini; i <= fin && i < ecgSignal.Count; i++)
                    {
                        if (ecgSignal[i] > max)
                        {
                            max = ecgSignal[i];
                            indexMax = i;
                        }
                        else if (i > (ini + window) && smallerWindow(ref ecgSignal, i, window, 1))
                            break;
                    }
                    picosT.Add(((indexMax) / (double)hercios));
                }
            }

            return picosT;
        }

        public static List<double> DetectarPicosP1(List<double> ecgSignal, List<double> picosQ, int ventanaAnchoMax, int ventanaAnchoMin, int hercios, double ruido, int window)
        {
            List<double> picosP = new List<double>();

            int PQsegMax = (int)(ventanaAnchoMax * 1.0 + (ventanaAnchoMax * ruido));
            int PQsegMin = (int)(ventanaAnchoMin * 1.0 - (ventanaAnchoMin * ruido));

            double min, max;
            int indexMin, indexMax;
            int ini, fin;

            bool normal = NormalOInvertido(ecgSignal);

            if (!normal)
            {
                foreach (double picoQ in picosQ)
                {
                    indexMin = (int)((picoQ*hercios) - PQsegMin) >=0 ? (int) (picoQ*hercios) - PQsegMin: 0;
                    min = ecgSignal[indexMin];
                    ini = indexMin;
                    fin = ((int)(picoQ * hercios) - PQsegMax) >= 0 ? (int)(picoQ * hercios) - PQsegMax : 0;
                    for (int i = ini; i >= fin; i--)
                    {
                        if (ecgSignal[i] < min)
                        {
                            min = ecgSignal[i];
                            indexMin = i;
                        }
                        else if (i < (ini-window) && higherWindow(ref ecgSignal, i, window, 0))
                            break;
                    }
                    picosP.Add(((indexMin) / (double)hercios));
                }
            }
            else
            {
                foreach (double picoQ in picosQ)
                {
                    indexMax = (int)((picoQ*hercios)-PQsegMin) >= 0 ? (int)((picoQ * hercios) - PQsegMin) : 0;
                    max = ecgSignal[indexMax];
                    ini = indexMax;
                    fin = ((int)(picoQ * hercios) - PQsegMax) >= 0 ? (int)(picoQ * hercios) - PQsegMax : 0;

                    for (int i = ini; i >= fin && i > 0; i--)
                    {
                        if (ecgSignal[i] > max)
                        {
                            max = ecgSignal[i];
                            indexMax = i;
                        }
                        else if (i < (ini - window) && smallerWindow(ref ecgSignal, i, window, 0))
                            break;
                    }
                    picosP.Add(((indexMax) / (double)hercios));
                }
            }

            return picosP;
        }

        private static bool higherWindow(ref List<double> ecgSignal, int pos, int win, int dir)
        {
            bool dev = true;
            if (dir == 0)
            {
                for (int i = pos; i <= pos + win; i++)
                {
                    if (ecgSignal[pos] < ecgSignal[i])
                    {
                        dev = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = pos; i >= pos - win; i--)
                {
                    if (ecgSignal[pos] < ecgSignal[i])
                    {
                        dev = false;
                        break;
                    }
                }
            }
            return dev;
        }

        private static bool smallerWindow(ref List<double> ecgSignal, int pos, int win, int dir)
        {
            bool dev = true;
            if (dir == 0)
            {
                for (int i = pos; i <= pos + win; i++)
                {
                    if (ecgSignal[pos] > ecgSignal[i])
                    {
                        dev = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = pos; i >= pos - win; i--)
                {
                    if (ecgSignal[pos] > ecgSignal[i])
                    {
                        dev = false;
                        break;
                    }
                }
            }
            return dev;
        }

        public static List<double> DetectarPicosP2(List<double> ecgSignal, List<double> picosQ, int ventanaAnchoMax, int ventanaAnchoMin, int hercios, double ruido)
        {
            List<double> picosP = new List<double>();

            int PQsegMax = (int)(ventanaAnchoMax*1.0 + (ventanaAnchoMax * ruido));
            int PQsegMin = (int)(ventanaAnchoMin*1.0 - (ventanaAnchoMin * ruido));

            double min, max;
            int indexMin, indexMax;
            int ini, fin;

            bool normal = NormalOInvertido(ecgSignal);
            bool Psignalpos = PposOneg(ecgSignal, picosQ, ventanaAnchoMax, ventanaAnchoMin, ruido);

            if (!normal)
            {
                foreach (double picoQ in picosQ)
                {
                    indexMin = ((int)((picoQ * hercios) - 1))>=0? (int)((picoQ * hercios)-1):0;
                    min = ecgSignal[indexMin];
                    ini = indexMin;
                    fin = ((int)(picoQ * hercios) - PQsegMax) >=0? (int)(picoQ * hercios) - PQsegMax : 0;
                    for (int i = ini; i >= fin && i > 0; i--)
                    {
                        if (ecgSignal[i] < min)
                        {
                            min = ecgSignal[i];
                            indexMin = i;
                        }
                    }
                    picosP.Add(((indexMin) / (double)hercios));
                }
            }
            else
            {
                foreach (double picoQ in picosQ)
                {
                    indexMax = ((int)((picoQ * hercios) - 1)) >= 0 ? (int)((picoQ * hercios) - 1) : 0;
                    max = ecgSignal[indexMax];
                    ini = indexMax;
                    fin = ((int)(picoQ * hercios) - PQsegMax) >= 0 ? (int)(picoQ * hercios) - PQsegMax : 0;

                    for (int i = ini; i >= fin && i > 0; i--)
                    {
                        if (ecgSignal[i] > max)
                        {
                            max = ecgSignal[i];
                            indexMax = i;
                        }
                    }
                    picosP.Add(((indexMax) / (double)hercios));
                }
            }

            return picosP;
        }
    }
}
