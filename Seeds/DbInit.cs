using eTaskAdvisor.WebApi.Data;

namespace eTaskAdvisor.WebApi.Seeds.Archived {
    public static class DbInit
    {
        public static void Initialize(AppDbContext context)
        {
            /*
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Clients.Any())
            {
                return;
            }

            var clients = new Client[]
            {
                new Client{Name="Client 1", Password="abc1"},
                new Client{Name="Client 2", Password="abc2"}
            };

            foreach (var client in clients)
            {
                context.Clients.Add(client);
            }
            context.SaveChanges();
            */
        }
    }
}