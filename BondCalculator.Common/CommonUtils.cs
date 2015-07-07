using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BondCalculator.Common
{
    public class CommonUtils
    {
        static double[] pow10 = { 1e0, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10 };
        /// <summary>
        /// common function to truncate decimal without rounding
        /// </summary>
        /// <param name="x"></param>
        /// <param name="precision"></param>
        /// <returns></returns>

        public static decimal Truncate(decimal x, int precision)
        {
            if (precision < 0)
                throw new ArgumentException();
            if (precision == 0)
                return (decimal)Math.Truncate(x);
            decimal m = precision >= pow10.Length ? (decimal)Math.Pow(10, precision) : (decimal)pow10[precision];
            return (decimal)(Math.Truncate(x * m) / m);
        }
        
    }

    

    
}
