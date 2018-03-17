using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
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
            {
                options.SignOutScheme = options.SignInScheme;
            }

            if (options.StateDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName);
                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            if (options.StringDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName, typeof(string).FullName);
                options.StringDataFormat = new SecureDataFormat<string>(new StringSerializer(), dataProtector);
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
}
