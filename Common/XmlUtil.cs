using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NicoV2.Common
{
    public class XmlUtil
    {
        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀを指定したﾊﾟｽに保存します。
        /// </summary>
        /// <typeparam name="T">ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀの型</typeparam>
        /// <param name="filePath">保存するﾊﾟｽ</param>
        /// <param name="data">ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀ</param>
        public static void Serialize<T>(string filePath, T data)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Encoding = Constants.Encoding,
                Indent = true
            };
            using (var sw = XmlWriter.Create(filePath, settings))
            {
                serializer.WriteObject(sw, data);
            }
            //var se = new XmlSerializer(typeof(T));
            //using (var sw = new StreamWriter(filePath, false, new UTF8Encoding(false)))
            //{
            //    se.Serialize(sw, data);
            //}
        }

        /// <summary>
        /// 指定したﾊﾟｽをｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀとして読み込みます。
        /// </summary>
        /// <typeparam name="T">ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀの型</typeparam>
        /// <param name="filePath">読み込むﾊﾟｽ</param>
        /// <returns>ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀ</returns>
        public static T Deserialize<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                var serializer = new DataContractSerializer(typeof(T));
                using (var sr = XmlReader.Create(filePath))
                {
                    return (T)serializer.ReadObject(sr);
                }
                //var se = new XmlSerializer(typeof(T));

                //using (var sr = new StreamReader(filePath, new UTF8Encoding(false)))
                //{
                //    return (T)se.Deserialize(sr);
                //}
            }
            else
            {
                return default(T);
            }
        }
    }
}
