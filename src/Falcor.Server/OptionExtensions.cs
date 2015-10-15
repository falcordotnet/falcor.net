using System;

namespace Falcor.Server
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Gets the value or else returns a default value.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="option"></param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetOrElse<T>(this IOption<T> option, T defaultValue)
        {
            if (option == null) throw new ArgumentNullException("option");
            return option.IsEmpty ? defaultValue : option.Get();
        }
    }
}