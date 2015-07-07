using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BondCalculator.Common
{
    //this is singleton class, loads appsettings on first use.
    public sealed class AppConfig
    {
        #region Private Static Fields
        static readonly AppConfig instance = new AppConfig();
        #endregion //Private Static Fields

        #region Private Fields
        private int presentValuePrecision = 7;
        private int yieldPrecision = 9;
        private int yieldSolverIterations = 1200;
        private int maxMaturityYearsAllowed = 500;
        #endregion //Private Fields

        #region Public Properties
        public int PresentValuePrecision
        {
            get { return presentValuePrecision; }

        }        

        public int YieldPrecision
        {
            get { return yieldPrecision; }

        }        

        public int YieldSolverIterations
        {
            get { return yieldSolverIterations; }

        }       

        public int MaxMaturityYearsAllowed
        {
            get { return maxMaturityYearsAllowed; }
        }

        #endregion //Public Properties

        #region Static Constructor
        static AppConfig()
        {

        }
        #endregion //Static Constructor

        #region Private Constructor
        AppConfig()
        {
            InitializeConfig();
        }
        #endregion //Private Constructorx 

        #region Public Static Fields
        public static AppConfig Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion //Public Static Fields

        #region Private Methods
        private void InitializeConfig()
        {
            try
            {
                //use default values if parsing fails
                if (!Int32.TryParse(ConfigurationManager.AppSettings["PresentValuePrecision"], out presentValuePrecision))
                {
                    presentValuePrecision = 7;
                    //log warning with parse issue
                }
                if (!Int32.TryParse(ConfigurationManager.AppSettings["YieldPrecision"], out yieldPrecision))
                {
                    yieldPrecision = 9;
                    //log warning with parse issue
                }
                if (!Int32.TryParse(ConfigurationManager.AppSettings["YieldSolverIterations"], out yieldSolverIterations))
                {
                    yieldSolverIterations = 1200;
                    //log warning with parse issue
                }
                if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxMaturityYearsAllowed"], out maxMaturityYearsAllowed))
                {
                    maxMaturityYearsAllowed = 150;
                    //log warning with parse issue
                }
            }
            catch (Exception e)
            {
                //log exception
            }
        }
        #endregion //Private Methods
    }
}
