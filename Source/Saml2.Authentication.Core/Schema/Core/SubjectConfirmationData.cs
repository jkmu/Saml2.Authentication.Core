using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Core
{
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

        private DateTime? notOnOrAfterField;
        /// <summary>
        /// Gets or sets the not on or after.
        /// </summary>
        /// <value>The not on or after.</value>
        [XmlIgnore] 
        public DateTime? NotOnOrAfter
        {
            get {
                return notOnOrAfterField;
            }
            set { notOnOrAfterField = value; }
        }

        /// <summary>
        /// Gets or sets the not on or after string.
        /// </summary>
        /// <value>The not on or after string.</value>
        [XmlAttribute("NotOnOrAfter")] 
        public string NotOnOrAfterString
        {
            get 
            { 
                if(notOnOrAfterField.HasValue)
                {
                    return Saml2Utils.ToUTCString(notOnOrAfterField.Value);
                }
                else
                {
                    return null;
                }
            }
            set 
            {
                if(string.IsNullOrEmpty(value))
                    notOnOrAfterField = null;
                else
                    notOnOrAfterField = Saml2Utils.FromUTCString(value);
            }
        }

        private DateTime? notBeforeField;

        /// <summary>
        /// Gets or sets the not before.
        /// </summary>
        /// <value>The not before.</value>
        [XmlIgnore]
        public DateTime? NotBefore
        {
            get { return notBeforeField; }
            set { notBeforeField = value;}
        }

        /// <summary>
        /// Gets or sets the not on or after string.
        /// </summary>
        /// <value>The not on or after string.</value>
        [XmlAttribute("NotBefore")]
        public string NotBeforeString
        {
            get
            {
                if(notBeforeField.HasValue)
                {
                    return Saml2Utils.ToUTCString(notBeforeField.Value);
                }else
                {
                    return null;
                }
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                    notBeforeField = null;
                else
                    notBeforeField = Saml2Utils.FromUTCString(value);
            }
        }


        private string recipientField;

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [XmlAttribute("Recipient", DataType = "anyURI")] 
        public string Recipient
        {
            get { return recipientField; }
            set{ recipientField = value;}
        }

        private string inResponseToField;

        /// <summary>
        /// Gets or sets the in response to.
        /// </summary>
        /// <value>The in response to.</value>
        [XmlAttribute("InResponseTo", DataType = "NCName")] 
        public string InResponseTo
        {
            get { return inResponseToField; }
            set { inResponseToField = value; }
        }

        private string addressField;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [XmlAttribute("Address", DataType = "string")] 
        public string Address
        {
            get { return addressField; }
            set { addressField = value; }
        }

        private XmlAttribute[] anyAttrField;

        /// <summary>
        /// Gets or sets any attr.
        /// </summary>
        /// <value>Any attr.</value>
        [XmlAnyAttribute] public XmlAttribute[] AnyAttr
        {
            get { return anyAttrField; }
            set { anyAttrField = value; }
        }

        private XmlElement[] anyElementField;

        /// <summary>
        /// Gets or sets the any-elements-array.
        /// </summary>
        /// <value>The any-elements-array</value>
        [XmlAnyElement]
        public XmlElement[] AnyElements
        {
            get { return anyElementField; }
            set { anyElementField = value; }
        }
    }
}