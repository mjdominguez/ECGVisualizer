using System.Windows.Forms;
using System.Drawing;

namespace ECGVisualizer
{
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
            int r = -1;
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
}
