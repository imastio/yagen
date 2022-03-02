using System.Collections.Generic;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The Yagen Manifest definition
    /// </summary>
    public class YagenManifest
    {
        /// <summary>
        /// The yagen global options
        /// </summary>
        public YagenOptionsManifest Options { get; set; }

        /// <summary>
        /// The yagen goals
        /// </summary>
        public List<YagenGoalManifest> Goals { get; set; }
    }
}