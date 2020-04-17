using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.Enums
{
    public enum PlayerAction
    {
        shoot,
        surender
    }

    // define Player type to know what to do with the message
    public enum PlayerType
    {
        Host,
        Guest
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

    // define alle availible subscriptions
    public enum Subscriptions
    {
        ChannelOne,
        ChannelTwo,
        ChannelThree,
        ChannelFour,
        Join
    }
}
