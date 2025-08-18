using System.Security.Cryptography;
using System.Text;

namespace Tools.Cryptographie
{
    public class RsaEncryption
    {
        private readonly RSACryptoServiceProvider _serviceProvider;

        public RsaEncryption(in AsymmetricKeySizes asymmetricKeySize = AsymmetricKeySizes.Size2048)
        {
            _serviceProvider = new RSACryptoServiceProvider((int)asymmetricKeySize);
        }

        public RsaEncryption(in byte[] keyBlob)
        {
            _serviceProvider = new RSACryptoServiceProvider();
            _serviceProvider.ImportCspBlob(keyBlob);
        }

        public RsaEncryption(in string keyPem)
        {
            _serviceProvider = new RSACryptoServiceProvider();
            _serviceProvider.ImportFromPem(keyPem);
        }

        public byte[] ExportAsBlob(bool includePrivateKey)
        {
            return _serviceProvider.ExportCspBlob(includePrivateKey);
        }

        public string ExportAsString(bool includePrivateKey)
        {
            return (includePrivateKey) ? _serviceProvider.ExportRSAPrivateKeyPem() : _serviceProvider.ExportRSAPublicKeyPem();
        }

        public bool PublicKeyOnly
        {
            get { return _serviceProvider.PublicOnly; }
        }

        public byte[] Encrypt(string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            byte[] toEncrypt = Encoding.Unicode.GetBytes(value);
            return Encrypt(toEncrypt);
        }

        public byte[] Encrypt(byte[] content)
        {
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            return _serviceProvider.Encrypt(content, true);
        }

        public byte[] Decrypt(byte[] cypher)
        {
            ArgumentNullException.ThrowIfNull(cypher, nameof(cypher));
            return _serviceProvider.Decrypt(cypher, true);
        }

        public string DecryptAsString(byte[] cypher)
        {
            return Encoding.Unicode.GetString(Decrypt(cypher));
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
