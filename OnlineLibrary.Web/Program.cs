using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Repository;
using OnlineLibrary.Repository.Interface;
using OnlineLibrary.Repository.Implementation;
using OnlineLibrary.Service.Interface;
using OnlineLibrary.Service.Implementation;
using Stripe.Climate;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Member>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));
builder.Services.AddScoped(typeof(IBorrowingCartRepository), typeof(BorrowingCartRepository));
builder.Services.AddScoped(typeof(IBorrowingHistoryRepository), typeof(BorrowingHistoryRepository));

builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<ICategorySevice, CategoryService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<ICategorySevice, CategoryService>();
builder.Services.AddTransient<IBorrowingCartService, BorrowingCartService>();
builder.Services.AddTransient<IBorrowingHistoryService, BorrowingHistoryService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();