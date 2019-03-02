namespace dk.nita.saml20.Validation
{
    using Schema.Core;
    using Schema.Protocol;

    internal interface ISaml2AttributeValidator
    {
        void ValidateAttribute(SamlAttribute samlAttribute);

        void ValidateEncryptedAttribute(EncryptedElement encryptedElement);
    }
}