using System.Threading;

namespace System
{
    internal sealed class SR
    {
        // Fields
        private static SR loader;

        internal const string WrongActionForCtor = "WrongActionForCtor";
        internal const string MustBeResetAddOrRemoveActionForCtor = "MustBeResetAddOrRemoveActionForCtor";
        internal const string ResetActionRequiresNullItem = "ResetActionRequiresNullItem";
        internal const string ResetActionRequiresIndexMinus1 = "ResetActionRequiresIndexMinus1";
        internal const string IndexCannotBeNegative = "IndexCannotBeNegative";
        internal const string ObservableCollectionReentrancyNotAllowed = "ObservableCollectionReentrancyNotAllowed";

        // Methods
        internal SR()
        {
        }

        private static SR GetLoader()
        {
            if (loader == null)
            {
                SR sr = new SR();
                Interlocked.CompareExchange<SR>(ref loader, sr, null);
            }
            return loader;
        }

        public static object GetObject(string name)
        {
            return name;
        }

        public static string GetString(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", @" $1");
        }

        public static string GetString(string name, out bool usedFallback)
        {
            usedFallback = false;
            return GetString(name);
        }

        public static string GetString(string name, params object[] args)
        {
            string format = GetString(name);
            if ((args == null) || (args.Length <= 0))
            {
                return format;
            }
            for (int i = 0; i < args.Length; i++)
            {
                string str2 = args[i] as string;
                if ((str2 != null) && (str2.Length > 0x400))
                {
                    args[i] = str2.Substring(0, 0x3fd) + "...";
                }
            }
            return string.Format(format, args);
        }
    }
}