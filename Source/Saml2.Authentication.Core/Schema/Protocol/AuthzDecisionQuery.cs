using System;
using System.Xml.Serialization;
using my=dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Core;
namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;AuthzDecisionQuery&gt; element is used to make the query "Should these actions on this resource
    /// be allowed for this subject, given this evidence?" A successful response will be in the form of assertions
    /// containing authorization decision statements.
    /// Note: The &lt;AuthzDecisionQuery&gt; feature has been frozen as of SAML V2.0, with no
    /// future enhancements planned. Users who require additional functionality may want to
    /// consider the eXtensible Access Control Markup Language [XACML], which offers
    /// enhanced authorization decision features.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class AuthzDecisionQuery : SubjectQueryAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AuthzDecisionQuery";

        private my.Action[] actionField;

        private Evidence evidenceField;

        private string resourceField;


        /// <summary>
        /// Gets or sets the action.
        /// The actions for which authorization is requested.
        /// </summary>
        /// <value>The action.</value>
        [XmlElement("Action", Namespace=Saml2Constants.ASSERTION)]
        public my.Action[] Action
        {
            get { return actionField; }
            set { actionField = value; }
        }


        /// <summary>
        /// Gets or sets the evidence.
        /// A set of assertions that the SAML authority MAY rely on in making its authorization decision
        /// </summary>
        /// <value>The evidence.</value>
        [XmlElement(Namespace=Saml2Constants.ASSERTION)]
        public Evidence Evidence
        {
            get { return evidenceField; }
            set { evidenceField = value; }
        }


        /// <summary>
        /// Gets or sets the resource.
        /// A URI reference indicating the resource for which authorization is requested.
        /// </summary>
        /// <value>The resource.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Resource
        {
            get { return resourceField; }
            set { resourceField = value; }
        }
    }
}