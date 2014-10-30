using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MuzInst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeveloperForm developer = new DeveloperForm();
            this.Hide();
            developer.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            this.Hide();
            userForm.Show();
        }
    }
}
