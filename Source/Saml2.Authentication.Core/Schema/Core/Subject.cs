using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The optional &lt;Subject&gt; element specifies the principal that is the subject of all of the (zero or more)
    /// statements in the assertion.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class Subject
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Subject";

        private object[] itemsField;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("BaseID", typeof (BaseIDAbstract))]
        [XmlElementAttribute("EncryptedID", typeof (EncryptedElement))]
        [XmlElementAttribute("NameID", typeof (NameID))]
        [XmlElementAttribute("SubjectConfirmation", typeof (SubjectConfirmation))]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }
}