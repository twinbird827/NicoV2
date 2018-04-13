using NicoV2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.IO
{
    public abstract class Ini
    {
        /// <summary>
        /// INIﾌｧｲﾙのﾊﾟｽ
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// ｾｸｼｮﾝ
        /// </summary>
        private string Section { get; set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="path">読み取るINIﾌｧｲﾙのﾊﾟｽ</param>
        /// <param name="section">規定のｾｸｼｮﾝ</param>
        protected Ini(string path, string section)
        {
            Path = path;
            Section = section;
        }

        /// <summary>
        /// sectionとkeyからiniﾌｧｲﾙの設定値を取得、設定します。 
        /// </summary>
        /// <returns>指定したsectionとkeyの組合せが無い場合は""が返ります。</returns>
        protected string this[string section, string key, string defaultValue = ""]
        {
            set
            {
                Win32Methods.WritePrivateProfileString(section, key, value, Path);
            }
            get
            {
                StringBuilder sb = new StringBuilder(256);
                Win32Methods.GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, Path);
                return sb.ToString();
            }
        }

        /// <summary>
        /// keyからiniﾌｧｲﾙの設定値を取得、設定します。sectionは規定値を指定します。
        /// </summary>
        /// <returns>keyの組合せが無い場合は""が返ります。</returns>
        protected string this[string key, string defaultValue = ""]
        {
            set
            {
                this[Section, key, defaultValue] = value;
            }
            get
            {
                return this[Section, key, defaultValue];
            }
        }

    }
}
