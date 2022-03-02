using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The yaml operation interface
    /// </summary>
    public interface IYamlOperator
    {
        /// <summary>
        /// Applies the yaml operator within the context
        /// </summary>
        /// <param name="context">The context of operation</param>
        /// <returns></returns>
        Task<YamlOperationResult> Apply(YamlOperationContext context);
    }
}