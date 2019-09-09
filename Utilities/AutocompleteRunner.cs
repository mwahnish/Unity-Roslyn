using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AutocompleteRunner
{
    

    public static CompletionList Autocomplete(string data, int position)
    {
        AdhocWorkspace workspace = new AdhocWorkspace();

        CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(
           OutputKind.DynamicallyLinkedLibrary,
           usings: new[] { "System", "System.Collections", "System.Collections.Generic","UnityEngine"
        });

        
        ProjectInfo scriptProjectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "Script", "Script", LanguageNames.CSharp, isSubmission: true)
           .WithMetadataReferences(new[]
           {
       MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
           })
           .WithCompilationOptions(compilationOptions);

        Project scriptProject = workspace.AddProject(scriptProjectInfo);
        DocumentInfo scriptDocumentInfo = DocumentInfo.Create(
            DocumentId.CreateNewId(scriptProject.Id), "Script",
            sourceCodeKind: SourceCodeKind.Script,
            loader: TextLoader.From(TextAndVersion.Create(SourceText.From(data), VersionStamp.Create())));
        Document scriptDocument = workspace.AddDocument(scriptDocumentInfo);

        // cursor position is at the end
        

        var completionService = CompletionService.GetService(scriptDocument);
        Task<CompletionList> results = completionService.GetCompletionsAsync(scriptDocument, position -1);
        

        return results.Result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
