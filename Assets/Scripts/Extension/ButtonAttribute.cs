using System;

namespace Scripts.Extension
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public string textButton;

        public ButtonAttribute(string text) => textButton = text;
    }
}
