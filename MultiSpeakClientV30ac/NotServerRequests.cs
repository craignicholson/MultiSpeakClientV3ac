// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotServerRequests.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   Defines the MdmServerRequests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;

    using MultiSpeakClientV30ac.proxyNOT416;

    /// <summary>
    /// The not server requests.
    /// </summary>
    public static class NotServerRequests
    {
        /// <summary>
        /// The status of fail when parameters are missing.
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
        /// Message can be error or generic message for fail or success.
        /// </param>
        public static void RunCommand(Options options, string appName, string appVersion, string version, out string message)
        {
            try
            {
                message = string.Empty;
                var client = new NOT_Server() { Url = options.EndPoint, };
                var header = new MultiSpeakMsgHeader()
                {
                    UserID = options.UserId,
                    Pwd = options.Pwd,
                    AppName = appName,
                    AppVersion = appVersion,
                    Company = options.Company,
                    MessageID = new Guid().ToString(),
                    TimeStamp = DateTime.Now,
                    TimeStampSpecified = true
                };
                client.MultiSpeakMsgHeaderValue = header;

                var client2 = new proxyMDM416.MDM_Server() { Url = options.EndPoint, };
                var header2 = new proxyMDM416.MultiSpeakMsgHeader()
                {
                    UserID = options.UserId,
                    Pwd = options.Pwd,
                    AppName = appName,
                    AppVersion = appVersion,
                    Company = options.Company,
                    MessageID = new Guid().ToString(),
                    TimeStamp = DateTime.Now,
                    TimeStampSpecified = true
                };
                client2.MultiSpeakMsgHeaderValue = header2;

                ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, errors) => true;

                // Goals keep the Methods in alpha order.
                switch (options.Method)
                {
                    case "OutageEventChangedNotification":
                        message = OutageEventChangedNotification(client, options);
                        break;
                    case "TransformerBankChangedNotification":
                        message = TransformerBankChangedNotification(client);
                        break;
                    case "ServiceLocationNetworkChangedNotification":
                        message = ServiceLocationNetworkChangedNotification(client);
                        break;
                    default:
                        Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                        Console.WriteLine("Check the list of methods in the README.md for each Server.");
                        break;
                }

                // PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
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

        /// <summary>
        /// The service location network changed notification.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ServiceLocationNetworkChangedNotification(NOT_Server client)
        {
            var changedServiceLocations = new[]
                                              {
                                                new serviceLocation
                                                    {
                                                        annotationList = null,
                                                        AnyAttr = null,
                                                        comments = null,
                                                        contactInfo = null,
                                                        customerID = null,
                                                        description = null,
                                                        electricServiceList = null,
                                                        errorString = null,
                                                        extensionsList = null,
                                                        extensions = null,
                                                    }   
                                              };

            var response = client.ServiceLocationNetworkChangedNotification(changedServiceLocations);

            if (response == null)
            {
                return Successfull;
            }

            var serializer = new XmlSerializer(typeof(errorObject[]));
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            XmlUtil.WriteToFile(xml, $"ServiceLocationNetworkChangedNotification.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// The transformer bank changed notification.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string TransformerBankChangedNotification(NOT_Server client)
        {
            var changedTransformerBanks = new[]
                                              {
                                                new transformerBank
                                                    {
                                                        objectID =  null,
                                                        annotationList = null,
                                                        AnyAttr = null,
                                                        bankRatings = null,
                                                        centerTapPhase = mspPhase.A,
                                                        centerTapPhaseSpecified = true,
                                                        comments = null,
                                                        description = null,
                                                        electricLocationFields = null,
                                                        equipmentContainerID = null,
                                                    }
                                              };

            var response = client.TransformerBankChangedNotification(changedTransformerBanks);

            if (response == null)
            {
                return Successfull;
            }

            var serializer = new XmlSerializer(typeof(errorObject[]));
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            XmlUtil.WriteToFile(xml, $"TransformerBankChangedNotification.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// The outage event changed notification.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string OutageEventChangedNotification(NOT_Server client, Options options)
        {
            var outageEvents = new[]
                                              {
                                                  new outageEvent
                                                      {
                                                         message = null,
                                                         extensionsList = null,
                                                         AnyAttr = null,
                                                         comments = null,
                                                         extensions = null,
                                                         errorString = null,
                                                         objectID = null,
                                                         verb = action.New,
                                                         deviceID = null,
                                                         facilityID = null,
                                                         gridLocation = null,
                                                         replaceID = null,
                                                         ETOR = DateTime.Now,
                                                         ETORSpecified = true,
                                                         GMLLocation = null,
                                                         GPSLocation = null,
                                                         ODEventCount = null,
                                                         actualFault = null,
                                                         area = null,
                                                         completed = DateTime.Now,
                                                         completedSpecified = true,
                                                         crewActionEvents = null,
                                                         crewsDispatched = null,
                                                         customersAffected = null,
                                                         customersRestored = null,
                                                         deviceType = null,
                                                         faultType = null,
                                                         feeder = null,
                                                         startTime = DateTime.Now.AddHours(-2),
                                                         startTimeSpecified = true,
                                                      }
                                              };

            var response = client.OutageEventChangedNotification(outageEvents);

            if (response == null)
            {
                return Successfull;
            }

            var serializer = new XmlSerializer(typeof(errorObject[]));
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            XmlUtil.WriteToFile(xml, $"OutageEventChangedNotification.ERROR", "3AC", logFileDirectory);
            return xml;
        }
    }

}
