using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ABS.Logic.Shared.Airbrake
{
	public class AirbrakeNotifier
	{
		public AirbrakeNotifier(AirbrakeConfig config, string logger = null, string httpRequestHandler = null)
		{

		}

		public void AddFilter(Func<Notice, Notice> filter)
		{

		}
		public void Notify(Exception exception, IHttpContext context = null, Severity severity = null)
		{

		}
		public void NotifyAsync(Exception exception, IHttpContext context = null, Severity severity = null)
		{

		}

		//public AirbrakeConfig New ()
		//{
		//	return null;
		//}
		//public Exception Notify ()
		//{
		//	return null;
		//}
		//public Exception NotifyAsync (AirbrakeConfig config)
		//{
		//	return null;
		//}
	}

	public class Severity
	{
	}

	public interface IHttpContext
	{
	}

	public class Notice
	{
	}

	public class AirbrakeConfig
	{
		public string Environment { get; set; }
		public string AppVersion { get; set; }
		public string ProjectKey { get; set; }
		public string ProjectId { get; set; }
		public string Host { get; set; }
		public string LogFile { get; set; }
		public string ProxyUri { get; set; }
		public string ProxyUsername { get; set; }
		public string ProxyPassword { get; set; }
		public IList<string> IgnoreEnvironments { get; set; }
		public IList<string> WhitelistKeys { get; set; }
		public IList<string> BlacklistKeys { get; set; }

		//public string Environment = string.Empty;
		//public string AppVersion = string.Empty;
		//public string ProjectKey = string.Empty;
		//public string ProjectId = string.Empty;
		//public string Host = string.Empty;
		//public string LogFile = string.Empty;
		//public string ProxyUri = string.Empty;
		//public string ProxyUsername = string.Empty;
		//public string ProxyPassword = string.Empty;
		//public IList<string> IgnoreEnvironments = null;
		//public IList<string> WhitelistKeys = null;
		//public IList<string> BlacklistKeys = null;

		//public string New ()
		//{
		//	return null;
		//}

		public static AirbrakeConfig Load(IDictionary<string, string> settings)
		{
			return new AirbrakeConfig();
		}
	}
}
