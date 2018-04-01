using System;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;
using my = dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Validation
{
    internal class Saml2StatementValidator : ISaml2StatementValidator
    {
        private ISaml2AttributeValidator _attributeValidator;

        public ISaml2AttributeValidator AttributeValidator
        {
            get
            {
                if (_attributeValidator == null)
                    _attributeValidator = new Saml2AttributeValidator();
                return _attributeValidator;
            }
        }

        public virtual void ValidateStatement(StatementAbstract statement)
        {
            if (statement == null) throw new ArgumentNullException("statement");

            // Validate all possible statements in the assertion
            if (statement is AuthnStatement)
                ValidateAuthnStatement((AuthnStatement)statement);
            else if (statement is AuthzDecisionStatement)
                ValidateAuthzDecisionStatement((AuthzDecisionStatement)statement);
            else if (statement is AttributeStatement)
                ValidateAttributeStatement((AttributeStatement)statement);
            else
                throw new Saml2FormatException(String.Format("Unsupported Statement type {0}", statement.GetType()));

        }

        /// <summary>
        /// [SAML2.0std] section 2.7.2
        /// </summary>
        /// <param name="statement"></param>
        private void ValidateAuthnStatement(AuthnStatement statement)
        {
            if (statement.AuthnInstant == null)
                throw new Saml2FormatException("AuthnStatement MUST have an AuthnInstant attribute");

            if (!Saml2Utils.ValidateOptionalString(statement.SessionIndex))
                throw new Saml2FormatException("SessionIndex attribute of AuthnStatement must contain at least one non-whitespace character");

            if (statement.SubjectLocality != null)
            {
                if (!Saml2Utils.ValidateOptionalString(statement.SubjectLocality.Address))
                    throw new Saml2FormatException("Address attribute of SubjectLocality must contain at least one non-whitespace character");

                if (!Saml2Utils.ValidateOptionalString(statement.SubjectLocality.DNSName))
                    throw new Saml2FormatException("DNSName attribute of SubjectLocality must contain at least one non-whitespace character");
            }

            ValidateAuthnContext(statement.AuthnContext);
        }

        /// <summary>
        /// [SAML2.0std] section 2.7.2.2
        /// </summary>
        /// <param name="authnContext"></param>
        private void ValidateAuthnContext(AuthnContext authnContext)
        {
            if (authnContext == null)
                throw new Saml2FormatException("AuthnStatement MUST have an AuthnContext element");

            // There must be at least one item if an authentication statement is present
            if (authnContext.Items == null || authnContext.Items.Length == 0)
                throw new Saml2FormatException("AuthnContext element MUST contain at least one AuthnContextClassRef, AuthnContextDecl or AuthnContextDeclRef element");

            // Cannot happen when using .NET auto-generated serializer classes, but other implementations may fail to enforce the 
            // correspondence on the size of the arrays involved
            if (authnContext.Items.Length != authnContext.ItemsElementName.Length)
                throw new Saml2FormatException("AuthnContext parse error: Mismathing Items vs ItemElementNames counts");

            // Validate the anyUri xsi schema type demands on context reference types
            // We do not support by-value authentication types (since Geneva does not allow it)

            if (authnContext.Items.Length > 2)
                throw new Saml2FormatException("AuthnContext MUST NOT contain more than two elements.");

            bool authnContextDeclRefFound = false;
            for (int i = 0; i < authnContext.ItemsElementName.Length; ++i)
            {
                switch (authnContext.ItemsElementName[i])
                {
                    case ItemsChoiceType5.AuthnContextClassRef:
                        if (i > 0)
                            throw new Saml2FormatException("AuthnContextClassRef must be in the first element");
                        if (!Uri.IsWellFormedUriString((string)authnContext.Items[i], UriKind.Absolute))
                            throw new Saml2FormatException("AuthnContextClassRef has a value which is not a wellformed absolute uri");
                        break;
                    case ItemsChoiceType5.AuthnContextDeclRef:
                        if (authnContextDeclRefFound)
                            throw new Saml2FormatException("AuthnContextDeclRef MUST only be present once.");
                        authnContextDeclRefFound = true;
                        if (!Uri.IsWellFormedUriString((string)authnContext.Items[i], UriKind.Absolute))
                            throw new Saml2FormatException("AuthnContextDeclRef has a value which is not a wellformed absolute uri");
                        break;
                    case ItemsChoiceType5.AuthnContextDecl:
                        throw new Saml2FormatException("AuthnContextDecl elements are not allowed in this implementation");
                    default:
                        throw new Saml2FormatException(string.Format("Subelement {0} of AuthnContext is not supported", authnContext.ItemsElementName[i]));
                }
            }

            // No authenticating authorities? We are done
            if (authnContext.AuthenticatingAuthority == null || authnContext.AuthenticatingAuthority.Length == 0)
                return;

            // Values MUST have xsi schema type anyUri:
            foreach (string authnAuthority in authnContext.AuthenticatingAuthority)
            {
                if (!Uri.IsWellFormedUriString(authnAuthority, UriKind.Absolute))
                    throw new Saml2FormatException("AuthenticatingAuthority array contains a value which is not a wellformed absolute uri");
            }
        }

        /// <summary>
        /// [SAML2.0std] section 2.7.3
        /// </summary>
        /// <param name="statement"></param>
        private void ValidateAttributeStatement(AttributeStatement statement)
        {
            if (statement.Items == null || statement.Items.Length == 0)
                throw new Saml2FormatException("AttributeStatement MUST contain at least one Attribute or EncryptedAttribute");

            foreach (object o in statement.Items)
            {
                if (o == null)
                    throw new Saml2FormatException("null-Attributes are not supported");

                if (o is SamlAttribute)
                    AttributeValidator.ValidateAttribute((SamlAttribute) o);
                else if (o is EncryptedElement)
                    AttributeValidator.ValidateEncryptedAttribute((EncryptedElement)o);
                else
                    throw new Saml2FormatException(string.Format("Subelement {0} of AttributeStatement is not supported", o.GetType()));
            }
        }

        /// <summary>
        /// [SAML2.0std] section 2.7.4
        /// </summary>
        private void ValidateAuthzDecisionStatement(AuthzDecisionStatement statement)
        {
            // This has type anyURI, and can be empty (special case in the standard), but not null.
            if (statement.Resource == null)
                throw new Saml2FormatException("Resource attribute of AuthzDecisionStatement is REQUIRED");

            // If it is not empty, it MUST BE a valid URI
            if (statement.Resource.Length > 0 && !Uri.IsWellFormedUriString(statement.Resource, UriKind.Absolute))
                throw new Saml2FormatException("Resource attribute of AuthzDecisionStatement has a value which is not a wellformed absolute uri");

            // NOTE: Decision property validation is done implicitly be the deserializer since it is represented by an enumeration

            if (statement.Action == null || statement.Action.Length == 0)
                throw new Saml2FormatException("At least one Action subelement must be present for an AuthzDecisionStatement element");

            foreach (my.Action action in statement.Action)
            {
                // NOTE: [SAML2.0std] claims that the Namespace is [Optional], but according to the schema definition (and Geneva)
                // NOTE: it has use="required"
                if (!Saml2Utils.ValidateRequiredString(action.Namespace))
                    throw new Saml2FormatException("Namespace attribute of Action element must contain at least one non-whitespace character");

                if (!Uri.IsWellFormedUriString(action.Namespace, UriKind.Absolute))
                    throw new Saml2FormatException("Namespace attribute of Action element has a value which is not a wellformed absolute uri");
            }

        }
    }
}