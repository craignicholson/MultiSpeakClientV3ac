// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EaServerRequests.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   Defines the EaServerRequests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.Net;

    using MultiSpeakClientV30ac.proxyEA3ac;

    /// <summary>
    /// The EA server requests.
    /// </summary>
    public static class EaServerRequests
    {
        /// <summary>
        /// The log file directory.
        /// </summary>
        private static string logFileDirectory;

        /// <summary>
        /// Gets or sets the log file directory.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetLogFileDirectory()
        {
            return logFileDirectory;
        }

        /// <summary>
        /// Gets or sets the log file directory.  If the directory is not set the file will write to same directory as the bin.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetLogFileDirectory(string value)
        {
            logFileDirectory = value;
        }

        /// <summary>
        /// The run command.
        /// </summary>
        /// <param name="options">
        /// options from the CLI interface
        /// </param>
        /// <param name="appName">
        /// The app Name.
        /// </param>
        /// <param name="appVersion">
        /// The app Version.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RunCommand(Options options, string appName, string appVersion, string version)
        {
            var client = new EA_Server() { Url = options.EndPoint, };
            var header = new MultiSpeakMsgHeader()
            {
                UserID = options.UserId,
                Pwd = options.Pwd,
                AppName = appName,
                AppVersion = appVersion,
                Company = options.Company,
                Version = version,
                MessageID = new Guid().ToString()
            };
            client.MultiSpeakMsgHeaderValue = header;

            ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, errors) => true;

            // TODO: Switch Statement
            return "Success";
        }
    }
}
