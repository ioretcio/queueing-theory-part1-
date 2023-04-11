using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace queueing_theory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //косметика нова ліза супер дєвочка
        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            task1grid.ColumnCount = System.Convert.ToInt32(numericUpDown1.Value);
            task1grid.RowCount = System.Convert.ToInt32(numericUpDown1.Value);
        }


        //зчитування даних з таблиці
        string getGridText(DataGridView dataGrid)
        {
            string result = "";
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                for (int j = 0; j < dataGrid.ColumnCount; j++)
                {
                    result +=Convert.toDouble(dataGrid[j, i].Value) + " ";
                }
                result += "\n";
            }
            return result;
        }



        void putDataToGrid(string data, DataGridView dataGrid)
        {
            string[] rows = data.Split('\n');
            dataGrid.RowCount = rows.Length - 1;
            bool f = true;
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                string[] columns = rows[i].Split(' ');
                if (f)
                {
                    dataGrid.ColumnCount = columns.Length - 1; f = false;
                }
                for (int j = 0; j < dataGrid.ColumnCount; j++)
                {
                    dataGrid[j, i].Value = columns[j];
                }
            }
        }


        //перевірка на повноту вхідних даних, сума ймовірностей по кожному вузлу має бути 1
        bool checkGrid()
        {
            bool message=false;
            for (int i = 0; i < task1grid.RowCount; i++)
            {
                double summ = 0;
                for (int j = 0; j < task1grid.ColumnCount; j++)
                {
                    summ +=Convert.toDouble(task1grid[j, i].Value.ToString().Replace(',', '.'));
                }
                if (Math.Abs(summ - 1) > 0.000000001)  //з урахуванням похибки обчислення
                {
                    for (int j = 0; j < task1grid.ColumnCount; j++)
                    {
                        task1grid.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Pink;
                    }
                    message = true;
                }
                else
                {
                    for (int j = 0; j < task1grid.ColumnCount; j++)
                    {
                        task1grid.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }
            if(message)
            MessageBox.Show("В матриці присутні не одиничні рядки");
            return !message;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (checkGrid())
            {
                List<double[]> answers = new List<double[]>();
                int k = (int)numericUpDown2.Value;
                int dimension = (int)task1grid.RowCount;

                double[,] matrix = new double[dimension, dimension];
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        matrix[j, i] = Convert.toDouble(task1grid[i, j].Value); //зчитування даних з матриці вхідної
                    }
                }
                answers.Add(new double[dimension]); // після першого кроку
                for (int i = 0; i < dimension; i++)
                {
                    answers[0][i] = matrix[0, i]; //на першій ітерації ймовірності станів дорівнюють вхідній матриці
                }
                for (int i = 1; i < k; i++) // к - кількість ітерацій
                {
                    answers.Add(new double[dimension]);
                    for (int j = 0; j < dimension; j++)
                    {
                        for (int l = 0; l < dimension; l++)
                        {
                            answers[i][j] += answers[i - 1][l] * matrix[l, j]; //просто перемножується дані попередніх відповідей та акумулюємо у стан даного вузла
                        }
                        answers[i][j] = Math.Round(answers[i][j], 5); //для виведення
                    }
                }
                richTextBox1.Text = "";
                for (int i = 0; i < k; i++)
                {
                    richTextBox1.Text += "Ймовірності після кроку " + (i).ToString() + ":\n";
                    for (int z = 0; z < dimension; z++)
                    {
                        richTextBox1.Text += $"P{z}({i})={ answers[i][z].ToString()}\n";
                    }
                    richTextBox1.Text += "\n";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = openFileDialog.OpenFile();
                string alldata = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    alldata = reader.ReadToEnd();
                    putDataToGrid(alldata, task1grid);
                }
            }
            numericUpDown1.Value = task1grid.RowCount;
            numericUpDown2.Value = task1grid.RowCount;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileContent = getGridText(task1grid);
            try
            {
                using (StreamWriter sw = new StreamWriter(DateTime.Now.ToString().Replace(':', ' ') + ".txt", false, System.Text.Encoding.Default))
                {
                    sw.Write(fileContent);
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);
            }
        }
    }
}