using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class JsonProvider
{
    public T Load<T>(string path, JsonSerializerSettings serializerSettings)
    {
        var isExist = File.Exists(path);
        if (isExist == false)
            throw new FileNotFoundException(path);

        var dataJson = File.ReadAllText(path);
        if (string.IsNullOrEmpty(dataJson) == true)
            throw new FormatException();

        if (Regex.IsMatch(dataJson.Trim(), @"{\s*}") == true)
            throw new FormatException();

        return Deserialize<T>(dataJson, serializerSettings);
    }

    public void Save(object data, string path, JsonSerializerSettings serializerSettings)
    {
        var jsonData = Serialize(data, serializerSettings);
        File.WriteAllText(path, jsonData);
    }

    public string Serialize(object data, JsonSerializerSettings serializerSettings)
    {
        return JsonConvert.SerializeObject(data, serializerSettings);
    }

    public T Deserialize<T>(string jsonData, JsonSerializerSettings serializerSettings)
    {
        return JsonConvert.DeserializeObject<T>(jsonData, serializerSettings);
    }
}