﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace BondCalculationEngine
{
    public class DefaultBondCalculationEngine:IBondCalculationEngine
    {
        private int numIterations = 1200;
        public int NumIterations
        {
            get { return numIterations; }
            set { numIterations = value; }
        }

        #region IBondCalculationEngine
        decimal IBondCalculationEngine.CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate)
        {
            try
            {
               
                return Price(yearsToMaturity, couponRate, discountRate, faceValue, frequency);
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
               
                return SolveYield(yearsToMaturity, couponRate, presentValue, faceValue, frequency);
            }
            catch (Exception e)
            {
                //todo: log exception
                throw e;
            }
        }
        #endregion

        #region Private Methods

        private decimal PowerDecimal(decimal num, decimal power)
        {
            decimal res = 1.0m;
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

        private decimal Price(int yearsToMaturity, decimal rate, decimal yld, decimal faceValue, int frequency)
        {

            decimal presentValueOfRedemption = faceValue / PowerDecimal(1 + yld / frequency, yearsToMaturity);
            decimal presentValueOfCouponPayments = 0;
            for (int k = 1; k <= yearsToMaturity; k++)
            {
                presentValueOfCouponPayments += (faceValue * rate / frequency) / PowerDecimal(1 + yld / frequency, k);
            }

            return presentValueOfRedemption + presentValueOfCouponPayments;
        }


        private decimal PriceDerivative(int yearsToMaturity, decimal rate, decimal yld, decimal faceValue, int frequency)
        {


            decimal presentValueOfRedemption = faceValue * (1 - yearsToMaturity) * PowerDecimal(1 + yld / frequency, -yearsToMaturity);
            decimal presentValueOfCouponPayments = 0;
            for (int k = 1; k <= yearsToMaturity; k++)
            {
                presentValueOfCouponPayments += (faceValue * (1 - k) * rate * PowerDecimal(1 + yld / frequency, -k)) / PowerDecimal(frequency, 2);
            }

            return presentValueOfRedemption + presentValueOfCouponPayments;
        }
        private decimal SolveYield(int yearsToMaturity, decimal rate, decimal pr, decimal faceValue,int frequency)
        {
            //twik with num iterations for solving
            
            return Solve(
                x => Price(yearsToMaturity, rate, x, faceValue, frequency) - pr,
                y => PriceDerivative(yearsToMaturity, rate, y, faceValue, frequency), numIterations);
        }

        #endregion



    }
}
