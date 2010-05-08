using System;

namespace GherkinEditor
{
    public class Language : IEquatable<Language>
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

        public bool Equals(Language other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.IsoCode, IsoCode) && Equals(other.English, English) && Equals(other.Native, Native);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Language)) return false;
            return Equals((Language) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = IsoCode.GetHashCode();
                result = (result*397) ^ English.GetHashCode();
                result = (result*397) ^ Native.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Language left, Language right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Language left, Language right)
        {
            return !Equals(left, right);
        }
    }
}