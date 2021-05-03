using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/*
    URL	Что делает
    /	
    Выводит первую страницу списка товаров всех категорий

    /Page2	
    Выводит указанную страницу (в этом случае страницу 2), отображая товары всех категорий

    /Симулятор	
    Отображает первую страницу элементов указанной категории (в этом случае игры в разделе "Симуляторы")

    /Симулятор/Page2	
    Отображает заданную страницу (в этом случае страницу 2) элементов указанной категории (Симулятор)
*/

namespace SportStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Product", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{category}",
                new { controller = "Product", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Product", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}