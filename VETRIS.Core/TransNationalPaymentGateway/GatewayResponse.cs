using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.TransNationalPaymentGateway
{
   

    public class CardInfo
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("card_type")]
        public string card_type { get; set; }

        [JsonProperty("first_six")]
        public string first_six { get; set; }

        [JsonProperty("last_four")]
        public string last_four { get; set; }

        [JsonProperty("masked_card")]
        public string masked_card { get; set; }

        [JsonProperty("expiration_date")]
        public string expiration_date { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("auth_code")]
        public string auth_code { get; set; }

        [JsonProperty("processor_response_code")]
        public string processor_response_code { get; set; }

        [JsonProperty("processor_response_text")]
        public string processor_response_text { get; set; }

        [JsonProperty("processor_type")]
        public string processor_type { get; set; }

        [JsonProperty("processor_id")]
        public string processor_id { get; set; }

        [JsonProperty("avs_response_code")]
        public string avs_response_code { get; set; }

        [JsonProperty("cvv_response_code")]
        public string cvv_response_code { get; set; }

        [JsonProperty("processor_specific")]
        public ProcessorSpecific processor_specific { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("amount")]
        public decimal amount { get; set; }

        [JsonProperty("tax_amount")]
        public int tax_amount { get; set; }

        [JsonProperty("tax_exempt")]
        public bool tax_exempt { get; set; }

        [JsonProperty("shipping_amount")]
        public int shipping_amount { get; set; }

        [JsonProperty("discount_amount")]
        public int discount_amount { get; set; }

        [JsonProperty("payment_adjustment_type")]
        public string payment_adjustment_type { get; set; }

        [JsonProperty("payment_adjustment_value")]
        public int payment_adjustment_value { get; set; }

        [JsonProperty("currency")]
        public string currency { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("order_id")]
        public string order_id { get; set; }

        [JsonProperty("po_number")]
        public string po_number { get; set; }

        [JsonProperty("ip_address")]
        public string ip_address { get; set; }

        [JsonProperty("email_receipt")]
        public bool email_receipt { get; set; }

        [JsonProperty("email_address")]
        public string email_address { get; set; }

        [JsonProperty("payment_method")]
        public string payment_method { get; set; }

        [JsonProperty("response")]
        public ResponseInfo response { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("response_code")]
        public int response_code { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("billing_address")]
        public BillingAddress billing_address { get; set; }

        [JsonProperty("shipping_address")]
        public BillingAddress shipping_address { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }
    }

    public class ProcessorSpecific
    {
    }

    public class ResponseInfo
    {
        [JsonProperty("card")]
        public CardInfo card { get; set; }
    }

    public class GatewayResponse
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("data")]
        public Data data { get; set; }

        public bool success()
        {
            return this.data.response_code == 100 || this.data.response_code == 110 || this.data.response_code == 101;
        }
    }
}
