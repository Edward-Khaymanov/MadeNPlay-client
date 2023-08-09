using Newtonsoft.Json;
using System.Text;

namespace LobbySystem
{
    public class BinarySerializer
    {
        public byte[] Serialize(object data)
        {
            if (data == null)
                return null;

            var serializedData = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(serializedData);
            return bytes;
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default;

            var bytes = Encoding.UTF8.GetString(data);
            var result = JsonConvert.DeserializeObject<T>(bytes);
            return result;
        }
    }
}