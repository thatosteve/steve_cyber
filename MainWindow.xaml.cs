using steve_cyber;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace steve_cyber
{
    public partial class MainWindow : Window
    {
        // Collections for responses
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        user_name check_name = new user_name();

        // Memory storage for user preferences and conversation context
        Dictionary<string, string> userMemory = new Dictionary<string, string>();
        string lastTopic = string.Empty;
        string lastResponseText = string.Empty;

        // Conversation flow tracking
        int counting = 0;

        // User variables
        string username = string.Empty;

        

        public MainWindow()
        {
            InitializeComponent();

            // Initialize chatbot responses
            new Steve_Cyber(reply, ignore) { };

           

            // Voice greeting
            voice_greeting greet = new voice_greeting();
            greet.greet();
        }

        

        // proceed event handler
        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        // submit name event handler
        private void submit_name(object sender, RoutedEventArgs e)
        {
            username = check_name.submit_name(usernames_input, chats);

            // Store name in memory
            userMemory["name"] = username;

            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            // Send personalized welcome - only ONE welcome message
            error_method("Steve_Cyber", "Hello " + username + "! I'm here to help you stay safe online.");
            error_method("Steve_Cyber", "You can ask me about passwords, phishing, scams, privacy, 2FA, and more.");
            error_method("Steve_Cyber", "Try saying 'tell me about scams' or 'I'm interested in privacy'");
        }

        // Input validation method
        private bool ValidateInput(string input)
        {
            // Check for empty input
            if (string.IsNullOrWhiteSpace(input))
            {
                error_method("Steve_Cyber", "Please enter a question or message.");
                return false;
            }

            // Check for maximum length (200 characters)
            if (input.Length > 200)
            {
                error_method("Steve_Cyber", "Your message is too long. Please keep it under 200 characters.");
                return false;
            }

            // Check if input is just numbers or symbols
            bool onlySymbols = true;
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    onlySymbols = false;
                    break;
                }
            }

            if (onlySymbols && input.Length > 0)
            {
                error_method("Steve_Cyber", "I couldn't understand that. Please use words to ask your question.");
                return false;
            }

            return true;
        }

        // send event handler
        private void send(object sender, RoutedEventArgs e)
        {
            string rawQuestion = question.Text.ToString().Trim();

            // Input validation
            if (!ValidateInput(rawQuestion))
            {
                question.Clear();
                return;
            }

            string questions = RemoveSpecialCharacters(rawQuestion);

            // Show user message in GREEN
            error_method_user(username, rawQuestion);

            // Check for conversation flow commands first
            if (HandleConversationFlow(questions))
            {
                question.Clear();
                return;
            }

            // Check for memory recall commands
            if (HandleMemoryCommands(questions))
            {
                question.Clear();
                return;
            }

            auto_show_interest();
            ai_check(questions);
            question.Clear();
        }

        // Handle conversation flow (follow-up questions)
        private bool HandleConversationFlow(string input)
        {
            string lowerInput = input.ToLower();

            // Check for "tell me more" or "more examples"
            if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") ||
                lowerInput.Contains("more examples") || lowerInput.Contains("more about") ||
                lowerInput.Contains("tell me about") || lowerInput.Contains("what about") ||
                lowerInput.Contains("another tip") || lowerInput.Contains("more tips"))
            {
                // Extract topic from the input if they said "tell me about X"
                string requestedTopic = ExtractTopicFromInput(lowerInput);

                if (!string.IsNullOrEmpty(requestedTopic))
                {
                    // User asked about a specific topic
                    string moreInfo = GetMoreInfoOnTopic(requestedTopic);
                    error_method("Steve_Cyber", moreInfo);
                    lastTopic = requestedTopic;
                    return true;
                }
                else if (!string.IsNullOrEmpty(lastTopic))
                {
                    // User said "tell me more" about the last topic
                    string moreInfo = GetMoreInfoOnTopic(lastTopic);
                    error_method("Steve_Cyber", "Here is more information about " + lastTopic + ":");
                    error_method("Steve_Cyber", moreInfo);
                    return true;
                }
                else
                {
                    error_method("Steve_Cyber", "Please ask me about a specific topic first. Try 'tell me about phishing' or 'explain passwords'");
                    return true;
                }
            }

            return false;
        }

        // Extract topic from user input
        private string ExtractTopicFromInput(string input)
        {
            string[] topics = { "password", "phishing", "scam", "privacy", "firewall", "2fa", "two factor", "wifi", "vpn", "hacking", "malware" };

            foreach (string topic in topics)
            {
                if (input.Contains(topic))
                {
                    if (topic == "2fa" || topic == "two factor")
                        return "2fa";
                    return topic;
                }
            }
            return string.Empty;
        }

        // Get more information on a topic
        private string GetMoreInfoOnTopic(string topic)
        {
            Dictionary<string, string> extendedInfo = new Dictionary<string, string>
            {
                { "password", "Strong passwords should be at least 12 characters long and include uppercase, lowercase, numbers, and symbols. Never reuse passwords across different accounts! Use a password manager like Bitwarden or LastPass. Change your passwords every 3-6 months for important accounts." },
                { "phishing", "Phishing emails often create urgency ('Your account will be closed!'), have spelling errors, or come from slightly misspelled email addresses. Always hover over links before clicking. Never download attachments from unknown senders. Report phishing emails to your email provider." },
                { "scam", "Common scams in South Africa: Fake banking calls (pretending to be from Capitec, FNB, or Standard Bank), WhatsApp 'Hi Mom' scams, fake job offers asking for money, lottery scams, and SIM swap scams. Never share your OTP or PIN with anyone. Hang up and call the official bank number." },
                { "privacy", "Review your privacy settings on social media monthly. Limit what personal info you share publicly - birthday, address, phone number, and location. Use different emails for different purposes. Be careful with online quizzes that ask for personal information." },
                { "firewall", "A firewall monitors incoming and outgoing network traffic. Windows Defender Firewall is built into Windows - make sure it's always enabled. It blocks unauthorized access while allowing legitimate traffic. Never disable your firewall unless you know exactly what you're doing." },
                { "2fa", "Two-Factor Authentication adds a second layer of security. Use authenticator apps (Google Authenticator, Microsoft Authenticator) instead of SMS when possible. Enable 2FA on email, banking, social media, and any account that offers it. It blocks 99.9% of account takeovers." },
                { "wifi", "Public Wi-Fi is convenient but risky. Hackers can intercept your data on open networks. Use a VPN on public Wi-Fi. Avoid logging into banking or email. Turn off file sharing. Use HTTPS websites only. Better yet, use your mobile data for sensitive activities." }
            };

            foreach (var key in extendedInfo.Keys)
            {
                if (topic.ToLower().Contains(key))
                {
                    return extendedInfo[key];
                }
            }

            return "Staying safe online requires constant awareness. Always think before you click, verify the source of messages, and when in doubt - don't click! Trust your instincts.";
        }

        // Handle memory commands (recall user information)
        private bool HandleMemoryCommands(string input)
        {
            string lowerInput = input.ToLower();

            // Recall user's interests
            if (lowerInput.Contains("what am i interested in") || lowerInput.Contains("what did i tell you") ||
                lowerInput.Contains("what do you remember") || lowerInput.Contains("recall my interests") ||
                lowerInput.Contains("what do you know about me"))
            {
                if (userMemory.ContainsKey("interests") && !string.IsNullOrEmpty(userMemory["interests"]))
                {
                    error_method("Steve_Cyber", "You told me you are interested in: " + userMemory["interests"]);
                    error_method("Steve_Cyber", "Would you like me to share some tips about " + userMemory["interests"] + "?");
                }
                else
                {
                    error_method("Steve_Cyber", "You haven't told me about your interests yet. Try saying 'I am interested in privacy' or 'I like learning about passwords' and I will remember!");
                }
                return true;
            }

            // Recall name
            if (lowerInput.Contains("what is my name") || lowerInput.Contains("do you know my name"))
            {
                error_method("Steve_Cyber", "Of course! Your name is " + username + "!");
                return true;
            }

            return false;
        }

        // Store user interest in memory
        private void StoreInterestInMemory(string interests)
        {
            if (userMemory.ContainsKey("interests"))
            {
                userMemory["interests"] = userMemory["interests"] + ", " + interests;
            }
            else
            {
                userMemory["interests"] = interests;
            }
        }

        // KeyDown event handler for Enter key to send message
        private void question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send(sender, e);
            }
        }

        // Clear button event handler
        private void clearText(object sender, RoutedEventArgs e)
        {
            question.Clear();
            question.Focus();
        }

        // Clear Chat button event handler
        private void clearChat(object sender, RoutedEventArgs e)
        {
            chats.Items.Clear();
            DisplayAsciiArt();
            error_method("Steve_Cyber", "Chat history has been cleared. How can I help you today?");
        }

        // start of ai_chat method
        private void ai_check(string questions)
        {
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("Steve_Cyber", "Please enter a valid question.");
                question.Clear();
                return;
            }

            string[] words = questions.ToLower().Split(new char[] { ' ', ',', '.', '?', '!', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
            bool found = false;
            string message = string.Empty;
            Random indexer = new Random();
            List<string> per_word = new List<string>();
            List<string> answers_found = new List<string>();

            bool isInterestStatement = false;
            string detectedInterests = string.Empty;

            foreach (string word in words)
            {
                if (word.Length < 3 || ignore.Contains(word.ToLower()))
                    continue;

                per_word.Clear();

                // Interest detection
                if (word.Contains("interested") || (word.Contains("like") && Array.Exists(words, w => w == "learning")))
                {
                    isInterestStatement = true;
                    HashSet<string> currentInterests = new HashSet<string>();

                    foreach (string interest in words)
                    {
                        string clean = interest.ToLower().Trim();
                        clean = Regex.Replace(clean, @"[^a-zA-Z0-9\s]", "");

                        if (!ignore.Contains(clean) && clean != "interested" && clean != "and" && clean != "in" && clean != "like" && clean != "learning" && clean.Length >= 3)
                        {
                            currentInterests.Add(clean);
                        }
                    }

                    detectedInterests = string.Join(", ", currentInterests);

                    if (!string.IsNullOrWhiteSpace(detectedInterests))
                    {
                        StoreInterestInMemory(detectedInterests);

                        string filename = "interested_topic.txt";
                        bool userFound = false;

                        if (File.Exists(filename))
                        {
                            string[] lines = File.ReadAllLines(filename);
                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (lines[i].StartsWith(username))
                                {
                                    userFound = true;
                                    string existing = lines[i].Replace(username + " interested in:", "").ToLower();
                                    HashSet<string> existingSet = new HashSet<string>(existing.Split(',').Select(x => x.Trim()).Where(x => x != ""));

                                    foreach (string item in currentInterests)
                                    {
                                        existingSet.Add(item);
                                    }

                                    string finalList = string.Join(", ", existingSet);
                                    lines[i] = username + " interested in: " + finalList;
                                    File.WriteAllLines(filename, lines);
                                    break;
                                }
                            }
                        }

                        if (!userFound)
                        {
                            File.AppendAllText(filename, username + " interested in: " + detectedInterests + "\n");
                        }

                        message += "Thanks for sharing! I will remember that you are interested in " + detectedInterests + ". ";
                        lastTopic = detectedInterests;
                        found = true;
                    }
                }

                // Search for matching answers
                bool wordFound = false;
                foreach (string answer in reply)
                {
                    if (answer.ToLower().Contains(word))
                    {
                        wordFound = true;
                        per_word.Add(answer);
                    }
                }

                if (wordFound && per_word.Count > 0)
                {
                    found = true;
                    int indexing = indexer.Next(0, per_word.Count);
                    string selectedAnswer = per_word[indexing];
                    answers_found.Add(selectedAnswer);

                    string[] answerParts = selectedAnswer.Split(' ');
                    if (answerParts.Length > 0)
                    {
                        string possibleTopic = answerParts[0].ToLower();
                        if (possibleTopic == "password" || possibleTopic == "phishing" ||
                            possibleTopic == "scam" || possibleTopic == "privacy" ||
                            possibleTopic == "2fa" || possibleTopic == "wifi" || possibleTopic == "firewall")
                        {
                            lastTopic = possibleTopic;
                        }
                    }
                }
            }

            if (found && answers_found.Count > 0)
            {
                answers_found = answers_found.Distinct().ToList();

                foreach (string per_answer in answers_found)
                {
                    string displayAnswer = per_answer;
                    int spaceIndex = per_answer.IndexOf(' ');
                    if (spaceIndex > 0 && spaceIndex < 20)
                    {
                        displayAnswer = per_answer.Substring(spaceIndex + 1);
                    }
                    message += displayAnswer + "\n";
                }

                if (userMemory.ContainsKey("interests") && !isInterestStatement)
                {
                    string[] personalMessages = {
                        "Speaking of which, as someone interested in " + userMemory["interests"] + "... ",
                        "Based on your interest in " + userMemory["interests"] + "... ",
                        "Since you care about " + userMemory["interests"] + "... "
                    };
                    Random rand = new Random();
                    message = personalMessages[rand.Next(personalMessages.Length)] + message;
                }

                error_method("Steve_Cyber", message.TrimEnd('\n'));
            }
            else if (!isInterestStatement)
            {
                string[] fallbackMessages = {
                    "I'm not sure about that. Could you rephrase? Try asking about passwords, phishing, scams, privacy, or 2FA.",
                    "That's outside my cybersecurity knowledge. Ask me about staying safe online!",
                    "I focus on cybersecurity topics. Try 'What is phishing?' or 'How do I create strong passwords?'",
                    "Let's stay on topic! I can help with passwords, phishing emails, scams, privacy, and online safety.",
                    "I didn't quite understand that. Could you rephrase your question about cybersecurity?"
                };
                Random random = new Random();
                error_method("Steve_Cyber", fallbackMessages[random.Next(fallbackMessages.Length)]);
            }
        }

        // method to remove special characters
        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'' || c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    sanitized.Append(' ');
                }
            }

            string result = sanitized.ToString();
            result = Regex.Replace(result, @"\s+", " ").Trim();
            return result;
        }

        // method count to show interests randomly
        private void auto_show_interest()
        {
            if (counting == 4)
            {
                string filename = "interested_topic.txt";
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(username))
                        {
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex >= 0)
                            {
                                string interests = line.Substring(colonIndex + 14).Trim();
                                if (!string.IsNullOrEmpty(interests))
                                {
                                    string[] reminderMessages = {
                                        "Just a friendly reminder - you are interested in " + interests + "!",
                                        "Remembering your interest in " + interests + " - want to learn more?",
                                        "Since you care about " + interests + ", let me share a tip!"
                                    };
                                    Random rand = new Random();
                                    error_method("Steve_Cyber", reminderMessages[rand.Next(reminderMessages.Length)]);
                                }
                                break;
                            }
                        }
                    }
                }
                counting = 0;
            }
            else
            {
                counting++;
            }
        }

        // error method for BOT messages (CYAN color)
        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

            // Bot message - Cyan theme
            messageBorder.Background = new SolidColorBrush(Color.FromRgb(22, 33, 62));
            messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255));
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            // Bot name in CYAN
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
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        // error method for USER messages (GREEN color)
        private void error_method_user(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

            // User message - Green theme
            messageBorder.Background = new SolidColorBrush(Color.FromRgb(22, 33, 62));
            messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 153));
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };

            // User name in GREEN
            Brush nameColor = new SolidColorBrush(Color.FromRgb(0, 255, 153));
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
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }
    }
}