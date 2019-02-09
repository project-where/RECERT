using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recert.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Household Household;

        public MainWindow()
        {
            Household = new Household();
            InitializeComponent();
            this.PopulateStates();
            this.stateComboBox.SelectedIndex = 0;
            this.PersonGrid.Visibility = Visibility.Hidden;
            this.BirthDatePicker.SelectedDate = DateTime.Today;
            this.BirthDatePicker.DisplayDate = DateTime.Today;
            this.BirthDatePicker.DisplayDateStart = new DateTime(1880, 1, 1);
            this.BirthDatePicker.DisplayDateEnd = DateTime.Today;
        }

        public string SplitCamelCase(string name)
        {
            string s = string.Empty;
            foreach (char ch in name)
            {
                if (ch.ToString().ToUpper() == ch.ToString())
                {
                    s += $" {ch}";
                }
                else
                {
                    s += ch.ToString();
                }
            }
            return s.Substring(1);
        }

        public void PopulateStates()
        {
            for(int i = 0; i <= 58; i++)
            {
                this.stateComboBox.Items.Add(this.SplitCamelCase(((State)i).ToString()));
            }
        }

        private void RefreshPeopleList()
        {
            this.peopleList.Items.Clear();
            foreach (Person p in this.Household.People)
            {
                this.peopleList.Items.Add(p);
            }
        }

        private void RefreshAccountList()
        {
            this.accountsList.Items.Clear();
            Person p = this.peopleList.SelectedItem as Person;
            foreach(Account a in p.Accounts.Accounts)
            {
                this.accountsList.Items.Add(a);
            }
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            this.Household.People.Add(new Person());
            RefreshPeopleList();
        }

        private void RemovePersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.peopleList.SelectedIndex != -1)
            {
                this.Household.People.RemoveAt(this.peopleList.SelectedIndex);
            }
            RefreshPeopleList();
        }

        private void PeopleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.peopleList.SelectedIndex != -1)
            {
                this.PersonGrid.Visibility = Visibility.Visible;
                this.selectNameLabel.Visibility = Visibility.Hidden;
                Person person = this.Household.People[this.peopleList.SelectedIndex];

                // Income
                this.firstNameField.Text = person.FirstName;
                this.lastNameField.Text = person.LastName;
                this.BirthDatePicker.SelectedDate = person.BirthDate;
                this.BirthDatePicker.DisplayDate = person.BirthDate;
                this.incomeCheckBox.IsChecked = person.Income.EmploymentIncome;
                this.ssCheckBox.IsChecked = person.Income.SS;
                this.ssiCheckBox.IsChecked = person.Income.SSI;
                this.sspCheckBox.IsChecked = person.Income.SSP;
                this.unemploymentCheckBox.IsChecked = person.Income.ReceivesUnemployment;
                this.informalAlimonyCheckBox.IsChecked = person.Income.ReceivesInformalAlimony;
                this.courtOrderAlimonyCheckBox.IsChecked = person.Income.ReceivesCourtOrderedAlimony;
                this.contributionsCheckBox.IsChecked = person.Income.Contributions;
                this.debitCardCheckBox.IsChecked = person.Income.DirectDebitCard;

                // Education
                this.graduatedHighSchoolCheckBox.IsChecked = person.Education.GraduatedHighSchool;
                this.fullTumeStudentCheckBox.IsChecked = person.Education.FullTimeStudent;

                // Employment
                this.employedCheckBox.IsChecked = person.Employment.Employed;
                this.selfEmployedCheckBox.IsChecked = person.Employment.SelfEmployed;

                // Life insurance
                this.lifeInsuranceComboBox.SelectedIndex = (int)person.LifeInsurance;

                // Accounts
                this.accountsList.Items.Clear();
                foreach(Account ac in person.Accounts.Accounts)
                {
                    this.accountsList.Items.Add(ac);
                }
            }
            else
            {
                this.PersonGrid.Visibility = Visibility.Hidden;
                this.selectNameLabel.Visibility = Visibility.Visible;
            }
        }

        private void HouseholdNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            this.Household.HouseholdName = box.Text;
        }

        private void HouseholdStreetNumberField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            try { this.Household.Address.Number = int.Parse(box.Text); }
            catch { }
        }

        private void HouseholdStreetNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            this.Household.Address.Street = box.Text;
        }

        private void HouseholdhouseholdApartmentField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            this.Household.Address.Apartment = box.Text;
        }

        private void HouseholdhouseholdCityField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            this.Household.Address.City = box.Text;
        }

        private void StateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            this.Household.Address.State = (State)box.SelectedIndex;
        }

        private void HouseholdhouseholdCountryField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            this.Household.Address.Country = box.Text;
        }

        private void HouseholdZIPField_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            try { this.Household.Address.ZipCode = int.Parse(box.Text); }
            catch { }
            
        }

        private void FirstNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            int selected = this.peopleList.SelectedIndex;
            TextBox box = sender as TextBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.FirstName = box.Text;
            this.RefreshPeopleList();
            this.peopleList.SelectedIndex = selected;
        }

        private void LastNameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            int selected = this.peopleList.SelectedIndex;
            TextBox box = sender as TextBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.LastName = box.Text;
            this.RefreshPeopleList();
            this.peopleList.SelectedIndex = selected;
        }

        private void IncomeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.EmploymentIncome = (bool)box.IsChecked;
        }

        private void SsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.SS = (bool)box.IsChecked;
        }

        private void SsiCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.SSI = (bool)box.IsChecked;
        }

        private void SspCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.SSP = (bool)box.IsChecked;
        }

        private void UnemploymentCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.ReceivesUnemployment = (bool)box.IsChecked;
        }

        private void InformalAlimonyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.ReceivesInformalAlimony = (bool)box.IsChecked;
        }

        private void CourtOrderAlimonyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.ReceivesCourtOrderedAlimony = (bool)box.IsChecked;
        }

        private void ContributionsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.Contributions = (bool)box.IsChecked;
        }

        private void DebitCardCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Income.DirectDebitCard = (bool)box.IsChecked;
        }

        private void GraduatedHighSchoolCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Education.GraduatedHighSchool = (bool)box.IsChecked;
        }

        private void FullTumeStudentCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Education.FullTimeStudent = (bool)box.IsChecked;
        }

        private void EmployedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Employment.Employed = (bool)box.IsChecked;
        }

        private void SelfEmployedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Person p = this.peopleList.SelectedItem as Person;
            p.Employment.SelfEmployed = (bool)box.IsChecked;
        }

        private void LifeInsuranceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            Person p = this.peopleList.SelectedItem as Person;
            if (p != null)
            {
                p.LifeInsurance = (LifeInsuranceType)box.SelectedIndex;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Person p = this.peopleList.SelectedItem as Person;
            Account ac = new Account();
            ac.Type = (AccountType)accountTypeComboBox.SelectedIndex;
            p.Accounts.Accounts.Add(ac);
            RefreshAccountList();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Person p = this.peopleList.SelectedItem as Person;
            if (this.accountsList.SelectedIndex != -1)
            {
                p.Accounts.Accounts.RemoveAt(this.accountsList.SelectedIndex);
            }
            RefreshAccountList();
        }

        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;
            Person p = this.peopleList.SelectedItem as Person;
            if (p != null)
            {
                p.BirthDate = (DateTime)picker.SelectedDate;
            }
        }

        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            this.Household = new Household();
            DisplayObject(this.Household);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Household household;
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Load household profile";
            open.Filter = "Serialized JSON|*.json";
            open.ShowDialog();
            if (open.FileName != null && open.FileName != string.Empty)
            {
                using (StreamReader reader = new StreamReader(open.FileName))
                {
                    household = JsonConvert.DeserializeObject<Household>(reader.ReadToEnd());
                }

                this.DisplayObject(household);
            }
        }

        public void DisplayObject(Household household)
        {
            this.householdNameField.Text = household.HouseholdName;
            this.householdStreetNumberField.Text = household.Address.Number.ToString();
            this.householdStreetNameField.Text = household.Address.Street;
            this.householdhouseholdApartmentField.Text = household.Address.Apartment;
            this.householdhouseholdCityField.Text = household.Address.City;
            this.stateComboBox.SelectedIndex = (int)household.Address.State;
            this.householdhouseholdCountryField.Text = household.Address.Country;
            this.householdZIPField.Text = household.Address.ZipCode.ToString();

            if(this.householdZIPField.Text == "0")
            {
                this.householdZIPField.Text = string.Empty;
            }
            if(this.householdStreetNumberField.Text == "0")
            {
                this.householdStreetNumberField.Text = string.Empty;
            }

            try
            {
                this.Household = household;
                this.RefreshPeopleList();
                this.RefreshAccountList();
            }
            catch { }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".json";
            saveDialog.Filter = "Serialized JSON|*.json";
            saveDialog.ShowDialog();
            if (saveDialog.FileName != null && saveDialog.FileName != string.Empty)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    writer.Write(JsonConvert.SerializeObject(this.Household, Formatting.Indented));
                }
            }
        }


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://proj-where.com/recert-py-1");
        }

        private void GitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/project-where/Recert");
        }

        private void GenerateReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (this.peopleList.SelectedItem != null)
            {
                ReceiptDialog receipt = new ReceiptDialog(this.Household, (Person)this.peopleList.SelectedItem);
                receipt.ShowDialog();
            }
            else
            {
                //MessageBox.Show("Please, select a name from the list before generating a receipt", "Select Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
