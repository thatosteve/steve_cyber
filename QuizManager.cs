using System;
using System.Collections.Generic;

namespace steve_cyber
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        private ActivityLog activityLog;
        private bool quizActive;

        public QuizManager(ActivityLog log)
        {
            activityLog = log;
            questions = new List<QuizQuestion>();
            LoadQuestions();
            ResetQuiz();
        }

        private void LoadQuestions()
        {
            questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Reporting phishing emails helps prevent scams and protects others."
            ));

            questions.Add(new QuizQuestion(
                "Which of the following is a strong password?",
                new List<string> { "password123", "PurpleElephant@2024", "12345678", "qwerty" },
                1,
                "A strong password uses a mix of uppercase, lowercase, numbers, and special characters."
            ));

            questions.Add(new QuizQuestion(
                "What is two-factor authentication (2FA)?",
                new List<string> { "A type of virus", "An extra layer of security", "A password manager", "A firewall" },
                1,
                "2FA adds a second layer of security by requiring a second form of verification."
            ));

            questions.Add(new QuizQuestion(
                "True or False: Public Wi-Fi is always safe for banking.",
                new List<string> { "True", "False" },
                1,
                "Public Wi-Fi networks are often unsecured and can be intercepted by hackers."
            ));

            questions.Add(new QuizQuestion(
                "What is phishing?",
                new List<string> { "A type of fishing", "A scam where attackers pretend to be trusted sources", "A password", "A firewall" },
                1,
                "Phishing attacks use deception to trick users into revealing sensitive information."
            ));

            questions.Add(new QuizQuestion(
                "What should you do if your account is hacked?",
                new List<string> { "Do nothing", "Change your password and enable 2FA", "Share your password", "Delete the account" },
                1,
                "Immediately secure your account by changing passwords and enabling extra security."
            ));

            questions.Add(new QuizQuestion(
                "True or False: You should use the same password for all accounts.",
                new List<string> { "True", "False" },
                1,
                "Using the same password across accounts is risky - if one is compromised, all are vulnerable."
            ));

            questions.Add(new QuizQuestion(
                "What does a firewall do?",
                new List<string> { "Protects against physical theft", "Controls network traffic", "Manages passwords", "Scans for viruses" },
                1,
                "A firewall monitors and controls incoming and outgoing network traffic."
            ));

            questions.Add(new QuizQuestion(
                "How often should you update your passwords?",
                new List<string> { "Never", "Every 3-6 months", "Every year", "Only when hacked" },
                1,
                "Regular password changes every 3-6 months help protect your accounts."
            ));

            questions.Add(new QuizQuestion(
                "True or False: HTTPS websites are always 100 percent safe.",
                new List<string> { "True", "False" },
                1,
                "HTTPS encrypts data in transit but doesn't guarantee the website is legitimate."
            ));

            questions.Add(new QuizQuestion(
                "What is a VPN used for?",
                new List<string> { "To watch videos", "To encrypt internet traffic", "To store passwords", "To scan for viruses" },
                1,
                "A VPN encrypts your internet traffic, especially important on public networks."
            ));

            questions.Add(new QuizQuestion(
                "What should you do with suspicious links?",
                new List<string> { "Click them to check", "Report them and don't click", "Share them with friends", "Ignore them" },
                1,
                "Always be cautious with suspicious links and report them when possible."
            ));

            activityLog.AddLogEntry("Quiz loaded with " + questions.Count + " questions");
        }

        public void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            quizActive = true;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                return questions[currentQuestionIndex];
            }
            return null;
        }

        public string SubmitAnswer(int selectedIndex)
        {
            if (!quizActive)
            {
                return "The quiz is not active. Start a new quiz by typing 'quiz'.";
            }

            QuizQuestion current = GetCurrentQuestion();
            if (current == null)
            {
                return "No more questions available.";
            }

            if (selectedIndex == current.CorrectAnswerIndex)
            {
                score++;
                activityLog.AddLogEntry("Quiz: Correct answer for question " + (currentQuestionIndex + 1));
                return "Correct! " + current.Feedback;
            }
            else
            {
                string correctAnswer = current.Options[current.CorrectAnswerIndex];
                activityLog.AddLogEntry("Quiz: Incorrect answer for question " + (currentQuestionIndex + 1));
                return "Incorrect. The correct answer was: " + correctAnswer + "\n" + current.Feedback;
            }
        }

        public string MoveToNextQuestion()
        {
            currentQuestionIndex++;
            if (currentQuestionIndex >= questions.Count)
            {
                quizActive = false;
                string result = "Quiz Complete!\n" +
                                "Score: " + score + "/" + questions.Count + "\n" +
                                "Percentage: " + ((double)score / questions.Count * 100).ToString("F0") + "%\n\n";

                if (score >= questions.Count * 0.8)
                {
                    result += "Excellent! You are a cybersecurity pro! Keep up the great work!";
                }
                else if (score >= questions.Count * 0.6)
                {
                    result += "Good job! Keep learning to become a cybersecurity expert!";
                }
                else
                {
                    result += "Keep practicing! Cybersecurity is important for everyone. Review the tips and try again!";
                }

                activityLog.AddLogEntry("Quiz completed. Score: " + score + "/" + questions.Count);
                return result;
            }
            return "Next question loaded.";
        }

        public string GetProgress()
        {
            return "Question " + (currentQuestionIndex + 1) + " of " + questions.Count + " | Score: " + score;
        }

        public bool IsQuizActive()
        {
            return quizActive && currentQuestionIndex < questions.Count;
        }

        public int GetTotalQuestions()
        {
            return questions.Count;
        }

        public int GetCurrentQuestionNumber()
        {
            return currentQuestionIndex + 1;
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Feedback { get; set; }

        public QuizQuestion(string question, List<string> options, int correctIndex, string feedback)
        {
            QuestionText = question;
            Options = options;
            CorrectAnswerIndex = correctIndex;
            Feedback = feedback;
        }
    }
}
