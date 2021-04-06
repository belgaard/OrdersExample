using System;
using System.Net.Http;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Orders.ExternalDependencies;
using Orders.InstrumentCache;
using Orders.Orders.PlaceOrder;
using Orders.Qte;
using PlaceOrderRequest = Orders.Qte.PlaceOrderRequest;

namespace Orders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "orders", Version = "v1" });
            });

            // TODO: Move QTE stuff to its own composition root!
            services.AddSingleton<PlaceOrderRequestHandler>();
            services.AddSingleton<Qte.Qte>();
            services.AddSingleton<PlaceOrderRequestHandler>();
            services.AddSingleton<QteOrderMapper>();
            services.AddSingleton<ISessionTradesFacade, SessionTradesFacade>();
            services.AddSingleton<ISessionTrading, SessionTrading>();
            services.AddSingleton<IInstrumentCacheFacade, InstrumentCacheFacade>();

            services.AddSingleton(ChannelFactory);

            static Tbl.Protos.Tbl.TblClient ChannelFactory(IServiceProvider a)
            {
                HttpClient httpClient = new HttpClient {BaseAddress = new Uri("https://localhost:5001/")};
                return new Tbl.Protos.Tbl.TblClient(GrpcChannel.ForAddress(httpClient.BaseAddress,
                    new GrpcChannelOptions {HttpClient = httpClient}));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "orders v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Tbl.Protos.PlaceOrderRequest, PlaceOrderRequest>().ReverseMap(); // TODO: Is it worth it?
        }
    }
}
