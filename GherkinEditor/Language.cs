namespace GherkinEditor
{
    public class Language
    {
        public Language(string iso_code, string english, string native)
        {
            IsoCode = iso_code;
            English = english;
            Native = native;
        }
        public string IsoCode { get; private set; }
        public string English { get; private set; }
        public string Native { get; private set; }
    }
}