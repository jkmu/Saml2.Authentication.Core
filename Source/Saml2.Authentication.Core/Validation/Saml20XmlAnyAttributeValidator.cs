using System;
using System.Xml;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Validation
{
    internal class Saml20XmlAnyAttributeValidator
    {
        public void ValidateXmlAnyAttributes(XmlAttribute[] anyAttributes)
        {
            if (anyAttributes == null) throw new ArgumentNullException("anyAttributes");

            if (anyAttributes.Length == 0)
                return;

            foreach (XmlAttribute attr in anyAttributes)
            {
                if (!Saml20Utils.ValidateRequiredString(attr.Prefix))
                    throw new Saml20FormatException("Attribute extension xml attributes MUST BE namespace qualified");

                foreach (string samlns in Saml20Constants.SAML_NAMESPACES)
                {
                    if (attr.NamespaceURI == samlns)
                        throw new Saml20FormatException("Attribute extension xml attributes MUST NOT use a namespace reserved by SAML");
                }
            }
        }

    }
}
