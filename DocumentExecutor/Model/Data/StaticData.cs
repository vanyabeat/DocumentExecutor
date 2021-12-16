using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DocumentExecutor.Model.Data
{
    public class StaticData
    {
        private static Lazy<StaticData> _lazy;

        public static StaticData Instance => _lazy.Value;

        public static List<Division> Divisions { get; private set; }

        public static List<IdentifierType> IdentifierTypes { get; private set; }

        public StaticData(string path)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path));
                if (obj != null)
                    IdentifierTypes =
                        JsonConvert.DeserializeObject<List<IdentifierType>>(obj["IdentifierTypes"].ToString() ??
                                                                            string.Empty);
                if (obj != null)
                    Divisions = JsonConvert.DeserializeObject<List<Division>>(obj["Divisions"].ToString() ??
                                                                              string.Empty);
            }
            catch (Exception e)
            {
                Divisions = new List<Division>();
                IdentifierTypes = new List<IdentifierType>();
            }

            _lazy = new Lazy<StaticData>(() => new StaticData(path));
        }
    }

  
}