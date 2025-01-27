using System.Text;
using Microsoft.Extensions.Logging;
using Minimal.Api;
using Minimal.Api.Properties;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<GuidGenerator>();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); 

app.MapGet("get_Example", () => "Hello From Get");
app.MapPost("post_Example", () => "Hello From Post");

app.MapGet("ok_Example", () => Results.Ok(new
{
    Name = "Abdulrub Zaman"
}));

app.MapGet("Slow_Example",  async () =>
{
    await Task.Delay(2000);
    return Results.Ok(new
    {
        Name = "Asadullah Khan Mirza Ghalib",
        Age = 57,
        Profession = "Poetry"
    });
});

app.MapMethods("heads-or-options", new[] { "Head", "Options" }, () =>
"THIS IS FROM EITHER HEAD OR OPTIONS");

var handler = () => "this is from variable ";
app.MapGet("handler", handler);

app.MapGet("fromanotherclass", exampleMethod.somemethod);

app.MapGet("getparam/{age}", (int age) =>
{
    return $"age provided and age is{age}";
});

app.MapGet("people/search", (string? searchterm, PeopleService peopleService) =>
{
    if(searchterm is null)
    {
       return Results.NotFound();
    }
    var results = peopleService.Search(searchterm); 
    return Results.Ok(results); 

});

app.MapGet("mix/{routeParam}", (string routeParam, int queryParam, GuidGenerator guidGenerator)=>
{
    return $"{routeParam} {queryParam} {guidGenerator}";
});

app.MapPost("people", (Person person) =>
{
    return Results.Ok(person);
});

app.MapGet("map-point", (MapPoint mapPoint) => {


return Results.Ok(mapPoint);
});

app.MapPost("map-point", (MapPoint mapPoint) => {


    return Results.Ok(mapPoint);
});

app.MapGet("stream-result", () =>
{
    var memoryStream = new MemoryStream();
    var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
    streamWriter.Write("Hello Saalo");
    streamWriter.Flush();
    memoryStream.Seek(0, SeekOrigin.Begin);
    return Results.Stream(memoryStream,"text/plain"); }


);

app.MapGet("redirect", () => Results.Redirect("https://www.google.com"));

app.MapGet("logging", (ILogger < Program > ILogger) =>
{
    ILogger.LogInformation("hello from the enpoint");
    Results.Ok();
});




app.Run();
