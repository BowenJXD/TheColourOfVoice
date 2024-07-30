using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> cachedCSV = new();
        
        public Dictionary<string, Dictionary<string, string>> LoadCSV(string path)
        {
            if (cachedCSV.TryGetValue(path, out var cachedData))
            {
                return cachedData;
            }
            
            string csv = Resources.Load<TextAsset>(PathDefines.SpellConfig).text;
            List<string[]> lines = CSVSerializer.ParseCSV(csv);
            
            var data = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 1; i < lines.Count; i++)
            {
                var row = lines[i];
                var rowData = new Dictionary<string, string>();
                for (int j = 0; j < lines[0].Length; j++)
                {
                    rowData.Add(lines[0][j], row[j]);
                }
                data.Add(row[0], rowData);
            }
            
            cachedCSV.Add(path, data);
            return data;
        }
    }
}