using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// is an extension point that allows new SAML queries to be
    /// defined that specify a single SAML subject.
    /// </summary>
    [XmlInclude(typeof (AuthzDecisionQuery))]
    [XmlInclude(typeof (AttributeQuery))]
    [XmlInclude(typeof (AuthnQuery))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.PROTOCOL, IsNullable = false)]
    public abstract class SubjectQueryAbstract : RequestAbstract
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SubjectQuery";

        private Subject subjectField;
        
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        [XmlElement(Namespace=Saml2Constants.ASSERTION)]
        public Subject Subject
        {
            get { return subjectField; }
            set { subjectField = value; }
        }
    }
}