using System.Collections.Generic;
using System.Reflection;

namespace DictionaryAPItest
{
    public class WordDetail
    {
        public string Word { get; set; }
        public string Phonetic { get; set; }
        public List<Phonetic> Phonetics { get; set; }
        public List<Meaning> Meanings { get; set; }
        public License License { get; set; }
        public List<string> SourceURLs { get; set; }
    }
}