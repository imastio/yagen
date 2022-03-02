using System.IO;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The yaml evaluator that does nothing to the file
    /// </summary>
    public class NopYamlEvaluator : IYamlEvaluator
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

            // write all the content into a temporary file
            await File.WriteAllTextAsync(temp, source);

            // output the temporary file with all the content
            return new YamlEvaluationResult
            {
                OutputFile = new FileInfo(temp)
            };
        }
    }
}