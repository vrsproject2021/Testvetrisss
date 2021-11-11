using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QBFC13Lib;

namespace QBVetrisLib
{
    public class QBAccess
    {
        public static IMsgSetRequest CreateRequest(QBSessionManager sessionManager)
        {
            IMsgSetRequest request = sessionManager.CreateMsgSetRequest("US", 13, 0);
            request.Attributes.OnError = ENRqOnError.roeContinue;
            return request;
        }
        public static IMsgSetResponse DoRequest(QBSessionManager sessionManager, IMsgSetRequest request)
        {
            try
            {
                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "VetrisQuery");
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                //Send the request and get the response from QuickBooks
                IMsgSetResponse response = sessionManager.DoRequests(request);
                sessionManager.EndSession();
                sessionManager.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void BuildQueryAccount(IMsgSetRequest rq, params string[] names)
        {
            IAccountQuery q = rq.AppendAccountQueryRq();
            q.IncludeRetElementList.Add("ListID");
            q.IncludeRetElementList.Add("AccountNumber");
            q.IncludeRetElementList.Add("AccountType");
            q.IncludeRetElementList.Add("ParentRef");
            q.IncludeRetElementList.Add("Name");
            q.IncludeRetElementList.Add("FullName");
            foreach (var name in names)
            {
                if (!string.IsNullOrEmpty(name))
                    q.ORAccountListQuery.FullNameList.Add(name);

            }
            if (names == null || names.Length == 0)
            {
                q.ORAccountListQuery.AccountListFilter.MaxReturned.SetValue(10000);
            }

        }
        public static void BuildQueryAccount(IMsgSetRequest rq, string name, params ENAccountType[] types)
        {
            IAccountQuery q = rq.AppendAccountQueryRq();
            q.IncludeRetElementList.Add("ListID");
            q.IncludeRetElementList.Add("AccountNumber");
            q.IncludeRetElementList.Add("AccountType");
            q.IncludeRetElementList.Add("Name");
            q.IncludeRetElementList.Add("FullName");
            q.IncludeRetElementList.Add("ParentRef");
            if (!string.IsNullOrEmpty(name))
                q.ORAccountListQuery.FullNameList.Add(name);
            else
            {
                q.ORAccountListQuery.AccountListFilter.MaxReturned.SetValue(10000);
            }
            if (types != null && types.Length > 0)
            {
                foreach (var item in types)
                {
                    q.ORAccountListQuery.AccountListFilter.AccountTypeList.Add(item);
                }
            }

        }

        public static void BuildQueryCustomer(IMsgSetRequest rq, params string[] names)
        {
            ICustomerQuery q = rq.AppendCustomerQueryRq();
            q.IncludeRetElementList.Add("ListID");
            q.IncludeRetElementList.Add("AccountNumber");
            q.IncludeRetElementList.Add("Name");
            q.IncludeRetElementList.Add("FullName");
            q.IncludeRetElementList.Add("CompanyName");
            q.IncludeRetElementList.Add("IsActive");
            q.IncludeRetElementList.Add("EditSequence");

            foreach (var name in names)
            {
                if (!string.IsNullOrEmpty(name))
                    q.ORCustomerListQuery.FullNameList.Add(name);

            }
            if (names == null || names.Length == 0)
            {

                q.ORCustomerListQuery.CustomerListFilter.MaxReturned.SetValue(10000);
                q.ORCustomerListQuery.CustomerListFilter.ActiveStatus.SetValue(ENActiveStatus.asAll);
            }
        }
        /// <summary>
        /// Get GL Accounts from response
        /// </summary>
        /// <param name="response"> IMsgSetResponse type</param>
        /// <param name="error">Error message from Quickbooks</param>
        /// <returns>List of AccountEntity Object (only ListID, Name, FullName, AccountNumber will be returned)</returns>

        public static List<AccountEntity> getAccount(IMsgSetResponse response, ref string error)
        {
            string statusNode = "AccountQueryRs";
            List<AccountEntity> result = new List<AccountEntity>();
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return result;
            }

            result = doc.Descendants("AccountRet")
                    .Select(i => new AccountEntity
                    {
                        ListID = i.Element("ListID").Value,
                        Name = i.Element("Name").Value,
                        FullName = i.Element("FullName") != null ? i.Element("FullName").Value : null,
                        AccountNumber = i.Element("AccountNumber") != null ? i.Element("AccountNumber").Value : null,
                        AccountType = i.Element("AccountType").Value
                    })
                    .ToList();

            return result;

        }
        /// <summary>
        /// Get Customers/Billing Accounts from response
        /// </summary>
        /// <param name="response"> IMsgSetResponse type</param>
        /// <param name="error">Error message from Quickbooks</param>
        /// <returns>List of CustomerEntity Object (only ListID, Name, FullName,CompanyName, IsActive will be returned)</returns>
        public static List<CustomerEntity> getCustomer(IMsgSetResponse response, ref string error)
        {
            string statusNode = "CustomerQueryRs";
            List<CustomerEntity> result = new List<CustomerEntity>();
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return result;
            }


            result = doc.Descendants("CustomerRet")
                    .Select(i => new CustomerEntity
                    {
                        ListID = i.Element("ListID").Value,
                        Name = i.Element("Name").Value,
                        FullName = i.Element("FullName").Value,
                        IsActive = i.Element("IsActive").Value == "true",
                        CompanyName = i.Element("CompanyName").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .ToList();

            return result;

        }

        internal static JournalEntity getTransactionStatus(IMsgSetResponse response, ref string error)
        {
            string statusNode = "JournalEntryAddRs";
            JournalEntity result = null;
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && (statusCode.Value == "0" || statusCode.Value == "530")))
            {
                error = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return result;
            }
            result = doc.Descendants("JournalEntryRet")
                    .Select(i => new JournalEntity
                    {
                        TxnID = i.Element("TxnID").Value,
                        TimeCreated = Convert.ToDateTime(i.Element("TimeCreated").Value),
                        TimeModified = Convert.ToDateTime(i.Element("TimeModified").Value)
                    })
                    .FirstOrDefault();

            return result;
        }

        public static void BuildAddCustomer(IMsgSetRequest rq, CustomerEntity o)
        {
            
            ICustomerAdd co = rq.AppendCustomerAddRq();
            co.AccountNumber.SetValue(o.AccountNumber);
            co.Salutation.SetValue(o.Salutation);
            co.FirstName.SetValue(o.FirstName);
            co.LastName.SetValue(o.LastName);
            co.MiddleName.SetValue(o.MiddleName);
            co.Name.SetValue(o.Name);
            co.CompanyName.SetValue(o.CompanyName);
            co.Phone.SetValue(o.Phone);
            co.Email.SetValue(o.Email);
            co.ExternalGUID.SetValue(new Guid(o.ExternalGUID).ToString("B"));
            co.IsActive.SetValue(o.IsActive);

            if (o.BillAddress != null)
            {
                co.BillAddress.Addr1.SetValue(o.BillAddress.Address1);
                co.BillAddress.Addr2.SetValue(o.BillAddress.Address2);
                co.BillAddress.Addr3.SetValue(o.BillAddress.Address3);
                co.BillAddress.Addr4.SetValue(o.BillAddress.Address4);
                co.BillAddress.Addr5.SetValue(o.BillAddress.Address5);
                co.BillAddress.City.SetValue(o.BillAddress.City);
                co.BillAddress.State.SetValue(o.BillAddress.State);
                co.BillAddress.PostalCode.SetValue(o.BillAddress.PostalCode);
                co.BillAddress.Country.SetValue(o.BillAddress.Country);
            }
            if (o.ShipAddress != null)
            {
                co.ShipAddress.Addr1.SetValue(o.ShipAddress.Address1);
                co.ShipAddress.Addr2.SetValue(o.ShipAddress.Address2);
                co.ShipAddress.Addr3.SetValue(o.ShipAddress.Address3);
                co.ShipAddress.Addr4.SetValue(o.ShipAddress.Address4);
                co.ShipAddress.Addr5.SetValue(o.ShipAddress.Address5);
                co.ShipAddress.City.SetValue(o.ShipAddress.City);
                co.ShipAddress.State.SetValue(o.ShipAddress.State);
                co.ShipAddress.PostalCode.SetValue(o.ShipAddress.PostalCode);
                co.ShipAddress.Country.SetValue(o.ShipAddress.Country);
            }


        }
        public static void BuildModCustomer(IMsgSetRequest rq, CustomerEntity o)
        {
            ICustomerMod co = rq.AppendCustomerModRq();
            co.ListID.SetValue(o.ListID);
            co.EditSequence.SetValue(o.EditSequence);
            co.AccountNumber.SetValue(o.AccountNumber);
            co.Salutation.SetValue(o.Salutation);
            co.FirstName.SetValue(o.FirstName);
            co.LastName.SetValue(o.LastName);
            co.MiddleName.SetValue(o.MiddleName);
            co.Name.SetValue(o.Name);
            co.CompanyName.SetValue(o.CompanyName);
            co.Phone.SetValue(o.Phone);
            co.Email.SetValue(o.Email);
            co.IsActive.SetValue(o.IsActive);

            if (o.BillAddress != null)
            {
                co.BillAddress.Addr1.SetValue(o.BillAddress.Address1);
                co.BillAddress.Addr2.SetValue(o.BillAddress.Address2);
                co.BillAddress.Addr3.SetValue(o.BillAddress.Address3);
                co.BillAddress.Addr4.SetValue(o.BillAddress.Address4);
                co.BillAddress.Addr5.SetValue(o.BillAddress.Address5);
                co.BillAddress.City.SetValue(o.BillAddress.City);
                co.BillAddress.State.SetValue(o.BillAddress.State);
                co.BillAddress.PostalCode.SetValue(o.BillAddress.PostalCode);
                co.BillAddress.Country.SetValue(o.BillAddress.Country);
            }
            if (o.ShipAddress != null)
            {
                co.ShipAddress.Addr1.SetValue(o.ShipAddress.Address1);
                co.ShipAddress.Addr2.SetValue(o.ShipAddress.Address2);
                co.ShipAddress.Addr3.SetValue(o.ShipAddress.Address3);
                co.ShipAddress.Addr4.SetValue(o.ShipAddress.Address4);
                co.ShipAddress.Addr5.SetValue(o.ShipAddress.Address5);
                co.ShipAddress.City.SetValue(o.ShipAddress.City);
                co.ShipAddress.State.SetValue(o.ShipAddress.State);
                co.ShipAddress.PostalCode.SetValue(o.ShipAddress.PostalCode);
                co.ShipAddress.Country.SetValue(o.ShipAddress.Country);
            }


        }
        public static void CreateJournalRequest(IMsgSetRequest rq
            , string refNumber
            , DateTime date
            , bool isAdjustment
            , string Remarks
            , Action<IJournalEntryAdd> debits
            , Action<IJournalEntryAdd> credits)
        {
            IJournalEntryAdd je = rq.AppendJournalEntryAddRq();
            debits(je);
            credits(je);
            je.TxnDate.SetValue(date);
            je.RefNumber.SetValue(refNumber);
            je.IsAdjustment.SetValue(isAdjustment);
            je.Memo.SetValue(Remarks);
        }
        public static void BuildQueryJournal(IMsgSetRequest rq, DateTime? from, DateTime? to, params string[] txnIds)
        {
            IJournalEntryQuery q = rq.AppendJournalEntryQueryRq();
            q.IncludeRetElementList.Add("TxnID");
            q.IncludeRetElementList.Add("EditSequence");
            q.IncludeRetElementList.Add("TimeCreated");
            q.IncludeRetElementList.Add("TimeModified");
            q.IncludeRetElementList.Add("TxnNumber");
            q.IncludeRetElementList.Add("TxnDate");
            q.IncludeRetElementList.Add("RefNumber");
            q.IncludeRetElementList.Add("IsAdjustment");
            q.IncludeRetElementList.Add("Memo"); // memo does not return
            q.IncludeRetElementList.Add("JournalDebitLine");
            q.IncludeRetElementList.Add("JournalCreditLine");

            foreach (var txnId in txnIds)
            {
                if (!string.IsNullOrEmpty(txnId))
                    q.ORTxnQuery.RefNumberList.Add(txnId);

            }
            if (txnIds == null || txnIds.Length == 0)
            {

                q.ORTxnQuery.TxnFilter.MaxReturned.SetValue(10000);
                if (from.HasValue)
                    q.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(from.Value.Date);
                if (to.HasValue)
                    q.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(to.Value.Date);
            }
        }
        internal static List<JournalEntity> getTransactions(IMsgSetResponse response, ref string error)
        {
            string statusNode = "JournalEntryQueryRs";
            List<JournalEntity> result = new List<JournalEntity>();
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && (statusCode.Value == "0" || statusCode.Value == "530")))
            {
                error = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return result;
            }
            result = doc.Descendants("JournalEntryRet")
                    .Select(i => new JournalEntity
                    {
                        TxnID = i.Element("TxnID").Value,
                        TxnDate = new DateTime(Convert.ToInt32(i.Element("TxnDate").Value.Substring(0, 4)), Convert.ToInt32(i.Element("TxnDate").Value.Substring(5, 2)), Convert.ToInt32(i.Element("TxnDate").Value.Substring(8, 2))),
                        IsAdjustment = i.Element("IsAdjustment").Value == "true",
                        RefNumber = i.Element("RefNumber").Value,
                        TimeCreated = Convert.ToDateTime(i.Element("TimeCreated").Value),
                        TimeModified = Convert.ToDateTime(i.Element("TimeModified").Value),
                        Lines = i.Descendants("JournalDebitLine")
                                .Where(j => j.Element("Amount") != null)
                                .Select(d => new JournalDetailEntity
                                {
                                    CustomerFullName = d.Element("EntityRef").Element("FullName").Value,
                                    AccountNumber = d.Element("AccountRef").Element("FullName").Value,
                                    DebitAmount = Convert.ToDouble(d.Element("Amount").Value),
                                })
                                .ToList()
                                .Union(
                                    i.Descendants("JournalCreditLine")
                                    .Where(j => j.Element("Amount") != null)
                                    .Select(d => new JournalDetailEntity
                                    {
                                        CustomerFullName = d.Element("EntityRef").Element("FullName").Value,
                                        AccountNumber = d.Element("AccountRef").Element("FullName").Value,
                                        CreditAmount = Convert.ToDouble(d.Element("Amount").Value),
                                    })
                                    .ToList()
                                )
                                .ToList()
                    })
                    .ToList();

            return result;
        }

        public static void BuildQueryVendorByName(IMsgSetRequest rq, params string[] names)
        {
            IVendorQuery q = rq.AppendVendorQueryRq();
            q.IncludeRetElementList.Add("ListID");
            q.IncludeRetElementList.Add("AccountNumber");
            q.IncludeRetElementList.Add("Name");
            q.IncludeRetElementList.Add("FullName");
            q.IncludeRetElementList.Add("CompanyName");
            q.IncludeRetElementList.Add("IsActive");
            q.IncludeRetElementList.Add("EditSequence");

            foreach (var name in names)
            {
                if (!string.IsNullOrEmpty(name))
                    q.ORVendorListQuery.FullNameList.Add(name);

            }
            if (names == null || names.Length == 0)
            {

                q.ORVendorListQuery.VendorListFilter.MaxReturned.SetValue(10000);
                q.ORVendorListQuery.VendorListFilter.ActiveStatus.SetValue(ENActiveStatus.asAll);
            }
        }
        public static void BuildQueryVendorByListID(IMsgSetRequest rq, params string[] ListIDs)
        {
            IVendorQuery q = rq.AppendVendorQueryRq();
            q.IncludeRetElementList.Add("ListID");
            q.IncludeRetElementList.Add("AccountNumber");
            q.IncludeRetElementList.Add("Name");
            q.IncludeRetElementList.Add("FullName");
            q.IncludeRetElementList.Add("CompanyName");
            q.IncludeRetElementList.Add("IsActive");
            q.IncludeRetElementList.Add("EditSequence");

            foreach (var listID in ListIDs)
            {
                if (!string.IsNullOrEmpty(listID))
                    q.ORVendorListQuery.ListIDList.Add(listID);

            }
            if (ListIDs == null || ListIDs.Length == 0)
            {

                q.ORVendorListQuery.VendorListFilter.MaxReturned.SetValue(10000);
                q.ORVendorListQuery.VendorListFilter.ActiveStatus.SetValue(ENActiveStatus.asAll);
            }
        }

        public static void BuildAddRadiologist(IMsgSetRequest rq, VendorEntity v)
        {

           IVendorAdd vd = rq.AppendVendorAddRq();
            vd.AccountNumber.SetValue(v.AccountNumber);
            vd.Salutation.SetValue(v.Salutation);
            vd.FirstName.SetValue(v.FirstName);
            vd.LastName.SetValue(v.LastName);
            vd.MiddleName.SetValue(v.MiddleName);
            vd.Name.SetValue(v.Name);
            vd.CompanyName.SetValue(v.CompanyName);
            vd.Phone.SetValue(v.Phone);
            vd.Email.SetValue(v.Email);
            vd.ExternalGUID.SetValue(new Guid(v.ExternalGUID).ToString("B"));
            vd.IsActive.SetValue(v.IsActive);



            if (v.Address != null)
            {
                vd.VendorAddress.Addr1.SetValue(v.Address.Address1);
                vd.VendorAddress.Addr2.SetValue(v.Address.Address2);
                vd.VendorAddress.Addr3.SetValue(v.Address.Address3);
                vd.VendorAddress.Addr4.SetValue(v.Address.Address4);
                vd.VendorAddress.Addr5.SetValue(v.Address.Address5);
                vd.VendorAddress.City.SetValue(v.Address.City);
                vd.VendorAddress.State.SetValue(v.Address.State);
                vd.VendorAddress.PostalCode.SetValue(v.Address.PostalCode);
                vd.VendorAddress.Country.SetValue(v.Address.Country);
            }
            if (v.ShipAddress != null)
            {
                vd.ShipAddress.Addr1.SetValue(v.ShipAddress.Address1);
                vd.ShipAddress.Addr2.SetValue(v.ShipAddress.Address2);
                vd.ShipAddress.Addr3.SetValue(v.ShipAddress.Address3);
                vd.ShipAddress.Addr4.SetValue(v.ShipAddress.Address4);
                vd.ShipAddress.Addr5.SetValue(v.ShipAddress.Address5);
                vd.ShipAddress.City.SetValue(v.ShipAddress.City);
                vd.ShipAddress.State.SetValue(v.ShipAddress.State);
                vd.ShipAddress.PostalCode.SetValue(v.ShipAddress.PostalCode);
                vd.ShipAddress.Country.SetValue(v.ShipAddress.Country);
            }


        }

        public static void BuildModRadiologist(IMsgSetRequest rq, VendorEntity v)
        {
            IVendorMod vd = rq.AppendVendorModRq();
            vd.ListID.SetValue(v.ListID);
            vd.EditSequence.SetValue(v.EditSequence);
            vd.AccountNumber.SetValue(v.AccountNumber);
            vd.Salutation.SetValue(v.Salutation);
            vd.FirstName.SetValue(v.FirstName);
            vd.LastName.SetValue(v.LastName);
            vd.MiddleName.SetValue(v.MiddleName);
            vd.Name.SetValue(v.Name);
            vd.CompanyName.SetValue(v.CompanyName);
            vd.Phone.SetValue(v.Phone);
            vd.Email.SetValue(v.Email);
            vd.IsActive.SetValue(v.IsActive);

            if (v.Address != null)
            {
                vd.VendorAddress.Addr1.SetValue(v.Address.Address1);
                vd.VendorAddress.Addr2.SetValue(v.Address.Address2);
                vd.VendorAddress.Addr3.SetValue(v.Address.Address3);
                vd.VendorAddress.Addr4.SetValue(v.Address.Address4);
                vd.VendorAddress.Addr5.SetValue(v.Address.Address5);
                vd.VendorAddress.City.SetValue(v.Address.City);
                vd.VendorAddress.State.SetValue(v.Address.State);
                vd.VendorAddress.PostalCode.SetValue(v.Address.PostalCode);
                vd.VendorAddress.Country.SetValue(v.Address.Country);
            }
            if (v.ShipAddress != null)
            {
                vd.ShipAddress.Addr1.SetValue(v.ShipAddress.Address1);
                vd.ShipAddress.Addr2.SetValue(v.ShipAddress.Address2);
                vd.ShipAddress.Addr3.SetValue(v.ShipAddress.Address3);
                vd.ShipAddress.Addr4.SetValue(v.ShipAddress.Address4);
                vd.ShipAddress.Addr5.SetValue(v.ShipAddress.Address5);
                vd.ShipAddress.City.SetValue(v.ShipAddress.City);
                vd.ShipAddress.State.SetValue(v.ShipAddress.State);
                vd.ShipAddress.PostalCode.SetValue(v.ShipAddress.PostalCode);
                vd.ShipAddress.Country.SetValue(v.ShipAddress.Country);
            }


        }

        public static List<VendorEntity> getRadiologist(IMsgSetResponse response, ref string error)
        {
            string statusNode = "VendorQueryRs";
            List<VendorEntity> result = new List<VendorEntity>();
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf(statusNode).Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return result;
            }


            result = doc.Descendants("VendorRet")
                    .Select(i => new VendorEntity
                    {
                        ListID = i.Element("ListID").Value,
                        Name = i.Element("Name").Value,
                        FullName = i.Element("FullName").Value,
                        IsActive = i.Element("IsActive").Value == "true",
                        CompanyName = i.Element("CompanyName").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .ToList();

            return result;

        }
    }
}
