using Newtonsoft.Json;
using System;

namespace Demos.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/8/24 14:19:24
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2022/8/24 14:19:24    CoderMan/CoderMan1012                
    ///

    [Serializable]
    public class DataModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonIgnore]
        public string InspectResult { get; set; }
    }
}
