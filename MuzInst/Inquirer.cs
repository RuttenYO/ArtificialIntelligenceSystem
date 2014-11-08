using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuzInst
{
    public class Question
    {
        public string title;
        public string variableName;
        public List<string> answers;
        public Question()
        {
            answers = new List<string>();
        }
    }        

    public class Inquirer
    {
        List<Question> questions;
        int c;

        public Inquirer()
        {
            questions = new List<Question>();
            c = 0;
        }

        public void addQuestion(List<string> q)
        {
            Question tempQuestion = new Question();
            tempQuestion.title = q.ElementAt(0);
            tempQuestion.variableName = q.ElementAt(1);
            for (int i = 2; i < q.Count; i++)
                tempQuestion.answers.Add(q.ElementAt(i));
            questions.Add(tempQuestion);
        }

        public Question getQuestion()
        {
            Question result;
            if (c == questions.Count) result = null;
            else result = questions.ElementAt(c++);
            return result;
        }

        public Question getQuestionAtIndex(int index)//возвращает один вопрос
        {
            if (index < questions.Count) return questions.ElementAt(index);
            return null;
        }

        public void addAnswerForQuestion(List<string> question, string answer)
        {
            question.Add(answer);
        }

        public void addAnswerForQuestion(int index, string answer)
        {
            Question tempQ = questions.ElementAt(index);
            tempQ.answers.Add(answer);
        }
        

        public bool deleteAnswerForQuestion(Question question, string answer)
        {
            if (question.answers.Contains(answer))
            {
                int i = question.answers.FindIndex((elem) => elem == answer);
                question.answers.RemoveAt(i);
                return true;
            }
            else return false;
        }

        public void deleteQuestion(int index)
        {
            if (index < questions.Count)
                questions.RemoveAt(index);
        }

        public int getCountOfQuestions()
        {
            return questions.Count;
        }

        public void saveQuestionsToFile()
        {
            System.IO.File.WriteAllText(@".\QUIZ.db", string.Empty);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\QUIZ.db", true))
            {
                for (int i = 0; i < this.getCountOfQuestions(); i++)
                {
                    Question tempQ = this.getQuestionAtIndex(i);
                    file.WriteLine(tempQ.title);
                    file.WriteLine(tempQ.variableName);
                    for (int j = 0; j < tempQ.answers.Count; j++)
                        file.WriteLine(tempQ.answers.ElementAt(j));
                    file.WriteLine("=====");
                }
            }
        }
    
    }

    public class Rule
    {
        public struct RuleStruct
        {
            public string variable;
            public string value;
        }
        public List<RuleStruct> conditions;
        public RuleStruct result;
        public string logicalValue;
        public Rule(Inquirer inquirer)
        {
            conditions = new List<RuleStruct>();
            buildRuleStructWithInqurer(inquirer);
        }
        public void buildRuleStructWithInqurer(Inquirer inquirer)
        {
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                Question q = inquirer.getQuestionAtIndex(i);
                RuleStruct r = new RuleStruct();
                r.variable = q.variableName;
                conditions.Add(r);
            }
        }
        public void addValueForVariable(string variable, string value)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (String.Equals(conditions[i].variable, variable))
                {
                    RuleStruct tempRuleStruct = new RuleStruct();
                    tempRuleStruct.variable = variable;
                    tempRuleStruct.value = value;
                    conditions[i] = tempRuleStruct;
                }
            }
        }
        public void addResult(string variable, string value)
        {
            result.variable = variable;
            result.value = value;
        }
        public void addLogical(string value)
        {
            logicalValue = value;
        }
    }

    public class RuleProcessor
    {
        List<Rule> rules;
        public RuleProcessor()
        {
            rules = new List<Rule>();
        }
        public void AddRule(Rule rule)
        {
            rules.Add(rule);
        }

        public void saveRulesToFile()
        {
            System.IO.File.WriteAllText(@".\Rules.db", string.Empty);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\Rules.db", true))
            {
                for (int i = 0; i < rules.Count; i++)
                {
                    Rule tempRule = rules[i];

                    for (int j = 0; j < tempRule.conditions.Count; j++)
                    {
                        if (tempRule.conditions[j].value != null)
                        {
                            file.WriteLine(tempRule.conditions[j].variable);
                            file.WriteLine(tempRule.conditions[j].value);
                        }
                    }
                    if (tempRule.logicalValue != null) file.WriteLine(tempRule.logicalValue);

                    file.WriteLine(tempRule.result.variable);
                    file.WriteLine(tempRule.result.value);
                    file.WriteLine("=====");
                }
            }
        }
    }
}
