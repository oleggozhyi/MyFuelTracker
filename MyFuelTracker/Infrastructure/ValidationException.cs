﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Infrastructure
{
	public class ValidationException : Exception
	{
		public ValidationException() {}
		public ValidationException(string message) : base(message) {}
		public ValidationException(string message, Exception inner) : base(message, inner) {}
	}
}
