using Microsoft.Extensions.Configuration;

namespace E_Homework
{
    public interface ICommonSettings
    {
        string ClientId { get; set; }
        string Instance { get; set; }
        string TenantId { get; set; }
        string AIConnectionString { get; set; }
        string InstrumentationKey { get; set; }
        string GetValueFromKey(string sectionsName, string keyName);
    }

    public class CommonSettings : ICommonSettings
    {
        public static readonly string sectionName = "AppSettings";
        private readonly IConfiguration ? configuration;

        public CommonSettings()
        {

        }

        public CommonSettings (IConfiguration config)
        {
            configuration = config;
            this.ClientId = GetValueFromKey(sectionName, "ClientId");
            this.Instance = GetValueFromKey(sectionName, "Instance");
            this.TenantId = GetValueFromKey(sectionName, "TenantId");
            this.AIConnectionString = GetValueFromKey(sectionName, "AIConnectionString");
            this.InstrumentationKey = GetValueFromKey(sectionName, "InstrumentationKey");
        }

        public string ClientId { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string AIConnectionString { get; set; } = string.Empty;
        public string InstrumentationKey { get; set; } = string.Empty;

        public string Authority
        {
            get
            {
                return $"{Instance}{TenantId}";
            }
        }

        public string GetValueFromKey(string sectionsName, string keyName)
        {
            return this.configuration?.GetValue<string>($"{sectionName}:{keyName}")??string.Empty;
        }
    }
}
