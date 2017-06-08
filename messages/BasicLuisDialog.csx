using System;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
        
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"You have reached the none intent. You said: {result.Query} yo yo yo"); //
        
        context.Wait(MessageReceived);
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("activity")]
    public async Task ActivityIntent(IDialogContext context, LuisResult result)
    {       
        await context.PostAsync($"Hey {GetNameFromContext(context)}, You have 1231 open activities"); 
        
        context.Wait(MessageReceived);
    }
    
    [LuisIntent("applications")]
    public async Task ApplicationsIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"{GetNameFromContext(context)} You have 445 applications assigned."); 
        
        context.Wait(MessageReceived);
    }
    
	public string GetNameFromContext(IDialogContext context)
	{
		return context.Activity.From.Name;
	}
	
    public async Task DoStuff()
    {
        var getApplicationActivitiesRequest = new HttpRequestMessage(HttpMethod.Post, "http://applications.proxy.zuto.network"); 
        
         var httpClient = new HttpClient();
        try
        {
         await httpClient.SendAsync(getApplicationActivitiesRequest);
        }
        catch(Exception ex)
        {
          //  await context.PostAsync($"Boom {ex}"); 
        }
    }
}