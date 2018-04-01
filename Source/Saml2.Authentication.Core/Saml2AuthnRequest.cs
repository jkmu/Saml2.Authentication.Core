using System;
using System.Collections.Generic;
using System.Xml;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20
{
    /// <summary>
    /// Encapsulates a SAML 2.0 authentication request
    /// </summary>
    public class Saml20AuthnRequest
    {
        private readonly AuthnRequest _request;

        #region Request properties

        /// <summary>
        /// The ID attribute of the &lt;AuthnRequest&gt; message.
        /// </summary>
        public string ID
        {
            get => _request.ID;
            set => _request.ID = value;
        }

        /// <summary>
        /// The 'Destination' attribute of the &lt;AuthnRequest&gt;.
        /// </summary>
        public string Destination
        {
            get => _request.Destination;
            set => _request.Destination = value;
        }

        ///<summary>
        /// The 'ForceAuthn' attribute of the &lt;AuthnRequest&gt;.
        ///</summary>
        public bool? ForceAuthn
        {
            get => _request.ForceAuthn;
            set => _request.ForceAuthn = value;
        }

        ///<summary>
        /// The 'IsPassive' attribute of the &lt;AuthnRequest&gt;.
        ///</summary>
        public bool? IsPassive
        {
            get => _request.IsPassive;
            set => _request.IsPassive = value;
        }

        /// <summary>
        /// Gets or sets the IssueInstant of the AuthnRequest.
        /// </summary>
        /// <value>The issue instant.</value>
        public DateTime? IssueInstant
        {
            get => _request.IssueInstant;
            set => _request.IssueInstant = value;
        }

        /// <summary>
        /// Gets or sets the issuer value.
        /// </summary>
        /// <value>The issuer value.</value>
        public string Issuer
        {
            get => _request.Issuer.Value;
            set => _request.Issuer.Value = value;
        }

        /// <summary>
        /// Gets or sets the issuer format.
        /// </summary>
        /// <value>The issuer format.</value>
        public string IssuerFormat
        {
            get => _request.Issuer.Format;
            set => _request.Issuer.Format = value;
        }

        #endregion

        /// <summary>
        /// Gets the underlying schema class object.
        /// </summary>
        /// <value>The request.</value>
        public AuthnRequest Request => _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml20AuthnRequest"/> class.
        /// </summary>
        public Saml20AuthnRequest()
        {
            _request = new AuthnRequest
            {
                Version = Saml20Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now
            };

        }

        private void SetConditions(List<ConditionAbstract> conditions)
        {
            _request.Conditions = new Conditions { Items = conditions };
        }

        /// <summary>
        /// Sets the ProtocolBinding on the request
        /// </summary>
        public string ProtocolBinding
        {
            get => _request.ProtocolBinding;
            set => _request.ProtocolBinding = value;
        }

        /// <summary>
        /// Returns the AuthnRequest as an XML document.
        /// </summary>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(_request));
            return doc;
        }
    }
}