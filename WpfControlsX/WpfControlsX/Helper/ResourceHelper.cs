using System.Windows;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/18 23:04:36
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/18 23:04:36                     BigWang         首次编写         
    ///

    /// <summary>
    ///     资源帮助类
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        ///     获取资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetResource<T>(string key)
        {
            return Application.Current.TryFindResource(key) is T resource ? resource : default;
        }
    }
}