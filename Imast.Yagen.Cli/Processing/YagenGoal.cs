using System.Collections.Generic;
using System.IO;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The resolved goal layers for processing
    /// </summary>
    public class YagenGoal
    {
        /// <summary>
        /// The goal name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The goal options
        /// </summary>
        public YagenGoalOptions Options { get; set; }

        /// <summary>
        /// The environment files
        /// </summary>
        public List<FileInfo> EnvFiles { get; set; }

        /// <summary>
        /// The goal layers
        /// </summary>
        public List<DirectoryInfo> Layers { get; set; }
    }
}