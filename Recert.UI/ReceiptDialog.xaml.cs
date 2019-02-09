using Microsoft.Win32;
using Recert.IO;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Drawing.Printing;
using System.Drawing;

namespace Recert.UI
{
    /// <summary>
    /// Interaction logic for ReceiptDialog.xaml
    /// </summary>
    public partial class ReceiptDialog : Window
    {
        private Household household { get; set; }
        private Person person { get; set; }

        public ReceiptDialog(Household h, Person p)
        {
            this.household = h;
            this.person = p;
            InitializeComponent();
            this.GeerateReceipt();
        }

        private void GeerateReceipt()
        {
            this.textBox.AppendText($"Receipt from {DateTime.Now.ToShortDateString()}\n");
            this.textBox.AppendText("\n");
            Announce("Personal detals");
            this.textBox.AppendText($"First Name:       {this.person.FirstName}\n");
            this.textBox.AppendText($"Last Name:        {this.person.LastName}\n");
            this.textBox.AppendText($"Age to date:      {this.person.GetAge()}\n");
            this.textBox.AppendText("\n");
            this.textBox.AppendText($"Household Name:   {this.household.HouseholdName}\n");
            this.textBox.AppendText($"Address:          {this.household.Address.Number} {this.household.Address.Street}\n");
            if (this.household.Address.Apartment != string.Empty)
            {
                this.textBox.AppendText($"                  Apt: {this.household.Address.Apartment}\n");
            }
            this.textBox.AppendText($"                  {this.household.Address.City}, {this.household.Address.State}\n");
            this.textBox.AppendText($"                  {this.household.Address.Country}\n");
            this.textBox.AppendText($"                  {this.household.Address.ZipCode}\n");
            this.textBox.AppendText("\n");
            this.textBox.AppendText("\n");

            Announce("Document List");
            this.person.Documents.Sort();
            foreach (Document doc in this.person.Documents.Documents)
            {
                if (doc.Comments == null || doc.Comments == string.Empty)
                {
                    this.textBox.AppendText($"[ ]  {doc.ToString()}\n\n");
                }
                else
                {
                    this.textBox.AppendText($"[ ]  {doc.ToString()} | {doc.Comments}\n\n");
                }
            }
        }

        private void Announce(string message)
        {
            string delimeter = string.Empty;
            foreach(char ch in message) { delimeter += "="; }
            this.textBox.AppendText(delimeter + "\n");
            this.textBox.AppendText(message.ToUpper() + "\n");
            this.textBox.AppendText(delimeter + "\n");
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(new TextRange(this.textBox.Document.ContentStart, this.textBox.Document.ContentEnd).Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "(Rich Text File)|*.rtf";
            saveDialog.Title = "Save Receipt";
            saveDialog.DefaultExt = ".rtf";
            saveDialog.ShowDialog();
            
            if (saveDialog.FileName != null && saveDialog.FileName != string.Empty)
            {
                TextRange range;
                FileStream fStream;
                range = new TextRange(this.textBox.Document.ContentStart, this.textBox.Document.ContentEnd);
                fStream = new FileStream(saveDialog.FileName, FileMode.Create);
                range.Save(fStream, DataFormats.Rtf);
                fStream.Close();

            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument p = new PrintDocument();
            p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(new TextRange(this.textBox.Document.ContentStart, this.textBox.Document.ContentEnd).Text, new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

            };
            try
            {
                List<string> printers = new List<string>();
                foreach(var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    printers.Add(printer.ToString());
                }
                PrinterDialog selectPrinter = new PrinterDialog(printers);
                selectPrinter.ShowDialog();

                if (selectPrinter.Print)
                {
                    p.PrinterSettings.PrinterName = (string)selectPrinter.listBox.SelectedItem;
                    p.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry, something went horribly wrong while printing your document :/", "Printing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
