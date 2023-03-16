using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marmara.Models
{
    public class ExamModel
    {
        public List<string> QuestionAndAnswers { get; set; }
        public int? userExamId { get; set; }
    }
}