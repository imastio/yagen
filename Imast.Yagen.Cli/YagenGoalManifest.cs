using System.Collections.Generic;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The Yagen Goal manifest
    /// </summary>
    public class YagenGoalManifest
    {
        /// <summary>
        /// The goal name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The env files
        /// </summary>
        public List<string> EnvFiles { get; set; }

        /// <summary>
        /// The values file
        /// </summary>
        public List<string> ValueFiles { get; set; }

        /// <summary>
        /// The goal options
        /// </summary>
        public YagenOptionsManifest Options { get; set; }

        /// <summary>
        /// The layer directories for processing
        /// </summary>
        public List<string> Layers { get; set; }
    }
}