using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrectionEncryptedData.ConsoleApp.Models
{
    public class PublicKeyInfo
    {
        public byte[] PublicKey { get; }

        public PublicKeyInfo(byte[] publicKey)
        {
            PublicKey = publicKey;
        }
    }
}
