namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Represent test report model
    /// </summary>
    public class TestReportModel
    {
        public int N { get; set; }

        public string Version { get; set; }

        public string Feature { get; set; }

        public string UserStory { get; set; }

        public string Requirement { get; set; }

        public string RequirementId { get; set; }

        public string Design { get; set; }

        public string TestName { get; set; }

        public string BuildNumber { get; set; }

        public string ExecutionDate { get; set; }

        public string ExecutionStatus { get; set; }

        public string Comments { get; set; }   
    }
}
