using System;
using System.Collections.Generic;

namespace steve_cyber
{
    public class ActivityLog
    {
        private List<LogEntry> logEntries;
        private int maxEntries = 10;

        public ActivityLog()
        {
            logEntries = new List<LogEntry>();
        }

        public void AddLogEntry(string description)
        {
            logEntries.Insert(0, new LogEntry
            {
                Timestamp = DateTime.Now,
                Description = description
            });

            // Keep only last 10 entries by default
            if (logEntries.Count > maxEntries)
            {
                logEntries.RemoveRange(maxEntries, logEntries.Count - maxEntries);
            }
        }

        public string GetRecentLogs(int count = 10)
        {
            if (logEntries.Count == 0)
            {
                return "No activity logged yet.";
            }

            string result = "Activity Log (Last " + Math.Min(count, logEntries.Count) + " actions):\n\n";
            int displayCount = Math.Min(count, logEntries.Count);

            for (int i = 0; i < displayCount; i++)
            {
                result += (i + 1) + ". " + logEntries[i].Timestamp.ToString("HH:mm") + " - " + logEntries[i].Description + "\n";
            }

            if (logEntries.Count > count)
            {
                result += "\nType 'show more' to see all " + logEntries.Count + " entries.";
            }

            return result;
        }

        public string GetFullLog()
        {
            if (logEntries.Count == 0)
            {
                return "No activity logged yet.";
            }

            string result = "Complete Activity Log (" + logEntries.Count + " entries):\n\n";
            for (int i = 0; i < logEntries.Count; i++)
            {
                result += (i + 1) + ". " + logEntries[i].Timestamp.ToString("yyyy-MM-dd HH:mm") + " - " + logEntries[i].Description + "\n";
            }
            return result;
        }

        public int GetEntryCount()
        {
            return logEntries.Count;
        }

        public bool HasEntries()
        {
            return logEntries.Count > 0;
        }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}