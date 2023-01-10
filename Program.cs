using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using _ConsoleApp1;

    /*
    //СТВОРЕННЯ GENERIC HOST
    static void Main()
    {
        var host = Host.CreateDefaultBuilder()
              .ConfigureServices((hostBuilderContext, configurationBuilder) =>
              {
                  hostBuilderContext.HostingEnvironment.EnvironmentName = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
                  var env = hostBuilderContext.HostingEnvironment;
                  configurationBuilder.AddEnvironmentVariables();
                  configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                  configurationBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
              })
               .ConfigureServices((hostBuilderContext, serviceCollection) =>
             {
             });
    }
}
          /*
                 // Connection/Transaction for ADO.NET/DAPPER database
                 serviceCollection.AddScoped((s) => new SqlConnection(hostBuilderContext.Configuration.GetConnectionString("MSSQLConnection")));
                 serviceCollection.AddScoped<IDbTransaction>(s =>
                 {
                     SqlConnection conn = s.GetRequiredService<SqlConnection>();
                     conn.Open();
                     return conn.BeginTransaction();
                 });
                 //Connection for EF database + DbContext
                 serviceCollection.AddDbContext<MyEventsDbContext>(options =>
                 {
                     string connectionString = hostBuilderContext.Configuration.GetConnectionString("MSSQLConnection");
                     options.UseSqlServer(connectionString);
                 });
                 // Dependendency Injection for Repositories/UOW from ADO.NET DAL
                 serviceCollection.AddScoped<IEventRepository, EventRepository>();
                 serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
                 serviceCollection.AddScoped<IUserProfileRepository, UserProfileRepository>();
                 serviceCollection.AddScoped<IGalleryRepository, GalleryRepository>();
                 serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
                 serviceCollection.AddScoped<IImageRepository, ImageRepository>();
                 serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
                 // Dependendency Injection for Repositories/UOW from EF DAL
                 serviceCollection.AddScoped<IEFEventRepository, EFEventRepository>();
                 serviceCollection.AddScoped<IEFCategoryRepository, EFCategoryRepository>();
                 serviceCollection.AddScoped<IEFUserProfileRepository, EFUserProfileRepository>();
                 serviceCollection.AddScoped<IEFGalleryRepository, EFGalleryRepository>();
                 serviceCollection.AddScoped<IEFMessageRepository, EFMessageRepository>();
                 serviceCollection.AddScoped<IEFImageRepository, EFImageRepository>();
                 serviceCollection.AddScoped<IEFUnitOfWork, EFUnitOfWork>();
                 //Forms
                 serviceCollection.AddSingleton<FormMainMenu>();
                 serviceCollection.AddSingleton<ServiceArgs>();
                 serviceCollection.AddTransient<AllEventsForm>();
                 serviceCollection.AddTransient<CategoryForm>();
                 serviceCollection.AddTransient<DetaisOfEventForm>();
                 serviceCollection.AddTransient<ForumForm>();
                 serviceCollection.AddTransient<GalleryForm>();
                 serviceCollection.AddTransient<ProfileForm>();
             })
    }
   }
              *//*

internal sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("Hello World!");

                    // Simulate real work is being done
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    // Stop the application once the work is done
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

*/
namespace _ConsoleApp1
    {
        class Program
        {

            static string connectionString = @"Data Source=WIN-HGKNVQ9J21J;Initial Catalog=BeautySalon;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            private static void Add() // Додавання до бази даних
            {
            }

            public List<Oll> GetOll()
            {
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                List<Oll> list = connection.Query<Oll>("SELECT * FROM Users").ToList<Oll>();
                connection.Close();
                foreach (Oll oll in list)
                {
                    Console.WriteLine($"{oll.LoginID} {oll.UsersFirstName.PadLeft(10)} {oll.UsersLastName} {oll.UsersEmail.PadLeft(8)} ");
                }
                return list;
            }


            // добавление пользователя
            private static void AddUser(string LoginID, string UsersFirstName, string UsersLastName, string UsersEmail)
            {
                // название процедуры
                string sqlExpression = "sp_InsertUser";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // параметр для ввода логіна
                    SqlParameter loginParam = new SqlParameter
                    {
                        ParameterName = "@LoginID",
                        Value = LoginID
                    };
                    // добавляем параметр
                    command.Parameters.Add(loginParam);

                    // параметр для ввода імя
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@UsersFirstName",
                        Value = UsersFirstName
                    };
                    // добавляем параметр
                    command.Parameters.Add(nameParam);


                    // параметр для ввода прізвище
                    SqlParameter LastNameParam = new SqlParameter
                    {
                        ParameterName = "@UsersLastName",
                        Value = UsersLastName
                    };
                    command.Parameters.Add(LastNameParam);

                    // параметр для ввода прізвище
                    SqlParameter EmailUser = new SqlParameter
                    {
                        ParameterName = "@UsersEmail",
                        Value = UsersEmail
                    };
                    command.Parameters.Add(EmailUser);


                    var result = command.ExecuteNonQuery();

                    Console.WriteLine("Id добавленного объекта: {0}", result);
                }
            }

            // вывод всех пользователей
            private static void GetUsers()
            {
                // название процедуры
                string sqlExpression = "sp_GetUsers";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));

                        while (reader.Read())
                        {

                            string LoginID = reader.GetString(0);
                            string UsersFirstName = reader.GetString(1);
                            string UsersLastName = reader.GetString(2);
                            string UsersEmail = reader.GetString(3);
                            Console.WriteLine("{0} \t{1} \t{2} \t{3}", LoginID, UsersFirstName, UsersLastName, UsersEmail);
                        }
                    }
                    reader.Close();
                }
            }




            private void ListEmail() // Відображення всієї інформації про пошту
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT UsersEmail FROM Users;");
                    foreach (var ListEmail in list)
                        Console.WriteLine(ListEmail.UsersEmail);
                }
            }



            private void ServiceInfo() // Відображення всієї інформації про пошту
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT Servicess,Price FROM Serviced");
                    foreach (var ServiceInfo in list)
                        Console.WriteLine(ServiceInfo.Servicess + " " + ServiceInfo.Price);
                }
            }


            private void CityInfo() // Відображення всієї інформації про пошту
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT CitySalon FROM SalonCity");
                    foreach (var CityInfo in list)
                        Console.WriteLine(CityInfo.CitySalon);
                }
            }


            private void MastersInfo() // Відображення всієї інформації про пошту
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT FirstName,LastName,MastersType FROM Masters");
                    foreach (var MastersInfo in list)
                        Console.WriteLine(MastersInfo.FirstName + " " + MastersInfo.FirstName + " " + MastersInfo.MastersType);
                }
            }

            private void MorePrice() // Відображення всієї інформації про пошту
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT Servicess,Price FROM Serviced where Price>250");
                    foreach (var MorePrice in list)
                        Console.WriteLine(MorePrice.Servicess + " " + MorePrice.Price);
                }
            }

            private void MinPrice() // Відображення про мінімальне
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT  min(Price) as MinPrice FROM Serviced ;");
                    foreach (var MinPrice in list)
                        Console.WriteLine(MinPrice.MinPrice);
                }
            }

            private void PasswordInfo() // Вiдобразити кiлькiсть покупцiв у кожному мiстi
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT LoginID,Passwordd FROM Passwordd");
                    foreach (var PasswordInfo in list)
                        Console.WriteLine(PasswordInfo.LoginID + " " + PasswordInfo.Passwordd);
                }
            }


            private void OrdeerInfo() // Вiдобразити кiлькiсть покупцiв у кожному мiстi
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT LoginID, COUNT(ServiceID) as Count_service FROM Ordeer GROUP BY  LoginID");
                    foreach (var OrdeerInfo in list)
                        Console.WriteLine(OrdeerInfo.LoginID + " " + OrdeerInfo.Count_service);
                }
            }

            private void OrdeerUsers() // Вiдобразити клієнта та послугу яку він обрав
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT Users.UsersFirstName ,Users.UsersLastName,Serviced.Servicess FROM Users ,Serviced,Ordeer WHERE Users.LoginID=Ordeer.LoginID and  Ordeer.ServiceID=Serviced.ServiceID;");
                    foreach (var OrdeerUsers in list)
                        Console.WriteLine(OrdeerUsers.UsersFirstName + " " + OrdeerUsers.UsersLastName + " " + OrdeerUsers.Servicess);
                }
            }

            private void Masters() // Вiдобразити майстра та місто
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                //SELECT Masters.FirstName ,Masters.LastName ,SalonCity.CitySalon FROM Masters  inner join SalonCity on  Masters.SalonID = Masters.SalonID;
                //запит можна виконати ще за допоиогю join

                var list = connection.Query("SELECT Masters.FirstName ,Masters.LastName ,SalonCity.CitySalon FROM Masters ,SalonCity WHERE Masters.SalonID=Masters.SalonID ;");
                    foreach (var MastersInfo in list)
                        Console.WriteLine(MastersInfo.FirstName + " " + MastersInfo.LastName + " " + MastersInfo.CitySalon);
                }
            }
            private void Users_Count_Order() // Вiдобразити майстра та місто
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT Users.UsersFirstName ,Users.UsersLastName,count(Serviced.Servicess) as Кiлькiсть FROM Users ,Serviced,Ordeer WHERE Users.LoginID=Ordeer.LoginID and  Ordeer.ServiceID=Serviced.ServiceID group by Users.UsersFirstName ,Users.UsersLastName   ORDER BY  Кiлькiсть; ");
                    foreach (var Users_Count_Order in list)
                        Console.WriteLine(Users_Count_Order.UsersFirstName + " " + Users_Count_Order.UsersLastName + " " + Users_Count_Order.Кiлькiсть);
                }
            }

            private void Users_Sum() // Вiдобразити суму послуг 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var list = connection.Query("SELECT Users.UsersFirstName ,Users.UsersLastName,sum(Serviced.Price) as Price_User  FROM Users ,Serviced,Ordeer  WHERE Users.LoginID=Ordeer.LoginID and  Ordeer.ServiceID=Serviced.ServiceID  group by Users.UsersFirstName ,Users.UsersLastName;");
                    foreach (var Users_Sum in list)
                        Console.WriteLine(Users_Sum.UsersFirstName + " " + Users_Sum.UsersLastName + " " + Users_Sum.Price_User);
                }
            }

        private void MasterCount() // Вiдобразити суму послуг 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var list = connection.Query("SELECT Masters.FirstName ,Masters.LastName ,COUNT(Serviced.Servicess) as servicCount FROM Masters ,Serviced where   Masters.ServiceID=Serviced.ServiceID group by Masters.FirstName ,Masters.LastName ;");
                foreach (var MasterCount in list)
                    Console.WriteLine(MasterCount.FirstName + " " + MasterCount.LastName + " " + MasterCount.servicCount);
            }
        }

        private static async Task Main(string[] args)
            {

                Console.WriteLine("1. Вiдображення всiх клiєнтiв");
                Console.WriteLine("2. Вiдображення email  всiх клiєнтiв");
                Console.WriteLine("3. Вiдображення спискок послуг та прайс  ");
                Console.WriteLine("4. Вiдображення список мiст з нашими салонами");
                Console.WriteLine("5. Вiдображення всiх майстрiв");
                Console.WriteLine("6. Вiдображення суми бiльше 250грн");
                Console.WriteLine("7. Вiдображення мiнiмальної цiни за послуги");//-
                Console.WriteLine("8. Вiдображення логiна та пароля");
                Console.WriteLine("9. Вiдображення користувача та кiлькiсть його послуг");
                Console.WriteLine("10. Вiдображення замовлення та користувача");
                Console.WriteLine("11. Вiдображення майстра та мiста в якому вiн працює ");
                Console.WriteLine("12. Добавити нового клiєнта");
                Console.WriteLine("13. Вiдображення клiєнта та  кiлькостi  його послуг");
                Console.WriteLine("14. Вiдображення цiни з кожного клiєнта ");
                Console.WriteLine("15. Вiдображення кiлькiсть послуг,  якi надає майстер ");

            var lu = new Program();
                int n = Convert.ToInt32(Console.ReadLine());

                while (n != 20)
                {
                    switch (n)
                    {
                        case 1:
                            lu.GetOll();
                            break;
                        case 2:
                            lu.ListEmail();
                            break;
                        case 3:
                            lu.ServiceInfo();
                            break;
                        case 4:
                            lu.CityInfo();
                            break;
                        case 5:
                            lu.MastersInfo();
                            break;
                        case 6:
                            lu.MorePrice();
                            break;
                        case 7:
                            lu.MinPrice();
                            break;
                        case 8:
                            lu.PasswordInfo();
                            break;
                        case 9:
                            lu.OrdeerInfo();
                            break;
                        case 10:
                            lu.OrdeerUsers();
                            break;
                        case 11:
                            lu.Masters();
                            break;
                        case 12:

                            Console.Write("Введіть логiн");
                            string LoginID = Console.ReadLine();

                            Console.Write("Введіть iмя:");
                            string UsersFirstName = Console.ReadLine();

                            Console.Write("Введить прiзвище:");
                            string UsersLastName = Console.ReadLine();

                            Console.Write("Введіть пошту клiєнта:");
                            string UsersEmail = Console.ReadLine();

                            AddUser(LoginID, UsersFirstName, UsersLastName, UsersEmail);
                            Console.WriteLine();
                            GetUsers();
                            Console.Read();
                            break;
                        case 13:
                            lu.Users_Count_Order();
                            break;
                        case 14:
                            lu.Users_Sum();
                            break;
                        case 15:
                        lu.MasterCount();
                        break;

                        
                        default:
                            Console.WriteLine("Error");
                            break;

                    }
                    n = Convert.ToInt32(Console.ReadLine());
                }


            }
        }
 }
