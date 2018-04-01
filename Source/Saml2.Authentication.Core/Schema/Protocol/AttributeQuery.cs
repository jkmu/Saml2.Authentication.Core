using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;AttributeQuery&gt; element is used to make the query "Return the requested attributes for this
    /// subject." A successful response will be in the form of assertions containing attribute statements, to the
    /// extent allowed by policy.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class AttributeQuery : SubjectQueryAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AttributeQuery";

        private SamlAttribute[] samlAttributeField;


        /// <summary>
        /// Gets or sets the attribute.
        /// Each &lt;saml:Attribute&gt; element specifies an attribute whose value(s) are to be returned. If no
        /// attributes are specified, it indicates that all attributes allowed by policy are requested. If a given
        /// &lt;saml:Attribute&gt; element contains one or more &lt;saml:AttributeValue&gt; elements, then if
        /// that attribute is returned in the response, it MUST NOT contain any values that are not equal to the
        /// values specified in the query. In the absence of equality rules specified by particular profiles or
        /// attributes, equality is defined as an identical XML representation of the value
        /// </summary>
        /// <value>The attribute.</value>
        [XmlElement("Attribute", Namespace=Saml2Constants.ASSERTION)]
        public SamlAttribute[] SamlAttribute
        {
            get { return samlAttributeField; }
            set { samlAttributeField = value; }
        }
    }
}