using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The yq-based merging operator implementation
    /// </summary>
    public class YqMergeYamlOperator : IYamlOperator
    {
        /// <summary>
        /// Applies the yaml operator within the context
        /// </summary>
        /// <param name="context">The context of operation</param>
        /// <returns></returns>
        public async Task<YamlOperationResult> Apply(YamlOperationContext context)
        {
            // get the directory
            var outputFileDirectory = Path.GetDirectoryName(context.OutputFilePath) ?? string.Empty;

            // make sure directories are created
            Directory.CreateDirectory(outputFileDirectory);

            // make sure file is there
            if (!File.Exists(context.ExistingFile.FullName))
            {
                throw new YagenException($"The file {context.ExistingFile.FullName} does not exist to merge");
            }

            // the evaluation process start info
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "yq",
                ArgumentList = { "eval-all",  "select(fi == 0) * select(fi == 1)", context.ExistingFile.FullName, context.EvaluatedSourceFile.FullName}
            };

            // create an evaluation process
            var process = new Process
            {
                StartInfo = startInfo
            };

            // start the process
            process.Start();

            // read the standard output 
            var result = await process.StandardOutput.ReadToEndAsync();

            // wait for completion
            await process.WaitForExitAsync();

            // write all content to replace regardless of previous version 
            await File.WriteAllTextAsync(context.OutputFilePath, result);

            // the result output
            return new YamlOperationResult
            {
                OutputFile = new FileInfo(context.OutputFilePath)
            };
        }
    }
}