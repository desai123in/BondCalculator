using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BondCalculator
{
    public class BondCalculatorViewModel:INotifyPropertyChanged
    {
        #region Private Fields
        private BondCalculatorModel calculatorModel;
        #endregion

        #region Constructors
        public BondCalculatorViewModel()
        {
            calculatorModel = new BondCalculatorModel();
            calculatorModel.NotifyVM = () => { NotifyPropertyChanged("IsValid"); };
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public BondCalculatorModel CalculatorModel
        {
            get { return calculatorModel; }            
        }

        public bool IsValid
        {
            get { return this.calculatorModel.IsValid; }
            
        }

        #region Private Methods
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                // property changed
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion




    }
}
