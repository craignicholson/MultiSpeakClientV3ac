// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlUtil.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   Defines the XmlUtil type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.IO;

    /// <summary>
    /// The xml writer used to write a string formatted as xml by a serializer to disk.  
    /// </summary>
    public static class XmlUtil
    {
        /// <summary>
        /// The write to file.
        /// </summary>
        /// <param name="xml">
        ///     The xml.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        /// <param name="version">
        ///     The version.
        /// </param>
        /// <param name="fileDirectory">location of the files</param>
        public static void WriteToFile(string xml, string method, string version, string fileDirectory)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $@"{fileDirectory}\{method}.{version}.Response" + $@".{timestamp}.xml";
                using (var file = new StreamWriter(fileName, append: false))
                {
                    file.WriteLine(xml);
                }

                Console.WriteLine($"File writen to disk : {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
