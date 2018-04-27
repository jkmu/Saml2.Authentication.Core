using System;

namespace dk.nita.saml20
{
    /// <summary>
    ///     This exception is thrown to indicate an error in the SAML2 toolkit. It was introduced to make it easy to
    ///     distinguish between
    ///     exceptions thrown deliberately by the toolkit, and exceptions that are thrown as the result of bugs.
    /// </summary>
    public class Saml2Exception : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Exception" /> class.
        /// </summary>
        public Saml2Exception()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Exception" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public Saml2Exception(string msg) : base(msg)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2Exception" /> class.
        /// </summary>
        /// <param name="msg">A message describing the problem that caused the exception.</param>
        /// <param name="cause">Another exception that may be related to the problem.</param>
        public Saml2Exception(string msg, Exception cause) : base(msg, cause)
        {
        }
    }
}