using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuzInst
{
    public class Inquirer
    {

        List<List<string>> list;

        int c;

         public Inquirer()
        {
            list = new List<List<string>>();//инициализация списка
            c = 0;
        }

        public void addQuestion(List<string> q)
        {
            list.Add(q);
        }

        public List<string> getQuestion()//возвращает список вопросов
        {
            List<string> result;
            if (c == list.Count) result = null;
            else result = list.ElementAt(c++);
            return result;
        }

        public List<string> getQuestionAtIndex(int index)//возвращает один вопрос
        {
            if (index < list.Count) return list.ElementAt(index);
            return null;
        }

        public void addAnswerForQuestion(List<string> question, string answer)
        {
            question.Add(answer);
        }

        public void addAnswerForQuestion(int index, string answer)
        {
            List<string> tempQ = list.ElementAt(index);
            tempQ.Add(answer);
        }
        

        public bool deleteAnswerForQuestion(List<string> question, string answer)
        {
            if (question.Contains(answer))
            {
                int i = question.FindIndex((elem) => elem == answer);
                question.RemoveAt(i);
                return true;
            }
            else return false;
        }

        public void deleteQuestion(int index)
        {
            if (index < list.Count)
                list.RemoveAt(index);
        }

        public int getCountOfQuestions()
        {
            return list.Count;
        }
    
    }
}
