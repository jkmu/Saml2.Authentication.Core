using System;
using System.Collections.Generic;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core
{
    /// <summary>
    ///     Encapsulates a SAML 2.0 authentication request
    /// </summary>
    public class Saml2AuthnRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2AuthnRequest" /> class.
        /// </summary>
        public Saml2AuthnRequest()
        {
            Request = new AuthnRequest
            {
                Version = Saml2Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now
            };
        }

        /// <summary>
        ///     Gets the underlying schema class object.
        /// </summary>
        /// <value>The request.</value>
        public AuthnRequest Request { get; }

        /// <summary>
        ///     Sets the ProtocolBinding on the request
        /// </summary>
        public string ProtocolBinding
        {
            get => Request.ProtocolBinding;
            set => Request.ProtocolBinding = value;
        }

        private void SetConditions(List<ConditionAbstract> conditions)
        {
            Request.Conditions = new Conditions {Items = conditions};
        }

        /// <summary>
        ///     Returns the AuthnRequest as an XML document.
        /// </summary>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(Request));
            return doc;
        }

        #region Request properties

        /// <summary>
        ///     The ID attribute of the &lt;AuthnRequest&gt; message.
        /// </summary>
        public string ID
        {
            get => Request.ID;
            set => Request.ID = value;
        }

        /// <summary>
        ///     The 'Destination' attribute of the &lt;AuthnRequest&gt;.
        /// </summary>
        public string Destination
        {
            get => Request.Destination;
            set => Request.Destination = value;
        }

        /// <summary>
        ///     The 'ForceAuthn' attribute of the &lt;AuthnRequest&gt;.
        /// </summary>
        public bool? ForceAuthn
        {
            get => Request.ForceAuthn;
            set => Request.ForceAuthn = value;
        }

        /// <summary>
        ///     The 'IsPassive' attribute of the &lt;AuthnRequest&gt;.
        /// </summary>
        public bool? IsPassive
        {
            get => Request.IsPassive;
            set => Request.IsPassive = value;
        }

        /// <summary>
        ///     Gets or sets the IssueInstant of the AuthnRequest.
        /// </summary>
        /// <value>The issue instant.</value>
        public DateTime? IssueInstant
        {
            get => Request.IssueInstant;
            set => Request.IssueInstant = value;
        }

        /// <summary>
        ///     Gets or sets the issuer value.
        /// </summary>
        /// <value>The issuer value.</value>
        public string Issuer
        {
            get => Request.Issuer.Value;
            set => Request.Issuer.Value = value;
        }

        /// <summary>
        ///     Gets or sets the issuer format.
        /// </summary>
        /// <value>The issuer format.</value>
        public string IssuerFormat
        {
            get => Request.Issuer.Format;
            set => Request.Issuer.Format = value;
        }

        #endregion
    }
}