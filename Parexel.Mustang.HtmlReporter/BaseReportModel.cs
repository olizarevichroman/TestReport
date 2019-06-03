using System;
using System.Collections.Generic;
using System.Text;

namespace Parexel.Mustang.HtmlReporter
{
    public class BaseReportModel
    {
        public int N { get; set; }

        public string Version { get; set; }

        public string TestName { get; set; }

        public string BuildNumber { get; set; }

        public string ExecutionDate { get; set; }

        public string ExecutionStatus { get; set; }

        public string Comments { get; set; }
    }
}
