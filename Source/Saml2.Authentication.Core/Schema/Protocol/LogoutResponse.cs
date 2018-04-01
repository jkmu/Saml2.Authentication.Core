using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The SAML LogoutResponse
    /// </summary>
    [Serializable]
    [XmlType(Namespace = Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.PROTOCOL, IsNullable = false)]
    public class LogoutResponse : StatusResponse
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "LogoutResponse";
    }
}