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
            wallTime.Stop();
            
            // TODO: Save Server, Method, Elasped time to log file for performance monitoring
            // TODO: Figure out a way to PASS/FAIL all the methods.
            const string Result = "PASS";
            Console.WriteLine($"{Result} | {options.Server} | {options.Method} |  {wallTime.Elapsed} | {options.EndPoint}");
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
        /// <param name="options">Options are the the CLI options, you can run the program without options to view the help.</param>
        private static void ProcessArgs(Options options)
        {
            try
            {
                switch (options.Server)
                {
                    case "CB_Server":
                        {
                            CbServerRequests.SetLogFileDirectory(FileDirectory);
                            CbServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            throw new NotImplementedException($"{options.Server} Server is not available. You might have used the wrong server");
                        }

                    case "EA_Server":
                        {
                            EaServerRequests.SetLogFileDirectory(FileDirectory);
                            EaServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            throw new NotImplementedException($"{options.Server} Server is not available. You might have used the wrong server");
                        }

                    case "MDM_Server":
                        {
                            MdmServerRequests.SetLogFileDirectory(FileDirectory);
                            MdmServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            break;
                        }

                    case "MR_Server":
                        {
                            MrServerRequests.SetLogFileDirectory(FileDirectory);
                            MrServerRequests.RunCommand(options, AppName, AppVersion, Version);
                            break;
                        }

                    case "OA_Server":
                        {
                            OaServerRequests.SetLogFileDirectory(FileDirectory);
                            OaServerRequests.RunCommand(options, AppName, AppVersion, Version);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} - Occured");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception : {ex.InnerException.Message}");
                }

                PrintClassStdOut.PrintObject(options);
            }
        }
    }
}
