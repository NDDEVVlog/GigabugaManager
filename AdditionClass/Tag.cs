using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GiGaBuGaManager.AdditionClass;


namespace GiGaBuGaManager.AdditionClass
{
    class Tag
    {
       
            public string TagName { get; set; }

            public Color TagButtonColor { get; set; }
            public Color TagBackgroundColor { get; set; }
            public Color TagTextColor { get; set; }

            public Tag() { }

            public Tag(string name, Color buttonColor, Color backgroundColor, Color textColor)
            {
                TagName = name;
                TagButtonColor = buttonColor;
                TagBackgroundColor = backgroundColor;
                TagTextColor = textColor;
            }
        

        public void SetTagName(string name)
        {
            TagName = name;
        }

        public string GetTagName() => TagName;
    }
}
