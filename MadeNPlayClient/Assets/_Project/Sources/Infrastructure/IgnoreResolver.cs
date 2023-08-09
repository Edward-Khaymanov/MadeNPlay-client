using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MadeNPlayShared
{
    public class IgnoreResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = new List<JsonProperty>();
            var properties = type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(prop => prop.CanWrite == true && prop.CanRead == true)
                .Where(prop => Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)) == false)
                .Select(prop => base.CreateProperty(prop, memberSerialization));

            var publicFields = type
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(field => base.CreateProperty(field, memberSerialization));

            var serializeFields = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => Attribute.IsDefined(field, typeof(SerializeField)))
                .Select(field => base.CreateProperty(field, memberSerialization));

            props.AddRange(properties);
            props.AddRange(publicFields);
            props.AddRange(serializeFields);
            props.ForEach(prop => { prop.Writable = true; prop.Readable = true; });

            return props;
        }
    }
}