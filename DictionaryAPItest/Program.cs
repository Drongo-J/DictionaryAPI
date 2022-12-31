using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DictionaryAPItest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var wordDetail = GetWordDetail("address").Result;
            var d = wordDetail;
            Console.ReadLine();
        }

        private static HttpClient _client = new HttpClient();
        private static readonly string URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";
        public static async Task<WordDetail> GetWordDetail(string word)
        {
            var url = URL + word;

            var response = Task.Run(() => _client.GetAsync(url)).Result;
            var str = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JsonHelper.FormatJson(str));
            var data = JsonConvert.DeserializeObject<List<WordDetail>>(str);
            return data.First();
        }
    }

    public class JsonHelper
    {
        private const string INDENT_STRING = "    ";
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}