using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Saml2.Authentication.Core.Authentication;
using System.Text;

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

            if (options.ObjectDataFormat == null)
            {
                var dataProtector =
                    options.DataProtectionProvider.CreateProtector(typeof(Saml2Handler).FullName,
                        typeof(object).FullName);
                options.ObjectDataFormat = new SecureDataFormat<object>(new ObjectSerializer(), dataProtector);
            }
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