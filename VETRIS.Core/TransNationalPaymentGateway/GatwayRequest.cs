using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace VETRIS.Core.TransNationalPaymentGateway
{
    public class GatewayRequst
    {
        [JsonProperty("type")]
        [RegularExpression("sale|credit|recurring", ErrorMessage = "The type must be either 'sale' or 'credit' or 'recurring' only.")]
        public string type { get; set; }

        [JsonProperty("amount")]
        [Range(1, 1000000, ErrorMessage = "amount range is 1 to 1,000,000 cents i.e. US$ 0.01 to US$ 10,000.00.")]
        public int amount { get; set; }

        [JsonProperty("tax_amount")]
        public int tax_amount { get; set; }

        [JsonProperty("shipping_amount")]
        public int shipping_amount { get; set; }

        [JsonProperty("currency")]
        [Required]
        [RegularExpression("USD", ErrorMessage = "currency must be USD.")]
        public string currency { get; set; }

        [JsonProperty("description")]
        [MaxLength(255, ErrorMessage = "description should not exceed 255 characters.")]
        public string description { get; set; }

        [JsonProperty("order_id")]
        [Required]
        public string order_id { get; set; }

        [JsonProperty("po_number")]
        public string po_number { get; set; }

        [JsonProperty("ip_address")]
        [Required]
        public string ip_address { get; set; }

        [JsonProperty("email_receipt")]
        public bool email_receipt { get; set; }

        [JsonProperty("email_address")]
        public string email_address { get; set; }

        [JsonProperty("create_vault_record")]
        public bool create_vault_record { get; set; }

        [JsonProperty("payment_method")]
        [Required]
        public PaymentMethod payment_method { get; set; }

        [JsonProperty("billing_address")]
        public BillingAddress billing_address { get; set; }

        [JsonProperty("shipping_address")]
        public BillingAddress shipping_address { get; set; }
    }
    public class CardInput
    {
        [JsonProperty("entry_type")]
        [RegularExpression("keyed|swiped", ErrorMessage = "The entry_type must be either 'keyed' or 'swiped' only.")]
        public string entry_type { get; set; }

        [JsonProperty("number")]
        [RegularExpression(@"\d{16}", ErrorMessage = "number must be of 16 digis")]
        public string number { get; set; }

        [JsonProperty("expiration_date")]
        [RegularExpression(@"[0-1][0-2]\/\d{2}", ErrorMessage = "expiration_date must be in DD/YY format")]
        [MinLength(5, ErrorMessage = "expiration_date must be of 5 characters long"), MaxLength(5, ErrorMessage = "expiration_date must be of 5 characters long")]
        public string expiration_date { get; set; }

        [JsonProperty("cvc")]
        [RegularExpression(@"\d{3}", ErrorMessage = "cvc must be of 3 digis")]
        public string cvc { get; set; }
    }

    public class PaymentMethod
    {
        [JsonProperty("card")]
        [Required(ErrorMessage = "card is required.")]
        public CardInput card { get; set; }
    }
    public class BillingAddress
    {
        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("company")]
        public string company { get; set; }

        [JsonProperty("address_line_1")]
        public string address_line_1 { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("postal_code")]
        public string postal_code { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("fax")]
        public string fax { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }
    }

    

    
}
