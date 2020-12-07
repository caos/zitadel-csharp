﻿namespace Zitadel.Authentication
{
    /// <summary>
    /// A set of default values for Zitadel themed authentication/authorization.
    /// </summary>
    public static class ZitadelDefaults
    {
        /// <summary>
        /// Default display name.
        /// </summary>
        public const string DisplayName = "Zitadel";

        /// <summary>
        /// Default authentication scheme name for AddZitadel() and
        /// AddZitadelWithSession().
        /// </summary>
        public const string AuthenticationScheme = "Zitadel";

        /// <summary>
        /// Default authentication scheme name for AddZitadelAuthenticationHandler().
        /// </summary>
        public const string HandlerAuthenticationScheme = "ZitadelAuthHandler";

        /// <summary>
        /// Default online (non self hosted) Zitadel issuer.
        /// <a href="https://console.zitadel.ch">Zitadel.ch</a>
        /// </summary>
        public const string Issuer = "https://issuer.zitadel.ch";

        /// <summary>
        /// Default callback path for local login redirection.
        /// </summary>
        public const string CallbackPath = "/signin-zitadel";

        /// <summary>
        /// Default well-known discovery url for the online Zitadel instance.
        /// </summary>
        public const string DiscoveryEndpoint = "https://issuer.zitadel.ch/.well-known/openid-configuration";

        /// <summary>
        /// The name for the token claim (JWT or opaque via user-info endpoint)
        /// that contains rule allocations.
        /// </summary>
        public const string RoleClaimName = "urn:zitadel:iam:org:project:roles";

        /// <summary>
        /// The name for the token claim (JWT or opaque via user-info endpoint)
        /// that contains the scoped and used primary domain.
        /// </summary>
        public const string PrimaryDomainClaimName = "urn:zitadel:iam:org:domain:primary";
    }
}