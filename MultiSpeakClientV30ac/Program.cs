﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using CommandLine;

    using MultiSpeakClientV30ac.proxyMDM3ac;
    using MultiSpeakClientV30ac.proxyMR3ac;
    using MultiSpeakClientV30ac.proxyOA3ac;

    using action = MultiSpeakClientV30ac.proxyMR3ac.action;
    using extensionsItemExtType = MultiSpeakClientV30ac.proxyMR3ac.extensionsItemExtType;
    using remoteReconnectSetting = MultiSpeakClientV30ac.proxyMR3ac.remoteReconnectSetting;
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
            try
            {
                switch (options.Server)
                {
                    case "OA_Server":
                        {
                            var client = new OA_Server { Url = options.EndPoint, };
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
                            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                delegate { return true; };

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
                                    GetOutageEventStatus(client, options);
                                    break;
                                case "GetOutageStatusByLocation":
                                    GetOutageStatusByLocation(client, options);
                                    break;
                                case "GetCustomersAffectedByOutage":
                                    GetCustomersAffectedByOutage(client, options);
                                    break;
                                case "GetCustomerOutageHistory":
                                    GetCustomerOutageHistory(client, options);
                                    break;
                                case "ODEventNotification":
                                    SendOdEventNotification(client, options);
                                    break;
                                default:
                                    Console.WriteLine(
                                        $"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                    break;
                            }

                            break;
                        }

                    case "MDM_Server":
                        {
                            var client = new MDM_Server() { Url = options.EndPoint, };
                            var header = new proxyMDM3ac.MultiSpeakMsgHeader()
                            {
                                UserID = options.UserId,
                                Pwd = options.Pwd,
                                AppName = AppName,
                                AppVersion = AppVersion,
                                Company = options.Company,
                                Version = Version,
                                MessageID = new Guid().ToString()
                            };
                            client.MultiSpeakMsgHeaderValue = header;

                            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                (obj, certificate, chain, errors) => true;
                            switch (options.Method)
                            {
                                case "GetCDSupportedMeters":
                                    GetCdSupportedMeters(client);
                                    break;
                                case "InitiateOutageDetectionEventRequest":
                                    SendInitiateOutageDetectionEventRequest(client, options);
                                    break;
                                case "PingUrl":
                                    PingUrl(client);
                                    break;
                                case "InitiateCDStateRequest":
                                    // InitiateCdStateRequest(client, options);
                                    break;
                                default:
                                    Console.WriteLine(
                                        $"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                    break;
                            }

                            break;
                        }

                    case "MR_Server":
                        {
                            var client = new MR_Server { Url = options.EndPoint, };
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
                                case "CancelDisconnectedStatus":
                                    SendCancelDisconnectedStatus(client, options);
                                    break;
                                case "CancelUsageMonitoring":
                                    SendCancelUsageMonitoring(client, options);
                                    break;
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
                                case "IntiaiteCDStateRequest":
                                    InitiateCdStateRequest(client, options);
                                    break;
                                case "InitiateMeterReadByMeterNumber":
                                    InitiateMeterReadByMeterNumber(client, options);
                                    break;
                                case "InitiateUsageMonitoring":
                                    InitiateUsageMonitoring(client, options);
                                    break;
                                case "InitiateDisconnectedStatus":
                                    InitiateDisconnectedStatus(client, options);
                                    break;
                                case "MeterAddNotification":
                                    SendMeterNotification(client, options, options.Method);
                                    break;
                                case "MeterChangedNotification":
                                    SendMeterNotification(client, options, options.Method);
                                    break;
                                case "MeterRemoveNotification":
                                    SendMeterNotification(client, options, options.Method);
                                    break;
                                case "MeterRetireNotification":
                                    SendMeterNotification(client, options, options.Method);
                                    break;
                                case "ServiceLocationChangeNotification":
                                    SendServiceLocationChangeNotification(client, options);
                                    break;
                                default:
                                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                                    Console.WriteLine(
                                        $"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                                    break;
                            }

                            break;
                        }

                    default:
                        Console.WriteLine($"{options.Server} not found. Did you mean OA_Server or MDM_Server?");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} - Occured");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception : {ex.InnerException.Message}");
                }

                PrintOptions(options);
            }
        }

        /// <summary>
        /// SendMeterNotification for the four types of Notifications, see methods.
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Expects options.Device</param>
        /// <param name="method">MeterAddNotification, MeterChangeNotification, MeterRemoveNotification, MeterRetireNotification</param>
        private static void SendMeterNotification(MR_Server client, Options options, string method)
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

                var meters = new[]
                {
                    new proxyMR3ac.meter
                    {
                        meterNo = options.Device,
                        serialNumber = options.Device,
                        meterType = "Meter Type",
                        manufacturer = string.Empty,
                        sealNumberList = new[] { "12345678" , "ABCDEFG" },
                        AMRType = "AMR Type",
                        AMRDeviceType = string.Empty,
                        AMRVendor = string.Empty,
                        nameplate = new proxyMR3ac.nameplate { dials = 6, dialsSpecified = true, multiplier = 100, multiplierSpecified = true },
                        utilityInfo = new proxyMR3ac.utilityInfo { accountNumber = "AccountNumber", servLoc = "LocationNumber", custID = "customerID" },
                        meterConnectionStatus = proxyMR3ac.meterConnectionStatus.Connected,
                        meterConnectionStatusSpecified = true,
                        remoteReconnectSetting = remoteReconnectSetting.Arm
                    }
                };

                proxyMR3ac.errorObject[] response = null;
                switch (options.Method)
                {
                    case "MeterAddNotification":
                        response = client.MeterAddNotification(meters);
                        break;
                    case "MeterChangedNotification":
                        response = client.MeterChangedNotification(meters);
                        break;
                    case "MeterRemoveNotification":
                        response = client.MeterRemoveNotification(meters);
                        break;
                    case "MeterRetireNotification":
                        response = client.MeterRetireNotification(meters);
                        break;
                    default:
                        Console.WriteLine("Check the list of methods in the README.md for each Server.");
                        Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                        break;
                }

                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
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
        /// SendCancelDisconnectedStatus to MR_Server
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Expects options.Device</param>
        private static void SendCancelDisconnectedStatus(MR_Server client, Options options)
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

                var meterNos = new[] { options.Device };
                var response = client.CancelDisconnectedStatus(meterNos);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
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
        /// SendCancelUsageMonitoring
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Exports option.Device</param>
        private static void SendCancelUsageMonitoring(MR_Server client, Options options)
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

                var meterNos = new[] { options.Device };
                var response = client.CancelUsageMonitoring(meterNos);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
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
        /// Send a ServiceLocationChangeNotification
        /// </summary>
        /// <param name="client">Expects the MR_Server Client</param>
        /// <param name="options">Expects a deviceId</param>
        private static void SendServiceLocationChangeNotification(MR_Server client, Options options)
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

                var serviceLocations = new[]
                {
                    new proxyMR3ac.serviceLocation
                    {
                        objectID  = options.Device,  // Stuff the device id here for testing.
                        verb = action.Change,
                        extensionsList = new[]
                        {
                            new proxyMR3ac.extensionsItem { extName = "homeAC", extValue = "830" },
                            new proxyMR3ac.extensionsItem { extName = "homePhone", extValue = "7082476" },
                            new proxyMR3ac.extensionsItem { extName = "voteSw", extValue = "Y" },
                            new proxyMR3ac.extensionsItem { extName = "votingDistCd", extValue = "6" },
                            new proxyMR3ac.extensionsItem { extName = "rateStartDt", extValue = "02/18/2016" },
                            new proxyMR3ac.extensionsItem { extName = "tempLocationCd", extValue = "P" },
                            new proxyMR3ac.extensionsItem { extName = "dnpSw", extValue = "false", extType = extensionsItemExtType.boolean, extTypeSpecified = true },
                        },
                        gridLocation = "63556308",
                        facilityID = "63556308",
                        custID = "209740",
                        accountNumber = "209740001",
                        servAddr1 = "822 SCHUMACHER",
                        servCity = "NEW BRAUNFELS",
                        servState = "TX",
                        servZip = "78130",
                        revenueClass = "Craig+31",
                        servStatus = "1",
                        billingCycle = "8",
                        route = "36",
                        acRecvBal = -50.01f,
                        acRecvCur = -50.01f,
                        connectDate = DateTime.Parse("1999-12-31"),
                        network = new proxyMR3ac.network
                        {
                            boardDist = "6",
                            franchiseDist = "2",
                            schoolDist = "34",
                            district = "20",
                            county = "6",
                            cityCode = "2",
                            substationCode = "68",
                            feeder = "5",
                            phaseCd = proxyMR3ac.phaseCd.A,
                            eaLoc = new proxyMR3ac.eaLoc { name = "27" },
                            linkedTransformer = new proxyMR3ac.linkedTransformer
                            {
                                bankID = "T63556308002", unitList = new[] { "T-32405" }
                            },
                            linemanServiceArea = "08",
                         },
                        phoneList = new[] { new proxyMR3ac.phoneNumber { phone = "8307082476", phoneType = proxyMR3ac.phoneNumberPhoneType.Home } },
                        timezone = new proxyMR3ac.timeZone { DSTEnabled = true, UTCOffset = -6, name = "Central Standard Time" },
                        description = "MTR ON A/E HOME"
                    }
                };
                var response = client.ServiceLocationChangedNotification(serviceLocations);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
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

        private static void PrintOptions(object options)
        {
            Console.WriteLine("Options >");
            var t = options.GetType();
            foreach (var pi in t.GetProperties())
            {
                var name = pi.Name;
                var value = string.Empty;
                if (pi.GetValue(options) != null)
                {
                    value = pi.GetValue(options, null).ToString();
                }

                Console.WriteLine($"\t{name} : {value}");
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
        private static void PrintErrorObjects(IEnumerable<object> errorObjects)
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
            var response = client.PingURL();

            // This MessageID will be the MessageID from the server and not Message from the Client
            // since the response will contain the out soap header.
            // This shows how one can use the Header to pass information from the client to the server
            // and from the server to the client.  The Chunking Examples for GetAllConnectivity is
            // the best use case I have for how the headers are used.
            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

            // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
            // so instead of passing response we paass repsonse.ToArray<object>()
            if (response != null)
            {
                PrintErrorObjects(response.ToArray<object>());
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
                        xml = sww.ToString();
                    }
                }

                WriteToFile(xml, $"GetAMRSupportedMeters.{objectsRemaining}", "3AC");
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
            }
        }

        private static void GetCdSupportedMeters(MDM_Server client)
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

                var serializer = new XmlSerializer(typeof(proxyMR3ac.meter[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                WriteToFile(xml, $"GetCDSupportedMeters.{objectsRemaining}", "3AC");
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


        private static void InitiateUsageMonitoring(MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var response = client.InitiateUsageMonitoring(meterNos, options.ResponseUrl, transactionId);

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

            WriteToFile(xml, $"InitiateUsageMonitoring.{options.Device}", "3AC");
        }

        private static void InitiateDisconnectedStatus(MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var response = client.InitiateDisconnectedStatus(meterNos);

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

            WriteToFile(xml, $"InitiateDisconnectedStatus.{options.Device}", "3AC");
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
                        xml = sww.ToString();
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

            PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);
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

                Console.WriteLine($"GetAllActiveOutageEvents items in response : {response.Length}");
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
                const string UrlCustomers = " MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetCustomerOutageHistory ";
                const string Url = " MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -v Restoration ";
                var meterNo = $"-d {item.meterNo}";
                var duration = item.interruptionDuration;
                Console.WriteLine(meterNo);

                using (var file = new StreamWriter("BATCH_GetCustomerOutageHistory.txt", true))
                {
                    file.WriteLine($"{UrlCustomers} -a {item.accountNumber} -l {item.servLoc}");
                }

                using (var file = new StreamWriter("BATCH_ODEventNotifications.txt", true))
                {
                    file.WriteLine($"{Url}{meterNo}");
                }

                using (var file = new StreamWriter("BATCH_MeterIdentifiers.txt", true))
                {
                    file.WriteLine(item.meterNo);
                }

                // If no duration skip writing the data to a file.
                if (duration == null)
                {
                    continue;
                }

                if (int.Parse(duration) <= 0)
                {
                    continue;
                }

                Console.WriteLine($"meterNo : {meterNo} | duration : {duration}");
                using (var file = new StreamWriter("GetActiveOutages_greater_than_zero.csv", true))
                {
                    file.WriteLine($"{options.OutageEventId},{meterNo},{duration}");
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
                Console.WriteLine("GetOutageEventStatus takes the parameter OutageEventID : -o YourOutageEventId");
                return;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventId");
                return;
            }

            var response = client.GetOutageEventStatus(options.OutageEventId);
            Console.WriteLine($"comments : {response.comments} | errorString : {response.errorString}");
            Console.WriteLine($"objectID : {response.objectID} | outageStatus : {response.outageStatus}");

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
                Console.WriteLine("GetOutageStatusByLocation takes the parameter OutageEventID : -l YourLocationID");
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
            Console.WriteLine($"location.status : {response.status} | statusSpecified : {response.statusSpecified}");

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
                Console.WriteLine($"OutageEventId->{options.OutageEventId},{item.meterNo},{item.accountNumber},{item.servLoc},interruptionDuration>{item.interruptionDuration},{item.timeRestored},{item.outageDescription}");

                // If no duration skip writing the data to a file.
                if (item.interruptionDuration == null)
                {
                    continue;
                }

                if (int.Parse(item.interruptionDuration) <= 0)
                {
                    continue;
                }

                using (var file = new StreamWriter("GetCustomerOutageDurations_greater_than_zero.csv", true))
                {
                    file.WriteLine($"OutageEventId->{options.OutageEventId},{item.meterNo},{item.accountNumber},{item.servLoc},interruptionDuration>{item.interruptionDuration},{item.timeRestored},{item.outageDescription}");
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
                        xml = sww.ToString();
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
                    Console.WriteLine("SendOdEventNotification requires options.Device : -d 123456789");
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
        private static void InitiateCdStateRequest(MR_Server client, Options options)
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
                                     // var response =
                                     //    client.Initiate(cdstates, responseUrl, transactionId, expirationTime);
                PrintMultiSpeakMsgHeader(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we paass repsonse.ToArray<object>()
                //if (response != null)
                //{
                //    PrintErrorObjects(response.ToArray<object>());
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
