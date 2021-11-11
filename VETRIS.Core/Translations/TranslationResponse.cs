using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.Core.Translations
{
    public partial class TranslationResponse
    {
        [JsonProperty("data")]
        public TranslationData Data { get; set; }
    }

    public partial class TranslationData
    {
        [JsonProperty("translations")]
        public TranslationText[] Translations { get; set; }
    }

    public partial class TranslationText
    {
        [JsonProperty("translatedText")]
        public string TranslatedText { get; set; }

        [JsonProperty("detectedSourceLanguage")]
        public string DetectedSourceLanguage { get; set; }
    }

    
}