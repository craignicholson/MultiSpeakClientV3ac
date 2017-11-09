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

    using CommandLine;

    /// <summary>
    /// The main entry point for the program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The directory where the output files will be written, output being the results from Gets and Errors.
        /// TODO: FIX THIS - MAKE IT CONFIGURATION
        /// </summary>
        private const string FileDirectory = @"C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug\Milsoft";

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
        /// The logger we will use to log
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            var message = ProcessArgs(options);
            wallTime.Stop();

            // Console.WriteLine($"{options.Server} | {options.Method} |  {wallTime.Elapsed} | {Result} ");
            Log.Info($"HEADER | {options.Method}-{options.Server} | {wallTime.Elapsed}");
            if (message != "SUCCESS" || message != "FAIL")
            {
                message = "FAIL - " + message;
            }

            Log.Info($"Detail | Message | {message}");
            var t = options.GetType();
            foreach (var pi in t.GetProperties())
            {
                var name = pi.Name;
                var value = string.Empty;
                if (pi.GetValue(options) != null)
                {
                    value = pi.GetValue(options, null).ToString();
                }

                if (value != string.Empty)
                {
                    Log.Info($"Detail | {name} | {value}");
                }
            }

            Console.WriteLine("TASK FINISHED");
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
        /// ProcessArgs maps the Server to correct class and lets the class processes the arguments and execute the commands.
        /// </summary>
        /// <param name="options">
        /// Options are the the CLI options, you can run the program without options to view the help.
        /// </param>
        /// <returns>
        /// error string, which might be just a value and not an error <see cref="string"/>.
        /// </returns>
        private static string ProcessArgs(Options options)
        {
            var message = string.Empty;
            try
            {
                switch (options.Server)
                {
                    case "CB_Server":
                        {
                            CbServerRequests.SetLogFileDirectory(FileDirectory);
                            CbServerRequests.RunCommand(options, AppName, AppVersion, Version, out message);
                            break;
                        }

                    case "EA_Server":
                        {
                            EaServerRequests.SetLogFileDirectory(FileDirectory);
                            EaServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            throw new NotImplementedException($"{options.Server} Server is not available. Not Implmented in this MultiSpeakClientV3ac");
                        }

                    case "NOT_Server":
                        {
                            NotServerRequests.SetLogFileDirectory(FileDirectory);
                            NotServerRequests.RunCommand(options, AppName, AppVersion, Version, out message);
                            break;
                        }

                    case "MDM_Server":
                        {
                            MdmServerRequests.SetLogFileDirectory(FileDirectory);
                            MdmServerRequests.RunCommand(options, AppName, AppVersion, Version, out message);
                            break;
                        }

                    case "MR_Server":
                        {
                            MrServerRequests.SetLogFileDirectory(FileDirectory);
                            MrServerRequests.RunCommand(options, AppName, AppVersion, Version, out message);
                            break;
                        }

                    case "OA_Server":
                        {
                            OaServerRequests.SetLogFileDirectory(FileDirectory);
                            OaServerRequests.RunCommand(options, AppName, AppVersion, Version, out message);
                            break;
                        }

                    case "OD_Server":
                        {
                            OdServerRequests.SetLogFileDirectory(FileDirectory);
                            OdServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            throw new NotImplementedException($"{options.Server} Server is not available. You might have used the wrong server");
                        }

                    default:
                        Console.WriteLine($"{options.Server} not found. Did you mean OA_Server or MDM_Server?");
                        break;
                }

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MultiSpeakClientV30ac Error: {ex.Message}");
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception : {ex.InnerException.Message}");
                    message = ex.InnerException.Message;
                }

                PrintClassStdOut.PrintObject(options);
                return message;
            }
        }
    }
}