using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Payroll
{
    public partial class login : Form
    {
    
        public login()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BQDEANC\SQLEXPRESS;Initial Catalog=db_payroll;Integrated Security=True");


        private void btnlogin_Click(object sender, EventArgs e)
        {
            String username = txtusername.Text;
            String password = txtpassword.Text;

            try
            {
                String querry = "SELECT * FROM tbl_userpass WHERE username ='" + txtusername.Text + "' AND password ='" + txtpassword.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, con);

                DataTable dataTable = new DataTable();
                sda.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    username = txtusername.Text;
                    password = txtpassword.Text;

                    menu menu = new menu();
                    menu.Show();
                    this.Hide();
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    MessageBox.Show("Invalid Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtusername.Clear();
                    txtpassword.Clear();

                    txtusername.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                con.Close();
            }

        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
