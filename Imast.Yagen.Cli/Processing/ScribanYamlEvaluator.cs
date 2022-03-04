using System.IO;
using System.Threading.Tasks;
using Scriban;
using Scriban.Runtime;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The scriban-based yaml template evaluator
    /// </summary>
    public class ScribanYamlEvaluator : IYamlEvaluator
    {
        /// <summary>
        /// The logic of YAML evaluation
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns></returns>
        public async Task<YamlEvaluationResult> Evaluate(YamlEvaluationContext context)
        {
            // get the temp file name
            var temp = Path.GetTempFileName();

            // read all source text
            var source = await File.ReadAllTextAsync(context.SourceFile.FullName);

            // the template context
            var templateContext = new TemplateContext();
            
            // add all the values
            context.ValuesCollection?.ForEach(values =>
            {
                // create a new script object
                var scriptObject = new ScriptObject();

                // populate with values dictionary
                scriptObject.Import(values);

                // add next script object
                templateContext.PushGlobal(scriptObject);
            });

            // the parsed template
            var parsed = Template.Parse(source);

            // evaluate data 
            var evaluated = await parsed.RenderAsync(templateContext);

            // write all the evaluated content into a temporary file
            await File.WriteAllTextAsync(temp, evaluated);

            // output the temporary file with all the content
            return new YamlEvaluationResult
            {
                OutputFile = new FileInfo(temp)
            };
        }
    }
}