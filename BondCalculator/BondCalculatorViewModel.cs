using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BondCalculationEngine;
using BondCalculator.Common;
using System.Threading;

namespace BondCalculator
{
    public class BondCalculatorViewModel:INotifyPropertyChanged
    {
        #region Private Fields
        private BondCalculatorModel calculatorModel;
        private IBondCalculationEngine calculationEngine;
        private bool yieldGiven;
        private string statusText = "";
        private bool calculationRunning = false;

        #endregion

        #region Constructors
        public BondCalculatorViewModel()
        {
            calculatorModel = new BondCalculatorModel();
            calculationEngine = Factory.GetBondCalculationEngine("Default");

            calculatorModel.NotifyVM = () => { NotifyPropertyChanged("IsValid"); };
            //CalculateCommand = new DelegateCommand(param => { _calc.Number(Convert.ToInt32(param)); UpdateDisplay(); },
            //                                param => _calc.CanDoNumber());

            CalculateCommand = new DelegateCommand(param => PriceBond(), (param) => IsValid && !calculationRunning);
            YieldToggleCommand = new DelegateCommand(param => { yieldGiven = (bool)param; StatusText = ""; }, (param) => true);
            PVToggleCommand = new DelegateCommand(param => { yieldGiven = !(bool)param; StatusText = ""; }, (param) => true);

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

        public string StatusText
        {
            get { return this.statusText; }
            set { statusText = value;
            NotifyPropertyChanged("StatusText");
            }

        }
        public DelegateCommand CalculateCommand { get; private set; }
        public DelegateCommand YieldToggleCommand { get; private set; }
        public DelegateCommand PVToggleCommand { get; private set; }

        #endregion


        #region Private Methods

        private void PriceBond()
        {

            decimal result;
                try
                {                   
                    if (!yieldGiven)
                    {
                        using (Task<decimal> task = new Task<decimal>(() => calculationEngine.CalculateYield(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.PresentValue)))
                        {

                            StatusText = "Started Calculation...";
                            calculationRunning = true;

                            task.Start();
                            task.Wait();// wait for the task to finish, without blocking the main thread

                            if (!task.IsFaulted)
                            {
                                calculatorModel.Yield = task.Result;//the task has finished its background work, and we can take the result
                                StatusText = "Yield =";//Signal the completion of the task
                            }
                        }
                    }
                    else
                    {
                        using (Task<decimal> task = new Task<decimal>(() => calculationEngine.CalculatePresentValue(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.Yield)))
                        {

                            StatusText = "Started Calculation...";
                            calculationRunning = true;

                            task.Start();
                            task.Wait();// wait for the task to finish, without blocking the main thread

                            if (!task.IsFaulted)
                            {
                                calculatorModel.PresentValue = task.Result;//the task has finished its background work, and we can take the result
                                StatusText = "Present Value = ";//Signal the completion of the task
                            }
                        }
                    }
                }
                catch(AggregateException ae)
                {
                    //log excepion
                    StatusText = "Error in calculation.";
                }
                catch(Exception e)
                {
                    //log excepion
                    StatusText = "Error in calculation.";
                }
                finally
                {
                    calculationRunning = false;
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
