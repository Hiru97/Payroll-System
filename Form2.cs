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

namespace Payroll
{
    public partial class salary : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BQDEANC\SQLEXPRESS;Initial Catalog=db_payroll;Integrated Security=True");
        public salary()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.MinimumSize = new Size(800, 600);
        }

        private decimal CalculateBasePay(decimal monthlysalary, decimal allowances, decimal overtimerate, decimal overtimehours)
        {
            return monthlysalary + allowances + (overtimerate * overtimehours);
        }

        private decimal CalculateNoPay(decimal monthlysalary, int leavedays, DateTime startdate, DateTime enddate, int salaryCycleDays)
        {


            return (monthlysalary / salaryCycleDays) * leavedays;

        }

        private decimal CalculateGrossPay(decimal basepay, decimal nopay, decimal taxrate)
        {
            decimal taxamount = (basepay * taxrate) / 100;
            decimal grosspay = basepay - nopay - taxamount;

            return grosspay < 0 ? 0 : grosspay;
        }


       

        public void Clear()
        {
            txtempid.ResetText();
            txtallowance.Text = "";
            txtmonthlysalary.Text = "";
            txtovertimerate.Text = "";
            txtovertimehours.Text = "";
            startdate.Value = DateTime.Now;
            enddate.Value = DateTime.Now;
            txtleavedays.Text = "";
            txttaxrate.Text = "";
            txtnopay.Text = "";
            txtbasepay.Text = "";
            txtgrosspay.Text = "";
        }

        private void GetEmployee()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[emp_salary]", con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("empid", typeof(int));
            dt.Load(Rdr);

            txtempid.DataBindings.Clear();
            txtempid.DataBindings.Add("Text", dt, "empid");

            con.Close();
        }

        private void LoadSettings()
        {
            try
            {
                con.Open();
                string SelectSql = "SELECT * FROM Settings";
                using (SqlCommand command = new SqlCommand(SelectSql, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            startdate.Value = (DateTime)reader["StartDate"];
                            enddate.Value = (DateTime)reader["EndDate"];
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Load_Emp_Data()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT empid, monthlysalary, overtimehours, allowance FROM [dbo].[emp_salary] WHERE empid = '" + txtempid.Text + "'", con);
                cmd.Parameters.AddWithValue("@empid", txtempid.Text);
                SqlDataReader dr = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void cmbEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadEmployee();
        }

        public static float CalculateNoPay(float Monthly_Salary, DateTime Start_Date, DateTime End_Date, int Leaves)
        {
            int Worked_Days = (End_Date - Start_Date).Days; float No_Pay_Value = (Monthly_Salary / Worked_Days) * Leaves; return No_Pay_Value;
        }

        public static float CalculateBasePay(float MonthlySalary, float Allowances, float OverTimeHours, float OverTimeRate)
        {
            float Base_Pay_Value = MonthlySalary + Allowances + (OverTimeHours * OverTimeRate); return Base_Pay_Value;
        }

        public static float CalculateGrossPay(float Base_Pay_Value, float No_Pay_Value, float GovernmentTaxRate)
        {
            float Gross_Pay_Value = Base_Pay_Value - (No_Pay_Value + Base_Pay_Value * GovernmentTaxRate); return Gross_Pay_Value;
        }

        private void loadEmployee()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT empid, monthlysalary, overtimehours, allowance FROM [dbo].[emp_salary] WHERE empid = '" + txtempid.Text + "'", con);
                cmd.Parameters.AddWithValue("@empid", txtempid.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtovertimerate.Text = dr.GetValue(2).ToString();
                    txtallowance.Text = dr.GetValue(3).ToString();
                }
                else
                {
                    MessageBox.Show("Employee Not Found");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void picsearch_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[emp_salary] WHERE empid = @empid", con);
                cmd.Parameters.AddWithValue("@empid", int.Parse(txtempid.Text));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    startdate.Value = Convert.ToDateTime(reader["startdate"]);
                    enddate.Value = Convert.ToDateTime(reader["enddate"]);
                    txtmonthlysalary.Text = reader["monthlysalary"].ToString();
                    txtovertimerate.Text = reader["overtimerate"].ToString();
                    txtovertimehours.Text = reader["overtimehours"].ToString();
                    txtallowance.Text = reader["allowance"].ToString();
                    txtleavedays.Text = reader["leavedays"].ToString();
                    txttaxrate.Text = reader["taxrate"].ToString();
                    txtnopay.Text = reader["nopay"].ToString();
                    txtbasepay.Text = reader["basepay"].ToString();
                    txtgrosspay.Text = reader["gross"].ToString();
                }
                else
                {
                    MessageBox.Show("No record found for the given Employee ID.", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btncal_Click(object sender, EventArgs e)
        {
            try
            {
                int employeeID = int.Parse(txtempid.Text);
                DateTime startDate = startdate.Value;
                DateTime endDate = enddate.Value;
                decimal monthlySalary = decimal.Parse(txtmonthlysalary.Text);
                decimal allowances = decimal.Parse(txtallowance.Text);
                int leavedays = int.Parse(txtleavedays.Text);
                decimal overtimeRate = decimal.Parse(txtovertimerate.Text);
                decimal overtimeHours = decimal.Parse(txtovertimehours.Text);
                decimal taxRate = decimal.Parse(txttaxrate.Text);

                int salaryCycleDays = (endDate - startDate).Days;

                if (salaryCycleDays == 0)
                {
                    MessageBox.Show("Error: Salary cycle duration is zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal basePay = CalculateBasePay(monthlySalary, allowances, overtimeRate, overtimeHours);
                decimal noPay = CalculateNoPay(monthlySalary, leavedays, startDate, endDate, salaryCycleDays);
                decimal grossPay = CalculateGrossPay(basePay, noPay, taxRate);

                txtnopay.Text = noPay.ToString();
                txtbasepay.Text = basePay.ToString();
                txtgrosspay.Text = grossPay.ToString();
                MessageBox.Show("Employee Salary Calculation Success!", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[emp_salary] SET startdate = @startdate, enddate = @enddate, monthlysalary = @monthlysalary, overtimerate = @overtimerate, overtimehours = @overtimehours, allowance = @allowance, leavedays = @leavedays, taxrate = @taxrate, nopay = @nopay, basepay = @basepay, grosspay = @grosspay WHERE empid = @empid", con);

                cmd.Parameters.AddWithValue("@empid", int.Parse(txtempid.Text));
                cmd.Parameters.AddWithValue("@startdate", startdate.Value);
                cmd.Parameters.AddWithValue("@enddate", enddate.Value);
                cmd.Parameters.AddWithValue("@monthlysalary", decimal.Parse(txtmonthlysalary.Text)); 
                cmd.Parameters.AddWithValue("@overtimerate", decimal.Parse(txtovertimerate.Text));
                cmd.Parameters.AddWithValue("@overtimehours", decimal.Parse(txtovertimehours.Text)); 
                cmd.Parameters.AddWithValue("@allowance", decimal.Parse(txtallowance.Text)); 
                cmd.Parameters.AddWithValue("@leavedays", int.Parse(txtleavedays.Text)); 
                cmd.Parameters.AddWithValue("@taxrate", decimal.Parse(txttaxrate.Text)); 
                cmd.Parameters.AddWithValue("@nopay", decimal.Parse(txtnopay.Text)); 
                cmd.Parameters.AddWithValue("@basepay", decimal.Parse(txtbasepay.Text)); 
                cmd.Parameters.AddWithValue("@grosspay", decimal.Parse(txtgrosspay.Text)); 

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee Salary Record Successfully Updated!", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Please check the format of your input values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btndelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[emp_salary] WHERE empid = @empid", con);
                cmd.Parameters.AddWithValue("@empid", int.Parse(txtempid.Text));

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Employee Salary Record Successfully Deleted!", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                }
                else
                {
                    MessageBox.Show("No record found for deletion.", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[emp_salary] (startdate, enddate, leavedays, overtimehours, taxrate, nopay, basepay, grosspay, empid, allowance, overtimerate, monthlysalary) VALUES (@startdate, @enddate, @leavedays, @overtimehours, @taxrate, @nopay, @basepay, @grosspay, @empid, @allowance, @overtimerate, @monthlysalary)", con);

                cmd.Parameters.AddWithValue("@startdate", startdate.Value);
                cmd.Parameters.AddWithValue("@enddate", enddate.Value);
                cmd.Parameters.AddWithValue("@leavedays", txtleavedays.Text);
                cmd.Parameters.AddWithValue("@overtimehours", txtovertimehours.Text);
                cmd.Parameters.AddWithValue("@taxrate", int.Parse(txttaxrate.Text));
                cmd.Parameters.AddWithValue("@nopay", float.Parse(txtnopay.Text));
                cmd.Parameters.AddWithValue("@basepay", float.Parse(txtbasepay.Text));
                cmd.Parameters.AddWithValue("@grosspay", float.Parse(txtgrosspay.Text));
                cmd.Parameters.AddWithValue("@allowance", float.Parse(txtallowance.Text));
                cmd.Parameters.AddWithValue("@overtimerate", float.Parse(txtovertimerate.Text));
                cmd.Parameters.AddWithValue("@monthlysalary", float.Parse(txtmonthlysalary.Text));
                cmd.Parameters.AddWithValue("@empid", int.Parse(txtempid.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Salary Record Successfully Saved!", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[emp_salary]", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            grid.Rows.Add(reader["empid"], reader["monthlysalary"], reader["overtimehours"], reader["allowance"]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found.", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            
        }

        private void picsearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                if (int.TryParse(txtempid.Text, out int employeeId))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[emp_salary] WHERE empid = @empid", con);
                    cmd.Parameters.AddWithValue("@empid", employeeId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        startdate.Value = Convert.ToDateTime(reader["startdate"]);
                        enddate.Value = Convert.ToDateTime(reader["enddate"]);
                        txtmonthlysalary.Text = reader["monthlysalary"].ToString();
                        txtovertimerate.Text = reader["overtimerate"].ToString();
                        txtovertimehours.Text = reader["overtimehours"].ToString();
                        txtallowance.Text = reader["allowance"].ToString();
                        txtleavedays.Text = reader["leavedays"].ToString();
                        txttaxrate.Text = reader["taxrate"].ToString();
                        txtnopay.Text = reader["nopay"].ToString();
                        txtbasepay.Text = reader["basepay"].ToString();
                        txtgrosspay.Text = reader["grosspay"].ToString();

                    }
                    else
                    {
                        MessageBox.Show("No record found for the given Employee ID.", "Grifindo Payroll System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Employee ID. Please enter a valid numeric value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void salary_Load(object sender, EventArgs e)
        {
            this.emp_salaryTableAdapter.Fill(this.db_payrollDataSet5.emp_salary);

        }

        private void picback_Click(object sender, EventArgs e)
        {
            this.Close();
            menu menu = new menu();
            menu.Show();
        }

        private void piclogout_Click(object sender, EventArgs e)
        {
            login login = new login();
            this.Close();
            login.Show();
        }
    }
    }
    

