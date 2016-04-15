using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace E.Deezer.Tests.Utils
{
    public class TestService
    {
        private const string HOST = "http://localhost";

        private short port;
        private WebServer server;
        private Dictionary<string, Func<Dictionary<string, string>, string>> endpoints;
        
        public TestService(short port)
        {
            this.port = port;
            endpoints = new Dictionary<string, Func<Dictionary<string, string>, string>>();

        }
    
        //Add an endpoint to the Test Service
        public bool RegisterEndpoint(string endpoint, Func<Dictionary<string, string>,string> handler)
        {
            if (endpoint.EndsWith("/")) { endpoint = endpoint.TrimEnd('/'); }

            if (endpoints.ContainsKey(endpoint)) { return false; }

            endpoints.Add(endpoint, handler);
            return true;
        }

        //Remove a registered endpoint
        public bool UnregisterEndpoint(string endpoint)
        {
            if(endpoints.ContainsKey(endpoint))
            {
                endpoints.Remove(endpoint);
                return true;
            }

            return false;
        }


        public void Start()
        {
            List<string> endpointStrings = new List<string>();

            foreach(string key in endpoints.Keys)
            {
                string url = string.Format("{0}:{1}/{2}/", HOST, port, key);
                endpointStrings.Add(url);
            }

            server = new WebServer(endpointStrings.ToArray(), DispatchHandlers);
            server.Run();
        }

        public void Stop()
        {
            server.Stop();
            server = null;
        }


        private string DispatchHandlers(HttpListenerRequest request)
        {
            string requestUrl = request.Url.LocalPath.Remove(0,1);

            if(endpoints.ContainsKey(requestUrl))
            {
                return endpoints[requestUrl](ProcessQueryString(request.QueryString));
            }

            return string.Empty;
        }


        private Dictionary<string, string> ProcessQueryString(NameValueCollection queryString)
        {
            Dictionary<string, string> qs = new Dictionary<string, string>();

            foreach(string key in queryString.AllKeys)
            {
                qs.Add(key, queryString.Get(key));
            }
            return qs;
        }
    }
}
