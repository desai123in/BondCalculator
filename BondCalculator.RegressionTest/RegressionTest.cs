using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BondCalculator.Model;
using BondCalculator.Common;
using BondCalculationEngine;
using System.Collections.Generic;
using CsvHelper;
using System.IO;

namespace BondCalculator.RegressionTest
{
    [TestClass]
    public class RegressionTest
    {
        [TestMethod]
        [DeploymentItem("YieldFromPV.csv")]
        public void ProcessYieldFromPVExpectation()
        {
            List<BondCalculatorModel> inputs = ReadTestInput("YieldFromPV.csv",true);

            IBondCalculationEngine engine = Factory.GetBondCalculationEngine("Default");

            foreach(var input in inputs)
            {
                decimal expectedYield = input.Yield;
                decimal actualYield = engine.CalculateYield(input.CouponRate, input.YearsToMaturity, input.Frequency, input.FaceValue, input.PresentValue);

                actualYield = CommonUtils.Truncate(actualYield, 9);
                Assert.AreEqual<decimal>(expectedYield, actualYield);
            }


        }

        [TestMethod]
        [DeploymentItem("PVFromYield.csv")]
        public void ProcessPVFromYieldExpectation()
        {
            List<BondCalculatorModel> inputs = ReadTestInput("PVFromYield.csv",false);

            IBondCalculationEngine engine = Factory.GetBondCalculationEngine("Default");

            foreach (var input in inputs)
            {
                decimal expectedPresentValue = input.PresentValue;
                decimal actualPresentValue = engine.CalculatePresentValue(input.CouponRate, input.YearsToMaturity, input.Frequency, input.FaceValue,input.Yield);

                actualPresentValue = CommonUtils.Truncate(actualPresentValue, 7);
                Assert.AreEqual<decimal>(expectedPresentValue, actualPresentValue);
            }


        }


        private List<BondCalculatorModel> ReadTestInput(string testfile,bool expectYield)
        {            
            List<BondCalculatorModel> inputs = new List<BondCalculatorModel>();

            using (StreamReader reader = File.OpenText(testfile))
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read())
                {
                    BondCalculatorModel model = new BondCalculatorModel();

                    model.CouponRate = csv.GetField<decimal>(0);
                    model.YearsToMaturity = csv.GetField<int>(1);
                    model.Frequency = csv.GetField<int>(2);
                    model.FaceValue = csv.GetField<decimal>(3);
                    if (expectYield)
                    {
                        model.PresentValue = csv.GetField<decimal>(4);
                        model.Yield = csv.GetField<decimal>(5);
                    }
                    else
                    {
                        model.Yield = csv.GetField<decimal>(4);
                        model.PresentValue = csv.GetField<decimal>(5);                        
                    }
                    

                    inputs.Add(model);
                } 
    
            }
            return inputs;            
            
        }

    }
}
