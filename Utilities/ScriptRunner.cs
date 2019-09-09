using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis;
using System.IO;
using UnityEditor;

public class ScriptRunner
{

    private static ScriptState<object> scriptState = null;
    private static ScriptOptions options = null;
    

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log();
    }

    public static object Execute(string code, object target)
    {
        
        if (options == null)
        {
            options = ScriptOptions.Default;
            options = DoStandardImports(options);
            options = LoadAllDllsInFolder(options, "F:/Program Files/Unity Editors/2019.1.12f1/Editor/Data/Managed/UnityEngine/");
            options = LoadAllDllsInFolder(options, "F:/ABXY/Client Work/DigitalCM/Roslyn-Unity/Library/ScriptAssemblies");
            
        }
        
        var globals = new Globals { target = target };
        scriptState = scriptState == null ? CSharpScript.RunAsync(code,options,globals:globals, globalsType: typeof(Globals)).Result : scriptState.ContinueWithAsync(code, options).Result;
        if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            return scriptState.ReturnValue;
        return null;
    }

    public class Globals
    {
        public object target;
    }

    public static object Execute(string code)
    {
        return Execute(code, null);
    }

    private static ScriptOptions DoStandardImports(ScriptOptions options)
    {
        options = options.AddImports("UnityEngine", "System.Collections", "System.Collections.Generic");

        System.Reflection.Assembly netstandard = System.AppDomain.CurrentDomain.Load("netstandard");
        options = options.AddReferences(netstandard);
        return options;
    }

    private static ScriptOptions LoadAllDllsInFolder(ScriptOptions options, string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory))
        {
            if (Path.GetExtension(file) == ".dll")
                options = options.AddReferences(MetadataReference.CreateFromFile(file));
        }
        return options;
    }

    public static void ResetSession()
    {
        scriptState = null;
        options = null;
    }
}
