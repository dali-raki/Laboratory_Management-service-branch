using GLAB.Implementation.Services.Members;
using GLAB.Implementation.Services.Teams;
using GLAB.App.Teams;
using GLAB.Implementation.Services.Accounts;
using Glab.Infrastructures.Storages.TeamsStorages;
using Glab.Infrastructures.Storages.MembersStorages;
using Glab.Infrastructures.Storages.UserStorage;
using Glab.Infrastructures.Storages.LaboratoriesStorages;
using Glab.App.Members;
using Glab.App.Accounts;
using Glab.App.Users;
using Glab.App.Laboratoires;
using Glab.Implementation.Services.Laboratoires;
using Glab.Implementation.Services.Users;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILaboratoryStorage, LaboratoryStorage>();
builder.Services.AddScoped<ITeamStorage, TeamStorage>();
builder.Services.AddScoped<IMemberStorage, MemberStorage>();

builder.Services.AddScoped<ILabService, LabService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMemberService, MemberService>();




builder.Services.AddAuthentication().AddCookie();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddScoped<IMemberStorage, MemberStorage>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserStorage, UserStorage>();

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

//add
app.MapControllers();

app.Run();

