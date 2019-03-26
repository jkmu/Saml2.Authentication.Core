namespace dk.nita.saml20.Validation
{
    using Schema.Core;

    internal interface ISaml2StatementValidator
    {
        void ValidateStatement(StatementAbstract statement);
    }
}