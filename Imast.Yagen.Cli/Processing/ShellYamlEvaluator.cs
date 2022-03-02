using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The shell-based yaml evaluator
    /// </summary>
    public class ShellYamlEvaluator : IYamlEvaluator
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

            // the shell content builder
            var shellContentBuilder = new StringBuilder();

            // add shebang
            shellContentBuilder.AppendLine("#!/usr/bin/env sh");
            
            // error exit mode
            shellContentBuilder.AppendLine("set -o errexit -o pipefail");

            // start content
            shellContentBuilder.AppendLine("cat <<EOF");

            // add yaml content
            shellContentBuilder.AppendLine(source);

            // end the content
            shellContentBuilder.AppendLine("EOF");

            // write all the content into a temporary file
            await File.WriteAllTextAsync(temp, shellContentBuilder.ToString());
            
            // the evaluation process start info
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "/bin/sh",
                Arguments = temp
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

            // write all the evaluated content into a temporary file
            await File.WriteAllTextAsync(temp, result);

            // output the temporary file with all the content
            return new YamlEvaluationResult
            {
                OutputFile = new FileInfo(temp)
            };
        }
    }
}