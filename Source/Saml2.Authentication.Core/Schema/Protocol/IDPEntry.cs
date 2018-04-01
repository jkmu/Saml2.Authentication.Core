using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;IDPEntry&gt; element specifies a single identity provider trusted by the requester to authenticate the
    /// presenter.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class IDPEntry
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "IDPEntry";

        private string locField;
        private string nameField;
        private string providerIDField;


        /// <summary>
        /// Gets or sets the provider ID.
        /// The unique identifier of the identity provider.
        /// </summary>
        /// <value>The provider ID.</value>
        [XmlAttribute(DataType="anyURI")]
        public string ProviderID
        {
            get { return providerIDField; }
            set { providerIDField = value; }
        }


        /// <summary>
        /// Gets or sets the name.
        /// A human-readable name for the identity provider
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }


        /// <summary>
        /// Gets or sets the loc.
        /// A URI reference representing the location of a profile-specific endpoint supporting the authentication
        /// request protocol. The binding to be used must be understood from the profile of use.
        /// </summary>
        /// <value>The loc.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Loc
        {
            get { return locField; }
            set { locField = value; }
        }
    }
}