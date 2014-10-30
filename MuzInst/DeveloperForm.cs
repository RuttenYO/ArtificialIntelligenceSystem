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
    public partial class DeveloperForm : Form
    {
        public Inquirer inquirer;

        public DeveloperForm()
        {
            inquirer = new Inquirer();
            InitializeComponent();
            getQuestionFromFile();
            showQuestionsToCombobox();

        }

        public void showQuestionsToCombobox()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                comboBox1.Items.Add(inquirer.getQuestionAtIndex(i).ElementAt(0));
            }
        }


        public void putQuestionToFile(string question)
        {

        }

        public void putAnswerToFile(string answer)
        {

        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e) //подтягивает ответы на выбранный вопрос
        {
            listBox1.Items.Clear();
            List<string> tempQ = inquirer.getQuestionAtIndex(comboBox1.SelectedIndex);

            for (int i = 1; i < tempQ.Count; i++)
            {
                listBox1.Items.Add(tempQ.ElementAt(i));
            }
        }

        public void updateListBoxOfAnswers(int index)//обновляет листбокс во всех ситуациях
        {
            List<string> tempQ = inquirer.getQuestionAtIndex(index);

            listBox1.Items.Clear();

            for (int i = 1; i < tempQ.Count; i++)
            {
                listBox1.Items.Add(tempQ.ElementAt(i));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tempQ = new List<string>();
            tempQ.Add(textBox1.Text);
            inquirer.addQuestion(tempQ);
            showQuestionsToCombobox();
        }

        private void button2_Click(object sender, EventArgs e)
        {

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

 /*       public void printVectorToFile()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\QUIZ.db", true))
            {
                for (int i = 0; i < list.Count; i++) file.Write(list.ElementAt(i) + " ");
                file.WriteLine();
            }
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> tempList;
            tempList = inquirer.getQuestion();
            string answer = listBox1.Items[listBox1.SelectedIndex].ToString();
            inquirer.deleteAnswerForQuestion(tempList, answer);
            updateListBoxOfAnswers(listBox1.SelectedIndex-1);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int tempIndex = comboBox1.SelectedIndex;
            string tempAnsw = textBox2.Text;

            List<string> tempQ = inquirer.getQuestionAtIndex(tempIndex);
            if (!tempQ.Contains(tempAnsw))
            {
                inquirer.addAnswerForQuestion(tempIndex, tempAnsw);
                updateListBoxOfAnswers(tempIndex);
            }
            else
            {
                MessageBox.Show("Уже существует такой элемнт!");
            }
        }

    }
}
