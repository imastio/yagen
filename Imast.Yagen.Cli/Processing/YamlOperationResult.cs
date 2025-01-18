using System.IO;

namespace Imast.Yagen.Cli.Processing;

/// <summary>
/// The YAML Operation result
/// </summary>
public class YamlOperationResult
{
    /// <summary>
    /// The output file
    /// </summary>
    public FileInfo OutputFile { get; set; }
}