using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The YAML Generator command
    /// </summary>
    public class YagenRootCommand : RootCommand
    {
        /// <summary>
        /// The yagen handler
        /// </summary>
        private readonly YagenHandler handler;

        /// <summary>
        /// Yet another YAML Generator
        /// </summary>
        /// <param name="handler">The handler instance</param>
        public YagenRootCommand(YagenHandler handler) : base("Yet Another (YAML) Generator")
        {
            // keep handler
            this.handler = handler;
            
            // add input directory option
            this.AddOption(new Option<DirectoryInfo>(new[] { "-i", "--input" }, () => new DirectoryInfo(Directory.GetCurrentDirectory()), "The input directory").ExistingOnly());
            
            // add output directory option
            this.AddOption(new Option<DirectoryInfo>(new[] { "-o", "--output" }, () => new DirectoryInfo(Directory.GetCurrentDirectory()), "The output directory").ExistingOnly());
            
            // add recipe file location
            this.AddOption(new Option<FileInfo>(new []{ "-r", "--recipe" }, "The recipe file location").ExistingOnly());
            
            // add goals argument
            this.AddArgument(new Argument<IEnumerable<string>>("goals", "The list of goals to execute"));
            
            // assign the handler
            this.Handler = CommandHandler.Create<YagenArguments>(handler.Execute);
        }
    }
}