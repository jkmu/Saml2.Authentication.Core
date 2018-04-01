using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;Scoping&gt; element specifies the identity providers trusted by the requester to authenticate the
    /// presenter, as well as limitations and context related to proxying of the &lt;AuthnRequest&gt; message to
    /// subsequent identity providers by the responder.
    /// </summary>
    [GeneratedCode("xsd", "2.0.50727.42")]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class Scoping
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Scoping";

        private IDPList iDPListField;

        private string proxyCountField;
        private string[] requesterIDField;


        /// <summary>
        /// Gets or sets the IDP list.
        /// An advisory list of identity providers and associated information that the requester deems acceptable
        /// to respond to the request.
        /// </summary>
        /// <value>The IDP list.</value>
        public IDPList IDPList
        {
            get { return iDPListField; }
            set { iDPListField = value; }
        }


        /// <summary>
        /// Gets or sets the requester ID.
        /// Identifies the set of requesting entities on whose behalf the requester is acting. Used to communicate
        /// the chain of requesters when proxying occurs, as described in Section 3.4.1.5. See Section 8.3.6 for a
        /// description of entity identifiers.
        /// </summary>
        /// <value>The requester ID.</value>
        [XmlElement("RequesterID", DataType="anyURI")]
        public string[] RequesterID
        {
            get { return requesterIDField; }
            set { requesterIDField = value; }
        }


        /// <summary>
        /// Gets or sets the proxy count.
        /// Specifies the number of proxying indirections permissible between the identity provider that receives
        /// this &lt;AuthnRequest&gt; and the identity provider who ultimately authenticates the principal. A count of
        /// zero permits no proxying, while omitting this attribute expresses no such restriction.
        /// </summary>
        /// <value>The proxy count.</value>
        [XmlAttribute(DataType="nonNegativeInteger")]
        public string ProxyCount
        {
            get { return proxyCountField; }
            set { proxyCountField = value; }
        }
    }
}