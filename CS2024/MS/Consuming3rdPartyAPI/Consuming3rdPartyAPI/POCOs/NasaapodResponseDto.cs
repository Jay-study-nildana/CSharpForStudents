﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Consuming3rdPartyAPI.POCOS.NASAAPOD;
//
//    var nasaapodResponseDto = NasaapodResponseDto.FromJson(jsonString);

namespace Consuming3rdPartyAPI.POCOS.NASAAPOD
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NasaapodResponseDto
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonProperty("hdurl")]
        public Uri Hdurl { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("service_version")]
        public string ServiceVersion { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class NasaapodResponseDto
    {
        public static NasaapodResponseDto FromJson(string json) => JsonConvert.DeserializeObject<NasaapodResponseDto>(json, Consuming3rdPartyAPI.POCOS.NASAAPOD.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NasaapodResponseDto self) => JsonConvert.SerializeObject(self, Consuming3rdPartyAPI.POCOS.NASAAPOD.Converter.Settings);
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