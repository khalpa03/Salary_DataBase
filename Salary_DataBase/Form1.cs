using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using System.Reflection;


namespace Salary_DataBase
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class Form1 : Form
    {
        private readonly checkUser _user;

        DataBase dataBase = new DataBase();
        int selectedRow;
        string dep1 = "";
        string h = "";
        string tab = "";
        string ti = "";
        string tar = "";
        string textBox_per_num = "";
        string textBox_timenum = "";
        string textBox_id = "";
        public Form1(checkUser user)
        {
            _user = user;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void IsAdmin()
        {
            if (!_user.IsAdmin)
            {
                управлениеToolStripMenuItem.Visible = false;
                btnNew.Visible = false;
                btnSave.Visible = false;
                btn_delete.Visible = false;
                btnUpdate.Visible = false;
                btnNew_dep.Visible = false;
                btn_Change_dep.Visible = false;
                btn_delete_dep.Visible = false;
                btnUpdate_dep.Visible = false;
                btnSave_dep.Visible = false;
                button1.Visible = false;
                btn_Change_data.Visible = false;
                btn_delete_data.Visible = false;
                button3.Visible = false;
                btnSave_data.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button7.Visible = false;
                button6.Visible = false;
                button5.Visible = false;
                button14.Visible = false;
                button13.Visible = false;
                button10.Visible = false;
                button11.Visible = false;
                button12.Visible = false;
            }
            comboBox_id.Enabled = _user.IsAdmin;
            comboBox_depart.Enabled = _user.IsAdmin;
            textBox_search.Enabled = _user.IsAdmin;
            textBox_search_tar.Enabled = _user.IsAdmin;
            textBox_search_tab.Enabled = _user.IsAdmin;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Personnel_Number", "Tабельный номер");
            dataGridView1.Columns.Add("Full_name", "ФИО");
            dataGridView1.Columns.Add("id_tariff_scale", "id Тарифной сетки");
            dataGridView1.Columns.Add("Department_name", "Отдел");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt64(0), record.GetString(1), record.GetInt64(2), record.GetString(3), RowState.ModifiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            if (_user.IsAdmin) {
                dgw.Rows.Clear();
                string queryString = $"select * from Employee";
                SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

                dataBase.openConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRow(dgw, reader);
                }
                reader.Close();
            } else {
                Searchuser(dgw);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox1.Text = $"{_user.Login}: {_user.Status}";
            IsAdmin();

            log_in log_In = new log_in();

            textBox_id_user.Text = log_in.id_num.ToString();

            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet2.TariffScale". При необходимости она может быть перемещена или удалена.
            this.tariffScaleTableAdapter1.Fill(this.salaryDataSet2.TariffScale);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet3.Timesheet". При необходимости она может быть перемещена или удалена.
            this.timesheetTableAdapter1.Fill(this.salaryDataSet3.Timesheet);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet21.Timesheet". При необходимости она может быть перемещена или удалена.
            this.timesheetTableAdapter.Fill(this.salaryDataSet21.Timesheet);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet2.Employee". При необходимости она может быть перемещена или удалена.
            this.employeeTableAdapter.Fill(this.salaryDataSet2.Employee);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet2.Timesheet". При необходимости она может быть перемещена или удалена.
            this.timesheetTableAdapter.Fill(this.salaryDataSet2.Timesheet);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet2.Department". При необходимости она может быть перемещена или удалена.
            this.departmentTableAdapter1.Fill(this.salaryDataSet2.Department);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet2.Employee". При необходимости она может быть перемещена или удалена.
            this.employeeTableAdapter.Fill(this.salaryDataSet2.Employee);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet1.TariffScale". При необходимости она может быть перемещена или удалена.
            this.tariffScaleTableAdapter.Fill(this.salaryDataSet1.TariffScale);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "salaryDataSet.Department". При необходимости она может быть перемещена или удалена.
            this.departmentTableAdapter.Fill(this.salaryDataSet.Department);

            CreateColumns();
            CreateColumnsDep();
            CreateColumns_Data();
            CreateColumnsTar();
            CreateColumnsTab();
            RefreshDataGrid(dataGridView1);
            RefreshDataGridDep(dataGridView2);
            RefreshDataGrid_Data(dataGridView3);
            RefreshDataGridTar(dataGridView4);
            RefreshDataGridTab(dataGridView5);
            dataGridView1.Columns["IsNew"].Visible = false;
            dataGridView2.Columns["IsNew"].Visible = false;
            dataGridView3.Columns["IsNew"].Visible = false;
            dataGridView4.Columns["IsNew"].Visible = false;
            dataGridView5.Columns["IsNew"].Visible = false;
        }

        private void Searchuser(DataGridView dgw)
        {
                dgw.Rows.Clear();

            log_in log_In = new log_in();
            int id_user = log_in.id_num;

                var filtr = comboBox_filtr.Text;
                string searchString = $"select * from Employee where Personnel_Number = '{id_user}'";

                SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    ReadSingleRow(dgw, read);
                }
                read.Close();
        }

        private void SearchuserTime(DataGridView dgw)
        {
            dgw.Rows.Clear();

            log_in log_In = new log_in();
            int id_user = log_in.id_num;

            var filtr = comboBox_filtr.Text;

            string searchString = $"select * from TimesheetEmployees where Table_number = '{id_user}'";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRowTab(dgw, read);
            }
            read.Close();
        }

        private void SearchuserTar(DataGridView dgw)
        {
            dgw.Rows.Clear();

            log_in log_In = new log_in();
            int id_user = log_in.id_num;

            var filtr = comboBox_filtr.Text;

            string searchString = $"EXEC procedure_tar {id_user}";
            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRowTar(dgw, read);
            }
            read.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_per_num = row.Cells[0].Value.ToString();
                textBox_fio.Text = row.Cells[1].Value.ToString();
                comboBox_id.Text = row.Cells[2].Value.ToString();
                comboBox_depart.Text = row.Cells[3].Value.ToString();

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
            this.departmentTableAdapter.Fill(this.salaryDataSet.Department);
            this.tariffScaleTableAdapter.Fill(this.salaryDataSet1.TariffScale);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var fio = textBox_fio.Text;
            var id = comboBox_id.Text;
            var department = comboBox_depart.Text;

            var addQuery = $"insert into Employee (Full_name, id_tariff_scale, Department_name) values ('{fio}', '{id}', '{department}')";
            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запсиь успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataBase.closeConnection();
            RefreshDataGrid(dataGridView1);
            update();


        }

        private void Search(DataGridView dgw)
        {
            try
            {
            dgw.Rows.Clear();
            var filtr = comboBox_filtr.Text;
            string searchString = $"select * from Employee where {filtr} like '%" + textBox_search.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
            

            }
            catch (SqlException)
            {
                if (comboBox_filtr.Text == "")
                {
                    MessageBox.Show("Выберите фильтр, перед тем как искать!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                comboBox_filtr.Text = comboBox_filtr.Items[0].ToString();
                textBox_search.Text = "";
            }


        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);

        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
        }

        private void update()
        {
            dataBase.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var id1 = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery1 = $"EXEC procedure_emp_delete {id1}";
                    var command1 = new SqlCommand(deleteQuery1, dataBase.getConnection());
                    command1.ExecuteNonQuery();
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Employee where Personnel_Number = {id}";
                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var per_num = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var fio = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var id = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var depart = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    var changeQuery = $"update Employee set Full_name = '{fio}', id_tariff_scale = '{id}', Department_name = '{depart}' where Personnel_Number = '{per_num}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }

            }
            dataBase.closeConnection();

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {

            deleteRow();
            update();
            ClearFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            update();
            RefreshDataGrid(dataGridView1);
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var per_num = textBox_per_num;
            var fio = textBox_fio.Text;
            var id = comboBox_id.Text;
            var depart = comboBox_depart.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(per_num, fio, id, depart);
                dataGridView1.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
            }

        }

        private void btn_Change_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
            update();
            RefreshDataGrid(dataGridView1);
        }

        private void ClearFields()
        {
            //textBox_per_num.Text = "";
            textBox_fio.Text = "";
            comboBox_id.Text = "";
            comboBox_depart.Text = "";
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        //ТАБЛИЦА DEPARTMENT

        private void CreateColumnsDep()
        {
            dataGridView2.Columns.Add("Department_name", "Отдел");
            dataGridView2.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRowDep(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), RowState.ModifiedNew);
        }
        private void RefreshDataGridDep(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Department";
            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRowDep(dgw, reader);
            }
            reader.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];

                textBox_dep.Text = row.Cells[0].Value.ToString();
                dep1 = row.Cells[0].Value.ToString();

            }
        }

        private void btnUpdate_Click_dep(object sender, EventArgs e)
        {
            RefreshDataGridDep(dataGridView2);
            ClearFieldsDep();
        }

        private void btnNew_Click_dep(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var dep = textBox_dep.Text;

            var addQuery = $"insert into Department (Department_name) values ('{dep}')";
            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запсиь успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataBase.closeConnection();
            updateDep();
            RefreshDataGridDep(dataGridView2);

        }

        private void SearchDep(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from Department where Department_name like '%" + textBox_search_dep.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRowDep(dgw, read);
            }
            read.Close();
        }

        private void textBox_search_dep_TextChanged(object sender, EventArgs e)
        {
            SearchDep(dataGridView2);

        }

        private void deleteRowDep()
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.Rows[index].Visible = false;

            if (dataGridView2.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index].Cells[1].Value = RowState.Deleted;
                return;
            }
            dataGridView2.Rows[index].Cells[1].Value = RowState.Deleted;
        }

        private void updateDep()
        {
            dataBase.openConnection();
            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[1].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var dep_name1 = Convert.ToString(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"EXEC procedure_Dep_delete '{dep_name1}'";
                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                    var dep_name = Convert.ToString(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery1 = $"delete from Department where Department_name = '{dep_name}'";
                    var command1 = new SqlCommand(deleteQuery1, dataBase.getConnection());
                    command1.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var depart1 = Convert.ToString(dataGridView2.Rows[index].Cells[0].Value);
                    var changeQuery = $"EXEC procedure_dep_UPDATE '{depart1}', '{dep1}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                    var depart = Convert.ToString(dataGridView2.Rows[index].Cells[0].Value);

                    var changeQuery1 = $"update Department set Department_name = '{depart}' where Department_name = '{dep1}'";

                    var command1 = new SqlCommand(changeQuery1, dataBase.getConnection());
                    command1.ExecuteNonQuery();

                }

            }
            dataBase.closeConnection();

        }

        private void btn_delete_dep_Click(object sender, EventArgs e)
        {

            deleteRowDep();
            ClearFieldsDep();
            updateDep();
            RefreshDataGridDep(dataGridView2);
        }

        private void btnSave_dep_Click(object sender, EventArgs e)
        {
            updateDep();
            RefreshDataGridDep(dataGridView2);
        }

        private void Change_dep()
        {
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;
            var dep = textBox_dep.Text;

            if (dataGridView2.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView2.Rows[selectedRowIndex].Cells[1].Value = RowState.Modified;
            }
            dataGridView2.Rows[selectedRowIndex].SetValues(dep);
            dataGridView2.Rows[selectedRowIndex].Cells[1].Value = RowState.Modified;
        }

        private void btn_Change_dep_Click(object sender, EventArgs e)
        {
            Change_dep();
            ClearFieldsDep();
            updateDep();
            RefreshDataGridDep(dataGridView2);
        }

        private void ClearFieldsDep()
        {
            textBox_dep.Text = "";
        }

        private void btn_Clear_Click_dep(object sender, EventArgs e)
        {
            ClearFieldsDep();
        }
        ////////////////////////////////////////////////////////////////////////Timesheet

        private void CreateColumns_Data()
        {
            dataGridView3.Columns.Add("Timesheet_number", "Номер табеля");
            dataGridView3.Columns.Add("Data", "Дата");
            dataGridView3.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRow_Data(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt64(0), record.GetDateTime(1).ToLongDateString(), RowState.ModifiedNew);
        }
        private void RefreshDataGrid_Data(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Timesheet";
            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_Data(dgw, reader);
            }
            reader.Close();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedRow];

                textBox_timenum = row.Cells[0].Value.ToString();
                textBox_Data.Text = row.Cells[1].Value.ToString();

            }
        }

        private void btnUpdate_Click_Data(object sender, EventArgs e)
        {
            RefreshDataGrid_Data(dataGridView3);
            ClearFields_data();
        }

        private void btnNew_Click_Data(object sender, EventArgs e)
        {
            dataBase.openConnection();

            DateTime data = textBox_Data.Value;
            string data1 = Convert.ToString(data);
            string day = "";
            string month = "";
            string year = "";
            for (int i = 0; i < data1.Length; i++)
            {
                if (i == 3 || i == 4)
                {
                    day += data1[i];
                }
                if (i == 0 || i == 1)
                {
                    month += data1[i];
                }
                if (i == 6 || i == 7 || i == 8 || i == 9)
                {
                    year += data1[i];
                }
            }
            data1 = day + "-" + month + "-" + year;

            var addQuery = $"insert into Timesheet (Data) values ('{data1}')";
            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запсиь успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataBase.closeConnection();
            update_data();
            RefreshDataGrid_Data(dataGridView3);

        }

        private void Search_data(DataGridView dgw)
        {
            try
            {
                dgw.Rows.Clear();
                var filtr = comboBox_filtr_data.Text;
                string searchString = $"select * from Timesheet where {filtr} like '%" + textBox_search_data.Text + "%'";

                SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    ReadSingleRow_Data(dgw, read);
                }
                read.Close();
            }
            catch (SqlException)
            {
                if (comboBox_filtr_data.Text == "")
                {
                    MessageBox.Show("Выберите фильтр, перед тем как искать!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                comboBox_filtr_data.Text = comboBox_filtr_data.Items[0].ToString();
                textBox_search_data.Text = "";
            }
        }

        private void textBox_search_data_TextChanged(object sender, EventArgs e)
        {
            Search_data(dataGridView3);

        }

        private void deleteRow_data()
        {
            int index = dataGridView3.CurrentCell.RowIndex;

            dataGridView3.Rows[index].Visible = false;

            if (dataGridView3.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView3.Rows[index].Cells[2].Value = RowState.Deleted;
                return;
            }
            dataGridView3.Rows[index].Cells[2].Value = RowState.Deleted;
        }

        private void update_data()
        {
            dataBase.openConnection();
            for (int index = 0; index < dataGridView3.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView3.Rows[index].Cells[2].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var id1 = Convert.ToInt64(dataGridView3.Rows[index].Cells[0].Value);
                    var deleteQuery1 = $"EXEC procedure_Tab_delete {id1}";
                    var command1 = new SqlCommand(deleteQuery1, dataBase.getConnection());
                    command1.ExecuteNonQuery();
                    var id = Convert.ToInt64(dataGridView3.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Timesheet where Timesheet_number = {id}";
                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    string day = "";
                    string month = "";
                    string year = "";
                    var id = dataGridView3.Rows[index].Cells[0].Value.ToString();
                    var data = dataGridView3.Rows[index].Cells[1].Value.ToString();
                    for (int i = 0; i <data.Length; i++)
                    {
                        if (i==3 || i==4)
                        {
                            day += data[i];
                        }
                        if (i == 0 || i == 1)
                        {
                            month += data[i];
                        }
                        if (i == 6 || i == 7 || i == 8 || i == 9)
                        {
                            year += data[i];
                        }
                    }
                    string data1 = day + "-" + month + "-" + year;
                    var changeQuery = $"update Timesheet set Data = '{data1}' where Timesheet_number = '{id}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }

            }
            dataBase.closeConnection();

        }

        private void btn_delete_data_Click(object sender, EventArgs e)
        {

            deleteRow_data();
            update_data();
            ClearFields_data();
        }

        private void btnSave_data_Click(object sender, EventArgs e)
        {
            update_data();
            RefreshDataGrid_Data(dataGridView3);
        }

        private void Change_data()
        {
            var selectedRowIndex = dataGridView3.CurrentCell.RowIndex;
            var id = textBox_timenum;
            DateTime data = textBox_Data.Value;

            if (dataGridView3.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView3.Rows[selectedRowIndex].SetValues(id, data);
                dataGridView3.Rows[selectedRowIndex].Cells[2].Value = RowState.Modified;
            }
            dataGridView3.Rows[selectedRowIndex].SetValues(id, data);
            dataGridView3.Rows[selectedRowIndex].Cells[2].Value = RowState.Modified;

        }

        private void btn_Change_data_Click(object sender, EventArgs e)
        {
            Change_data();
            ClearFields_data();
            update_data();
            RefreshDataGrid_Data(dataGridView3);
        }

        private void ClearFields_data()
        {
            textBox_Data.Text = "";
        }

        private void btn_Clear_data_Click(object sender, EventArgs e)
        {
            ClearFields_data();
        }
        /////////////////////////////////////////////////////////////////////TariffScale
        private void CreateColumnsTar()
        {
            dataGridView4.Columns.Add("Post", "Должность");
            dataGridView4.Columns.Add("Rank", "Разряд");
            dataGridView4.Columns.Add("Rate", "Ставка");
            dataGridView4.Columns.Add("Tariff_scale_id", "id");
            dataGridView4.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRowTar(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), record.GetInt32(1), record.GetDouble(2), record.GetInt64(3), RowState.ModifiedNew);
        }
        private void RefreshDataGridTar(DataGridView dgw)
        {

            if (_user.IsAdmin)
            {
                dgw.Rows.Clear();
                string queryString = $"select * from TariffScale";
                SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

                dataBase.openConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRowTar(dgw, reader);
                }
                reader.Close();
            }
            else
            {
                SearchuserTar(dgw);
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView4.Rows[selectedRow];

                textBox_id = row.Cells[3].Value.ToString();
                textBox_Post.Text = row.Cells[0].Value.ToString();
                comboBox_Rank.Text = row.Cells[1].Value.ToString();
                textBox_Rate.Text = row.Cells[2].Value.ToString();

            }
        }

        private void btnUpdateTar_Click(object sender, EventArgs e)
        {
            RefreshDataGridTar(dataGridView4);
            ClearFieldsTar();
        }

        private void btnNewTar_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var post = textBox_Post.Text;
            var rank = comboBox_Rank.Text;
            var rate = textBox_Rate.Text;
            float rate1 = float.Parse(rate);
            if (rate1 > 1 && rate1 < 9)
            {
                var addQuery = $"insert into TariffScale (Post, Rank, Rate) values ('{post}', '{rank}', '{rate}')";
                var command = new SqlCommand(addQuery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запсиь успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataBase.closeConnection();
                updateTar();
                RefreshDataGridTar(dataGridView4);
            }
            else
            {
                MessageBox.Show("Ставка должна быть больше 1 и меньше 9!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void SearchTar(DataGridView dgw)
        {
            try
            {
                dgw.Rows.Clear();
                var filtr = comboBox_filtr_tar.Text;
                string searchString = $"select * from TariffScale where {filtr} like '%" + textBox_search_tar.Text + "%'";

                SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    ReadSingleRowTar(dgw, read);
                }
                read.Close();
            }
            catch (SqlException)
            {
                if (comboBox_filtr_tar.Text == "")
                {
                    MessageBox.Show("Выберите фильтр, перед тем как искать!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                comboBox_filtr_tar.Text = comboBox_filtr_tar.Items[0].ToString();
                textBox_search_tar.Text = "";
            }
        }

        private void textBox_searchTar_TextChanged(object sender, EventArgs e)
        {
            SearchTar(dataGridView4);

        }

        private void deleteRowTar()
        {
            int index = dataGridView4.CurrentCell.RowIndex;

            dataGridView4.Rows[index].Visible = false;

            if (dataGridView4.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView4.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView4.Rows[index].Cells[4].Value = RowState.Deleted;
        }

        private void updateTar()
        {
            dataBase.openConnection();
            for (int index = 0; index < dataGridView4.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView4.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var id1 = Convert.ToInt64(dataGridView4.Rows[index].Cells[3].Value);
                    var deleteQuery1 = $"EXEC procedure_Tar_delete {id1}";
                    var command1 = new SqlCommand(deleteQuery1, dataBase.getConnection());
                    command1.ExecuteNonQuery();
                    var id = Convert.ToInt64(dataGridView4.Rows[index].Cells[3].Value);
                    var deleteQuery = $"delete from TariffScale where Tariff_scale_id = {id}";
                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var post = dataGridView4.Rows[index].Cells[1].Value.ToString();
                    var rank = dataGridView4.Rows[index].Cells[2].Value.ToString();
                    var rate = dataGridView4.Rows[index].Cells[3].Value.ToString();
                    var id = dataGridView4.Rows[index].Cells[0].Value.ToString();

                    var changeQuery = $"update TariffScale set Post = '{post}', Rank = '{rank}', Rate = '{rate}' where Tariff_scale_id = '{id}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }

            }
            dataBase.closeConnection();

        }

        private void btn_deleteTar_Click(object sender, EventArgs e)
        {

            deleteRowTar();
            updateTar();
            ClearFieldsTar();
        }

        private void btnSaveTar_Click(object sender, EventArgs e)
        {
            updateTar();
            RefreshDataGridTar(dataGridView4);
        }

        private void ChangeTar()
        {
            var selectedRowIndex = dataGridView4.CurrentCell.RowIndex;
            var post = textBox_Post.Text;
            var rank = comboBox_Rank.Text;
            var id = textBox_id;
            var rate = textBox_Rate.Text;
            float rate1 = float.Parse(rate);
            if (rate1 > 1 && rate1 < 9)
            {
                if (dataGridView4.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
                {
                    dataGridView4.Rows[selectedRowIndex].SetValues(id, post, rank, rate);
                    dataGridView4.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
                }
            }
            else
            {
                MessageBox.Show("Ставка должна быть больше 1 и меньше 9!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btn_ChangeTar_Click(object sender, EventArgs e)
        {
            ChangeTar();
            ClearFieldsTar();
            updateTar();
            RefreshDataGridTar(dataGridView4);
        }

        private void ClearFieldsTar()
        {
            textBox_Post.Text = "";
            comboBox_Rank.Text = "";
            textBox_Rate.Text = "";
        }

        private void btn_ClearTar_Click(object sender, EventArgs e)
        {
            ClearFieldsTar();
        }
        /////////////////////////////////////////////////////////////////TimesheetEmployees//////////////////////////////////////
        ///
        private void CreateColumnsTab()
        {
            dataGridView5.Columns.Add("Hours", "Часы");
            dataGridView5.Columns.Add("Table_number", "Номер сотрудника");
            dataGridView5.Columns.Add("Timesheet_number", "Номер даты");
            dataGridView5.Columns.Add("Tariff_scale_id", "Номер тариф. сетки");
            dataGridView5.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRowTab(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetInt64(1), record.GetInt64(2), record.GetInt64(3), RowState.ModifiedNew);
        }
        private void RefreshDataGridTab(DataGridView dgw)
        {
            if (_user.IsAdmin) {
                dgw.Rows.Clear();
                string queryString = $"select * from TimesheetEmployees";
                SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

                dataBase.openConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRowTab(dgw, reader);
                }
                reader.Close();
            } else {
                SearchuserTime(dgw);
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView5.Rows[selectedRow];

                textBox_hours.Text = row.Cells[0].Value.ToString();
                textBox_emp.Text = row.Cells[1].Value.ToString();
                textBox_numdata.Text = row.Cells[2].Value.ToString();
                textBox_tariff.Text = row.Cells[3].Value.ToString();
                h = row.Cells[0].Value.ToString();
                tab = row.Cells[1].Value.ToString();
                ti = row.Cells[2].Value.ToString();
                tar = row.Cells[3].Value.ToString();

            }
        }

        private void btnUpdateTab_Click(object sender, EventArgs e)
        {
            RefreshDataGridTab(dataGridView5);
            ClearFieldsTab();
            this.timesheetTableAdapter.Fill(this.salaryDataSet2.Timesheet);
            this.employeeTableAdapter.Fill(this.salaryDataSet2.Employee);
            this.tariffScaleTableAdapter1.Fill(this.salaryDataSet2.TariffScale);

        }

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var hours = textBox_hours.Text;
            var tabnum = textBox_emp.Text;
            var time = textBox_numdata.Text;
            var tariff = textBox_tariff.Text;

            var addQuery = $"insert into TimesheetEmployees (Hours, Table_number, Timesheet_number, Tariff_scale_id) values ('{hours}', '{tabnum}', '{time}', '{tariff}')";
            var command = new SqlCommand(addQuery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запсиь успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataBase.closeConnection();
            updateTab();
            RefreshDataGridTab(dataGridView5);

        }

        private void SearchTab(DataGridView dgw)
        {
            try
            {
                dgw.Rows.Clear();
                var filtr = comboBox_filtr_tab.Text;
                string searchString = $"select * from TimesheetEmployees where {filtr} like '%" + textBox_search_tab.Text + "%'";

                SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    ReadSingleRowTab(dgw, read);
                }
                read.Close();
            }
            catch (SqlException)
            {
                if (comboBox_filtr_tab.Text == "")
                {
                    MessageBox.Show("Выберите фильтр, перед тем как искать!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                comboBox_filtr_tab.Text = comboBox_filtr_tab.Items[0].ToString();
                textBox_search_tab.Text = "";
            }
        }

        private void textBox_searchTab_TextChanged(object sender, EventArgs e)
        {
            SearchTab(dataGridView5);

        }

        private void deleteRowTab()
        {
            int index = dataGridView5.CurrentCell.RowIndex;

            dataGridView5.Rows[index].Visible = false;

            if (dataGridView5.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView5.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView5.Rows[index].Cells[4].Value = RowState.Deleted;
        }

        private void updateTab()
        {
            dataBase.openConnection();
            for (int index = 0; index < dataGridView5.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView5.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var hours = Convert.ToInt32(dataGridView5.Rows[index].Cells[0].Value);
                    var tabnum = Convert.ToInt32(dataGridView5.Rows[index].Cells[1].Value);
                    var timenum = Convert.ToInt32(dataGridView5.Rows[index].Cells[2].Value);
                    var tari = Convert.ToInt32(dataGridView5.Rows[index].Cells[3].Value);
                    var deleteQuery = $"delete from TimesheetEmployees where Hours = {hours} and Table_number = {tabnum} and Timesheet_number = {timenum} and Tariff_scale_id = {tari}";
                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var hours = dataGridView5.Rows[index].Cells[0].Value.ToString();
                    var tabnum = dataGridView5.Rows[index].Cells[1].Value.ToString();
                    var timenum = dataGridView5.Rows[index].Cells[2].Value.ToString();
                    var tari = dataGridView5.Rows[index].Cells[3].Value.ToString();

                    var changeQuery = $"update TimesheetEmployees set Hours = '{hours}', Table_number = '{tabnum}', Timesheet_number = '{timenum}', Tariff_scale_id = '{tari}' where Hours = '{h}' and Table_number = '{tab}' and Timesheet_number = '{ti}' and Tariff_scale_id = '{tar}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }

            }
            dataBase.closeConnection();

        }

        private void btn_deleteTab_Click(object sender, EventArgs e)
        {

            deleteRowTab();
            updateTab();
            ClearFieldsTab();
        }

        private void btnSaveTab_Click(object sender, EventArgs e)
        {
            updateTab();
            RefreshDataGridTab(dataGridView5);
        }

        private void ChangeTab()
        {
            var selectedRowIndex = dataGridView5.CurrentCell.RowIndex;
            var hours = textBox_hours.Text;
            var tabnum = textBox_emp.Text;
            var timenum = textBox_numdata.Text;
            var tari = textBox_tariff.Text;

            if (dataGridView5.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView5.Rows[selectedRowIndex].SetValues(hours, tabnum, timenum, tari);
                dataGridView5.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
            }

        }

        private void btn_ChangeTab_Click(object sender, EventArgs e)
        {
            ChangeTab();
            ClearFieldsTab();
            updateTab();
            RefreshDataGridTab(dataGridView1);
        }

        private void ClearFieldsTab()
        {
            textBox_hours.Text = "";
            textBox_emp.Text = "";
            textBox_numdata.Text = "";
            textBox_tariff.Text = "";
        }

        private void btn_ClearTab_Click(object sender, EventArgs e)
        {
            ClearFieldsTab();
        }

        private void comboBox_Rank_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            { e.Handled = true; }
        }

        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel_Admin addfrm = new Panel_Admin();
            addfrm.Show();
        }
    }
    }