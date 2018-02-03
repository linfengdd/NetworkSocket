
using Microsoft.Extensions.Logging;
using NetworkSocket.Core;
using NetworkSocket.Fast;
using Serilog;

namespace WebApi
{
    /// <summary>
    /// fast协议全局过滤器
    /// </summary>        
    public class FastGlobalFilter : FastFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public FastGlobalFilter() { 
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            // 标记已处理
            filterContext.ExceptionHandled = true;
        }
        /// <summary>
        /// 在执行Api行为前触发       
        /// </summary>
        /// <param name="filterContext">上下文</param>       
        /// <returns></returns>
        protected override void OnExecuting(ActionContext filterContext)
        {
            Log.Information($"接收到新的消息{filterContext.Action.ApiName}");
        }
    }
}