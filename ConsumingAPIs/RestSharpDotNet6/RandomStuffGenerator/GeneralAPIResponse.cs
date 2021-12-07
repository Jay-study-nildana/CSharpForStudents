﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RandomStuffGenerator;
//
//    var adminResponse = AdminResponse.FromJson(jsonString);

namespace RandomStuffGenerator.General
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using AllTheInterfaces.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GeneralAPIResponse : IResponse
    {
        [JsonProperty("listOfResponses")]
        public List<string> ListOfResponses { get; set; }

        [JsonProperty("dateTimeOfResponse")]
        public DateTimeOffset DateTimeOfResponse { get; set; }

        [JsonProperty("operationSuccessful")]
        public bool OperationSuccessful { get; set; }

        [JsonProperty("detailsAboutOperation")]
        public string DetailsAboutOperation { get; set; }
    }

    public partial class GeneralAPIResponse
    {
        public static GeneralAPIResponse FromJson(string json) => JsonConvert.DeserializeObject<GeneralAPIResponse>(json, RandomStuffGenerator.General.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GeneralAPIResponse self) => JsonConvert.SerializeObject(self, RandomStuffGenerator.General.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
