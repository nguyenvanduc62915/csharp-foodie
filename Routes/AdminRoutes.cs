namespace AppCore.Routes;

public static class AdminRoutes {
    public static void MapAdminRoutes(this WebApplication app) {
        app.MapControllerRoute(
            name: "admin",
            pattern: "{controller=Admin}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "admin.category",
            pattern: "/Admin/{controller=Category}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "admin.product",
            pattern: "/Admin/{controller=Product}/{action=Index}/{id?}"
        );
    }
}