using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using bdapiaaa.Models;
using Microsoft.AspNetCore;

namespace bdapiaaa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BdapiContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BdapiContext")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "bdapiaaa", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "bdapiaaa v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class DebtorsController : ControllerBase
    {
        private readonly BdapiContext _context;

        public DebtorsController(BdapiContext context)
        {
            _context = context;
        }

        [HttpGet("api/debtors")]
        public ActionResult<IEnumerable<Debtor>> GetDebtors()
        {
            var debtors = _context.Abonents.Select(a => new Debtor
            {
                Id = a.Id,
                LastName = a.LastName,
                PhoneNumber = a.PhoneNumber,
                MonthlyPayment = a.MonthlyPayment,
                DebtOrOverpayment = _context.PaymentRegistrations.Where(pr => pr.PhoneNumber == a.PhoneNumber).Sum(pr => pr.DebtOrOverpayment)
            }).ToList();
            return debtors;
        }

        [HttpGet("api/debtors/{phoneNumber}")]
        public ActionResult<Debtor> GetDebtor(string phoneNumber)
        {
            var debtor = _context.Abonents.FirstOrDefault(a => a.PhoneNumber == phoneNumber);
            if (debtor == null)
            {
                return NotFound();
            }
            var debtOrOverpayment = _context.PaymentRegistrations.Where(pr => pr.PhoneNumber == phoneNumber).Sum(pr => pr.DebtOrOverpayment);
            return new Debtor
            {
                Id = debtor.Id,
                LastName = debtor.LastName,
                PhoneNumber = debtor.PhoneNumber,
                MonthlyPayment = debtor.MonthlyPayment,
                DebtOrOverpayment = debtOrOverpayment
            };
        }
    }

    public class Debtor
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? MonthlyPayment { get; set; }
        public decimal? DebtOrOverpayment { get; set; }
    }
}