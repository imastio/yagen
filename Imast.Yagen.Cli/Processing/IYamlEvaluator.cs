using System.Threading.Tasks;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The YAML Evaluation interface
    /// </summary>
    public interface IYamlEvaluator
    {
        /// <summary>
        /// Evaluates the YAML within given context
        /// </summary>
        /// <param name="context">The evaluation context</param>
        /// <returns></returns>
        Task<YamlEvaluationResult> Evaluate(YamlEvaluationContext context);
    }
}