using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Log
{
    public class RichTextParagraph
    {
        public List<RichTextWrapper> Wrappers { get; }
        public RichTextParagraph(List<RichTextWrapper> wrappers)
        {
            Wrappers = wrappers;
        }

        public RichTextParagraph(RichTextWrapper wrapper)
        {
            Wrappers = new List<RichTextWrapper>() { wrapper };
        }

        public RichTextParagraph(params RichTextWrapper[] wrappers)
        {
            Wrappers = new List<RichTextWrapper>() { };
            Wrappers.AddRange(wrappers);
        }
    }
}
