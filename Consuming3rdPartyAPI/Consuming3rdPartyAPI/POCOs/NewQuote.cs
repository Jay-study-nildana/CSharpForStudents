﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Consuming3rdPartyAPI.POCOs;
//
//    var newQuote = NewQuote.FromJson(jsonString);

namespace Consuming3rdPartyAPI.POCOs
{

    public partial class NewQuote
    {
        [JsonProperty("quoteAuthor")]
        public string QuoteAuthor { get; set; }

        [JsonProperty("quoteContent")]
        public string QuoteContent { get; set; }

        [JsonProperty("optionalAdditionalNotes")]
        public string OptionalAdditionalNotes { get; set; }
    }

    public partial class NewQuote
    {
        public static NewQuote FromJson(string json) => JsonConvert.DeserializeObject<NewQuote>(json, Consuming3rdPartyAPI.POCOs.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NewQuote self) => JsonConvert.SerializeObject(self, Consuming3rdPartyAPI.POCOs.Converter.Settings);
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

