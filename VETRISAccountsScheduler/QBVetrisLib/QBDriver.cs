using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QBVetrisLib
{
    public class QBDriver : IDisposable
    {
        private QBSessionManager sessionManager = null;
        public QBDriver()
        {
            sessionManager = new QBSessionManager();
        }

        public CustomerEntity CreateBillingAccount(CustomerEntity customer, ref string error)
        {
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildAddCustomer(request, customer);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf("CustomerAddRs").Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf("CustomerAddRs").Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return null;
            }
            var result = doc.Descendants("CustomerRet")
                    .Select(i => new CustomerEntity
                    {
                        ListID = i.Element("ListID").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .FirstOrDefault();
            customer.ListID = result.ListID;
            customer.EditSequence = result.EditSequence;

            return customer;
        }
        public bool UpdateBillingAccount(CustomerEntity customer, ref string error)
        {
            if (string.IsNullOrEmpty(customer.ListID))
            {
                error = "Please supply ListID";
                return false;
            }
            if (string.IsNullOrEmpty(customer.EditSequence))
            {
                error = "Please supply EditSequence";
                return false;
            }
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildModCustomer(request, customer);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf("CustomerModRs").Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf("CustomerModRs").Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return false;
            }
            var result = doc.Descendants("CustomerRet")
                    .Select(i => new CustomerEntity
                    {
                        ListID = i.Element("ListID").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .FirstOrDefault();
            customer.ListID = result.ListID;
            customer.EditSequence = result.EditSequence;

            return true;
        }

        public bool IsBillingAccountExists(string fullname, ref string error)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryCustomer(request, fullname);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var customers = QBAccess.getCustomer(response, ref error);
            return customers.Count > 0;
        }

        public bool IsVendorNameExists(string fullname, ref string error)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryVendorByName(request, fullname);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var customers = QBAccess.getCustomer(response, ref error);
            return customers.Count > 0;
        }
        public bool IsVendorListIDExists(string listID, ref string error)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryVendorByName(request, listID);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var customers = QBAccess.getCustomer(response, ref error);
            return customers.Count > 0;
        }

        public VendorEntity CreateRadiologist(VendorEntity radiologist, ref string error)
        {
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildAddRadiologist(request, radiologist);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf("VendorAddRs").Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf("VendorAddRs").Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return null;
            }
            var result = doc.Descendants("VendorRet")
                    .Select(i => new CustomerEntity
                    {
                        ListID = i.Element("ListID").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .FirstOrDefault();
            radiologist.ListID = result.ListID;
            radiologist.EditSequence = result.EditSequence;

            return radiologist;
        }
        public bool UpdateRadiologist(VendorEntity radiologist, ref string error)
        {
            if (string.IsNullOrEmpty(radiologist.ListID))
            {
                error = "Please supply ListID";
                return false;
            }
            if (string.IsNullOrEmpty(radiologist.EditSequence))
            {
                error = "Please supply EditSequence";
                return false;
            }
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildModRadiologist(request, radiologist);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            XElement doc = XElement.Parse(response.ToXMLString());
            var statusCode = doc.DescendantsAndSelf("radiologistModRs").Attributes().FirstOrDefault(i => i.Name == "statusCode");
            if (!(statusCode != null && statusCode.Value == "0"))
            {
                error = doc.DescendantsAndSelf("radiologistModRs").Attributes().FirstOrDefault(i => i.Name == "statusMessage").Value;
                return false;
            }
            var result = doc.Descendants("radiologistRet")
                    .Select(i => new VendorEntity
                    {
                        ListID = i.Element("ListID").Value,
                        EditSequence = i.Element("EditSequence").Value
                    })
                    .FirstOrDefault();
            radiologist.ListID = result.ListID;
            radiologist.EditSequence = result.EditSequence;

            return true;
        }

        public List<VendorEntity> getRadiologists(ref string error, params string[] names)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryVendorByName(request, names);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var radiologists = QBAccess.getRadiologist(response, ref error);
            return radiologists;
        }

        public List<CustomerEntity> getBillingAccounts(ref string error, params string[] names)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryCustomer(request, names);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var customers = QBAccess.getCustomer(response, ref error);
            return customers;
        }
        public bool IsAccountExists(string accountNumber, ref string error)
        {

            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryAccount(request, accountNumber);
            //Send the request and get the response from QuickBooks
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var accounts = QBAccess.getAccount(response, ref error);
            return accounts.Count > 0;
        }

        public bool CreateJournal(JournalEntity jr, ref string error)
        {
            var bReturn = false;

            // check for errors
            var lineNo = 0;
            var err = "";
            jr.Lines.ForEach(i =>
            {
                if (i.IsValid)
                {
                    err += string.IsNullOrEmpty(err) ? "" : "\n" + string.Format("Line No. {0}: {1}", lineNo, i.ErrorMessage);
                }
                lineNo++;
            });

            if (!string.IsNullOrEmpty(err))
            {
                error = err;
                bReturn = false;
                return false;
            }

            var drLines = jr.Lines.Where(i => i.DebitAmount > 0).ToList();
            var crLines = jr.Lines.Where(i => i.CreditAmount > 0).ToList();
            double dr = drLines.Sum(i => i.DebitAmount) ?? 0;
            double cr = crLines.Sum(i => i.CreditAmount) ?? 0;
            dr = Math.Round(dr, 2);
            cr = Math.Round(cr, 2);

            if (dr != cr)
            {
                error = "Debit and Credit mismatch.";
                return bReturn;
            }

            var accounts = jr.Lines
                      .Select(i => i.AccountNumber).Distinct().ToList()
                      .Select(i => new AccountListIDPair { AccountNumber = i })
                      .ToList();
            var customers = jr.Lines
                      .Select(i => i.CustomerFullName).Distinct().ToList()
                      .Select(i => new CustomerListIDPair { FullName = i })
                      .ToList();

            //find customers
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryCustomer(request, customers.Select(i => i.FullName).ToArray());
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var customerData = QBAccess.getCustomer(response, ref error);
            customerData.ForEach(c =>
            {
                var cc = customers.FirstOrDefault(i => i.FullName == c.FullName);
                if (cc != null) cc.ListID = c.ListID;
            });
            //find accounts
            request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryAccount(request, accounts.Select(i => i.AccountNumber).ToArray());
            response = QBAccess.DoRequest(sessionManager, request);
            var accountsData = QBAccess.getAccount(response, ref error);
            accountsData.ForEach(a =>
            {
                var aa = accounts.FirstOrDefault(i => i.AccountNumber == a.AccountNumber);
                if (aa != null) aa.ListID = a.ListID;
            });

            // process header
            request = QBAccess.CreateRequest(sessionManager);
            QBAccess.CreateJournalRequest(request, jr.RefNumber, jr.TxnDate, jr.IsAdjustment, jr.Remarks,
                (hdr =>
                { // debits
                    drLines.ForEach(line =>
                    {
                        var acc = accounts.FirstOrDefault(i => i.AccountNumber == line.AccountNumber);
                        var cust = customers.FirstOrDefault(i => i.FullName == line.CustomerFullName);

                        IORJournalLine jline = hdr.ORJournalLineList.Append();
                        jline.JournalDebitLine.AccountRef.ListID.SetValue(acc.ListID);
                        jline.JournalDebitLine.Memo.SetValue(line.Remarks);
                        jline.JournalDebitLine.EntityRef.ListID.SetValue(cust.ListID);
                        jline.JournalDebitLine.Amount.SetValue((double)line.DebitAmount.Value);
                    });

                }),
                (hdr =>
                { // credits
                    crLines.ForEach(line =>
                    {
                        var acc = accounts.FirstOrDefault(i => i.AccountNumber == line.AccountNumber);
                        var cust = customers.FirstOrDefault(i => i.FullName == line.CustomerFullName);

                        IORJournalLine jline = hdr.ORJournalLineList.Append();
                        jline.JournalCreditLine.AccountRef.ListID.SetValue(acc.ListID);
                        jline.JournalCreditLine.Memo.SetValue(line.Remarks);
                        jline.JournalCreditLine.EntityRef.ListID.SetValue(cust.ListID);
                        jline.JournalCreditLine.Amount.SetValue((double)line.CreditAmount.Value);
                    });
                }));
            response = QBAccess.DoRequest(sessionManager, request);
            var status = QBAccess.getTransactionStatus(response, ref error);
            if (status != null)
            {
                jr.TxnID = status.TxnID;
                jr.TimeCreated = status.TimeCreated;
                jr.TimeModified = status.TimeModified;
                bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// Get List of Journal Entry
        /// </summary>
        /// <param name="error"></param>
        /// <param name="fromDate">From date (can be null)</param>
        /// <param name="toDate">To date (can be null)</param>
        /// <param name="txnIds">List of transaction Id</param>
        /// <returns></returns>
        public List<JournalEntity> GetJournlEntries(ref string error, DateTime? fromDate, DateTime? toDate, params string[] txnIds)
        {
            IMsgSetRequest request = QBAccess.CreateRequest(sessionManager);
            QBAccess.BuildQueryJournal(request, fromDate, toDate, txnIds);
            IMsgSetResponse response = QBAccess.DoRequest(sessionManager, request);
            var xml = response.ToXMLString();
            var result = QBAccess.getTransactions(response, ref error);

            if (result != null)
            {
                // result returns each line of journal entry, line contains accountnumber which is now holding account full name
                // so, need to find and replace with account number.

                //find accounts
                request = QBAccess.CreateRequest(sessionManager);
                var accountnames = result.SelectMany(i => i.Lines).Select(i => i.AccountNumber).Distinct().ToList();
                QBAccess.BuildQueryAccount(request, accountnames.ToArray());
                response = QBAccess.DoRequest(sessionManager, request);
                var accountsData = QBAccess.getAccount(response, ref error);
                // replace account number of each line of journal entry
                result.ForEach(i =>
                {
                    i.Lines.ForEach(l =>
                    {
                        var a = accountsData.FirstOrDefault(x => x.FullName == l.AccountNumber);
                        if (a != null) l.AccountNumber = a.AccountNumber;
                    });
                });
            }

            return result;
        }
        public void Dispose()
        {
            sessionManager = null;
        }
    }

    #region Internal classes

    internal class AccountListIDPair
    {
        public string AccountNumber { get; set; }
        public string ListID { get; set; }
    }
    internal class CustomerListIDPair
    {
        public string FullName { get; set; }
        public string ListID { get; set; }
    }
    #endregion
}
