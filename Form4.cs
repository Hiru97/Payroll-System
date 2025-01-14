using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void btnreg_Click(object sender, EventArgs e)
        {
            empreg empreg = new empreg();
            empreg.Show();
            Visible = false;

        }

        private void piclogout_Click(object sender, EventArgs e)
        {
            login login = new login();
            this.Close();
            login.Show();
        }

        private void btnsalaryscale_Click(object sender, EventArgs e)
        {
            salary salary = new salary();
            salary.Show();
            Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settings settings = new settings();
            settings.Show();
            Visible = false;
        }
    }
    }

