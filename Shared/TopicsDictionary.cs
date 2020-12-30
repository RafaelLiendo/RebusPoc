using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class TopicsDictionary : Dictionary<Type, string>
    {
    }

    public static class TopicsDictionaryExtension
    {
        public static Task Subscribe(this IBus bus, TopicsDictionary topicsDictionary)
        {
            return Task.WhenAll(
                topicsDictionary.Keys
                    .Select(topicType => bus.Subscribe(topicType))
                    .ToArray());
        }
    }
}
