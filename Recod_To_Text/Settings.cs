using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recod_To_Text
{
    public class Settings
    {
        #region メンバ変数
        private string subscriptionKey;
        private string region;
        private string location;
        private string device;
        private string proxy_Host;
        private string proxy_Port;
        #endregion

        #region プロパティ
        public string SubscriptionKey
        {
            get { return subscriptionKey; }
            set { subscriptionKey = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Device
        {
            get { return device; }
            set { device = value; }
        }

        public string PROXY_Host
        {
            get { return proxy_Host; }
            set { proxy_Host = value; }
        }

        public string PROXY_Port
        {
            get { return proxy_Port; }
            set { proxy_Port = value; }
        }
        #endregion
    }
}
