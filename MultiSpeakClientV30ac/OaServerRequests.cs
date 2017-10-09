// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OaServerRequests.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   Defines the OaServerRequests type.
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

    using MultiSpeakClientV30ac.proxyOA3ac;

    /// <summary>
    /// The OA server requests.
    /// </summary>
    public static class OaServerRequests
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
        /// Error Message 
        /// </param>
        public static void RunCommand(
            Options options,
            string appName,
            string appVersion,
            string version,
            out string message)
        {
            // Wrap this in try catch instead of each individual method/
            // Note this could get confusing...  Just trying it for now to keep the code simple.
            try
            {
                message = string.Empty;

                var client = new OA_Server { Url = options.EndPoint };
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

                // self-signed cert override
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                switch (options.Method)
                {
                    case "AssessmentLocationChangedNotification":
                        message = AssessmentLocationChangedNotification(client, options);
                        break;
                    case "AssignCrewsToOutage":
                        break;
                    case "AVLChangedNotification":
                        break;
                    case "CHEventNotification":
                        break;
                    case "CloseCalls":
                        break;
                    case "ConnectDisconnectChangedNotification":
                        break;
                    case "CustomerChangedNotification":
                        break;
                    case "DiscardOutage":
                        break;
                    case "GetActiveOutages":
                        message = GetActiveOutages(client);
                        break;
                    case "GetActiveCalls":
                        break;
                    case "GetAllActiveOutageEvents":
                        message = GetAllActiveOutageEvents(client);
                        break;
                    case "GetAllCircuitElements":
                        message = GetAllCircuitElements(client);
                        break;
                    case "GetAllConnectivity":
                        message = GetAllConnectivity(client);
                        break;
                    case "GetAllCrews":
                        break;
                    case "GetAllLoadFlowResults":
                        break;
                    case "GetAllShortCircuitAnalysisResults":
                        break;
                    case "GetChildCircuitElements":
                        break;
                    case "GetChildConnectivity":
                        break;
                    case "GetCircuitElementsNearLatLong":
                        break;
                    case "GetCustomerOutageHistory":
                        message = GetCustomerOutageHistory(client, options);
                        break;
                    case "GetCustomersAffectedByOutage":
                        message = GetCustomersAffectedByOutage(client, options);
                        break;
                    case "GetDeviceTreeConnectivity":
                        break;
                    case "GetDomainMembers":
                        break;
                    case "GetDomainNames":
                        break;
                    case "GetDownlineCircuitElements":
                        break;
                    case "GetDownlineConnectivity":
                        break;
                    case "GetDownlineMeterConnectivity":
                        break;
                    case "GetLoadFlowResultByObjectID":
                        break;
                    case "GetMeterConnectivityBySubstation":
                        break;
                    case "GetModifiedConnectivity":
                        break;
                    case "GetOutageDurationEvents":
                        message = GetOutageDurationEvents(client, options);
                        break;
                    case "GetOutageEvent":
                        break;
                    case "GetOutageEventStatus":
                        message = GetOutageEventStatus(client, options);
                        break;
                    case "GetOutageHistoryOnServiceLocation":
                        break;
                    case "GetOutageReasonCodes":
                        break;
                    case "GetOutageStatusByLocation":
                        message = GetOutageStatusByLocation(client, options);
                        break;
                    case "GetParentCircuitElements":
                        break;
                    case "GetParentConnectivity":
                        break;
                    case "GetShortCircuitAnalysisResultByObjectID":
                        break;
                    case "GetSiblingMeterConnectivity":
                        break;
                    case "GetSubstationNames":
                        break;
                    case "GetUplineCircuitElements":
                        break;
                    case "GetUplineConnectivity":
                        break;
                    case "GetUplineMeterConnectivity":
                        break;
                    case "MeterAddNotification":
                        break;
                    case "MeterChangedNotification":
                        break;
                    case "MeterExchangeNotification":
                        break;
                    case "MeterRemoveNotification":
                        break;
                    case "MeterRetireNotification":
                        break;
                    case "ODEventNotification":
                        message = SendOdEventNotification(client, options);
                        break;
                    case "OutageReasonChangedNotification":
                        break;
                    case "ResolvedCaller":
                        break;
                    case "RestoreOutage":
                        break;
                    case "SCADAAnalogChangedNotification":
                        break;
                    case "SCADAAnalogChangedNotificationByPointID":
                        break;
                    case "SCADAAnalogChangedNotificationForPower":
                        break;
                    case "SCADAAnalogChangedNotificationForVoltage":
                        break;
                    case "SCADAStatusChangedNotification":
                        break;
                    case "SCADAStatusChangedNotificationByPointID":
                        break;
                    case "ServiceLocationChangedNotification":
                        break;
                    case "UnassignCrewsFromOutage":
                        break;
                    case "UnassignOutagesFromCrew":
                        break;
                    case "UpdateMessageStatus":
                        break;

                    default:
                        Console.WriteLine($"MultiSpeakClient3AC {options.Method} not found in {options.Server}.");
                        Console.WriteLine("Check the list of methods in the README.md for each Server.");
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
        /// The assessment location changed notification.
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
        private static string AssessmentLocationChangedNotification(OA_Server client, Options options)
        {
            // Another way to teste for nulls is using null propagation
            // which is really confusing to everyone I bet at the beginning.
            // This one statement replaces the two below.
            if (options?.Location == null)
            {
                Console.WriteLine("AssessmentLocationChangedNotification is missing Options or Options.Location");
                return Fail;
            }

            /*if (options == null)
            {
                return Fail;
            }

            if (options.Location == null)
            {
                return Fail;
            }*/


            var locations = new[]
                                {
                                    new assessmentLocation
                                        {
                                            address =
                                                new address
                                                    {
                                                        address1 = "4111 Newton Ave",
                                                        address2 = "#16",
                                                        city = "Shreveport",
                                                        country = "USA",
                                                        postalCode = "71104",
                                                        state = "GA"
                                                    },
                                            AnyAttr = null,
                                            assessmentList =
                                                new[]
                                                    {
                                                        new assessment
                                                            {
                                                                jobNumber = "1",
                                                                AnyAttr = null,
                                                                category =
                                                                    string.Empty,
                                                                closedBy =
                                                                    string.Empty,
                                                                closedOn =
                                                                    DateTime.Now,
                                                                closedOnSpecified =
                                                                    true,
                                                                comments =
                                                                    "no comment",
                                                                created =
                                                                    DateTime.Now,
                                                                createdBy =
                                                                    "craig nicholson",
                                                                createdSpecified =
                                                                    true
                                                            }
                                                    },
                                            comments = "no comment",
                                            extensions = null,
                                            errorString = "put your errors here",
                                            extensionsList = null,
                                            facilityID = "12345",
                                            gpsLocation =
                                                new gpsLocation
                                                    {
                                                        collected = DateTime.Now,
                                                        collectedSpecified = true,
                                                        latitude = 93,
                                                        latitudeSpecified = true,
                                                        longitude = -87,
                                                        longitudeSpecified = true
                                                    },
                                            gridLocation = "98223280,89458945",
                                        }
                                };
            var transactionId = Guid.NewGuid().ToString();

            var response = client.AssessmentLocationChangedNotification(locations, transactionId);

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

        /// <summary>
        /// The get all connectivity returns a dump of the entire connectivity model.  This method
        /// requires multiple requests using the LastSent and ObjectsRemaining
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetAllConnectivity(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllConnectivity(lastReceived);

                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);
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

                XmlUtil.WriteToFile(xml, $"GetAllConnectivity.{objectsRemaining}", "3AC", logFileDirectory);
            }

            return Successfull;
        }

        /// <summary>
        /// The get active outages.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetActiveOutages(OA_Server client)
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

            PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
            XmlUtil.WriteToFile(xml, $"GetActiveOutages.{response.Length}", "3AC", logFileDirectory);

            return Successfull;
        }

        /// <summary>
        /// The get all active outage events.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetAllActiveOutageEvents(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllActiveOutageEvents(lastReceived);
                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);

                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetAllActiveOutageEvents objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine($"deviceid : {item.deviceID}");
                }

                Console.WriteLine($"GetAllActiveOutageEvents items in response : {response.Length}");
                var serializer = new XmlSerializer(typeof(outageEvent[]));
                string xml;
                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                XmlUtil.WriteToFile(xml, $"GetAllActiveOutageEvents.{objectsRemaining}", "3AC", logFileDirectory);
                PrintClassStdOut.PrintObject(
                    client.MultiSpeakMsgHeaderValue);
            }

            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetOutageDurationEvents(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return Fail;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return Fail;
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

            var serializer = new XmlSerializer(typeof(outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetOutageDurationEvents.{options.OutageEventId}", "3AC", logFileDirectory);
            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetOutageEventStatus(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                Console.WriteLine("GetOutageEventStatus takes the parameter OutageEventID : -o YourOutageEventId");
                return Fail;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventId");
                return Fail;
            }

            var response = client.GetOutageEventStatus(options.OutageEventId);
            Console.WriteLine($"comments : {response.comments} | errorString : {response.errorString}");
            Console.WriteLine($"objectID : {response.objectID} | outageStatus : {response.outageStatus}");

            var serializer = new XmlSerializer(typeof(outageEventStatus));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetOutageEventStatus.{options.OutageEventId}", "3AC", logFileDirectory);
            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetOutageStatusByLocation(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                Console.WriteLine("GetOutageStatusByLocation takes the parameter OutageEventID : -l YourLocationID");
                return Fail;
            }

            if (options.Location == null)
            {
                Console.WriteLine("CLI options.Location is missing, please add -l YourLocationID");
                return Fail;
            }

            var location = new outageLocation
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

            XmlUtil.WriteToFile(xml, $"GetOutageStatusByLocation.{options.Location}", "3AC", logFileDirectory);
            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetCustomerOutageHistory(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return Fail;
            }

            if (options.Account == null)
            {
                Console.WriteLine("CLI options.Account is missing, please add -a YourAccount");
                return Fail;
            }

            if (options.Location == null)
            {
                Console.WriteLine("CLI options.Location is missing, please add -l YourLocation");
                return Fail;
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

            var serializer = new XmlSerializer(typeof(outageDurationEvent[]));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetCustomerOutageHistory.{options.Account}.{options.Location}", "3AC", logFileDirectory);
            return Successfull;
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetCustomersAffectedByOutage(OA_Server client, Options options = null)
        {
            if (options == null)
            {
                return Fail;
            }

            if (options.OutageEventId == null)
            {
                Console.WriteLine("CLI options.OutageEventId is missing, please add -o YourOutageEventID");
                return Fail;
            }

            var response = client.GetCustomersAffectedByOutage(options.OutageEventId);

            var serializer = new XmlSerializer(typeof(customersAffectedByOutage));
            string xml;

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, response);
                    xml = sww.ToString();
                }
            }

            XmlUtil.WriteToFile(xml, $"GetCustomersAffectedByOutage.{options.OutageEventId}", "3AC", logFileDirectory);
            return Successfull;
        }

        /// <summary>
        /// The get all circuit elements.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetAllCircuitElements(OA_Server client)
        {
            // objectsRemaining and LastSent variables and re-send if required.
            var objectsRemaining = 1;
            var lastReceived = string.Empty;
            while (objectsRemaining > 0)
            {
                var response = client.GetAllCircuitElements(lastReceived);

                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
                lastReceived = client.MultiSpeakMsgHeaderValue.LastSent;
                int.TryParse(client.MultiSpeakMsgHeaderValue.ObjectsRemaining, out objectsRemaining);
                Console.WriteLine($"GetAllCircuitElements objectsRemaining : {objectsRemaining}");
                foreach (var item in response)
                {
                    Console.WriteLine(item.elementName);
                }

                var serializer = new XmlSerializer(typeof(circuitElement[]));
                string xml;

                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, response);
                        xml = sww.ToString();
                    }
                }

                XmlUtil.WriteToFile(xml, $"GetAllCircuitElements.{objectsRemaining}", "3AC", logFileDirectory);
                PrintClassStdOut.PrintObject(client.MultiSpeakMsgHeaderValue);
            }

            return Successfull;
        }

        #region SendCommands

        /// <summary>
        /// Send Outage Detection Event Notification
        /// </summary>
        /// <param name="client">
        /// Expects the OA_Server client
        /// </param>
        /// <param name="options">
        /// Requires Device option
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string SendOdEventNotification(OA_Server client, Options options = null)
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

                const string EventTypeError = "EventType is missing. Please review the required options for types of events.";
                if (options.EventType == null)
                {
                    Console.WriteLine(EventTypeError);
                    return Fail;
                }

                outageEventType outEventType;
                switch (options.EventType)
                {
                    case "Inferred":
                        outEventType = outageEventType.Inferred;
                        break;
                    case "Instantaneous":
                        outEventType = outageEventType.Instantaneous;
                        break;
                    case "Outage":
                        outEventType = outageEventType.Outage;
                        break;
                    case "NoResponse":
                        outEventType = outageEventType.NoResponse;
                        break;
                    case "PowerOff":
                        outEventType = outageEventType.PowerOff;
                        break;
                    case "PowerOn":
                        outEventType = outageEventType.PowerOn;
                        break;
                    case "Restoration":
                        outEventType = outageEventType.Restoration;
                        break;
                    default:
                        outEventType = outageEventType.Outage;
                        Console.WriteLine($"{options.EventType} not found. Defaulting to Outage. Did you mean Inferred,Instantaneous,Outage,NoResponse,PowerOff,PowerOn,Restoration?");
                        break;
                }

                var transactionId = Guid.NewGuid().ToString();
                var outages = new[]
                {
                    new outageDetectionEvent
                    {
                        eventTime = DateTime.UtcNow,
                        eventTimeSpecified = true,
                        outageEventType = outEventType,
                        outageEventTypeSpecified = true,
                        outageDetectDeviceID = options.Device,
                        outageDetectDeviceTypeSpecified = true,
                        outageDetectDeviceType = outageDetectDeviceType.Meter,
                        outageLocation = new outageLocation
                                             {
                            meterNo = options.Device
                        },
                        messageList = new[]
                        {
                            new message
                            {
                                comments = "PowerRestoredOutageEvent"
                            }
                        }
                    }
                };
                var response = client.ODEventNotification(outages, transactionId);
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

                XmlUtil.WriteToFile(xml, $"ODEventNotification.{options.Device}.{options.EventType}", "3AC", logFileDirectory);
                return xml;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        #endregion
    }
}