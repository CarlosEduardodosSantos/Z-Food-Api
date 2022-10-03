﻿// Copyright [2011] [PagSeguro Internet Ltda.]
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Uol.PagSeguro.Log
{
    /// <summary>
    ///
    /// </summary>
    internal static class PagSeguroTrace
    {
        private enum Level
        {
            None = 0,
            Info = 1,
            Warn = 2,
            Error = 3
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string FormatMessage(Level level, string message)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}: {2}",
                DateTime.Now,
                level,
                message);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Trace.TraceInformation(FormatMessage(Level.Info, message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            Trace.TraceError(FormatMessage(Level.Warn, message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Trace.TraceError(FormatMessage(Level.Error, message));
        }
    }
}