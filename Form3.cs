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
    public partial class empreg : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BQDEANC\SQLEXPRESS;Initial Catalog=db_payroll;Integrated Security=True");
        string Gender;

        public empreg()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.MinimumSize = new Size(800, 600);
            LoadDataToGrid();   
        }

        private void picexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void piclogout_Click(object sender, EventArgs e)
        {
            login login = new login();
            this.Close();
            login.Show();
        }

        private void rbmale_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void rbfemale_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void btnreg_Click(object sender, EventArgs e)
        {
            if (txtempid.Text == " " || txtnic.Text == " " || txtaddress.Text == " " || this.dob.Text == " " || Gender == " " || txtmobile.Text == " " || txtemail.Text == " " || txtquali.Text == " " || txtposition.Text == " " || this.joindate.Text == " " || txthrrate.Text == " ")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[emp_reg](empid,name,nic,address,dob,gender,mobile,email,qualifications,position,joindate,hourlyrate)values (@ID,@N,@NIC,@A,@D,@G,@M,@E,@Q,@P,@J,@H)", con);
                    cmd.Parameters.AddWithValue("@ID", txtempid.Text);
                    cmd.Parameters.AddWithValue("@N", txtname.Text);
                    cmd.Parameters.AddWithValue("@NIC", txtnic.Text);
                    cmd.Parameters.AddWithValue("@A", txtaddress.Text);
                    cmd.Parameters.AddWithValue("@D", dob.Text);
                    cmd.Parameters.AddWithValue("@G", Gender);
                    cmd.Parameters.AddWithValue("@M", txtmobile.Text);
                    cmd.Parameters.AddWithValue("@E", txtemail.Text);
                    cmd.Parameters.AddWithValue("@Q", txtquali.Text);
                    cmd.Parameters.AddWithValue("@P", txtposition.Text);
                    cmd.Parameters.AddWithValue("@J", joindate.Text);
                    cmd.Parameters.AddWithValue("@H", txthrrate.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Employee Record Registered Successfully");

                    LoadDataToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void picsearch_Click(object sender, EventArgs e)
        {
            try
            {
                int empID = int.Parse(txtempid.Text);

                string query_select = "SELECT empid, name, nic, address, dob, gender, mobile, email, qualifications, position, joindate, hourlyrate FROM [dbo].[emp_reg] WHERE empid = @empid";

                using (SqlCommand cmd = new SqlCommand(query_select, con))
                {
                    cmd.Parameters.AddWithValue("@empid", empID);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtname.Text = dr["name"].ToString();
                            txtnic.Text = dr["nic"].ToString();
                            txtaddress.Text = dr["address"].ToString();
                            dob.Text = dr["dob"].ToString();

                            if (dr["gender"].ToString() == "Male")
                                rbmale.Checked = true;
                            else
                                rbfemale.Checked = true;

                            txtmobile.Text = dr["mobile"].ToString();
                            txtemail.Text = dr["email"].ToString();
                            txtquali.Text = dr["qualifications"].ToString();
                            txtposition.Text = dr["position"].ToString();
                            joindate.Text = dr["joindate"].ToString();
                            txthrrate.Text = dr["hourlyrate"].ToString();

                            MessageBox.Show("Employee Details Found");
                        }
                        else
                        {
                            MessageBox.Show("Employee Not Found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtempid.Text) || string.IsNullOrWhiteSpace(txtnic.Text) || string.IsNullOrWhiteSpace(txtaddress.Text) ||
        string.IsNullOrWhiteSpace(dob.Text) || string.IsNullOrWhiteSpace(Gender) || string.IsNullOrWhiteSpace(txtmobile.Text) ||
        string.IsNullOrWhiteSpace(txtemail.Text) || string.IsNullOrWhiteSpace(txtquali.Text) || string.IsNullOrWhiteSpace(txtposition.Text) ||
        string.IsNullOrWhiteSpace(joindate.Text) || string.IsNullOrWhiteSpace(txthrrate.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    int empID = int.Parse(txtempid.Text);

                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE [dbo].[emp_reg] SET name=@N, nic=@NIC, address=@A, dob=@D, gender=@G, mobile=@M, email=@E, qualifications=@Q, position=@P, joindate=@J, hourlyrate=@H WHERE empid=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", empID);
                    cmd.Parameters.AddWithValue("@N", txtname.Text);
                    cmd.Parameters.AddWithValue("@NIC", txtnic.Text);
                    cmd.Parameters.AddWithValue("@A", txtaddress.Text);
                    cmd.Parameters.AddWithValue("@D", dob.Text);
                    cmd.Parameters.AddWithValue("@G", Gender);
                    cmd.Parameters.AddWithValue("@M", txtmobile.Text);
                    cmd.Parameters.AddWithValue("@E", txtemail.Text);
                    cmd.Parameters.AddWithValue("@Q", txtquali.Text);
                    cmd.Parameters.AddWithValue("@P", txtposition.Text);
                    cmd.Parameters.AddWithValue("@J", joindate.Text);
                    cmd.Parameters.AddWithValue("@H", txthrrate.Text);

                    LoadDataToGrid();

                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee Record Updated Successfully");
                        LoadDataToGrid();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No record updated. Employee Not Found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                int empID = int.Parse(txtempid.Text);

                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[emp_reg] WHERE empid = @empid", con);
                cmd.Parameters.AddWithValue("@empid", empID);

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Employee Record Deleted Successfully");
                    LoadDataToGrid(); // Reload data to grid after delete
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("No record deleted. Employee Not Found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void ClearFields()
        {
            txtname.Clear();
            txtnic.Clear();
            txtaddress.Clear();

            dob.Value = DateTime.Now;
            joindate.Value = DateTime.Now;

            rbmale.Checked = false;
            rbfemale.Checked = false;
            txtmobile.Clear();
            txtemail.Clear();
            txtquali.Clear();
            txtposition.Clear();

            txthrrate.Clear();
            txtempid.Clear();
        }

        private void picback_Click(object sender, EventArgs e)
        {
            this.Close();
            menu menu = new menu();
            menu.Show();
        }

        private void LoadDataToGrid()
        {
            try
            {
                con.Open();

                string query_select_all = "SELECT empid, name, nic, address, dob, gender, mobile, email, qualifications, position, joindate, hourlyrate FROM [dbo].[emp_reg]";

                using (SqlCommand cmd = new SqlCommand(query_select_all, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        grid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void empreg_Load(object sender, EventArgs e)
        {
            LoadDataToGrid();
        }

        private void empreg_Load_1(object sender, EventArgs e)
        {
            
            this.emp_regTableAdapter.Fill(this.db_payrollDataSet1.emp_reg);

        }

        private void picreset_Click(object sender, EventArgs e)
        {
            ClearFields(); 
            LoadDataToGrid(); 
        }
    }
}
