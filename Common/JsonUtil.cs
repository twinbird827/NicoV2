using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Common
{
    public class JsonUtil
    {

        public static void Serialize<T>(string filePath, T data)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(filePath, false, Constants.Encoding))
            {
                var settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true,
                };
                var serializer = new DataContractJsonSerializer(typeof(T), settings);
                serializer.WriteObject(stream, data);
                writer.Write(Constants.Encoding.GetString(stream.ToArray()));
            }
        }

        public static T Deserialize<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            var message = File.ReadAllText(filePath);
            using (var stream = new MemoryStream(Constants.Encoding.GetBytes(message)))
            {
                var settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true,
                };
                var serializer = new DataContractJsonSerializer(typeof(T), settings);
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
