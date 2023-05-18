namespace RenderBlazorInExistingApp.Server
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        private readonly string CustomAllowedOrigins = "_customAllowedOrigins";

        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddCors(options =>
            {
                options.AddPolicy(name: CustomAllowedOrigins, builder =>
                {
                    builder.WithOrigins("https://localhost:7163", "https://localhost:5001", "http://localhost/");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(CustomAllowedOrigins);

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
