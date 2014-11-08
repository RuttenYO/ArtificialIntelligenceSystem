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
        public RuleProcessor ruleProcessor;

        public DeveloperForm()
        {
            inquirer = new Inquirer();
            InitializeComponent();
            inquirer.getQuestionsFromFile();
            ruleProcessor = new RuleProcessor(inquirer);
            ruleProcessor.getRulesFromFile();
            showQuestionsToCombobox();
            label1.Text = "";
            comboBox1.SelectedIndex = 0;
            initRuleData();
        }

        public void showQuestionsToCombobox()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                comboBox1.Items.Add(inquirer.getQuestionAtIndex(i).title);
            }
            listBox1.Items.Clear();
        }
     
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e) //подтягивает ответы на выбранный вопрос
        {
            listBox1.Items.Clear();
            Question tempQ = inquirer.getQuestionAtIndex(comboBox1.SelectedIndex);

            label1.Text = tempQ.variableName;

            for (int i = 0; i < tempQ.answers.Count; i++)
            {
                listBox1.Items.Add(tempQ.answers[i]);
            }
        }

        public void updateListBoxOfAnswers(int index)//обновляет листбокс во всех ситуациях
        {
            Question tempQ = inquirer.getQuestionAtIndex(index);

            label1.Text = tempQ.variableName;

            listBox1.Items.Clear();

            for (int i = 0; i < tempQ.answers.Count; i++)
            {
                listBox1.Items.Add(tempQ.answers[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tempQ = new List<string>();
            tempQ.Add(textBox1.Text);
            tempQ.Add(textBox3.Text);
            inquirer.addQuestion(tempQ);
            showQuestionsToCombobox();
            comboBox1.SelectedIndex = comboBox1.Items.Count-1;
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            inquirer.saveQuestionsToFile();
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            inquirer.deleteQuestion(comboBox1.SelectedIndex);
            showQuestionsToCombobox();
            comboBox1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Question tempQ = inquirer.getQuestionAtIndex(comboBox1.SelectedIndex);
            string answer = listBox1.Items[listBox1.SelectedIndex].ToString();
            inquirer.deleteAnswerForQuestion(tempQ, answer);
            updateListBoxOfAnswers(comboBox1.SelectedIndex);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int tempIndex = comboBox1.SelectedIndex;
            string tempAnsw = textBox2.Text;

            Question tempQ = inquirer.getQuestionAtIndex(tempIndex);
            if (!tempQ.answers.Contains(tempAnsw))
            {
                inquirer.addAnswerForQuestion(tempIndex, tempAnsw);
                updateListBoxOfAnswers(tempIndex);
            }
            else
            {
                MessageBox.Show("Уже существует такой элемент!");
            }
            textBox2.Clear();
        }

        private void initRuleData()
        {
            Question q;
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                q = inquirer.getQuestionAtIndex(i);
                variable1ComboBox.Items.Add(q.variableName);
                
            }
            variable1ComboBox.SelectedIndex = 0;

            logicComboBox.Items.Add("И");
            logicComboBox.Items.Add("ИЛИ");
            logicComboBox.SelectedIndex = 0;
            
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                q = inquirer.getQuestionAtIndex(i);
                variable2ComboBox.Items.Add(q.variableName);

            }
            variable2ComboBox.Items.Add("НЕ ИСПОЛЬЗОВАТЬ");
            variable2ComboBox.SelectedIndex = 0;

            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                q = inquirer.getQuestionAtIndex(i);
                resultVariableComboBox.Items.Add(q.variableName);
            }
            resultVariableComboBox.SelectedIndex = 0;
            
            
        }

        private void addRuleButton_Click(object sender, EventArgs e)
        {
            Rule rule = new Rule(inquirer);

            rule.addValueForVariable(variable1ComboBox.Text, value1ComboBox.Text);

            if (!String.Equals(variable2ComboBox.Text,"НЕ ИСПОЛЬЗОВАТЬ"))
            {
                rule.addLogical(logicComboBox.Text);
                rule.addValueForVariable(variable2ComboBox.Text, value2ComboBox.Text);
            }

            rule.addResult(resultVariableComboBox.Text, resultValueComboBox.Text);

            ruleProcessor.AddRule(rule);

            ruleProcessor.saveRulesToFile();
        }

        private void variable1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Question tempQ = inquirer.getQuestionAtIndex(variable1ComboBox.SelectedIndex);

            value1ComboBox.Items.Clear();

            for (int i = 0; i < tempQ.answers.Count; i++)
            {
                value1ComboBox.Items.Add(tempQ.answers[i]);
            }
            value1ComboBox.SelectedIndex = 0;
        }

        private void variable2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            value2ComboBox.Items.Clear();
            value2ComboBox.Text = "";
            if (variable2ComboBox.SelectedIndex != variable1ComboBox.Items.Count)
            {
                
                Question tempQ = inquirer.getQuestionAtIndex(variable2ComboBox.SelectedIndex);

               
                for (int i = 0; i < tempQ.answers.Count; i++)
                {
                    value2ComboBox.Items.Add(tempQ.answers[i]);
                }
                value2ComboBox.SelectedIndex = 0;
            }
           
            
        }

        private void DeveloperForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void resultVariableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Question tempQ = inquirer.getQuestionAtIndex(resultVariableComboBox.SelectedIndex);
            
            resultValueComboBox.Items.Clear();

            for (int i = 0; i < tempQ.answers.Count; i++)
            {
                resultValueComboBox.Items.Add(tempQ.answers[i]);
            }
            resultValueComboBox.SelectedIndex = 0;
        }

    }
}
