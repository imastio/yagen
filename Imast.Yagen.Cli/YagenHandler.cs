using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Imast.Yagen.Cli.Processing;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Imast.Yagen.Cli
{
    /// <summary>
    /// The Yagen handler implementation
    /// </summary>
    public class YagenHandler
    {
        /// <summary>
        /// The yaml manifest file by default
        /// </summary>
        private const string DEFAULT_MANIFEST_FILENAME = "yagen.yml";

        /// <summary>
        /// Replace arrays by default or not
        /// </summary>
        private const bool DEFAULT_OPTIONS_REPLACE_ARRAYS = false;

        /// <summary>
        /// The implementation of yagen handler
        /// </summary>
        /// <param name="arguments">The parsed arguments</param>
        /// <returns></returns>
        public virtual async Task<int> Execute(YagenArguments arguments)
        {
            // the manifest file
            var manifestFile = arguments.Manifest?.FullName;

            // if manifest file is not there
            if (string.IsNullOrWhiteSpace(manifestFile))
            {
                manifestFile = Path.Combine(arguments.Input.FullName, DEFAULT_MANIFEST_FILENAME);
            }

            // check if file exists
            if (!File.Exists(manifestFile))
            {
                throw new YagenException($"Could not locate manifest file {manifestFile}");
            }

            
            // make sure input directory is there
            if (string.IsNullOrWhiteSpace(arguments.Input?.FullName) || !Directory.Exists(arguments.Input.FullName))
            {
                throw new YagenException($"The input directory {arguments.Input?.FullName} does not exist");
            }

            // make sure output directory is there
            if (string.IsNullOrWhiteSpace(arguments.Output?.FullName) || !Directory.Exists(arguments.Output.FullName))
            {
                throw new YagenException($"The output directory {arguments.Output?.FullName} does not exist");
            }

            // make sure input and output directories are not same
            if (string.Equals(arguments.Input.FullName, arguments.Output.FullName))
            {
                arguments.Output = new DirectoryInfo(Path.Combine(arguments.Output.FullName, "build"));
            }

            // build deserializer
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // try deserialize manifest
            var manifest = deserializer.Deserialize<YagenManifest>(await File.ReadAllTextAsync(manifestFile));

            // execute the concrete logic
            return await this.ExecuteImpl(arguments, manifest);
        }

        /// <summary>
        /// The execution implementation for yagen handler
        /// </summary>
        /// <param name="arguments">The arguments</param>
        /// <param name="manifest">The manifest</param>
        /// <returns></returns>
        protected virtual async Task<int> ExecuteImpl(YagenArguments arguments, YagenManifest manifest)
        {
            // the set of requested goals
            var requestedGoals = arguments.Goals?.ToList() ?? new List<string>();

            // all the defined goals
            var definedGoals = manifest.Goals?.Select(goal => goal.Name).ToList() ?? new List<string>();

            // no goal to execute
            if (definedGoals.Count == 0)
            {
                return 0;
            }

            // if nothing is requested consider running all defined goals
            if (requestedGoals.Count == 0)
            {
                requestedGoals = definedGoals;
            }

            // make sure all the requested goals are present 
            var missing = requestedGoals.Where(requested => !definedGoals.Contains(requested)).ToList();

            // there are missing goals
            if (missing.Count > 0)
            {
                throw new YagenException($"Some requested goals ({string.Join(", ", missing)}) are missing from the manifest");
            }

            // start processing each requested goal
            foreach (var goalName in requestedGoals)
            {
                // the output directory
                var outputDirectory = Path.Combine(arguments.Output.FullName, goalName);

                // find the goal
                var goalManifest = manifest.Goals?.FirstOrDefault(g => string.Equals(g.Name, goalName));

                // make sure goal is there
                if (goalManifest == null)
                {
                    throw new YagenException("The goal is missing");
                }

                // map the goal
                var goal = MapGoal(arguments, manifest, goalManifest);

                await new GoalProcessor(goal, new DirectoryInfo(outputDirectory)).Execute();
            }

            return 0;
        }

        /// <summary>
        /// Maps the goal from the manifest and configuration
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <param name="manifest">The manifest object</param>
        /// <param name="goal">The goal manifest</param>
        /// <returns></returns>
        private static YagenGoal MapGoal(YagenArguments args, YagenManifest manifest, YagenGoalManifest goal)
        {
            // build a goal
            return new YagenGoal
            {
                Name = goal.Name,
                Layers = goal.Layers?.Select(path => new DirectoryInfo(Path.GetFullPath(path, args.Input.FullName))).ToList() ?? new List<DirectoryInfo>(),
                EnvFiles = goal.EnvFiles?.Select(path => new FileInfo(Path.GetFullPath(path, args.Input.FullName))).ToList() ?? new List<FileInfo>(),
                ValueFiles = goal.ValueFiles?.Select(path => new FileInfo(Path.GetFullPath(path, args.Input.FullName))).ToList() ?? new List<FileInfo>(),
                Options = MapGoalOptions(manifest.Options, goal.Options)
            };
        }
        
        /// <summary>
        /// Maps the yagen goal options
        /// </summary>
        /// <param name="global">The global options</param>
        /// <param name="options">The goal options</param>
        /// <returns></returns>
        private static YagenGoalOptions MapGoalOptions(YagenOptionsManifest global, YagenOptionsManifest options)
        {
            return new YagenGoalOptions
            {
                ReplaceArrays = options?.ReplaceArrays ?? global?.ReplaceArrays ?? DEFAULT_OPTIONS_REPLACE_ARRAYS
            };
        }
    }
}