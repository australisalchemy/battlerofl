using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace battlerofl
{
    public class Log
    {
        private readonly string m_LogVersion = "v1.000";
        private EventsWindow m_EventsWindow = new EventsWindow();

        public Log()
        {
            AddEvent("RoflCorp Events Logging System | Battlerofl | " + m_LogVersion, "Initialized");
            AddEvent("Events Window opened at: " + DateTime.Now, "Initialized");
        }

        public void AddEvent(string message, string eventName)
        {
           string format = string.Format("[{0}] ({1}): {2}\n", DateTime.Now, eventName, message);
           m_EventsWindow.txtEvents.AppendText(format);
        }

        public void RemoveEvent(string eventName)
        {
            m_EventsWindow.txtEvents.Text.Replace(eventName, "");
        }

        public void SaveLog(string @path)
        {
            TextWriter write = new StreamWriter(@path);
            write.Write(m_EventsWindow.txtEvents.Text);

            write.Close();
        }

        public void ShowWindow()
        {
            m_EventsWindow.ShowDialog();
        }
    }
}
