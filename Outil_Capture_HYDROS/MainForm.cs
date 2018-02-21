using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Outil_Capture_HYDROS
{
    public partial class MainForm : Form
    {
        // Global vars
        ProcessStartInfo startInfo;
        Process proc;
        List<Trame> trames = new List<Trame>();
        string COMport;
        bool procStarted;

        public MainForm(string args)
        {
            InitializeComponent();
            InitProgram(args);
        }

        private void InitProgram(string args)
        {
            COMport = args;
            startInfo = new ProcessStartInfo()
            {
                FileName = "get_frame.exe",
                Arguments = COMport,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            proc = new Process();
            proc.StartInfo = startInfo;
            proc.OutputDataReceived += new DataReceivedEventHandler(OutputData_Received);

            procStarted = proc.Start();
            proc.BeginOutputReadLine();
            
        }

        private void OutputData_Received(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                trames.Add(new Trame(e.Data));
                AppendOutput(listBox1, e.Data);
                if(défilementAutoToolStripMenuItem.Checked) ScrollToBottom(listBox1);
            }
        }

        private void AppendOutput(ListBox listBox, string data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action < ListBox, string>)AppendOutput, listBox, data);
            }
            else
            {
                if (data != null) listBox.Items.Add(data);
            }
        }

        private void ScrollToBottom(ListBox listBox)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action<ListBox>) ScrollToBottom, listBox);
            } 
            else
            {
                if(listBox.Items.Count != 0) listBox.TopIndex = listBox.Items.Count - 1;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Connecté à " + COMport;
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            proc.Kill();
            Application.Exit();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string[] data = new string[6];
            data[0] = trames[index].IDTotem;
            data[1] = trames[index].NumPaquet.ToString();
            data[2] = string.Format("{0:N3} mm", trames[index].Data[0]);
            data[3] = string.Format("{0:N2} °C", trames[index].Data[1]);
            data[4] = string.Format("{0:N2} °C", trames[index].Data[3]);
            data[5] = string.Format("{0} %", trames[index].Data[2]);

            textBox1.Text = data[0];
            textBox6.Text = data[1];
            textBox2.Text = data[2];
            textBox3.Text = data[3];
            textBox4.Text = data[4];
            textBox5.Text = data[5];
        }

        private void défilementAutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (défilementAutoToolStripMenuItem.Checked) défilementAutoToolStripMenuItem.Checked = false;
            else défilementAutoToolStripMenuItem.Checked = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("get_frame"))
            {
                process.Kill();
            }
        }
    }
}
