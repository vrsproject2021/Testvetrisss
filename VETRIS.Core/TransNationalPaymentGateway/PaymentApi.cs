using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using VETRIS.Core.MyPayments;

namespace VETRIS.Core.TransNationalPaymentGateway
{
    public class PaymentApi
    {
        public string production_url { get; set; } //"https://secure.tnbcigateway.com/api/transact.php";
        public string API_Key { get; set; }
        
        #region Response Codes
        private readonly Dictionary<string, string> responseCodes = new Dictionary<string, string> { { "0", "Unknown"},
                { "99", "Pending payment - used in redirect processors prior to payment being received"},
                { "100", "Approved"},
                { "110", "Partial approval"},
                { "101", "Approved, pending customer approval"},
                { "200", "Decline"},
                { "201", "Do not honor"},
                { "202", "Insufficient funds"},
                { "203", "Excededs withdrawl limit"},
                { "204", "Invalid Transaction"},
                { "220", "Invalid Amount"},
                { "221", "No such Issuer"},
                { "222", "No credit Acct"},
                { "223", "Expired Card"},
                { "225", "Invalid CVC"},
                { "226", "Cannot Verify Pin"},
                { "240", "Refer to issuer"},
                { "250", "Pick up card (no fraud)"},
                { "251", "Lost card, pick up (fraud account)"},
                { "252", "Stolen card, pick up (fraud account)"},
                { "253", "Pick up card, special condition"},
                { "261", "Stop recurring"},
                { "262", "Stop recurring"},
                { "300", "Gateway Decline"},
                { "310", "Gateway Decline - Rule Engine"},
                { "400", "Transaction error returned by processor"},
                { "410", "Invalid merchant configuration"},
                { "421", "Communication error with processor"},
                { "430", "Duplicate transaction at processor"},
                { "440", "Processor Format error"}
            };
        #endregion

        public Dictionary<string, string> OnlineDirectPay(
            string payment_token,
            string type,
            decimal amount,
            string firstname, 
            string lastname, 
            string address1, 
            string address2, 
            string city, 
            string state, 
            string country, 
            string zip,
            string customer_vault=null,
            string customer_vault_id=null
            )
        {
            Dictionary<string, string> _response = null;
            using (var client = new HttpClient())
            {
                try
                {
                    
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                    var builder = new UriBuilder(production_url);
                    
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "security_key", API_Key},
                        { "payment_token", payment_token },
                        { "type", type },
                        { "amount", string.Format("{0:0.00}", amount) },
                        { "firstname", firstname }, 
                        { "lastname", lastname }, 
                        { "address1", address1 }, 
                        { "address2", address2 }, 
                        { "city", city }, 
                        { "state", state }, 
                        { "country", country }, 
                        { "zip", zip },
                        { "customer_vault", customer_vault },
                        { "customer_vault_id", customer_vault_id }
                    });
                    
                    var response = client.PostAsync(builder.ToString(),content).Result;

                    var ret = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        _response = new Dictionary<string, string> {{"status","forbidden" }};
                    }
                    else
                    {
                        _response = Regex.Matches(ret, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
            }
            return _response;
        }
        public Dictionary<string, string> OnlineDirectPay(
            string type,
            decimal amount,
            string currency,
            string cvv,
            string customer_vault_id
            )
        {
            Dictionary<string, string> _response = null;
            using (var client = new HttpClient())
            {
                try
                {

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                    var builder = new UriBuilder(production_url);

                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "security_key", API_Key},
                        { "type", type },
                        { "amount", Convert.ToString(amount) },
                        { "cvv", cvv },
                        { "currency", "USD" }, 
                        { "customer_vault_id", customer_vault_id }
                    });

                    var response = client.PostAsync(builder.ToString(), content).Result;

                    var ret = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        _response = new Dictionary<string, string> { { "status", "forbidden" } };
                    }
                    else
                    {
                        _response = Regex.Matches(ret, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
            }
            return _response;
        }

        
        public Dictionary<string, string> OnlineDirectRefund(
            string transactionid,
            decimal amount,
            string payment="creditcard"
            )
        {
            Dictionary<string, string> _response = null;
            using (var client = new HttpClient())
            {
                try
                {

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                    var builder = new UriBuilder(production_url);

                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        { "security_key", API_Key},
                        { "type", "refund" },
                        { "amount", Convert.ToString(amount) },
                        { "transactionid", transactionid },
                        { "payment", payment }
                    });

                    var response = client.PostAsync(builder.ToString(), content).Result;

                    var ret = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        _response = new Dictionary<string, string> { { "status", "forbidden" } };
                    }
                    else
                    {
                        _response = Regex.Matches(ret, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
            }
            return _response;
        }

        
        
    }
}
