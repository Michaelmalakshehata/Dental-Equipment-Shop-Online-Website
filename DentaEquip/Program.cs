using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Repositories;
using DentaEquip.BL.Repositories.EmailService;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EntityContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DentaEquipDB"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Default Password settings.
    //options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<EntityContext>()
           .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddAntiforgery(options=> { options.SuppressXFrameOptionsHeader = true; });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});
//Inject Services
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(ServiceGeneric<>));
builder.Services.AddScoped(typeof(IGenericServiceSoftDelete<>), typeof(ServiceGenericSoftDelete<>));
builder.Services.AddScoped(typeof(IServiceMainCategory), typeof(ServiceMainCategory));
builder.Services.AddScoped(typeof(IServiceSubCategory), typeof(ServiceSubCategory));
builder.Services.AddScoped(typeof(IServiceBrand), typeof(ServiceBrand));
builder.Services.AddScoped(typeof(IServiceProduct), typeof(ServiceProduct));
builder.Services.AddScoped(typeof(IServiceShowProduct), typeof(ServiceShowProduct));

builder.Services.AddScoped(typeof(IServiceCart), typeof(ServiceCart));
builder.Services.AddScoped(typeof(IServiceWishList), typeof(ServiceWishList));
builder.Services.AddScoped(typeof(IServiceRequests), typeof(ServiceRequests));
builder.Services.AddScoped(typeof(IServiceFinishOrder), typeof(ServiceFinishedOrders));
builder.Services.AddScoped(typeof(IServiceComments), typeof(ServiceComments));
builder.Services.AddScoped(typeof(IHomeService), typeof(HomeService));
builder.Services.AddScoped(typeof(IServiceFilterProduct), typeof(ServiceFilterProduct));

builder.Services.AddScoped(typeof(IRelationModelsRestoreAndDelete), typeof(RelationModelsRestoreAndDelete));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseNotyf();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
