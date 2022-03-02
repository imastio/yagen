using System.IO;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The removing operator implementation
    /// </summary>
    public class RemoveYamlOperator : IYamlOperator
    {
        /// <summary>
        /// Applies the yaml operator within the context
        /// </summary>
        /// <param name="context">The context of operation</param>
        /// <returns></returns>
        public Task<YamlOperationResult> Apply(YamlOperationContext context)
        {
            // make sure output file gets deleted if exists
            if (File.Exists(context.OutputFilePath))
            {
                File.Delete(context.OutputFilePath);
            }

            // the result output
            return Task.FromResult(new YamlOperationResult
            {
                OutputFile = null
            });
        }
    }
}