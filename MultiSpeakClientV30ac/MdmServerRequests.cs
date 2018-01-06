// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MdmServerRequests.cs" company="Craig">
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

    using MultiSpeakClientV30ac.proxyMDM3ac;
    using MultiSpeakClientV30ac.proxyMDM416;

    using action = MultiSpeakClientV30ac.proxyMDM3ac.action;
    using CDState = MultiSpeakClientV30ac.proxyMDM3ac.CDState;
    using connectDisconnectEvent = MultiSpeakClientV30ac.proxyMDM3ac.connectDisconnectEvent;
    using errorObject = MultiSpeakClientV30ac.proxyMDM3ac.errorObject;
    using extensionsItem = MultiSpeakClientV30ac.proxyMDM3ac.extensionsItem;
    using extensionsItemExtType = MultiSpeakClientV30ac.proxyMDM3ac.extensionsItemExtType;
    using loadActionCode = MultiSpeakClientV30ac.proxyMDM3ac.loadActionCode;
    using MDM_Server = MultiSpeakClientV30ac.proxyMDM3ac.MDM_Server;
    using MultiSpeakMsgHeader = MultiSpeakClientV30ac.proxyMDM3ac.MultiSpeakMsgHeader;
    using readingValue = MultiSpeakClientV30ac.proxyMDM3ac.readingValue;
    using serviceType = MultiSpeakClientV30ac.proxyMDM3ac.serviceType;

    /// <summary>
    /// The MDM server requests.
    /// </summary>
    public static class MdmServerRequests
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
        public static void RunCommand(
            Options options,
            string appName,
            string appVersion,
            string version,
            out string message)
        {
            try
            {
                message = string.Empty;
                var client = new MDM_Server() { Url = options.EndPoint, };
                var header = new MultiSpeakMsgHeader()
                                 {
                                     UserID = options.UserId,
                                     Pwd = options.Pwd,
                                     AppName = appName,
                                     AppVersion = appVersion,
                                     Company = options.Company,
                                     Version = version,
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
                    case "GetCDMeterState":
                        message = GetCdMeterState(client, options);
                        break;
                    case "GetCDSupportedMeters":
                        message = GetCdSupportedMeters(client);
                        break;
                    case "InitiateConnectDisconnect":
                        message = InitiateConnectDisconnect(client, options);
                        break;
                    case "InitiateCDStateRequest":
                        message = InitiateCdStateRequest(client, options);
                        break;
                    case "InitiateOutageDetectionEventRequest":
                        message = InitiateOutageDetectionEventRequest(client, options);
                        break;
                    case "PingUrl":
                        message = PingUrl(client);
                        break;
                    case "ReadingChangedNotification":
                        message = ReadingChangedNotification(client, options);
                        break;
                    case "ReadingChangedNotificationAmpTest":
                        message = ReadingChangedNotificationAmpTest(client2, options);
                        break;
                    case "OutageEventChangedNotification":
                        message = OutageEventChangedNotification(client2, options);
                        break;
                    case "ODEventNotification":
                        message = ODEventNotification(client2, options);
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

        private static string ODEventNotification(proxyMDM416.MDM_Server client2, Options options)
        {
            try
            {
                if (options == null)
                {
                    Console.WriteLine("SendOdEventNotification requires options.Device : -d 123456789");
                    return Fail;
                }

                if (options.Device == null)
                {
                    Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                    return Fail;
                }

                const string eventTypeError =
                    "EventType is missing. Please review the required options for types of events.";
                if (options.EventType == null)
                {
                    Console.WriteLine(eventTypeError);
                    return Fail;
                }

                proxyMDM416.outageEventType outEventType;
                switch (options.EventType)
                {
                    case "Inferred":
                        outEventType = proxyMDM416.outageEventType.Inferred;
                        break;
                    case "Instantaneous":
                        outEventType = proxyMDM416.outageEventType.Instantaneous;
                        break;
                    case "Outage":
                        outEventType = proxyMDM416.outageEventType.Outage;
                        break;
                    case "NoResponse":
                        outEventType = proxyMDM416.outageEventType.NoResponse;
                        break;
                    case "PowerOff":
                        outEventType = proxyMDM416.outageEventType.PowerOff;
                        break;
                    case "PowerOn":
                        outEventType = proxyMDM416.outageEventType.PowerOn;
                        break;
                    case "Restoration":
                        outEventType = proxyMDM416.outageEventType.Restoration;
                        break;
                    default:
                        outEventType = proxyMDM416.outageEventType.Outage;
                        Console.WriteLine(
                            $"{options.EventType} not found. Defaulting to Outage. Did you mean Inferred,Instantaneous,Outage,NoResponse,PowerOff,PowerOn,Restoration?");
                        break;
                }

                var transactionId = Guid.NewGuid().ToString();
                transactionId = DateTime.Now.ToString("u");
                var outages = new[]
                                  {
                                      new proxyMDM416.outageDetectionEvent
                                          {
                                              eventTime = DateTime.Now.AddHours(-3),
                                              eventTimeSpecified = true,
                                              outageEventType = outEventType,
                                              outageEventTypeSpecified = true,
                                              //outageDetectDeviceID = options.Device,
                                              //outageDetectDeviceTypeSpecified = true,
                                              //outageDetectDeviceType =
                                              //    proxyMDM416.outageDetectDeviceType.Meter,
                                              //outageLocation =
                                              //    new proxyMDM416.outageLocation
                                              //        {
                                              //            meterNo = options.Device,
                                              //        },
                                              messageList =
                                                  new[]
                                                      {
                                                          new proxyMDM416.message
                                                              {
                                                                  comments = "test"
                                                              }
                                                      }
                                          }
                                  };
                var response = client2.ODEventNotification(outages, transactionId);
                PrintClassStdOut.PrintObject(client2.MultiSpeakMsgHeaderValue);

                // We might not get a response, if so exit
                if (response == null)
                {
                    return Successfull;
                }

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                PrintClassStdOut.ErrorObjects(response.ToArray<object>());
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

                XmlUtil.WriteToFile(
                    xml,
                    $"ODEventNotification.{options.Device}.{options.EventType}",
                    "3AC",
                    logFileDirectory);
                return xml;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
    

        /// <summary>
        /// Get the Connect or Disconnect State from the AMI database, this is not the current state of the meter in the 'real' world
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// Expects options.Device
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetCdMeterState(MDM_Server client, Options options)
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

            var response = client.GetCDMeterState(options.Device);

            // Serializer requires the specific class type and i have yet figured
            // out a way to do this with reflection.
            var serializer = new XmlSerializer(typeof(loadActionCode));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetCDMeterState.{options.Device}", "3AC", logFileDirectory);
            return Successfull;
        }

        /// <summary>
        /// Get CD Supported Meters
        /// </summary>
        /// <param name="client">
        /// client we need
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetCdSupportedMeters(MDM_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetCDSupportedMeters(lastReceived);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetCDSupportedMeters objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine(item.meterNo);
                }

                // Serializer requires the specific class type and i have yet figured
                // out a way to do this with reflection.
                var serializer = new XmlSerializer(typeof(meter[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                XmlUtil.WriteToFile(xml, $"GetCDSupportedMeters.{objectsRemaining}", "3AC", logFileDirectory);
            }

            return Successfull;
        }

        /// <summary>
        /// The initiate cd state request.
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
        private static string InitiateCdStateRequest(MDM_Server client, Options options)
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

            if (options.ResponseUrl == null)
            {
                Console.WriteLine("ResponseUrl is missing. Please add a ResponseUrl - example: -r http://yourserver/NOT_Server.asmx");
                return Fail;
            }

            var cdstates = new[]
                               {
                                   new CDState
                                       {
                                           meterNo = options.Device,
                                           meterID = options.Device,
                                           comments = "Craig's Test for EPHRPA-71437667",
                                           serviceType = serviceType.Electric,
                                           serviceTypeSpecified = true
                                       }
                               };
            var requestDate = DateTime.Now;
            var responseUrl = options.ResponseUrl;
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)requestDate.AddMinutes(30).ToOADate();

            // TimeStamp is unix timestamp, so we use ToOADate to get a double and cast to float. 
            var response = client.InitiateCDStateRequest(cdstates, responseUrl, transactionId, expirationTime);
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

            XmlUtil.WriteToFile(xml, $"InitiateCDStateRequest.{options.Device}.ERROR", "3AC", logFileDirectory);
            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            return xml;
        }

        /// <summary>
        /// Initiate Connect Disconnect for a meter
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// expects options.Device
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string InitiateConnectDisconnect(MDM_Server client, Options options)
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

            var connectDisconnectEvents = new[]
                               {
                                   new connectDisconnectEvent
                                       {
                                         meterNo  = options.Device,
                                         loadActionCodeSpecified = true,
                                         loadActionCode = loadActionCode.Disconnect,
                                         meterID = options.Device,
                                         objectID = options.Device,
                                         comments = "testing for electsolve"
                                       }
                               };
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)DateTime.Now.AddHours(1).ToOADate();
            var response = client.InitiateConnectDisconnect(connectDisconnectEvents, options.ResponseUrl, transactionId, expirationTime);
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
            XmlUtil.WriteToFile(xml, $"InitiateConnectDisconnect.{options.Device}.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// Send Initiate Outage Detection Event Request
        /// </summary>
        /// <param name="client">
        /// Expects MDM_Server, or MR_Server clients.
        /// </param>
        /// <param name="options">
        /// Requires Device and ResponseUrl
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string InitiateOutageDetectionEventRequest(MDM_Server client, Options options = null)
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

            if (options.ResponseUrl == null)
            {
                Console.WriteLine("ResponseUrl is missing. Please add a ResponseUrl : -r http://yourserver/NOT_Server.asmx");
                return Fail;
            }

            var meters = new[] { options.Device };
            var requestDate = DateTime.Now;

            var responseUrl = options.ResponseUrl;
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime =
                (float)requestDate.AddMinutes(30)
                    .ToOADate(); // TimeStamp is unix timestamp, so we use ToOADate to get a double and cast to float. 
            var response = client.InitiateOutageDetectionEventRequest(
                meters,
                requestDate,
                responseUrl,
                transactionId,
                expirationTime);

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
            XmlUtil.WriteToFile(xml, $"InitiateOutageDetectionEventRequest.{options.Device}.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// The ping url.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string PingUrl(MDM_Server client)
        {
            var response = client.PingURL();
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
            XmlUtil.WriteToFile(xml, $"PingURL.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// Send Reading Changed Notification
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// out options
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ReadingChangedNotification(MDM_Server client, Options options)
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

            // Populate the entire structure out so we can test the entire meterReads object[]
            var meterReads = new[]
                                 {
                                         new meterRead
                                             {
                                                 meterNo = options.Device,
                                                 comments = "MultiSpeakClient Test Sent ReadingChangedNotification",
                                                 deviceID = options.Device,
                                                 errorString = "No errors reported.",
                                                 AnyAttr = null,
                                                 extensions = null,
                                                 extensionsList = new[]
                                                                      {
                                                                          new extensionsItem
                                                                          {
                                                                              extName = "AMISystem",
                                                                              extTypeSpecified = true,
                                                                              extType = extensionsItemExtType.Name,
                                                                              extValue = "Sensus"
                                                                          },
                                                                          new extensionsItem
                                                                          {
                                                                              extName = "Routed From",
                                                                              extTypeSpecified = true,
                                                                              extType = extensionsItemExtType.Name,
                                                                              extValue = "MultiSpeakBroker"
                                                                          },
                                                                          new extensionsItem
                                                                          {
                                                                               extName = "Number of Retries",
                                                                               extTypeSpecified = true,
                                                                               extType = extensionsItemExtType.@int,
                                                                               extValue = "1"
                                                                          },
                                                                      },
                                                 kVAr = 1f,
                                                 kVArSpecified = true,
                                                 kW = 10f,
                                                 kWSpecified = true,
                                                 kWDateTime = DateTime.Now.AddDays(-1),
                                                 kWDateTimeSpecified = true,
                                                 negKWh = "-1",
                                                 posKWh = "1",
                                                 objectID = options.Device,
                                                 readingDate = DateTime.Now,
                                                 readingDateSpecified = true,
                                                 readingValues = new[]
                                                                     {
                                                                         new readingValue
                                                                             {
                                                                                 dateTime = DateTime.Now,
                                                                                 dateTimeSpecified = true,
                                                                                 extensionsList = new[]
                                                                                              {
                                                                                                  new extensionsItem
                                                                                                  {
                                                                                                          extName = "AMISystem",
                                                                                                          extTypeSpecified = true,
                                                                                                          extType = extensionsItemExtType.Name,
                                                                                                          extValue = "Sensus"
                                                                                                  },
                                                                                                  new extensionsItem
                                                                                                  {
                                                                                                      extName = "AMIUom",
                                                                                                      extTypeSpecified = true,
                                                                                                      extType = extensionsItemExtType.Name,
                                                                                                      extValue = "kWh-R"
                                                                                                  },
                                                                                                  new extensionsItem
                                                                                                  {
                                                                                                      extName = "AMIDescription",
                                                                                                      extTypeSpecified = true,
                                                                                                      extType = extensionsItemExtType.Name,
                                                                                                      extValue = "Kilowatt Reverse Flow Channel 1"
                                                                                                  }
                                                                                              },
                                                                                 extensions = null,
                                                                                 value = "100",
                                                                                 fieldName = "Check the extentionList",
                                                                                 name = "KiloWatt Hours",
                                                                                 readingType = "On Demand Read",
                                                                                 readingValueType = readingValueReadingValueType.Energy,
                                                                                 readingValueTypeSpecified = true,
                                                                                 units = "kWh"
                                                                             }
                                                                     },
                                                 verb = action.New,
                                                 TOUReadings = new[] { new TOUReading { } },
                                                 momentaryEvents = "No momentary events to report at this time.",
                                                 momentaryOutages = "No mommentary outages to report at this time.",
                                                 phase = phaseCd.A,
                                                 phaseSpecified = true,
                                                 replaceID = "We are not using replace id at this time.",
                                                 sustainedOutages = "No sustained outages reported",
                                                 utility = "My Power Company"
                                             }
                                     };
            var transactionId = options.TransactionId;
            var response = client.ReadingChangedNotification(meterReads, transactionId);
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
            XmlUtil.WriteToFile(xml, $"ReadingChangedNotification.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// Send Reading Changed Notification
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// out options
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ReadingChangedNotificationAmpTest(proxyMDM416.MDM_Server client, Options options)
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

            // Populate the entire structure out so we can test the entire meterReads object[]
            var meterReads = new[]
                                 {
                                         new proxyMDM416.meterReading
                                             {                                             
                                                 comments = "MultiSpeakClient Test Sent ReadingChangedNotification",
                                                 errorString = "No errors reported.",
                                             }
                                     };
            var transactionId = options.TransactionId;
            var response = client.ReadingChangedNotification(meterReads, transactionId);
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
            XmlUtil.WriteToFile(xml, $"ReadingChangedNotification.ERROR", "3AC", logFileDirectory);
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
        private static string OutageEventChangedNotification(proxyMDM416.MDM_Server client, Options options)
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

            // Data for GMLocation
            var points = new proxyMDM416.PointType();
            var coords = new proxyMDM416.CoordType() { X = 2191236.56044671M, Y = 13757840.7320126M, YSpecified = true };
            points.Item = coords;

            // Example
            // <outageReasonCodeList>
            //    <extensions>
            //        <q1:outageReasonContainer 
            //            xmlns:q1="http://www.multispeak.org/Version_4.1_Release">
            //            <q1:outageReasonList>
            //                <q1:outageReasonItem>
            //                    <q1:category>Cause</q1:category>
            //                    <q1:outageReason>
            //                        <q1:description>Animal</q1:description>
            //                        <q1:outageReportingCodeList>
            //                            <q1:outageReportingCode>020</q1:outageReportingCode>
            //                        </q1:outageReportingCodeList>
            //                    </q1:outageReason>
            //                </q1:outageReasonItem>
            //                <q1:outageReasonItem>
            //                    <q1:category>Weather Condition</q1:category>
            //                    <q1:outageReason>
            //                        <q1:description>Extreme Heat</q1:description>
            //                        <q1:outageReportingCodeList>
            //                            <q1:outageReportingCode>030</q1:outageReportingCode>
            //                        </q1:outageReportingCodeList>
            //                    </q1:outageReason>
            //                </q1:outageReasonItem>
            //            </q1:outageReasonList>
            //        </q1:outageReasonContainer>
            //    </extensions>
            // </outageReasonCodeList>
            var outagereasoncontainer = new proxyMDM416.outageReasonContainer
            {
                // TODO: MAYBE - Milsoft has ns on the xml elements
                //  xmlns:q1="http://www.multispeak.org/Version_4.1_Release"
                outageReasonList = new proxyMDM416.outageReasonItem[]
                {
                    new proxyMDM416.outageReasonItem
                    {
                        category = "Weather Condition",
                        outageReason = new proxyMDM416.outageReason()
                        {
                            description = "Extreme Heat",
                            outageReportingCodeList = new proxyMDM416.outageReportingCode[]
                            {
                                new proxyMDM416.outageReportingCode
                                {
                                    Value = "030"
                                }
                            }
                        }
                    },
                    new proxyMDM416.outageReasonItem
                    {
                        category = "Cause",
                        outageReason = new proxyMDM416.outageReason()
                        {
                            description = "Animal",
                            outageReportingCodeList = new proxyMDM416.outageReportingCode[]
                            {
                                new proxyMDM416.outageReportingCode
                                {
                                    Value = "020"
                                }
                            }
                        }
                    }
                },              
            };

            // Convert the the  outagereasoncontainer to XmlElement
            XmlElement[] xmlElements = new XmlElement[1];
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(outagereasoncontainer.GetType()).Serialize(writer, outagereasoncontainer);
            }

            xmlElements[0] = doc.DocumentElement;

            var outageEvents = new[]
                                   {
                                       new proxyMDM416.outageEvent
                                       {
                                           objectID = "2017-10-27-0003",
                                           extensionsList = new[]
                                                                {
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "isClosed",
                                                                            extValue = new extValue { Value = "True" },
                                                                            extType  = extType.boolean,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "substationName",
                                                                            extValue = new extValue { Value = "PARKWAY_T2" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "feederCode",
                                                                            extValue = new extValue { Value = "3" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "phaseCode",
                                                                            extValue = new extValue { Value = "B" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "statusPhaseB",
                                                                            extValue = new extValue { Value = "NormalOrRestored" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                },
                                           objectName = "T61563680002",

                                           GMLLocation = points,
                                           GPSLocation = new proxyMDM416.GPSLocation() { GPSValidity = true, GPSValiditySpecified = true, latitude = 29.576805699162243, longitude = -98.299250294859718 } ,
                                           gridLocation = "61563680",
                                           area = "West",
                                           problemLocation = "T61563680002",
                                           deviceID = new proxyMDM416.objectRef()
                                                          {
                                                              name = "T61563680002",
                                                              noun  = new XmlQualifiedName("transformerBank"),
                                                              objectID = "86101734-89a6-11e6-90e7-1866da2dc956"
                                                          },
                                           deviceType = "Transformer",
                                           outagedPhase = proxyMDM416.phaseCode.B,
                                           substationCode = "60",
                                           feeder = "P301",
                                           outageStatus = proxyMDM416.outageStatus.Restored,
                                           startTime = DateTime.Now.AddHours(-3),
                                           startTimeSpecified = true,
                                           completed = DateTime.Now,
                                           completedSpecified = true,
                                           customersAffected = "6",
                                           priorityCustomersCount = "0",
                                           ODEventCount = "1",
                                           customersRestored = "1",

                                           outageReasonCodeList = new proxyMDM416.outageReasonCodeList()
                                                                      {
                                                                          extensions = new proxyMDM416.extensions()
                                                                          {
                                                                              Any = xmlElements
                                                                          }
                                                                      },
                                       },

                                       new proxyMDM416.outageEvent
                                       {
                                           objectID = "2017-10-27-0002",
                                           extensionsList = new[]
                                                                {
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "substationName",
                                                                            extValue = new extValue { Value = "PARKWAY_T2" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "feederCode",
                                                                            extValue = new extValue { Value = "3" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "phaseCode",
                                                                            extValue = new extValue { Value = "B" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "statusPhaseB",
                                                                            extValue = new extValue { Value = "NormalOrRestored" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                },
                                           objectName = "T61563680002",

                                           GMLLocation = points,
                                           GPSLocation = new proxyMDM416.GPSLocation() { GPSValidity = true, GPSValiditySpecified = true, latitude = 29.576805699162243, longitude = -98.299250294859718 } ,
                                           gridLocation = "61563680",
                                           area = "West",
                                           problemLocation = "T61563680002",
                                           deviceID = new proxyMDM416.objectRef()
                                                          {
                                                              name = "T61563680002",
                                                              noun  = new XmlQualifiedName("transformerBank"),
                                                              objectID = "86101734-89a6-11e6-90e7-1866da2dc956"
                                                          },
                                           deviceType = "Transformer",
                                           outagedPhase = proxyMDM416.phaseCode.B,
                                           substationCode = "60",
                                           feeder = "P301",
                                           outageStatus = proxyMDM416.outageStatus.Restored,
                                           startTime = DateTime.Now.AddHours(-3),
                                           startTimeSpecified = true,
                                           completed = DateTime.Now,
                                           completedSpecified = true,
                                           customersAffected = "6",
                                           priorityCustomersCount = "0",
                                           ODEventCount = "1",
                                           customersRestored = "1",
                                           //outageReasonCodeList = new proxyMDM416.outageReasonCodeList()
                                           //{
                                           //    extensions = new proxyMDM416.extensions()
                                           //    {
                                           //        Any = xmlElements
                                           //    }
                                           //},
                                       },
                                       new proxyMDM416.outageEvent
                                       {
                                           objectID = "2017-10-27-0002",
                                           verb = proxyMDM416.action.Delete,
                                           comments = @"1 associated calls deleted
                                                          -----------------------------------------
                                                      Oct 27 2017  1:46PM
                                           OutageRecID: 2017 - 10 - 27 - 0002 discarded from DisSpatch | Outage Events by Dispatcher: LUIS2016\Administrator </ comments >
                                           ",
                                           extensionsList = new[]
                                                                {
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "substationName",
                                                                            extValue = new extValue { Value = "PARKWAY_T2" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "feederCode",
                                                                            extValue = new extValue { Value = "3" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "phaseCode",
                                                                            extValue = new extValue { Value = "B" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                    new proxyMDM416.extensionsItem
                                                                        {
                                                                            extName = "statusPhaseB",
                                                                            extValue = new extValue { Value = "NormalOrRestored" },
                                                                            extType  = extType.@string,
                                                                            extTypeSpecified = true
                                                                        },
                                                                },
                                           objectName = "T61563680002",
                                           GMLLocation = points,
                                           GPSLocation = new proxyMDM416.GPSLocation() { GPSValidity = true, GPSValiditySpecified = true, latitude = 29.576805699162243, longitude = -98.299250294859718 } ,
                                           gridLocation = "61563680",
                                           area = "West",
                                           problemLocation = "T61563680002",
                                           deviceID = new proxyMDM416.objectRef()
                                                          {
                                                              name = "T61563680002",
                                                              noun  = new XmlQualifiedName("transformer"),
                                                              objectID = "86101734-89a6-11e6-90e7-1866da2dc956"
                                                          },
                                           deviceType = "Transformer",
                                           outagedPhase = proxyMDM416.phaseCode.B,
                                           substationCode = "60",
                                           feeder = "P301",
                                           outageStatus = proxyMDM416.outageStatus.Restored,
                                           startTime = DateTime.Now.AddHours(-3),
                                           startTimeSpecified = true,
                                           completed = DateTime.Now,
                                           completedSpecified = true,
                                           customersAffected = "6",
                                           priorityCustomersCount = "0",
                                           ODEventCount = "1",
                                           customersRestored = "1",
                                           outageReasonCodeList = new proxyMDM416.outageReasonCodeList()
                                           {
                                               extensions = new proxyMDM416.extensions()
                                               {
                                                   Any = xmlElements
                                               }
                                           },
                                       }
                                   };
            proxyMDM416.errorObject[] response = null;
            for (var i = 0; i < 100000; i++)
            {
                // reset a value for error checking the loaded data 
                outageEvents[0].ODEventCount = i.ToString();
                outageEvents[1].ODEventCount = i.ToString();
                outageEvents[2].ODEventCount = i.ToString();
                response = client.OutageEventChangedNotification(outageEvents);

            }

            if (response == null)
            {
                return Successfull;
            }

            var serializer = new XmlSerializer(typeof(proxyMDM416.errorObject[]));
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
            if (response.Length > 0)
            {
                XmlUtil.WriteToFile(xml, $"ReadingChangedNotification.ERROR", "3AC", logFileDirectory);
            }

            return xml;
        }
    }
}