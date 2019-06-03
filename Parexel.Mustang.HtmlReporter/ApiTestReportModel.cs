namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Represent test report model
    /// </summary>
    public class ApiTestReportModel : BaseReportModel
    {
        public string Feature { get; set; }

        public string UserStory { get; set; }

        public string Requirement { get; set; }

        public string RequirementId { get; set; }

        public string Design { get; set; } 
    }
}
