﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MrServerRequests.cs" company="Craig">
//   Blah
// </copyright>
// <summary>
//   Defines the MrServerRequests type.
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

    using MultiSpeakClientV30ac.proxyMR3ac;

    /// <summary>
    /// The MR server requests.
    /// </summary>
    public static class MrServerRequests
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
            var client = new MR_Server { Url = options.EndPoint };
            var header = new MultiSpeakMsgHeader
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
            switch (options.Method)
            {
                case "CancelDisconnectedStatus":
                    CancelDisconnectedStatus(client, options);
                    break;
                case "CancelUsageMonitoring":
                    CancelUsageMonitoring(client, options);
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
                    Console.WriteLine($"{options.Method} is located in MDM_Server and not MR_Server, change -m to -m MDM_Server and try again.");
                    break;
                case "InitiateConnectDisconnect":
                    Console.WriteLine($"{options.Method} is located in MDM_Server and not MR_Server, change -m to -m MDM_Server and try again.");
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
                    MeterNotification(client, options);
                    break;
                case "MeterChangedNotification":
                    MeterNotification(client, options);
                    break;
                case "MeterRemoveNotification":
                    MeterNotification(client, options);
                    break;
                case "MeterRetireNotification":
                    MeterNotification(client, options);
                    break;
                case "ServiceLocationChangeNotification":
                    ServiceLocationChangeNotification(client, options);
                    break;
                default:
                    Console.WriteLine("Check the list of methods in the README.md for each Server.");
                    Console.WriteLine(
                        $"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                    break;
            }

            // TODO: I wonder if I can just output the Print for MultiSpeakHeader here for the client response.
            return "Success";
        }

        /// <summary>
        /// CancelDisconnectedStatus to MR_Server
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Expects options.Device</param>
        private static void CancelDisconnectedStatus(MR_Server client, Options options)
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
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send Cancel Usage Monitoring
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Exports option.Device</param>
        private static void CancelUsageMonitoring(MR_Server client, Options options)
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
                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintClassStdOut.ErrorObjects(response.ToArray<object>());
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

                XmlUtil.WriteToFile(xml, $"GetAMRSupportedMeters.{objectsRemaining}", "3AC", logFileDirectory);
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
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
            var serializer = new XmlSerializer(typeof(historyLog[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetHistoryLogByMeterNo.{options.Device}", "3AC", logFileDirectory);
            PrintClassStdOut.PrintObject(
                client.MultiSpeakMsgHeaderValue);
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
            var serializer = new XmlSerializer(typeof(meterRead));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetLatestReadingByMeterNo.{options.Device}", "3AC", logFileDirectory);
            PrintClassStdOut.PrintObject(
                client.MultiSpeakMsgHeaderValue);
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

                var serializer = new XmlSerializer(typeof(meterRead[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                XmlUtil.WriteToFile(xml, $"GetLatestReadings.{objectsRemaining}", "3AC", logFileDirectory);
                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);
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

            PrintClassStdOut.PrintObject(
                client.MultiSpeakMsgHeaderValue);

            // We might not get a response, if so exit
            if (response == null)
            {
                return;
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

            XmlUtil.WriteToFile(xml, $"InitiateMeterReadByMeterNumber.{options.Device}", "3AC", logFileDirectory);
        }
        
        /// <summary>
        /// Initiate Disconnected Status
        /// </summary>
        /// <param name="client">the client</param>
        /// <param name="options">expected options</param>
        private static void InitiateDisconnectedStatus(MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var response = client.InitiateDisconnectedStatus(meterNos);

            PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

            // We might not get a response, if so exit
            if (response == null)
            {
                return;
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

            XmlUtil.WriteToFile(xml, $"InitiateDisconnectedStatus.{options.Device}", "3AC", logFileDirectory);
        }

        /// <summary>
        /// Initiate Usage Monitoring
        /// </summary>
        /// <param name="client">the client</param>
        /// <param name="options">expected options</param>
        private static void InitiateUsageMonitoring(MR_Server client, Options options)
        {
            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var response = client.InitiateUsageMonitoring(meterNos, options.ResponseUrl, transactionId);

            PrintClassStdOut.PrintObject(
                client.MultiSpeakMsgHeaderValue);

            // We might not get a response, if so exit
            if (response == null)
            {
                return;
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

            XmlUtil.WriteToFile(xml, $"InitiateUsageMonitoring.{options.Device}", "3AC", logFileDirectory);
        }

        /// <summary>
        /// MeterNotification for the four types of Notifications, see methods.
        /// </summary>
        /// <param name="client">Expects MR_Server</param>
        /// <param name="options">Expects options.Device</param>
        private static void MeterNotification(MR_Server client, Options options)
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
                    new meter
                    {
                        meterNo = options.Device,
                        serialNumber = options.Device,
                        meterType = "Meter Type",
                        manufacturer = string.Empty,
                        sealNumberList = new[] { "12345678", "ABCDEFG" },
                        AMRType = "AMR Type",
                        AMRDeviceType = string.Empty,
                        AMRVendor = string.Empty,
                        nameplate = new nameplate { dials = 6, dialsSpecified = true, multiplier = 100, multiplierSpecified = true },
                        utilityInfo = new utilityInfo { accountNumber = "AccountNumber", servLoc = "LocationNumber", custID = "customerID" },
                        meterConnectionStatus = meterConnectionStatus.Connected,
                        meterConnectionStatusSpecified = true,
                        remoteReconnectSetting = remoteReconnectSetting.Arm
                    }
                };

                errorObject[] response = null;
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

                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send a Service Location Change Notification
        /// </summary>
        /// <param name="client">Expects the MR_Server Client</param>
        /// <param name="options">Expects a deviceId</param>
        private static void ServiceLocationChangeNotification(MR_Server client, Options options)
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
                    new serviceLocation
                    {
                        objectID  = options.Device,  // Stuff the device id here for testing.
                        verb = action.Change,
                        extensionsList = new[]
                        {
                            new extensionsItem { extName = "homeAC", extValue = "830" },
                            new extensionsItem { extName = "homePhone", extValue = "7082476" },
                            new extensionsItem { extName = "voteSw", extValue = "Y" },
                            new extensionsItem { extName = "votingDistCd", extValue = "6" },
                            new extensionsItem { extName = "rateStartDt", extValue = "02/18/2016" },
                            new extensionsItem { extName = "tempLocationCd", extValue = "P" },
                            new extensionsItem { extName = "dnpSw", extValue = "false", extType = extensionsItemExtType.boolean, extTypeSpecified = true }
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
                        network = new network
                        {
                            boardDist = "6",
                            franchiseDist = "2",
                            schoolDist = "34",
                            district = "20",
                            county = "6",
                            cityCode = "2",
                            substationCode = "68",
                            feeder = "5",
                            phaseCd = phaseCd.A,
                            eaLoc = new eaLoc { name = "27" },
                            linkedTransformer = new linkedTransformer
                            {
                                bankID = "T63556308002", unitList = new[] { "T-32405" }
                            },
                            linemanServiceArea = "08"
                         },
                        phoneList = new[] { new phoneNumber { phone = "8307082476", phoneType = phoneNumberPhoneType.Home } },
                        timezone = new timeZone { DSTEnabled = true, UTCOffset = -6, name = "Central Standard Time" },
                        description = "MTR ON A/E HOME"
                    }
                };
                var response = client.ServiceLocationChangedNotification(serviceLocations);
                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);

                // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
                // so instead of passing response we pass repsonse.ToArray<object>()
                if (response != null)
                {
                    PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}