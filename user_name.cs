using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace steve_cyber
{
    public class user_name
    {
        public string submit_name(TextBox user_name, ListView chats)
        {
            string filename = "user_names.txt";

            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, "auto_create\n");
            }

            string name = user_name.Text.ToString();
            bool found = check_name(name);

            if (!found)
            {
                File.AppendAllText(filename, name + "\n");
                error_method("CyberMind", "Hello " + name + "! Welcome to CyberMind Cybersecurity Assistant.", chats);
            }
            else
            {
                error_method("CyberMind", "Welcome back " + name + "! How can I help you stay safe online today?", chats);
            }

            return name;
        }

        private Boolean check_name(string name)
        {
            string filename = "user_names.txt";
            bool found_name = false;
            string[] names = File.ReadAllLines(filename);

            foreach (string name_found in names)
            {
                if (name_found.ToLower() == name.ToLower())
                {
                    found_name = true;
                }
            }

            return found_name;
        }

        private void error_method(string name, string message, ListView chats)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

            if (name.ToLower().Contains("cybermind"))
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(83, 52, 131));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255));
            }
            else
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(22, 33, 62));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255));
            }
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            Brush nameColor = new SolidColorBrush(Color.FromRgb(0, 212, 255));
            Brush messageColor = new SolidColorBrush(Color.FromRgb(238, 238, 238));

            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
        }
    }
}