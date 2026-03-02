using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Host.ConfigureDiscordBot((_, bot) =>
{
    bot.Token = builder.Configuration.GetValue<string>("Discord:Token");
    bot.OwnerIds = [builder.Configuration.GetValue<ulong>("Discord:OwnerId")];
    bot.ApplicationId = builder.Configuration.GetValue<ulong>("Discord:ApplicationId");
    bot.UseMentionPrefix = true;
    bot.Intents = GatewayIntents.All;
    bot.Status = UserStatus.DoNotDisturb;
    bot.Activities = [new LocalActivity("with the Grand Archive API", ActivityType.Playing)];
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();