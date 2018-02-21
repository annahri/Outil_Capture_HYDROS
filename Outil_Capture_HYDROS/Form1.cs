using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Outil_Capture_HYDROS
{
    public partial class Form1 : Form
    {
        // Global vars
        public string comPort;

        public Form1()
        {
            InitializeComponent();
            GetPortList(comboBox1);
        }

        private void GetPortList(ComboBox comboBox)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox.Items.Add(s);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("get_frame"))
            {
                process.Kill();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (File.Exists("get_frame.exe"))
                {
                    comPort = comboBox1.Text;
                    this.Hide();
                    var mainForm = new MainForm(comPort);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Programme 'get_frame.exe' introuvable", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Selectionner le COM port", "Oups");
            }
        }
    }
}
