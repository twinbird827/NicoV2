using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static void SaveXml<T>(string filePath, T data)
        {
            var se = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(filePath, false, new UTF8Encoding(false)))
            {
                se.Serialize(sw, data);
            }
        }

        /// <summary>
        /// 指定したﾊﾟｽをｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀとして読み込みます。
        /// </summary>
        /// <typeparam name="T">ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀの型</typeparam>
        /// <param name="filePath">読み込むﾊﾟｽ</param>
        /// <returns>ｵﾌﾞｼﾞｪｸﾄﾃﾞｰﾀ</returns>
        public static T ReadXml<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                var se = new XmlSerializer(typeof(T));

                using (var sr = new StreamReader(filePath, new UTF8Encoding(false)))
                {
                    return (T)se.Deserialize(sr);
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}
