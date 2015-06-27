using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondCalculator
{
    public class BondCalculatorViewModel
    {
        private BondCalculatorModel calculatorModel = new BondCalculatorModel();

        public BondCalculatorModel CalculatorModel
        {
            get { return calculatorModel; }            
        }

        public bool IsValid
        {
            get { return this.calculatorModel.IsValid; }
        }


    }
}
