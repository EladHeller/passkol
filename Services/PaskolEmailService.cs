using Infrastructure.Email;
using System.Collections.Generic;
using System.Linq;
using Model.Auctions;
using Infrastructure.Helper;
using System.Data.Entity;
using Repository;
using Repository.Interfaces;
using Model;
using System;
using Infrastructure.Configuration;

namespace Services
{
    public class PaskolEmailService
    {
        private const string ARTIST_DECLINE = "  בקשתך {0} אמן נדחתה  ";
        private const string MUSIC_DECLINE = "  בקשתך {0} מוסיקה נדחתה  ";
        private const string PHONE_PURCHASE_OK = "  בקשתך לרכישת יצירה אושרה";
        private const string CLOSE_NOTIFY_NOT_WON = "לא זכית במכרז ";
        private const string CLOSE_NOTIFY_CUSTOMER = "המכרז נסגר בהצלחה";
        private const string OPEN_INVITE_ARTIST = "הנך מוזמן להשתתף במכרז";
        private const string RESET_PASSWORD = "הקוד שלך לאיפוס סיסמה הינו: {0}";
        private const string PURCHASE_MUSIC_OK = "הרכישה בוצעה בהצלחה";

        private IEmailService _emailService;
        private MailUnsubscribeRepository _unsbscribeRep;

        public PaskolEmailService(IEmailService emailSrv, MailUnsubscribeRepository unsbscribeRep)
        {
            this._unsbscribeRep = unsbscribeRep;
            this._emailService = emailSrv;
        }

        public void NewArtistArtAgntRelationShip(string ToEmail, string Subscriber, bool ToArtist, string confirmUrl,string removeUrl)
        {
            string userType = ToArtist ? "אמן" : "סוכן";
            string OnUserType = ToArtist ? "סוכן" : "אמן";
            SendEmail(ToEmail, string.Format("נרשמת כ{0} בשיתוף עם ה{1} {2}", userType,OnUserType,Subscriber),null,
                string.Format(@"שלום {0},<br />
                                התקבלה במערכת פסקול בקשה לרשום אותך כ{0} בשיתוף עם {1} {2}. אם הנך מאשר בקשה זו – <a href=""{3}"" target=""_blank"">לחץ כאן</a> אם אינך מכיר את ה{0} זה – <a href=""{4}"" target=""_blank"">לחץ כאן</a>: ואנו נקבל דיווח בעניין.
                                <br />
                                בהצלחה,<br />
                                צוות פסקול
                                ", userType,OnUserType,Subscriber, confirmUrl,removeUrl));
        }

        public void AlertBuyerPurchaseSuccess(string emailBuyer, string musicUrl)
        {
            SendEmail(emailBuyer, "מזל טוב על רכישת היצירה החדשה.",
                                  "מזל טוב על רכישת היצירה החדשה.",
                                string.Format(@"מזל טוב על רכישת היצירה החדשה.<br>
                                        <a href=""{0}"" target=""_blank"">לחץ כאן</a> להורדת היצירה.<br>
                                        למידע אודות הרכישה <a href=""{1}"" target=""_blank"">לחץ כאן</a><br>
                                        <br>
                                        תודה,<br>
                                        צוות פסקול", musicUrl, CommonHelper.GetBaseUrl() + "Customer/PurchaseReport"));
        }

        public void AlertArtistPurchaseSuccess(string emailArtist)
        {
            SendEmail(emailArtist, "כרגע בוצעה רכישה של היצירה שלך!",
                                  "כרגע בוצעה רכישה של היצירה שלך!",
                                string.Format(@"מזל טוב!<br>
                                    כרגע בוצעה רכישה של היצירה שלך!<br>
                                    למידע אודות הרכישה <a href=""{0}"" target=""_blank"">לחץ כאן</a><br>
                                    אנחנו מזמינים אותך להעלות יצירות נוספות.<br>
                                    תודה,<br>
                                    צוות פסקול<br>
                        ", CommonHelper.GetBaseUrl() + "Artist/SellReport"));
        }

        public void ResetPassword(string code, string mail, string name)
        {
            SendEmail(mail, "איפוס סיסמה לאתר פסקול", "שחזור סיסמה", 
                string.Format(@"שלום {0},<br>
                            בהמשך לבקשתך, <br>
                            אנו שולחים לך בזאת את סיסמתך האישית לכניסה לאתר:<br>
                            שם משתמש: {1}<br>
                            סיסמא: {2}<br>
                            תוכל לשנות את סיסמתך בכל עת, על ידי כניסה ללשונית החשבון שלי שבאתר.<br>
                            למעבר לדף החשבון שלי באתר <a href=""{3}"" target=""_blank"">לחץ כאן</a> <br>
                            על מנת לשמור על פרטיותך אנא שמור על פרטים אלו. <br>
                            צוות פסקול<br>", 
            name,mail,code,CommonHelper.GetBaseUrl() + "Customer/MyProfile"));
        }

        public void RegisterArtist(string mail, string name,string Password)
        {
            var baseUrl = CommonHelper.GetBaseUrl();
            SendEmail(mail, "הרשמה לפסקול", null,
                string.Format(@"שלום {0},<br>
                            ברוך הבא לפסקול! <br>
                            אנו שמחים שבחרת בנו ומתרגשים להכיר לך את הפלטפורמה החדשה והמתקדמת בארץ למסחור היצירות שלך.עשרות פירסומאים, במאים ומנהלי תוכן נמצאים כאן בכדי למצוא את היצירה שלך!<br>
                            <br>
                            בהמשך לתהליך ההרשמה, אנא קרא את הסכם האמן\סוכן ואשר אותו בעמוד האישי שלך - 	בכדי להתחיל אנו ניקח אותך צעד צעד בבניית עמוד האמן שלך - <a href=""{4}"" target=""_blank"">לחץ כאן</a><br>
                            <br>
                             שם משתמש: {1}<br>
                            סיסמא: {2}<br>
                            <br>
                            לשאלות ותשובות <a href=""{3}"" target=""_blank"">לחץ כאן</a><br>
                            <br>
                            צוות פסקול<br>",
            name, mail, Password, baseUrl + "Info/faq", baseUrl + "Artist/MyProfile?isFirstEntrance=1"));
        }

        public void RegisterArtistAgnt(string mail, string name, string Password)
        {
            var baseUrl = CommonHelper.GetBaseUrl();
            SendEmail(mail, "הרשמה לפסקול", null,
                string.Format(@"שלום {0},<br>
                           ברוך הבא לפסקול! <br>
                            אנו שמחים שבחרת בנו ומתרגשים להכיר לך את הפלטפורמה החדשה והמתקדמת בארץ למסחור היצירות ושל האמנים שלך. עשרות פירסומאים, במאים ומנהלי תוכן נמצאים כאן בכדי למצוא את היצירה שלך!<br>
                            בהמשך לתהליך ההרשמה, אנא קרא את הסכם האמן\סוכן ואשר אותו בעמוד האישי שלך - בכדי להתחיל אנו ניקח אותך צעד צעד בבניית עמוד הסוכן שלך - <a href=""{4}"" target=""_blank"">לחץ כאן</a><br>
                            <br>
                             שם משתמש: {1}<br>
                            סיסמא: {2}<br>
                            <br>
                            לשאלות ותשובות <a href=""{3}"" target=""_blank"">לחץ כאן</a><br>
                            <br>
                            צוות פסקול<br>",
            name, mail, Password, baseUrl + "Info/faq", baseUrl + "ArtistAgent/MyProfile"));
        }

        public void RegisterCustomer(string mail, string name, string Password)
        {
            var baseUrl = CommonHelper.GetBaseUrl();
            SendEmail(mail, "הרשמה לפסקול", null,
                string.Format(@"שלום {0},<br>
                          אנו שמחים שבחרת בנו ומתרגשים להכיר לך את הפלטפורמה החדשה והמתקדמת בארץ למציאת תוכן מוזיקלי לפרוייקטים השונים שלך! עשרות אמנים, יוצרים מקוריים, מלחינים ונגנים נמצאים כאן בכדי לתת לך את הספרייה העשירה והמגוונת ביותר של תוכן מוזיקלי בארץ, במרחק של שניות ספורות.	
                            אנו ניקח אותך צעד צעד ונראה לך כיצד למצוא את התוכן הנכון בפסקול – <a href=""{4}"" target=""_blank"">לחץ כאן</a>
                             <br>
                            שם משתמש: {1}<br>
                            סיסמא: {2}<br>
                            <br>
                            לשאלות ותשובות <a href=""{3}"" target=""_blank"">לחץ כאן</a><br>
                            <br>
                            צוות פסקול<br>",
            name, mail, Password, baseUrl + "Info/faq", baseUrl + "Customer/MyProfile"));
        }

        public void DeclineNewUpdateMusic(string To, string Reason, string musicName)
        {
            SendEmail(To, "עדכון יצירה לא אושר", "עדכון יצירה לא אושר", 
                string.Format(@"היצירה {0} שהעלית למערכת, לא אושרה לשימוש ע""י מנהל המערכת.<br>
                                סיבת הסירוב: {1}<br>
                                להעלאה מחדש של היצירה, <a href=""{2}"" target=""_blank"">לחץ כאן</a><br>
                                תודה,<br>
                                צוות פסקול<br>
                                ", musicName, Reason, CommonHelper.GetBaseUrl() + "Artist/MySongs"));
        }

        public void DeclineNewUpdateArtist(string To, string Reason)
        {
            
            SendEmail(To, "עדכון אמן לא אושר", "עדכון אמן לא אושר", 
                string.Format(@"משתמש האמן שפתחת במערכת, לא אושרה לשימוש ע""י מנהל המערכת.<br>
                            סיבת הסירוב: {0}<br>
                            לפתיחה מחדש של האמן במערכת, <a href=""{1}"" target=""_blank"">לחץ כאן</a><br>
                            תודה,<br>
                            צוות פסקול<br>
                            ", Reason, CommonHelper.GetBaseUrl()));
        }
        
        public bool ShareMusic(string userMail, string friendMail, string comments, bool sendCopy, string musicURL)
        {
            comments = comments ?? string.Empty;
            comments = comments.Replace("\n", "<br />");

            bool success = true;
            string subject = string.Format("{0} שיתף איתך יצירה מפסקול", userMail);
            success &= SendEmail(friendMail, subject,subject, comments, "עבור ליצירה", musicURL);

            if (sendCopy)
            {
                success &= SendEmail(userMail, subject, subject, comments, "עבור ליצירה", musicURL);
            }
            return success;

        }

        public void CloseAuction(Auction a)
        {
            // Notify not won artist
            List<string> NotWon = new List<string>();
            NotWon.Add("maozimoshe@gmail.com"); // just for test

            // Get not won user

            if (a.WinnerArtist != null && a.Participants != null)
            {
                NotWon.AddRange(a.Participants
                .Where(u => u.Id != a.WinnerArtist.Id)
                .Select(c => c.User.Email));
                // Send to not won user

                foreach (var nwa in NotWon)
                {
                    SendEmail(nwa, CLOSE_NOTIFY_NOT_WON,
                          string.Format(@"שלום {0}, <br>
                        ברצוננו להודיע לך שהמכרז ליצירת מוזיקה מקורית לפרוייקט {1} הסתיים.
                        תודה על השתתפותך!<br><br>", a.WinnerArtist.UserName, a.AuctionName),
                        string.Format(@"יוצר המכרז החליט שלא להשתמש ביצירה שהגשת, היצירה שלך תעלה אוטומטית לעמוד האמן שלך ותתווסף אוטומטית לרפטואר שלך בפסקול.<br>
                                        אם תרצה לערוך שינויים או להסיר את היצירה – <a href=""{0}"" target=""_blank"">לחץ כאן</a> בכדי לעבור לעמוד האישי שלך. <br>
                                        <br>
                                        אנו מקווים לראותך שוב במכרז הבא!<br>
                                        צוות פסקול<br>
                                        ",CommonHelper.GetBaseUrl() + "Artist/MySongs"));
                }

                // Send to won user
                SendEmail(a.WinnerArtist.Email, "זכית במכרז",
                        string.Format(@"שלום {0}, <br>
                        ברצוננו להודיע לך שהמכרז ליצירת מוזיקה מקורית לפרוייקט {1} הסתיים.
                        תודה על השתתפותך!<br><br>",a.WinnerArtist.UserName, a.AuctionName),
                        string.Format(@"ברכות! יוצר המכרז החליט להתשמש ביצירה שהגשת!<br>
                        אנחנו נמשיך בתהליך מול יוצר המכרז ונעדכן אותך ממש בקרוב לגבי פרטים נוספים, תוכל להתעדכן בהתקדמות גם בעמוד האמן שלך - <a href=""{0}"" target=""_blank"">לחץ כאן</a><br>
                        צוות פסקול
                        ", CommonHelper.GetBaseUrl() + "Artist/OriginalMusic"));
            }

            // Send to customer
            SendEmail(a.Customer.Email, "המכרז הסתיים",
                    "המכרז הסתיים",
                    String.Format(@"המכרז הסתיים וכעת אתה מוזמן לבחון את ההצעות השונות שקיבלת ולבחור את הזוכה.<br>
                        <a href=""{0}"" target=""_blank"">לחץ כאן</a> לעמוד מכרז.<br>
                        תודה,<br>
                        צוות פסקול<br>", CommonHelper.GetBaseUrl() + "Customer/OriginalMusic"));
        }

        public void MailToTeam(string firstName, string lastName, string email, string phone, string content)
        {
            SendEmail(WebConf.TeamMail, "התקבלה פנייה מצור קשר", "",
                string.Format(@"
                <strong>שם פרטי</strong> - {0}<br/>
                <strong>שם משפחה</strong> - {1}<br/>
                <strong>טלפון</strong> - {2}<br/>
                <strong>דוא""ל</strong> - {3}<br/>
                <strong>תוכן הפניה</strong>: <br/>
                {4}
                ", firstName,lastName,phone,email,content.Replace("\n","<br/>")));
        }

        public void InviteArtistToAuction(Auction a)
        {
            List<string> Artist = new List<string>();
            Artist.AddRange(a.Participants.Select(c => (c.User.Email)));

            foreach (var ar in Artist)
            {
                SendEmail(ar, OPEN_INVITE_ARTIST,
                OPEN_INVITE_ARTIST,
                string.Format(@"נפתח מכרז חדש הרלוונטי לתחומך ואנו מזמינים אותך להשתתף.
                        <a href=""{0}"" target=""_blank"">לחץ כאן</a> לעמוד מכרז.<br>
                        תודה,<br>
                        צוות פסקול<br>
                        ", CommonHelper.GetBaseUrl() + "Artist/OriginalMusic"));
            }
        }

        private bool SendEmail(string to, string subject, string title, string text)
        {
            return SendEmail(to,subject,title,text,"","");
        }

        private bool SendEmail(string to, string subject, string title, string text, 
            string buttomeText, string buttonUrl)
        {
            if (CanSendEmail(to))
            {
                return this._emailService.sendMail(to, subject, title, text, GetTokenForEmail(to),buttomeText, buttonUrl);
            }
            return true;
        }

        private bool CanSendEmail(string email)
        {
            bool canSend = true;
            var Unsubscribe = _unsbscribeRep.GetByID(email)?.Unsubscribe;
            return Unsubscribe == null ? canSend : !Unsubscribe.Value;
        }

        public bool UnsubscribeMail(string Email, string token)
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(token))
            {
                // if this is the user that allowed to unsubscribe
                var emailToUnSubscribe = _unsbscribeRep.GetByID(Email);
                if (emailToUnSubscribe?.Guid == token)
                {
                    // UnSubscribe
                    emailToUnSubscribe.Unsubscribe = true;
                    _unsbscribeRep.Update(emailToUnSubscribe);
                    _unsbscribeRep.Uow.Commit();
                    return true;
                }
            }

            return false;
        }

        private string GetTokenForEmail(string email)
        {
            var EmailEntity = _unsbscribeRep.GetByID(email);
            if (EmailEntity == null)
            {
                EmailEntity = new MailUnsubscribe() { Email = email, Guid = Guid.NewGuid().ToString(), Unsubscribe = false };
                _unsbscribeRep.Add(EmailEntity);
                _unsbscribeRep.Uow.Commit();
            }

            return EmailEntity.Guid;
        }
    }
}
