using System;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using Newtonsoft.Json;

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
        var reply = context.MakeMessage();
		reply.ChannelData = new FacebookChannelData()
		{
		  Attachment = new FacebookAttachment()
		  {
			Type = "image",
			Payload = new FacebookImage() { Url = "https://media.giphy.com/media/3ornjSL2sBcPflIDiU/giphy.gif"}
		  }
		}; 
        
		//send message
		await context.PostAsync(reply);

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

public class FacebookAttachment
{
	public FacebookAttachment()
	{
		this.Type = "template";
	}

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("payload")]
	public dynamic Payload { get; set; }

	//make sure ToString converts the payload
	public override string ToString()
	{
		return this.Payload.ToString();
	}
}

public class FacebookChannelData
{
	[JsonProperty("attachment")]
	public FacebookAttachment Attachment
	{
		get;
		internal set;
	}
}

public class FacebookImage
{
	[JsonProperty("url")]
	public string Url
	{
		get;
		internal set;
	}
}