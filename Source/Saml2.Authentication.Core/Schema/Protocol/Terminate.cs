using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The Saml20 protocol Terminate class
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace=Saml20Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.PROTOCOL, IsNullable=false)]
    public class Terminate
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Terminate";
    }
}