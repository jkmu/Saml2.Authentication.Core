using System;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Validation
{
    internal class Saml2AttributeValidator : ISaml2AttributeValidator
    {
        private Saml2XmlAnyAttributeValidator _anyAttrValidator;
        public Saml2XmlAnyAttributeValidator AnyAttrValidator
        {
            get
            {
                if (_anyAttrValidator != null)
                    return _anyAttrValidator;

                _anyAttrValidator = new Saml2XmlAnyAttributeValidator();
                return _anyAttrValidator;
            }
        }

        private Saml2EncryptedElementValidator _encElemValidator = new Saml2EncryptedElementValidator();
        public Saml2EncryptedElementValidator EncElemValidator
        {
            get
            {
                if (_encElemValidator != null)
                    return _encElemValidator;

                _encElemValidator = new Saml2EncryptedElementValidator();
                return _encElemValidator;
            }
        }

        /// <summary>
        /// [SAML2.0std] section 2.7.3.1
        /// </summary>
        public void ValidateAttribute(SamlAttribute samlAttribute)
        {
            if (samlAttribute == null) throw new ArgumentNullException("samlAttribute");

            if (!Saml2Utils.ValidateRequiredString(samlAttribute.Name))
                throw new Saml2FormatException("Name attribute of Attribute element MUST contain at least one non-whitespace character");
            
            if (samlAttribute.AttributeValue != null)
            {
                foreach (object o in samlAttribute.AttributeValue)
                {
                    if (o == null)
                        throw new Saml2FormatException("null-AttributeValue elements are not supported");
                }
            }

            if (samlAttribute.AnyAttr != null)
                AnyAttrValidator.ValidateXmlAnyAttributes(samlAttribute.AnyAttr);
        }

        /// <summary>
        /// [SAML2.0std] section 2.7.3.2
        /// </summary>
        public void ValidateEncryptedAttribute(EncryptedElement encryptedElement)
        {
            EncElemValidator.ValidateEncryptedElement(encryptedElement, "EncryptedAttribute");
        }
    }
}