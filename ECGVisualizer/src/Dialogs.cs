using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ECGVisualizer
{
    public class Dialogs
    {
        public static void openFileDial(ref Atribs atributes)
        {
            atributes.openFileDialog1 = new OpenFileDialog();
            atributes.openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            atributes.openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            atributes.openFileDialog1.Title = "Please select an ECG recorded file.";
            atributes.archivoCargado = false;
        }

        public static void saveFileDial(ref Atribs atributes)
        {
            atributes.saveFileDialog1 = new SaveFileDialog();

            atributes.saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            //atributes.saveFileDialog1.FilterIndex = 2;
            atributes.saveFileDialog1.RestoreDirectory = true;
            //atributes.saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";


            //atributes.saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            atributes.saveFileDialog1.Title = "Please select the name for the new ECG file.";
            //atributes.archivoCargado = false;
        }

        public static void configReport(ref Atribs atributes)
        {
            atributes.doc = new XmlDocument();

            atributes.reportFile = Directory.GetCurrentDirectory() + "/report.csv";

            atributes.segments = new double[12][];
            for (int i = 0; i < 12; i++)
            {
                atributes.segments[i] = new double[6];
            }
            atributes.inversionT = new bool[12] {false, false, false, false, false, false, false, false, false, false, false, false };

            atributes.repAge = true;
            atributes.repHeight = true;
            atributes.repWeight = true;
            atributes.repRace = true;
            atributes.repGender = false;
            atributes.repPQ = false;
            atributes.repQR = false;
            atributes.repRS = false;
            atributes.repST = false;
            atributes.repPR = false;
            atributes.repRR = true;
            atributes.repQT = true;
            atributes.repQT_c = true;
            atributes.repQRS = true;
            atributes.repi1 = true;
            atributes.repi2 = true;
            atributes.repi3 = true;
            atributes.repaVR = true;
            atributes.repaVL = true;
            atributes.repaVF = true;
            atributes.repV1 = true;
            atributes.repV2 = true;
            atributes.repV3 = true;
            atributes.repV4 = true;
            atributes.repV5 = true;
            atributes.repV6 = true;
        }
    }
}
