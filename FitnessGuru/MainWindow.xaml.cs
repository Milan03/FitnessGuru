/*
 * File Name: MainWindow.xaml.cs
 * Date:      May 9, 2013
 * Author:    Milan Sobat
 * Student #: 0469245
 * Course:    INFO-3071
 * Purpose:   This WPF application simulates a form that may be filled out
 *            by a new member of the Fitness Guru Inc. gym. When the form
 *            is filled and the user clicks 'Submit', the information is 
 *            saved to a 'NewMember.txt' file.
 */

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace FitnessGuru
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        enum months { JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC };

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Disable DOB Day and Month combobox
            ComboBoxDOBDay.IsEnabled = false;
            ComboBoxDOBMonth.IsEnabled = false;

            //Date of birth 
            //Year
            for (int i = 1910; i <= DateTime.Now.Year - 16; ++i)
                ComboBoxDOBYear.Items.Add(i);

            //Month
            ComboBoxDOBMonth.ItemsSource = Enum.GetValues(typeof(months));

            //Provinces combobox
            ComboBoxProvince.ItemsSource = new[] { "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT" };

            //Trainers combobox
            ComboBoxTrainers.ItemsSource = new[] { "Tim", "Jannas", "Ryan", "Kimberly", "Michael", 
                                                    "Nick", "Tiffany", "Aaron", "Stefan", "Caroline" };
        }
        private void ComboBoxDOBYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxDOBMonth.IsEnabled = true;
        }
        private void ComboBoxDOBMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Date of birth (day)
                //First find the year and month selected
                //Add proper days according to month/year
                ComboBoxDOBDay.Items.Clear();
                ComboBoxDOBDay.IsEnabled = true;
                object oYear = ComboBoxDOBYear.SelectedItem;
                int selectedYear = (int)oYear;
                int selectedMonth = ComboBoxDOBMonth.SelectedIndex + 1;

                for (int i = 1; i <= DateTime.DaysInMonth(selectedYear, selectedMonth); ++i)
                    ComboBoxDOBDay.Items.Add(i);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " +ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Close comfirmation
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            MessageBoxResult answer = MessageBox.Show("Do you really want to exit?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
       
        // Tracking program level choice for output
        string prgLvlChoice = "";
        private void RadioButtonBeginner_Checked(object sender, RoutedEventArgs e)
        {
            prgLvlChoice = (string)RadioButtonBeginner.Content;
        }
        
        private void RadioButtonExperienced_Checked(object sender, RoutedEventArgs e)
        {
            prgLvlChoice = (string)RadioButtonExperienced.Content;
        }
        
        private void RadioButtonExpert_Checked(object sender, RoutedEventArgs e)
        {
            prgLvlChoice = (string)RadioButtonExpert.Content;
        }
        
        // Tracking of payment option input for outpout
        string finalString = "";

        private void CheckBoxDD_Checked(object sender, RoutedEventArgs e)
        {
            finalString += "|" + CheckBoxDD.Content + "|";
        }
        private void CheckBoxDD_Unchecked(object sender, RoutedEventArgs e)
        {
            finalString = "";
            if (CheckBoxAnnual.IsChecked == true)
                finalString += (string)("|" + CheckBoxAnnual.Content + "|");
            if (CheckBoxMonthly.IsChecked == true)
                finalString += (string)("|" + CheckBoxMonthly.Content + "|");
        }
        
        private void CheckBoxMonthly_Checked(object sender, RoutedEventArgs e)
        {
            finalString +=  "|" + CheckBoxMonthly.Content + "|";
        }
        private void CheckBoxMonthly_Unchecked(object sender, RoutedEventArgs e)
        {
            finalString = "";
            if (CheckBoxDD.IsChecked == true)
                finalString += (string)("|" + CheckBoxDD.Content + "|");
            if (CheckBoxAnnual.IsChecked == true)
                finalString += (string)("|" + CheckBoxAnnual.Content + "|");
        }

        private void CheckBoxAnnual_Checked(object sender, RoutedEventArgs e)
        {
            finalString += "|" + CheckBoxAnnual.Content + "|";
        }
        private void CheckBoxAnnual_Unchecked(object sender, RoutedEventArgs e)
        {
            finalString = "";
            if (CheckBoxDD.IsChecked == true)
                finalString += (string)("|" + CheckBoxDD.Content + "|");
            if (CheckBoxMonthly.IsChecked == true)
                finalString += (string)("|" + CheckBoxMonthly.Content + "|");
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Check to see if everything is filled out
            if ( TextBoxFN.Text.Trim().Length == 0 || TextBoxLN.Text.Trim().Length == 0 ||
                ComboBoxDOBYear.SelectedIndex < 0 || ComboBoxDOBMonth.SelectedIndex < 0 ||
                ComboBoxDOBDay.SelectedIndex < 0 || TextBoxAddress.Text.Trim().Length == 0 ||
                TextBoxCity.Text.Trim().Length == 0 || ComboBoxProvince.SelectedIndex < 0 ||
                TextBoxComments.Text.Trim().Length == 0 || ComboBoxTrainers.SelectedIndex < 0 )
                MessageBox.Show("Please fill in the missing personal information.", "Not Complete", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if ( RadioButtonBeginner.IsChecked == false && RadioButtonExperienced.IsChecked == false &&
                RadioButtonExpert.IsChecked == false )
                MessageBox.Show("Please fill in the program level.", "Not Complete", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (CheckBoxDD.IsChecked == false && CheckBoxMonthly.IsChecked == false && 
                CheckBoxAnnual.IsChecked == false)
                MessageBox.Show("Please fill in the payment info.", "Not Complete", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else //if all OK, save to file
            {
                StreamWriter outStream = new StreamWriter("NewMember.txt");
                outStream.WriteLine("First Name: \t\t" + TextBoxFN.Text
                                        + "\r\nLast Name: \t\t" + TextBoxLN.Text
                                        + "\r\nDOB (Year/Month/Day): \t" + ComboBoxDOBMonth.SelectedItem + " " + ComboBoxDOBDay.SelectedItem + ", "
                                            + ComboBoxDOBYear.SelectedItem
                                        + "\r\nAddress: \t\t" + TextBoxAddress.Text
                                        + "\r\nCity: \t\t\t" + TextBoxCity.Text
                                        + "\r\nProvince: \t\t" + ComboBoxProvince.SelectedItem
                                        + "\r\nPostal Code: \t\t" + TextBoxPC.Text
                                        + "\r\nComments: \r\n"
                                            + TextBoxComments.Text
                                        + "\r\nTrainer: \t\t" + ComboBoxTrainers.SelectedItem
                                        + "\r\nProgram Level: \t\t" + prgLvlChoice
                                        + "\r\nPayment Method: \t" + finalString);
                outStream.Close();
                MessageBox.Show("Your information has been saved, thank you!", "Form Complete", MessageBoxButton.OK, MessageBoxImage.Information);                
            }
        }



















        

        

        
    }
}
