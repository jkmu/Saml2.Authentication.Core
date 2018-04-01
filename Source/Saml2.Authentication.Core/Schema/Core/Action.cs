using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;Action&gt; element specifies an action on the specified resource for which permission is sought. Its
    /// string-data content provides the label for an action sought to be performed on the specified resource,
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class Action
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Action";

        private string namespaceField;

        private string valueField;


        /// <summary>
        /// Gets or sets the namespace.
        /// A URI reference representing the namespace in which the name of the specified action is to be
        /// interpreted. If this element is absent, the namespace
        /// urn:oasis:names:tc:SAML:1.0:action:rwedc-negation specified in Section 8.1.2 is in
        /// effect.
        /// </summary>
        /// <value>The namespace.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Namespace
        {
            get { return namespaceField; }
            set { namespaceField = value; }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlTextAttribute]
        public string Value
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
}