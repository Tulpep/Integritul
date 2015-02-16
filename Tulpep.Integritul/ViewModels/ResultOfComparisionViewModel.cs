using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tulpep.Integritul.Models;

namespace Tulpep.Integritul.ViewModels
{
    public class ResultOfComparisionViewModel : Screen
    {
        public IEnumerable<ResultOfComparision> ResultList { get; set; } 

        public ResultOfComparisionViewModel(IEnumerable<ResultOfComparision> result)
        {
            ResultList = result;
        }
    }
}
