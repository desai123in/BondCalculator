using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BondCalculator
{
    public class BondCalculatorModel:ModelBase
    {
        #region Private Fields

        private decimal couponRate = 10m;
        private int yearsToMaturity = 5;
        private int frequency = 1;
        private decimal faceValue = 1000m;
        private decimal yield = 15m;
        private decimal presentValue;

        #endregion
        #region Public Properties
        //if (this._CustomerId != value)
        //    {
        //        if (value == "abc") 
        //            base.AddError("CustomerId", "abc not allowed");
        //        else
        //            base.RemoveError("CustomerId");
 
        //        this._CustomerId = value;
        //        base.NotifyPropertyChanged("CustomerId", new Action<bool>((valid) => { AppMessages.CustomerIsValid.Send(valid); }));
        //    }
        public decimal CouponRate
        {
            get { return couponRate; }
            set 
            {
                if (!couponRate.Equals(value))
                {
                    if (value.Equals(101))
                        base.AddError("CouponRate", ">100 rate not allowed");
                    else
                        base.RemoveError("CouponRate");
                    couponRate = value;
                    NotifyPropertyChanged("CouponRate");
                }
            }
        }

        public int YearsToMaturity
        {
            get { return yearsToMaturity; }
            set { 
                    yearsToMaturity = value;
                    NotifyPropertyChanged("YearsToMaturity");
                }
        }

        public int Frequency
        {
            get { return frequency; }
            set { 
                    frequency = value;
                    NotifyPropertyChanged("Frequency");
                }
        }

        public decimal FaceValue
        {
            get { return faceValue; }
            set { 
                    faceValue = value;
                    NotifyPropertyChanged("FaceValue");
                }
        }

        public decimal Yield
        {
            get { return yield; }
            set { 
                    yield = value;
                    NotifyPropertyChanged("Yield");
                }
        }

        public decimal PresentValue
        {
            get { return presentValue; }
            set { 
                    presentValue = value;
                    NotifyPropertyChanged("PresentValue");
                }
        }

        #endregion

        #region private methods
        
        #endregion

        #region Public Members
        
        #endregion
    }
}
