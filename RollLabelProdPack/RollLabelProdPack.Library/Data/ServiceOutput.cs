using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Data
{
    public class ServiceOutput
    {

        #region properties
        public DataSet ResultSet { get; set; }
        public string ReturnParameters { get; set; }
        public string ServiceException { get; set; }
        public bool SuccessFlag { get; set; }
        public object ReturnValue { get; set; }
        public object[] ReturnValues { get; set; }
        public string MethodName { get; set; }
        public string CallStack { get; set; }
        #endregion


        #region constructors

        /// <summary></summary>
        public ServiceOutput() { }

        /// <summary></summary>
        public ServiceOutput(bool successFlag)
        {
            SuccessFlag = successFlag;
        }

        /// <summary></summary>
        public ServiceOutput(bool successFlag, object webValue)
        {
            SuccessFlag = successFlag;
            ReturnValue = webValue;
        }

        /// <summary></summary>
        public ServiceOutput(string webException)
        {
            SuccessFlag = false;
            ServiceException = webException;
        }

        /// <summary></summary>
        public ServiceOutput(string webException, string methodName)
        {
            SuccessFlag = false;
            ServiceException = webException;
            MethodName = methodName;
        }

        /// <summary></summary>
        public ServiceOutput(string webException, string methodName, string callStack)
        {
            SuccessFlag = false;
            ServiceException = webException;
            MethodName = methodName;
            CallStack = callStack;
        }
        #endregion

    }
}
