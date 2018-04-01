using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Validation
{
    internal interface ISaml2AttributeValidator
    {
        void ValidateAttribute(SamlAttribute samlAttribute);

        void ValidateEncryptedAttribute(EncryptedElement encryptedElement);
    }
}