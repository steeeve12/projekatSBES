using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [Serializable]
    [DataContract]
    public class SecurityEvent
    {

        public SecurityEvent(string clientId, string clientComputerName, DateTime timeStamp, int eventId, string eventDescription)
        {
            ClientId = clientId;
            ClientComputerName = clientComputerName;
            Timestamp = timeStamp;
            EventId = eventId;
            EventDescription = eventDescription;
        }

        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string ClientComputerName { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public string EventDescription { get; set; }
    }
}
