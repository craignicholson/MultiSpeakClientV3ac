// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CbServerRequests.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   The CB server request
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.Net;

    using MultiSpeakClientV30ac.proxyCB3ac;

    /// <summary>
    /// The CB server request
    /// </summary>
    public static class CbServerRequests
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
            var client = new CB_Server() { Url = options.EndPoint, };
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

            switch (options.Method)
            {
                case "CDStateChangedNotification":
                    CdStateChangedNotification(client, options);
                    break;
                default:
                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                    Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                    break;
            }

            return "Success";
        }

        /// <summary>
        /// Connect Disconnect Changed Notification
        /// Typically sent after a connect or disconnect command to turn on or off a meter.
        /// </summary>
        /// <param name="client">the client</param>
        /// <param name="options">the options, options.Device is required</param>
        private static void CdStateChangedNotification(CB_Server client, Options options)
        {
            if (options == null)
            {
                return;
            }

            if (options.Device == null)
            {
                Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                return;
            }

            var meterNo = options.Device;

            // Set them all to Disconnected
            const loadActionCode StateChange = loadActionCode.Open;
            var transactionId = Guid.NewGuid().ToString();
            var errorString = string.Empty;
            client.CDStateChangedNotification(meterNo, StateChange, transactionId, errorString);
        }
    }
}
