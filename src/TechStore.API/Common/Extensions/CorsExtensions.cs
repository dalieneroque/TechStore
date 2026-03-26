namespace TechStore.API.Common.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorFrontend", policy =>
                {
                    policy.WithOrigins("https://lojatechstore.netlify.app")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });

                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins("https://localhost:7258", "http://localhost:5151")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // escolhe automaticamente o ambiente
            if (env.IsDevelopment())
            {
                app.UseCors("AllowLocalhost");
            }
            else
            {
                app.UseCors("AllowBlazorFrontend");
            }

            return app;
        }
    }
}
