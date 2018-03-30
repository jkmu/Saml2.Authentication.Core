using System;
using System.Xml;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20
{
    /// <summary>
    /// Encapsulates the LogoutResponse schema class
    /// </summary>
    public class Saml20LogoutResponse
    {
        private LogoutResponse _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml20LogoutResponse"/> class.
        /// </summary>
        public Saml20LogoutResponse()
        {
            _response = new LogoutResponse();
            _response.Version = Saml20Constants.Version;
            _response.ID = "id" + Guid.NewGuid().ToString("N");
            _response.Issuer = new NameID();
            _response.IssueInstant = DateTime.Now;
            _response.Status = new Status();
            _response.Status.StatusCode = new StatusCode();
        }

        /// <summary>
        /// Gets LogoutResponse as an XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.PreserveWhitespace = true;
            doc.LoadXml(Serialization.SerializeToXmlString(_response));
            return doc;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID
        {
            get
            {
                return _response.ID;
            }
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public string StatusCode
        {
            get { return _response.Status.StatusCode.Value; }
            set { _response.Status.StatusCode.Value = value;}
        }

        /// <summary>
        /// Gets the underlying Response schema class instance.
        /// </summary>
        /// <value>The response.</value>
        public LogoutResponse Response
        {
            get { return _response; }
        }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get { return _response.Issuer.Value; }
            set { _response.Issuer.Value = value; }
        }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        public string Destination
        {
            get { return _response.Destination; }
            set { _response.Destination = value; }
        }

        /// <summary>
        /// Gets or sets the id of the LogoutRequest to which this LogoutResponse corresponds.
        /// </summary>
        /// <value>InResponseTo.</value>
        public string InResponseTo
        {
            get { return _response.InResponseTo; }
            set { _response.InResponseTo = value; }
        }

    }
}
