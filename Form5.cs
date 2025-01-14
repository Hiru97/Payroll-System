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
    public partial class settings : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BQDEANC\SQLEXPRESS;Initial Catalog=db_payroll;Integrated Security=True");

        public settings()
        {
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[emp_settings]", con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Assuming you have columns in your Settings table named DateRange, SalaryCycleBegin, SalaryCycleEnd, and LeavesPerYear
                    txtdaterange.Text = reader["daterange"].ToString();
                    startdate.Value = Convert.ToDateTime(reader["startdate"]);
                    enddate.Value = Convert.ToDateTime(reader["enddate"]);
                    txtleaves.Text = reader["leaves"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        private void btnsave_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[emp_settings] (daterange, startdate, enddate, leaves) VALUES (@daterange, @startdate, @enddate, @leaves)", con);

                cmd.Parameters.AddWithValue("@daterange", txtdaterange.Text);
                cmd.Parameters.AddWithValue("@startdate", startdate.Value);
                cmd.Parameters.AddWithValue("@enddate", enddate.Value);
                cmd.Parameters.AddWithValue("@leaves", txtleaves.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Settings Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No Settings Saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving Settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            txtdaterange.Text = string.Empty;
            startdate.Value = DateTime.Now;
            enddate.Value = DateTime.Now;
            txtleaves.Text = string.Empty;
        }
    }
}

