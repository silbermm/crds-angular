using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace crds_angular.Util
{
    public static class CSV
    {
        public static void Create<T>(List<T> list, Stream stream, string delimiter) where T : class
        {
            var sw = new StreamWriter(stream, Encoding.UTF8);
            var headers = new List<string>();

            foreach (var row in list)
            {
                if (headers.Count == 0)
                {
                    WriteHeaders(sw, row, headers, delimiter);
                }

                WriteRow(sw, row, headers, delimiter);
            }

            // Reset the stream position to the beginning
            stream.Seek(0, SeekOrigin.Begin);
        }

        private static void WriteHeaders<T>(TextWriter sw, T row, List<string> headers, string delimiter)
        {
            var json = JsonConvert.SerializeObject(row);
            var jobj = JObject.Parse(json);
            var tokens = jobj.Children();
            var initialCell = true;

            foreach (var token in tokens)
            {
                var header = ((JProperty) token).Name;
                headers.Add(header);

                if (initialCell)
                {
                    sw.Write(delimiter);
                    initialCell = false;
                }

                sw.Write(header);
            }

            sw.Write(Environment.NewLine);
            sw.Flush();
        }

        private static void WriteRow<T>(TextWriter sw, T row, List<string> headers, string delimiter)
        {
            var initialCell = true;

            foreach (var name in headers)
            {
                if (initialCell)
                {
                    sw.Write(delimiter);
                    initialCell = false;
                }

                var attrName = name.Replace(" ", "");
                var value = row.GetType().GetProperty(attrName).GetValue(row);

                sw.Write(value.ToString());
            }

            sw.Write(Environment.NewLine);
            sw.Flush();
        }
    }
}