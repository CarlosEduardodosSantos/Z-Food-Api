using System;
using System.IO;
using System.Threading;
using APIAlturas.BackgroundServices;
using APIAlturas.ExtensionLogger;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace APIAlturas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<UsuariosCartaoConsumoDao>();
            services.AddTransient<GrupoConsumoDao>();
            services.AddTransient<FarmaciasDao>();
            services.AddTransient<EstacionamentoDAO>();
            services.AddTransient<HospedagemDAO>();
            services.AddTransient<CartaoDao>();
            services.AddTransient<CartaoConsumoDAO>();
            services.AddTransient<CategoriaDAO>();
            services.AddTransient<ComplementoDao>();
            services.AddTransient<CupomDao>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<FlyerDAO>();
            services.AddTransient<FormaPagamentoDao>();
            services.AddSingleton<GiftCardDAO>();
            services.AddTransient<GrupoProdutoDao>();
            services.AddTransient<LocalizacaoDao>();
            services.AddTransient<OneSignalService>();
            services.AddTransient<PasswordChangeRequestDao>();
            services.AddTransient<PaymentAutenticacaoDao>();
            services.AddTransient<PaymentRetornoDao>();
            services.AddTransient<PedidoDAO>();
            services.AddSingleton<PedidoUserZimmerDao>();
            services.AddTransient<ProdutoDao>();
            services.AddTransient<ProdutoOpcaoDao>();
            services.AddTransient<ProdutoShiftsDAO>();
            services.AddTransient<RepositorioLogger>();
            services.AddTransient<RestauranteDAO>();
            services.AddSingleton<RestauranteRatingDAO>();
            services.AddSingleton<RestauranteShiftsDao>();
            services.AddTransient<SetorDao>();
            services.AddTransient<UsersDAO>();
            services.AddTransient<ZcashMovimentacaoDao>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
        }));
            
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            
            services.AddMvc();

            services.AddSingleton<VerificarStatusParceiroServices>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "Produtos")),
                RequestPath = new PathString("/ProdutoImages")
            });

            app.UseHttpsRedirection();
            app.UseCorsMiddleware();
            app.UseCors("MyPolicy");
            app.UseMvc();

            app.ApplicationServices.GetService<VerificarStatusParceiroServices>().StartAsync(new CancellationToken());

        }
    }
}