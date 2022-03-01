using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// Yet Another YAML Generator
    /// </summary>
    public class Yagen
    {
        /// <summary>
        /// Generate YAML based on your recipe
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static Task Main(string[] args)
        {
            // create new handler
            var handler = new YagenHandler();
            
            // instantiate only command parser
            return new YagenRootCommand(handler).Parse(args).InvokeAsync();
        }
    }
}