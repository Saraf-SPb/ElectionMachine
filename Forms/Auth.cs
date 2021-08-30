using ElectionMachine.Core;
using ElectionMachine.Core.DB;
using System;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Auth : Form
    {
        DBContext db;
        public Auth()
        {
            InitializeComponent();
            db = new DBContext();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Program.GlobalUser = DBHelper.Auth(tbLogin.Text, tbPassword.Text);
            if (Program.GlobalUser != null)
            {
                this.Hide();
                Main main = new Main();
                main.ShowDialog();
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Не верные логин или пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
