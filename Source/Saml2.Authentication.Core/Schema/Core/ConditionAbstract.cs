using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The Saml ConditionAbstract class.
    /// Serves as an extension point for new conditions.
    /// </summary>
    [XmlInclude(typeof (ProxyRestriction))]
    [XmlInclude(typeof (OneTimeUse))]
    [XmlInclude(typeof (AudienceRestriction))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public abstract class ConditionAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Condition";
    }
}