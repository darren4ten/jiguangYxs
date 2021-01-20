using System.Collections.Concurrent;

namespace Logic.Log
{
    public class LogMangerBase : ILogManager
    {
        protected ConcurrentBag<RichTextParagraph> Records { get; set; } = new ConcurrentBag<RichTextParagraph>();

        protected ILogUpdate _receiver;

        public LogMangerBase()
        {
        }

        public LogMangerBase(ILogUpdate receiver)
        {
            _receiver = receiver;
        }

        public virtual void AttachRecevier(ILogUpdate receiver)
        {
            _receiver = receiver;
        }

        public virtual void LogAction(RichTextParagraph record)
        {

        }
    }
}
