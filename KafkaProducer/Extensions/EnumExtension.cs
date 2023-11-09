﻿#nullable disable


namespace KafkaProducer.Extensions
{
    public static class EnumExtensions
    {
        public static string Name<T>(this T obj)
        {
            return Enum.GetName(typeof(T), obj);
        }
    }
}
