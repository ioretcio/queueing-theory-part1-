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
        //косметика
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

        private void startCalculation(object sender, EventArgs e)
        {
            if (checkGrid())
            {
                List<double[]> p_small = new List<double[]>();
                int countOfEtaps = (int)numericUpDown2.Value;
                int n = (int)task1grid.RowCount;

                double[,] P_big = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        P_big[j, i] = Convert.toDouble(task1grid[i, j].Value); //зчитування даних з матриці вхідної
                    }
                }
                p_small.Add(new double[n]); // після першого кроку
                for (int i = 0; i < n; i++)
                {
                    p_small[0][i] = P_big[0, i]; //на першій ітерації ймовірності станів дорівнюють вхідній матриці
                }
                for (int i = 1; i < countOfEtaps; i++) // кількість ітерацій
                {





                    p_small.Add(new double[n]); // спочатку всі нулі
                    for (int j = 0; j < n; j++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            p_small[i][j] += p_small[i - 1][l] * P_big[l, j]; //просто перемножується дані попередніх відповідей та вхідну матрицю ймовірностей акумулюємо у стан даного вузла
                        }
                        p_small[i][j] = Math.Round(p_small[i][j], 5); //для виведення
                    }




                }
                //тут уже пораховані всі ймовірності
                richTextBox1.Text = "";
                for (int i = 0; i < countOfEtaps; i++)
                {
                    richTextBox1.Text += "Ймовірності після кроку " + (i).ToString() + ":\n";
                    for (int z = 0; z < n; z++)
                    {
                        richTextBox1.Text += $"P{z}({i})={ p_small[i][z].ToString()}\n";
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
                string alldata;
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