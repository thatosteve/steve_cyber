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
        // Existing collections
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        user_name check_name = new user_name();

        // Memory and conversation
        Dictionary<string, string> userMemory = new Dictionary<string, string>();
        string lastTopic = string.Empty;
        string currentTopic = string.Empty;
        int counting = 0;
        string username = string.Empty;

        // Part 3 - New features
        private ActivityLog activityLog;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private bool isQuizMode = false;
        private bool isTaskMode = false;

        
      

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Part 3 features
            activityLog = new ActivityLog();
            taskManager = new TaskManager(activityLog);
            quizManager = new QuizManager(activityLog);

            activityLog.AddLogEntry("Application started");

            new Steve_Cyber(reply, ignore) { };
            

            voice_greeting greet = new voice_greeting();
            greet.greet();
        }

      

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            username = check_name.submit_name(usernames_input, chats);
            userMemory["name"] = username;
            activityLog.AddLogEntry("User logged in: " + username);

            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            string[] welcomeTips = {
                "I am here to help you stay safe online!",
                "You can ask me about passwords, phishing, scams, and more!",
                "Try saying 'I am interested in privacy' and I will remember it!",
                "Type 'quiz' to test your cybersecurity knowledge!",
                "Type 'tasks' to manage your cybersecurity tasks!",
                "Type 'log' to see what I have done for you!"
            };

            Random rand = new Random();
            error_method("Steve_Cyber", "Hello " + username + "!");
            error_method("Steve_Cyber", welcomeTips[rand.Next(welcomeTips.Length)]);
            error_method("Steve_Cyber", "What would you like to learn about today?");
        }

        // ---- PART 3: QUIZ METHODS ----

        private void StartQuiz(object sender, RoutedEventArgs e)
        {
            quizManager.ResetQuiz();
            isQuizMode = true;
            isTaskMode = false;

            QuizQuestion firstQuestion = quizManager.GetCurrentQuestion();
            if (firstQuestion != null)
            {
                activityLog.AddLogEntry("Quiz started by " + username);
                DisplayQuizQuestion(firstQuestion);
            }
        }

        private void DisplayQuizQuestion(QuizQuestion question)
        {
            string message = "QUIZ TIME!\n\n";
            message += "Question " + quizManager.GetCurrentQuestionNumber() + " of " + quizManager.GetTotalQuestions() + "\n";
            message += question.QuestionText + "\n\n";

            char optionLabel = 'A';
            for (int i = 0; i < question.Options.Count; i++)
            {
                message += optionLabel + ") " + question.Options[i] + "\n";
                optionLabel++;
            }

            message += "\nType your answer (A, B, C, or D)";
            message += "\nProgress: " + quizManager.GetProgress();

            error_method("Steve_Cyber", message);
        }

        private void HandleQuizInput(string input)
        {
            if (!quizManager.IsQuizActive())
            {
                error_method("Steve_Cyber", "The quiz has ended. Type 'quiz' to start a new one!");
                isQuizMode = false;
                return;
            }

            string lowerInput = input.ToLower().Trim();
            int selectedIndex = -1;

            if (lowerInput == "a") selectedIndex = 0;
            else if (lowerInput == "b") selectedIndex = 1;
            else if (lowerInput == "c") selectedIndex = 2;
            else if (lowerInput == "d") selectedIndex = 3;

            if (selectedIndex == -1)
            {
                error_method("Steve_Cyber", "Please enter A, B, C, or D for your answer.");
                return;
            }

            QuizQuestion current = quizManager.GetCurrentQuestion();
            if (selectedIndex >= current.Options.Count)
            {
                error_method("Steve_Cyber", "Invalid option. Please choose A, B, C, or D.");
                return;
            }

            string feedback = quizManager.SubmitAnswer(selectedIndex);
            error_method("Steve_Cyber", feedback);

            string nextResult = quizManager.MoveToNextQuestion();

            if (quizManager.IsQuizActive())
            {
                DisplayQuizQuestion(quizManager.GetCurrentQuestion());
            }
            else
            {
                error_method("Steve_Cyber", nextResult);
                isQuizMode = false;
                activityLog.AddLogEntry("Quiz completed by " + username);
            }
        }

        // PART 3 TASK METHOD

        private void ShowTasks(object sender, RoutedEventArgs e)
        {
            isTaskMode = true;
            isQuizMode = false;

            List<CybersecurityTask> tasks = taskManager.GetTasks(username);

            if (tasks.Count == 0)
            {
                error_method("Steve_Cyber", "You have no tasks. Type 'add task: [title]' to create one!");
                return;
            }

            string message = "YOUR CYBERSECURITY TASKS:\n\n";
            foreach (CybersecurityTask task in tasks)
            {
                message += "ID: " + task.Id + "\n";
                message += "Title: " + task.Title + "\n";
                if (!string.IsNullOrEmpty(task.Description))
                {
                    message += "Description: " + task.Description + "\n";
                }
                message += "Status: " + task.GetStatus() + "\n";
                message += task.GetReminderString() + "\n";
                message += "-------------------\n";
            }

            message += "\nCommands:\n";
            message += "- complete [id]  - Mark task as complete\n";
            message += "- delete [id]    - Delete a task\n";
            message += "- add task: [title] - Add new task";

            error_method("Steve_Cyber", message);
            activityLog.AddLogEntry("Tasks viewed by " + username);
        }

        private void HandleTaskInput(string input)
        {
            string lowerInput = input.ToLower().Trim();

            // Add task
            if (lowerInput.StartsWith("add task:") || lowerInput.StartsWith("add task "))
            {
                string title = input.Substring(input.IndexOf(':') + 1).Trim();
                if (string.IsNullOrEmpty(title))
                {
                    error_method("Steve_Cyber", "Please provide a task title. Example: 'add task: Enable 2FA'");
                    return;
                }

                bool result = taskManager.AddTask(username, title, "", null);
                if (result)
                {
                    error_method("Steve_Cyber", "Task added successfully! Use 'tasks' to view it.");
                    activityLog.AddLogEntry("Task added: " + title);
                }
                else
                {
                    error_method("Steve_Cyber", "Failed to add task. Please check your database connection.");
                }
                return;
            }

            // Add full task with description and reminder
            if (lowerInput.StartsWith("add full task:"))
            {
                string taskData = input.Substring(input.IndexOf(':') + 1).Trim();
                string[] parts = taskData.Split('|');

                string title = parts.Length > 0 ? parts[0].Trim() : "";
                string description = parts.Length > 1 ? parts[1].Trim() : "";
                int days = parts.Length > 2 ? int.Parse(parts[2].Trim()) : 0;

                if (string.IsNullOrEmpty(title))
                {
                    error_method("Steve_Cyber", "Please provide a task title.");
                    return;
                }

                DateTime? reminderDate = days > 0 ? (DateTime?)DateTime.Now.AddDays(days) : null;

                bool result = taskManager.AddTask(username, title, description, reminderDate);
                if (result)
                {
                    string msg = "Task added successfully: " + title;
                    if (reminderDate.HasValue)
                    {
                        msg += " (Reminder set for " + days + " days from now)";
                    }
                    error_method("Steve_Cyber", msg);
                    activityLog.AddLogEntry("Full task added: " + title);
                }
                else
                {
                    error_method("Steve_Cyber", "Failed to add task. Please check your database connection.");
                }
                return;
            }

            // Complete task
            if (lowerInput.StartsWith("complete"))
            {
                string[] parts = input.Split(' ');
                if (parts.Length < 2)
                {
                    error_method("Steve_Cyber", "Please provide a task ID. Example: 'complete 3'");
                    return;
                }

                if (int.TryParse(parts[1], out int taskId))
                {
                    bool result = taskManager.MarkTaskComplete(taskId);
                    if (result)
                    {
                        error_method("Steve_Cyber", "Task " + taskId + " marked as completed!");
                        activityLog.AddLogEntry("Task completed: ID " + taskId);
                    }
                    else
                    {
                        error_method("Steve_Cyber", "Failed to complete task. Please check the ID.");
                    }
                }
                else
                {
                    error_method("Steve_Cyber", "Invalid task ID. Please enter a number.");
                }
                return;
            }

            // Delete task
            if (lowerInput.StartsWith("delete"))
            {
                string[] parts = input.Split(' ');
                if (parts.Length < 2)
                {
                    error_method("Steve_Cyber", "Please provide a task ID. Example: 'delete 3'");
                    return;
                }

                if (int.TryParse(parts[1], out int taskId))
                {
                    bool result = taskManager.DeleteTask(taskId);
                    if (result)
                    {
                        error_method("Steve_Cyber", "Task " + taskId + " deleted successfully!");
                        activityLog.AddLogEntry("Task deleted: ID " + taskId);
                    }
                    else
                    {
                        error_method("Steve_Cyber", "Failed to delete task. Please check the ID.");
                    }
                }
                else
                {
                    error_method("Steve_Cyber", "Invalid task ID. Please enter a number.");
                }
                return;
            }

            error_method("Steve_Cyber", "Unknown task command. Available commands:\n- tasks - View all tasks\n- add task: [title]\n- complete [id]\n- delete [id]");
        }

        // ---- PART 3: ACTIVITY LOG METHODS ----

        private void ShowActivityLog(object sender, RoutedEventArgs e)
        {
            string log = activityLog.GetRecentLogs(10);
            error_method("Steve_Cyber", log);
            activityLog.AddLogEntry("Activity log viewed by " + username);
        }

        private void HandleLogInput(string input)
        {
            string lowerInput = input.ToLower().Trim();

            if (lowerInput == "show more" && activityLog.GetEntryCount() > 10)
            {
                string fullLog = activityLog.GetFullLog();
                error_method("Steve_Cyber", fullLog);
                activityLog.AddLogEntry("Full activity log viewed by " + username);
            }
            else
            {
                error_method("Steve_Cyber", "Type 'show more' to see all log entries.");
            }
        }

        // ---- EXISTING METHODS (updated for Part 3) ----

        // Input validation
        private bool ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                error_method("Steve_Cyber", "Please enter a question or message.");
                return false;
            }

            if (input.Length > 200)
            {
                error_method("Steve_Cyber", "Your message is too long. Please keep it under 200 characters.");
                return false;
            }

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
                error_method("Steve_Cyber", "I could not understand that. Please use words to ask your question.");
                return false;
            }

            return true;
        }

        // --- FIXED send() method with reordered checks ---
        private void send(object sender, RoutedEventArgs e)
        {
            string rawQuestion = question.Text.ToString().Trim();

            if (!ValidateInput(rawQuestion))
            {
                question.Clear();
                return;
            }

            string questions = RemoveSpecialCharacters(rawQuestion);

            // ========== QUIZ MODE ==========
            if (isQuizMode)
            {
                error_method_user(username, rawQuestion);
                HandleQuizInput(rawQuestion);
                question.Clear();
                return;
            }

            // ========== QUIZ TRIGGER ==========
            if (rawQuestion.ToLower().Contains("quiz") || rawQuestion.ToLower().Contains("test"))
            {
                error_method_user(username, rawQuestion);
                StartQuiz(sender, e);
                question.Clear();
                return;
            }

            // ========== TASK COMMANDS (MUST BE BEFORE GENERAL "task" CHECK) ==========
            if (rawQuestion.ToLower().Contains("add task") ||
                rawQuestion.ToLower().StartsWith("complete ") ||
                rawQuestion.ToLower().StartsWith("delete "))
            {
                error_method_user(username, rawQuestion);
                HandleTaskInput(rawQuestion);
                question.Clear();
                return;
            }

            // ========== TASK TRIGGER (Show tasks) ==========
            if (rawQuestion.ToLower().Contains("task") || rawQuestion.ToLower().Contains("tasks"))
            {
                error_method_user(username, rawQuestion);
                ShowTasks(sender, e);
                question.Clear();
                return;
            }

            // ========== LOG TRIGGER ==========
            if (rawQuestion.ToLower().Contains("log") || rawQuestion.ToLower().Contains("what have you done"))
            {
                error_method_user(username, rawQuestion);
                ShowActivityLog(sender, e);
                question.Clear();
                return;
            }

            // ========== LOG SHOW MORE ==========
            if (rawQuestion.ToLower().Contains("show more") && rawQuestion.ToLower().Contains("log"))
            {
                error_method_user(username, rawQuestion);
                HandleLogInput(rawQuestion);
                question.Clear();
                return;
            }

            // ========== NORMAL CHAT ==========
            error_method_user(username, rawQuestion);

            if (HandleConversationFlow(questions))
            {
                question.Clear();
                return;
            }

            if (HandleMemoryCommands(questions))
            {
                question.Clear();
                return;
            }

            auto_show_interest();
            ai_check(questions);
            question.Clear();
        }

        private bool HandleConversationFlow(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") ||
                lowerInput.Contains("another tip") || lowerInput.Contains("another one") ||
                lowerInput.Contains("more please") || lowerInput.Contains("continue") ||
                lowerInput.Contains("go on"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    error_method("Steve_Cyber", "Let me share more about " + lastTopic + "!");
                    string moreInfo = GetMoreInfoOnTopic(lastTopic);
                    error_method("Steve_Cyber", moreInfo);
                    return true;
                }
                else
                {
                    error_method("Steve_Cyber", "I have not shared any topic yet. Ask me about cybersecurity like 'What is phishing?' or 'Tell me about passwords!'");
                    return true;
                }
            }

            if (lowerInput.Contains("another tip") || lowerInput.Contains("more tips") ||
                lowerInput.Contains("different tip") || lowerInput.Contains("another one please"))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    string alternateTip = GetAlternateTip(currentTopic);
                    error_method("Steve_Cyber", alternateTip);
                    return true;
                }
                else
                {
                    error_method("Steve_Cyber", "What topic would you like another tip about? Try asking about passwords, phishing, or privacy first!");
                    return true;
                }
            }

            return false;
        }

        private string GetMoreInfoOnTopic(string topic)
        {
            Dictionary<string, string> extendedInfo = new Dictionary<string, string>
            {
                { "password", "Strong passwords should be at least 12 characters long and include uppercase, lowercase, numbers, and symbols. Never reuse passwords across different accounts! Consider using a password manager like Bitwarden or LastPass to generate and store secure passwords." },
                { "phishing", "Phishing emails often create urgency ('Your account will be closed!'), have spelling errors, or come from slightly misspelled email addresses. Always hover over links before clicking and never download attachments from unknown senders!" },
                { "scam", "Online scammers in South Africa often pretend to be from banks like Capitec, FNB, or SARS. Remember: No legitimate company will ever ask for your PIN, password, or OTP via phone, email, or WhatsApp. Hang up and call the official number!" },
                { "privacy", "Your personal information is valuable to scammers! Limit what you share on social media - birthday, address, location tags, and even pet names can be used to guess security questions. Review your privacy settings monthly!" },
                { "firewall", "A firewall is like a security guard for your internet connection. Windows has a built-in firewall - make sure it is always enabled! It blocks unauthorized access while allowing legitimate traffic through." },
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

            return "Staying safe online requires constant awareness. Always think before you click, verify the source of messages, and when in doubt - do not click! Trust your instincts.";
        }

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
                    "Never send money to someone you have not met in person - even if they promise lottery winnings or a 'free gift'!",
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
                "Do not install apps from outside the official Google Play Store or Apple App Store!",
                "If something feels off about an email or message - trust your gut and do not click!"
            };

            Random random = new Random();
            return generalTips[random.Next(generalTips.Length)];
        }

        private bool HandleMemoryCommands(string input)
        {
            string lowerInput = input.ToLower();

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
                    error_method("Steve_Cyber", "You have not told me about your interests yet! Try saying 'I am interested in privacy' or 'I like learning about passwords' and I will remember!");
                }
                return true;
            }

            if (lowerInput.Contains("what is my name") || lowerInput.Contains("do you know my name"))
            {
                error_method("Steve_Cyber", "Of course! Your name is " + username + "!");
                return true;
            }

            return false;
        }

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

        private void question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send(sender, e);
            }
        }

        private void clearText(object sender, RoutedEventArgs e)
        {
            question.Clear();
            question.Focus();
        }

        private void clearChat(object sender, RoutedEventArgs e)
        {
            chats.Items.Clear();
           
            error_method("Steve_Cyber", "Chat history has been cleared. How can I help you today?");
            activityLog.AddLogEntry("Chat cleared by " + username);
        }

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
                        activityLog.AddLogEntry("User interest stored: " + detectedInterests);
                    }
                }

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
                activityLog.AddLogEntry("User asked about: " + questions.Substring(0, Math.Min(30, questions.Length)));
            }
            else if (!isInterestStatement)
            {
                string[] fallbackMessages = {
                    "I am not sure about that. Could you rephrase? Try asking about passwords, phishing, scams, privacy, or 2FA.",
                    "That is outside my cybersecurity knowledge. Ask me about staying safe online!",
                    "I focus on cybersecurity topics. Try 'What is phishing?' or 'How do I create strong passwords?'",
                    "Let us stay on topic! I can help with passwords, phishing emails, scams, privacy, and online safety.",
                    "I did not quite understand that. Could you rephrase your question about cybersecurity?"
                };
                Random random = new Random();
                error_method("Steve_Cyber", fallbackMessages[random.Next(fallbackMessages.Length)]);
            }
        }

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

        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

            if (name.ToLower().Contains("steve_cyber") || name.ToLower().Contains("steve"))
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(22, 33, 62));
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
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        private void error_method_user(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(8)
            };

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