﻿using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;

namespace E.Deezer.Tests.Utils
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported) { throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later."); }

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0) { throw new ArgumentException("prefixes"); }

            // A responder method is required
            if (method == null) { throw new ArgumentException("method"); }

            foreach (string s in prefixes) { _listener.Prefixes.Add(s); }

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes) : this(prefixes, method) { }

        public void Run()
        {
            Console.WriteLine("Webserver starting...");

            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _responderMethod(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);

                                //See if this fixes...
                                ctx.Response.AddHeader("Content-Type", "application/json;charset=utf-8");

                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            Console.WriteLine("Webserver stopping...");

            _listener.Stop();
            _listener.Close();

            Console.WriteLine("Webserver stopped.");
        }
    }
}