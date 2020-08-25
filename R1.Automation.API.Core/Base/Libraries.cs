using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Threading.Tasks;
using RestSharp.Serialization.Json;
using System.Text;

namespace R1.Automation.API.Core.Base
{
   public class Libraries
    {
        /// <summary>This method is used for initialized request with method</summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns>This returns rest request with url and method</returns>
        public IRestRequest GetRequest(string url, Method method)
        {
            switch (method)
            {
                case Method.GET:
                    return new RestRequest(url, Method.GET);
                case Method.POST:
                    return new RestRequest(url, Method.POST);
                case Method.PUT:
                    return new RestRequest(url, Method.PUT);
                case Method.PATCH:
                    return new RestRequest(url, Method.PATCH);
                case Method.DELETE:
                    return new RestRequest(url, Method.DELETE);
                default:
                    return null;
            }

        }

        /// <summary>This method is used for Add Path Parameter with request</summary>
        /// <param name="request"></param>
        /// <param name="pathParams"></param>
        public void AddPathParameter(IRestRequest request, Dictionary<String, String> pathParams)
        {
            foreach (var param in pathParams)
                request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);
        }

        /// <summary>This method is used for add body with request</summary>
        /// <param name="request"></param>
        /// <param name="addBody"></param>
        public void AddPostRequestBody(IRestRequest request, string addBody)
        {
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", addBody, ParameterType.RequestBody);

        }

        /// <summary>This method is used for add headers with request</summary>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        public void AddHeadersForGetPost(IRestRequest request, Dictionary<String, String> headers)
        {
            request.RequestFormat = DataFormat.Json;
            request.AddHeaders(headers);
        }



        /// <summary>This method is used for execute request</summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns>This return response</returns>
        public async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(RestClient client, IRestRequest request) where T : class, new()
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();

            client.ExecuteAsync<T>(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response.";
                    throw new ApplicationException(message, restResponse.ErrorException);
                }

                taskCompletionSource.SetResult(restResponse);
            });

            return await taskCompletionSource.Task;
        }

        /// <summary>This method is used for Deserialize Response</summary>
        /// <param name="restResponse"></param>
        /// <returns>This return Deserialized response</returns>
        public Dictionary<string, string> DeserializeResponse(IRestResponse restResponse)
        {
            var JSONObj = new JsonDeserializer().Deserialize<Dictionary<string, string>>(restResponse);

            return JSONObj;
        }

        /// <summary>This method is used for Get Response Object</summary>
        /// <param name="response"></param>
        /// <param name="responseObject"></param>
        /// <returns>This returns Response Object in string</returns>
        public string GetResponseObject(IRestResponse response, string responseObject)
        {
            JObject obs = JObject.Parse(response.Content);
            return obs[responseObject].ToString();
        }

        /// <summary>This method is used for add query parameter in request</summary>
        /// <param name="request"></param>
        /// <param name="pathParams"></param>
        public void AddQueryParameter(IRestRequest request, Dictionary<String, String> pathParams)
        {
            foreach (var param in pathParams)
                request.AddQueryParameter(param.Key, param.Value);
        }
    }
}
