using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Email
{
    public class EmailAttachment
    {

        #region variables

        /// <summary></summary>
        private string _attachmentPath = string.Empty;

        #endregion

        #region properties

        /// <summary></summary>
        public string AttachmentPath { get { return _attachmentPath; } set { _attachmentPath = value; } }

        #endregion

        #region constructors

        /// <summary></summary>
        public EmailAttachment(string attachment)
        {
            _attachmentPath = attachment;
        }

        #endregion


    }
}
