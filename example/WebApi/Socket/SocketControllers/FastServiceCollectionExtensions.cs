using Microsoft.Extensions.DependencyInjection;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class FastServiceCollectionExtensions
    {
        /// <summary>
        /// IOC
        /// </summary>
        /// <param name="services"></param>
        public static void AddFastService(
            this IServiceCollection services)
        {
            //对外提供的服务
            foreach (var item in GetFastControllers()) {
                services.AddScoped(item);
                //增加Lazy注册方式
                //Type type = typeof(Lazy<>);
                //type = type.MakeGenericType(item);
                //services.AddScoped(type);
            }
        }
        /// <summary>
        /// 查询所有FastApiService
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<System.Type> GetFastControllers() {

            return Assembly.GetExecutingAssembly().GetTypes().Where(item => item.IsAbstract == false)
                .Where(item => typeof(FastApiService).IsAssignableFrom(item));
        }


    }
}