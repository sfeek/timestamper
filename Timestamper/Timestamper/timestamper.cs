using System;
using System.IO;
using System.Media;
using System.Security;
using System.Windows.Forms;

namespace Timestamper
{
    public partial class timestamper : Form
    {
        String filePath = string.Empty;
        String startTime = string.Empty;
        String endTime = string.Empty;
        Int32 eventCounter = 0;
        Timer timer1;

        public timestamper()
        {
            InitializeComponent();
            timer1 = new Timer();
            timer1.Tick += new System.EventHandler(OnTimerEvent);
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy,HH:mm:ss");
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filePath = saveFileDialog1.FileName;
                    btnStart.Enabled = true;
                }
                catch (SecurityException ex)
                {
                    btnStart.Enabled = false;
                    MessageBox.Show($"File Error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (filePath == String.Empty) return;

            startTime = GetTimestamp(DateTime.Now);
            try
            {
                timer1.Interval = Convert.ToInt32(Convert.ToDouble(txtSeconds.Text) * 1000.0);
            }
            catch { return; }
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            eventCounter++;
            SystemSounds.Asterisk.Play();
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            timer1.Enabled = false;
            eventDone();
        }

        private void eventDone()
        {
            endTime = GetTimestamp(DateTime.Now);
            SystemSounds.Asterisk.Play();
            File.AppendAllText(filePath, eventCounter.ToString()+","+startTime+","+endTime + Environment.NewLine);
        }

        private void OnTimerEvent(object sender, EventArgs e)
        {
            eventDone();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            timer1.Enabled = false;
        }
    }
}
