using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace crds_angular.Services
{
    public class MinistryPlatformBaseService
    {

        protected  Dictionary<string, object> getDictionary(Object input)
        {
            var dictionary = input.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(input, null));
            return dictionary;
        }
    }
}