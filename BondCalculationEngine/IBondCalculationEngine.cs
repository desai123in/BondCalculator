using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondCalculationEngine
{
    //Base interface, takes basic value types as parameters to not link it to any spcific model
    public interface IBondCalculationEngine
    {
        decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
        decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);

    }
}
