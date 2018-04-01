using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The structure of SignedInfo includes the canonicalization algorithm, a signature algorithm, and one or 
    /// more references. The SignedInfo element may contain an optional ID attribute that will allow it to be 
    /// referenced by other signatures and objects. 
    /// 
    /// SignedInfo does not include explicit signature or digest properties (such as calculation time, 
    /// cryptographic device serial number, etc.). If an application needs to associate properties with the 
    /// signature or digest, it may include such information in a SignatureProperties element within an Object 
    /// element
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SignedInfo {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SignedInfo";
        
        private CanonicalizationMethod canonicalizationMethodField;
        
        private SignatureMethod signatureMethodField;
        
        private Reference[] referenceField;
        
        private string idField;


        /// <summary>
        /// Gets or sets the canonicalization method.
        /// </summary>
        /// <value>The canonicalization method.</value>
        public CanonicalizationMethod CanonicalizationMethod {
            get {
                return canonicalizationMethodField;
            }
            set {
                canonicalizationMethodField = value;
            }
        }


        /// <summary>
        /// Gets or sets the signature method.
        /// </summary>
        /// <value>The signature method.</value>
        public SignatureMethod SignatureMethod {
            get {
                return signatureMethodField;
            }
            set {
                signatureMethodField = value;
            }
        }


        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        [XmlElementAttribute("Reference")]
        public Reference[] Reference {
            get {
                return referenceField;
            }
            set {
                referenceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttributeAttribute(DataType="ID")]
        public string Id {
            get {
                return idField;
            }
            set {
                idField = value;
            }
        }
    }
}