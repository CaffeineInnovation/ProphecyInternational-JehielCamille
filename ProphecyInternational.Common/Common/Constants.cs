using System;

namespace ProphecyInternational.Common.Common
{
    public static class Constants
    {
        public const string DEFAULT_CONNECTION = "DefaultConnection";
        public const string ALLOW_LOCALHOST = "AllowLocalhost";
        public const string JWT_KEY = "Jwt:Key";
        public const string JWT_ISSUER = "Jwt:Issuer";
        public const string JWT_USER_NAME = "Jwt:UserName";
        public const string JWT_PASSWORD = "Jwt:Password";
        public const string BEARER = "Bearer";
        public const string JWT = "Jwt";
        public const string AUTHORIZATION = "Authorization";
        public const string ENTER_JWT_DESC = "Enter '{JWT token}' in the text input below.";

        public static readonly DateTime DEFAULT_DATE = new DateTime(2025, 02, 11);

        public static class Labels
        {
            public const string TITLE = "ProphecyInternational: Call Center Management";
            public const string V1 = "v1";
        }
    }
}
