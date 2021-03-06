﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Cormo.Injects;
using Cormo.Injects.Exceptions;
using Cormo.Web.Api;

namespace Cormo.Web.Impl
{
    public class RouteParamProducer
    {
        [Produces, RouteParamAttribute]
        T GetRouteParam<T>(IInjectionPoint ip, HttpRequestMessage request)
        {
            if (ip == null)
                throw new InjectionException("RouteParam needs injection point");
            
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (request == null || converter == null)
                throw new UnsatisfiedDependencyException(ip);

            var name = GetRouteName(ip);
            var routeData = request.GetRouteData();
            if(routeData == null)
                throw new UnsatisfiedDependencyException(ip);

            object value;
            if(TryGetRouteValue(routeData, name, out value))
            {
                try
                {
                    return (T) converter.ConvertFromString(value.ToString());
                }
                catch (Exception e)
                {
                    // TODO log
                }
            }
            
            return GetDefaultValue<T>(ip);
        }

        private static bool TryGetRouteValue(IHttpRouteData routeData, string name, out object value)
        {
            if (routeData.Values.TryGetValue(name, out value))
                return true;

            var subroutes = routeData.GetSubRoutes();
            if (subroutes != null)
            {
                object val=null;
                if (subroutes.Select(x => x.Values).Any(x => x.TryGetValue(name, out val)))
                {
                    value = val;
                    return true;
                }
            }
            return false;
        }

        protected T GetDefaultValue<T>(IInjectionPoint ip)
        {
            var attrDefault = ip.Qualifiers.OfType<RouteParamAttribute>().Select(x => x.Default).OfType<T>().Take(1).ToArray();
            if (attrDefault.Any())
                return attrDefault[0];

            return default(T);
        }

        protected string GetRouteName(IInjectionPoint ip)
        {
            var attrName = ip.Qualifiers.OfType<RouteParamAttribute>().Select(x => x.Name).SingleOrDefault();
            if (string.IsNullOrEmpty(attrName))
            {
                var methodParam = ip as IMethodParameterInjectionPoint;
                if (methodParam != null)
                    return methodParam.ParameterInfo.Name;

                return ip.Member.Name;
            }
            return attrName;
        }
    }
}