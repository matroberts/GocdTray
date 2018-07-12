namespace System
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            value = value.Trim();

            if (value.Length == 0)
            {
                throw new ArgumentException("Must specify valid information for parsing in the string.", "value");
            }

            Type t = typeof(T);

            if (!t.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", "T");
            }

            return (T)Enum.Parse(t, value, true);
        }
        public static T? ToEnumNullable<T>(this string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            else
                return value.ToEnum<T>();
        }
    }
}