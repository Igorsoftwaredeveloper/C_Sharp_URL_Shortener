using c.Components;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapGet("/{shortKey}", (string shortKey, HttpContext context) => { context.Response.Redirect(new MongoClient(new MongoUrl("mongodb://localhost:27017")).GetDatabase("UrlDB").GetCollection<Url>("UrlRecord").FindSync<Url>(Builders<Url>.Filter.Eq(u => u.ShortKey, shortKey)).First().Site); });
app.MapPost("/", (Url url) => {	new MongoClient(new MongoUrl("mongodb://localhost:27017")).GetDatabase("UrlDB").GetCollection<Url>("UrlRecord").InsertOne(url); });

app.Run();
public class Url
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string  Site { get; set; }
    public required string  ShortKey { get; set; }
}

 


