using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;IDPList&gt; element specifies the identity providers trusted by the requester to authenticate the
    /// presenter.
    /// </summary>
    [GeneratedCode("xsd", "2.0.50727.42")]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class IDPList
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "IDPList";

        private string getCompleteField;
        private IDPEntry[] iDPEntryField;

        /// <summary>
        /// Gets or sets the IDP entry.
        /// Information about a single identity provider.
        /// </summary>
        /// <value>The IDP entry.</value>
        [XmlElement("IDPEntry")]
        public IDPEntry[] IDPEntry
        {
            get { return iDPEntryField; }
            set { iDPEntryField = value; }
        }

        /// <summary>
        /// Gets or sets the get complete.
        /// If the &lt;IDPList&gt; is not complete, using this element specifies a URI reference that can be used to
        /// retrieve the complete list. Retrieving the resource associated with the URI MUST result in an XML
        /// instance whose root element is an &lt;IDPList&gt; that does not itself contain a &lt;GetComplete&gt;
        /// element.
        /// </summary>
        /// <value>The get complete.</value>
        [XmlElement(DataType="anyURI")]
        public string GetComplete
        {
            get { return getCompleteField; }
            set { getCompleteField = value; }
        }
    }
}