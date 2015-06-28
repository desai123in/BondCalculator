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

            calculatorModel.NotifyVM = () => { NotifyPropertyChanged("ReadyForCalculation"); };


            CalculateCommand = new DelegateCommand(param => PriceBond(), (param) => ((ReadyForCalculation) && (!calculationRunning)));
            
            YieldToggleCommand = new DelegateCommand(param => { 
                yieldGiven = (bool)param;
                StatusText = "";
                if (yieldGiven)
                {
                    calculatorModel.Yield = 0.0m;
                    calculatorModel.PresentValue = 0.0m;
                }
            }, (param) => true);

            PVToggleCommand = new DelegateCommand(param => { 
                yieldGiven = !(bool)param;
                StatusText = "";
                if (!yieldGiven)
                {
                    calculatorModel.PresentValue = 0.0m;
                    calculatorModel.Yield = 0.0m;
                }
            }, (param) => true);

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

        public bool ReadyForCalculation
        {
            get { return this.calculatorModel.IsValid && !calculationRunning; }
            
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
            try
            {
                StatusText = "Calculation In Progress......";
                calculationRunning = true;
                NotifyPropertyChanged("ReadyForCalculation");
                if (!yieldGiven)
                {                   
                    Task<decimal>.Factory.StartNew(() => calculationEngine.CalculateYield(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.PresentValue))
                        .ContinueWith(t => 
                        {
                            try
                            {
                                if (!t.IsFaulted)
                                {
                                    calculatorModel.Yield = t.Result;
                                    StatusText = "Yield = ";
                                }
                            }
                            catch (AggregateException ae)
                            {
                                //log excepion
                                StatusText = "Error in calculation.";
                            }
                            finally
                            {
                                calculationRunning = false;
                                NotifyPropertyChanged("ReadyForCalculation");
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {                    
                    Task<decimal>.Factory.StartNew(() => calculationEngine.CalculatePresentValue(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.Yield))
                            .ContinueWith(t => 
                            {
                                try
                                {
                                    if (!t.IsFaulted)
                                    {
                                        calculatorModel.PresentValue = t.Result;
                                        StatusText = "Present Value = ";
                                    }
                                }
                                catch (AggregateException ae)
                                {
                                    //log excepion
                                    StatusText = "Error in calculation.";
                                }
                                finally
                                {
                                    calculationRunning = false;
                                    NotifyPropertyChanged("ReadyForCalculation");
                                }

                            }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }           
            catch (Exception e)
            {
                //log excepion
                StatusText = "Error in calculation.";
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
