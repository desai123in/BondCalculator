using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BondCalculator.Common;

namespace BondCalculator.Model
{
    /// <summary>
    /// Model to bind bond attributes to UI
    /// supports Property notification and data validation
    /// some validation rules are assumed as not specified in requirements.
    /// coupon frequency is added which can be from 1 to 4 coupon payment per year
    /// </summary>
    public class BondCalculatorModel:ModelBase
    {
        #region Private Fields

        private decimal couponRate = 0.10m;
        private int yearsToMaturity = 5;
        private int frequency = 1;
        private decimal faceValue = 1000m;
        private decimal yield = 0.0m;
        private decimal presentValue = 832.40m;
        

        #endregion
        #region Public Properties
      
        public decimal CouponRate
        {
            get { return couponRate; }
            set 
            {
                if (!couponRate.Equals(value))
                {
                    if (value.CompareTo(1.0m) > 0 || value.CompareTo(0.001m) <0)
                        base.AddError("CouponRate", "Allowed range: 0.1% to 100% ");
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
                if (!yearsToMaturity.Equals(value))
                    {
                        int maxmaturity = AppConfig.Instance.MaxMaturityYearsAllowed;
                        if (value.CompareTo(0) < 0 || value.CompareTo(maxmaturity) > 0)
                            base.AddError("YearsToMaturity", string.Format("Allowed range: 1 to {0}",maxmaturity));
                        else
                            base.RemoveError("YearsToMaturity");

                        yearsToMaturity = value;
                        NotifyPropertyChanged("YearsToMaturity");
                    }                   
                }
        }

        public int Frequency
        {
            get { return frequency; }
            set {                     

                    if (!frequency.Equals(value))
                    {
                        if (value.CompareTo(1) < 0 || value.CompareTo(4) > 0)
                            base.AddError("Frequency", "Allowed range: 1 to 4");
                        else
                            base.RemoveError("Frequency");

                        frequency = value;
                        NotifyPropertyChanged("Frequency");
                    }
                }
        }

        public decimal FaceValue
        {
            get { return faceValue; }
            set {
                if (!faceValue.Equals(value))
                    {
                        if (value.CompareTo(0.0m) < 0)
                            base.AddError("FaceValue", "Allowed range: Non Negetive value ");
                        else
                            base.RemoveError("FaceValue");                     

                        faceValue = value;
                        NotifyPropertyChanged("FaceValue");
                    }
                }
        }

        public decimal Yield
        {
            get { return yield; }
            set {
                    if (!yield.Equals(value))
                    {
                        if (value.CompareTo(1.0m) > 0)
                            base.AddError("Yield", "Allowed range: < 100% ");
                        else
                            base.RemoveError("Yield");
                        yield = value;
                        NotifyPropertyChanged("Yield");
                    }                   
                }
        }

        public decimal PresentValue
        {
            get { return presentValue; }
            set {
                    if (!presentValue.Equals(value))
                    {
                        presentValue = value;
                        NotifyPropertyChanged("PresentValue");
                    }
                }
        }

        #endregion

        #region private methods
        
        #endregion

        #region Public Members
        
        #endregion
    }
}
