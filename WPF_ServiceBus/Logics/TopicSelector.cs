using ServiceBus.Entities.Enums;
using ServiceBus.Entities.models;
using ServiceBus.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Functions
{
    public static class TopicSelector
    {

        public static Dictionary<int, Subscriptions> subscription = new Dictionary<int, Subscriptions>()
        {
            { 1, Subscriptions.ChannelOne },
            { 2, Subscriptions.ChannelTwo },
            { 3, Subscriptions.ChannelThree },
            { 4, Subscriptions.ChannelFour },
        };

        public static Subscriptions GetTopicSubscription(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.id == StaticResources.user.id)
                {
                    return subscription[player.orderNumber];
                }
            }

            return Subscriptions.Join;
        }
    }
}
