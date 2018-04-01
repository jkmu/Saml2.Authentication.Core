using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;AudienceRestriction&gt; element specifies that the assertion is addressed to one or more
    /// specific audiences identified by &lt;Audience&gt; elements. Although a SAML relying party that is outside the
    /// audiences specified is capable of drawing conclusions from an assertion, the SAML asserting party
    /// explicitly makes no representation as to accuracy or trustworthiness to such a party. It contains the
    /// following element:
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class AudienceRestriction : ConditionAbstract
    {
        private List<string> audienceField;

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AudienceRestriction";

        /// <summary>
        /// Gets or sets the audience.
        /// A URI reference that identifies an intended audience. The URI reference MAY identify a document
        /// that describes the terms and conditions of audience membership. It MAY also contain the unique
        /// identifier URI from a SAML name identifier that describes a system entity
        /// </summary>
        /// <value>The audience.</value>
        [XmlElementAttribute("Audience", DataType="anyURI")]
        public List<string> Audience
        {
            get { return audienceField; }
            set { audienceField = value; }
        }
    }
}