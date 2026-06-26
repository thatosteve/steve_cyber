using System.Collections;

namespace steve_cyber
{
    public class Steve_Cyber
    {
        public Steve_Cyber(ArrayList reply, ArrayList ignore)
        {
            answers(reply);
            words(ignore);
        }

        private void words(ArrayList ignoring)
        {
            ignoring.Add("a");
            ignoring.Add("about");
            ignoring.Add("above");
            ignoring.Add("across");
            ignoring.Add("after");
            ignoring.Add("afterwards");
            ignoring.Add("again");
            ignoring.Add("against");
            ignoring.Add("all");
            ignoring.Add("almost");
            ignoring.Add("alone");
            ignoring.Add("along");
            ignoring.Add("already");
            ignoring.Add("also");
            ignoring.Add("although");
            ignoring.Add("always");
            ignoring.Add("am");
            ignoring.Add("among");
            ignoring.Add("amongst");
            ignoring.Add("amount");
            ignoring.Add("an");
            ignoring.Add("and");
            ignoring.Add("another");
            ignoring.Add("any");
            ignoring.Add("anyhow");
            ignoring.Add("anyone");
            ignoring.Add("anything");
            ignoring.Add("anyway");
            ignoring.Add("anywhere");
            ignoring.Add("are");
            ignoring.Add("around");
            ignoring.Add("as");
            ignoring.Add("at");
            ignoring.Add("back");
            ignoring.Add("be");
            ignoring.Add("became");
            ignoring.Add("because");
            ignoring.Add("become");
            ignoring.Add("becomes");
            ignoring.Add("becoming");
            ignoring.Add("been");
            ignoring.Add("before");
            ignoring.Add("beforehand");
            ignoring.Add("behind");
            ignoring.Add("being");
            ignoring.Add("below");
            ignoring.Add("beside");
            ignoring.Add("besides");
            ignoring.Add("between");
            ignoring.Add("beyond");
            ignoring.Add("both");
            ignoring.Add("but");
            ignoring.Add("by");
            ignoring.Add("can");
            ignoring.Add("cannot");
            ignoring.Add("could");
            ignoring.Add("did");
            ignoring.Add("do");
            ignoring.Add("does");
            ignoring.Add("doing");
            ignoring.Add("done");
            ignoring.Add("down");
            ignoring.Add("during");
            ignoring.Add("each");
            ignoring.Add("either");
            ignoring.Add("else");
            ignoring.Add("elsewhere");
            ignoring.Add("enough");
            ignoring.Add("etc");
            ignoring.Add("even");
            ignoring.Add("ever");
            ignoring.Add("every");
            ignoring.Add("everyone");
            ignoring.Add("everything");
            ignoring.Add("everywhere");
            ignoring.Add("except");
            ignoring.Add("few");
            ignoring.Add("first");
            ignoring.Add("for");
            ignoring.Add("former");
            ignoring.Add("formerly");
            ignoring.Add("from");
            ignoring.Add("further");
            ignoring.Add("had");
            ignoring.Add("has");
            ignoring.Add("have");
            ignoring.Add("having");
            ignoring.Add("he");
            ignoring.Add("hence");
            ignoring.Add("her");
            ignoring.Add("here");
            ignoring.Add("hereafter");
            ignoring.Add("hereby");
            ignoring.Add("herein");
            ignoring.Add("hereupon");
            ignoring.Add("hers");
            ignoring.Add("herself");
            ignoring.Add("him");
            ignoring.Add("himself");
            ignoring.Add("his");
            ignoring.Add("how");
            ignoring.Add("however");
            ignoring.Add("i");
            ignoring.Add("if");
            ignoring.Add("in");
            ignoring.Add("indeed");
            ignoring.Add("inside");
            ignoring.Add("instead");
            ignoring.Add("into");
            ignoring.Add("is");
            ignoring.Add("it");
            ignoring.Add("its");
            ignoring.Add("itself");
            ignoring.Add("last");
            ignoring.Add("later");
            ignoring.Add("latter");
            ignoring.Add("latterly");
            ignoring.Add("least");
            ignoring.Add("less");
            ignoring.Add("lot");
            ignoring.Add("many");
            ignoring.Add("may");
            ignoring.Add("me");
            ignoring.Add("meanwhile");
            ignoring.Add("might");
            ignoring.Add("more");
            ignoring.Add("moreover");
            ignoring.Add("most");
            ignoring.Add("mostly");
            ignoring.Add("much");
            ignoring.Add("must");
            ignoring.Add("my");
            ignoring.Add("myself");
            ignoring.Add("name");
            ignoring.Add("namely");
            ignoring.Add("neither");
            ignoring.Add("never");
            ignoring.Add("nevertheless");
            ignoring.Add("next");
            ignoring.Add("no");
            ignoring.Add("nobody");
            ignoring.Add("none");
            ignoring.Add("noone");
            ignoring.Add("nor");
            ignoring.Add("not");
            ignoring.Add("nothing");
            ignoring.Add("now");
            ignoring.Add("nowhere");
            ignoring.Add("of");
            ignoring.Add("off");
            ignoring.Add("often");
            ignoring.Add("on");
            ignoring.Add("once");
            ignoring.Add("one");
            ignoring.Add("only");
            ignoring.Add("or");
            ignoring.Add("other");
            ignoring.Add("others");
            ignoring.Add("otherwise");
            ignoring.Add("ought");
            ignoring.Add("our");
            ignoring.Add("ours");
            ignoring.Add("ourselves");
            ignoring.Add("out");
            ignoring.Add("outside");
            ignoring.Add("over");
            ignoring.Add("own");
            ignoring.Add("part");
            ignoring.Add("per");
            ignoring.Add("perhaps");
            ignoring.Add("please");
            ignoring.Add("put");
            ignoring.Add("rather");
            ignoring.Add("re");
            ignoring.Add("same");
            ignoring.Add("see");
            ignoring.Add("seem");
            ignoring.Add("seemed");
            ignoring.Add("seeming");
            ignoring.Add("seems");
            ignoring.Add("several");
            ignoring.Add("she");
            ignoring.Add("should");
            ignoring.Add("show");
            ignoring.Add("side");
            ignoring.Add("since");
            ignoring.Add("so");
            ignoring.Add("some");
            ignoring.Add("somehow");
            ignoring.Add("someone");
            ignoring.Add("something");
            ignoring.Add("sometime");
            ignoring.Add("sometimes");
            ignoring.Add("somewhere");
            ignoring.Add("still");
            ignoring.Add("such");
            ignoring.Add("take");
            ignoring.Add("than");
            ignoring.Add("that");
            ignoring.Add("the");
            ignoring.Add("their");
            ignoring.Add("theirs");
            ignoring.Add("them");
            ignoring.Add("themselves");
            ignoring.Add("then");
            ignoring.Add("thence");
            ignoring.Add("there");
            ignoring.Add("thereafter");
            ignoring.Add("thereby");
            ignoring.Add("therefore");
            ignoring.Add("therein");
            ignoring.Add("thereupon");
            ignoring.Add("these");
            ignoring.Add("they");
            ignoring.Add("this");
            ignoring.Add("those");
            ignoring.Add("though");
            ignoring.Add("through");
            ignoring.Add("throughout");
            ignoring.Add("thru");
            ignoring.Add("thus");
            ignoring.Add("to");
            ignoring.Add("together");
            ignoring.Add("too");
            ignoring.Add("toward");
            ignoring.Add("towards");
            ignoring.Add("under");
            ignoring.Add("unless");
            ignoring.Add("until");
            ignoring.Add("up");
            ignoring.Add("upon");
            ignoring.Add("us");
            ignoring.Add("used");
            ignoring.Add("very");
            ignoring.Add("via");
            ignoring.Add("was");
            ignoring.Add("we");
            ignoring.Add("well");
            ignoring.Add("were");
            ignoring.Add("what");
            ignoring.Add("whatever");
            ignoring.Add("when");
            ignoring.Add("whence");
            ignoring.Add("whenever");
            ignoring.Add("where");
            ignoring.Add("whereafter");
            ignoring.Add("whereas");
            ignoring.Add("whereby");
            ignoring.Add("wherein");
            ignoring.Add("whereupon");
            ignoring.Add("wherever");
            ignoring.Add("whether");
            ignoring.Add("which");
            ignoring.Add("while");
            ignoring.Add("whither");
            ignoring.Add("who");
            ignoring.Add("whoever");
            ignoring.Add("whole");
            ignoring.Add("whom");
            ignoring.Add("whose");
            ignoring.Add("why");
            ignoring.Add("will");
            ignoring.Add("with");
            ignoring.Add("within");
            ignoring.Add("without");
            ignoring.Add("would");
            ignoring.Add("yes");
            ignoring.Add("yet");
            ignoring.Add("hey");
            ignoring.Add("you");
            ignoring.Add("your");
            ignoring.Add("yours");
            ignoring.Add("yourself");
            ignoring.Add("yourselves");
        }

        public void answers(ArrayList add_answers)
        {
            // Greetings South African context
            add_answers.Add("greeting Howzit! I'm CyberMind, your digital guardian! Doing great, thanks for asking. How are you today?");
            add_answers.Add("greeting Sharp sharp! CyberMind here, ready to help you stay safe online! What can I assist you with?");
            add_answers.Add("greeting Aweh! I'm CyberMind, your cybersecurity assistant. Hope you're having a secure day!");

            // Purpose
            add_answers.Add("purpose My mission is to help South Africans stay safe online by sharing practical cybersecurity tips and guidance.");
            add_answers.Add("purpose I'm your digital security buddy - here to teach you about online safety, spotting scams, and protecting your data.");
            add_answers.Add("purpose Think of me as your personal cybersecurity coach. I'll help you navigate the digital world safely!");

            // Cybersecurity basics
            add_answers.Add("cybersecurity Cybersecurity is like locking your doors at night - but for the digital world! It protects your devices, accounts, and personal info from online threats.");
            add_answers.Add("cybersecurity It's the practice of defending computers, servers, mobile devices, and networks from malicious attacks. Think digital bodyguard!");
            add_answers.Add("cybersecurity In simple terms, it's staying safe online - from creating strong passwords to spotting fake emails. Your digital shield!");

            // Password safety
            add_answers.Add("password A strong password is your first line of defense! Use at least 12 characters with a mix of letters, numbers, and symbols. Never use 'password123'!");
            add_answers.Add("password Here's a pro tip: Use a passphrase like 'PurpleElephantDances@Midnight' - easy to remember, hard to crack!");
            add_answers.Add("password Never reuse passwords across different accounts. If one gets hacked, they all become vulnerable! Use a password manager instead.");

            // Phishing awareness
            add_answers.Add("phishing Phishing is when scammers send fake emails or texts pretending to be from legitimate companies. They want you to click dangerous links or share personal info!");
            add_answers.Add("phishing Red flags: Urgent language like 'Your account will be closed!', spelling mistakes, suspicious email addresses, and requests for personal information.");
            add_answers.Add("phishing Before clicking any link in an email, hover over it first to see the real web address. When in doubt, go directly to the website instead!");

            // Scam detection - relevant for South Africa
            add_answers.Add("scam Online scams are everywhere in South Africa! Common ones include fake job offers, lottery winnings, and 'bank verification' calls. Never send money to someone you haven't met in person!");
            add_answers.Add("scam If something sounds too good to be true, it probably is! Legitimate companies won't ask for your password or OTP over the phone.");
            add_answers.Add("scam Always verify before trusting. Got a call from 'your bank'? Hang up and call them back on their official number from their website.");

            // Privacy protection
            add_answers.Add("privacy Your personal information is valuable! Limit what you share on social media - birthday, address, and location posts can be used by scammers.");
            add_answers.Add("privacy Review your privacy settings on Facebook, Instagram, and TikTok regularly. Set profiles to private and be careful what you post!");
            add_answers.Add("privacy Think before you share! Once something is online, it stays online forever. Would you want your grandmother or future employer to see it?");

            // Two-factor authentication
            add_answers.Add("2fa Two-Factor Authentication (2FA) adds an extra layer of security. Even if someone steals your password, they still need your phone to get in!");
            add_answers.Add("2fa Enable 2FA on your email, banking, and social media accounts. Use an authenticator app like Google Authenticator - it's more secure than SMS!");
            add_answers.Add("2fa 2FA is like having a second lock on your door. It takes 30 seconds to set up but blocks 99.9% of account hacks!");

            // Public Wi-Fi safety
            add_answers.Add("wifi Public Wi-Fi at malls, coffee shops, and airports is convenient but risky! Avoid logging into banking or email on public networks.");
            add_answers.Add("wifi Use a VPN (Virtual Private Network) when connecting to public Wi-Fi. It encrypts your data so hackers can't steal it!");
            add_answers.Add("wifi If you must use public Wi-Fi, stick to 'https' websites and turn off file sharing. Better yet, use your mobile data for sensitive activities.");

            // Firewall
            add_answers.Add("firewall A firewall monitors and controls incoming and outgoing network traffic based on security rules.");
            add_answers.Add("firewall Think of a firewall as a security guard that checks everything trying to enter or leave your computer.");
            add_answers.Add("firewall Windows has a built-in firewall - make sure it's always turned on for maximum protection!");

            // VPN
            add_answers.Add("vpn A VPN creates a secure, encrypted tunnel for your internet traffic, hiding your online activity from prying eyes.");
            add_answers.Add("vpn VPNs are essential when using public Wi-Fi at coffee shops, airports, or hotels to protect your data.");
            add_answers.Add("vpn A good VPN will hide your IP address and encrypt your data, making it much harder for hackers to intercept your information.");

            // Hacked account
            add_answers.Add("hacked If your account is hacked, immediately change your password and enable two-factor authentication.");
            add_answers.Add("hacked Check your account settings for any unauthorized changes like email forwarding rules or linked devices.");
            add_answers.Add("hacked Contact the platform's support team and let your friends know so they don't fall for any scams sent from your account.");

            // Fraud
            add_answers.Add("fraud If you suspect fraud, contact your bank immediately using the official number on their website or your bank card.");
            add_answers.Add("fraud Report fraud to the South African Banking Risk Information Centre (SABRIC) and open a case at your local police station.");
            add_answers.Add("fraud Monitor your bank statements regularly for unauthorized transactions and set up transaction alerts on your accounts.");

            // Sentiment responses
            add_answers.Add("frustrated I understand your frustration! Cybersecurity can feel overwhelming. Let's break it down step by step together.");
            add_answers.Add("frustrated It's okay to feel frustrated when things aren't working. I'm here to help you understand online safety better.");
            add_answers.Add("frustrated Take a deep breath. We'll work through this cybersecurity topic together until it makes sense.");

            add_answers.Add("confused That's okay, confusion is normal when learning about cybersecurity. Let me explain it more clearly for you.");
            add_answers.Add("confused Let me break this down into smaller steps so it's easier to understand.");
            add_answers.Add("confused No worries at all! I'll help you understand this cybersecurity concept better.");

            add_answers.Add("worried It's normal to feel worried about online safety. The good news is that simple habits make a huge difference!");
            add_answers.Add("worried Don't panic - most cybersecurity issues can be prevented with basic precautions. I'm here to guide you.");
            add_answers.Add("worried I understand your concern. Let me share some practical steps to help keep your information safe.");

            add_answers.Add("curious That's the right attitude! Being curious about online safety is the first step to protecting yourself.");
            add_answers.Add("curious I love your curiosity! Cybersecurity is fascinating and essential in today's digital world.");
            add_answers.Add("curious Great question! Let me explain how that works in simple terms.");

            add_answers.Add("happy That's wonderful to hear! Staying positive helps with learning. Ready for more cybersecurity tips?");
            add_answers.Add("happy Awesome! I'm glad you're enjoying learning about online safety.");
            add_answers.Add("happy That's great! Keep up the good work protecting yourself online.");

            add_answers.Add("sad I'm sorry you're feeling this way. I'm here to help and support you with cybersecurity.");
            add_answers.Add("sad That sounds tough. Remember that taking small steps to improve your online safety can make a big difference.");
            add_answers.Add("sad I hope things improve for you soon. I'm always here if you need cybersecurity advice or just someone to talk to.");

            add_answers.Add("angry I understand you're angry. Let's focus on solving the cybersecurity issue together.");
            add_answers.Add("angry It's okay to feel upset, but let me help you fix the problem step by step.");
            add_answers.Add("angry Take your time. I'm here to help you work through this and find a solution.");
        }
    }
}