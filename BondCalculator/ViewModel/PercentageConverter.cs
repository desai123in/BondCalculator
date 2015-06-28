using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace BondCalculator.ViewModel
{
    /// <summary>
    /// decimal to percentage converter, created for yield/coupon attributes
    /// </summary>

    public class PercentageConverter : IValueConverter
    {
        #region IValueConverter Members
        //E.g. Model 0.042367 --> UI "4.2367%"
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = 4;
            if(parameter != null)
                Int32.TryParse(parameter.ToString(), out i);

            var fraction = decimal.Parse(value.ToString());
            return fraction.ToString(string.Format("P{0}",i));
        }

        //E.g. UI "4.2367 %" --> Model 0.042367
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Trim any trailing percentage symbol that the user may have included
            var valueWithoutPercentage = value.ToString().TrimEnd(' ', '%');
            return decimal.Parse(valueWithoutPercentage) / 100;
        }

        #endregion //IValueConverter Members
    }
}
