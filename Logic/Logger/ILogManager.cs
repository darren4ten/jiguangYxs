using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Log
{
    public interface ILogManager
    {
        void LogAction(RichTextParagraph record);
    }
}
