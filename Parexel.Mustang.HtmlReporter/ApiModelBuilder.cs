using System.Linq;
using NUnit.Framework;

namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Build model from data received from TestContext
    /// </summary>
    public class ApiModelBuilder : BaseModelBuilder
    {
        /// <summary>
        /// Take data from text context and build model
        /// </summary>
        /// <param name="context">Test context</param>
        /// <returns>Model based on current test context</returns>
        public override TestReportModel GetModel(TestContext context)
        {
            var model = new TestReportModel();
            var attributes = GetMethod(context).CustomAttributes;
            foreach(var property in properties)
            {
                var currentAttributes = attributes.Where(attribute => attribute.AttributeType.Name.StartsWith(property.Name));
                if (currentAttributes.Count() != 0)
                {
                    property.SetValue(model, currentAttributes
                        .Select(attribute => attribute.ConstructorArguments.Select(argument => argument.Value.ToString()).Aggregate((first, second) => $"{first} {second}"))
                        .Distinct().Aggregate((first, second) => $"{first} <br> {second}"));
                }
            }
            SetFeatureInfo(context, model);
            SetValuesFromContext(context, model);
            return model;
        }

        /// <summary>
        /// Get Feature attribute and set data to the model
        /// </summary>
        /// <param name="context">Test context</param>
        /// <param name="model">Model</param>
        public void SetFeatureInfo(TestContext context, TestReportModel model)
        {
            model.Feature = BaseModelBuilder.AssemblyWithTests.GetType($"{context.Test.ClassName}").CustomAttributes
                .Where(attribute => attribute.AttributeType.Name == "FeatureAttribute").FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value.ToString();
        }
    }
}
