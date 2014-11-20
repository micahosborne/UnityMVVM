namespace System.Diagnostics.Contracts
{
    public static class Contract
    {
        public static void Assert(bool condition)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, null, null, null);
        }

        public static void Assert(bool condition, String userMessage)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
        }

        /// <summary>
        /// Without contract rewriting, failing Assert/Assumes end up calling this method.
        /// Code going through the contract rewriter never calls this method. Instead, the rewriter produced failures call
        /// System.Runtime.CompilerServices.ContractHelper.RaiseContractFailedEvent, followed by
        /// System.Runtime.CompilerServices.ContractHelper.TriggerFailure.
        /// </summary>
        private static void ReportFailure(ContractFailureKind failureKind, String userMessage, String conditionText, Exception innerException)
        {
        }

        public enum ContractFailureKind
        {
            Precondition,
            Postcondition,
            PostconditionOnException,
            Invariant,
            Assert,
            Assume,
        }
    }
}