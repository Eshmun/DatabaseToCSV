using System;
using System.Windows.Forms;

namespace DatabaseToCSV
{
    public partial class DatabaseForm : Form
    {
        DatabaseHandler databaseHandler = new DatabaseHandler("\\data_new.db");

        public bool Saved { get; set; } = false;
        public object[] Rows { get; set; }

        public DatabaseForm()
        {
            InitializeComponent();
        }

        private void getButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int amount = (int)amountUpDown.Value;
            DateTime startDate = startDatePicker.Value.Date + startTimePicker.Value.TimeOfDay;
            DateTime endDate = endDatePicker.Value.Date + endTimePicker.Value.TimeOfDay;
            Rows = databaseHandler.GetRows(amount, startDate, endDate);
            foreach (Object[] row in Rows)
            {
                dataGridView1.Rows.Add(row);
            }
        }

        private void DatabaseForm_Load(object sender, EventArgs e)
        {
            Saved = false;
            startDatePicker.MaxDate = databaseHandler.GetEndDate();
            startDatePicker.MinDate = databaseHandler.GetStartDate();
            endDatePicker.MaxDate = databaseHandler.GetEndDate();
            endDatePicker.MinDate = databaseHandler.GetStartDate();


            startDatePicker.Value = databaseHandler.GetStartDate();
            startTimePicker.Value = databaseHandler.GetStartDate();

            startTimePicker.ShowUpDown = true;
            endTimePicker.ShowUpDown = true;
        }
        private void DatabaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            Saved = false;
            dataGridView1.Rows.Clear();
            e.Cancel = true;
            ActiveForm.Hide();
        }

            private void clearButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1)
            {
                for (int row = 0; row < dataGridView1.Rows.Count - 1; row++)
                {
                    Object[] tempRow = { dataGridView1[0, row].Value, dataGridView1[1, row].Value, dataGridView1[2, row].Value, dataGridView1[3, row].Value };
                    Rows.SetValue(tempRow, row);
                }
                Saved = true;
                ActiveForm.Hide();
            }
            else
            {
                MessageBox.Show("Nothing selected");
            }

        }

    }
}
