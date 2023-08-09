using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class testser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var set = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        var ser = JsonConvert.SerializeObject(Color.green, Formatting.Indented, set);
        //File.WriteAllText("C:\\Users\\111\\Desktop\\maps\\qwe.json", ser);

        //var col = JsonConvert.DeserializeObject<Color>(ser);
        ColorUtility.TryParseHtmlString("#FFFFFF00", out Color col);
        Debug.LogError(col);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
