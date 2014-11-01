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
            getQuestionFromFile();
        }

        public void getQuestionFromFile()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(@".\QUIZ.db", true))
            {
                string temp;
                List<string> tempList;

                tempList = new List<string>();
                while (!file.EndOfStream)
                {
                    temp = file.ReadLine();
                    if (!temp.Equals("====="))
                    {
                        tempList.Add(temp);
                    }
                    else
                    {
                        inquirer.addQuestion(tempList);
                        tempList = new List<string>();
                    }
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            Question tempQ = inquirer.getQuestion();
            label1.Text = tempQ.title;
            for (int i = 0; i < tempQ.answers.Count; i++)
                comboBox1.Items.Add(tempQ.answers.ElementAt(i));
            comboBox1.Text = tempQ.answers.ElementAt(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

    }
}
