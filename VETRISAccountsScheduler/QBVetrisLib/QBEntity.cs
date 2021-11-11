using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBVetrisLib
{
    public class AccountEntity
    {
        public string ListID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
    }

    public class CustomerEntity
    {
        public string ListID { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// There is no delete concept in Quick Books, it has just active/inactive
        /// </summary>
        public bool IsActive { get; set; }
        public Address BillAddress { get; set; }
        public Address ShipAddress { get; set; }
        public string EditSequence { get; set; }
        public string ExternalGUID { get; set; }
    }

    public class VendorEntity
    {
        public string ListID { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// There is no delete concept in Quick Books, it has just active/inactive
        /// </summary>
        public bool IsActive { get; set; }
        public Address Address { get; set; }
        public Address ShipAddress { get; set; }
        public string EditSequence { get; set; }
        public string ExternalGUID { get; set; }
    }

    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }


    /// <summary>
    /// Journal Detail Line
    /// </summary>
    public class JournalDetailEntity
    {
        /// <summary>
        /// Account Number for the GL
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Debit Amount
        /// </summary>
        public double? DebitAmount { get; set; }
        /// <summary>
        /// Credit Amount
        /// </summary>
        public double? CreditAmount { get; set; }
        /// <summary>
        /// Billing account name
        /// </summary>
        public string CustomerFullName { get; set; }
        /// <summary>
        /// Line remarks if any
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Is Line valid
        /// </summary>
        public bool IsValid { get { return string.IsNullOrEmpty(ErrorMessage); } }
        /// <summary>
        /// Line error message
        /// </summary>
        public string ErrorMessage
        {
            get
            {

                if (DebitAmount > 0 && (CreditAmount??0) == 0)
                {
                    return null;
                }
                if ((DebitAmount??0) == 0 && CreditAmount > 0)
                {
                    return null;
                }
                if (DebitAmount < 0)
                {
                    return "Debit amount is negative.";
                }
                if (CreditAmount < 0)
                {
                    return "Credit amount is negative.";
                }
                if (DebitAmount > 0 && CreditAmount > 0)
                {
                    return "Both Debit and Credit amount cannot have values.";
                }
                return "Either Debit or Credit amount must be there.";

            }
        }
    }

    /// <summary>
    /// Journal Header Line
    /// This will be used for all transactions like Invoice, Reversal of Invoice, Payment etc.
    /// </summary>
    public class JournalEntity
    {
        public JournalEntity()
        {
            Lines = new List<JournalDetailEntity>();
        }
        /// <summary>
        /// Transaction Date
        /// </summary>
        public DateTime TxnDate { get; set; }
        /// <summary>
        /// Reference Number like Invoice No, Payment No etc.
        /// </summary>
        public string RefNumber { get; set; }
        /// <summary>
        /// Mark true if it is adjustments like in Payments
        /// </summary>
        public bool IsAdjustment { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Ledger/Account detail lines for Debit Credits
        /// </summary>
        public List<JournalDetailEntity> Lines { get; set; }

        #region output from QuickBooks
        /// <summary>
        /// To be returned on creation, In the case of update you must supply it.
        /// These will return on successful creation
        /// </summary>
        public string TxnID { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeModified { get; set; }
        #endregion
    }
}
