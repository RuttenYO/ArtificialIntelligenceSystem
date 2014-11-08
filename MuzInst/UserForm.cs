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
    public partial class UserForm : Form
    {
        Inquirer inquirer;

        public UserForm()
        {
            inquirer = new Inquirer();
            InitializeComponent();
            inquirer.getQuestionsFromFile();

            nextQuestionAction(); 
        }

        public void nextQuestionAction()
        {
            Question tempQ = inquirer.getQuestion();
            if (tempQ!=null)
            {
                comboBox1.Items.Clear();
                label1.Text = tempQ.title;
                for (int i = 0; i < tempQ.answers.Count; i++)
                    comboBox1.Items.Add(tempQ.answers[i]);
                comboBox1.SelectedIndex = 0;
            }
        }
  
        private void button2_Click_1(object sender, EventArgs e)
        {
            nextQuestionAction();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
