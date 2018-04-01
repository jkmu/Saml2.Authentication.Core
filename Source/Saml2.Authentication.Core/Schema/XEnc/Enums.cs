using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// ItemsChoice for Referencelists
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC, IncludeInSchema=false)]
    public enum ItemsChoiceType3
    {
        /// <summary>
        /// DataReference
        /// </summary>
        DataReference,


        /// <summary>
        /// KeyReference
        /// </summary>
        KeyReference,
    }
}