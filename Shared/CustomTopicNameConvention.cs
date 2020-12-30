using Rebus.Config;
using Rebus.Topic;
using System;
using System.ComponentModel;

namespace Shared
{
    public static class CustomTopicNameConventionExtension
    {
        public static void UseCustomTopicNameConvention(this OptionsConfigurer c, string prefix = null, string suffix = null)
        {
            c.Register<ITopicNameConvention>(c => new CustomTopicNameConvention(prefix, suffix));
        }
    }

    public class CustomTopicNameConvention : ITopicNameConvention
    {
        private readonly string _prefix;
        private readonly string _suffix;
       
        public CustomTopicNameConvention(string prefix = null, string suffix = null)
        {
            if(prefix != null && prefix.IndexOfAny(new char[] { '.', '+', '´' }) != -1)
            {
                throw new ArgumentException("O Azure Service Bus não suporta os caracteres ., +, ´ ");
            }

            if (suffix != null && suffix.IndexOfAny(new char[] { '.', '+', '´' }) != -1)
            {
                throw new ArgumentException("O Azure Service Bus não suporta os caracteres ., +, ´ ");
            }

           _prefix = prefix;
           _suffix = suffix;
        }

        public string GetTopic(Type eventType)
        {
            string eventName;
            var displayNameAnnotation = GetDisplayNameAnnotation(eventType);

            if (displayNameAnnotation != null)
            {
                eventName = displayNameAnnotation.DisplayName;
            }
            else if (eventType.IsNested)
            {
                eventName = eventType.DeclaringType.Name + eventType.Name;
            }
            else
            {
                eventName = eventType.Name;
            }

            return $"{_prefix}{eventName}{_suffix}";
        }

        private DisplayNameAttribute GetDisplayNameAnnotation(Type eventType)
        {
            return (DisplayNameAttribute) Attribute.GetCustomAttribute(eventType, typeof(DisplayNameAttribute));
        }
    }
}
