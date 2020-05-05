using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DatabaseToCSV
{
    public partial class OpenFile : Form
    {
        private StreamReader streamReader;
        private string fileName;
        public OpenFile(string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;
        }

        //The datagrid is filled as soon as the form is loaded
        private void OpenFile_Load(object sender, EventArgs e)
        {
            streamReader = new StreamReader(fileName);
            streamReader.ReadLine();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] splitLine = line.Trim().Split(',');
                dataGridView1.Rows.Add(splitLine);
            }
            streamReader.Close();
        }
    }
}
