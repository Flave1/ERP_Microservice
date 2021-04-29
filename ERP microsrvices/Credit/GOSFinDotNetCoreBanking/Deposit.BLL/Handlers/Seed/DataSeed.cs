
using Deposit.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Deposit.Handlers.Deposit
{
    public static class DataSeed
    {
         
 
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();
  
            
            }
            return host;
        }
         
    }
}
