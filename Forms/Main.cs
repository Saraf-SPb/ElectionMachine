using ElectionMachine.Core;
using System;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Main : Form
    {
        private BindingSource BindingSource = new BindingSource();
        public Main()
        {
            InitializeComponent();
            BindingSource.DataSource = DBHelper.GetAllElectorateList();
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
                DBHelper.AddElectorat(tbFIO.Text, tbPhone.Text), 
                "Добавление", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
            tbFIO.Text = "";
            tbPhone.Text = "";

            BindingSource.DataSource = DBHelper.GetAllElectorateList();
            BindingSource.ResetBindings(false);
        }
    }
}
