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
        string currentTopic = string.Empty;

        // Conversation flow tracking
        int followUpCount = 0;

        // User variables
        string username = string.Empty;
        int counting = 0;

       

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

            // Send personalized welcome with cybersecurity tips
            string[] welcomeTips = {
                "I'm here to help you stay safe online!",
                "You can ask me about passwords, phishing, scams, and more!",
                "Try saying 'I'm interested in privacy' and I'll remember it!",
                "Ask me 'tell me more' after any tip for extra information!"
            };

            Random rand = new Random();
            error_method("CyberMind", "Hello " + username + "!");
            error_method("CyberMind", welcomeTips[rand.Next(welcomeTips.Length)]);
            error_method("CyberMind", "What would you like to learn about today?");
        }

        // send event handler
        private void send(object sender, RoutedEventArgs e)
        {
            string rawQuestion = question.Text.ToString().Trim();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("CyberMind", "Please enter a question.");
                return;
            }

            string questions = RemoveSpecialCharacters(rawQuestion);
            error_method(username, rawQuestion);

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

            // Check for follow-up requests
            if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") ||
                lowerInput.Contains("another tip") || lowerInput.Contains("another one") ||
                lowerInput.Contains("more please") || lowerInput.Contains("continue") ||
                lowerInput.Contains("go on"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    followUpCount++;
                    error_method("CyberMind", "Let me share more about " + lastTopic + "!");

                    // Provide additional information based on last topic
                    string moreInfo = GetMoreInfoOnTopic(lastTopic);
                    error_method("CyberMind", moreInfo);
                    return true;
                }
                else
                {
                    error_method("CyberMind", "I haven't shared any topic yet. Ask me about cybersecurity like 'What is phishing?' or 'Tell me about passwords!'");
                    return true;
                }
            }

            // Check for "give me another tip"
            if (lowerInput.Contains("another tip") || lowerInput.Contains("more tips") ||
                lowerInput.Contains("different tip") || lowerInput.Contains("another one please"))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    string alternateTip = GetAlternateTip(currentTopic);
                    error_method("CyberMind", alternateTip);
                    return true;
                }
                else
                {
                    error_method("CyberMind", "What topic would you like another tip about? Try asking about passwords, phishing, or privacy first!");
                    return true;
                }
            }

            return false;
        }

        // Get more information on a topic
        private string GetMoreInfoOnTopic(string topic)
        {
            Dictionary<string, string> extendedInfo = new Dictionary<string, string>
            {
                { "password", "Strong passwords should be at least 12 characters long and include uppercase, lowercase, numbers, and symbols. Never reuse passwords across different accounts! Consider using a password manager like Bitwarden or LastPass to generate and store secure passwords." },
                { "phishing", "Phishing emails often create urgency ('Your account will be closed!'), have spelling errors, or come from slightly misspelled email addresses. Always hover over links before clicking and never download attachments from unknown senders!" },
                { "scam", "Online scammers in South Africa often pretend to be from banks like Capitec, FNB, or SARS. Remember: No legitimate company will ever ask for your PIN, password, or OTP via phone, email, or WhatsApp. Hang up and call the official number!" },
                { "privacy", "Your personal information is valuable to scammers! Limit what you share on social media - birthday, address, location tags, and even pet names can be used to guess security questions. Review your privacy settings monthly!" },
                { "firewall", "A firewall is like a security guard for your internet connection. Windows has a built-in firewall - make sure it's always enabled! It blocks unauthorized access while allowing legitimate traffic through." },
                { "2fa", "Two-Factor Authentication (2FA) adds a second lock to your accounts. Even if someone steals your password, they still need your phone to get in. Use authenticator apps like Google Authenticator or Microsoft Authenticator instead of SMS when possible!" },
                { "wifi", "Public Wi-Fi at malls, coffee shops, and airports is convenient but risky! Hackers can intercept your data. Always use a VPN (Virtual Private Network) on public networks, and avoid logging into banking, email, or social media." }
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

        // Get alternate tip for a topic
        private string GetAlternateTip(string topic)
        {
            Dictionary<string, List<string>> tipAlternatives = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Use a passphrase like 'PurpleElephantDances@Midnight' - easy to remember, hard to crack!",
                    "Enable Two-Factor Authentication (2FA) on all accounts that offer it - especially email and banking!",
                    "Change your passwords every 3-6 months for important accounts like email, banking, and social media!",
                    "Never write passwords on sticky notes or save them in unencrypted files on your computer!"
                }},
                { "phishing", new List<string> {
                    "Check the sender's email address carefully - 'support@paypa1.com' is fake, 'support@paypal.com' is real!",
                    "Never click links in suspicious emails - type the website address directly into your browser instead!",
                    "Scammers also use SMS (smishing) and phone calls (vishing). Be suspicious of unexpected messages!",
                    "Look for red flags: urgent language, spelling errors, generic greetings like 'Dear Customer', and requests for personal info!"
                }},
                { "scam", new List<string> {
                    "Got a call from 'your bank'? Hang up and call back using the official number from their website or your bank card!",
                    "Never send money to someone you haven't met in person - even if they promise lottery winnings or a 'free gift'!",
                    "Job scams are common in SA. Never pay for a job application or training - legitimate jobs pay YOU!",
                    "WhatsApp scams: 'Hi Mom, I broke my phone' messages are often scammers. Always verify by calling the person directly!"
                }},
                { "privacy", new List<string> {
                    "Think before you post! Photos can reveal your location, workplace, daily routine, and even your house number!",
                    "Review your privacy settings on Facebook, Instagram, TikTok, and LinkedIn every few months - platforms change settings often!",
                    "Use different email addresses for different purposes: one for banking, one for shopping, one for social media!",
                    "Never share your ID number, passport number, or home address online - not even in 'harmless' quizzes!"
                }}
            };

            Random rand = new Random();

            foreach (var key in tipAlternatives.Keys)
            {
                if (topic.ToLower().Contains(key))
                {
                    return tipAlternatives[key][rand.Next(tipAlternatives[key].Count)];
                }
            }

            string[] generalTips = {
                "Always use unique passwords for every account - no exceptions!",
                "Keep your software, apps, and operating system updated - updates fix security holes!",
                "Enable fingerprint or face recognition on your phone for extra security!",
                "Don't install apps from outside the official Google Play Store or Apple App Store!",
                "If something feels off about an email or message - trust your gut and don't click!"
            };

            Random random = new Random();
            return generalTips[random.Next(generalTips.Length)];
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
                    error_method("CyberMind", "You told me you're interested in: " + userMemory["interests"]);
                    error_method("CyberMind", "Would you like me to share some tips about " + userMemory["interests"] + "?");
                }
                else
                {
                    error_method("CyberMind", "You haven't told me about your interests yet! Try saying 'I'm interested in privacy' or 'I like learning about passwords' and I'll remember!");
                }
                return true;
            }

            // Recall name
            if (lowerInput.Contains("what is my name") || lowerInput.Contains("do you know my name"))
            {
                error_method("CyberMind", "Of course! Your name is " + username + "!");
                return true;
            }

            // Store interests in memory when detected in ai_check
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

        // start of ai_chat method
        private void ai_check(string questions)
        {
            // Check if user entered anything meaningful
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("CyberMind", "Please enter a valid question.");
                question.Clear();
                return;
            }

            // Check if the question contains only special characters or empty after cleaning
            if (questions.Length == 0 || string.IsNullOrWhiteSpace(questions))
            {
                error_method("CyberMind", "I couldn't understand that.");
                question.Clear();
                return;
            }

            // Variables for processing
            string[] words = questions.ToLower().Split(new char[] { ' ', ',', '.', '?', '!', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
            bool found = false;
            string message = string.Empty;
            Random indexer = new Random();
            List<string> per_word = new List<string>();
            List<string> answers_found = new List<string>();

            // Track if this is an interest statement
            bool isInterestStatement = false;
            string detectedInterests = string.Empty;

            // Process each word
            foreach (string word in words)
            {
                // Skip very short words or ignored words
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
                        // Store in memory
                        StoreInterestInMemory(detectedInterests);

                        // Save to file
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

                        message += "Thanks for sharing! I'll remember that you're interested in " + detectedInterests + ". ";
                        currentTopic = detectedInterests;
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

                    // Extract topic for conversation flow
                    string[] answerParts = selectedAnswer.Split(' ');
                    if (answerParts.Length > 0)
                    {
                        string possibleTopic = answerParts[0].ToLower();
                        if (possibleTopic == "cybersecurity" || possibleTopic == "phishing" || possibleTopic == "password" ||
                            possibleTopic == "firewall" || possibleTopic == "vpn" || possibleTopic == "fraud" ||
                            possibleTopic == "scam" || possibleTopic == "privacy" || possibleTopic == "2fa" || possibleTopic == "wifi")
                        {
                            currentTopic = possibleTopic;
                            lastTopic = possibleTopic;
                        }
                    }
                }
            }

            // Show responses or error message
            if (found && answers_found.Count > 0)
            {
                // Remove duplicate answers
                answers_found = answers_found.Distinct().ToList();

                foreach (string per_answer in answers_found)
                {
                    // Remove the keyword prefix from display
                    string displayAnswer = per_answer;
                    int spaceIndex = per_answer.IndexOf(' ');
                    if (spaceIndex > 0 && spaceIndex < 20)
                    {
                        displayAnswer = per_answer.Substring(spaceIndex + 1);
                    }
                    message += displayAnswer + "\n";
                }

                // Personalize response if user has interests stored
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

                error_method("CyberMind", message.TrimEnd('\n'));
                followUpCount = 0;
            }
            else if (!isInterestStatement)
            {
                string[] fallbackMessages = {
                    "I'm not sure about that. Could you rephrase? Try asking about passwords, phishing, scams, or online safety!",
                    "That's outside my cybersecurity knowledge. Ask me about staying safe online!",
                    "I focus on cybersecurity topics. Try 'What is phishing?' or 'How do I create strong passwords?'",
                    "Let's stay on topic! I can help with passwords, phishing emails, scams, privacy, and online safety.",
                    "I didn't quite understand that. Could you rephrase your question about cybersecurity?"
                };
                Random random = new Random();
                error_method("CyberMind", fallbackMessages[random.Next(fallbackMessages.Length)]);
            }
        }
        // end of ai_chat method

        // method to remove special characters
        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in input)
            {
                // Keep letters, numbers, spaces, and basic punctuation
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'' || c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    // Replace other special characters with space
                    sanitized.Append(' ');
                }
            }

            // Clean up extra spaces and trim
            string result = sanitized.ToString();
            result = Regex.Replace(result, @"\s+", " ").Trim();

            return result;
        }
        // end of method to remove special characters

        // method count to show interests randomly
        private void auto_show_interest()
        {
            // check if four messages have passed
            if (counting == 4)
            {
                // read the user's interests from file
                string filename = "interested_topic.txt";

                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);

                    // find the user's line
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(username))
                        {
                            // get the interests part
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex >= 0)
                            {
                                string interests = line.Substring(colonIndex + 14).Trim();
                                if (!string.IsNullOrEmpty(interests))
                                {
                                    string[] reminderMessages = {
                                        "Just a friendly reminder - you're interested in " + interests + "!",
                                        "Remembering your interest in " + interests + " - want to learn more?",
                                        "Since you care about " + interests + ", let me share a tip!"
                                    };
                                    Random rand = new Random();
                                    error_method("CyberMind", reminderMessages[rand.Next(reminderMessages.Length)]);
                                }
                                break;
                            }
                        }
                    }
                }

                // reset counting
                counting = 0;
            }
            else
            {
                // incrementing
                counting++;
            }
        }
        // end of count interest method

        // Display formatted message with border
        private void DisplayFormattedMessage(string sender, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                Background = new SolidColorBrush(Color.FromRgb(83, 52, 131)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8)
            };

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 11
            };

            messageText.Inlines.Add(new Run
            {
                Text = sender + ":\n",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 212, 255)),
                FontWeight = FontWeights.Bold
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = new SolidColorBrush(Color.FromRgb(238, 238, 238))
            });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        // error method
        private void error_method(string name, string message)
        {
            // Create a border for chats
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

            // Set different background for user vs bot
            if (name.ToLower().Contains("cybermind") || name.ToLower().Contains("steve"))
            {
                // Bot message - Dark blue/purple theme
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(83, 52, 131));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255));
            }
            else
            {
                // User message - Dark teal theme
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

            // Set color based on sender
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
        // end of error method
    }
    // end of class
}
// end of namespace