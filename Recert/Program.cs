using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Recert.IO;
using Recert.UI;

namespace Recert
{
    class Recert
    {
        public static Household Household { get; set; }
        public static MainWindow Window { get; set; }

        [STAThread]
        static void Main(string[] args)
        {
            Household = new Household();
            Window = new MainWindow();
            Window.ShowDialog();
        }
    }
}
