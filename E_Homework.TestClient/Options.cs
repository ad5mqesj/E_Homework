using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Homework.TestClient
{
    public class Options
    {
        [Option('u', "converterurl", HelpText = "URL to connect to Converter service.")]
        public string Converter { get; set; } = string.Empty;

        [Option('f', "file", HelpText = "File to convert.")]
        public string InputFile { get; set; } = string.Empty;


        //non arguments options
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ResourceId { get; set; } = string.Empty;
        public string Authority
        {
            get { return $"{Instance}{TenantId}"; }
        }
    }
}
