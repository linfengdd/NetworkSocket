using Microsoft.Extensions.Logging;
using NetworkSocket.Core;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi
{
    /// <summary>
    /// 与设备交互的服务类
    /// </summary>
    public class FastController : FastApiService
    {
        #region 属性
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<FastController> _logger;
        /// <summary>
        /// 获取其它已登录的会话
        /// </summary>
        public IEnumerable<FastSession> OtherSessions
        {
            get
            {
                return this
                    .CurrentContext
                    .FastSessions
                    .Where(item => item != this.CurrentContext.Session);
            }
        }
        #endregion


        #region 构造器
        /// <summary>
        /// 多参数构造器
        /// </summary>
        /// <param name="logger"></param>
        public FastController(ILogger<FastController> logger)
        {
            this._logger = logger;
        }
        #endregion


        /// <summary>
        /// 获取所有报告信息，测试
        /// </summary>
        /// <returns></returns>
        [Api("getAllReports")]
        public string GetAllReports() {
            _logger.LogInformation("调用了获取报告");
            return $"getAllReports";
        }

        

    }
}
