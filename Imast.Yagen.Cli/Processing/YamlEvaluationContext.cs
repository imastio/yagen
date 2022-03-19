using System.Collections.Generic;
using System.IO;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The YAML Evaluation context
    /// </summary>
    public class YamlEvaluationContext
    {
        /// <summary>
        /// The goal for evaluation
        /// </summary>
        public YagenGoal Goal { get; set; }

        /// <summary>
        /// The source file
        /// </summary>
        public FileInfo SourceFile { get; set; }
        
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

        /// <summary>
        /// The values collection
        /// </summary>
        public IDictionary<object, object> Values { get; set; }
    }
}
