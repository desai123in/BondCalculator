using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BondCalculationEngine;
using BondCalculator.Common;
using BondCalculator.Model;
using System.Threading;

namespace BondCalculator.ViewModel
{
    public class BondCalculatorViewModel:INotifyPropertyChanged
    {
        #region Private Fields
        private BondCalculatorModel calculatorModel;
        private IBondCalculationEngine calculationEngine;
        //this field will inform viewmodel if Yield or PV needs to be computed.
        private bool yieldGiven;
        private string statusText = "";
        //this field is to prevent user from running new calculation when one is already pending.
        private bool calculationRunning = false;

        #endregion

        #region Constructors
        public BondCalculatorViewModel()
        {
            //create model object
            calculatorModel = new BondCalculatorModel();
            
            //create calculation engine object, this will ideally come through Unity or other DI container
            calculationEngine = Factory.GetBondCalculationEngine("Default");

            //register event from model, will be raised when error state of model properties will change
            //will be used to enable/disable calculator button/radio button
            calculatorModel.NotifyVM = () => { NotifyPropertyChanged("ReadyForCalculation"); };


            CalculateCommand = new DelegateCommand(param => PriceBond(), (param) => ((ReadyForCalculation) && (!calculationRunning)));
            
            //capture action when user wants to switch from ComputeYield to ComputePV and vice-versa
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

        //to update user of calculation status
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

        //this method will be called when user clicks Calculate button
        private void PriceBond()
        {
            
                StatusText = "Calculation In Progress......";
                calculationRunning = true;
                NotifyPropertyChanged("ReadyForCalculation");

                //if we want to compute Yield
                if (!yieldGiven)
                {                   
                    Task<decimal>.Factory.StartNew(() => calculationEngine.CalculateYield(calculatorModel.CouponRate, calculatorModel.YearsToMaturity, calculatorModel.Frequency, calculatorModel.FaceValue, calculatorModel.PresentValue))
                        .ContinueWith(t => 
                        {
                            try
                            {
                                if (t.Result < 1)
                                {
                                    calculatorModel.Yield = t.Result;
                                    StatusText = "Yield = ";
                                }
                                else //if calculation comes back with yield > 100% then just show error
                                {
                                    /*todo:log excepion*/
                                    StatusText = "Error: Yield > 100%";
                                }
                               
                            }
                            //catch errors coming from Task
                            catch (AggregateException ae)
                            {
                                /*todo:log excepion*/
                                StatusText = "Error in calculation.";
                            }
                            catch (Exception e)
                            {
                                /*todo:log excepion*/
                                StatusText = "Error in calculation.";
                            }
                            finally //clear calculation status and notify View of the same.
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

                                    calculatorModel.PresentValue = t.Result;
                                    StatusText = "Present Value = ";

                                }
                                catch (AggregateException ae)
                                {
                                    /*todo:log excepion*/
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
