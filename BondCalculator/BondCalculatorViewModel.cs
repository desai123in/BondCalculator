using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BondCalculationEngine;
using BondCalculator.Common;

namespace BondCalculator
{
    public class BondCalculatorViewModel:INotifyPropertyChanged
    {
        #region Private Fields
        private BondCalculatorModel calculatorModel;
        private IBondCalculationEngine calculationEngine;
        private bool calculateYield;

        #endregion

        #region Constructors
        public BondCalculatorViewModel()
        {
            calculatorModel = new BondCalculatorModel();
            calculationEngine = Factory.GetBondCalculationEngine("Default");

            calculatorModel.NotifyVM = () => { NotifyPropertyChanged("IsValid"); };
            CalculateCommand = new DelegateCommand(param => { _calc.Number(Convert.ToInt32(param)); UpdateDisplay(); },
                                            param => _calc.CanDoNumber());
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Property
        public BondCalculatorModel CalculatorModel
        {
            get { return calculatorModel; }            
        }

        public bool IsValid
        {
            get { return this.calculatorModel.IsValid; }
            
        }

        public bool CalcluateYield
        {
            set { calculateYield = value; }
        }

        public DelegateCommand CalculateCommand { get; private set; }

        #endregion


        #region Private Methods

        private void PriceBond()
        {
            if(calculateYield)
            {
                try
                {
                    calculationEngine.CalculateYield(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.PresentValue);
                }
                catch(Exception e)
                {

                }
            }
        }

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
