using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GiGaBuGaManager.AdditionClass
{
    public class FileItem
    {
        public Icon Icon { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public string Tags { get; set; }

        public FileItem(Icon icon, string name, string path,string Tags = "")
        {
            Icon = icon;
            Name = name;
            Path = path;
            this.Tags = Tags;
        }
        public override string ToString()
        {
            return $"{Icon} | {Name} | {Path}";
        }
    }
}
