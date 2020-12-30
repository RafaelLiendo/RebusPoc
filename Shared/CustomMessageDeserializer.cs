
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rebus.Config;
using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Serialization;

namespace Shared
{
    public static class CustomMessageDeserializerExtension
    {
        public static OptionsConfigurer UseCustomMessageDeserializer(this OptionsConfigurer c, TopicsDictionary topicsDictionary)
        {
            c.Decorate<ISerializer>(c => new CustomMessageDeserializer(c.Get<ISerializer>(), topicsDictionary));
            return c;
        }
    }
    public class CustomMessageDeserializer : ISerializer
    {
        readonly ISerializer _serializer;
        readonly TopicsDictionary _topicsDictionary;

        public CustomMessageDeserializer(ISerializer serializer, TopicsDictionary topicsDictionary)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _topicsDictionary = topicsDictionary ?? throw new ArgumentNullException(nameof(topicsDictionary));
        }

        public Task<TransportMessage> Serialize(Message message) => _serializer.Serialize(message);

        public async Task<Message> Deserialize(TransportMessage transportMessage)
        {
            throw new NotImplementedException();
            //var headers = transportMessage.Headers.Clone();
            //var json = Encoding.UTF8.GetString(transportMessage.Body);
            //var typeName = headers.GetValue(Headers.Type);

            //// if we don't know the type, just deserialize the message into a JObject
            //if (!_topicsDictionary.TryGetValue(typeName, out var type))
            //{
            //    return new Message(headers, JsonConvert.DeserializeObject<JObject>(json));
            //}

            //var body = JsonConvert.DeserializeObject(json, type);

            //return new Message(headers, body);
        }
    }
}
