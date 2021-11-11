using System;

namespace DicomWin
{
    internal class SessionInfo
    {
        public Guid TransactionId { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
        public DateTime Date { get; set; }
    }
}