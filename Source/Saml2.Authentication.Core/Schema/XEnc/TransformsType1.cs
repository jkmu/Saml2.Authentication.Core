using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// The Transforms type
    /// </summary>
    [Serializable]
    [XmlType(TypeName="TransformsType", Namespace=Saml2Constants.XENC)]
    public class TransformsType1
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "TransformsType";

        private Transform[] transformField;


        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        /// <value>The transform.</value>
        [XmlElement("Transform", Namespace=Saml2Constants.XMLDSIG)]
        public Transform[] Transform
        {
            get { return transformField; }
            set { transformField = value; }
        }
    }
}