using System;
using System.Diagnostics;
using Caliburn.Micro;

namespace MyFuelTracker
{
	public class DebugLogger : ILog
	{
		private readonly Type _type;

		public DebugLogger(Type type)
		{
			_type = type;
		}

		private void WriteLog(string level, string format, params object[] args)
		{
			Debug.WriteLine("[{0}] : {1}", level, string.Format(format, args));
		}

		public void Info(string format, params object[] args)
		{
			WriteLog("INFO", format, args);
		}

		public void Warn(string format, params object[] args)
		{
			Warn("INFO", format, args);
		}

		public void Error(Exception exception)
		{
			WriteLog("ERROR", exception.ToString());
		}
	}
}