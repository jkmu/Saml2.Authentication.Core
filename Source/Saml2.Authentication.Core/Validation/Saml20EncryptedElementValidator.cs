using System;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Validation
{
    internal class Saml20EncryptedElementValidator
    {
        public void ValidateEncryptedElement(EncryptedElement encryptedElement, string parentNodeName)
        {
            if (encryptedElement == null) throw new ArgumentNullException("encryptedElement");

            if (encryptedElement.encryptedData == null)
                throw new Saml20FormatException(String.Format("An {0} MUST contain an xenc:EncryptedData element", parentNodeName));

            if (encryptedElement.encryptedData.Type != null
                && !string.IsNullOrEmpty(encryptedElement.encryptedData.Type)
                && encryptedElement.encryptedData.Type != Saml20Constants.XENC + "Element")
                throw new Saml20FormatException(String.Format("Type attribute of EncryptedData MUST have value {0} if it is present", Saml20Constants.XENC + "Element"));
            
        }
    }
}
