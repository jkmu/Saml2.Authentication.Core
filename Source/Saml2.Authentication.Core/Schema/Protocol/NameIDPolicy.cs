using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;NameIDPolicy&gt; element tailors the name identifier in the subjects of assertions resulting from an
    /// &lt;AuthnRequest&gt;.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class NameIDPolicy
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "NameIDPolicy";

        private bool? allowCreateField;

        private string formatField;

        private string sPNameQualifierField;


        /// <summary>
        /// Gets or sets the format.
        /// Specifies the URI reference corresponding to a name identifier format defined in this or another
        /// specification (see Section 8.3 for examples). The additional value of
        /// urn:oasis:names:tc:SAML:2.0:nameid-format:encrypted is defined specifically for use
        /// within this attribute to indicate a request that the resulting identifier be encrypted.
        /// </summary>
        /// <value>The format.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        /// <summary>
        /// Gets or sets the SP name qualifier.
        /// Optionally specifies that the assertion subject's identifier be returned (or created) in the namespace of
        /// a service provider other than the requester, or in the namespace of an affiliation group of service
        /// providers.
        /// </summary>
        /// <value>The SP name qualifier.</value>
        [XmlAttribute]
        public string SPNameQualifier
        {
            get { return sPNameQualifierField; }
            set { sPNameQualifierField = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [allow create].
        /// A Boolean value used to indicate whether the identity provider is allowed, in the course of fulfilling the
        /// request, to create a new identifier to represent the principal. Defaults to "false". When "false", the
        /// requester constrains the identity provider to only issue an assertion to it if an acceptable identifier for
        /// the principal has already been established. Note that this does not prevent the identity provider from
        /// creating such identifiers outside the context of this specific request (for example, in advance for a
        /// large number of principals).
        /// </summary>
        /// <value><c>true</c> if [allow create]; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool? AllowCreate
        {
            get { return allowCreateField; }
            set { allowCreateField = value; }
        }

        /// <summary>
        /// Gets or sets the AllowCreate string.
        /// </summary>
        /// <value>The AllowCreate string.</value>
        [XmlAttribute(AttributeName = "AllowCreate")]
        public string AllowCreateString
        {
            get
            {
                if (allowCreateField.HasValue)
                    return allowCreateField.ToString();
                else
                    return null;
            }
            set {
                if (string.IsNullOrEmpty(value))
                    allowCreateField = null;
                else
                    allowCreateField = Convert.ToBoolean(value);
            }
        }
    }
}