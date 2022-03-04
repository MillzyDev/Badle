using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Badle.Wordle
{
    public class Words
    {
        private static Words _wordle;

        public static Words Instance
        {
            get
            {
                if (_wordle == null)
                    _wordle = new Words();
                return _wordle;
            }
        }

        public string[] Answers
        {
            get; 
            private set;
        }

        public string[] Acceptables
        {
            get;
            private set;
        }

        private Words()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string answersJson = GetResource("Resources/possible_words.json", assembly);
            string acceptablesJson = GetResource("Resources/acceptable_words.json", assembly);

            Answers = JsonConvert.DeserializeObject<string[]>(answersJson);
            Acceptables = JsonConvert.DeserializeObject<string[]>(acceptablesJson);
        }

        private static string GetResource(string name, Assembly assembly)
        {
            name = FormatResourceName(assembly, name);
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (name == null)
                    return null;


                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static string FormatResourceName(Assembly assembly, string name)
        {
            return assembly.GetName().Name + "." + name.Replace(' ', '_')
                .Replace('\\', '.')
                .Replace('/', '.');
        }
    }
}
