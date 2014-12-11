﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinistryPlatform.Translation.Helpers
{
    public class MPFormatConversion
    {
        public static JArray MPFormatToJson(PlatformService.SelectQueryResult mpObject)
        {
            //map the reponse into name/value pairs
            var json = new JArray();
            foreach (var dataItem in mpObject.Data)
            {
                var jObject = new JObject();
                foreach (var mpField in mpObject.Fields)
                {
                    var jProperty = new JProperty(mpField.Name, dataItem[mpField.Index]);
                    jObject.Add(jProperty);
                }
                json.Add(jObject);
            }

            return json;
        }

        public static JArray MPFormatToJson(string mpFormat)
        {
            PlatformService.SelectQueryResult mpObject;

            try
            {
                mpObject = JsonConvert.DeserializeObject<PlatformService.SelectQueryResult>(mpFormat);
            }
            catch
            {
                //TO-DO: add proper error handler
                return null;
            }            

            //map the reponse into name/value pairs
            var json = new JArray();
            foreach (var dataItem in mpObject.Data)
            {
                var jObject = new JObject();
                foreach (var mpField in mpObject.Fields)
                {
                    var jProperty = new JProperty(mpField.Name, dataItem[mpField.Index]);
                    jObject.Add(jProperty);
                }
                json.Add(jObject);
            }

            return json;
        }

        public static string JsonToMPFormat(JArray json)
        {
            var list = new List<MPPostValue>();
            foreach (JObject jObj in json)
            {
                foreach (var j in jObj)
                {
                    var x = new MPPostValue();
                    x.Key = j.Key;
                    x.Value = j.Value.ToString();
                    list.Add(x);
                    System.Diagnostics.Debug.WriteLine(j.Value);
                }

            }
            System.Diagnostics.Debug.WriteLine(list.ToString());
            string jsonOut = JsonConvert.SerializeObject(list);
            return jsonOut;
        }
    }

    public class MPPostValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}