using System;
using System.Xml;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Validation
{
    internal class Saml2XmlAnyAttributeValidator
    {
        public void ValidateXmlAnyAttributes(XmlAttribute[] anyAttributes)
        {
            if (anyAttributes == null) throw new ArgumentNullException("anyAttributes");

            if (anyAttributes.Length == 0)
                return;

            foreach (XmlAttribute attr in anyAttributes)
            {
                if (!Saml2Utils.ValidateRequiredString(attr.Prefix))
                    throw new Saml2FormatException("Attribute extension xml attributes MUST BE namespace qualified");

                foreach (string samlns in Saml2Constants.SAML_NAMESPACES)
                {
                    if (attr.NamespaceURI == samlns)
                        throw new Saml2FormatException("Attribute extension xml attributes MUST NOT use a namespace reserved by SAML");
                }
            }
        }

    }
}
