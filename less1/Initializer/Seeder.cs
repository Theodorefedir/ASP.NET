using less1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace less1.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            context.Database.Migrate();

            if (!context.Categories.Any())
            {

                var filePath = Path.Combine(env.WebRootPath, "seedData", "CategoriesAndProducts.json");

                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);

                var categories = JsonSerializer.Deserialize<List<Category>>(json);

                if (categories != null)
                {
                    context.Categories.AddRange(categories);
                    context.SaveChanges();                    
                }
                //List<Category> categories = new List<Category>
                //{
                //    new Category
                //    {
                //        Name = "Процесори (CPU)",
                //        Description = "Центральні процесори для настільних ПК та серверів.",
                //        Image = "https://s.ek.ua/posts/files/6001/wide_pic.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Материнські плати",
                //        Description = "Плти для об'єднання всіх компонентів системи в єдине ціле.",
                //        Image = "https://s.ek.ua/posts/files/6619/wide_pic.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Оперативна пам'ять (RAM)",
                //        Description = "Модулі швидкої тимчасової пам'яті (DDR4, DDR5).",
                //        Image = "https://img.moyo.ua/img/news_desc/1663/166351_1653562382_0.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Відеокарти (GPU)",
                //        Description = "Графічні адаптери для ігор, рендерингу та обробки графіки.",
                //        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRMrlqSfU0wP5Pq8f20kzIUBgPSo9jWYdLqNJH_SyHhrJel18he8go2tsOH&s=10"
                //    },
                //    new Category
                //    {
                //        Name = "Накопичувачі (SSD / HDD)",
                //        Description = "Твердотільні та жорсткі диски для зберігання даних.",
                //        Image = "https://gamingpc.com.ua/image/cache/blog/u-in2dt7es-700x394.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Блоки живлення",
                //        Description = "Пристрої для забезпечення стабільного живлення всіх компонентів ПК.",
                //        Image = "https://s.ek.ua/posts/files/6732/wide_pic.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Корпуси",
                //        Description = "Захисні шасі для зручного та безпечного розміщення комплектуючих.",
                //        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ6kq1AGmaOhev6OtoOa2GOSQAeD5kkAjGxOOuD7i3jNHuzAjVSK9rx5YA&s=10"
                //    },
                //    new Category
                //    {
                //        Name = "Системи охолодження",
                //        Description = "Кулери, радіатори та комплекти рідинного охолодження (СЖО).",
                //        Image = "https://hotline.ua/img/uploads/clients/%D0%BE%D0%B7%D0%BB%D0%B0%D0%B6%D0%B4%D0%B5%D0%BD%D0%B8%D0%B5=5ee8a886018b5.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Звукові карти",
                //        Description = "Дискретні пристрої для виведення та обробки високоякісного звуку.",
                //        Image = "https://mychooz.com/wp-content/uploads/2020/06/09-1.jpg"
                //    },
                //    new Category
                //    {
                //        Name = "Мережеві адаптери",
                //        Description = "Wi-Fi модулі та дротові мережеві карти (Ethernet) для підключення до інтернету.",
                //        Image = "https://server-shop.ua/assets/images/resources/21692/catalog/d38ad1a1e5713ec78af07d566a627f31523c2e00.jpg"
                //    }
                //};

                //context.Categories.AddRange(categories);
                //context.SaveChanges();
            }
        }
    }
}
