// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Craig Nicholson">
//   MIT Lic
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using CommandLine;

    using MultiSpeakClientV30ac.proxyMDM3ac;
    using MultiSpeakClientV30ac.proxyMR3ac;
    using MultiSpeakClientV30ac.proxyOA3ac;

    using serviceType = proxyMDM3ac.serviceType;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The file directory where the output files will be written
        /// TODO: FIX THIS - MAKE IT CONFIGURATION
        /// </summary>
        private const string FileDirectory = @"C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug\LG";

        /// <summary>
        /// The MultiSpeak Message Header .Version, Version of MultiSpeak
        /// </summary>
        private const string Version = "3.0AC";

        /// <summary>
        /// The app name for MultiSpeak Message Header AppName
        /// </summary>
        private const string AppName = "MultiSpeakClient30ac";

        /// <summary>
        /// The app version for MultiSpeak Message Header AppVersion
        /// </summary>
        private const string AppVersion = "1.0beta";

        /// <summary>
        /// Main Entry Point into Application
        /// </summary>
        /// <param name="args">Arguments sent in to call the MultiSpeak Methods</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello MultiSpeak 3AC Client - Grab a cup of Joe and let's go!");
            Console.WriteLine(Art());
            Console.WriteLine("\n");
            var options = new Options();
            var isValid = Parser.Default.ParseArgumentsStrict(args, options);
            if (!isValid)
            {
                return;
            }

            var wallTime = new System.Diagnostics.Stopwatch();  // Get the wall time for sampling stats
            wallTime.Start();
            ProcessArgs(options);
            Console.WriteLine("TASK FINISHED");
            wallTime.Stop();
            Console.WriteLine($"Execution Time : {wallTime.Elapsed}");
        }

        /// <summary>
        /// Art needs to be read from a file to make this easier.  And swap ASCII Art.  :-)
        /// </summary>
        /// <returns>coffee as a string</returns>
        private static string Art()
        {
            const string Coffee = @"
                            \ | ( | ) / /
                          _________________
                          |               |
                          |               |
                          |               /--\
                          |               |  |
                           \             /\--/
                            \___________ /";
            return Coffee;
        }

        /// <summary>
        /// ProcessArgs processes the arguments and executes the correct method.
        /// </summary>
        /// <param name="options">Options are the the CLI options, you can run the program without options to view the help.</param>
        private static void ProcessArgs(Options options)
        {
            switch (options.Server)
            {
                case "OA_Server":
                    {
                        var client = new OA_Server
                        {
                            Url = options.EndPoint,
                        };
                        var header = new proxyOA3ac.MultiSpeakMsgHeader
                        {
                            UserID = options.UserId,
                            Pwd = options.Pwd,
                            AppName = AppName,
                            AppVersion = AppVersion,
                            Company = options.Company,
                            Version = Version
                        };
                        client.MultiSpeakMsgHeaderValue = header;

                        // self-signed cert override
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                        {
                            return true;
                        };

                        switch (options.Method)
                        {
                            case "GetActiveOutages":
                                GetActiveOutages(client);
                                break;
                            case "GetAllActiveOutageEvents":
                                GetAllActiveOutageEvents(client);
                                break;
                            case "GetAllConnectivity":
                                GetAllConnectivity(client);
                                break;
                            case "GetAllCircuitElements":
                                GetAllCircuitElements(client);
                                break;
                            case "GetOutageDurationEvents":
                                GetOutageDurationEvents(client, options);
                                break;
                            case "GetOutageEventStatus":
                                GetOutageEventStatus(client);
                                break;
                            case "GetOutageStatusByLocation":
                                GetOutageStatusByLocation(client);
                                break;
                            case "GetCustomersAffectedByOutage":
                                GetCustomersAffectedByOutage(client);
                                break;
                            case "GetCustomerOutageHistory":
                                GetCustomerOutageHistory(client, options);
                                break;
                            case "ODEventNotification":
                                SendOdEventNotification(client, options);
                                break;
                            default:
                                Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                break;
                        }

                        break;
                    }

                case "MDM_Server":
                    {
                        var client = new MDM_Server()
                        {
                            Url = options.EndPoint,
                        };
                        var header = new proxyMDM3ac.MultiSpeakMsgHeader()
                        {
                            UserID = options.UserId,
                            Pwd = options.Pwd,
                            AppName = AppName,
                            AppVersion = AppVersion,
                            Company = options.Company,
                            Version = Version
                        };
                        client.MultiSpeakMsgHeaderValue = header;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            (obj, certificate, chain, errors) => true;
                        switch (options.Method)
                        {
                            case "InitiateOutageDetectionEventRequest":
                                SendInitiateOutageDetectionEventRequest(client, options);
                                break;
                            case "PingUrl":
                                PingUrl(client);
                                break;
                            case "InitiateCDStateRequest":
                                InitiateCdStateRequest(client, options);
                                break;
                            default:
                                Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                break;
                        }

                        break;
                    }

                case "MR_Server":
                    {
                        var client = new MR_Server
                        {
                            Url = options.EndPoint,
                        };
                        var header = new proxyMR3ac.MultiSpeakMsgHeader()
                        {
                            UserID = options.UserId,
                            Pwd = options.Pwd,
                            AppName = AppName,
                            AppVersion = AppVersion,
                            Company = options.Company,
                            Version = Version
                        };
                        client.MultiSpeakMsgHeaderValue = header;

                        // self-signed cert override - because people don't want to pay for valid certs. and .Net blocks self-signed
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            (obj, certificate, chain, errors) => true;
                        switch (options.Method)
                        {
                            case "GetAMRSupportedMeters":
                                GetAmrSupportedMeters(client);
                                break;
                            case "GetHistoryLogByMeterNo":
                                GetHistoryLogByMeterNo(client, options);
                                break;
                            case "GetLatestReadingByMeterNo":
                                GetLatestReadingByMeterNo(client, options);
                                break;
                            case "GetLatestReadings":
                                GetLatestReadings(client);
                                break;
                            case "InitiateMeterReadByMeterNumber":
                                InitiateMeterReadByMeterNumber(client, options);
                                break;
                            default:
                                Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                break;
                        }

                        break;
                    }

                default:
                    Console.WriteLine($"{options.Server} not found. Did you mean OA_Server or MDM_Server?");
                    break;
            }
        }

        #region SupportingMethods

        /// <summary>
        /// The write to file.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        private static void WriteToFile(string xml, string method, string version)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $@"{FileDirectory}\{method}.{version}.Response" + $@".{timestamp}.xml";
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

        /// <summary>
        /// Print MultiSpeak Message Header for OA_Server prints out all name values pairs in the soap header object.
        /// Best use case is printing out the header for a response.  LastSent, ObjectsRemaining
        /// are used when 'chunking' data.  Typical use case for synchronous web services, so the client
        /// can request the next set of data.  You can use this method to Print out the client Headers
        /// however the main intention is printing out the Server's Headers since the values are actionable.
        /// </summary>
        /// <param name="multiSpeakMessageHeaderValue">Headers Returned</param> 
        private static void PrintMultiSpeakMsgHeader(object multiSpeakMessageHeaderValue)
        {
            Console.WriteLine("MultiSpeakMsgHeader Server >");
            var t = multiSpeakMessageHeaderValue.GetType();
            foreach (var pi in t.GetProperties())
            {
                var name = pi.Name;
                var value = string.Empty;
                if (pi.GetValue(multiSpeakMessageHeaderValue) != null)
                {
                    value = pi.GetValue(multiSpeakMessageHeaderValue, null).ToString();
                }

                Console.WriteLine($"\t{name} : {value}");
            }
        }

        /// <summary>
        /// The print error objects.
        /// </summary>
        /// <param name="errorObjects">
        /// The error objects.
        /// </param>
        private static void PrintErrorObjects(object[] errorObjects)
        {
            foreach (var error in errorObjects)
            {
                Console.WriteLine("errorObject[] >");
                var t = error.GetType();
                foreach (var pi in t.GetProperties().ToArray())
                {
                    var name = pi.Name;
                    var value = string.Empty;
                    if (pi.GetValue(error) != null)
                    {
                        value = pi.GetValue(error, null).ToString();
                    }

                    Console.WriteLine($"\t{name} : {value}");
                }
            }
        }

        /// <summary>
        /// The ping url.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void PingUrl(MDM_Server client)
        {
            try
            {
                // Setting the MessageID from the client as an example
                client.MultiSpeakMsgHeaderValue.MessageID = "Message from the Client";
                Console.WriteLine("Request from before request is made : client.MultiSpeakMsgHeaderValue ");

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                var response = client.PingURL();

                // This MessageID will be the MessageID from the server and not Message from the Client
                // since the response will contain the out soap header.
                // This shows how one can use the Header to pass information from the client to the server
                // and from the server to the client.  The Chunking Examples for GetAllConnectivity is
                // the best use case I have for how the headers are used.
                Console.WriteLine("Response from Server");
                Console.WriteLine($"MultiSpeakMsgHeaderValue.MessageID : {client.MultiSpeakMsgHeaderValue.MessageID}");
                Console.WriteLine("Response from server : client.MultiSpeakMsgHeaderValue ");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// The get amr supported meters.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetAmrSupportedMeters(MR_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAMRSupportedMeters(lastReceived);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetAMRSupportedMeters objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine(item.meterNo);
                }

                var serializer = new XmlSerializer(typeof(proxyMR3ac.meter[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }

                WriteToFile(xml, $"GetAMRSupportedMeters.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        /// <summary>
        /// The get history log by meter no.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetHistoryLogByMeterNo(MR_Server client, Options options)
        {
            var response = client.GetHistoryLogByMeterNo(options.Device, DateTime.Now.Subtract(TimeSpan.FromDays(1000)), DateTime.Now);
            var serializer = new XmlSerializer(typeof(proxyMR3ac.historyLog[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetHistoryLogByMeterNo.{options.Device}", "3AC");
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
        }

        /// <summary>
        /// The get latest reading by meter no.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetLatestReadingByMeterNo(MR_Server client, Options options)
        {
            var response = client.GetLatestReadingByMeterNo(options.Device);
            var serializer = new XmlSerializer(typeof(proxyMR3ac.meterRead));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetLatestReadingByMeterNo.{options.Device}", "3AC");
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
        }

        /// <summary>
        /// The get latest readings.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetLatestReadings(MR_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetLatestReadings(lastReceived);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetLatestReadings objectsRemaining : {objectsRemaining}");

                var serializer = new XmlSerializer(typeof(proxyMR3ac.meterRead[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                WriteToFile(xml, $"GetLatestReadings.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        /// <summary>
        /// The initiate meter read by meter number.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void InitiateMeterReadByMeterNumber(MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)DateTime.Now.AddHours(1).ToOADate();
            var response = client.InitiateMeterReadByMeterNumber(meterNos, options.ResponseUrl, transactionId, expirationTime);

            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

            // We might not get a response, if so exit
            if (response == null)
            {
                return;
            }

            // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
            // so instead of passing response we paass repsonse.ToArray<object>()
            PrintErrorObjects(response.ToArray<object>());
            var serializer = new XmlSerializer(typeof(proxyMR3ac.errorObject[]));
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"InitiateMeterReadByMeterNumber.{options.Device}", "3AC");
        }

        #endregion

        #region GetCommands

        /// <summary>
        /// The get all connectivity returns a dump of the entire connectivity model.  This method
        /// requires multiple requests using the LastSent and ObjectsRemaining
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetAllConnectivity(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllConnectivity(lastReceived);

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);

                // TODO: Serializing is not working - Does not write out the data expected.
                // TODO: Reading directly from bytes works in another application, we get back all the connectivity data
                var serializer = new XmlSerializer(typeof(MultiSpeak));
                string xml;
                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }

                WriteToFile(xml, $"GetAllConnectivityChunking.{objectsRemaining}", "3AC");
            }
        }

        /// <summary>
        /// The get active outages.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetActiveOutages(OA_Server client)
        {
            var response = client.GetActiveOutages();
            Console.WriteLine("GetActiveOutages");
            Console.WriteLine($"Active Outages : {response.Length}");
            foreach (var item in response)
            {
                Console.WriteLine(item);
            }

            var serializer = new XmlSerializer(typeof(string[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, "GetActiveOutages", "3AC");
        }

        /// <summary>
        /// The get all active outage events.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetAllActiveOutageEvents(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllActiveOutageEvents(lastReceived);

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);

                Console.WriteLine($"GetAllActiveOutageEvents objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine($"deviceid : {item.deviceID}");
                }

                var serializer = new XmlSerializer(typeof(proxyOA3ac.outageEvent[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                WriteToFile(xml, $"GetAllActiveOutageEvents.{objectsRemaining}", "3AC");

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        /// <summary>
        /// The get outage duration events.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetOutageDurationEvents(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }

            var response = client.GetOutageDurationEvents(options.OutageEventId);

            // Help a coder out, print out account and servLoc and append to a file
            foreach (var item in response)
            {
                const string Url = " MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -v Restoration ";
                var data = $"-d {item.meterNo}";
                Console.WriteLine(data);
                using (var file = new StreamWriter("BATCH_ODEventNotifications.txt", true))
                {
                    file.WriteLine($"{Url}{data}");
                }
            }

            var serializer = new XmlSerializer(typeof(proxyOA3ac.outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetOutageDurationEvents.{options.OutageEventId}", "3AC");
        }

        /// <summary>
        /// The get outage event status.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetOutageEventStatus(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }

            var response = client.GetOutageEventStatus(options.OutageEventId);

            Console.WriteLine(response.comments);

            var serializer = new XmlSerializer(typeof(proxyOA3ac.outageEventStatus));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetOutageEventStatus.{options.OutageEventId}", "3AC");
        }

        /// <summary>
        /// The get outage status by location.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetOutageStatusByLocation(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return;
            }

            if (options.Location == null)
            {
                Console.WriteLine("CLI options.Location is missing, please add -l YourLocationID");
                return;
            }

            var location = new proxyOA3ac.outageLocation
            {
                servLoc = options.Location
            };
            var response = client.GetOutageStatusByLocation(location);
            var serializer = new XmlSerializer(typeof(locationStatus));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetOutageStatusByLocation.{options.Location}", "3AC");
        }

        /// <summary>
        /// The get customer outage history.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetCustomerOutageHistory(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return;
            }

            if (options.Account == null)
            {
                Console.WriteLine("CLI options.Account is missing, please add -a YourAccount");
                return;
            }

            if (options.Location == null)
            {
                Console.WriteLine("CLI options.Location is missing, please add -l YourLocation");
                return;
            }

            var response = client.GetCustomerOutageHistory(options.Account, options.Location);
            foreach (var item in response)
            {
                Console.WriteLine(item.outageDescription);
            }

            var serializer = new XmlSerializer(typeof(proxyOA3ac.outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetCustomerOutageHistory.{options.Account}.{options.Location}", "3AC");
        }

        /// <summary>
        /// The get customers affected by outage.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void GetCustomersAffectedByOutage(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }

            var response = client.GetCustomersAffectedByOutage(options.OutageEventId);

            var serializer = new XmlSerializer(typeof(proxyOA3ac.customersAffectedByOutage));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            WriteToFile(xml, $"GetCustomersAffectedByOutage.{options.OutageEventId}", "3AC");
        }

        /// <summary>
        /// The get all circuit elements.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void GetAllCircuitElements(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllCircuitElements(lastReceived);

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetAllCircuitElements objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine(item.elementName);
                }

                var serializer = new XmlSerializer(typeof(proxyOA3ac.circuitElement[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }

                WriteToFile(xml, $"GetAllCircuitElements.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }
        #endregion

        #region SendCommands

        /// <summary>
        /// Send Outage Detection Event Notification
        /// </summary>
        /// <param name="client">Expects the OA_Server client</param>
        /// <param name="options">Requires Device option</param>
        private static void SendOdEventNotification(OA_Server client, Options options = null)
        {
            try
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

                const string EventTypeError = "EventType is missing. Please review the required options for types of events.";
                if (options.EventType == null)
                {
                    Console.WriteLine(EventTypeError);
                    return;
                }

                proxyOA3ac.outageEventType outEventType;
                switch (options.EventType)
                {
                    case "Inferred":
                        outEventType = proxyOA3ac.outageEventType.Inferred;
                        break;
                    case "Instantaneous":
                        outEventType = proxyOA3ac.outageEventType.Instantaneous;
                        break;
                    case "Outage":
                        outEventType = proxyOA3ac.outageEventType.Outage;
                        break;
                    case "NoResponse":
                        outEventType = proxyOA3ac.outageEventType.NoResponse;
                        break;
                    case "PowerOff":
                        outEventType = proxyOA3ac.outageEventType.PowerOff;
                        break;
                    case "PowerOn":
                        outEventType = proxyOA3ac.outageEventType.PowerOn;
                        break;
                    case "Restoration":
                        outEventType = proxyOA3ac.outageEventType.Restoration;
                        break;
                    default:
                        outEventType = proxyOA3ac.outageEventType.Outage;
                        Console.WriteLine($"{options.EventType} not found. Defaulting to Outage. Did you mean Inferred,Instantaneous,Outage,NoResponse,PowerOff,PowerOn,Restoration?");
                        break;
                }

                var transactionId = Guid.NewGuid().ToString();
                var outages = new[]
                {
                    new proxyOA3ac.outageDetectionEvent
                    {
                        eventTime = DateTime.UtcNow,
                        eventTimeSpecified = true,
                        outageEventType = outEventType,
                        outageEventTypeSpecified = true,
                        outageDetectDeviceID = options.Device,
                        outageDetectDeviceTypeSpecified = true,
                        outageDetectDeviceType = proxyOA3ac.outageDetectDeviceType.Meter,
                        outageLocation = new proxyOA3ac.outageLocation()
                        {
                            meterNo = options.Device
                        },
                        messageList = new[]
                        {
                            new proxyOA3ac.message
                            {
                                comments = "PowerRestoredOutageEvent",
                            }
                        }
                    }
                };
                var response = client.ODEventNotification(outages, transactionId);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // We might not get a response, if so exit
                if (response == null)
                {
                    return;
                }

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                PrintErrorObjects(response.ToArray<object>());
                var serializer = new XmlSerializer(typeof(proxyOA3ac.errorObject[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                WriteToFile(xml, $"ODEventNotification.{options.Device}.{options.EventType}", "3AC");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send Initiate Outage Detection Event Request
        /// </summary>
        /// <param name="client">Expects MDM_Server, or MR_Server clients.</param>
        /// <param name="options">Requires Device and ResponseUrl</param>
        private static void SendInitiateOutageDetectionEventRequest(MDM_Server client, Options options = null)
        {
            try
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

                if (options.ResponseUrl == null)
                {
                    Console.WriteLine("ResponseUrl is missing. Please add a ResponseUrl : -r http://yourserver/NOT_Server.asmx");
                    return;
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

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        private static void InitiateCdStateRequest(MDM_Server client, Options options)
        {
            var cdstates = new[]
            {
                new proxyMDM3ac.CDState
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
                    return;
                }

                if (options.Device == null)
                {
                    Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                    return;
                }

                if (options.ResponseUrl == null)
                {
                    Console.WriteLine(
                        "ResponseUrl is missing. Please add a ResponseUrl : -r http://yourserver/NOT_Server.asmx");
                    return;
                }

                var requestDate = DateTime.Now;
                var responseUrl = options.ResponseUrl;
                var transactionId = Guid.NewGuid().ToString();
                var expirationTime =
                    (float)requestDate.AddMinutes(30)
                        .ToOADate(); // TimeStamp is unix timestamp, so we use ToOADate to get a double and cast to float. 
                var response =
                    client.InitiateCDStateRequest(cdstates, responseUrl, transactionId, expirationTime);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
