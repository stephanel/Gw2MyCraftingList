using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2CraftToolDbUpdater
{
    class DbLine
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _is_checked;
        public bool Is_checked
        {
            get
            {
                return _is_checked;
            }
            set
            {
                _is_checked = value;
            }
        }
        
        public DbLine(int id, bool is_checked)
        {
            this._id = id;
            this._is_checked = is_checked;
        }
    }
}
