using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;AuthzDecisionStatement&gt; element describes a statement by the SAML authority asserting that
    /// a request for access by the assertion subject to the specified resource has resulted in the specified
    /// authorization decision on the basis of some optionally specified evidence. Assertions containing
    /// &lt;AuthzDecisionStatement&gt; elements MUST contain a &lt;Subject&gt; element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class AuthzDecisionStatement : StatementAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AuthzDecisionStatement";

        private Action[] actionField;
        private DecisionType decisionField;

        private Evidence evidenceField;

        private string resourceField;


        /// <summary>
        /// Gets or sets the action.
        /// The set of actions authorized to be performed on the specified resource.
        /// </summary>
        /// <value>The action.</value>
        [XmlElementAttribute("Action")]
        public Action[] Action
        {
            get { return actionField; }
            set { actionField = value; }
        }


        /// <summary>
        /// Gets or sets the evidence.
        /// A set of assertions that the SAML authority relied on in making the decision.
        /// </summary>
        /// <value>The evidence.</value>
        public Evidence Evidence
        {
            get { return evidenceField; }
            set { evidenceField = value; }
        }


        /// <summary>
        /// Gets or sets the resource.
        /// A URI reference identifying the resource to which access authorization is sought. This attribute MAY
        /// have the value of the empty URI reference (""), and the meaning is defined to be "the start of the
        /// current document", as specified by IETF RFC 2396 [RFC 2396] Section 4.2.
        /// </summary>
        /// <value>The resource.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Resource
        {
            get { return resourceField; }
            set { resourceField = value; }
        }


        /// <summary>
        /// Gets or sets the decision.
        /// The decision rendered by the SAML authority with respect to the specified resource. The value is of
        /// the DecisionType simple type.
        /// </summary>
        /// <value>The decision.</value>
        [XmlAttributeAttribute]
        public DecisionType Decision
        {
            get { return decisionField; }
            set { decisionField = value; }
        }
    }
}