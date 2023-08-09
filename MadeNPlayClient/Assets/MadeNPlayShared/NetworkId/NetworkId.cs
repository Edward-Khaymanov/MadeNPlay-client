using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MadeNPlayShared
{
    public class NetworkId : MonoBehaviour
    {
        [SerializeField, HideInInspector] private uint _id;

        public uint Id => _id;

        public void Generate()
        {
            var guid = Guid.NewGuid().ToString();
            _id = GenerateHash(guid);
        }

        private uint GenerateHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToUInt32(hashBytes, 0);
            }
        }
    }
}