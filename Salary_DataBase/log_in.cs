using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Salary_DataBase
{
    public partial class log_in : Form
    {
        DataBase dataBase = new DataBase();
        public static int id_num;

        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void log_in_Load(object sender, EventArgs e) 
        {
            textBox_password.PasswordChar = '●';
            pictureBox_closed.Visible= true;
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength= 50; 


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (textBox_login.Text == "admin" && textBox_password.Text == "admin")
            {
                textBox1.Text = "0";
                textBox1.PasswordChar = '*';
                var loginUser = textBox_login.Text;
                var passUser = textBox_password.Text;

                int a = int.Parse(textBox1.Text);
                id_num = a;

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                string querystring = $"select id_user, login_user, password_user, is_admin, id from Register where login_user = '{loginUser}' and password_user ='{passUser}' and id = '{0}'";

                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                {
                    var user = new checkUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));


                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form frm1 = new Form1(user);
                    this.Hide();
                    frm1.ShowDialog();
                    this.Show();
                }
                else
                    MessageBox.Show("Taкого аккаунта не существует!", "Аккаунт не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                var loginUser = textBox_login.Text;
                var passUser = textBox_password.Text;

                if (textBox1.Text == "")
                {
                    MessageBox.Show("Заполните id!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    id_num = int.Parse(textBox1.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    string querystring = $"select id_user, login_user, password_user, is_admin, id from Register where login_user = '{loginUser}' and password_user ='{passUser}' and id = '{id_num}'";

                    SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    if (textBox_login.Text != "" && textBox_password.Text != "" && textBox1.Text != "")
                    {
                        if (table.Rows.Count == 1)
                        {
                            var user = new checkUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));


                            MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form frm1 = new Form1(user);
                            this.Hide();
                            frm1.ShowDialog();
                            this.Show();
                        }
                        else
                            MessageBox.Show("Taкого аккаунта не существует!", "Аккаунт не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else MessageBox.Show("Присутствуют пустые значения!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up frm_sign = new sign_up();
            frm_sign.Show();
            this.Hide();
        }

        private void pictureBox_open_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar= false;
            pictureBox_open.Visible= false;
            pictureBox_closed.Visible = true;
        }

        private void pictureBox_closed_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = true;
            pictureBox_open.Visible = true;
            pictureBox_closed.Visible = false;
        }
    }
}
