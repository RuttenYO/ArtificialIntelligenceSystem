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
        RuleProcessor ruleProcessor;

        List<RuleStruct> answers;

        Question currentQuestion;

        public UserForm()
        {
            InitializeComponent();
            inquirer = new Inquirer();
            inquirer.getQuestionsFromFile();

            ruleProcessor = new RuleProcessor(inquirer);
            ruleProcessor.getRulesFromFile();

            answers = new List<RuleStruct>();

            nextQuestionAction(); 
        }

        public void nextQuestionAction()
        {
            currentQuestion = inquirer.getQuestion();

            if (currentQuestion != null)
            {
                comboBox1.Items.Clear();
                label1.Text = currentQuestion.title;
                for (int i = 0; i < currentQuestion.answers.Count; i++)
                    comboBox1.Items.Add(currentQuestion.answers[i]);
                comboBox1.SelectedIndex = 0;
            }
        }


        private void answerLastQuestion()
        {
            if (currentQuestion != null)
            {
                RuleStruct answer = new RuleStruct();

                answer.variable = currentQuestion.variableName;
                answer.value = comboBox1.SelectedItem.ToString();

                answers.Add(answer);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            answerLastQuestion();

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

        private void button3_Click(object sender, EventArgs e)
        {
            List<RuleStruct> results = ruleProcessor.getConclusionWithUserAnswers(answers);
            string message = "Результаты:\n";
            foreach (RuleStruct answer in results)
            {
                message += answer.variable.ToString() + "==" + answer.value.ToString() + ";\n";
            }
            MessageBox.Show(message);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            nextQuestionAction();
        }

    }
}
