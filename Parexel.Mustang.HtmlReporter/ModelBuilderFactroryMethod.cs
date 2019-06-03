namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Creates model builder according to test type
    /// </summary>
    public static class ModelBuilderFactroryMethod
    {
        /// <summary>
        /// Return model builder
        /// </summary>
        /// <param name="testType">Test type</param>
        /// <returns>Model builder</returns>
        public static BaseModelBuilder GetModelBuilder(TestType testType)
        {
            if (testType == TestType.Api)
            {
                return new ApiModelBuilder();
            }
            else return new SpecFlowModelBuilder();
        }
    }
}
