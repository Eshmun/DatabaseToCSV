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
    public partial class MainForm : Form
    {
        DatabaseForm databaseForm = new DatabaseForm();
        private FileStream fileStream;
        private string fileName;

        public MainForm()
        {
            InitializeComponent();
        }

        //Funtion to switch to the DatabaseForm
        private void openButton_Click(object sender, EventArgs e)
        {
            databaseForm.Show();
        }

        //This function is called when the MainForm gets focus. It checks whether the user has succesfully selected data, if true then it changes the opendatabase button to green
        private void MainForm_GotFocus(object sender, EventArgs e)
        {
            openDatabaseButton.BackColor = Color.Transparent;
            if (databaseForm.Saved)
            {
                Console.WriteLine("Saved");
                openDatabaseButton.BackColor = Color.Green;
            }
            Console.WriteLine("MainForm Focus");

        }

        //Function to handle the file saving. It saves, appends or overwrites depending on the selected radiobutton
        private void saveFileButton_Click(object sender, EventArgs e)
        {
            bool succesful = false;
            if (databaseForm.Saved)
            {
                string option = "newButton";
                foreach (RadioButton radioButton in groupBox1.Controls.OfType<RadioButton>())
                {
                    if (radioButton.Checked)
                    {
                        option = radioButton.Name;
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Comma Separated Files(.csv)|*.csv|All Files(*.*)|*.*";
                saveFileDialog.FilterIndex = 0;


                switch (option)
                {
                    case "newButton":
                        Console.WriteLine("new");

                        saveFileDialog.CheckFileExists = false;
                        saveFileDialog.Title = "Save a new file";
                        saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (saveFileDialog.FileName != "")
                            {
                                fileName = saveFileDialog.FileName;
                                fileLabel.Text = fileName;
                                fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);

                                byte[] header = Encoding.ASCII.GetBytes("ID, Time, Amp, Voltage\n");
                                fileStream.Write(header, 0, header.Length);

                                byte[] newline;

                                foreach (Object[] row in databaseForm.Rows)
                                {
                                    newline = Encoding.ASCII.GetBytes(String.Format("{0},{1},{2},{3}\n",
                                                                                    row[0],
                                                                                    row[1],
                                                                                    ((float)row[2]).ToString("F", new System.Globalization.CultureInfo("en-US")),
                                                                                    ((float)row[3]).ToString("F", new System.Globalization.CultureInfo("en-US"))));
                                    fileStream.Write(newline, 0, newline.Length);
                                }
                                fileStream.Close();
                                succesful = true;
                            }
                        }
                        
                        break;
                    case "appendButton":
                        Console.WriteLine("append");

                        saveFileDialog.CheckFileExists = true;
                        saveFileDialog.Title = "Append to a file";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (saveFileDialog.FileName != "")
                            {
                                fileName = saveFileDialog.FileName;
                                fileLabel.Text = fileName;
                                fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);

                                byte[] newline;

                                foreach (Object[] row in databaseForm.Rows)
                                {
                                    newline = Encoding.ASCII.GetBytes(String.Format("{0},{1},{2},{3}\n",
                                                                                    row[0],
                                                                                    row[1],
                                                                                    ((float)row[2]).ToString("F", new System.Globalization.CultureInfo("en-US")),
                                                                                    ((float)row[3]).ToString("F", new System.Globalization.CultureInfo("en-US"))));
                                    fileStream.Write(newline, 0, newline.Length);
                                }
                                fileStream.Close();
                                succesful = true;
                            }
                        }

                        break;
                    case "overwriteButton":
                        Console.WriteLine("overwrite");
                        saveFileDialog.CheckFileExists = true;
                        saveFileDialog.Title = "Overwrite a file";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (saveFileDialog.FileName != "")
                            {
                                fileName = saveFileDialog.FileName;
                                fileLabel.Text = fileName;
                                fileStream = new FileStream(fileName, FileMode.Truncate, FileAccess.Write);

                                byte[] header = Encoding.ASCII.GetBytes("ID, Time, Amp, Voltage\n");
                                fileStream.Write(header, 0, header.Length);

                                byte[] newline;

                                foreach (Object[] row in databaseForm.Rows)
                                {
                                    newline = Encoding.ASCII.GetBytes(String.Format("{0},{1},{2},{3}\n",
                                                                                    row[0],
                                                                                    row[1],
                                                                                    ((float)row[2]).ToString("F", new System.Globalization.CultureInfo("en-US")),
                                                                                    ((float)row[3]).ToString("F", new System.Globalization.CultureInfo("en-US"))));
                                    fileStream.Write(newline, 0, newline.Length);
                                }
                                fileStream.Close();
                                succesful = true;

                            }
                        }
                        
                        break;
                    default:
                        break;
                }
                if (succesful)
                {
                    saveFileButton.BackColor = Color.Green; 
                }
            }
            else
            {
                MessageBox.Show("Select Database values first");
                saveFileButton.BackColor = Color.Transparent;
            }

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Function to handle opening a file. A Openfiledialog is started and the filename is passed to the OpenFile class
        private void openFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Comma Separated Files(.csv)|*.csv|All Files(*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "Select a File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName != "")
                {
                    fileLabel.Text = openFileDialog.FileName;
                    fileName = openFileDialog.FileName;
                    OpenFile openFile = new OpenFile(fileName);
                    openFile.Show();
                }

            }


        }
    }
}
