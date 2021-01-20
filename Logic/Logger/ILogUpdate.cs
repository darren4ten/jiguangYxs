using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Log
{
    public interface ILogUpdate
    {
        void LogUpdate(RichTextParagraph richTextWrapper);
    }
}
