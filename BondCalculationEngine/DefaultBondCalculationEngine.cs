using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondCalculationEngine
{
    public class DefaultBondCalculationEngine:IBondCalculationEngine
    {
        #region IBondCalculationEngine
        decimal IBondCalculationEngine.CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate)
        {
            try
            {
                return PRICE(yearsToMaturity, couponRate, discountRate, faceValue, frequency);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        decimal IBondCalculationEngine.CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue)
        {
            try
            {
                return YIELD(yearsToMaturity, couponRate, presentValue, faceValue, frequency);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Private Methods

        private decimal PowerDecimal(decimal num, decimal power)
        {
            decimal res = 1m;
            for (int i = 1; i <= power; i++)
            {
                res = res * num;
            }
            return res;
        }

        private decimal Solve(Func<decimal, decimal> function, Func<decimal, decimal> derivativeFunction, int iterations, decimal startingValue = 0m)
        {
            decimal x = startingValue;
            for (int i = 0; i < iterations; i++)
            {
                x -= function(x) / derivativeFunction(x);
            }
            return x;
        }

        private decimal PRICE(int yearsToMaturity, decimal rate, decimal yld, decimal redemption,int frequency)
        {

            decimal presentValueOfRedemption = redemption / PowerDecimal(1 + yld / frequency, yearsToMaturity);
            decimal presentValueOfCouponPayments = 0;
            for (int k = 1; k <= yearsToMaturity; k++)
            {
                presentValueOfCouponPayments += (redemption * rate / frequency) / PowerDecimal(1 + yld / frequency, k);
            }

            return presentValueOfRedemption + presentValueOfCouponPayments;
        }


        private decimal FirstDerivativeOfPRICE(int yearsToMaturity, decimal rate, decimal yld,decimal redemption, int frequency)
        {


            decimal presentValueOfRedemption = redemption * (1 - yearsToMaturity) * PowerDecimal(1 + yld / frequency, -yearsToMaturity);
            decimal presentValueOfCouponPayments = 0;
            for (int k = 1; k <= yearsToMaturity; k++)
            {
                presentValueOfCouponPayments += (redemption * (1 - k) * rate * PowerDecimal(1 + yld / frequency, -k)) / PowerDecimal(frequency, 2);
            }

            return presentValueOfRedemption + presentValueOfCouponPayments;
        }
        private decimal YIELD(int yearsToMaturity, decimal rate, decimal pr, decimal redemption,int frequency)
        {
            return Solve(
                x => PRICE(yearsToMaturity, rate, x, redemption, frequency) - pr,
                y => FirstDerivativeOfPRICE(yearsToMaturity, rate, y, redemption, frequency), 100);
        }

        #endregion



    }
}
