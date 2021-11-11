using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VETRIS.Core.Translations
{
    public class GoogleTranslation
    {
        #region Translate
        public static string Translate(string src, string URL,string APIKey)
        {
            string strReturn = string.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var builder = new UriBuilder(URL);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["target"] = "en";
                    query["key"] = APIKey;
                    query["q"] = src;
                    builder.Query = query.ToString();

                    var response = client.GetAsync(builder.ToString()).Result;
                    var translations = JsonConvert.DeserializeObject<TranslationResponse>(response.Content.ReadAsStringAsync().Result);
                    if (translations != null)
                    {

                        if (translations.Data != null && translations.Data.Translations != null && translations.Data.Translations != null && translations.Data.Translations.Length > 0)
                        {
                            strReturn = translations.Data.Translations[0].TranslatedText;
                        }
                    }


                }
                catch (Exception ex)
                {
                    strReturn = ex.Message.Trim();
                }
            }

            return strReturn;
        } 
        #endregion

        #region GetSourceLanguage
        public static string GetSourceLanguage(string src, string URL, string APIKey)
        {
            string strReturn = string.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var builder = new UriBuilder(URL);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["target"] = "en";
                    query["key"] = APIKey;
                    query["q"] = src;
                    builder.Query = query.ToString();

                    var response = client.GetAsync(builder.ToString()).Result;
                    var language = JsonConvert.DeserializeObject<TranslationResponse>(response.Content.ReadAsStringAsync().Result);
                    if (language != null)
                    {

                        if (language.Data != null && language.Data.Translations != null && language.Data.Translations != null && language.Data.Translations.Length > 0)
                        {
                            strReturn = language.Data.Translations[0].DetectedSourceLanguage;
                        }
                    }


                }
                catch (Exception ex)
                {
                    strReturn = ex.Message.Trim();
                }
            }

            return strReturn;
        }
        #endregion
    }
}
