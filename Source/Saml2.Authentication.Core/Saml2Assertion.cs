using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;
using dk.nita.saml20.Validation;
using Saml2.Authentication.Core.Validation;
using Signature = dk.nita.saml20.Schema.XmlDSig.Signature;

namespace Saml2.Authentication.Core
{
    /// <summary>
    ///     Encapsulates the functionality required of a DK-SAML 2.0 Assertion.
    /// </summary>
    public class Saml2Assertion
    {
        /// <summary>
        ///     Check the signature of the XmlDocument using the list of keys.
        ///     If the signature key is found, the SigningKey property is set.
        /// </summary>
        /// <param name="keys">
        ///     A list of KeyDescriptor elements. Probably extracted from the metadata describing the IDP that sent
        ///     the message.
        /// </param>
        /// <returns>True, if one of the given keys was able to verify the signature. False in all other cases.</returns>
        public bool CheckSignature(IEnumerable<AsymmetricAlgorithm> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
            {
                if (key == null)
                    continue;

                if (CheckSignature(key))
                    return true;
            }

            return false;
        }

        private bool CheckSignature(AsymmetricAlgorithm key)
        {
            if (XmlSignatureUtils.CheckSignature(_samlAssertion, key))
            {
                SigningKey = key;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Verifies the assertion's signature and its time to live.
        /// </summary>
        /// <exception cref="Saml2Exception">
        ///     if the assertion's signature can not be verified or its time to live has been
        ///     exceeded.
        /// </exception>
        public void CheckValid(IEnumerable<AsymmetricAlgorithm> trustedSigners)
        {
            if (!CheckSignature(trustedSigners))
                throw new Saml2Exception("Signature could not be verified.");

            if (IsExpired())
                throw new Saml2Exception("Assertion is no longer valid.");
        }

        /// <summary>
        ///     Checks if the expiration time has been exceeded.
        /// </summary>
        public bool IsExpired()
        {
            return DateTime.Now.ToUniversalTime() > NotOnOrAfter;
        }

        /// <summary>
        ///     Returns the KeyInfo element of the signature of the token.
        /// </summary>
        /// <returns>Null if the token is not signed. The KeyInfo element otherwise.</returns>
        public KeyInfo GetSignatureKeys()
        {
            if (!XmlSignatureUtils.IsSigned(_samlAssertion))
                return null;

            return XmlSignatureUtils.ExtractSignatureKeys(_samlAssertion);
        }

        /// <summary>
        ///     Returns the SubjectConfirmationData from the assertion subject items
        /// </summary>
        /// <returns>SubjectConfirmationData object from subject items, null if none present</returns>
        public SubjectConfirmationData GetSubjectConfirmationData()
        {
            foreach (var item in SubjectItems)
                if (item is SubjectConfirmation confirmation)
                    return confirmation.SubjectConfirmationData;
            return null;
        }

        /// <summary>
        ///     Gets the assertion as an XmlDocument.
        /// </summary>
        /// <returns></returns>
        public XmlElement GetXml()
        {
            return _samlAssertion;
        }

        /// <summary>
        ///     Signs the assertion with the given certificate.
        /// </summary>
        /// <param name="cert">The certificate to sign the assertion with.</param>
        public void Sign(X509Certificate2 cert)
        {
            CheckCertificateCanSign(cert);

            // Clear the strongly typed version of the assertion in preparation for a new source.
            _assertion = null;

            // Merge the modified attributes to the assertion.
            InsertAttributes();

            // Remove existing signatures when resigning the assertion
            var signatureParentNode = _samlAssertion; //FIX.DocumentElement;
            XmlNode sigNode;
            while ((sigNode = signatureParentNode.GetElementsByTagName(Signature.ELEMENT_NAME,
                       Saml2Constants.XMLDSIG)[0]) != null)
                signatureParentNode.RemoveChild(sigNode);

            var assertionDocument = new XmlDocument {XmlResolver = null};
            assertionDocument.Load(new StringReader(Serialization.SerializeToXmlString(_samlAssertion)));

            AddSignature(assertionDocument, cert);

            LoadXml(assertionDocument.DocumentElement, new List<AsymmetricAlgorithm>(new[] {cert.PublicKey.Key}));
        }

        private static void CheckCertificateCanSign(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
                throw new Saml2Exception("The private key must be part of the certificate.");
        }

        private static void AddSignature(XmlDocument assertionDocument, X509Certificate2 cert)
        {
            var signedXml = new SignedXml(assertionDocument);
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SigningKey = cert.PrivateKey;

            // Retrieve the value of the "ID" attribute on the root assertion element.
            var list = assertionDocument.GetElementsByTagName(Assertion.ELEMENT_NAME, Saml2Constants.ASSERTION);
            var el = (XmlElement) list[0];
            var reference = new Reference("#" + el.GetAttribute("ID"));

            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());

            signedXml.AddReference(reference);

            // Include the public key of the certificate in the assertion.
            //signedXml.KeyInfo = new KeyInfo();
            //signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert, X509IncludeOption.WholeChain));

            signedXml.ComputeSignature();
            // Append the computed signature. The signature must be placed as the sibling of the Issuer element.
            var nodes = assertionDocument.DocumentElement.GetElementsByTagName("Issuer", Saml2Constants.ASSERTION);
            if (nodes.Count != 1)
                throw new Saml2Exception("Assertion MUST contain one <Issuer> element.");
            assertionDocument.DocumentElement.InsertAfter(assertionDocument.ImportNode(signedXml.GetXml(), true),
                nodes[0]);
        }


        /// <summary>
        ///     Extracts the list of attributes from the &lt;AttributeStatement&gt; of the assertion, and
        ///     stores it in <code>_assertionAttributes</code>.
        /// </summary>
        private void ExtractAttributes()
        {
            _assertionAttributes = new List<SamlAttribute>(0);
            _encryptedAssertionAttributes = new List<EncryptedElement>(0);

            var list = _samlAssertion.GetElementsByTagName(AttributeStatement.ELEMENT_NAME, Saml2Constants.ASSERTION);

            if (list.Count == 0)
                return;

            // NOTE It would be nice to implement a better-performing solution where only the AttributeStatement is converted.
            // NOTE Namespace issues in the xml-schema "type"-attribute prevents this, though.
            var assertion = Serialization.Deserialize<Assertion>(new XmlNodeReader(_samlAssertion));

            var attributeStatements = assertion.GetAttributeStatements();
            if (attributeStatements.Count == 0 || attributeStatements[0].Items == null)
                return;

            var attributeStatement = attributeStatements[0];
            foreach (var item in attributeStatement.Items)
            {
                if (item is SamlAttribute attribute)
                    _assertionAttributes.Add(attribute);

                if (item is EncryptedElement element)
                    _encryptedAssertionAttributes.Add(element);
            }
        }

        /// <summary>
        ///     Merges the modified attributes into <code>AttributeStatement</code> of the assertion.
        /// </summary>
        private void InsertAttributes()
        {
            if (_assertionAttributes == null)
                return;

            // Generate the new AttributeStatement
            var attributeStatement = new AttributeStatement();
            var statements = new List<object>(_encryptedAssertionAttributes.Count + _assertionAttributes.Count);
            statements.AddRange(_assertionAttributes.ToArray());
            statements.AddRange(_encryptedAssertionAttributes.ToArray());
            attributeStatement.Items = statements.ToArray();

            var list = _samlAssertion.GetElementsByTagName(AttributeStatement.ELEMENT_NAME, Saml2Constants.ASSERTION);

            if (list.Count > 0) // Remove the old AttributeStatement.
                _samlAssertion.RemoveChild(list[0]); //FIX _samlAssertion.DocumentElement.RemoveChild(list[0]);

            // Only insert a new AttributeStatement if there are attributes.
            if (statements.Count > 0)
            {
                // Convert the new AttributeStatement to the Document Object Model and make a silent prayer that one day we will
                // be able to make this transition in a more elegant way.
                var attributeStatementDoc = Serialization.Serialize(attributeStatement);
                var attr = _samlAssertion.OwnerDocument.ImportNode(attributeStatementDoc.DocumentElement, true);
                // Insert the new statement.                            
                _samlAssertion.AppendChild(attr);
            }

            _encryptedAssertionAttributes = null;
            _assertionAttributes = null;
        }

        /// <summary>
        ///     Loads an assertion from XML.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="trustedSigners">The trusted signers.</param>
        private void LoadXml(XmlElement element, IEnumerable<AsymmetricAlgorithm> trustedSigners)
        {
            _samlAssertion = element;
            if (trustedSigners != null && XmlSignatureUtils.IsSigned(element))
                if (!CheckSignature(trustedSigners))
                    throw new Saml2Exception("Assertion signature could not be verified.");

            // Validate the saml20Assertion.      
            if (_autoValidate)
                AssertionValidator.ValidateAssertion(Assertion);
        }

        /// <summary>
        ///     Writes the token to a writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteAssertion(XmlWriter writer)
        {
            _samlAssertion.WriteTo(writer);
        }

        #region Private variables

        /// <summary>
        ///     The primary storage of the assertion. When deserialized or signed, the token will be stored in this field.
        /// </summary>
        private XmlElement _samlAssertion;

        /// <summary>
        ///     A strongly-typed version of the assertion. It is generated on-demand from the contents of the
        ///     <code>_samlAssertion</code>
        ///     field.
        /// </summary>
        private Assertion _assertion;

        private ISaml2AssertionValidator _assertionValidator;

        private AssertionProfile _profile;
        private readonly List<string> _audience;

        /// <summary>
        ///     An list of the unencrypted attributes in the assertion. This list is lazy initialized, ie. it will only be
        ///     retrieved
        ///     from the <code>_samlAssertion</code> field when it is requested through the <code>Attributes</code> property.
        ///     When the <code>Sign</code> method is called, the attributes in the list are embedded into the
        ///     <code>_samlAssertion</code>
        ///     and this variable is nulled.
        /// </summary>
        private List<SamlAttribute> _assertionAttributes;

        private List<EncryptedElement> _encryptedAssertionAttributes;

        private readonly bool _quirksMode;
        private readonly bool _autoValidate = true;

        #endregion

        #region Properties

        private ISaml2AssertionValidator AssertionValidator =>
            _assertionValidator ?? (_assertionValidator = new Saml2AssertionValidator(_audience, _quirksMode));

        /// <summary>
        ///     A strongly-typed version of the Saml Assertion. It is lazily generated based on the contents of the
        ///     <code>_samlAssertion</code> field.
        /// </summary>
        public Assertion Assertion
        {
            get
            {
                if (_assertion != null)
                    return _assertion;

                if (_samlAssertion == null)
                    throw new InvalidOperationException("No assertion is loaded.");

                var reader = new XmlNodeReader(_samlAssertion);
                _assertion = Serialization.Deserialize<Assertion>(reader);

                return _assertion;
            }
        }

        /// <summary>
        ///     Gets the assertion in XmlElement representation.
        /// </summary>
        /// <value>The XML assertion.</value>
        public XmlElement XmlAssertion => _samlAssertion;

        /// <summary>
        ///     Gets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public NameID Subject
        {
            get
            {
                foreach (var o in Assertion.Subject.Items)
                    if (o is NameID id)
                        return id;
                return null;
            }
        }

        /// <summary>
        ///     Gets the subject items.
        /// </summary>
        /// <value>The subject items.</value>
        public object[] SubjectItems => Assertion.Subject.Items;

        /// <summary>
        ///     They asymmetric key that can verify the signature of the assertion.
        /// </summary>
        public AsymmetricAlgorithm SigningKey { get; set; }

        /// <summary>
        ///     Retrieve the value of the &lt;Issuer&gt; element.
        /// </summary>
        public string Issuer => Assertion.Issuer.Value;

        /// <summary>
        ///     The ID attribute of the &lt;Assertion&gt; element.
        /// </summary>
        public string Id => Assertion.ID;

        /// <summary>
        ///     Gets the SessionIndex of the AuthnStatement
        /// </summary>
        public string SessionIndex
        {
            get
            {
                var list = Assertion.GetAuthnStatements();
                return list.Count > 0 ? list[0].SessionIndex : string.Empty;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the assertion has a OneTimeUse condition.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the assertion has a OneTimeUse condition; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneTimeUse
        {
            get
            {
                foreach (var item in Assertion.Conditions.Items)
                    if (item is OneTimeUse)
                        return true;
                return false;
            }
        }

        /// <summary>
        ///     The unencrypted attributes of the assertion.
        /// </summary>
        public List<SamlAttribute> Attributes
        {
            get
            {
                if (_assertionAttributes == null)
                    ExtractAttributes(); // Lazy initialization of the attributes list.                
                return _assertionAttributes;
            }
            set
            {
                // _assertionAttributes == null is reserved for signalling that the attribute is not initialized, so 
                // convert it to an empty list.
                if (value == null)
                    value = new List<SamlAttribute>(0);
                _assertionAttributes = value;
            }
        }

        /// <summary>
        ///     The encrypted attributes of the assertion.
        /// </summary>
        public List<EncryptedElement> EncryptedAttributes
        {
            get
            {
                if (_encryptedAssertionAttributes == null)
                    ExtractAttributes(); // Lazy initialization of the attributes list.
                return _encryptedAssertionAttributes;
            }

            set
            {
                // _encryptedAssertionAttributes == null is reserved for signalling that the attribute is not initialized, so 
                // convert it to an empty list.
                if (value == null)
                    value = new List<EncryptedElement>(0);

                _encryptedAssertionAttributes = value;
            }
        }

        /// <summary>
        ///     Gets or sets the encrypted id.
        /// </summary>
        /// <value>The encrypted id.</value>
        public string EncryptedId { get; set; }

        /// <summary>
        ///     Retrieve the NotOnOrAfter propoerty, if it is included in the assertion.
        /// </summary>
        public DateTime NotOnOrAfter
        {
            get
            {
                // Find the SubjectConfirmation element for the ValidTo attribute. [DKSAML] ch. 7.1.4.
                foreach (var o in Assertion.Subject.Items)
                    if (o is SubjectConfirmation)
                    {
                        var subjectConfirmation = (SubjectConfirmation) o;
                        if (subjectConfirmation.SubjectConfirmationData.NotOnOrAfter.HasValue)
                            return subjectConfirmation.SubjectConfirmationData.NotOnOrAfter.Value;
                    }

                return DateTime.MaxValue;
            }
        }

        /// <summary>
        ///     Gets the conditions element of the assertion.
        /// </summary>
        /// <value>The conditions element.</value>
        public Conditions Conditions => _assertion.Conditions;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Assertion" /> class.
        /// </summary>
        public Saml2Assertion()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Assertion" /> class.
        /// </summary>
        /// <param name="assertion">The assertion.</param>
        /// <param name="trustedSigners">If <code>null</code>, the signature of the given assertion is not verified.</param>
        /// <param name="audience"></param>
        /// <param name="quirksMode">if set to <c>true</c> quirks mode is enabled.</param>
        public Saml2Assertion(XmlElement assertion, IEnumerable<AsymmetricAlgorithm> trustedSigners,
            List<string> audience, bool quirksMode)
        {
            _audience = audience;
            _quirksMode = quirksMode;
            _profile = AssertionProfile.DKSaml;
            LoadXml(assertion, trustedSigners);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Assertion" /> class.
        /// </summary>
        /// <param name="assertion">The assertion.</param>
        /// <param name="trustedSigners">If <code>null</code>, the signature of the given assertion is not verified.</param>
        /// <param name="profile">Determines the type of validation to perform on the token</param>
        /// <param name="audience"></param>
        /// <param name="quirksMode">if set to <c>true</c> quirks mode is enabled.</param>
        public Saml2Assertion(XmlElement assertion, IEnumerable<AsymmetricAlgorithm> trustedSigners,
            AssertionProfile profile, List<string> audience, bool quirksMode)
        {
            _profile = profile;
            _audience = audience;
            _quirksMode = quirksMode;
            LoadXml(assertion, trustedSigners);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Assertion" /> class.
        /// </summary>
        /// <param name="assertion">The assertion.</param>
        /// <param name="trustedSigners">If <code>null</code>, the signature of the given assertion is not verified.</param>
        /// <param name="profile">Determines the type of validation to perform on the token</param>
        /// <param name="audience"></param>
        /// <param name="quirksMode">if set to <c>true</c> quirks mode is enabled.</param>
        /// <param name="autoValidate">Turn automatic validation on or off</param>
        public Saml2Assertion(XmlElement assertion, IEnumerable<AsymmetricAlgorithm> trustedSigners,
            AssertionProfile profile, List<string> audience, bool quirksMode, bool autoValidate)
        {
            _audience = audience;
            _profile = profile;
            _quirksMode = quirksMode;
            _autoValidate = autoValidate;
            LoadXml(assertion, trustedSigners);
        }

        #endregion
    }

    /// <summary>
    ///     Assertion validation profiles
    /// </summary>
    public enum AssertionProfile
    {
        /// <summary>
        ///     Validate assertion core profile
        /// </summary>
        Core,

        /// <summary>
        ///     Valiadate DKSaml profile (including core)
        /// </summary>
        DKSaml
    }
}