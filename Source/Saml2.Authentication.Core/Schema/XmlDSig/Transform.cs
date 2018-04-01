using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The optional Transforms element contains an ordered list of Transform elements; these describe how the 
    /// signer obtained the data object that was digested. The output of each Transform serves as input to the 
    /// next Transform. The input to the first Transform is the result of dereferencing the URI attribute of the 
    /// Reference element. The output from the last Transform is the input for the DigestMethod algorithm. When 
    /// transforms are applied the signer is not signing the native (original) document but the resulting 
    /// (transformed) document. (See Only What is Signed is Secure (section 8.1).) 
    /// 
    /// Each Transform consists of an Algorithm attribute and content parameters, if any, appropriate for the 
    /// given algorithm. The Algorithm attribute value specifies the name of the algorithm to be performed, and 
    /// the Transform content provides additional data to govern the algorithm's processing of the transform 
    /// input. (See Algorithm Identifiers and Implementation Requirements (section 6).) 
    /// 
    /// As described in The Reference Processing Model (section  4.3.3.2), some transforms take an XPath node-set 
    /// as input, while others require an octet stream. If the actual input matches the input needs of the 
    /// transform, then the transform operates on the unaltered input. If the transform input requirement differs 
    /// from the format of the actual input, then the input must be converted. 
    /// 
    /// Some Transforms may require explicit MIME type, charset (IANA registered "character set"), or other such 
    /// information concerning the data they are receiving from an earlier Transform or the source data, although 
    /// no Transform algorithm specified in this document needs such explicit information. Such data 
    /// characteristics are provided as parameters to the Transform algorithm and should be described in the 
    /// specification for the algorithm. 
    /// 
    /// Examples of transforms include but are not limited to base64 decoding [MIME], canonicalization [XML-C14N], 
    /// XPath filtering [XPath], and XSLT [XSLT]. The generic definition of the Transform element also allows 
    /// application-specific transform algorithms. For example, the transform could be a decompression routine 
    /// given by a Java class appearing as a base64 encoded parameter to a Java Transform algorithm. However, 
    /// applications should refrain from using application-specific transforms if they wish their signatures to 
    /// be verifiable outside of their application domain. Transform Algorithms (section 6.6) defines the list 
    /// of standard transformations. 
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class Transform
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Transform";

        private string algorithmField;
        private object[] itemsField;

        private string[] textField;


        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlAnyElement]
        [XmlElement("XPath", typeof (string))]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }


        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [XmlText]
        public string[] Text
        {
            get { return textField; }
            set { textField = value; }
        }


        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Algorithm
        {
            get { return algorithmField; }
            set { algorithmField = value; }
        }
    }
}