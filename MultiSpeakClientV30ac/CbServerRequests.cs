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
    using System.Runtime;

    using MultiSpeakClientV30ac.proxyCB3ac;

    /// <summary>
    /// The CB server request
    /// </summary>
    public static class CbServerRequests
    {
        /// <summary>
        /// We Failed
        /// This is a test to see how all this works passing the message back to the caller.
        /// </summary>
        private const string Fail = "FAIL";

        /// <summary>
        /// We passed
        /// </summary>
        private const string Successfull = "SUCCESS";

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
        /// <param name="message">
        /// The message.
        /// </param>
        public static void RunCommand(Options options, string appName, string appVersion, string version, out string message)
        {
            // Wrap this in try catch instead of each individual method/
            // Note this could get confusing...  Just trying it for now to keep the code simple.
            try
            {
                message = string.Empty;
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
                    case "CDStateChangeNotification":
                        message = CdStateChangedNotification(client, options);
                        break;
                    case "GetCustomerByMeterNo":
                        message = GetCustomerByMeterNo(client, options);
                        break;
                    default:
                        Console.WriteLine("Check the list of methods in the README.md for each Server.");
                        Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                        break;
                }

                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    message = ex.InnerException.Message;
                }
            }
        }

        private static string GetCustomerByMeterNo(CB_Server client, Options options)
        {
            if (options == null)
            {
                return Fail;
            }

            if (options.Device == null)
            {
                Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                return Fail;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Connect Disconnect Changed Notification
        /// Typically sent after a connect or disconnect command to turn on or off a meter.
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// the options, options.Device is required
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CdStateChangedNotification(CB_Server client, Options options)
        {
            if (options == null)
            {
                return Fail;
            }

            if (options.Device == null)
            {
                Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                return Fail;
            }

            if (options.TransactionId == null)
            {
                Console.WriteLine("TransactionId is missing. Please add a TransactionId: -t 0525376f-291f-423b-93ec-1f92b4535468");
                return Fail;
            }

            var meterNo = options.Device;

            // Set all StateChange all to Disconnected to this test stub.
            // TODO: Add ability to send in loadActionCodes
            const loadActionCode StateChange = loadActionCode.Open;
            var transactionId = options.TransactionId;
            var errorString = string.Empty;
            client.CDStateChangedNotification(meterNo, StateChange, transactionId, errorString);

            return Successfull;
        }
    }
}
