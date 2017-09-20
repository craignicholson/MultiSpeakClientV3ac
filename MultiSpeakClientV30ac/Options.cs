// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Craig Nicholson">
//   MIT Lic
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using CommandLine;

    /// <summary>
    /// The options for the CLI Interface via NuGet CommandLineParser used to help manage the CLI Interface.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        [Option('e', "EndPoint", Required = true,
            HelpText = "Url or Endpoint you will send requests.  Example : http://server/OA_Server, etc...")]
        public string EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Option('u', "UserID", Required = true,
            HelpText = "UserID for the multispeak endpoint.")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Option('p', "Password", Required = true,
            HelpText = "Pwd (Password) for the multispeak endpoint.")]
        public string Pwd { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [Option('c', "Company", Required = true,
            HelpText = "Company is required since some vendors use Company for security validation.")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        [Option('s', "Server", Required = true,
            HelpText = "MDM_Server or OA_Server")]
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        [Option('m', "Method", Required = true,
            HelpText = "This is method PingUrl, GetActiveOutages, etc...")]
        public string Method { get; set; }

        // Optional Parameters 

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [Option('l', "Location", Required = false,
            HelpText = "Location or servLoc used in methods")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        [Option('a', "Account", Required = false,
            HelpText = "Account used in methods... xyz")]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the outage event id.
        /// </summary>
        [Option('o', "OutageEventId", Required = false,
            HelpText = "OutageEventId used specifically for a few methods... xyz")]
        public string OutageEventId { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        [Option('d', "Device", Required = false,
            HelpText = "Device is a meterNo, meterIdentifier, etc.. used in methods which require meterNo.")]
        public string Device { get; set; }

        /// <summary>
        /// Gets or sets the response url.
        /// </summary>
        [Option('r', "ResponseUrl", Required = false,
            HelpText = "ResponseUrl is used for Notifications repsonses, like InitiateOutageDetectionEventRequest which are asynchronous.")]
        public string ResponseUrl { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        [Option('v', "EventType", Required = false,
            HelpText = "EventType is used to ODEventNotification {Instantaneous, Outage, Restoration, NoResponse, Inferred, PowerOn, PowerOff}")]
        public string EventType { get; set; }

        // Omitting long name, default --verbose

        /// <summary>
        /// Gets or sets a value indicating whether verbose.
        /// </summary>
        [Option(
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
}
