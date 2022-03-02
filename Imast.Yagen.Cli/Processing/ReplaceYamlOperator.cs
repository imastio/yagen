using System.IO;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The replacing operator implementation
    /// </summary>
    public class ReplaceYamlOperator : IYamlOperator
    {
        /// <summary>
        /// Applies the yaml operator within the context
        /// </summary>
        /// <param name="context">The context of operation</param>
        /// <returns></returns>
        public async Task<YamlOperationResult> Apply(YamlOperationContext context)
        {
            // read the content of source file
            var content = await File.ReadAllTextAsync(context.EvaluatedSourceFile.FullName);

            // get the directory
            var outputFileDirectory = Path.GetDirectoryName(context.OutputFilePath) ?? string.Empty;

            // make sure directories are created
            Directory.CreateDirectory(outputFileDirectory);

            // write all content to replace regardless of previous version 
            await File.WriteAllTextAsync(context.OutputFilePath, content);

            // the result output
            return new YamlOperationResult
            {
                OutputFile = new FileInfo(context.OutputFilePath)
            };
        }
    }
}