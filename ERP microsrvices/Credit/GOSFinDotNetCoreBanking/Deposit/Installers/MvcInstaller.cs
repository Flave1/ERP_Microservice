﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FluentValidation.AspNetCore;
using System;
using Deposit.AuthHandler.Inplimentation;
using Deposit.AuthHandler.Interface;
using System.Net.Http.Headers;
using GOSLibraries.Options;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.URI;
using Support.SDK;
using Deposit.Managers.InterfaceManagers;
using Deposit.Managers.Interface.temp;

namespace Deposit.Installers
{
    public class MvcInstaller : IInstaller
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            var tokenValidatorParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
            };

            services.AddSingleton(tokenValidatorParameters);
             
            services.AddScoped<IFilesHandlerService, FilesHandlerService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IBaseURIs>(configuration.GetSection("BaseURIs").Get<BaseURIs>());

           

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false; 
                options.Filters.Add<ValidationFilter>();
                
            })
                .AddFluentValidation(mvcConfuguration => mvcConfuguration.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddDetection();
            var baseuri = new BaseURIs();
            configuration.GetSection(nameof(BaseURIs)).Bind(baseuri);

            services.AddHttpClient("GOSDEFAULTGATEWAY", client =>
            {
                client.BaseAddress = new Uri(baseuri.LiveGateway);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            }); 
            //FLUTTERWAVE
            services.AddHttpClient("FLUTTERWAVE", client =>
            {
                client.BaseAddress = new Uri(baseuri.FlutterWave);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //string secretKeys = configuration.GetValue<string>("FlutterWaveKeys:live_secret_keys");
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + secretKeys);
            });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidatorParameters;
            });
             

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

           

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GODP Banking Cloud Solutions",
                    Version = "V1",
                    Description = "An API to perform business automated operations",
                    TermsOfService = new Uri("http://www.godp.co.uk/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Etim Essang",
                        Email = "etim.essang@godp.com.uk",
                        Url = new Uri("https://twitter.com/FavourE65881201"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GODP API LICX",
                        Url = new Uri("http://www.godp.co.uk/"),
                    },

                });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //x.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer ", new string[0] }
                };
                x.AddSecurityDefinition("Bearer ", new OpenApiSecurityScheme
                {
                    Description = "GODP Cloud Authorization header using bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {new OpenApiSecurityScheme {Reference = new OpenApiReference
                    {
                        Id = "Bearer ",
                        Type = ReferenceType.SecurityScheme
                    } }, new List<string>() }
                });
            });
        }
    }
}
