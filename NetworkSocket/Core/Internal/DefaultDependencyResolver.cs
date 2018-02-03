using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#if !NET45
using Microsoft.Extensions.DependencyInjection;
#endif

namespace NetworkSocket.Core
{
    /// <summary>
    /// 默认的依赖关系解析程序的实现
    /// </summary>
    internal class DefaultDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// 注入上下文
        /// </summary>
        #if !NET45
        private IServiceScope _scope;
        #endif
        /// <summary>
        /// 解析支持任意对象创建的一次注册的服务
        /// </summary>
        /// <param name="serviceType">所请求的服务或对象的类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        /// <summary>
        /// 带参数的
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public object GetService(Type serviceType, IServiceProvider provider)
        {
            try
            {
#if NETCOREAPP
                if (null == provider) 
                    return GetService(serviceType);
                _scope = provider.CreateScope();
                return _scope.ServiceProvider.GetRequiredService(serviceType);
#else
                return GetService(serviceType);
#endif

            }
            catch (Exception ex) {
                throw ex;
            }
           
        }


        /// <summary>
        /// 结束服务实例的生命
        /// </summary>
        /// <param name="service">服务实例</param>
        public void TerminateService(IDisposable service)
        {
#if NETCOREAPP
            _scope?.Dispose();
#endif
            service.Dispose();
        }
    }
}
