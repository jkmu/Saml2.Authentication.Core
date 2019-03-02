using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The Saml20 Conditions class
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class Conditions
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Conditions";

        /// <summary>
        /// Gets or sets the items.
        /// Items may be of types AudienceRestriction, Condition, OneTimeUse and ProxyRestriction
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("AudienceRestriction", typeof(AudienceRestriction))]
        [XmlElementAttribute("Condition", typeof(ConditionAbstract))]
        [XmlElementAttribute("OneTimeUse", typeof(OneTimeUse))]
        [XmlElementAttribute("ProxyRestriction", typeof(ProxyRestriction))]
        public List<ConditionAbstract> Items { get; set; }

        /// <summary>
        /// Gets or sets the not before.
        /// Specifies the earliest time instant at which the assertion is valid. The time value is encoded in UTC
        /// </summary>
        /// <value>The not before.</value>
        [XmlIgnore]
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the not before string.
        /// </summary>
        /// <value>The not before string.</value>
        [XmlAttribute("NotBefore")]
        public string NotBeforeString
        {
            get => NotBefore.HasValue ? Saml2Utils.ToUtcString(NotBefore.Value) : null;

            set => NotBefore = string.IsNullOrEmpty(value) ? (DateTime?)null : Saml2Utils.FromUtcString(value);
        }

        /// <summary>
        /// Gets or sets the not on or after.
        /// Specifies the time instant at which the assertion has expired. The time value is encoded in UTC.
        /// </summary>
        /// <value>The not on or after.</value>
        [XmlIgnore]
        public DateTime? NotOnOrAfter { get; set; }

        /// <summary>
        /// Gets or sets the not on or after string.
        /// </summary>
        /// <value>The not on or after string.</value>
        [XmlAttribute("NotOnOrAfter")]
        public string NotOnOrAfterString
        {
            get => NotOnOrAfter.HasValue ? Saml2Utils.ToUtcString(NotOnOrAfter.Value) : null;

            set => NotOnOrAfter = string.IsNullOrEmpty(value) ? (DateTime?)null : Saml2Utils.FromUtcString(value);
        }
    }
}