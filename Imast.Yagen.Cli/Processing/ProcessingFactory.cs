namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The processing module factory
    /// </summary>
    public class ProcessingFactory
    {
        /// <summary>
        /// Gets the evaluator based on name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public virtual IYamlEvaluator GetEvaluator(string name)
        {
            return name.ToLowerInvariant() switch
            {
                "sh" => new ShellYamlEvaluator(),
                _ => new NopYamlEvaluator()
            };
        }

        /// <summary>
        /// Gets the yaml operator based on the name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public virtual IYamlOperator GetOperator(string name)
        {
            return name.ToLowerInvariant() switch
            {
                "remove" => new RemoveYamlOperator(),
                "yq-mx" => new YqMergeYamlOperator(),
                _ => new ReplaceYamlOperator()
            };
        }
    }
}