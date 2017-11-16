using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class SecurityEvent
    {
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
