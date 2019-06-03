using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Parexel.Mustang.HtmlReporter
{
    /// <summary>
    /// Class to build model from UI tests (using SpecFlow)
    /// </summary>
    public class SpecFlowModelBuilder : BaseModelBuilder
    {
        /// <summary>
        /// Take data from text context and build model
        /// </summary>
        /// <param name="context">Test context</param>
        /// <returns>Model based on current test context</returns>
        public override TestReportModel GetModel(TestContext context)
        {
            var model = new TestReportModel();
            var attributes = GetMethod(context).CustomAttributes.Where(attribute => attribute.AttributeType == typeof(CategoryAttribute));
            
            var attributesValues = attributes.Select(attribute => attribute.ConstructorArguments.FirstOrDefault().Value.ToString());
            IEnumerable<string> currentPropertyValuesSequence;
            foreach (var property in properties)
            {
                currentPropertyValuesSequence = attributesValues.Where(attributeValue => attributeValue[0] == property.Name[0]);
                if (currentPropertyValuesSequence.Count() != 0)
                {
                    property.SetValue(model, currentPropertyValuesSequence.Aggregate((current, next) => $"{current} {next}"));
                }
            }
            SetValuesFromContext(context, model);
            return model;
        }
    }
}
