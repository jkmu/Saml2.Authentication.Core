using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The DSA KeyValue
    /// DSA keys and the DSA signature algorithm are specified in [DSS]. DSA public key values can have the following fields: 
    /// 
    /// P - a prime modulus meeting the [DSS] requirements 
    /// 
    /// Q - an integer in the range 2**159 &lt; Q &lt; 2**160 which is a prime divisor of P-1 
    /// 
    /// G - an integer with certain properties with respect to P and Q 
    /// 
    /// Y - G**X mod P (where X is part of the private key and not made public) 
    /// 
    /// J - (P - 1) / Q 
    /// 
    /// seed - a DSA prime generation seed 
    /// 
    /// pgenCounter - a DSA prime generation counter 
    /// 
    /// Parameter J is available for inclusion solely for efficiency as it is calculatable from P and Q. 
    /// Parameters seed and pgenCounter are used in the DSA prime number generation algorithm specified in 
    /// [DSS]. As such, they are optional but must either both be present or both be absent. This prime 
    /// generation algorithm is designed to provide assurance that a weak prime is not being used and it yields 
    /// a P and Q value. Parameters P, Q, and G can be public and common to a group of users. They might be 
    /// known from application context. As such, they are optional but P and Q must either both appear or both 
    /// be absent. If all of P, Q, seed, and pgenCounter are present, implementations are not required to check 
    /// if they are consistent and are free to use either P and Q or seed and pgenCounter. All parameters are 
    /// encoded as base64 [MIME] values. 
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class DSAKeyValue
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "DSAKeyValue";

        private byte[] gField;

        private byte[] jField;
        private byte[] pField;

        private byte[] pgenCounterField;
        private byte[] qField;
        private byte[] seedField;
        private byte[] yField;


        /// <summary>
        /// Gets or sets the P.
        /// </summary>
        /// <value>The P.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] P
        {
            get { return pField; }
            set { pField = value; }
        }


        /// <summary>
        /// Gets or sets the Q.
        /// </summary>
        /// <value>The Q.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] Q
        {
            get { return qField; }
            set { qField = value; }
        }


        /// <summary>
        /// Gets or sets the G.
        /// </summary>
        /// <value>The G.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] G
        {
            get { return gField; }
            set { gField = value; }
        }


        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] Y
        {
            get { return yField; }
            set { yField = value; }
        }


        /// <summary>
        /// Gets or sets the J.
        /// </summary>
        /// <value>The J.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] J
        {
            get { return jField; }
            set { jField = value; }
        }


        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>The seed.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] Seed
        {
            get { return seedField; }
            set { seedField = value; }
        }


        /// <summary>
        /// Gets or sets the pgen counter.
        /// </summary>
        /// <value>The pgen counter.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] PgenCounter
        {
            get { return pgenCounterField; }
            set { pgenCounterField = value; }
        }
    }
}