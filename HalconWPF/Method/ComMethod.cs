namespace HalconWPF.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/10/13 10:57:39
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/10/13 10:57:39    Taosy.W                 
    ///
    public static class ComMethod
    {
        /// <summary>
        /// 一维数组求和
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Sum(this int[] data)
        {
            int result = 0;
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i];
            }
            return result;
        }

        /// <summary>
        /// 一维数组平均
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Mean(this int[] data)
        {
            double result = data.Sum();
            return result / data.Length;
        }
    }
}
