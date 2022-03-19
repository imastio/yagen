using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Imast.Yagen.Cli.Ext;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Imast.Yagen.Cli.Processing
{
    /// <summary>
    /// The goal processor module
    /// </summary>
    public class GoalProcessor
    {
        /// <summary>
        /// The regex matcher
        /// </summary>
        private static readonly Regex YAML_MATCHER = new("(?<ymlonly>.*(?:\\.yml|\\.yaml))(?<extension>\\..*)?$");

        /// <summary>
        /// The evaluator matcher
        /// </summary>
        private static readonly Regex EVAL_MATCHER = new("\\.eval-(?<evaluator>\\w+)");

        /// <summary>
        /// The operator matcher
        /// </summary>
        private static readonly Regex OPERATOR_MATCHER = new("\\.op-(?<operator>\\w+)");

        /// <summary>
        /// The goal to process
        /// </summary>
        private readonly YagenGoal goal;

        /// <summary>
        /// The output directory
        /// </summary>
        private readonly DirectoryInfo outputDirectory;

        /// <summary>
        /// Creates new instance of goal processor
        /// </summary>
        /// <param name="goal">The goal to process</param>
        /// <param name="outputDirectory">The output directory</param>
        public GoalProcessor(YagenGoal goal, DirectoryInfo outputDirectory)
        {
            this.goal = goal;
            this.outputDirectory = outputDirectory;
        }

        /// <summary>
        /// Executes the 
        /// </summary>
        public async Task Execute()
        {
            // make sure all the layers are there
            var missingLayer = this.goal.Layers.Any(layer => !Directory.Exists(layer.FullName));

            // there is a missing layer
            if (missingLayer)
            {
                throw new YagenException("One of the layers is missing");
            }

            // make sure all the env files are there
            var missingEnv = this.goal.EnvFiles.Any(env => !File.Exists(env.FullName));

            // there is a missing env
            if (missingEnv)
            {
                throw new YagenException("One of the environment files is missing");
            }

            // make sure all the value files are there
            var missingValue = this.goal.ValueFiles.Any(env => !File.Exists(env.FullName));

            // there is a missing value file
            if (missingValue)
            {
                throw new YagenException("One of the value files is missing");
            }

            // recreate directory if exists
            if (Directory.Exists(outputDirectory.FullName))
            {
                Directory.Delete(outputDirectory.FullName, true);
            }

            // the output directory is created
            Directory.CreateDirectory(outputDirectory.FullName);

            // the stack of values
            var values = new List<IDictionary<object, object>>();

            // build deserializer
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // start processing value files
            foreach (var valueFile in this.goal.ValueFiles)
            {
                // deserialize values file
                var valueObject = deserializer.Deserialize<IDictionary<object, object>>(await File.ReadAllTextAsync(valueFile.FullName));

                // add to values collection
                values.Add(valueObject);
            }

            // keep all the values
            var allValues = new Dictionary<object, object>();

            // apply all the values
            values.ForEach(v => allValues.DeepApply(v));

            // start executing layers
            foreach (var layer in this.goal.Layers)
            {
                await this.ProcessLayer(layer, allValues);
            }
        }

        /// <summary>
        /// Process the given layer to the output directory
        /// </summary>
        /// <param name="layer">The layer to process</param>
        /// <param name="values">The values collection</param>
        /// <returns></returns>
        private async Task ProcessLayer(FileSystemInfo layer, IDictionary<object, object> values)
        {
            // gets all the files in the directory
            var sourceFiles = Directory.GetFiles(layer.FullName, "*", SearchOption.AllDirectories);

            // process each file
            foreach (var sourceFile in sourceFiles)
            {
                // try match file
                var fileMatch = YAML_MATCHER.Match(sourceFile);

                // skip if not a match
                if (!fileMatch.Success)
                {
                    continue;
                }

                // gets the directory of source file
                var sourceFileDirectory = Path.GetDirectoryName(sourceFile) ?? string.Empty;

                // make sure source file directory was captured
                if (string.IsNullOrWhiteSpace(sourceFileDirectory))
                {
                    throw new YagenException($"Could not get source file directory for {sourceFile}");
                }

                // the source file directory relative to layer 
                var relativeSourceDirectory = Path.GetRelativePath(layer.FullName, sourceFileDirectory);

                // the original file path with yaml extension only
                var ymlOnlyPath = fileMatch.Groups["ymlonly"].Value;

                // get the target yml file path
                var ymlFilename = Path.GetFileName(ymlOnlyPath);

                // the extra extension
                var extension = fileMatch.Groups["extension"].Value;

                // the processing factory
                var processingFactory = new ProcessingFactory();

                // resolve evaluator name
                var evaluatorName = ResolveEvaluator(extension);

                // gets the evaluator
                var evaluator = processingFactory.GetEvaluator(evaluatorName);
                
                // build the evaluation context
                var evalContext = new YamlEvaluationContext
                {
                    Goal = this.goal,
                    OutputDirectory = this.outputDirectory,
                    ResolvedFileName = ymlFilename,
                    SourceFile = new FileInfo(sourceFile),
                    LocalDirectoryPath = relativeSourceDirectory,
                    OutputFilePath = Path.Combine(this.outputDirectory.FullName, relativeSourceDirectory, ymlFilename),
                    Values = values
                };

                // evaluate and get result
                var evaluationResult = await evaluator.Evaluate(evalContext);
                
                // resolve operator name
                var operatorName = ResolveOperator(extension);

                // get the yaml operator
                var yamlOperator = processingFactory.GetOperator(operatorName);

                // the operator result
                await yamlOperator.Apply(new YamlOperationContext
                {
                    Goal = this.goal,
                    OutputDirectory = this.outputDirectory,
                    ResolvedFileName = ymlFilename,
                    OriginalSourceFile = new FileInfo(sourceFile),
                    EvaluatedSourceFile = evaluationResult.OutputFile,
                    OutputFilePath = evalContext.OutputFilePath,
                    LocalDirectoryPath = relativeSourceDirectory,
                    ExistingFile = new FileInfo(evalContext.OutputFilePath)
                });

            }
        }

        /// <summary>
        /// Gets the evaluator based on the extension
        /// </summary>
        /// <param name="extension">The extension</param>
        /// <returns></returns>
        private static string ResolveEvaluator(string extension)
        {
            // nothing to do if empty
            if (string.IsNullOrWhiteSpace(extension))
            {
                return string.Empty;
            }

            // try match and get evaluator
            var match = EVAL_MATCHER.Match(extension);

            // return evaluator if matched
            return !match.Success ? string.Empty : match.Groups["evaluator"].Value;
        }

        /// <summary>
        /// Gets the operator based on the extension
        /// </summary>
        /// <param name="extension">The extension</param>
        /// <returns></returns>
        private static string ResolveOperator(string extension)
        {
            // nothing to do if empty
            if (string.IsNullOrWhiteSpace(extension))
            {
                return string.Empty;
            }

            // try match and get operator
            var match = OPERATOR_MATCHER.Match(extension);

            // return operator if matched
            return !match.Success ? string.Empty : match.Groups["operator"].Value;
        }
    }
}