using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Main : Form
    {
        private readonly BindingSource BindingSource = new BindingSource();
        public Main()
        {
            InitializeComponent();
            if (Program.GlobalUser.IsAdmin)
            {
                btUsers.Visible = true;
                tbExport.Visible = true;
            }
            var tempElectorateList = Program.service.GetAllElectorateList();
            if (tempElectorateList.Count() == 0)
                tempElectorateList.Add(new ElectorateWithUserName());
            BindingSource.DataSource = tempElectorateList.OrderByDescending(i => i.CreateDate);

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

        private void BtAdd_Click(object sender, EventArgs e)
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

        private void BtUsers_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            Hide();
            users.ShowDialog();
            Show();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {

        }



        private void BtExport_Click(object sender, EventArgs e)
        {
            try
            {
                var csv = new StringBuilder();
                List<ElectorateWithUserName> electorates = Program.service.GetAllElectorateList();

                foreach (var el in electorates)
                {
                    var newLine = string.Format($"{el.FIO};{el.Phone};{el.UserName};{el.CreateDate}");
                    csv.AppendLine(newLine);
                }
                File.WriteAllText(@"export.csv", csv.ToString(), Encoding.Unicode);
                MessageBox.Show("Экспорт успешно завершен. Файл export.csv лежит в папке с программой");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
