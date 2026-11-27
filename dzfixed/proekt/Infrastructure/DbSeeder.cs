using WebApplication3.Domain;

namespace WebApplication3.Infrastructure
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Customers.Any())
            {
                var blocked = new Customer("blocked@example.com", "Заблокированный клиент");
                blocked.Block();

                db.Customers.AddRange(
                    new Customer("user@example.com", "Иван Иванов"),
                    blocked);
            }

            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product("Ноутбук", 59990m, 10),
                    new Product("Мышь", 990m, 50),
                    new Product("Клавиатура", 2490m, 30),
                    new Product("Монитор", 15990m, 5));
            }

            db.SaveChanges();
        }
    }
}
