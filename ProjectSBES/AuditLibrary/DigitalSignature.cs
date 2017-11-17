﻿using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuditLibrary
{
    public class DigitalSignature
    {
        /// <summary>
		/// 
		/// </summary>
		/// <param name="message"> a message/text to be digitally signed </param>
		/// <param name="hashAlgorithm"> an arbitrary hash algorithm </param>
		/// <param name="certificate"> certificate of a user who creates a signature </param>
		/// <returns> byte array representing a digital signature for the given message </returns>
		public static byte[] Create(SecurityEvent message, string hashAlgorithm, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = null;

            /// Looks for the certificate's private key to sign a message
            /// 
            if (certificate != null)
            {
                csp = (RSACryptoServiceProvider)certificate.PrivateKey;
            }
            if (csp == null)
            {
                //nema validnog sertifikata
                throw new Exception("Valid certificate was not found");
            }
            /// hash the message using SHA-1 (assume that "hashAlgorithm" is SHA-1)
            SHA1Managed sha1 = new SHA1Managed();
            
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream data;
            using (data = new MemoryStream())
            {
                bf.Serialize(data, message);
            }

            byte[] hash = sha1.ComputeHash(data.ToArray());

            /// Use RSACryptoServiceProvider support to create a signature using a previously created hash value
            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            return signature;
        }
        public static bool Verify(SecurityEvent message, string hashAlgorithm, byte[] signature, X509Certificate2 certificate)
        {
            /// Looks for the certificate's public key to verify a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            /// hash the message using SHA-1 (assume that "hashAlgorithm" is SHA-1)
            SHA1Managed sha1 = new SHA1Managed();

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream data;
            using (data = new MemoryStream())
            {
                bf.Serialize(data, message);
            }
            byte[] hash = sha1.ComputeHash(data.ToArray());


            /// Use RSACryptoServiceProvider support to compare two - hash value from signature and newly created hash value			
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }
    }
}
