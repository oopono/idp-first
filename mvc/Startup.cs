using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices1(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                //Ĭ����֤����
                options.DefaultScheme = "Cookies";
                //Ĭ��token��֤ʧ�ܺ��ȷ����֤�������
                options.DefaultChallengeScheme = "oidc";
            })
            //�����һ����ΪCookies��Cookie��֤����
            .AddCookie("Cookies")
            //���OpenIdConnect��֤����
            .AddOpenIdConnect("oidc", options =>
            {
                //options.Authority = "http://localhost:5000";
                //options.RequireHttpsMetadata = false;
                //options.ClientId = "apiClientCode";
                //options.ClientSecret = "apiSecret";
                //options.ResponseType = "code";
                //options.Scope.Add("scope1"); //�����Ȩ��Դ
                //options.SaveTokens = true;
                //options.GetClaimsFromUserInfoEndpoint = true;

                //ָ��Զ����֤�����ı��ص�¼������
                options.SignInScheme = "Cookies";
                //ͨ�����ط��ʼ�Ȩ����
                options.Authority = "https://localhost:5555";
                //Httpsǿ��Ҫ���ʶ
                options.RequireHttpsMetadata = false;
                //�ͻ���ID��֧������ģʽ����Ȩ��ģʽ������ģʽ�Ϳͻ���ģʽ����Ҫ�û���¼��
                //ʹ������ģʽ
                options.ClientId = "apiClientImpl";
                //options.ClientId = "apiClientCode";                
                options.ClientSecret = "apiSecret";
                //���Ʊ����ʶ
                options.SaveTokens = true;
                //��ӷ���secretapi��api��Ȩ�ޣ�����access_token
                options.Scope.Add("scope1");
                //������Ȩ�û���PhoneModel Claim����id_token����
                options.Scope.Add("PhoneModel");
                options.Scope.Add("openid");

                //���󷵻�id_token�Լ�token
                options.ResponseType = OpenIdConnectResponseType.IdTokenToken;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            //});

            services.ConfigureNonBreakingSameSiteCookies();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "http://localhost:5004";
                options.RequireHttpsMetadata = false;
                options.ClientId = "blogadminjs";
                options.ClientSecret = "12345678";
                options.SaveTokens = true;
                options.Scope.Add("blog.core.api");
                options.Scope.Add("roles");
                options.Scope.Add("profile");
                options.Scope.Add("openid");
                options.ResponseType = OpenIdConnectResponseType.IdTokenToken;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.ConfigureNonBreakingSameSiteCookies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();
            
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
