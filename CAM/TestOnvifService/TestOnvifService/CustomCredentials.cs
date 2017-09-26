using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace TestOnvifService
{
    public class CustomCredentials : ClientCredentials
    {
        public DateTime? ClientSyncDTG = null;

        public CustomCredentials(DateTime? clientSyncDTG = null)
        {
            ClientSyncDTG = clientSyncDTG;
        }

        protected CustomCredentials(CustomCredentials cc)
            : base(cc)
        { }

        public override System.IdentityModel.Selectors.SecurityTokenManager CreateSecurityTokenManager()
        {
            return new CustomSecurityTokenManager(this);
        }

        protected override ClientCredentials CloneCore()
        {
            return new CustomCredentials(this);
        }
    }

    public class CustomSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        public CustomSecurityTokenManager(CustomCredentials cred)
            : base(cred)
        { }

        public override System.IdentityModel.Selectors.SecurityTokenSerializer CreateSecurityTokenSerializer(System.IdentityModel.Selectors.SecurityTokenVersion version)
        {
            CustomCredentials cred = (CustomCredentials)ClientCredentials;
            return new CustomTokenSerializer(System.ServiceModel.Security.SecurityVersion.WSSecurity11, cred.ClientSyncDTG);
        }
    }

    public class CustomTokenSerializer : WSSecurityTokenSerializer
    {
        private DateTime? _clientSyncDTG = null;

        public CustomTokenSerializer(SecurityVersion sv, DateTime? clientSyncDTG = null)
            : base(sv)
        {
            _clientSyncDTG = clientSyncDTG;
        }

        protected override void WriteTokenCore(System.Xml.XmlWriter writer,
                                                System.IdentityModel.Tokens.SecurityToken token)
        {
            UserNameSecurityToken userToken = token as UserNameSecurityToken;
            string tokennamespace = "o";

            DateTime created = _clientSyncDTG == null ? DateTime.Now : _clientSyncDTG.Value;
            string createdStr = created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            // unique Nonce value - encode with SHA-1 for 'randomness'
            // in theory the nonce could just be the GUID by itself
            string phrase = Guid.NewGuid().ToString();
            var nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(phrase));
            //var nonce = GetSHA1String(phrase);

            // in this case password is plain text
            // for digest mode password needs to be encoded as:
            //PasswordAsDigest = Base64(SHA-1(Nonce + Created + Password))
            // and profile needs to change to
            //string password = GetSHA1String(nonce + createdStr + userToken.Password);

            byte[] bdate = Encoding.UTF8.GetBytes(createdStr);
            byte[] bpassword = Encoding.UTF8.GetBytes(userToken.Password);
            byte[] bnonce = Encoding.UTF8.GetBytes(phrase);
            List<byte> lstB = new List<byte>();
            lstB.AddRange(bnonce);
            lstB.AddRange(bdate);
            lstB.AddRange(bpassword);

            string password = GetSHA1String(lstB.ToArray()); //userToken.Password;

            //AppendLine sẽ die => Chỉ dùng Append thôi
            StringBuilder sb = new StringBuilder();
            sb.Append("<{0}:UsernameToken u:Id=\"" + token.Id + "\" xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">");
            sb.Append("<{0}:Username>" + userToken.UserName + "</{0}:Username>");
            sb.Append("<{0}:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">");
            sb.Append(password);
            sb.Append("</{0}:Password>");
            sb.Append("<{0}:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">");
            sb.Append(nonce);
            sb.Append("</{0}:Nonce>");
            sb.Append("<u:Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">");
            sb.Append(createdStr);
            sb.Append("</u:Created>");
            sb.Append("</{0}:UsernameToken>");
            string text = string.Format(sb.ToString(), tokennamespace);

            writer.WriteRaw(text);
        }

        public static string GetSHA1String(byte[] input)
        {
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1Hasher.ComputeHash(input);
            return Convert.ToBase64String(hashedDataBytes);
        }

        public static string GetSHA1String(string phrase)
        {
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(phrase));
            return Convert.ToBase64String(hashedDataBytes);
        }
    }
}
