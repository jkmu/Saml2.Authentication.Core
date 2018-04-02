using System;
using System.Collections.Generic;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Utils;
using Saml2.Authentication.Core.Validation;

namespace dk.nita.saml20.Validation
{
    internal class Saml2AssertionValidator : ISaml2AssertionValidator
    {
        private readonly List<string> _allowedAudienceUris;
        protected bool QuirksMode;

        public Saml2AssertionValidator(List<string> allowedAudienceUris, bool quirksMode)
        {
            _allowedAudienceUris = allowedAudienceUris;
            QuirksMode = quirksMode;
        }

        #region Properties

        private ISaml2NameIDValidator _nameIdValidator;

        private ISaml2NameIDValidator NameIdValidator => _nameIdValidator ?? (_nameIdValidator = new Saml2NameIdValidator());

        private ISaml2SubjectValidator _subjectValidator;

        private ISaml2SubjectValidator SubjectValidator => _subjectValidator ?? (_subjectValidator = new Saml2SubjectValidator());

        private ISaml2StatementValidator StatementValidator => _statementValidator ?? (_statementValidator = new Saml2StatementValidator());

        private ISaml2StatementValidator _statementValidator;
        #endregion

        #region Saml2AssertionValidator interface

        public virtual void ValidateAssertion(Assertion assertion)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            ValidateAssertionAttributes(assertion);
            ValidateSubject(assertion);
            ValidateConditions(assertion);
            ValidateStatements(assertion);
        }

        #region ISaml2AssertionValidator Members

        /// <summary>
        /// Null fields are considered to be valid
        /// </summary>
        private static bool ValidateNotBefore(DateTime? notBefore, DateTime now, TimeSpan allowedClockSkew)
        {
            return notBefore == null || TimeRestrictionValidation.NotBeforeValid(notBefore.Value, now, allowedClockSkew);
        }

        /// <summary>
        /// Handle allowed clock skew by increasing notOnOrAfter with allowedClockSkew
        /// </summary>
        private static bool ValidateNotOnOrAfter(DateTime? notOnOrAfter, DateTime now, TimeSpan allowedClockSkew)
        {
            return notOnOrAfter == null || TimeRestrictionValidation.NotOnOrAfterValid(notOnOrAfter.Value, now, allowedClockSkew);
        }

        public void ValidateTimeRestrictions(Assertion assertion, TimeSpan allowedClockSkew)
        {
            // Conditions are not required
            if (assertion.Conditions == null)
            {
                return;
            }

            var conditions = assertion.Conditions;
            var now = DateTime.UtcNow;
            // Negative allowed clock skew does not make sense - we are trying to relax the restriction interval, not restrict it any further
            if (allowedClockSkew < TimeSpan.Zero)
            {
                allowedClockSkew = allowedClockSkew.Negate();
            }
            
            // NotBefore must not be in the future
            if (!ValidateNotBefore(conditions.NotBefore, now, allowedClockSkew))
            {
                throw new Saml2FormatException("Conditions.NotBefore must not be in the future");
            }

            // NotOnOrAfter must not be in the past
            if (!ValidateNotOnOrAfter(conditions.NotOnOrAfter, now, allowedClockSkew))
            {
                throw new Saml2FormatException("Conditions.NotOnOrAfter must not be in the past");
            }

            foreach (AuthnStatement statement in assertion.GetAuthnStatements())
            {
                if (statement.SessionNotOnOrAfter != null
                    && statement.SessionNotOnOrAfter <= now)
                {
                    throw new Saml2FormatException("AuthnStatement attribute SessionNotOnOrAfter MUST be in the future");
                }

                // TODO: Consider validating that authnStatement.AuthnInstant is in the past
            }

            if (assertion.Subject != null)
            {
                foreach (var o in assertion.Subject.Items)
                {
                    var subjectConfirmation = o as SubjectConfirmation;
                    if (subjectConfirmation?.SubjectConfirmationData == null)
                    {
                        continue;
                    }

                    if (!ValidateNotBefore(subjectConfirmation.SubjectConfirmationData.NotBefore, now,
                        allowedClockSkew))
                    {
                        throw new Saml2FormatException("SubjectConfirmationData.NotBefore must not be in the future");
                    }

                    if (!ValidateNotOnOrAfter(subjectConfirmation.SubjectConfirmationData.NotOnOrAfter, now,
                        allowedClockSkew))
                    {
                        throw new Saml2FormatException("SubjectConfirmationData.NotOnOrAfter must not be in the past");
                    }
                }
                
            }
        }

        #endregion

        /// <summary>
        /// Validates that all the required attributes are present on the assertion.
        /// Furthermore it validates validity of the Issuer element.
        /// </summary>
        /// <param name="assertion"></param>
        private void ValidateAssertionAttributes(Assertion assertion)
        {
            //There must be a Version
            if (!Saml2Utils.ValidateRequiredString(assertion.Version))
            {
                throw new Saml2FormatException("Assertion element must have the Version attribute set.");
            }

            //Version must be 2.0
            if (assertion.Version != Saml2Constants.Version)
            {
                throw new Saml2FormatException("Wrong value of version attribute on Assertion element");
            }

            //Assertion must have an ID
            if (!Saml2Utils.ValidateRequiredString(assertion.ID))
            {
                throw new Saml2FormatException("Assertion element must have the ID attribute set.");
            }

            // Make sure that the ID elements is at least 128 bits in length (SAML2.0 std section 1.3.4)
            if (!Saml2Utils.ValidateIDString(assertion.ID))
            {
                throw new Saml2FormatException("Assertion element must have an ID attribute with at least 16 characters (the equivalent of 128 bits)");
            }

            //IssueInstant must be set.
            if (!assertion.IssueInstant.HasValue)
            {
                throw new Saml2FormatException("Assertion element must have the IssueInstant attribute set.");
            }

            //There must be an Issuer
            if (assertion.Issuer == null)
            {
                throw new Saml2FormatException("Assertion element must have an issuer element.");
            }

            //The Issuer element must be valid
            NameIdValidator.ValidateNameID(assertion.Issuer);
        }

        /// <summary>
        /// Validates the subject of an Asssertion
        /// </summary>
        /// <param name="assertion"></param>
        private void ValidateSubject(Assertion assertion)
        {
            if (assertion.Subject == null)
            {
                //If there is no statements there must be a subject
                // as specified in [SAML2.0std] section 2.3.3
                if (assertion.Items == null || assertion.Items.Length == 0)
                {
                    throw new Saml2FormatException("Assertion with no Statements must have a subject.");
                }

                foreach (var o in assertion.Items)
                {
                    //If any of the below types are present there must be a subject.
                    if (o is AuthnStatement || o is AuthzDecisionStatement || o is AttributeStatement)
                    {
                        throw new Saml2FormatException("AuthnStatement, AuthzDecisionStatement and AttributeStatement require a subject.");
                    }
                }
            }
            else
            {
                //If a subject is present, validate it
                SubjectValidator.ValidateSubject(assertion.Subject);
            }
        }

        /// <summary>
        /// Validates the Assertion's conditions 
        /// Audience restrictions processing rules are:
        ///  - Within a single audience restriction condition in the assertion, the service must be configured
        ///    with an audience-list that contains at least one of the restrictions in the assertion ("OR" filter)
        ///  - When multiple audience restrictions are present within the same assertion, all individual audience 
        ///    restriction conditions must be met ("AND" filter)
        /// </summary>
        private void ValidateConditions(Assertion assertion)
        {
            // Conditions are not required
            if (assertion.Conditions == null)
            {
                return;
            }

            var oneTimeUseSeen = false;
            var proxyRestrictionsSeen = false;
            
            ValidateConditionsInterval(assertion.Conditions);

            foreach (var cat in assertion.Conditions.Items)
            {
                if (cat is OneTimeUse)
                {
                    if (oneTimeUseSeen)
                    {
                        throw new Saml2FormatException("Assertion contained more than one condition of type OneTimeUse");
                    }
                    oneTimeUseSeen = true;
                    continue;
                }

                if (cat is ProxyRestriction proxyRestriction)
                {
                    if (proxyRestrictionsSeen)
                    {
                        throw new Saml2FormatException("Assertion contained more than one condition of type ProxyRestriction");
                    }
                    proxyRestrictionsSeen = true;

                    if (!string.IsNullOrEmpty(proxyRestriction.Count))
                    {
                        if (!uint.TryParse(proxyRestriction.Count, out _))
                            throw new Saml2FormatException("Count attribute of ProxyRestriction MUST BE a non-negative integer");
                    }

                    if (proxyRestriction.Audience != null)
                    {
                        foreach(var audience in proxyRestriction.Audience)
                        {
                            if (!Uri.IsWellFormedUriString(audience, UriKind.Absolute))
                                throw new Saml2FormatException("ProxyRestriction Audience MUST BE a wellformed uri");
                        }
                    }
                }

                // AudienceRestriction processing goes here (section 2.5.1.4 of [SAML2.0std])
                if (cat is AudienceRestriction audienceRestriction)
                {
                    // No audience restrictions? No problems...
                    if (audienceRestriction.Audience == null || audienceRestriction.Audience.Count == 0)
                        continue;

                    // If there are no allowed audience uris configured for the service, the assertion is not
                    // valid for this service
                    if (_allowedAudienceUris == null || _allowedAudienceUris.Count < 1)
                        throw new Saml2FormatException("The service is not configured to meet any audience restrictions");

                    string match = null;
                    foreach (var audience in audienceRestriction.Audience)
                    {
                        //In QuirksMode this validation is omitted
                        if (!QuirksMode)
                        {
                            // The given audience value MUST BE a valid URI
                            if (!Uri.IsWellFormedUriString(audience, UriKind.Absolute))
                                throw new Saml2FormatException("Audience element has value which is not a wellformed absolute uri");
                        }

                        match =
                            _allowedAudienceUris.Find(
                                allowedUri => allowedUri.Equals(audience));
                        if (match != null)
                            break;
                    }

                    if (match == null)
                        throw new Saml2FormatException("The service is not configured to meet the given audience restrictions");
                }
            }
        }
        
        /// <summary>
        /// If both conditions.NotBefore and conditions.NotOnOrAfter are specified, NotBefore 
        /// MUST BE less than NotOnOrAfter 
        /// </summary>
        /// <exception cref="Saml2FormatException">If <param name="conditions"/>.NotBefore is not less than <paramref name="conditions"/>.NotOnOrAfter</exception>        
        private static void ValidateConditionsInterval(Conditions conditions)
        {
            // No settings? No restrictions
            if (conditions.NotBefore == null && conditions.NotOnOrAfter == null)
                return;
            
            if (conditions.NotBefore != null && conditions.NotOnOrAfter != null && conditions.NotBefore.Value >= conditions.NotOnOrAfter.Value)
                throw new Saml2FormatException(String.Format("NotBefore {0} MUST BE less than NotOnOrAfter {1} on Conditions", Saml2Utils.ToUTCString(conditions.NotBefore.Value), Saml2Utils.ToUTCString(conditions.NotOnOrAfter.Value)));
        }

        /// <summary>
        /// Validates the details of the Statements present in the assertion ([SAML2.0std] section 2.7)
        /// NOTE: the rules relating to the enforcement of a Subject element are handled during Subject validation
        /// </summary>
        private void ValidateStatements(Assertion assertion)
        {
            // Statements are not required
            if (assertion.Items == null)
            {
                return;
            }

            foreach (var o in assertion.Items)
            {
                StatementValidator.ValidateStatement(o);
            }
        }
        #endregion
    }
}