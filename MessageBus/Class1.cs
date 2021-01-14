using System;
using System.Threading.Tasks;

namespace RebusExtensions
{
    public abstract class IBusExtensions
    {
        public Task Publish<TMessage>(string topicName, TMessage message);
        
    }
}
