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
using OnlineLibrary.Domain;

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

var stripeSettings = builder.Configuration.GetSection("StripeSettings").Get<StripeSettings>();
var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
if (!string.IsNullOrEmpty(stripeSecretKey))
{
    stripeSettings.SecretKey = stripeSecretKey;
}
builder.Services.Configure<StripeSettings>(options =>
{
    options.PublishableKey = stripeSettings.PublishableKey;
    options.SecretKey = stripeSettings.SecretKey;
});

var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
if (!string.IsNullOrEmpty(smtpPassword))
{
    emailSettings.SmtpPassword = smtpPassword;
}
builder.Services.Configure<EmailSettings>(options =>
{
    options.SmtpServer = emailSettings.SmtpServer;
    options.SmtpUserName = emailSettings.SmtpUserName;
    options.SmtpPassword = emailSettings.SmtpPassword;
    options.SmtpServerPort = emailSettings.SmtpServerPort;
    options.EnableSsl = emailSettings.EnableSsl;
    options.EmailDisplayName = emailSettings.EmailDisplayName;
    options.SendersName = emailSettings.SendersName;
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));
builder.Services.AddScoped(typeof(IBorrowingCartRepository), typeof(BorrowingCartRepository));
builder.Services.AddScoped(typeof(IBorrowingHistoryRepository), typeof(BorrowingHistoryRepository));

builder.Services.AddTransient<IEmailService, EmailService>();
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