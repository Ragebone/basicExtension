using System.Runtime.InteropServices;
using System.Text;
using RGiesecke.DllExport;
using System.Collections.Generic;

namespace basicExtension
{
    public class Extension
    {
        private static StringBuilder outP;
        // Query dictionary.
        private delegate int Del(string[] a);
        private static readonly Dictionary<string, Del> querys = new Dictionary<string, Del> {
            {"test", test}
        };

        [DllExport("RVExtensionVersion", CallingConvention = CallingConvention.Winapi)]
        public static void RVExtensionVersion(StringBuilder output, int outputSize)
        {
            outputSize--;
            output.Append("R1");
        }

        [DllExport("RVExtensionArgs", CallingConvention = CallingConvention.Winapi)]
        public static int RVExtensionArgs(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 4)] string[] args, int argsCnt)
        {
            outputSize--;
            outP = output;
            output.Append("");

            Del method;
            if (querys.TryGetValue(function.ToLower(), out method))
            {
                return method(args);
            }
            return 0;
        }

        // works
        [DllExport("RVExtension", CallingConvention = CallingConvention.Winapi)]
        public static void RVExtension(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string id)
        {
            outputSize--;
            output.Append("not supported");
        }

        // Query Methods :
        private static int test(string[] id)
        {
            outP.Append("true");
            return 0;
        }
    }
}