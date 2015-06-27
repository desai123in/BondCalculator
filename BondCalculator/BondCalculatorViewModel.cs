using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondCalculator
{
    public class BondCalculatorViewModel
    {
        private BondCalculatorModel calculator = new BondCalculatorModel();

        public BondCalculatorModel Calculator
        {
            get { return calculator; }            
        }


    }
}
