using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace JSONExporter
{

public class UnityJSONExporter : ScriptableObject 
{
    static void reset()
    {
        JEResource.Reset();
        JEComponent.Reset();
        JEScene.Reset();
        JEGameObject.Reset();

        JEComponent.RegisterStandardComponents();
    }

    public static JSONScene GenerateJSONScene()
    {
        // reset the exporter in case there was an error, Unity doesn't cleanly load/unload editor assemblies
        reset();    

        JEScene.sceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);

        JEScene scene = JEScene.TraverseScene(); 

        scene.Preprocess();
        scene.Process();
        scene.PostProcess();

        JSONScene jsonScene = scene.ToJSON() as JSONScene;

        reset();

        return jsonScene;

    }

    [MenuItem ("JSONExporter/Export to JSON")]
    public static void DoExport()
    {

        var jsonScene = GenerateJSONScene();
        
        // move me
        JsonConverter[] converters = new JsonConverter[]{new BasicTypeConverter()};
        string json = JsonConvert.SerializeObject(jsonScene, Formatting.Indented, converters);

        string filename = JEScene.sceneName + ".js";

        System.IO.File.WriteAllText(@"/Users/josh/Desktop/" + filename, json);
    }

}

}