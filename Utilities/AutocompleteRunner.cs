using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
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
        ProjectInfo projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "Autocompleter", "Autocompleter", LanguageNames.CSharp);
        Project project = workspace.AddProject(projectInfo);
        Document document = workspace.AddDocument(project.Id, "Autocomplete.cs", SourceText.From(data));
        CompletionService completer= CompletionService.GetService(document);
        Task<CompletionList> result = completer.GetCompletionsAsync(document, position);
        result.Wait();
        return result.Result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
