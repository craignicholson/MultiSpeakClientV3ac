// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintClassStdOut.cs" company="Craig">
//   blah
// </copyright>
// <summary>
//   Defines the PrintClassStdOut type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MultiSpeakClientV30ac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The print class standard out.
    /// </summary>
    public static class PrintClassStdOut
    {
        /// <summary>
        /// Print error objects. This can also be used to print out any object[] (object array).
        /// </summary>
        /// <param name="objects">
        /// The error objects.
        /// </param>
        public static void ErrorObjects(IEnumerable<object> objects)
        {
            foreach (var obj in objects)
            {
                Console.WriteLine($"{objects.GetType()} >");
                var t = obj.GetType();
                foreach (var pi in t.GetProperties().ToArray())
                {
                    var name = pi.Name;
                    var value = string.Empty;
                    if (pi.GetValue(obj) != null)
                    {
                        value = pi.GetValue(obj, null).ToString();
                    }

                    Console.WriteLine($"\t{name} : {value}");
                }
            }
        }

        /// <summary>
        /// PrintObject accepts a C# object and prints outs it's data.
        /// For object arrays object[] use the print error objects.
        /// Issues:  This will not print out ENUMS.
        /// </summary>
        /// <param name="obj">Object from the incoming caller.  Typically Option.cs and MultiSpeakMessageHeader</param>
        public static void PrintObject(object obj)
        {
            Console.WriteLine($"{obj.GetType()} >");
            var t = obj.GetType();
            foreach (var pi in t.GetProperties())
            {
                var name = pi.Name;
                var value = string.Empty;
                if (pi.GetValue(obj) != null)
                {
                    value = pi.GetValue(obj, null).ToString();
                }

                Console.WriteLine($"\t{name} : {value}");
            }
        }
    }
}
