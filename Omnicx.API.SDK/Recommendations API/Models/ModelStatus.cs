﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Omnicx.API.SDK.Recomendation.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for ModelStatus.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModelStatus
    {
        [EnumMember(Value = "Created")]
        Created,
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "Failed")]
        Failed
    }
}