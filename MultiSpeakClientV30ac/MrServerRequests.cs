// --------------------------------------------------------------------------------------------------------------------
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
                var client = new MR_Server { Url = options.EndPoint };
                var header = new MultiSpeakMsgHeader
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

                ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, errors) => true;
                switch (options.Method)
                {
                    case "CancelDisconnectedStatus":
                        message = CancelDisconnectedStatus(client, options);
                        break;
                    case "CancelUsageMonitoring":
                        message = CancelUsageMonitoring(client, options);
                        break;
                    case "GetAMRSupportedMeters":
                        message = GetAmrSupportedMeters(client);
                        break;
                    case "GetHistoryLogByMeterNo":
                        message = GetHistoryLogByMeterNo(client, options);
                        break;
                    case "GetLatestReadingByMeterNo":
                        message = GetLatestReadingByMeterNo(client, options);
                        break;
                    case "GetLatestReadings":
                        message = GetLatestReadings(client);
                        break;
                    case "IntiaiteCDStateRequest":
                        message = "IntiaiteCDStateRequest is in MDM_Server and not MR_Server";
                        Console.WriteLine(
                            $"{options.Method} is located in MDM_Server and not MR_Server, change -m to -m MDM_Server and try again.");
                        break;
                    case "InitiateConnectDisconnect":
                        message = "InitiateConnectDisconnect is in MDM_Server and not MR_Server";
                        Console.WriteLine(
                            $"{options.Method} is located in MDM_Server and not MR_Server, change -m to -m MDM_Server and try again.");
                        break;
                    case "InitiateMeterReadByMeterNumber":
                        message = InitiateMeterReadByMeterNumber(client, options);
                        break;
                    case "InitiateUsageMonitoring":
                        message = InitiateUsageMonitoring(client, options);
                        break;
                    case "InitiateDisconnectedStatus":
                        message = InitiateDisconnectedStatus(client, options);
                        break;
                    case "MeterAddNotification":
                        message = MeterNotification(client, options);
                        break;
                    case "MeterChangedNotification":
                        message = MeterNotification(client, options);
                        break;
                    case "MeterRemoveNotification":
                        message = MeterNotification(client, options);
                        break;
                    case "MeterRetireNotification":
                        message = MeterNotification(client, options);
                        break;
                    case "ServiceLocationChangeNotification":
                        message = ServiceLocationChangeNotification(client, options);
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

        /// <summary>
        /// CancelDisconnectedStatus to MR_Server
        /// </summary>
        /// <param name="client">
        /// Expects MR_Server
        /// </param>
        /// <param name="options">
        /// Expects options.Device
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CancelDisconnectedStatus(MR_Server client, Options options)
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

            var meterNos = new[] { options.Device };
            var response = client.CancelDisconnectedStatus(meterNos);
            if (response == null)
            {
                return Successfull;
            }

            // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
            // so instead of passing response we pass repsonse.ToArray<object>()
            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            return response[0].errorString;
        }

        /// <summary>
        /// Send Cancel Usage Monitoring
        /// </summary>
        /// <param name="client">
        /// Expects MR_Server
        /// </param>
        /// <param name="options">
        /// Exports option.Device
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CancelUsageMonitoring(MR_Server client, Options options)
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

            var meterNos = new[] { options.Device };
            var response = client.CancelUsageMonitoring(meterNos);
            if (response == null)
            {
                return Successfull;
            }

            // co-varient array conversion from errorObject[] to object[] can cause runtime error on write operation.
            // so instead of passing response we pass repsonse.ToArray<object>()
            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            return response[0].errorString;
        }

        /// <summary>
        /// The get amr supported meters.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetAmrSupportedMeters(MR_Server client)
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

                // TODO: Create IF EXISTS INSERT SCRITPS FOR METERS
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
            }

            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetHistoryLogByMeterNo(MR_Server client, Options options)
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

            // response[0].errorString;
            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetLatestReadingByMeterNo(MR_Server client, Options options)
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
            return Successfull;
        }

        /// <summary>
        /// The get latest readings.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetLatestReadings(MR_Server client)
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
            }

            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string InitiateMeterReadByMeterNumber(MR_Server client, Options options)
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

            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var expirationTime = (float)DateTime.Now.AddHours(1).ToOADate();
            var response = client.InitiateMeterReadByMeterNumber(meterNos, options.ResponseUrl, transactionId, expirationTime);
            if (response == null)
            {
                return Successfull;
            }

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

            XmlUtil.WriteToFile(xml, $"InitiateMeterReadByMeterNumber.{options.Device}.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// Initiate Disconnected Fail
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// expected options
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string InitiateDisconnectedStatus(MR_Server client, Options options)
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

            string[] meterNos = { options.Device };
            var response = client.InitiateDisconnectedStatus(meterNos);
            if (response == null)
            {
                return Successfull;
            }

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

            XmlUtil.WriteToFile(xml, $"InitiateDisconnectedStatus.{options.Device}.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// Initiate Usage Monitoring
        /// </summary>
        /// <param name="client">
        /// the client
        /// </param>
        /// <param name="options">
        /// expected options
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string InitiateUsageMonitoring(MR_Server client, Options options)
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

            string[] meterNos = { options.Device };
            var transactionId = Guid.NewGuid().ToString();
            var response = client.InitiateUsageMonitoring(meterNos, options.ResponseUrl, transactionId);
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

            XmlUtil.WriteToFile(xml, $"InitiateUsageMonitoring.{options.Device}.ERROR", "3AC", logFileDirectory);
            return xml;
        }

        /// <summary>
        /// MeterNotification for the four types of Notifications, see methods.
        /// </summary>
        /// <param name="client">
        /// Expects MR_Server
        /// </param>
        /// <param name="options">
        /// Expects options.Device
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string MeterNotification(MR_Server client, Options options)
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

            if (response == null)
            {
                return Successfull;
            }

            PrintClassStdOut.ErrorObjects(response.ToArray<object>());
            return response[0].errorString;
        }

        /// <summary>
        /// Send a Service Location Change Notification
        /// </summary>
        /// <param name="client">
        /// Expects the MR_Server Client
        /// </param>
        /// <param name="options">
        /// Expects a deviceId
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ServiceLocationChangeNotification(MR_Server client, Options options)
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
                if (response == null)
                {
                    return Successfull;
                }

                PrintClassStdOut.ErrorObjects(response.ToArray<object>());
                return response[0].errorString;
        }
    }
}
