namespace dk.nita.saml20.Validation
{
    using System;
    using System.Net.Mail;
    using Schema.Core;
    using Schema.Protocol;
    using Utils;

    internal class Saml2NameIdValidator : ISaml2NameIDValidator
    {
        readonly Saml2EncryptedElementValidator _encElemValidator = new Saml2EncryptedElementValidator();

        public void ValidateNameID(NameID nameId)
        {
            if (nameId == null)
            {
                throw new ArgumentNullException("nameId");
            }

            if (string.IsNullOrEmpty(nameId.Format))
            {
                return;
            }

            if (!Uri.IsWellFormedUriString(nameId.Format, UriKind.Absolute))
            {
                throw new Saml2FormatException("NameID element has Format attribute which is not a wellformed absolute uri.");
            }

            // The processing rules from [SAML2.0std] section 8.3 are implemented here
            if (nameId.Format == Saml2Constants.NameIdentifierFormats.Email)
            {
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Email Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                try
                {
                    var mailAddress = new MailAddress(nameId.Value);
                }
                catch (FormatException fe)
                {
                    throw new Saml2FormatException("Value of NameID is not a valid email address according to the IETF RFC 2822 specification", fe);
                }
                catch (IndexOutOfRangeException ie)
                {
                    throw new Saml2FormatException("Value of NameID is not a valid email address according to the IETF RFC 2822 specification", ie);
                }
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.X509SubjectName)
            {
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with X509SubjectName Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                // TODO: Consider checking for correct encoding of the Value according to the
                // XML Signature Recommendation (http://www.w3.org/TR/xmldsig-core/) section 4.4.4
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.Windows)
            {
                // Required format is 'DomainName\UserName' but the domain name and the '\' are optional
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Windows Format attribute MUST contain a Value that contains more than whitespace characters");
                }
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.Kerberos)
            {
                // Required format is 'name[/instance]@REALM'
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Kerberos Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                if (nameId.Value.Length < 3)
                {
                    throw new Saml2FormatException("NameID with Kerberos Format attribute MUST contain a Value with at least 3 characters");
                }

                if (nameId.Value.IndexOf("@") < 0)
                {
                    throw new Saml2FormatException("NameID with Kerberos Format attribute MUST contain a Value that contains a '@'");
                }

                // TODO: Consider implementing the rules for 'name', 'instance' and 'REALM' found in IETF RFC 1510 (http://www.ietf.org/rfc/rfc1510.txt) here 
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.Entity)
            {
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Entity Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                if (nameId.Value.Length > 1024)
                {
                    throw new Saml2FormatException("NameID with Entity Format attribute MUST have a Value that contains no more than 1024 characters");
                }

                if (nameId.NameQualifier != null)
                {
                    throw new Saml2FormatException("NameID with Entity Format attribute MUST NOT set the NameQualifier attribute");
                }

                if (nameId.SPNameQualifier != null)
                {
                    throw new Saml2FormatException("NameID with Entity Format attribute MUST NOT set the SPNameQualifier attribute");
                }

                if (nameId.SPProvidedID != null)
                {
                    throw new Saml2FormatException("NameID with Entity Format attribute MUST NOT set the SPProvidedID attribute");
                }
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.Persistent)
            {
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Persistent Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                if (nameId.Value.Length > 256)
                {
                    throw new Saml2FormatException("NameID with Persistent Format attribute MUST have a Value that contains no more than 256 characters");
                }
            }
            else if (nameId.Format == Saml2Constants.NameIdentifierFormats.Transient)
            {
                if (!Saml2Utils.ValidateRequiredString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Transient Format attribute MUST contain a Value that contains more than whitespace characters");
                }

                if (nameId.Value.Length > 256)
                {
                    throw new Saml2FormatException("NameID with Transient Format attribute MUST have a Value that contains no more than 256 characters");
                }

                if (!Saml2Utils.ValidateIdString(nameId.Value))
                {
                    throw new Saml2FormatException("NameID with Transient Format attribute MUST have a Value with at least 16 characters (the equivalent of 128 bits)");
                }
            }
        }

        public void ValidateEncryptedID(EncryptedElement encryptedId)
        {
            _encElemValidator.ValidateEncryptedElement(encryptedId, "EncryptedID");
        }
    }
}