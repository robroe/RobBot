using System;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Extensions;
using Microsoft.Bot.Builder.Luis.Models;

using Newtonsoft.Json;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
        
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Sorry I didn't quite get that. You can check the status of your application or add cars. You said: {result.Query}"); //
        
        context.Wait(MessageReceived);
    }

	[LuisIntent("welcome")]
    public async Task WelcomeIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Hi {GetNameFromContext(context)}. How can we help you? You can check the status of your application or add cars."); 
        
        context.Wait(MessageReceived);
    }

	[LuisIntent("welcome")]
    public async Task WelcomeIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Hi {GetNameFromContext(context)}. How can we help you? You can check the status of your application or add cars."); 
        
        context.Wait(MessageReceived);
    }

    [LuisIntent("activity")]
    public async Task ActivityIntent(IDialogContext context, LuisResult result)
    {       
        await context.PostAsync($"Hey {GetNameFromContext(context)}, You have 1231 open activities"); 
        
        context.Wait(MessageReceived);
    }

	[LuisIntent("GetStatus")]
    public async Task GetStatusIntent(IDialogContext context, LuisResult result)
    {       
        await context.PostAsync($"Hey {GetNameFromContext(context)}, your application is approved."); 
        
        context.Wait(MessageReceived);
    }

	[LuisIntent("addcar")]
    public async Task AddCarIntent(IDialogContext context, LuisResult result)
    {       
		var entity = new EntityRecommendation();
        result.TryFindEntity("reg", out entity.Entity);
        await context.PostAsync($"Added the car with reg '{entity}' for you."); 
        
        context.Wait(MessageReceived);
    }

	[LuisIntent("booktestdrive")]
    public async Task BookTestDriveIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"Your test drive has been booked."); 
        
        context.Wait(MessageReceived);
    }
    
    [LuisIntent("applications")]
	[LuisIntent("approved")]
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