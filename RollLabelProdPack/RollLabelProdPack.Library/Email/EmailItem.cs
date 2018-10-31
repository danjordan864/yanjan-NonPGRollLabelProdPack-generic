using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Email
{
    public class EmailItem
    {

        #region variables

        /// <summary></summary>
        private string _fromAddress = string.Empty;
        /// <summary></summary>
        private string _toAddress = string.Empty;
        /// <summary></summary>
        private string _ccAddress = string.Empty;
        /// <summary></summary>
        private string _subject = string.Empty;
        /// <summary></summary>
        private string _body = string.Empty;
        /// <summary></summary>
        private List<EmailAttachment> _attachments = new List<EmailAttachment>();

        #endregion

        #region properties

        /// <summary></summary>
        public List<EmailAttachment> Attachments { get { return _attachments; } set { _attachments = value; } }
        /// <summary></summary>
        public string Body { get { return _body; } set { _body = value; } }
        /// <summary></summary>
        public string Subject { get { return _subject; } set { _subject = value; } }
        /// <summary></summary>
        public string CCAddress { get { return _ccAddress; } set { _ccAddress = value; } }
        /// <summary></summary>
        public string ToAddress { get { return _toAddress; } set { _toAddress = value; } }
        /// <summary></summary>
        public string FromAddress { get { return _fromAddress; } set { _fromAddress = value; } }

        #endregion

        #region constructors

        /// <summary></summary>
        public EmailItem() { }

        /// <summary></summary>
        public EmailItem(string fromAddress)
        {
            this._fromAddress = fromAddress;
        }

        /// <summary></summary>
        public EmailItem(string fromAddress, string toAddress)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
        }

        public EmailItem(string fromAddress, string toAddress, string ccAddress)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
        }


        /// <summary></summary>
        public EmailItem(string fromAddress, string toAddress, string ccAddress, string subject)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
            this._subject = subject;
        }

        /// <summary></summary>
        public EmailItem(string fromAddress, string toAddress, string ccAddress, string subject, string body)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
            this._subject = subject;
            this._body = body;
        }

        #endregion

        #region add

        /// <summary></summary>
        public void AddAttachment(string attachment)
        {
            _attachments.Add(new EmailAttachment(attachment));
        }

        #endregion

        #region methods

        /// <summary></summary>
        public bool EmailReport()
        {
            try
            {
                var smtpHost = AppUtility.GetSMTPHost();
                var smtpPort = AppUtility.GetSMTPPort();
                var smtpUseCredentails = AppUtility.GetSMTPUseCredentials();
                var smtpUser = AppUtility.GetSMTPUser();
                var smtpPass = AppUtility.GetSMTPUser();
                var smtpUseTLS = AppUtility.GetSMTPUseTSL();

                string mailHost = smtpHost;
                string[] toAddressList = _toAddress.Split(';');
                string[] ccAddressList = _ccAddress.Split(';');
                MailMessage messageObject = new MailMessage();
                foreach (string email in toAddressList) { messageObject.To.Add(email); }
                foreach (string cc in ccAddressList) { if (cc != string.Empty) { messageObject.CC.Add(cc); } }
                messageObject.Body = _body;
                messageObject.Subject = _subject;
                messageObject.From = new MailAddress(_fromAddress);
                messageObject.IsBodyHtml = true;
                for (int i = 0; i < _attachments.Count; i++)
                {
                    messageObject.Attachments.Add(new Attachment(_attachments[i].AttachmentPath));
                }
                SmtpClient mailClient = new SmtpClient(mailHost);
                if (smtpUseCredentails)
                {
                    System.Net.NetworkCredential baseCredentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = baseCredentials;
                }
                if (smtpUseTLS) { mailClient.EnableSsl = true; mailClient.TargetName = $"STARTTLS/{mailHost}"; }
                mailClient.Port = smtpPort;
                mailClient.Send(messageObject);
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Method: EmailReport | Error Message: {ex.Message}";
                throw new Exception(message);
            }
        }


        #endregion

    }

}
