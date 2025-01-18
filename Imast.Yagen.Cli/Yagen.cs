using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Imast.Yagen.Cli;

/// <summary>
/// Yet Another YAML Generator
/// </summary>
public class Yagen
{
    /// <summary>
    /// Generate YAML based on your recipe
    /// </summary>
    /// <param name="args">The command line arguments</param>
    public static async Task Main(string[] args)
    {
        // create new handler
        var handler = new YagenHandler();

        // get parsed arguments
        var parsed = new YagenRootCommand(handler).Parse(args);

        try
        {
            // invoke parsed commands
            await parsed.InvokeAsync();
        }
        catch (YagenException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {e.Message}");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unknown Error: {e.Message}");
            Console.ResetColor();
        }
    }
}