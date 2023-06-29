using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace RollLabelProdPack.Library.Email
{
    /// <summary>
    /// Represents an email item to be sent.
    /// </summary>
    public class EmailItem
    {
        #region Variables

        /// <summary>
        /// The email address of the sender.
        /// </summary>
        private string _fromAddress = string.Empty;

        /// <summary>
        /// The email addresses of the recipients.
        /// </summary>
        private string _toAddress = string.Empty;

        /// <summary>
        /// The email addresses of the carbon copy recipients.
        /// </summary>
        private string _ccAddress = string.Empty;

        /// <summary>
        /// The subject of the email.
        /// </summary>
        private string _subject = string.Empty;

        /// <summary>
        /// The body of the email.
        /// </summary>
        private string _body = string.Empty;

        /// <summary>
        /// The list of email attachments.
        /// </summary>
        private List<EmailAttachment> _attachments = new List<EmailAttachment>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of email attachments.
        /// </summary>
        public List<EmailAttachment> Attachments { get { return _attachments; } set { _attachments = value; } }

        /// <summary>
        /// Gets or sets the body of the email.
        /// </summary>
        public string Body { get { return _body; } set { _body = value; } }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get { return _subject; } set { _subject = value; } }

        /// <summary>
        /// Gets or sets the email addresses of the carbon copy recipients.
        /// </summary>
        public string CCAddress { get { return _ccAddress; } set { _ccAddress = value; } }

        /// <summary>
        /// Gets or sets the email addresses of the recipients.
        /// </summary>
        public string ToAddress { get { return _toAddress; } set { _toAddress = value; } }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string FromAddress { get { return _fromAddress; } set { _fromAddress = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EmailItem class.
        /// </summary>
        public EmailItem() { }

        /// <summary>
        /// Initializes a new instance of the EmailItem class with the specified sender email address.
        /// </summary>
        /// <param name="fromAddress">The email address of the sender.</param>
        public EmailItem(string fromAddress)
        {
            this._fromAddress = fromAddress;
        }

        /// <summary>
        /// Initializes a new instance of the EmailItem class with the specified sender and recipient email addresses.
        /// </summary>
        /// <param name="fromAddress">The email address of the sender.</param>
        /// <param name="toAddress">The email address of the recipient.</param>
        public EmailItem(string fromAddress, string toAddress)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
        }

        /// <summary>
        /// Initializes a new instance of the EmailItem class with the specified sender, recipient, and carbon copy email addresses.
        /// </summary>
        /// <param name="fromAddress">The email address of the sender.</param>
        /// <param name="toAddress">The email address of the recipient.</param>
        /// <param name="ccAddress">The email addresses of the carbon copy recipients.</param>
        public EmailItem(string fromAddress, string toAddress, string ccAddress)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
        }

        /// <summary>
        /// Initializes a new instance of the EmailItem class with the specified sender, recipient, carbon copy email addresses, and subject.
        /// </summary>
        /// <param name="fromAddress">The email address of the sender.</param>
        /// <param name="toAddress">The email address of the recipient.</param>
        /// <param name="ccAddress">The email addresses of the carbon copy recipients.</param>
        /// <param name="subject">The subject of the email.</param>
        public EmailItem(string fromAddress, string toAddress, string ccAddress, string subject)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
            this._subject = subject;
        }

        /// <summary>
        /// Initializes a new instance of the EmailItem class with the specified sender, recipient, carbon copy email addresses, subject, and body.
        /// </summary>
        /// <param name="fromAddress">The email address of the sender.</param>
        /// <param name="toAddress">The email address of the recipient.</param>
        /// <param name="ccAddress">The email addresses of the carbon copy recipients.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        public EmailItem(string fromAddress, string toAddress, string ccAddress, string subject, string body)
        {
            this._fromAddress = fromAddress;
            this._toAddress = toAddress;
            this._ccAddress = ccAddress;
            this._subject = subject;
            this._body = body;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an attachment to the email.
        /// </summary>
        /// <param name="attachment">The path of the attachment file.</param>
        public void AddAttachment(string attachment)
        {
            _attachments.Add(new EmailAttachment(attachment));
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <returns>True if the email was sent successfully, otherwise false.</returns>
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
