using System;
using System.Xml;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20
{
    /// <summary>
    ///     Encapsulates the LogoutResponse schema class
    /// </summary>
    public class Saml2LogoutResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2LogoutResponse" /> class.
        /// </summary>
        public Saml2LogoutResponse()
        {
            Response = new LogoutResponse
            {
                Version = Saml2Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now,
                Status = new Status {StatusCode = new StatusCode()}
            };
        }

        /// <summary>
        ///     Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID => Response.ID;

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public string StatusCode
        {
            get => Response.Status.StatusCode.Value;
            set => Response.Status.StatusCode.Value = value;
        }

        /// <summary>
        ///     Gets the underlying Response schema class instance.
        /// </summary>
        /// <value>The response.</value>
        public LogoutResponse Response { get; }

        /// <summary>
        ///     Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get => Response.Issuer.Value;
            set => Response.Issuer.Value = value;
        }

        /// <summary>
        ///     Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        public string Destination
        {
            get => Response.Destination;
            set => Response.Destination = value;
        }

        /// <summary>
        ///     Gets or sets the id of the LogoutRequest to which this LogoutResponse corresponds.
        /// </summary>
        /// <value>InResponseTo.</value>
        public string InResponseTo
        {
            get => Response.InResponseTo;
            set => Response.InResponseTo = value;
        }

        /// <summary>
        ///     Gets LogoutResponse as an XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(Response));
            return doc;
        }
    }
}