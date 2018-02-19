using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Sharpbrake.Client;
using System.Configuration;
using ABS.Logic.Shared.Airbrake;

namespace ABS.GBS.Log
{
    public class SystemLog
    {
		protected AirbrakeNotifier _Notifier;
		public AirbrakeNotifier Notifier
		{
			get { return _Notifier; }
			set { _Notifier = value; }
		}
		//protected Airbrake.AirbrakeNotifier _Notifier;
		//public Airbrake.AirbrakeNotifier Notifier
		//{
		//	get { return _Notifier; }
		//	set { _Notifier = value; }
		//}

        public SystemLog()
        {
            Config();
        }

        public void Config()
        {
            var settings = ConfigurationManager.AppSettings.AllKeys
    .Where(key => key.StartsWith("Airbrake", StringComparison.OrdinalIgnoreCase))
    .ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

            var airbrakeConfiguration = AirbrakeConfig.Load(settings);
			Notifier = new AirbrakeNotifier(airbrakeConfiguration);
			//Notifier = null;
        }
    }
}