using System;
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
        private int yieldPrecision;
        private int presentValuePrecision;

        /// <summary>
        /// Initialize test settings.
        /// </summary>
        [TestInitialize]
        public void Init()
        {            
            target = UnityFactory.Resolve<IBondCalculationEngine>("Default");
            yieldPrecision = BondCalculator.Common.AppConfig.Instance.YieldPrecision;
            presentValuePrecision = BondCalculator.Common.AppConfig.Instance.PresentValuePrecision;
        }

        #region CalculateYield Tests
        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultForBaseCase()
        {
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 1000m;            
            #endregion //Input

            decimal expected = 0.1000000000m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, yieldPrecision);

            Assert.AreEqual<decimal>(expected, actual);


        }
        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultForNegativeYield()
        {
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 1600m;
            #endregion //Input

            decimal expected = -0.014744529m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, yieldPrecision);

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
            actual = CommonUtils.Truncate(actual, yieldPrecision);
            
            Assert.AreEqual<decimal>(expected, actual);


        }

        [TestMethod]
        public void CheckThatCalculateYieldMethodReturnsExpectedResultWhenPresentValueGreaterThenFaceValue()
        {
            #region Input
            decimal couponRate = 0.10m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal presentValue = 1100.0000m;
            #endregion //Input

            decimal expected = 0.075266056m;
            decimal actual = target.CalculateYield(couponRate, yearsToMaturity, frequency, faceValue, presentValue);
            actual = CommonUtils.Truncate(actual, yieldPrecision);

            Assert.AreEqual<decimal>(expected, actual);

        }
        #endregion //CalculateYield Tests

        #region CalculatePresentValue Tests
        [TestMethod]
        public void CheckThatCalculatePresentValueMethodReturnsExpectedResultForBaseCase()
        {
            #region Input
            decimal couponRate = 0.100000m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal yield = 0.100000000m;
            #endregion //Input

            decimal expected = 1000.0000000m;
            decimal actual = target.CalculatePresentValue(couponRate, yearsToMaturity, frequency, faceValue, yield);
            actual = CommonUtils.Truncate(actual, presentValuePrecision);

            Assert.AreEqual<decimal>(expected, actual);
        }

        [TestMethod]
        public void CheckThatCalculatePresentValueMethodReturnsExpectedResultForWhenYieldNotEqualCoupon()
        {
            #region Input
            decimal couponRate = 0.100000m;
            int yearsToMaturity = 5;
            int frequency = 1;
            decimal faceValue = 1000m;
            decimal yield = 0.15m;
            #endregion //Input

            decimal expected = 832.3922450m;
            decimal actual = target.CalculatePresentValue(couponRate, yearsToMaturity, frequency, faceValue, yield);
            actual = CommonUtils.Truncate(actual, presentValuePrecision);

            Assert.AreEqual<decimal>(expected, actual);
        }
        
        #endregion //CalculatePresentValue Tests
        
    }
}
