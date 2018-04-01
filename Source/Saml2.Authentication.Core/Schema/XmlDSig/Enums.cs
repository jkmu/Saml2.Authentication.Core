using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// ItemsChoice
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG, IncludeInSchema=false)]
    public enum ItemsChoiceType1
    {
        /// <summary>
        /// Item
        /// </summary>
        [XmlEnum("##any:")] Item,


        /// <summary>
        /// PGPKeyID
        /// </summary>
        PGPKeyID,


        /// <summary>
        /// PGPKeyPacket
        /// </summary>
        PGPKeyPacket,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG, IncludeInSchema=false)]
    public enum ItemsChoiceType
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("##any:")] Item,


        /// <summary>
        /// 
        /// </summary>
        X509CRL,


        /// <summary>
        /// 
        /// </summary>
        X509Certificate,


        /// <summary>
        /// 
        /// </summary>
        X509IssuerSerial,


        /// <summary>
        /// 
        /// </summary>
        X509SKI,


        /// <summary>
        /// 
        /// </summary>
        X509SubjectName,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG, IncludeInSchema=false)]
    public enum ItemsChoiceType2
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("##any:")] Item,


        /// <summary>
        /// 
        /// </summary>
        KeyName,


        /// <summary>
        /// 
        /// </summary>
        KeyValue,


        /// <summary>
        /// 
        /// </summary>
        MgmtData,


        /// <summary>
        /// 
        /// </summary>
        PGPData,


        /// <summary>
        /// 
        /// </summary>
        RetrievalMethod,


        /// <summary>
        /// 
        /// </summary>
        SPKIData,


        /// <summary>
        /// 
        /// </summary>
        X509Data,
    }
}