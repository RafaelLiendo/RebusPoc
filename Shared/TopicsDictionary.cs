using System;
using System.Collections.Concurrent;

namespace Shared
{
    public class TopicsDictionary : ConcurrentDictionary<Type, string>
    {
    }
}
