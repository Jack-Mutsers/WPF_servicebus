using Newtonsoft.Json;
using RestSharp;
using ServiceBus.Entities.Enums;
using ServiceBus.Entities.models;
using ServiceBus.Functions;
using ServiceBus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api
{
    public static class ApiConnector
    {
        public static ApiResponse CreateHost()
        {
            var client = new RestClient("http://localhost:5000/api/Hosting");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(StaticResources.user), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            apiResponse.topicData.subscription = TopicSelector.GetTopicSubscription(apiResponse.players.ToList());

            return apiResponse;
        }

        public static ApiResponse JoinHost()
        {
            var client = new RestClient("http://localhost:5000/api/Hosting/JoinGame?session_Code=" + StaticResources.sessionCode);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(StaticResources.user), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            apiResponse.topicData.subscription = TopicSelector.GetTopicSubscription(apiResponse.players.ToList());

            return apiResponse;
        }


    }
}
