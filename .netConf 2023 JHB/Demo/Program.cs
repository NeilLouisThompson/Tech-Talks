using Demo.Components;
using Microsoft.AspNetCore.ResponseCompression;

//TODO Step 03  -- Add/import namespaces - Program
//using Demo.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
.AddInteractiveServerComponents();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});


var app = builder.Build();

//middleware for dynamic compressing Http response
app.UseResponseCompression();

// TODO Step 04 -- Map our chat hub
//app.MapHub<ChatHub>("/chathub");

//TODO Step 05 -- Add Chat page and add it to NavMenu


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
