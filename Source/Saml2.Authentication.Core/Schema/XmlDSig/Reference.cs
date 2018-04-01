using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// Reference is an element that may occur one or more times. It specifies a digest algorithm and digest 
    /// value, and optionally an identifier of the object being signed, the type of the object, and/or a list 
    /// of transforms to be applied prior to digesting. The identification (URI) and transforms describe how the 
    /// digested content (i.e., the input to the digest method) was created. The Type attribute facilitates the 
    /// processing of referenced data. For example, while this specification makes no requirements over external 
    /// data, an application may wish to signal that the referent is a Manifest. An optional ID attribute permits 
    /// a Reference to be referenced from elsewhere. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class Reference
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Reference";

        private DigestMethod digestMethodField;

        private byte[] digestValueField;

        private string idField;
        private Transform[] transformsField;

        private string typeField;
        private string uRIField;


        /// <summary>
        /// Gets or sets the transforms.
        /// </summary>
        /// <value>The transforms.</value>
        [XmlArrayItem("Transform", IsNullable=false)]
        public Transform[] Transforms
        {
            get { return transformsField; }
            set { transformsField = value; }
        }


        /// <summary>
        /// Gets or sets the digest method.
        /// </summary>
        /// <value>The digest method.</value>
        public DigestMethod DigestMethod
        {
            get { return digestMethodField; }
            set { digestMethodField = value; }
        }


        /// <summary>
        /// Gets or sets the digest value.
        /// </summary>
        /// <value>The digest value.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] DigestValue
        {
            get { return digestValueField; }
            set { digestValueField = value; }
        }


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute(DataType="ID")]
        public string Id
        {
            get { return idField; }
            set { idField = value; }
        }


        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [XmlAttribute(DataType="anyURI")]
        public string URI
        {
            get { return uRIField; }
            set { uRIField = value; }
        }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Type
        {
            get { return typeField; }
            set { typeField = value; }
        }
    }
}