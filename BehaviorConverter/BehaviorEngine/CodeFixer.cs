using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorConverter.Exceptions;

namespace BehaviorConverter.BehaviorEngine
{
    public class CodeFixer
    {
        private readonly string baseSource;
        private readonly List<string> lines;
        private readonly string behaviorField;


        public CodeFixer(string source)
        {
            baseSource = source;
            lines = new List<string>();
            using (var rdr = new StreamReader(StreamUtils.StringToStream(baseSource)))
            {
                while (!rdr.EndOfStream)
                {
                    var line = rdr.ReadLine();
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    lines.Add(line);
                }
            }
            behaviorField = findFieldName();
            fix();
        }

        private string findFieldName()
        {
            var beforeNamenpace = false;

            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("namespace "))
                    beforeNamenpace = true;
                if (!line.TrimStart().StartsWith("private _ ") &&
                    (!line.TrimStart().StartsWith("_ ") || !beforeNamenpace)) continue;
                return line.TrimStart().StartsWith("private _ ")
                    ? line.TrimStart().Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries)[2]
                    : line.TrimStart().Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            throw new InvalidBehaviorFileException("Behavior field name could not be found.");
        }

        private void fix()
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var trim = lines[i].TrimStart();

                if (trim.StartsWith("namespace"))
                    lines[i] = $"namespace {behaviorField.ToUpperInvariant()}";

                if (trim.StartsWith($"private _ {behaviorField}") || trim.StartsWith($"_ {behaviorField}"))
                    lines[i] = lines[i].Replace($"_ {behaviorField}", $"wServer.logic.BehaviorDb._ {behaviorField}");

                if (trim.Contains("=> Behav()"))
                    lines[i] = lines[i].Replace("=> Behav()", "=> wServer.logic.BehaviorDb.Behav()");

                if (trim.StartsWith("new State"))
                    lines[i] = lines[i].Replace("new State", "new wServer.logic.State");
            }
        }

        public string GetFixed()
        {
            var builder = new StringBuilder();
            foreach (var line in lines)
                builder.AppendLine(line);

            File.WriteAllText("out.cs", builder.ToString());

            return builder.ToString();
        }
    }
}
