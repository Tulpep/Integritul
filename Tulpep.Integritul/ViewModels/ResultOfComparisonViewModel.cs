using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tulpep.Integritul.Models;

namespace Tulpep.Integritul.ViewModels
{
    public class ResultOfComparisonViewModel : Screen
    {
        public IEnumerable<ResultOfComparison> ResultList { get; set; } 

        public ResultOfComparisonViewModel(IEnumerable<ResultOfComparison> result)
        {
            ResultList = result;
        }

        public void Home()
        {
            IShell shell = IoC.Get<IShell>();
            shell.ChangeScreen(new HomeViewModel());
        }
    }
}
