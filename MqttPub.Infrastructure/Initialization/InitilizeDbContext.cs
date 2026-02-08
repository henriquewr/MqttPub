using Microsoft.EntityFrameworkCore;

namespace MqttPub.Infrastructure.Initialization
{
    public class InitilizeDbContext
    {
        public static void Initialize(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }
    }
}
