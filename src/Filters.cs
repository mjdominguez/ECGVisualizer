using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Filtering;

namespace ECGVisualizer
{
    class Filters
    {
        public static List<double> BandRejectionFilter(List<double> ecgSignal, double frecuenciaMuestreo, double frecuenciaCentral, double anchoBanda)
        {
            //double frecuenciaCentral = 55.0; // Frecuencia central del filtro de rechazo de banda
            //double anchoBanda = 5.0; // Ancho de banda del filtro de rechazo de banda

            double frecuenciaCorteInferior = frecuenciaCentral - anchoBanda / 2.0;
            double frecuenciaCorteSuperior = frecuenciaCentral + anchoBanda / 2.0;

            var filter = OnlineFilter.CreateBandstop(ImpulseResponse.Finite, frecuenciaMuestreo, frecuenciaCorteInferior, frecuenciaCorteSuperior, 3);

            List<double> filteredSignal = new List<double>();

            foreach (var sample in ecgSignal)
            {
                var filteredSample = filter.ProcessSample(sample);
                filteredSignal.Add(filteredSample);
            }

            return filteredSignal;
        }

        public static List<double> FixedAverageFilter(List<double> ecgSignal, int ventana)
        {
            List<double> filteredSignal = new List<double>();
            double sum = 0.0;
            int count = 0;
            foreach (double d in ecgSignal)
            {
                sum += d;
                count++;
                if (count == ventana)
                {
                    filteredSignal.Add(sum / ventana);
                    sum = 0.0;
                    count = 0;
                }
            }

            return filteredSignal;
        }

        public static List<double> MovingAverageFilter(List<double> signal, int windowSize)
        {
            List<double> filteredSignal = new List<double>();

            // Iterar sobre cada elemento de la señal
            for (int i = 0; i < signal.Count; i++)
            {
                // Obtener los índices de los elementos en la ventana deslizante
                int startIndex = Math.Max(0, i - (windowSize / 2));
                int endIndex = Math.Min(signal.Count - 1, i + (windowSize / 2));

                // Calcular la media de los elementos en la ventana
                double sum = 0;
                for (int j = startIndex; j <= endIndex; j++)
                {
                    sum += signal[j];
                }
                double average = sum / (endIndex - startIndex + 1);

                // Añadir el valor promedio a la señal filtrada
                filteredSignal.Add(average);
            }

            return filteredSignal;
        }

         public static List<double> SubtractSignals(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();

            // Iterar sobre cada elemento de las listas y restarlos
            for (int i = 0; i < signal1.Count; i++)
            {
                double subtraction = signal1[i] - signal2[i];
                result.Add(subtraction);
            }

            return result;
        }

        public static List<double> MedianFilter(List<double> signal, int windowSize)
        {
            List<double> filteredSignal = new List<double>();

            // Iterar sobre cada muestra de la señal
            for (int i = 0; i < signal.Count; i++)
            {
                // Obtener los índices de las muestras en la ventana deslizante
                int startIndex = Math.Max(0, i - (windowSize / 2));
                int endIndex = Math.Min(signal.Count - 1, i + (windowSize / 2));

                // Obtener los valores de las muestras en la ventana
                List<double> windowValues = signal.GetRange(startIndex, endIndex - startIndex + 1);

                // Calcular la mediana de la ventana y añadirla a la señal filtrada
                double median = windowValues.OrderBy(v => v).ElementAt(windowValues.Count / 2);
                filteredSignal.Add(median);
            }

            return filteredSignal;
        }
    }
}
