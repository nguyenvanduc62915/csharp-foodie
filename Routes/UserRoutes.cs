namespace AppCore.Routes;

// chưa dùng
public static class UserRoutes {
    public static void MapUserRoutes(this WebApplication app) {
        app.MapControllerRoute(
            name: "user",
            pattern: "User/{controller=Home}/{action=Index}/{id?}"
        );

    }
}