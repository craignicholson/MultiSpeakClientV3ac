using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CommandLine;
using MultiSpeakClientV30ac.proxyMDM3ac;
using MultiSpeakClientV30ac.proxyMR3ac;
using MultiSpeakClientV30ac.proxyOA3ac;

namespace MultiSpeakClientV30ac
{
    /// <summary>
    /// Options for the CLI Interface via NuGet CommandLineParser
    /// Used to help manage the CLI Interface.
    /// </summary>
    internal class Options
    {
        [Option('e', "EndPoint", Required = true,
            HelpText = "Url or Endpoint you will send requests Typically http://server/OA_Server, etc...")]
        public string EndPoint { get; set; }

        [Option('u', "UserID", Required = true,
            HelpText = "UserID for the multispeak endpoint.")]
        public string UserId { get; set; }

        [Option('p', "Password", Required = true,
            HelpText = "Pwd (Password) for the multispeak endpoint.")]
        public string Pwd { get; set; }

        [Option('c', "Company", Required = true,
            HelpText = "Company is required since some vendors use Company for security validation.")]
        public string Company { get; set; }

        [Option('s', "Server", Required = true,
            HelpText = "MDM_Server or OA_Server")]
        public string Server { get; set; }

        [Option('m', "Method", Required = true,
            HelpText = "This is method PingUrl, GetActiveOutages, etc...")]
        public string Method { get; set; }

        // Optional Parameters 
        [Option('l', "Location", Required = false,
            HelpText = "Location or servLoc used in methods")]
        public string Location { get; set; }

        [Option('a', "Account", Required = false,
            HelpText = "Account used in methods... xyz")]
        public string Account { get; set; }

        [Option('o', "OutageEventId", Required = false,
            HelpText = "OutageEventId used specifically for a few methods... xyz")]
        public string OutageEventId { get; set; }

        [Option('d', "Device", Required = false,
            HelpText = "Device is a meterNo, meterIdentifier, etc.. used in methods which require meterNo.")]
        public string Device { get; set; }

        [Option('r', "ResponseUrl", Required = false,
            HelpText = "ResponseUrl is used for Notifications repsonses, like InitiateOutageDetectionEventRequest which are asynchronous.")]
        public string ResponseUrl { get; set; }

        [Option('v', "EventType", Required = false,
            HelpText = "EventType is used to ODEventNotification {Instantaneous, Outage, Restoration, NoResponse, Inferred, PowerOn, PowerOff}")]
        public string EventType { get; set; }

        // Omitting long name, default --verbose
        [Option(
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

    }

    internal class Program
    {
        private const string Version = "3.0AC"; //MultiSpeakMsgHeader.Version, Version of Multispeak
        private const string AppName = "MultiSpeakClient30ac"; //MultiSpeakMsgHeader.AppName
        private const string AppVersion = "1.0beta"; //MultiSpeakMsgHeader.AppVersion
        /// <summary>
        /// Main Entry Point into Application
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello MultiSpeak 3AC Client - Grab a cup of Joe and let's go!");
            Console.WriteLine(Art());
            Console.WriteLine("\n");

            var options = new Options();
            var isValid = Parser.Default.ParseArgumentsStrict(args, options);
            if (!isValid)
                return;

            var wallTime = new System.Diagnostics.Stopwatch();  // Get the wall time for sampling stats
            wallTime.Start();

            var run = new Program();
            // Wrap in try catch here... and leave out the others???
            run.ProcessArgs(options);

            Console.WriteLine("TASK FINISHED");
            wallTime.Stop();
            //var st = new System.Diagnostics.StackTrace();
            //var method = st.GetFrame(1).GetMethod().Name;
            Console.WriteLine($"Execution Time : {wallTime.Elapsed}");
            //Console.ReadLine();
        }

        /// <summary>
        /// ProcessArgs processes the arguments and executes the correct method.
        /// </summary>
        /// <param name="options"></param>
        public void ProcessArgs(Options options)
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

                        //self-signed cert override - this is another way to do this... for testing...
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
                        {
                            return (true);
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

                        //self-signed cert override - because people don't want to pay for valid certs. and .Net blocks self-signed
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            (obj, certificate, chain, errors) => (true);
                        switch (options.Method)
                        {
                            case "InitiateOutageDetectionEventRequest":
                                SendInitiateOutageDetectionEventRequest(client, options);
                                break;
                            case "PingUrl":
                                PingUrl(client);
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

                        //self-signed cert override - because people don't want to pay for valid certs. and .Net blocks self-signed
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            (obj, certificate, chain, errors) => (true);
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
        /// Art needs to be read from a file to make this easier.  And swap ASCII Art.  :-)
        /// </summary>
        /// <returns></returns>
        private static string Art()
        {
            const string coffee = @"
                            \ | ( | ) / /
                          _________________
                          |               |
                          |               |
                          |               /--\
                          |               |  |
                           \             /\--/
                            \___________ /";
            return coffee;
        }

        /// <summary>
        /// Write to file in bin directory and move on... 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="method"></param>
        /// <param name="version"></param>
        private static void WriteToFile(string xml, string method, string version)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var dir = @"C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug\LG";
                var fileName = $@"{dir}\{method}.{version}.Response" + $@".{timestamp}.xml";
                using (var file = new StreamWriter(fileName, append: false))
                    file.WriteLine(xml);
                Console.WriteLine($"File writen to disk : {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// PrintMultiSpeakMsgHeader for OA_Server prints out all name values pairs in the soap header object.
        /// Best use case is printing out the header for a response.  LastSent, ObjectsRemaining
        /// are used when 'chunking' data.  Typical use case for synchronous web services, so the client
        /// can request the next set of data.  You can use this method to Print out the client Headers
        /// however the main intention is printing out the Server's Headers since the values are actionable.
        /// </summary>
        /// <param name="multiSpeakMsgHeaderValue"></param> 
        private static void PrintMultiSpeakMsgHeader(object multiSpeakMsgHeaderValue)
        {
            Console.WriteLine("MultiSpeakMsgHeader Server >");
            var t = multiSpeakMsgHeaderValue.GetType();
            foreach (var pi in t.GetProperties())
            {
                var name = pi.Name;
                var value = string.Empty;
                if (pi.GetValue(multiSpeakMsgHeaderValue) != null)
                    value = pi.GetValue(multiSpeakMsgHeaderValue, null).ToString();
                Console.WriteLine($"\t{name} : {value}");
            }
        }

        /// <summary>
        /// PrintErrorObjects for all MultiSpeak Servers since each wsdl instantiates its own errorObjects[]
        /// eg. proxyMDM3ac.errorObjects, proxyOA3ac.errorObjects
        /// </summary>
        /// <param name="errorObjects"></param>
        private static void PrintErrorObjects(object[] errorObjects)
        {
            if (errorObjects == null) return;
            foreach (var error in errorObjects)
            {
                Console.WriteLine("errorObject[] >");
                var t = error.GetType();
                foreach (var pi in t.GetProperties().ToArray())
                {
                    var name = pi.Name;
                    var value = string.Empty;
                    if (pi.GetValue(error) != null)
                        value = pi.GetValue(error, null).ToString();
                    Console.WriteLine($"\t{name} : {value}");
                }
            }
        }

        //private static void PrintErrorObjects(proxyMR3ac.errorObject[] errorObjects)
        //{
        //    Console.WriteLine("errorObject[] >");
        //    if (errorObjects == null) return;
        //    foreach (var error in errorObjects)
        //    {
        //        Console.WriteLine($"\t{error.errorString} : {error.errorString}");
        //        Console.WriteLine($"\t{error.eventTime} : {error.eventTime}");
        //        Console.WriteLine($"\t{error.nounType} : {error.nounType}");
        //        Console.WriteLine($"\t{error.objectID} : {error.objectID}");
        //    }
        //}
        //private static void PrintErrorObjects(proxyMDM3ac.errorObject[] errorObjects)
        //{
        //    Console.WriteLine("errorObject[] >");
        //    if (errorObjects == null) return;
        //    foreach (var error in errorObjects)
        //    {
        //        Console.WriteLine($"\t{error.errorString} : {error.errorString}");
        //        Console.WriteLine($"\t{error.eventTime} : {error.eventTime}");
        //        Console.WriteLine($"\t{error.nounType} : {error.nounType}");
        //        Console.WriteLine($"\t{error.objectID} : {error.objectID}");
        //    }
        //}
        //private static void PrintErrorObjects(proxyOA3ac.errorObject[] errorObjects)
        //{
        //    Console.WriteLine("errorObject[] >");
        //    if (errorObjects == null) return;
        //    foreach (var error in errorObjects)
        //    {
        //        Console.WriteLine($"\t{error.errorString} : {error.errorString}");
        //        Console.WriteLine($"\t{error.eventTime} : {error.eventTime}");
        //        Console.WriteLine($"\t{error.nounType} : {error.nounType}");
        //        Console.WriteLine($"\t{error.objectID} : {error.objectID}");
        //    }
        //}

        /// <summary> 
        /// PingUrl, this a test to demonstrate how the response for a synchronous call 
        /// will have the MultiSpeakMsgHeader values from the server which responds.
        /// Check the MultiSpeakBusArch30AC for the PingUrl method to change out going
        /// header values.
        /// <param> name="client" </param>
        /// </summary>
        public static void PingUrl(proxyMDM3ac.MDM_Server client)
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

                PrintErrorObjects(response);
                Console.WriteLine("Response from server : client.MultiSpeakMsgHeaderValue ");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void GetAmrSupportedMeters(proxyMR3ac.MR_Server client)
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

                var xsSubmit = new XmlSerializer(typeof(proxyMR3ac.meter[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }
                WriteToFile(xml, $"GetAMRSupportedMeters.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }

        }

        private static void GetHistoryLogByMeterNo(proxyMR3ac.MR_Server client, Options options)
        {
            var response = client.GetHistoryLogByMeterNo(options.Device, DateTime.Now.Subtract(TimeSpan.FromDays(1000)), DateTime.Now);
            var xsSubmit = new XmlSerializer(typeof(proxyMR3ac.historyLog[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetHistoryLogByMeterNo.{options.Device}", "3AC");
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
        }

        private static void GetLatestReadingByMeterNo(proxyMR3ac.MR_Server client, Options options)
        {
            var response = client.GetLatestReadingByMeterNo(options.Device);
            var xsSubmit = new XmlSerializer(typeof(proxyMR3ac.meterRead));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetLatestReadingByMeterNo.{options.Device}", "3AC");
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
        }

        private static void GetLatestReadings(proxyMR3ac.MR_Server client)
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

                var xsSubmit = new XmlSerializer(typeof(proxyMR3ac.meterRead[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }
                WriteToFile(xml, $"GetLatestReadings.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        private static void InitiateMeterReadByMeterNumber(proxyMR3ac.MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)DateTime.Now.AddHours(1).ToOADate();
            var response = client.InitiateMeterReadByMeterNumber(meterNos, options.ResponseUrl, transactionId, expirationTime);
           
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            PrintErrorObjects(response);

            var xsSubmit = new XmlSerializer(typeof(proxyMR3ac.errorObject[]));
            string xml;
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"InitiateMeterReadByMeterNumber.{options.Device}", "3AC");
        }

        #endregion

        #region GetCommands

        /// <summary>
        /// GetAllConnectivityChunking returns a dump of the entire connectivity model.  This method
        /// requires multiple requests using the LastSent and ObjectsRemaining
        /// </summary>
        /// <param name="client"></param>
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
                var xsSubmit = new XmlSerializer(typeof(MultiSpeak));
                string xml;
                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }
                WriteToFile(xml, $"GetAllConnectivityChunking.{objectsRemaining}", "3AC");
            }
        }

        private static void GetActiveOutages(OA_Server client)
        {
            var response = client.GetActiveOutages();
            Console.WriteLine("GetActiveOutages");
            Console.WriteLine($"Active Outages : {response.Length}");
            foreach (var item in response)
            {
                Console.WriteLine(item);
            }
            var xsSubmit = new XmlSerializer(typeof(string[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, "GetActiveOutages", "3AC");
        }

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
                var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.outageEvent[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
                        xml = sww.ToString(); // Your XML
                    }
                }
                WriteToFile(xml, $"GetAllActiveOutageEvents.{objectsRemaining}", "3AC");

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        private static void GetOutageDurationEvents(OA_Server client, Options options = null)
        {
            if (options == null) return;
            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }

            var response = client.GetOutageDurationEvents(options.OutageEventId);
            // Help a coder out, print out account and servLoc and append to a file
            foreach (var item in response)
            {
                //var url = "MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetCustomerOutageHistory ";
                var url = " MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -v Restoration ";
                //var data = $"-a {item.accountNumber} -l {item.servLoc}";
                var data = $"-d {item.meterNo}";
                Console.WriteLine(data);
                using (var file = new StreamWriter("BATCH_ODEventNotifications.txt", true))
                {
                    file.WriteLine($"{url}{data}");
                }
            }

            var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetOutageDurationEvents.{options.OutageEventId}", "3AC");
        }

        private static void GetOutageEventStatus(OA_Server client, Options options = null)
        {
            if (options == null) return;
            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }
            var response = client.GetOutageEventStatus(options.OutageEventId);

            Console.WriteLine(response.comments);

            var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.outageEventStatus));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetOutageEventStatus.{options.OutageEventId}", "3AC");
        }

        private static void GetOutageStatusByLocation(OA_Server client, Options options = null)
        {
            if (options == null) return;
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
            var xsSubmit = new XmlSerializer(typeof(locationStatus));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetOutageStatusByLocation.{options.Location}", "3AC");
        }

        private static void GetCustomerOutageHistory(OA_Server client, Options options = null)
        {
            if (options == null) return;
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
            var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetCustomerOutageHistory.{options.Account}.{options.Location}", "3AC");
        }

        private static void GetCustomersAffectedByOutage(OA_Server client, Options options = null)
        {
            if (options == null) return;
            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return;
            }
            var response = client.GetCustomersAffectedByOutage(options.OutageEventId);

            var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.customersAffectedByOutage));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }
            WriteToFile(xml, $"GetCustomersAffectedByOutage.{options.OutageEventId}", "3AC");
        }

        public void GetAllCircuitElements(OA_Server client)
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

                var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.circuitElement[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
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
        /// SendOdEventNotification sends
        /// </summary>
        /// <param name="client">Expects the OA_Server client</param>
        /// <param name="options"></param>
        public void SendOdEventNotification(OA_Server client, Options options = null)
        {
            try
            {
                if (options == null) return;
                if (options.Device == null)
                {
                    Console.WriteLine("Device is missing. Please add a meterNo: -d 123456789");
                    return;
                }
                const string eventTypeError = "EventType is missing. Please review the required options for types of events.";
                if (options.EventType == null)
                {
                    Console.WriteLine(eventTypeError);
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
                PrintErrorObjects(response);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                var xsSubmit = new XmlSerializer(typeof(proxyOA3ac.errorObject[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, response);
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
        /// SendInitiateOutageDetectionEventRequest
        /// </summary>
        /// <param name="client">Expects MDM_Server, or MR_Server clients.</param>
        /// <param name="options">Requiers Device and ResponseUrl</param>
        public void SendInitiateOutageDetectionEventRequest(MDM_Server client, Options options = null)
        {
            try
            {
                if (options == null) return;
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
                var response = client.InitiateOutageDetectionEventRequest(meters, requestDate, responseUrl,
                    transactionId, expirationTime);

                PrintErrorObjects(response);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
