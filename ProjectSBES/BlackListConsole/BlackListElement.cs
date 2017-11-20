using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListConsole
{
    public class BlackListElement
    {
        private string process;
        private string groupPerson;

        public BlackListElement() { }

        public string Process
        {
            get
            {
                return process;
            }

            set
            {
                process = value;
            }
        }

        public string GroupPerson
        {
            get
            {
                return groupPerson;
            }

            set
            {
                groupPerson = value;
            }
        }
    }
}
