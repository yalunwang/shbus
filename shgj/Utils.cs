using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shgj
{
    public  class Utils
    {
        /// <summary>
        /// 记录文本日志
        /// </summary>
        /// <param name="str">日志内容</param>
        /// <param name="path">文件物理地址</param>
        public static void WriteLog(string str, string path)
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path, true);
                sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                sw.WriteLine(str);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
                Console.WriteLine(e);
            }
        }
    }
}
