using NUnit.Framework;
using System;
using System.Reflection;

namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Base class for model builders
    /// </summary>
    public abstract class BaseModelBuilder
    {
        /// <summary>
        /// Stores properties of test report model
        /// </summary>
        protected PropertyInfo[] properties { get;} = typeof(TestReportModel).GetProperties();

        /// <summary>
        /// Assembly locker
        /// </summary>
        private static object assemblyLocker = new object();

        /// <summary>
        /// Stores assembly with tests
        /// </summary>
        public static Assembly AssemblyWithTests { get; set; }

        /// <summary>
        /// Build and return model from test context
        /// </summary>
        /// <param name="context">Test context</param>
        /// <returns>Test report model</returns>
        public abstract TestReportModel GetModel(TestContext context);

        /// <summary>
        /// Set values, which can be received without reflection
        /// </summary>
        /// <param name="context">Test context</param>
        /// <param name="model">Test report model</param>
        protected void SetValuesFromContext(TestContext context, TestReportModel model)
        {
            model.ExecutionStatus = context.Result.Outcome.Status.ToString();
            model.Version = "1.0";
            model.ExecutionDate = DateTime.Now.ToLongDateString();
            model.BuildNumber = Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER");
            model.TestName = context.Test.Name;
        }

        /// <summary>
        /// Get method which represent current test
        /// </summary>
        /// <param name="context">Test context</param>
        /// <returns>Test method</returns>
        protected MethodInfo GetMethod(TestContext context)
        {
            lock (assemblyLocker)
            {
                if (AssemblyWithTests == null)
                {
                    var testExecutionContext = typeof(TestContext).GetField("_testExecutionContext", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
                    var testExecutionContextValue = (NUnit.Framework.Internal.TestExecutionContext)testExecutionContext.GetValue(context);
                    var currentTest = testExecutionContextValue.CurrentTest;
                    var declaringTypeInfo = currentTest.GetType().GetField("DeclaringTypeInfo", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
                    var declaringTypeInfoValue = (NUnit.Framework.Internal.TypeWrapper)declaringTypeInfo.GetValue(currentTest);
                    AssemblyWithTests = declaringTypeInfoValue.Assembly;
                }
            }
            return AssemblyWithTests.GetType(context.Test.ClassName).GetMethod(context.Test.MethodName);
        }
    }
}
