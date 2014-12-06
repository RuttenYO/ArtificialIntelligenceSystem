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
            tempQuestion.title = q[0];
            tempQuestion.variableName = q[1];
            for (int i = 2; i < q.Count; i++)
                tempQuestion.answers.Add(q[i]);
            questions.Add(tempQuestion);
        }

        public Question getQuestion()
        {
            Question result;
            if (c == questions.Count) result = null;
            else result = questions[c++];
            return result;
        }

        public Question getQuestionAtIndex(int index)//возвращает один вопрос
        {
            if (index < questions.Count) return questions[index];
            return null;
        }

        public void addAnswerForQuestion(List<string> question, string answer)
        {
            question.Add(answer);
        }

        public void addAnswerForQuestion(int index, string answer)
        {
            Question tempQ = questions[index];
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

        public void getQuestionsFromFile()
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
                        addQuestion(tempList);
                        tempList = new List<string>();
                    }
                }
            }
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
                        file.WriteLine(tempQ.answers[j]);
                    file.WriteLine("=====");
                }
            }
        } 
    }
    
    public struct RuleStruct
    {
        public string variable;
        public string value;
        public static bool operator == (RuleStruct firstStruct, RuleStruct secondStruct)
        {
            if (String.Equals(firstStruct.value, secondStruct.value) && String.Equals(firstStruct.variable, secondStruct.variable))
                return true;
            return false;
        }
        public static bool operator != (RuleStruct firstStruct, RuleStruct secondStruct)
        {
            return !(firstStruct == secondStruct);
        }
    }

    public class Rule
    {
        public List<RuleStruct> conditions;
        public RuleStruct result;
        public string logicalValue;
        public bool used;
        public Rule(Inquirer inquirer)
        {
            conditions = new List<RuleStruct>();
            buildRuleStructWithInqurer(inquirer);
            used = false;
            logicalValue = "";
        }
        public void buildRuleStructWithInqurer(Inquirer inquirer)
        {
            for (int i = 0; i < inquirer.getCountOfQuestions(); i++)
            {
                Question q = inquirer.getQuestionAtIndex(i);
                RuleStruct r = new RuleStruct();
                r.variable = q.variableName;
                r.value = "";
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
        public bool containsAnswer(RuleStruct answer)
        {
            foreach (RuleStruct condition in conditions)
                if (condition == answer) return true;
            return false;
        }
    }

    public class RuleProcessor
    {
        List<Rule> rules;
        Inquirer inquirer;
        public RuleProcessor(Inquirer inquirer)
        {
            rules = new List<Rule>();
            this.inquirer = inquirer;
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
                    bool needLogical = false;
                    for (int j = 0; j < tempRule.conditions.Count; j++)
                    {
                        if (tempRule.conditions[j].value != "")
                        {
                            if (needLogical && tempRule.logicalValue != "") file.WriteLine(tempRule.logicalValue);
                            file.WriteLine(tempRule.conditions[j].variable);
                            file.WriteLine(tempRule.conditions[j].value);
                            needLogical = true;
                        }
                    }                 
                    file.WriteLine(tempRule.result.variable);
                    file.WriteLine(tempRule.result.value);
                    file.WriteLine("=====");
                }
            }
        }

        public void getRulesFromFile()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(@".\Rules.db", true))
            {
                string tempVariable, tempValue,tempLogical;
                while (!file.EndOfStream)
                {
                    Rule tempRule = new Rule(inquirer);
                    tempVariable = file.ReadLine();
                    tempValue = file.ReadLine();

                    tempRule.addValueForVariable(tempVariable, tempValue);

                    tempLogical = file.ReadLine();
                    if (String.Equals(tempLogical, "И") || String.Equals(tempLogical, "ИЛИ"))
                    { 
                        tempRule.addLogical(tempLogical);
                        tempVariable = file.ReadLine();
                        tempValue = file.ReadLine();
                        tempRule.addValueForVariable(tempVariable, tempValue);
                        tempVariable = file.ReadLine();
                    }
                    else
                        tempVariable = tempLogical;

                    tempValue = file.ReadLine();

                    tempRule.addResult(tempVariable, tempValue);
                    file.ReadLine();
                    rules.Add(tempRule);
                }
            }
        }

        List<string> buildConclusionVariableList()
        {
            List<string> result = new List<string>();
            foreach (Rule rule in rules)
            {
                if (!result.Contains(rule.result.variable))
                   result.Add(rule.result.variable);
            }
            return result;
        }

        List<string> buildConditionVariableList()
        {
            List<string> result = new List<string>();
            foreach (Rule rule in rules)
            {
                foreach (RuleStruct condition in rule.conditions)
                    if (!(condition.value.Equals("") || result.Contains(condition.variable)))
                        result.Add(condition.variable);
            }
            return result;
        }

        bool queueContainsVariable(Queue<RuleStruct> queue, string variable)
        {
            foreach (RuleStruct condition in queue)
                if (condition.variable.Equals(variable)) return true;
            return false;
        }

        bool secondConditionIsSatisfied(Rule rule, Queue<RuleStruct> queue)
        {
            foreach (RuleStruct condtion in rule.conditions)
            {
                if (!condtion.value.Equals(""))
                    foreach (RuleStruct answer in queue)
                        if (condtion == answer) return true;
            }
            return false;
        }

         public List<RuleStruct> getConclusionWithUserAnswers(List<RuleStruct> answers)
        {
            List<RuleStruct> results = new List<RuleStruct>();

            List<string> conclusionVariables = buildConclusionVariableList();

            List<string> conditionVariables = buildConditionVariableList();

            Queue<RuleStruct> variableQueue = new Queue<RuleStruct>();

            foreach (RuleStruct answer in answers)
            {
                variableQueue.Enqueue(answer);
            }

            while (variableQueue.Count != 0)
            {
                RuleStruct answer = variableQueue.Dequeue();

                foreach (Rule rule in rules)
                {
                    if (rule.used) continue;

                    if (rule.containsAnswer(answer))
                    {
                        rule.used = true;
                        if (rule.logicalValue.Equals("ИЛИ") || rule.logicalValue.Equals(""))
                        {
                            if (!conditionVariables.Contains(rule.result.variable)) results.Add(rule.result);
                            else if (!queueContainsVariable(variableQueue, rule.result.variable)) variableQueue.Enqueue(rule.result);
                        }
                        if (rule.logicalValue.Equals("И"))
                        {
                           if (secondConditionIsSatisfied(rule,variableQueue))
                           {
                               if (!conditionVariables.Contains(rule.result.variable)) results.Add(rule.result);
                               else if (!queueContainsVariable(variableQueue, rule.result.variable)) variableQueue.Enqueue(rule.result);
                           }
                        }
                    }
                }
            }



            return results;
        }

    }
}
