using ProjectCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vComputeClient
{
    public partial class Form1 : Form
    {
        _Client client;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new _Client(Microsoft.VisualBasic.Interaction.InputBox("Enter the server address","Server Details"));
            execute.Enabled = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string str = await client.RegisterClient();
            button1.Name = str;
            execute.Enabled = true;
            button1.BackColor = Color.Green;
            button1.Text = client.NodeName;
            button1.Enabled = false;
        }

        private async void execute_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            byte[] assemblyByte = client.complieAssembly(assemblyCode.Text,out error);
            if (assemblyByte == null)
                MessageBox.Show("There are some errors in your code: "+error);
            else
            {
                string assemblyName = Microsoft.VisualBasic.Interaction.InputBox("Enter assembly name", "Assembly Name");
                string taskId = await client.SubmitTask(assemblyName, assemblyByte, assemblyCodeParams.Text);
                string output = (string) await client.GetResult(taskId);
                MessageBox.Show("Output: " + output);
                return;
            }
           // MessageBox.Show("No result");
        }
    }
}
