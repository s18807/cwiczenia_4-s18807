using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Json.Net;
using Newtonsoft.Json;
using WebApplication1.Models;

public class GenericToCSV{
    public static void WriteCSV<T>(IEnumerable<T> items, string path)
    {
        Type itemType = typeof(T);
        var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .OrderBy(p => p.Name);

        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine(string.Join("; ", props.Select(p => p.Name)));

            foreach (var item in items)
            {
                byte[] bytes = Encoding.Default.GetBytes(item.ToString());
                string myString = Encoding.UTF8.GetString(bytes);
                writer.WriteLine(string.Join("; ", props.Select(p => p.GetValue(item, null))));
            }
        }
    }
}