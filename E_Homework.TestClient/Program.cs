using CommandLine;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using E_Homework.DTO.Models;
using System.Collections.Generic;

namespace E_Homework.TestClient
{
    partial class Program
    {
        public static Serilog.ILogger? Logger { get; private set; }
        static async Task<int> Main(string[] args)
        {
            var config = new ConfigurationBuilder()
              .AddEnvironmentVariables()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();
            Logger = Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config, "Serilog")
                .Enrich.FromLogContext()
                .CreateLogger();

            Options options = config.GetRequiredSection("AzureAD").Get<Options>();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(op =>
                {
                    options.Converter = op.Converter;
                    options.InputFile = op.InputFile;
                });

#if NORMALAUTH
            string bearertoken = GetAuthToken(options, Logger).Result;
            if (string.IsNullOrEmpty(bearertoken))
            {
                Logger?.Error("No teken was returned");
                Console.WriteLine("No token - exiting");
                return -1;
            }
#endif
            HttpClient client = new HttpClient();
#if NORMALAUTH

            var defaultRequestHeaders = client.DefaultRequestHeaders;
            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearertoken);
#endif
            var url = options.Converter.EndsWith("/") ? options.Converter.Remove(options.Converter.Length - 1, 1) : options.Converter;

            Uri uri = new Uri($"{url}/ConvertData");
            string data = File.ReadAllText(options.InputFile);
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, data);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<CommonDeviceData>? cdl = response.Content.ReadFromJsonAsync<List<CommonDeviceData>>().Result;
                if (cdl != null)
                    foreach (CommonDeviceData cd in cdl)
                        Console.WriteLine(cd.ToString());
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
            else
                Console.WriteLine($"ConvertData failed : {response.StatusCode}");
            return 0;
        }

        static async Task<string> GetAuthToken(Options config, ILogger logger)
        {
            bool isUsingClientSecret = AppUsesClientSecret(config.ClientSecret);
            IConfidentialClientApplication? app = null;

            if (isUsingClientSecret)
            {
                string authority = string.Concat(config.Instance, config.TenantId, "/oauth2/token");
                logger?.Information("isUsingClientSecret is true.");
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();
            }
            else
            {
                logger?.Error("Certificated authentication not supported!");
#pragma warning disable S3928 // param passed as part of thread context structure in timer handler
                throw (new ArgumentNullException("ClientSecret"));
#pragma warning restore S3928 // param passed as part of thread context structure in timer handler
            }
            string[] scopes = new string[] { $"{config.ClientId}/.default" };

            AuthenticationResult? result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            }
            catch (Exception e)
            {
                logger?.Error(e.Message);
            }
            return result?.AccessToken ?? String.Empty;
        }

        private static bool AppUsesClientSecret(string? ClientSecret)
        {
            string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";

            if (!String.IsNullOrWhiteSpace(ClientSecret) && ClientSecret != clientSecretPlaceholderValue)
            {
                return true;
            }
            else
                return false;
        }

        private static string GetFirstV4Address()
        {
            var list = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            var v4Addr = (from addr in list where addr.AddressFamily == AddressFamily.InterNetwork select addr).FirstOrDefault();
            return v4Addr?.ToString() ?? "127.0.0.1";
        }
    }

}