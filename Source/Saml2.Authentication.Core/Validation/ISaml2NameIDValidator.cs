namespace dk.nita.saml20.Validation
{
    using Schema.Core;
    using Schema.Protocol;

    internal interface ISaml2NameIDValidator
    {
        void ValidateNameID(NameID nameID);

        void ValidateEncryptedID(EncryptedElement encryptedID);
    }
}