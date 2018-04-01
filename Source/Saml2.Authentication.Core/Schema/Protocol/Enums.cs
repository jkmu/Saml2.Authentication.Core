using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    public enum AuthnContextComparisonType
    {
        /// <summary>
        /// 
        /// </summary>
        exact,


        /// <summary>
        /// 
        /// </summary>
        minimum,


        /// <summary>
        /// 
        /// </summary>
        maximum,


        /// <summary>
        /// 
        /// </summary>
        better,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL, IncludeInSchema=false)]
    public enum ItemsChoiceType7
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("urn:oasis:names:tc:SAML:2.0:assertion:AuthnContextClassRef")] AuthnContextClassRef,


        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("urn:oasis:names:tc:SAML:2.0:assertion:AuthnContextDeclRef")] AuthnContextDeclRef,
    }
}