using System.Threading.Tasks;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The Yagen handler implementation
    /// </summary>
    public class YagenHandler
    {
        /// <summary>
        /// The implementation of yagen handler
        /// </summary>
        /// <param name="arguments">The parsed arguments</param>
        /// <returns></returns>
        public Task<int> Execute(YagenArguments arguments)
        {
            return Task.FromResult(0);
        }
    }
}