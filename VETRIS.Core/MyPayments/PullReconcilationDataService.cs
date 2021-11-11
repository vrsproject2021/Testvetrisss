using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VETRIS.Core.TransNationalPaymentGateway;

namespace VETRIS.Core.MyPayments
{
    public class PullReconcilationDataService
    {
        public void Execute(string ConfigPath, ref string ReturnMessage, ref string CatchMessage,string api_key,DateTime fromDate,DateTime? toDate=null)
        {
            try
            {
                var api = new PaymentApi();
                api.API_Key = api_key;
                var data=api.TransactionsQuery(fromDate, toDate);
                if (data.Count == 0)
                    return;
                var core = new ARReconciliation();
                core.transactions = data;
                core.SaveRecord(ConfigPath, ref ReturnMessage, ref CatchMessage);
            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
            }
           
        }
    }
}
