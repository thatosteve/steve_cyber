using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows;

namespace steve_cyber
{
    public class TaskManager
    {
        private string connectionString;
        private ActivityLog activityLog;

        public TaskManager(ActivityLog log)
        {
            activityLog = log;
            // UPDATE THIS WITH YOUR MYSQL PASSWORD
            connectionString = "Server=localhost;Database=cyberchatbot;Uid=root;Pwd=Thatonong123;";

            try
            {
                CreateTableIfNotExists();
                activityLog.AddLogEntry("Database connection successful");
            }
            catch (Exception ex)
            {
                string msg = "Database connection failed: " + ex.Message;
                activityLog.AddLogEntry(msg);
                MessageBox.Show(msg, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTableIfNotExists()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS cybersecurity_tasks (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    username VARCHAR(100) NOT NULL,
                    title VARCHAR(255) NOT NULL,
                    description TEXT,
                    reminder_date DATETIME,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool AddTask(string username, string title, string description, DateTime? reminderDate)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO cybersecurity_tasks 
                                    (username, title, description, reminder_date) 
                                    VALUES (@username, @title, @description, @reminderDate)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", (object)reminderDate ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string logMessage = "Task added: " + title;
                            if (reminderDate.HasValue)
                            {
                                logMessage += " (Reminder set for " + reminderDate.Value.ToString("yyyy-MM-dd") + ")";
                            }
                            activityLog.AddLogEntry(logMessage);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "AddTask Error: " + ex.Message;
                activityLog.AddLogEntry(errorMsg);
                MessageBox.Show(errorMsg, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public List<CybersecurityTask> GetTasks(string username)
        {
            List<CybersecurityTask> tasks = new List<CybersecurityTask>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, title, description, reminder_date, is_completed, created_at 
                                    FROM cybersecurity_tasks 
                                    WHERE username = @username 
                                    ORDER BY is_completed ASC, created_at DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CybersecurityTask task = new CybersecurityTask
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                                    IsCompleted = reader.GetBoolean("is_completed"),
                                    CreatedAt = reader.GetDateTime("created_at")
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                activityLog.AddLogEntry("GetTasks Error: " + ex.Message);
            }
            return tasks;
        }

        public CybersecurityTask GetTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, title, description, reminder_date, is_completed, created_at FROM cybersecurity_tasks WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CybersecurityTask
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                                    IsCompleted = reader.GetBoolean("is_completed"),
                                    CreatedAt = reader.GetDateTime("created_at")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                activityLog.AddLogEntry("GetTask Error: " + ex.Message);
            }
            return null;
        }

        public bool SetReminder(int taskId, DateTime reminderDate)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE cybersecurity_tasks SET reminder_date = @reminderDate WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate);
                        cmd.Parameters.AddWithValue("@id", taskId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            activityLog.AddLogEntry("Reminder set for task ID " + taskId + " on " + reminderDate.ToString("yyyy-MM-dd"));
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "SetReminder Error: " + ex.Message;
                activityLog.AddLogEntry(errorMsg);
                MessageBox.Show(errorMsg, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public bool MarkTaskComplete(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE cybersecurity_tasks SET is_completed = TRUE WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            activityLog.AddLogEntry("Task marked as completed (ID: " + taskId + ")");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                activityLog.AddLogEntry("MarkTaskComplete Error: " + ex.Message);
            }
            return false;
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM cybersecurity_tasks WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            activityLog.AddLogEntry("Task deleted (ID: " + taskId + ")");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                activityLog.AddLogEntry("DeleteTask Error: " + ex.Message);
            }
            return false;
        }
    }

    public class CybersecurityTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public string GetStatus()
        {
            return IsCompleted ? "Completed" : "Pending";
        }

        public string GetReminderString()
        {
            if (ReminderDate.HasValue)
            {
                TimeSpan daysLeft = ReminderDate.Value - DateTime.Now;
                if (daysLeft.TotalDays > 0)
                {
                    return "Reminder: " + ReminderDate.Value.ToString("yyyy-MM-dd") + " (" + Math.Ceiling(daysLeft.TotalDays) + " days left)";
                }
                else if (daysLeft.TotalDays > -1)
                {
                    return "Reminder: TODAY!";
                }
                else
                {
                    return "Reminder: OVERDUE (was on " + ReminderDate.Value.ToString("yyyy-MM-dd") + ")";
                }
            }
            return "No reminder set";
        }
    }
}