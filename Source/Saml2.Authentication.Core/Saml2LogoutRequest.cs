using System;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core
{
    /// <summary>
    ///     Encapsulates the LogoutRequest schema class
    /// </summary>
    public class Saml2LogoutRequest
    {
        #region Constructor functions

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2LogoutRequest" /> class.
        /// </summary>
        public Saml2LogoutRequest()
        {
            Request = new LogoutRequest
            {
                Version = Saml2Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now
            };
        }

        #endregion

        /// <summary>
        ///     Returns the id of the logout request.
        /// </summary>
        public string ID => Request.ID;

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

        #region Properties

        /// <summary>
        ///     Gets or sets the reason for this logout request.
        ///     Defined values should be on uri form.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason
        {
            get => Request.Reason;
            set => Request.Reason = value;
        }

        /// <summary>
        ///     Gets or sets NotOnOrAfter.
        /// </summary>
        /// <value>NotOnOrAfter.</value>
        public DateTime? NotOnOrAfter
        {
            get => Request.NotOnOrAfter;
            set => Request.NotOnOrAfter = value;
        }

        /// <summary>
        ///     Gets or sets SubjectToLogOut.
        /// </summary>
        /// <value>SubjectToLogOut.</value>
        public NameID SubjectToLogOut
        {
            get => Request.Item as NameID;
            set => Request.Item = value;
        }

        /// <summary>
        ///     Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        public string Destination
        {
            get => Request.Destination;
            set => Request.Destination = value;
        }

        /// <summary>
        ///     Gets or sets the SessionIndex.
        /// </summary>
        /// <value>The SessionIndex.</value>
        public string SessionIndex
        {
            get => Request.SessionIndex[0];
            set => Request.SessionIndex = new[] {value};
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
        ///     Gets the underlying LogoutRequest schema class instance.
        /// </summary>
        /// <value>The request.</value>
        public LogoutRequest Request { get; }

        #endregion
    }
}