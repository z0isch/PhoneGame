using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace PhoneGameService.Logging
{
    public static class LogHelper
    {
        private static int _indentLevel = 0;
        private static string _tabString = new string('\t', 128);

        public static void Begin(ILog log, string functionSig)
        {
            log.Info(string.Format("{0}Begin {1}", _tabString.Substring(0, _indentLevel), functionSig));
            if(_indentLevel < 127 ) _indentLevel++;
        }

        public static void Middle(ILog log, string message)
        {
            log.Info(string.Format("{0}{1}", _tabString.Substring(0, _indentLevel), message));
        }

        public static void End(ILog log, string functionSig)
        {
            if(_indentLevel > 0 ) _indentLevel--;
            log.Info(string.Format("{0}End {1}", _tabString.Substring(0, _indentLevel), functionSig));
        }

        public static void BeginDebug(ILog log, string functionSig)
        {
            log.Debug(string.Format("{0}Begin {1}", _tabString.Substring(0, _indentLevel), functionSig));
            if (_indentLevel < 127) _indentLevel++;
        }

        public static void MiddleDebug(ILog log, string message)
        {
            log.Debug(string.Format("{0}{1}", _tabString.Substring(0, _indentLevel), message));
        }

        public static void EndDebug(ILog log, string functionSig)
        {
            if (_indentLevel > 0) _indentLevel--;
            log.Debug(string.Format("{0}End {1}", _tabString.Substring(0, _indentLevel), functionSig));
        }
    }
}
