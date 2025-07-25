using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RushtecRH;
using Microsoft.AspNetCore.StaticFiles;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddMemoryCache();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = { [".odt"] = "application/vnd.oasis.opendocument.text" }
    }
});

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapRazorPages();});
app.MapFallbackToPage("/Login");
app.MapRazorPages();
app.Run();
