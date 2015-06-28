﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BondCalculationEngine;
using BondCalculator.Common;
using System.Text;
namespace BondCalculator.UnitTests
{
    [TestClass]
    public class BondCalculationEngineTests
    {
        private IBondCalculationEngine target;
              
        [TestInitialize]
        public void Init()
        {
            target = Factory.GetBondCalculationEngine("Default");
        }

        #region CalculateYield Tests
        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultForBaseCase()
        {
            //   decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
            //   decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 1000m;            
            #endregion //Input

            decimal expected = 0.1000000000m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, 9);

            Assert.AreEqual<decimal>(expected, actual);


        }

        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultWhenYieldNotEqualToCoupon()
        {
            //   decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
            //   decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 832.4m;
            #endregion //Input

            decimal expected = 0.149997375m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, 9);
            
            Assert.AreEqual<decimal>(expected, actual);


        }

        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultWhenPresentValueGreaterThenFaceValue()
        {
            //   decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
            //   decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 1100.0000m;
            #endregion //Input

            decimal expected = 0.075266056m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, 9);

            Assert.AreEqual<decimal>(expected, actual);


        }
        #endregion //CalculateYield Tests

        #region CalculatePresentValue Tests
        [TestMethod]
        public void CheckThatCalculatePresentValueMethodReturnsExpectedResultForBaseCase()
        {
            //   decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
            //   decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);
            #region Input
            decimal couponRate = 0.100000m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal yield = 0.100000000m;
            #endregion //Input

            decimal expected = 1000.0000000m;
            decimal actual = target.CalculatePresentValue(couponRate, yearsToMaturity, frequency, faceValue, yield);
            actual = CommonUtils.Truncate(actual, 7);

            Assert.AreEqual<decimal>(expected, actual);


        }

        [TestMethod]
        public void CheckThatCalculatePresentValueMethodReturnsExpectedResultForWhenYieldNotEqualCoupon()
        {
            //   decimal CalculatePresentValue(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal discountRate);
            //   decimal CalculateYield(decimal couponRate, int yearsToMaturity, int frequency, decimal faceValue, decimal presentValue);
            #region Input
            decimal couponRate = 0.100000m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal yield = 0.15m;
            #endregion //Input

            decimal expected = 832.3922450m;
            decimal actual = target.CalculatePresentValue(couponRate, yearsToMaturity, frequency, faceValue, yield);
            actual = CommonUtils.Truncate(actual, 7);

            Assert.AreEqual<decimal>(expected, actual);


        }


        #endregion //CalculatePresentValue Tests

        #region Private Methods

      
        #endregion //Private Methods
    }
}
