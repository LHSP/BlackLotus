using EixoX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBlackLotus
{
    public class BlackLotusDb<T> : DatabaseStorage<T>
    {
        private static BlackLotusDb<T> _instance;
        public static BlackLotusDb<T> Instance
        {
            get { return _instance ?? (_instance = new BlackLotusDb<T>()); }
        }

        private BlackLotusDb()
            : base(new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["BlackLotusDbSqlServer"].ConnectionString), DatabaseAspect<T>.Instance)
        {

        }
    }
}
