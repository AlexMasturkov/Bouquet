﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.Utility
{
    public class EmailOptions
    {
        public string  SendGridKey { get; set; }
        public string SendGridUser { get; set; }

        public string MailJetKey { get; set; }
        public string MailJetAuth { get; set; }
    }
}
