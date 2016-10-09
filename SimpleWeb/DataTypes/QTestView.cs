using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWeb
{
    public class QTestView
    {
        public int QTestID { get; set; } = 0;

        public string Question { get; set; } = "";

        public string RightAnswer { get; set; } = "";

        public string WrongAnswers { get; set; } = "";

        public string QTestUp {
            get { return "Поднять вверх"; }
        }

        public string DeleteQTest {
            get { return "Удалить"; }
        }
    }
}
