using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Recert.UI
{
    /// <summary>
    /// Interaction logic for PrinterDialog.xaml
    /// </summary>
    public partial class PrinterDialog : Window
    {
        public bool Print = false;
        public PrinterDialog(List<string> printers)
        {
            InitializeComponent();
            foreach(string p in printers)
            {
                this.listBox.Items.Add(p);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButon_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBox.SelectedItem != null)
            {
                this.Print = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Printer not selected", "Please, select a printer to use", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
