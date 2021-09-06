using System;
using System.Windows.Forms;

namespace ElectionMachine.Forms
{
    public partial class Auth : Form
    {        
        public Auth()
        {
            InitializeComponent();            
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                var authResult = Program.service.Auth(tbLogin.Text, tbPassword.Text);
                Program.GlobalUser = authResult.Item1;
                //Program.GlobalUser = DBHelper.Auth(tbLogin.Text, tbPassword.Text);
                if (Program.GlobalUser != null)
                {                    
                    Main main = new Main();
                    this.Hide();
                    main.ShowDialog();
                    Application.Exit();
                }
                else
                {
                    if (authResult.Item2 == "OK")
                        MessageBox.Show("Не верные логин или пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show(authResult.Item2, "Ошибка доступа к БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
