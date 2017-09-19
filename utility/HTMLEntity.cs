namespace Utils
{
    public static class HTMLEntity
    {
        public static string XSSConvert(string oriHTML)
        {
            string convertHTML = oriHTML.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
                                .Replace("\"", "&quot;").Replace("'", "&#x27;").Replace("/", "&#x2f;");
            return convertHTML;
        }
        public static string XSSDeconvert(string oriHTML)
        {
            string convertHTML = oriHTML.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">")
                                .Replace("&quot;", "\"").Replace("&#x27;", "'").Replace("&#x2f;", "/");
            return convertHTML;
        }
    }
}