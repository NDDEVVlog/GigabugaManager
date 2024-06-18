using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GiGaBuGaManager.AdditionClass;

namespace GiGaBuGaManager
{
    static class Program
    {
       

        [STAThread]
        static void Main()
        {
            

            // Start the Windows Forms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Gigabuga());
        }

      
    }
}
