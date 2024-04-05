using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

using AppCore.App.Repositories;
using AppCore.Data;
using AppCore.Models;
using AppCore.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(120); // Thời gian chờ session hết hạn 2 tiếng
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "VanDuc";
});
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("AspApplicationDb"));
});

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderItemRepository>();
builder.Services.AddScoped<PaymentMethodRepository>();
builder.Services.AddScoped<AccountManagerRepository>();


builder.Services.AddIdentity<Account, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddIdentityCore<Account>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<SignInManager<Account>, SignInManager<Account>>();
builder.Services.AddScoped<UserManager<Account>, UserManager<Account>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeRoles(roleManager);
}
// SetupInitialData(app.Services);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapAdminRoutes(); 

app.Run();
                               
void SetupInitialData(IServiceProvider serviceProvider) {
    System.Console.WriteLine("sadasd");
}

async Task InitializeRoles(RoleManager<IdentityRole> roleManager) {
    string[] roleNames = { "Admin", "User", "Staff" }; // Add any additional roles
    IdentityResult roleResult;
    foreach (var roleName in roleNames) {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist) {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}