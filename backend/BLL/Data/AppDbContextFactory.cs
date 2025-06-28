using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BLL.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Получаем базовый путь (папка WebAPI)
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebAPI");
        
        // Конфигурируем построитель конфигурации
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        // Создаем построитель опций
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Получаем строку подключения
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Конфигурируем провайдер БД
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}