using System.Collections.Generic;
using System.IO;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The argument set for yagen
    /// </summary>
    public class YagenArguments
    {
        /// <summary>
        /// The input directory
        /// </summary>
        public DirectoryInfo Input { get; set; }
        
        /// <summary>
        /// The output directory
        /// </summary>
        public DirectoryInfo Output { get; set; }
        
        /// <summary>
        /// The manifest file
        /// </summary>
        public FileInfo Manifest { get; set; }
        
        /// <summary>
        /// The goals to execute
        /// </summary>
        public IEnumerable<string> Goals { get; set; }
    }
}