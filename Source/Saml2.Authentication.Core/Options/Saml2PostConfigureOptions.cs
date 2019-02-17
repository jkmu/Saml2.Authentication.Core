using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Options
{
    public class Saml2PostConfigureOptions : IPostConfigureOptions<Saml2Options>
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public Saml2PostConfigureOptions(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public void PostConfigure(string name, Saml2Options options)
        {
            options.DataProtectionProvider = options.DataProtectionProvider ?? _dataProtectionProvider;

            if (string.IsNullOrEmpty(options.SignOutScheme))
                options.SignOutScheme = options.SignInScheme;

            if (options.StateDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName);
                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            if (options.StringDataFormat == null)
            {
                var dataProtector =
                    options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName,
                        typeof(string).FullName);
                options.StringDataFormat = new SecureDataFormat<string>(new StringSerializer(), dataProtector);
            }

            if (options.ObjectDataFormat == null)
            {
                var dataProtector =
                    options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName,
                        typeof(object).FullName);
                options.ObjectDataFormat = new SecureDataFormat<object>(new ObjectSerializer(), dataProtector);
            }
        }
    }

    internal class StringSerializer : IDataSerializer<string>
    {
        public string Deserialize(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public byte[] Serialize(string model)
        {
            return Encoding.UTF8.GetBytes(model);
        }
    }

    internal class ObjectSerializer : IDataSerializer<object>
    {
        public byte[] Serialize(object model)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
        }

        public object Deserialize(byte[] data)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), typeof(object));
        }
    }
}