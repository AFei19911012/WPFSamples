using HalconDotNet;
using Wpf_Base.HalconWpf.Views;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/05 11:13:08
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/05 11:13:08    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CMetrologyObjectParam
    {
        /// <summary>
        /// 测量参数
        /// </summary>
        public HTuple measure_length1 { get; set; }
        public HTuple measure_length2 { get; set; }
        public HTuple measure_sigma { get; set; }
        public HTuple measure_threshold { get; set; }
        public HTuple measure_select { get; set; }
        public HTuple measure_transition { get; set; }
        public HTuple measure_interpolation { get; set; }
        public HTuple min_score { get; set; }
        public HTuple distance_threshold { get; set; }

        public CMetrologyObjectParam(MetrologyObjectVM metroVM)
        {
            measure_length1 = metroVM.NumLength1;
            measure_length2 = metroVM.NumLength2;
            measure_sigma = metroVM.NumSigma;
            measure_threshold = metroVM.NumThreshold;
            measure_select = metroVM.StrSelectSelect;
            measure_transition = metroVM.StrSelectTransition;
            measure_interpolation = metroVM.StrSelectInterpolation;
            min_score = metroVM.NumMinScore;
            distance_threshold = metroVM.IntMinInstances;
        }
    }
}