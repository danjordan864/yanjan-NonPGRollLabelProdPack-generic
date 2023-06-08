namespace RollLabelProdPack.Library.Email
{
    /// <summary>
    /// Represents an email attachment.
    /// </summary>
    public class EmailAttachment
    {
        #region Variables

        /// <summary>
        /// The path of the attachment file.
        /// </summary>
        private string _attachmentPath = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the path of the attachment file.
        /// </summary>
        public string AttachmentPath { get { return _attachmentPath; } set { _attachmentPath = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EmailAttachment class with the specified attachment path.
        /// </summary>
        /// <param name="attachment">The path of the attachment file.</param>
        public EmailAttachment(string attachment)
        {
            _attachmentPath = attachment;
        }

        #endregion
    }
}
