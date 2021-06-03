using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace JgYxs.UI.UserCtrl.Converter
{
    public sealed class Int32Extension : MarkupExtension
    {
        public Int32Extension(int value) { this.Value = value; }
        public int Value { get; set; }
        public override Object ProvideValue(IServiceProvider sp) { return Value; }
    }

    public sealed class BoolExtension : MarkupExtension
    {
        public BoolExtension(bool value) { this.Value = value; }
        public bool Value { get; set; }
        public override Object ProvideValue(IServiceProvider sp) { return Value; }
    }
}
