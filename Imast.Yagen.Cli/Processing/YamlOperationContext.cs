using System.IO;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The yaml processing operation context
    /// </summary>
    public class YamlOperationContext
    {
        /// <summary>
        /// The existing file
        /// </summary>
        public FileInfo ExistingFile { get; set; }

        /// <summary>
        /// The goal for evaluation
        /// </summary>
        public YagenGoal Goal { get; set; }
        
        /// <summary>
        /// The source file
        /// </summary>
        public FileInfo OriginalSourceFile { get; set; }

        /// <summary>
        /// The evaluated source file
        /// </summary>
        public FileInfo EvaluatedSourceFile { get; set; }

        /// <summary>
        /// The local directory path
        /// </summary>
        public string LocalDirectoryPath { get; set; }

        /// <summary>
        /// The resolved file name
        /// </summary>
        public string ResolvedFileName { get; set; }

        /// <summary>
        /// The output directory root
        /// </summary>
        public DirectoryInfo OutputDirectory { get; set; }

        /// <summary>
        /// The output file path
        /// </summary>
        public string OutputFilePath { get; set; }
    }
}