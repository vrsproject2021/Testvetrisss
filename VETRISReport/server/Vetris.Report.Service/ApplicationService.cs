using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;

namespace Vetris.Report.Service
{
    public abstract class ApplicationService
    {
        protected IDatabaseContext _db;
        protected ISessionInfo SessionInfo;
        protected ILog Logger;
        public ApplicationService(IDatabaseContext database, ISessionInfo session)
        {
            _db = database;
            SessionInfo = session;
            Logger = LogManager.GetLogger("Log");
        }

        public List<string> GetFonts()
        {
            var list = new List<string>();
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    list.Add(fa.Name);
                }
            }
            return list;
        }
    }
}
