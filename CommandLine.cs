// receives selected text. evaluates found code between [lang][/lang]. returns code and "\nResult: " and result
// modifies the zim file
using System.Collections.Generic;

namespace bozes
{
    internal class CommandLine
    {
        public List<string> Arguments { get; set; }
        public List<string> DoubleDashed { get; set; }
        public Dictionary<string, List<string>> Dashed { get; set; }
    }
}
