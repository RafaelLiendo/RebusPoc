using Rebus.Config;
using Rebus.Topic;
using System;
using System.ComponentModel;

namespace Shared
{
    public static class CustomTopicNameConventionExtension
    {
        public static OptionsConfigurer UseCustomTopicNameConvention(this OptionsConfigurer c, TopicsDictionary knownTypes =  null, string prefix = null, string suffix = null)
        {
            c.Register<ITopicNameConvention>(c => new CustomTopicNameConvention(knownTypes, prefix, suffix));
            return c;
        }
    }

    public class CustomTopicNameConvention : ITopicNameConvention
    {
        private readonly TopicsDictionary _knownTypes;
        private readonly string _prefix;
        private readonly string _suffix;

        public CustomTopicNameConvention(TopicsDictionary knownTypes = null, string prefix = null, string suffix = null)
        {
            if(prefix != null && prefix.IndexOfAny(new char[] { '.', '+', '´' }) != -1)
            {
                throw new ArgumentException("O Azure Service Bus não suporta os caracteres ., +, ´ ");
            }

            if (suffix != null && suffix.IndexOfAny(new char[] { '.', '+', '´' }) != -1)
            {
                throw new ArgumentException("O Azure Service Bus não suporta os caracteres ., +, ´ ");
            }

           _knownTypes = knownTypes;
           _prefix = prefix;
           _suffix = suffix;
        }

        public string GetTopic(Type eventType)
        {
            var topicName = (_knownTypes?.ContainsKey(eventType) ?? false)
                ? _knownTypes[eventType]
                : GetTopicByConvention(eventType);

            return topicName;
        }

        private string GetTopicByConvention(Type eventType)
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
