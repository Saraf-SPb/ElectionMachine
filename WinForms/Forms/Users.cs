using ElectionMachine.Core.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            var userList = Program.service.GetAllUsers();
            userList.Add(new User());
            UsersDGV.DataSource = userList;
            UsersDGV.Columns["Id"].Visible = false;
            UsersDGV.Columns["Name"].HeaderText = "Имя оператора";
            UsersDGV.Columns["Login"].HeaderText = "Логин";
            UsersDGV.Columns["Password"].HeaderText = "Пароль";
            UsersDGV.Columns["IsAdmin"].HeaderText = "Админ?";            
            UsersDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            List<User> users = new List<User>();
            foreach (DataGridViewRow row in UsersDGV.Rows)
            {
                if (row.Cells["Login"].Value != null)
                {                    
                    User temp = new User
                    {
                        Id = (int)row.Cells["Id"].Value,
                        Login = row.Cells["Login"].Value.ToString(),
                        Name = row.Cells["Name"].Value.ToString(),
                        Password = row.Cells["Password"].Value.ToString(),
                        IsAdmin = (bool)row.Cells["isAdmin"].Value
                    };

                    users.Add(temp);
                }
            }
            MessageBox.Show(Program.service.SaveUsers(users));
            
            var userList = Program.service.GetAllUsers();
            userList.Add(new User());
            UsersDGV.DataSource = userList;
        }
    }
}
