using FluentValidation;
using FluentValidation.Results;
using library.api;
using library.api.Data;
using library.api.Endpoints;
using library.api.Models;
using library.api.Properties.Auth;
using library.api.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddJsonFile("appsetting.Local.json", true, true);
builder.Services.AddAuthentication(apiKeyConstants.SchemeName)
    .AddScheme<apiKeyAuthSchemeOptions, apiKeyAuthHandler >(apiKeyConstants.SchemeName, _ =>
    {
        
    });
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDBConnectionFactory>(_ =>
           new SQLiteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")
    ));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IBookService,BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();



var app = builder.Build();

builder.Services.AddLibraryEndpoints();
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();



app.MapGet("status", () =>
{
    return Results.Extensions.html(@"<!doctype html>
<html>
<head><title>status page</title></head>
<body>
<h1>status from the server</h1>
<p>server is working fine</p>
</body>
</html>");
});

app.MapPost("books" ,
    [Authorize(AuthenticationSchemes = apiKeyConstants.SchemeName)]
     async (Book book, IBookService bookService,IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    var created = await bookService.CreateAsync(book);
    if (!created)
    {
        return Results.BadRequest(new List<ValidationFailure>
        {
            new  ("Isbn","A Book With this ISBN-13 is Already Exists") 
        }); 
    }
    return Results.Created($"/books/{book.Isbn}",book);
}) 
//.Accepts<Book>("application/json")
//.Produces<Book>(201)
//.Produces<IEnumerable<ValidationFailure>>(400)
;

app.MapGet("books", async ( IBookService ibookService, string searchTerm)=>
{
    if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedbooks = await ibookService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedbooks);
    }


    //return Results.CreatedAtRoute("GetBook", new { isbn = book.Isbn }, book);
    var books = await ibookService.GetAllAsync();
    return Results.Ok(books);
}).WithName("GetBook");
app.MapGet("book", async (IBookService ibookService) =>
{


    var books = await ibookService.GetAllAsync();
    return Results.Ok(books);
});

//for edit 
app.MapPut("books/{isbn}", async (string isbn,Book book, IBookService ibookService, IValidator<Book> validator) =>
{
    book.Isbn = isbn;
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    
    var updated = await ibookService.UpdateAsync(book);
    return updated ? Results.Ok(updated) : Results.NotFound();  

});
//FOR DELETE
app.MapDelete("books/{isbn}", async (string isbn, IBookService ibookService) =>
{
    var deleted = await ibookService.DeleteAsync(isbn);
    return deleted ? Results.NoContent() : Results.NotFound();
});


//to get the bbok by isbn
app.MapGet("books/{isbn}", async (string isbn, IBookService ibookService) =>
{
    var book = await ibookService.GetByIsbnAsync(isbn);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});



var databaseInitilizer = app.Services.GetRequiredService<DatabaseInitializer>();
 await databaseInitilizer.InitializeAsync();

//db init here


app.Run();
