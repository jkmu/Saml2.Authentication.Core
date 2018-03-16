using System;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Signature = dk.nita.saml20.Schema.XmlDSig.Signature;
using System.Collections.Generic;

namespace dk.nita.saml20.Utils
{
    ///<summary>
    ///</summary>
    public class SignedXMLWithIdResolvement : SignedXml
    {
        ///<summary>
        ///</summary>
        ///<param name="document"></param>
        public SignedXMLWithIdResolvement(XmlDocument document)
            : base(document)
        {

        }
        ///<summary>
        ///</summary>
        ///<param name="elem"></param>
        public SignedXMLWithIdResolvement(XmlElement elem)
            : base(elem)
        {

        }
        ///<summary>
        ///</summary>
        public SignedXMLWithIdResolvement()
            : base()
        {

        }

        /// <summary>
        /// </summary>
        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlElement elem = null;
            if ((elem = base.GetIdElement(document, idValue)) == null)
            {
                XmlNodeList nl = document.GetElementsByTagName("*");
                IEnumerator enumerator = nl.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var node = (XmlNode)enumerator.Current;
                    var nodeEnum = node.Attributes.GetEnumerator();
                    while (nodeEnum.MoveNext())
                    {
                        var attr = (XmlAttribute)nodeEnum.Current;
                        if (attr.LocalName.ToLower() == "id" && attr.Value == idValue && node is XmlElement)
                        {
                            return (XmlElement)node;
                        }
                    }
                }
            }
            return elem;
        }
    }
    /// <summary>
    /// This class contains methods that creates and validates signatures on XmlDocuments.
    /// </summary>
    public class XmlSignatureUtils
    {
        /// <summary>
        /// Verifies the signature of the XmlDocument instance using the key enclosed with the signature.
        /// </summary>
        /// <returns><code>true</code> if the document's signature can be verified. <code>false</code> if the signature could
        /// not be verified.</returns>
        /// <exception cref="InvalidOperationException">if the XmlDocument instance does not contain a signed XML document.</exception>
        public static bool CheckSignature(XmlDocument doc)
        {
            CheckDocument(doc);
            SignedXml signedXml = RetrieveSignature(doc);

            var lCert = GetCertificates(doc);
            if (CheckSignature(signedXml, lCert))
                return true;
            return false;
        }

        private static List<X509Certificate2> GetCertificates(XmlDocument doc)
        {
            List<X509Certificate2> lCert = new List<X509Certificate2>();
            XmlNodeList nodeList = doc.GetElementsByTagName("ds:X509Certificate");

            if (nodeList.Count == 0)
                nodeList = doc.GetElementsByTagName("X509Certificate");

            foreach (XmlNode xn in nodeList)
            {
                try
                {
                    X509Certificate2 xc = new X509Certificate2(Convert.FromBase64String(xn.InnerText));
                    lCert.Add(xc);
                }
                catch { }
            }

            return lCert;
        }

        /// <summary>
        /// Verifies the signature of the XmlDocument instance using the key given as a parameter.
        /// </summary>        
        /// <returns><code>true</code> if the document's signature can be verified. <code>false</code> if the signature could
        /// not be verified.</returns>
        /// <exception cref="InvalidOperationException">if the XmlDocument instance does not contain a signed XML document.</exception>
        public static bool CheckSignature(XmlDocument doc, AsymmetricAlgorithm alg)
        {
            CheckDocument(doc);
            SignedXml signedXml = RetrieveSignature(doc);
            return signedXml.CheckSignature(alg);
        }

        /// <summary>
        /// Verifies the signature of the XmlElement instance using the key given as a parameter.
        /// </summary>        
        /// <returns><code>true</code> if the element's signature can be verified. <code>false</code> if the signature could
        /// not be verified.</returns>
        /// <exception cref="InvalidOperationException">if the XmlDocument instance does not contain a signed XML element.</exception>
        public static bool CheckSignature(XmlElement el, AsymmetricAlgorithm alg)
        {
            SignedXml signedXml = RetrieveSignature(el);
            return signedXml.CheckSignature(alg);
        }

        /// <summary>
        /// Checks the signature using a list of certificates
        /// </summary>
        /// <param name="signedXml">Signed xml object for signature</param>
        /// <param name="trustedCertificates">List of certificates</param>
        /// <returns>true if signature is verified</returns>
        private static bool CheckSignature(SignedXml signedXml, IEnumerable<X509Certificate2> trustedCertificates)
        {
            foreach (X509Certificate2 cert in trustedCertificates)
            {
                if (signedXml.CheckSignature(cert.PublicKey.Key))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Verify the given document using a KeyInfo instance. The KeyInfo instance's KeyClauses will be traversed for 
        /// elements that can verify the signature, eg. certificates or keys. If nothing is found, an exception is thrown.
        /// </summary>
        public static bool CheckSignature(XmlDocument doc, KeyInfo keyinfo)
        {
            CheckDocument(doc);
            SignedXml signedXml = RetrieveSignature(doc);

            AsymmetricAlgorithm alg = null;
            X509Certificate2 cert = null;
            foreach (KeyInfoClause clause in keyinfo)
            {
                if (clause is RSAKeyValue)
                {
                    RSAKeyValue key = (RSAKeyValue)clause;
                    alg = key.Key;
                    break;
                }
                else if (clause is KeyInfoX509Data)
                {
                    KeyInfoX509Data x509data = (KeyInfoX509Data)clause;
                    int count = x509data.Certificates.Count;
                    cert = (X509Certificate2)x509data.Certificates[count - 1];
                }
                else if (clause is DSAKeyValue)
                {
                    DSAKeyValue key = (DSAKeyValue)clause;
                    alg = key.Key;
                    break;
                }
            }

            if (alg == null && cert == null)
                throw new InvalidOperationException("Unable to locate the key or certificate to verify the signature.");

            if (alg != null)
                return signedXml.CheckSignature(alg);
            else
                return signedXml.CheckSignature(cert, true);
        }

        /// <summary>
        /// Attempts to retrieve an asymmetric key from the KeyInfoClause given as parameter.        
        /// </summary>
        /// <param name="keyInfoClause"></param>
        /// <returns>null if the key could not be found.</returns>
        public static AsymmetricAlgorithm ExtractKey(KeyInfoClause keyInfoClause)
        {
            if (keyInfoClause is RSAKeyValue)
            {
                RSAKeyValue key = (RSAKeyValue)keyInfoClause;
                return key.Key;
            }
            else if (keyInfoClause is KeyInfoX509Data)
            {
                X509Certificate2 cert = GetCertificateFromKeyInfo((KeyInfoX509Data)keyInfoClause);

                return cert != null ? cert.PublicKey.Key : null;
            }
            else if (keyInfoClause is DSAKeyValue)
            {
                DSAKeyValue key = (DSAKeyValue)keyInfoClause;
                return key.Key;
            }

            return null;
        }

        /// <summary>
        /// Gets the certificate from key info.
        /// </summary>
        /// <param name="keyInfo">The key info.</param>
        /// <returns>The last certificate in the chain</returns>
        public static X509Certificate2 GetCertificateFromKeyInfo(KeyInfoX509Data keyInfo)
        {
            int count = keyInfo.Certificates.Count;
            if (count == 0)
                return null;

            X509Certificate2 cert = (X509Certificate2)keyInfo.Certificates[count - 1];
            return cert;
        }

        /// <summary>
        /// Do checks on the document given. Every public method accepting a XmlDocument instance as parameter should 
        /// call this method before continuing.
        /// </summary>        
        private static void CheckDocument(XmlDocument doc)
        {
            if (!doc.PreserveWhitespace)
                throw new InvalidOperationException(
                    "The XmlDocument must have its \"PreserveWhitespace\" property set to true when a signed document is loaded.");
        }

        /// <summary>
        /// Do checks on the element given. Every public method accepting a XmlElement instance as parameter should 
        /// call this method before continuing.
        /// </summary>        
        private static void CheckDocument(XmlElement el)
        {
            if (!el.OwnerDocument.PreserveWhitespace)
                throw new InvalidOperationException(
                    "The XmlDocument must have its \"PreserveWhitespace\" property set to true when a signed document is loaded.");
        }

        /// <summary>
        /// Checks if a document contains a signature.
        /// </summary>
        public static bool IsSigned(XmlDocument doc)
        {
            CheckDocument(doc);
            XmlNodeList nodeList =
                doc.GetElementsByTagName(Signature.ELEMENT_NAME, Saml20Constants.XMLDSIG);

            return nodeList.Count > 0;
        }

        /// <summary>
        /// Checks if an element contains a signature.
        /// </summary>
        public static bool IsSigned(XmlElement el)
        {
            CheckDocument(el);
            XmlNodeList nodeList =
                el.GetElementsByTagName(Signature.ELEMENT_NAME, Saml20Constants.XMLDSIG);

            return nodeList.Count > 0;
        }

        /// <summary>
        /// Returns the KeyInfo element that is included with the signature in the document.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the document is not signed.</exception>
        public static KeyInfo ExtractSignatureKeys(XmlDocument doc)
        {
            CheckDocument(doc);
            SignedXml signedXml = new SignedXml(doc.DocumentElement);

            XmlNodeList nodeList = doc.GetElementsByTagName(Signature.ELEMENT_NAME, Saml20Constants.XMLDSIG);
            if (nodeList.Count == 0)
                throw new InvalidOperationException("The XmlDocument does not contain a signature.");

            signedXml.LoadXml((XmlElement)nodeList[0]);
            return signedXml.KeyInfo;
        }

        /// <summary>
        /// Returns the KeyInfo element that is included with the signature in the element.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the document is not signed.</exception>
        public static KeyInfo ExtractSignatureKeys(XmlElement el)
        {
            CheckDocument(el);
            SignedXml signedXml = new SignedXml(el);

            XmlNodeList nodeList = el.GetElementsByTagName(Signature.ELEMENT_NAME, Saml20Constants.XMLDSIG);
            if (nodeList.Count == 0)
                throw new InvalidOperationException("The XmlDocument does not contain a signature.");

            signedXml.LoadXml((XmlElement)nodeList[0]);
            return signedXml.KeyInfo;
        }

        /// <summary>
        /// Digs the &lt;Signature&gt; element out of the document.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the document does not contain a signature.</exception>
        private static SignedXml RetrieveSignature(XmlDocument doc)
        {
            return RetrieveSignature(doc.DocumentElement);
        }

        /// <summary>
        /// Digs the &lt;Signature&gt; element out of the document.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the document does not contain a signature.</exception>
        private static SignedXml RetrieveSignature(XmlElement el)
        {
            SignedXml signedXml = new SignedXMLWithIdResolvement(el);
            XmlNodeList nodeList = el.GetElementsByTagName(Signature.ELEMENT_NAME, Saml20Constants.XMLDSIG);
            if (nodeList.Count == 0)
                throw new InvalidOperationException("Document does not contain a signature to verify.");

            signedXml.LoadXml((XmlElement)nodeList[0]);

            // verify that the inlined signature has a valid reference uri
            VerifyRererenceURI(signedXml, el.GetAttribute("ID"));

            return signedXml;
        }

        /// <summary>
        /// Verifies that the reference uri (if any) points to the correct element.
        /// </summary>
        /// <param name="signedXml">the ds:signature element</param>
        /// <param name="id">the expected id referenced by the ds:signature element</param>
        private static void VerifyRererenceURI(SignedXml signedXml, string id)
        {
            if (id == null)
            {
                throw new InvalidOperationException("Cannot match null id");
            }

            if (signedXml.SignedInfo.References.Count > 0)
            {
                Reference reference = (Reference)signedXml.SignedInfo.References[0];
                string uri = reference.Uri;

                // empty uri is okay - indicates that everything is signed
                if (uri != null && uri.Length > 0)
                {
                    if (!uri.StartsWith("#"))
                    {
                        throw new InvalidOperationException("Signature reference URI is not a document fragment reference. Uri = '" + uri + "'");
                    }
                    else if (uri.Length < 2 || !id.Equals(uri.Substring(1)))
                    {
                        throw new InvalidOperationException("Rererence URI = '" + uri.Substring(1) + "' does not match expected id = '" + id + "'");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("No references in Signature element");
            }
        }

        /// <summary>
        /// Signs an XmlDocument with an xml signature using the signing certificate given as argument to the method.
        /// </summary>
        /// <param name="doc">The XmlDocument to be signed</param>
        /// <param name="id">The is of the topmost element in the xmldocument</param>
        /// <param name="cert">The certificate used to sign the document</param>
        public static void SignDocument(XmlDocument doc, string id, X509Certificate2 cert)
        {
            SignedXml signedXml = new SignedXml(doc);
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SigningKey = cert.PrivateKey;

            // Retrieve the value of the "ID" attribute on the root assertion element.
            Reference reference = new Reference("#" + id);

            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());

            signedXml.AddReference(reference);

            // Include the public key of the certificate in the assertion.
            signedXml.KeyInfo = new KeyInfo();
            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert, X509IncludeOption.WholeChain));

            signedXml.ComputeSignature();
            // Append the computed signature. The signature must be placed as the sibling of the Issuer element.
            XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("Issuer", Saml20Constants.ASSERTION);
            // doc.DocumentElement.InsertAfter(doc.ImportNode(signedXml.GetXml(), true), nodes[0]);            
            nodes[0].ParentNode.InsertAfter(doc.ImportNode(signedXml.GetXml(), true), nodes[0]);
        }
    }
}