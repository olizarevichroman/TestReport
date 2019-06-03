using System;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Provides an ability to write data from model to html file
    /// </summary>
    public class Reporter
    {
        /// <summary>
        /// Store number of current test
        /// </summary>
        private static int _testNumber = 0;

        /// <summary>
        /// Check is current test context is first on the assembly
        /// </summary>
        private static bool _isFirstOnAssembly = true;

        /// <summary>
        /// Path to the output report file
        /// </summary>
        private static string _path;

        /// <summary>
        /// Model builder to build model from test context
        /// </summary>
        private BaseModelBuilder _modelBuilder;

        /// <summary>
        /// Store data with all data on the current feature
        /// </summary>
        private StringBuilder _featureDataBlock = new StringBuilder();

        /// <summary>
        /// Synchronization object
        /// </summary>
        private static readonly object _assemblyLocker = new object();

        private static object _writeLocker = new object();
        /// <summary>
        /// Represent format string to construct table
        /// </summary>
        private readonly string _table = "<table border=\"1\" style=\"border-collapse:collapse; width:100%\">";

        /// <summary>
        /// Represent format string to construct table rows
        /// </summary>
        private readonly string _row = "<tr>{0}</tr>";
  
        /// <summary>
        /// Represent format string to construct table header row
        /// </summary>
        private readonly string _headerRow = "<tr style=\"font-weight:bold\">{0}</tr>";

        /// <summary>
        /// Represent format string to construct table cells
        /// </summary>
        private readonly string _testItem = "<td style=\"padding: 10px\">{0}</td>";

        /// <summary>
        /// Properties of current test report model
        /// </summary>
        private static readonly PropertyInfo[] _properties = typeof(TestReportModel).GetProperties();

        /// <summary>
        /// Initializes a new instance of the <see cref="Reporter"/> class.
        /// Constructor check is file exist and create it if not, get test report model properties and write headers
        /// </summary>
        public Reporter(TestType testType = TestType.Api)
        {         
            _modelBuilder = ModelBuilderFactroryMethod.GetModelBuilder(testType);
        }

        /// <summary>
        /// Append data from the test context to the output report file
        /// </summary>
        /// <param name="testContext"></param>
        /// <returns></returns>
        public Reporter Append(TestContext testContext)
        {
            lock (_assemblyLocker)
            {
                if (_isFirstOnAssembly)
                {
                    _isFirstOnAssembly = false;
                    _path = $@"{testContext.TestDirectory}\Report.html";
                    WriteHeaders();
                }
            }          
            var model = _modelBuilder.GetModel(testContext);
            WriteData(model);
            return this;
        }
        
        /// Take all information from model
        /// </summary>
        /// <param name="model">Model to write</param>
        /// <returns>Model view</returns>
        private string GetView(TestReportModel model)
        {
            StringBuilder builder = new StringBuilder();
            Array.ForEach(_properties, (property) => builder.Append(GetFormat(_testItem, property.GetValue(model))));         
            return GetFormat(_row, builder.ToString()).ToString();
        }

        /// <summary>
        /// Get string by pattern and values
        /// </summary>
        /// <param name="format">Pattern</param>
        /// <param name="value">Values ti insert</param>
        /// <returns>Formatted string</returns>
        private string GetFormat(string format, params object[] value) => string.Format(format, value);

        /// <summary>
        /// Write data from model to the output report file
        /// </summary>
        /// <param name="model">Model to write</param>
        private void WriteData(TestReportModel model)
        {
            lock (_writeLocker)
            {
                model.N = ++_testNumber;
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.Write(GetView(model));
                }
            }
        }

        /// <summary>
        /// Write data to the output report file
        /// </summary>
        /// <param name="data">Data to write</param>
        private void WriteData(string data)
        {
            lock (_writeLocker)
            {
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.Write(data);
                }
            }
        }
        /// <summary>
        /// Write headers from model to the output report file
        /// </summary>
        private void WriteHeaders()
        {              
                StringBuilder builder = new StringBuilder();
                Array.ForEach(_properties, (property) => builder.Append(GetFormat(_testItem, property.Name)));
                string headers = GetFormat(_headerRow, builder.ToString());
                WriteData(_table + headers);
        }
    }
}
