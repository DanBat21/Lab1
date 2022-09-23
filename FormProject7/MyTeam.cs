using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FormProject7
{
    class MyTeam
    {
        private String name_team;// поля класса
        private Int16 scored_goals;

        public MyTeam(String name_team, Int16 scored_goals) // конструктор
        {
            this.name_team = name_team;
            this.scored_goals = scored_goals;
        }

        public String Name_team
        {
            get { return name_team; }
        }

        public Int16 Scored_goals
        {
            get { return scored_goals; }
            set { scored_goals = value; }
        }

        public static List<string> PlayingTeam(List<MyTeam> Z1, List<MyTeam> Z2)
        {
            List<string> teamname = new List<string>(7);
            for (int i = 0; i < Z1.Count; i++)
            {
                if (!teamname.Contains(Z1[i].name_team))
                    teamname.Add(Z1[i].name_team);
                if (!teamname.Contains(Z2[i].name_team))
                    teamname.Add(Z2[i].name_team);
            }
            return teamname;
        }

        public static int[] CreateResult(List<MyTeam> Z1, List<MyTeam> Z2, ComboBox cm1)
        {
            int[] points = new int[cm1.Items.Count];
            for (int j = 0; j < cm1.Items.Count; j++)
            {
                for (int i = 0; i < Z1.Count; i++)
                {
                    if (Z1[i].name_team == cm1.Items[j].ToString())
                    {
                        points[j] += Z1[i].scored_goals - Z2[i].scored_goals;
                    }
                    else if (Z2[i].name_team == cm1.Items[j].ToString())
                    {
                        points[j] += Z2[i].scored_goals - Z1[i].scored_goals;
                    }

                }

            }
            return points;
        }

        public static void Show(List<MyTeam> Z1, List<MyTeam> Z2, ref DataGridView dgv1)
        {
            dgv1.RowCount = Z1.Count;
            for (int i = 0; i < Z1.Count; i++)
            {
                dgv1.Rows[i].Cells[0].Value = Z1[i].name_team;
                dgv1.Rows[i].Cells[1].Value = Z2[i].name_team;
                dgv1.Rows[i].Cells[2].Value = Z1[i].scored_goals + ":" + Z2[i].scored_goals;

            }
        }

        public static void Show(ref DataGridView dgv2, int[] points, ComboBox cm1, List<string> list)
        {
            int match = 0;
            dgv2.Rows.Clear();
            dgv2.RowCount = list.Count;
            for (int i = 0; i < points.Length; i++)
            {
                if (list.Contains(cm1.Items[i].ToString()))
                {
                    dgv2.Rows[match].Cells[0].Value = cm1.Items[i].ToString();
                    dgv2.Rows[match].Cells[1].Value = points[i];
                    match++;
                }
            }
        }

        public static void SaveFile(List<MyTeam> Z1, List<MyTeam> Z2, string fname, string name, List<string> list)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fname, false, System.Text.Encoding.GetEncoding("utf-8"));
                sw.WriteLine(name); // Записывает в первую строку название турнира.
                sw.WriteLine(list.Count + "\n"); // Записывает количество команд
                
                for (int i = 0; i < Z1.Count; i++)
                {
                    sw.WriteLine(Z1[i].name_team);     // Записывает название команды1.                   
                    sw.WriteLine(Z1[i].scored_goals);   // Записывает количество забитых голов для команды1.
                    sw.WriteLine(Z2[i].name_team);    // Записывает название команды2.
                    sw.WriteLine(Z2[i].scored_goals);   // Записывает количество забитых голов для команды2.
                    sw.WriteLine();
                }
                sw.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public static List<List<MyTeam>> OpenFile(string fname, out string name)
        {
            try
            {
                StreamReader sr = new StreamReader(fname, System.Text.Encoding.GetEncoding("utf-8"));
                name = sr.ReadLine(); // Считывается первая строка с названием турнира.
                sr.ReadLine();
                sr.ReadLine();
                List<List<MyTeam>> teams = new List<List<MyTeam>>(2);
                List<MyTeam> team1 = new List<MyTeam>();
                List<MyTeam> team2 = new List<MyTeam>();
                while (!sr.EndOfStream)
                {
                    // Вносим построчно в список экземпляры класса
                    team1.Add(new MyTeam(sr.ReadLine().ToString(), Convert.ToInt16(sr.ReadLine())));
                    team2.Add(new MyTeam(sr.ReadLine().ToString(), Convert.ToInt16(sr.ReadLine())));
                    sr.ReadLine();
                }
                teams.Add(team1);
                teams.Add(team2);
                sr.Close();
                return teams;
            }
                catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}





