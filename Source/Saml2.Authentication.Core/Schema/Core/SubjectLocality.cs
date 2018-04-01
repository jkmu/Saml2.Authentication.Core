using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;SubjectLocality&gt; element specifies the DNS domain name and IP address for the system from
    /// which the assertion subject was authenticated. It has the following attributes:
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class SubjectLocality
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SubjectLocality";

        private string addressField;

        private string dNSNameField;


        /// <summary>
        /// Gets or sets the address.
        /// The network address of the system from which the principal identified by the subject was
        /// authenticated. IPv4 addresses SHOULD be represented in dotted-decimal format (e.g., "1.2.3.4").
        /// IPv6 addresses SHOULD be represented as defined by Section 2.2 of IETF RFC 3513 [RFC 3513]
        /// (e.g., "FEDC:BA98:7654:3210:FEDC:BA98:7654:3210").
        /// </summary>
        /// <value>The address.</value>
        [XmlAttributeAttribute]
        public string Address
        {
            get { return addressField; }
            set { addressField = value; }
        }


        /// <summary>
        /// Gets or sets the name of the DNS.
        /// The DNS name of the system from which the principal identified by the subject was authenticated.
        /// </summary>
        /// <value>The name of the DNS.</value>
        [XmlAttributeAttribute]
        public string DNSName
        {
            get { return dNSNameField; }
            set { dNSNameField = value; }
        }
    }
}