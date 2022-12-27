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


namespace Salary_DataBase
{
    public partial class sign_up : Form
    {
        DataBase dataBase = new DataBase();
       
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            

            var login = textBox_login2.Text;
            var password = textBox_password2.Text;
            var id = textBox1.Text;

            string querystring = $"insert into Register (login_user, password_user, is_admin, id) values ('{login}', '{password}', 0, '{id}')";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if (checkuser())
            {
                { MessageBox.Show("Аккаунт не создан!", "Пользователь уже существует!"); }
                dataBase.closeConnection();
            }
            if (checkuserId())
            {
                { MessageBox.Show("Аккаунт не создан!", "id зарегистрирован!"); }
                dataBase.closeConnection();
            }
            
            else
            {
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Аккаунт успешно создан!", "Успешно!");
                    log_in frm_login = new log_in();
                    this.Hide();
                    frm_login.ShowDialog();
                }
                else
                { MessageBox.Show("Аккаунт не создан!"); }
                dataBase.closeConnection();
            }
        }

        private Boolean checkuser()
        {
            var loginUser = textBox_login2.Text;
            var passUser = textBox_password2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();  
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, password_user, is_admin, id from Register where login_user = '{loginUser}' and password_user = '{passUser}'";
        
            SqlCommand command = new SqlCommand(querystring,dataBase.getConnection());
            adapter.SelectCommand= command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            { return false; }
        }

        private Boolean checkuserId()
        {
            var loginUser = textBox_login2.Text;
            var passUser = textBox_password2.Text;
            var id_num = textBox1.Text;
            if (textBox_login2.Text != "admin" && textBox_password2.Text != "admin" && textBox1.Text == "")
            {
                MessageBox.Show("Поле id пустое!");
                return false;
            }
            else {
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                string querystring = $"select id_user, login_user, password_user, is_admin, id from Register where id = '{id_num}'";

                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    return true;
                }
                else
                { return false; }
            }
            
        }

        private void sign_up_Load(object sender, EventArgs e)
        {
            textBox_password2.PasswordChar = '●';
            pictureBox_closed.Visible = true;
            textBox_login2.MaxLength = 50;
            textBox_password2.MaxLength = 50;
        }

        private void pictureBox_closed_Click(object sender, EventArgs e)
        {
            textBox_password2.UseSystemPasswordChar = true;
            pictureBox_open.Visible = true;
            pictureBox_closed.Visible = false;
        }

        private void pictureBox_open_Click(object sender, EventArgs e)
        {
            textBox_password2.UseSystemPasswordChar = false;
            pictureBox_open.Visible = false;
            pictureBox_closed.Visible = true;
        }
    }
}
