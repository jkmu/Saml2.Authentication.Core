using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// Item Choices
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION, IncludeInSchema=false)]
    public enum ItemsChoiceType6
    {
        /// <summary>
        /// Item of type Assertion
        /// </summary>
        Assertion,
        /// <summary>
        /// Item of type AssertionIDRef
        /// </summary>
        AssertionIDRef,
        /// <summary>
        /// Item of type AssertionURIRef
        /// </summary>
        AssertionURIRef,
        /// <summary>
        /// Item of type EncryptedAssertion
        /// </summary>
        EncryptedAssertion,
    }

    /// <summary>
    /// Decision types
    /// </summary>
    [Serializable]
    [XmlTypeAttribute(Namespace=Saml2Constants.ASSERTION)]
    public enum DecisionType
    {
        /// <summary>
        /// Permit decision type
        /// </summary>
        Permit,
        /// <summary>
        /// Deny decision type
        /// </summary>
        Deny,
        /// <summary>
        /// Indeterminate decision type
        /// </summary>
        Indeterminate,
    }

    /// <summary>
    /// Item Choices
    /// </summary>
    [Serializable]
    [XmlTypeAttribute(Namespace=Saml2Constants.ASSERTION, IncludeInSchema=false)]
    public enum ItemsChoiceType4
    {
        /// <summary>
        /// Item of type any 
        /// </summary>
        [XmlEnumAttribute("##any:")] Item,
        /// <summary>
        /// Item of type Assertion
        /// </summary>
        Assertion,
        /// <summary>
        /// Item of type AssertionIDRef
        /// </summary>
        AssertionIDRef,
        /// <summary>
        /// Item of type AssertionURIRef
        /// </summary>
        AssertionURIRef,
        /// <summary>
        /// Item of type EncryptedAssertion
        /// </summary>
        EncryptedAssertion,
    }

    /// <summary>
    /// Item Choices
    /// </summary>
    [Serializable]
    [XmlTypeAttribute(Namespace=Saml2Constants.ASSERTION, IncludeInSchema=false)]
    public enum ItemsChoiceType5
    {
        /// <summary>
        /// Item of type AuthnContextClassRef
        /// </summary>
        AuthnContextClassRef,
        /// <summary>
        /// Item of type AuthnContextDecl
        /// </summary>
        AuthnContextDecl,
        /// <summary>
        /// Item of type AuthnContextDeclRef
        /// </summary>
        AuthnContextDeclRef,
    }
}