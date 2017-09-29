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
        public static void RunCommand(Options options, string appName, string appVersion, string version, out string message)
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
                MessageID = new Guid().ToString()
            };
            client.MultiSpeakMsgHeaderValue = header;

            ServicePointManager.ServerCertificateValidationCallback =
                (obj, certificate, chain, errors) => true;

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
                default:
                    Console.WriteLine(
                        $"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                    break;
            }

            // TODO: I wonder if I can just output the Print for MultiSpeakHeader here for the client response.
        }

        /// <summary>
        /// Get the Connect or Disconnect State from the AMI database, this is not the current state of the meter in the 'real' world
        /// </summary>
        /// <param name="client">the client</param>
        /// <param name="options">Expects options.Device</param>
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
            PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
            PrintClassStdOut.PrintObject(response);
            return Successfull;
        }

        /// <summary>
        /// Get CD Supported Meters
        /// </summary>
        /// <param name="client">client we need</param>
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
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
            }

            // TODO:
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
        private static string InitiateCdStateRequest(MDM_Server client, Options options)
        {
            var cdstates = new[]
            {
                new CDState
                {
                    meterNo = "EPHRPA-71437667",
                    meterID = "EPHRPA-71437667",
                    comments = "Craig's Test for EPHRPA-71437667",
                    serviceType = serviceType.Electric,
                    serviceTypeSpecified = true
                }
            };

            try
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
                    Console.WriteLine(
                        "ResponseUrl is missing. Please add a ResponseUrl : -r http://yourserver/NOT_Server.asmx");
                    return Fail;
                }

                var requestDate = DateTime.Now;
                var responseUrl = options.ResponseUrl;
                var transactionId = Guid.NewGuid().ToString();
                var expirationTime =
                    (float)requestDate.AddMinutes(30)
                        .ToOADate(); // TimeStamp is unix timestamp, so we use ToOADate to get a double and cast to float. 
                var response =
                    client.InitiateCDStateRequest(cdstates, responseUrl, transactionId, expirationTime);
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                if (response == null)
                {
                    return Successfull;
                }

                PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                return Fail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Fail;
            }
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
                                         meterNo  = options.Device
                                       }  
                               };
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)DateTime.Now.AddHours(1).ToOADate();
            var response = client.InitiateConnectDisconnect(connectDisconnectEvents, options.ResponseUrl, transactionId, expirationTime);

            PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

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

            XmlUtil.WriteToFile(xml, $"InitiateConnectDisconnect.{options.Device}", "3AC", logFileDirectory);
            return xml;
        }


        /// <summary>
        /// Send Initiate Outage Detection Event Request
        /// </summary>
        /// <param name="client">Expects MDM_Server, or MR_Server clients.</param>
        /// <param name="options">Requires Device and ResponseUrl</param>
        private static string InitiateOutageDetectionEventRequest(MDM_Server client, Options options = null)
        {
            try
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

                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                if (response == null)
                {
                    return Successfull;
                }

                PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                return Fail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Fail;
            }
        }

        /// <summary>
        /// The ping url.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static string PingUrl(MDM_Server client)
        {
            var response = client.PingURL();

            // This MessageID will be the MessageID from the server and not Message from the Client
            // since the response will contain the out soap header.
            // This shows how one can use the Header to pass information from the client to the server
            // and from the server to the client.  The Chunking Examples for GetAllConnectivity is
            // the best use case I have for how the headers are used.
            PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

            // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
            // so instead of passing response we paass repsonse.ToArray<object>()
            if (response == null)
            {
                return Successfull;
            }

            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            return response[0].errorString;
        }

        /// <summary>
        /// Send Reading Changed Notification
        /// </summary>
        /// <param name="client">the client</param>
        /// <param name="options">out options</param>
        private static string ReadingChangedNotification(MDM_Server client, Options options)
        {
            try
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

                var meterReads = new[]
                                     {
                                         new meterRead
                                             {
                                                 meterNo = options.Device,
                                                 comments = string.Empty,
                                                 deviceID = string.Empty,
                                                 errorString = string.Empty,
                                                 AnyAttr = null,
                                                 extensions = null,
                                                 extensionsList = null,
                                                 kVAr = 1f,
                                                 kVArSpecified = true,
                                                 kW = 10f,
                                             }
                                     };
                var transactionId = Guid.NewGuid().ToString();
                var response = client.ReadingChangedNotification(meterReads, transactionId);
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
                if (response == null)
                {
                    return Successfull;
                }

                PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                return response[0].errorString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Fail;
            }
        }
    }
}