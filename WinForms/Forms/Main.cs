using ElectionMachine.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Main : Form
    {
        private BindingSource BindingSource = new BindingSource();
        public Main()
        {
            InitializeComponent();
            if (Program.GlobalUser.IsAdmin)
            {
                btUsers.Visible = true;
                tbExport.Visible = true;
            }
            var temp = Program.service.GetAllElectorateList();
            if (temp.Count() == 0)
                temp.Add(new ElectorateWithUserName());
            BindingSource.DataSource = temp.OrderByDescending(i => i.CreateDate);

            electoratDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            electoratDGV.DataSource = BindingSource;
            electoratDGV.Columns["Id"].Visible = false;
            electoratDGV.Columns["UserId"].Visible = false;
            electoratDGV.Columns["FIO"].HeaderText = "ФИО";
            electoratDGV.Columns["Phone"].HeaderText = "Телефон";
            electoratDGV.Columns["UserName"].HeaderText = "Оператор";
            electoratDGV.Columns["CreateDate"].HeaderText = "Дата добавления";
            electoratDGV.Columns["FIO"].DisplayIndex = 0;
            electoratDGV.Columns["Phone"].DisplayIndex = 1;
            electoratDGV.Columns["UserName"].DisplayIndex = 2;
            electoratDGV.Columns["CreateDate"].DisplayIndex = 3;

        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                Program.service.AddElectorat(tbFIO.Text, tbPhone.Text, Program.GlobalUser.Id),
                "Добавление",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            tbFIO.Text = "";
            tbPhone.Text = "";

            var temp = Program.service.GetAllElectorateList().OrderByDescending(i => i.CreateDate);
            BindingSource.DataSource = temp;

            electoratDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            electoratDGV.DataSource = BindingSource;
            electoratDGV.Columns["Id"].Visible = false;
            electoratDGV.Columns["UserId"].Visible = false;
            electoratDGV.Columns["FIO"].HeaderText = "ФИО";
            electoratDGV.Columns["Phone"].HeaderText = "Телефон";
            electoratDGV.Columns["UserName"].HeaderText = "Оператор";
            electoratDGV.Columns["CreateDate"].HeaderText = "Дата добавления";
            electoratDGV.Columns["FIO"].DisplayIndex = 0;
            electoratDGV.Columns["Phone"].DisplayIndex = 1;
            electoratDGV.Columns["UserName"].DisplayIndex = 2;
            electoratDGV.Columns["CreateDate"].DisplayIndex = 3;

            BindingSource.ResetBindings(false);
        }

        private void btUsers_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            this.Hide();
            users.ShowDialog();
            this.Show();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tbExport_Click(object sender, EventArgs e)
        {
            try
            {
                //before your loop
                var csv = new StringBuilder();

                System.Collections.Generic.List<ElectorateWithUserName> electorates = Program.service.GetAllElectorateList();

                foreach (var el in electorates)
                {
                    //in your loop
                    var fio = el.FIO;
                    var phone = el.Phone;
                    var username = el.UserName;
                    var createdate = el.CreateDate;
                    //Suggestion made by KyleMit
                    var newLine = string.Format($"{fio};{phone};{username};{createdate}");
                    csv.AppendLine(newLine);
                }
                File.WriteAllText(@"export.csv", csv.ToString(), Encoding.Unicode);
                MessageBox.Show("Экспорт успешно завершен. Файл export.csv лежит в папке с программой");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
