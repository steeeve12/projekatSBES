using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceDataHelper
    {
        public Dictionary<string, int> usersAttempts;
        public Dictionary<string, Stopwatch> forbidenUsers;
        public int maxAttempts = 2;
        public int timeOfDenial = 1000 * 60 * 1;
        public int eventCnt = 0;

        private static ServiceDataHelper serviceDataHelper = null;

        private ServiceDataHelper()
        {
            usersAttempts = new Dictionary<string, int>();
            forbidenUsers = new Dictionary<string, Stopwatch>();
        }

        public static ServiceDataHelper Helper()
        {
            if(serviceDataHelper == null)
            {
                serviceDataHelper = new ServiceDataHelper();
            }
            return serviceDataHelper;
        }
    }
}
