using System.Data;

namespace RollLabelProdPack.Library.Data
{
    /// <summary>
    /// Represents the output of a service operation.
    /// </summary>
    public class ServiceOutput
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result set of the service operation.
        /// </summary>
        public DataSet ResultSet { get; set; }

        /// <summary>
        /// Gets or sets the return parameters of the service operation.
        /// </summary>
        public string ReturnParameters { get; set; }

        /// <summary>
        /// Gets or sets the exception message if the service operation encountered an error.
        /// </summary>
        public string ServiceException { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the success status of the service operation.
        /// </summary>
        public bool SuccessFlag { get; set; }

        /// <summary>
        /// Gets or sets the return value of the service operation.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets the array of return values of the service operation.
        /// </summary>
        public object[] ReturnValues { get; set; }

        /// <summary>
        /// Gets or sets the name of the method that produced the service output.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the call stack information when an exception occurred.
        /// </summary>
        public string CallStack { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class.
        /// </summary>
        public ServiceOutput() { }

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class with a specified success flag.
        /// </summary>
        public ServiceOutput(bool successFlag)
        {
            SuccessFlag = successFlag;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class with a specified success flag and return value.
        /// </summary>
        public ServiceOutput(bool successFlag, object webValue)
        {
            SuccessFlag = successFlag;
            ReturnValue = webValue;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class with a specified exception message.
        /// </summary>
        public ServiceOutput(string webException)
        {
            SuccessFlag = false;
            ServiceException = webException;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class with a specified exception message and method name.
        /// </summary>
        public ServiceOutput(string webException, string methodName)
        {
            SuccessFlag = false;
            ServiceException = webException;
            MethodName = methodName;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceOutput class with a specified exception message, method name, and call stack.
        /// </summary>
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
