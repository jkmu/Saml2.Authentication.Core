namespace dk.nita.saml20.Schema.Core
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using Utils;

    /// <summary>
    /// The &lt;SubjectConfirmationData&gt; element has the SubjectConfirmationDataType complex type. It
    /// specifies additional data that allows the subject to be confirmed or constrains the circumstances under
    /// which the act of subject confirmation can take place. Subject confirmation takes place when a relying
    /// party seeks to verify the relationship between an entity presenting the assertion (that is, the attesting
    /// entity) and the subject of the assertion's claims.
    /// </summary>
    [XmlInclude(typeof(KeyInfoConfirmationData))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class SubjectConfirmationData
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SubjectConfirmationData";

        /// <summary>
        /// Gets or sets the not on or after.
        /// </summary>
        /// <value>The not on or after.</value>
        [XmlIgnore]
        public DateTime? NotOnOrAfter { get; set; }

        /// <summary>
        /// Gets or sets the not on or after string.
        /// </summary>
        /// <value>The not on or after string.</value>
        [XmlAttribute("NotOnOrAfter")] 
        public string NotOnOrAfterString
        {
            get => NotOnOrAfter.HasValue ? Saml2Utils.ToUtcString(NotOnOrAfter.Value) : null;

            set => NotOnOrAfter = string.IsNullOrEmpty(value) ? (DateTime?) null : Saml2Utils.FromUtcString(value);
        }

        /// <summary>
        /// Gets or sets the not before.
        /// </summary>
        /// <value>The not before.</value>
        [XmlIgnore]
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the not on or after string.
        /// </summary>
        /// <value>The not on or after string.</value>
        [XmlAttribute("NotBefore")]
        public string NotBeforeString
        {
            get => NotBefore.HasValue ? Saml2Utils.ToUtcString(NotBefore.Value) : null;

            set => NotBefore = string.IsNullOrEmpty(value) ? null : (DateTime?)Saml2Utils.FromUtcString(value);
        }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [XmlAttribute("Recipient", DataType = "anyURI")]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets the in response to.
        /// </summary>
        /// <value>The in response to.</value>
        [XmlAttribute("InResponseTo", DataType = "NCName")]
        public string InResponseTo { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [XmlAttribute("Address", DataType = "string")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets any attr.
        /// </summary>
        /// <value>Any attr.</value>
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }

        /// <summary>
        /// Gets or sets the any-elements-array.
        /// </summary>
        /// <value>The any-elements-array</value>
        [XmlAnyElement]
        public XmlElement[] AnyElements { get; set; }
    }
}