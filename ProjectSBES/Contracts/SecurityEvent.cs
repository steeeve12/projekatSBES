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
        /// <summary>
        /// Constructor used for creating a securitu event object
        /// </summary>
        /// <param name="serviceId"> id of a service who report a security event </param>
        /// <param name="serviceComputerName"> computer name of a machine where is run a service which reported a security event </param>
        /// <param name="clientId"> id of a client who tried to run a particular process </param>
        /// <param name="clientComputerName"> computer name of a machine where is run client which tried to run a particular process </param>
        /// <param name="timeStamp"> timestamp when the event is reported </param>
        /// <param name="eventId"> id of a forbidden process from a blacklist </param>
        /// <param name="eventDescription"> description of a security event </param>
        public SecurityEvent(string serviceId, string serviceComputerName, string clientId, string clientComputerName, DateTime timeStamp, int eventId, string eventDescription)
        {
            ServiceId = serviceId;
            ServiceComputerName = serviceComputerName;
            ClientId = clientId;
            ClientComputerName = clientComputerName;
            Timestamp = timeStamp;
            EventId = eventId;
            EventDescription = eventDescription;
        }

        [DataMember]
        public string ServiceId { get; set; }
        [DataMember]
        public string ServiceComputerName { get; set; }
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
