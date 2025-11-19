using Microsoft.AspNetCore.ResponseCompression;
using PdfMergerApp.Server.Services;
using PdfMergerApp.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register PDF merge service
builder.Services.AddScoped<IPdfMergeService, PdfMergeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();

// PDF Merge API endpoints
app.MapPost("/api/pdf/merge", async (MergeRequest request, IPdfMergeService pdfService) =>
{
    var result = await pdfService.MergePdfsAsync(request);
    
    if (result.Success && result.MergedFileData != null)
    {
        return Results.File(
            result.MergedFileData,
            result.ContentType,
            result.FileName);
    }
    
    return Results.BadRequest(new { error = result.ErrorMessage });
})
.WithName("MergePdfs");

app.MapPost("/api/pdf/validate", async (IFormFile file, IPdfMergeService pdfService) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new { error = "No file provided" });
    }

    using var stream = new MemoryStream();
    await file.CopyToAsync(stream);
    var fileData = stream.ToArray();

    var isValid = await pdfService.ValidatePdfAsync(fileData);
    return Results.Ok(new { isValid, fileName = file.FileName });
})
.WithName("ValidatePdf");

app.MapFallbackToFile("index.html");

app.Run();
