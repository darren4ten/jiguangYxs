using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Logic.Log
{
    public class FlowDocLogMananger : LogMangerBase, ILogManager
    {
        public FlowDocLogMananger() { }
        public FlowDocLogMananger(ILogUpdate receiver) : base(receiver)
        {
        }

        public override void LogAction(RichTextParagraph record)
        {
            if (_receiver != null)
            {
                _receiver.LogUpdate(record);
            }
            Records.Add(record);
        }
    }

}
