using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;

namespace Imast.Yagen.Cli;

/// <summary>
/// The YAML Generator command
/// </summary>
public class YagenRootCommand : RootCommand
{
    /// <summary>
    /// Yet another YAML Generator
    /// </summary>
    /// <param name="handler">The handler instance</param>
    public YagenRootCommand(YagenHandler handler) : base("Yet Another (YAML) Generator")
    {
        // add input directory option
        this.AddOption(new Option<DirectoryInfo>(["-i", "--input"], () => new DirectoryInfo(Directory.GetCurrentDirectory()), "The input directory").ExistingOnly());
            
        // add output directory option
        this.AddOption(new Option<DirectoryInfo>(["-o", "--output"], () => new DirectoryInfo(Directory.GetCurrentDirectory()), "The output directory").ExistingOnly());
            
        // add manifest file location
        this.AddOption(new Option<FileInfo>(["-m", "--manifest"], "The manifest file location").ExistingOnly());
        
        // add debug attribute
        this.AddOption(new Option<bool>(["-d", "--debug"], "Enable debug logs"));
            
        // add goals argument
        this.AddArgument(new Argument<IEnumerable<string>>("goals", "The list of goals to execute"));
            
        // assign the handler
        this.Handler = CommandHandler.Create<YagenArguments>(handler.Execute);
    }
}