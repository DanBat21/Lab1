using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FormProject7
{
    public partial class Form1 : Form
    {       
        List<MyTeam> team1 = new List<MyTeam>();
        List<MyTeam> team2 = new List<MyTeam>();
        
        int match_id = 0;
        Random rnd = new Random(Environment.TickCount);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "текст|*.txt";
            openFileDialog1.Title = "Открыть документ";
            openFileDialog1.Multiselect = false;

            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "текст|*.txt";
            saveFileDialog1.Title = "Сохранить документ";

            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 1;
            dataGridView1.Columns[0].Name = "Название команды1";
            dataGridView1.Columns[1].Name = "Название команды2";
            dataGridView1.Columns[2].Name = "Счет";

            dataGridView2.ColumnCount = 2;
            dataGridView2.RowCount = 1;
            dataGridView2.Columns[0].Name = "Название команды";
            dataGridView2.Columns[1].Name = "Очки";

            label2.Text = String.Empty;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (team1.Count != 0)
            {
                int[] res = MyTeam.CreateResult(team1, team2, comboBox2);

                if (tabControl1.SelectedIndex == 1)
                {
                    label2.Text = comboBox1.Text;
                    List<string> list = MyTeam.PlayingTeam(team1, team2);
                    MyTeam.Show(team1, team2, ref dataGridView1);      // Вызываем метод вывода данных в dataGrid.
                    MyTeam.Show(ref dataGridView2, res, comboBox2, list);
                }
                Diagram();
            }
            else
            {
                chart1.Visible = false;
                label2.Text = String.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            team1.Add(new MyTeam(comboBox2.Text.ToString(), Convert.ToInt16(numericUpDown1.Value)));
            team2.Add(new MyTeam(comboBox3.Text.ToString(), Convert.ToInt16(numericUpDown2.Value)));
                    
            match_id++;                     
        }


        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            team1.Clear();
            team2.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            label2.Text = String.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            button1.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label2.Text == String.Empty)
                MessageBox.Show("Заполните данные!");
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    List<string> list = MyTeam.PlayingTeam(team1, team2);

                    MyTeam.SaveFile(team1, team2, saveFileDialog1.FileName, label2.Text, list);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<List<MyTeam>> list = MyTeam.OpenFile(openFileDialog1.FileName, out string name);
                label2.Text = name;
                comboBox1.Text = name;
                team1.Clear();
                team2.Clear();
                team1 = list[0];
                team2 = list[1];

                int[] res = MyTeam.CreateResult(team1, team2, comboBox2);
                List<string> list1 = MyTeam.PlayingTeam(team1, team2);
                MyTeam.Show(team1, team2, ref dataGridView1);      // Вызываем метод вывода данных в dataGrid.
                MyTeam.Show(ref dataGridView2, res, comboBox2, list1);
            }           
        }

        private void Diagram()// метод рисует столбчатую диаграмму
        {
            chart1.Series.Clear();
            chart1.Visible = true;
            // Форматировать диаграмму
            chart1.BackColor = Color.Gray;
            chart1.BackSecondaryColor = Color.WhiteSmoke;
            chart1.BackGradientStyle = GradientStyle.DiagonalRight;

            chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            chart1.BorderlineColor = Color.Gray;
            chart1.BorderSkin.SkinStyle = BorderSkinStyle.None;

            // Форматировать область диаграммы
            chart1.ChartAreas[0].BackColor = Color.Wheat;
            
            // Добавить и форматировать заголовок
            chart1.Titles.Clear();
            
            chart1.Titles.Add("Турнир " + label2.Text.ToString());
            chart1.Titles[0].Font = new Font("Utopia", 16);

            chart1.Series.Add(new Series("ColumnSeries") { ChartType = SeriesChartType.Column });
           
            string[] xValues = new string[dataGridView2.RowCount];
            double[] yValues = new double[dataGridView2.RowCount];
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                xValues[i] = dataGridView2.Rows[i].Cells[0].Value.ToString();
                yValues[i] = Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value);
            }
                    
            chart1.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);
            chart1.Series["ColumnSeries"].Points[0].Color = Color.Red;
            chart1.Series["ColumnSeries"].Points[1].Color = Color.Green;

            chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
        }
    }   
}
