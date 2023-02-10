namespace HBH.FiltraCore
{
    public static class PropSearchFilterType
    {
        public const string Contains = "$contains";
        public const string NotContains = "$notContains";
        public const string Equal = "$eq";
        public const string NotEqual = "$ne";
        public const string StartWith = "$startsWith";
        public const string EndsWith = "$endsWith";
        public const string LessThan = "$lt";
        public const string LessThanOrEqual = "$lte";
        public const string GreaterThan = "$gt";
        public const string GreaterThanOrEqual = "$gte";
        public const string IsNull = "$null";
        public const string IsNotNull = "$notNull";
    }
}
