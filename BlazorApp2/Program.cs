using BlazorApp2.Components;
using Glab.App.Laboratoires;
using Glab.App.Members;
using Glab.Implementation.Services.Laboratoires;
using Glab.Infrastructures.Storages.LaboratoriesStorages;
using Glab.Infrastructures.Storages.MembersStorages;
using Glab.Infrastructures.Storages.TeamsStorages;
using GLAB.App.Teams;
using GLAB.Implementation.Services.Members;
using GLAB.Implementation.Services.Teams;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILaboratoryStorage, LaboratoryStorage>();
builder.Services.AddScoped<ITeamStorage, TeamStorage>();
builder.Services.AddScoped<IMemberStorage, MemberStorage>();

builder.Services.AddScoped<ILabService, LabService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMemberService, MemberService>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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
