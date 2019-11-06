// receives selected text. evaluates found code between [lang][/lang]. returns code and "\nResult: " and result
// modifies the zim file
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace bozes
{
    static class Program
    {
        private static Dictionary<string, Dictionary<string, string>> Languages = new Dictionary<string, Dictionary<string, string>>();

        static void Main(string[] args)
        {
            var CLI = ParseCommandLineToSettingsAndArgs(args);
            if (CLI.Arguments.Count == 0 && CLI.DoubleDashed.Count == 0)
            {
                return;
            }

            if (CLI.DoubleDashed.Contains("--debug"))
            {
                Debugger.Launch();
            }

            var selection = CLI.Arguments[0];
            if (selection.Length == 0)
            {
                return;
            }

            var location = System.Reflection.Assembly.GetEntryAssembly().Location;

            /*
            %f page source tempfile
            %d attachment directory
            %s real page source
            %p page name
            %n notebook location
            %D document root
            %t selected text or word user cursor
            %T selected formatted text

            However, we'll always assume that %t is the only one we understand.
            */

            var json = Path.ChangeExtension(location, ".json");
            if (!File.Exists(json))
            {
                Console.Write(selection);
                return;
            }

            var jsonText = File.ReadAllText(json);

            try
            {
                LoadLanguagesFromJson(jsonText);
            }
            catch
            {
                Console.Write(selection);
                return;
            }

            string result = string.Empty;

            foreach (var lang in Languages)
            {
                var rx = new Regex($@"\[{lang.Key}\](.*?)\[/{lang.Key}\]", RegexOptions.Singleline);
                var matches = rx.Match(selection);
                if (matches.Success)
                {
                    var code = matches.Groups[1].Value;
                    result = ExecuteCode(code, lang.Value);
                }
            }
            Console.Out.Write(selection + "\nResult: " + result);
        }

        private static void LoadLanguagesFromJson(string jsonText)
        {
            var doc = System.Text.Json.JsonDocument.Parse(jsonText);
            var root = doc.RootElement;
            var langsEnu = root.EnumerateObject();
            var name = string.Empty;
            foreach (var langs in langsEnu)
            {
                name = langs.Name;
                var langEnu = langs.Value.EnumerateObject();
                var langList = new Dictionary<string, string>();
                foreach (var lang in langEnu)
                {
                    langList.Add(lang.Name, lang.Value.GetString());
                }
                Languages.Add(name, langList);
            }
        }

        private static string ExecuteCode(string code, Dictionary<string, string> value)
        {
            var tempFile = Path.ChangeExtension(Path.GetTempFileName(), value["Extension"]);
            File.WriteAllText(tempFile, code);
            var proc = new Process();

            proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            proc.StartInfo.FileName = value["Binary"];
            proc.StartInfo.Arguments = value["Tail"].Replace("$f", tempFile);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            var output = proc.StandardOutput.ReadToEnd();
            return output;
        }

        static CommandLine ParseCommandLineToSettingsAndArgs(string[] args)
        {
            var results = new CommandLine
            {
                Arguments = new List<string>(),
                DoubleDashed = new List<string>(),
                Dashed = new Dictionary<string, string>()
            };

            foreach (string arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    results.DoubleDashed.Add(arg);
                }
                else
                {
                    if (arg.StartsWith("-"))
                    {
                        var pos = arg.IndexOfAny(new char[] { '=', ':' });
                        if (pos > -1)
                        {
                            results.Dashed[arg.Substring(0, pos)] = arg.Substring(pos+1);

                        }
                    }
                    else
                    {
                        results.Arguments.Add(arg);
                    }
                }
            }
            return results;
        }
    }

}
