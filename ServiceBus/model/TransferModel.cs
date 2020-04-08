using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.model
{
    public class TransferModel
    {
        public string sessionCode { get; set; }
        public string message { get; set; }
        public MessageType type { get; set; }

    }

    // define message type so the recievers know what to do with it
    public enum MessageType
    {
        JoinRequest, // only valid for the host
        Response, // only valid for guests
        Log,
        Action,
        ReadyUp
    }
}
