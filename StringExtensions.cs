namespace HBH.FiltraCore
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                case "": return "";
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
