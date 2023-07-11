namespace Todo.Domain.Extensions
{
    public static class UpdateValidationsExtension
    {
        public static string UpdateValidateString(this string update, string current)
        {
            if (string.IsNullOrEmpty(update))
            {
                return current;
            }

            return update;
        }

        public static int UpdateValidateInt(this int update, int current)
        {
            if (update <= 0 && (update != current))
            {
                return current;
            }

            return update;
        }
    }
}