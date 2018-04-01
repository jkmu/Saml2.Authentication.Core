using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The Saml20 StatementAbstract class. It's the baseclass for all statements in Saml20.
    /// </summary>
    [XmlInclude(typeof (AttributeStatement))]
    [XmlIncludeAttribute(typeof (AuthzDecisionStatement))]
    [XmlIncludeAttribute(typeof (AuthnStatement))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public abstract class StatementAbstract
    {        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Statement";
    }
}