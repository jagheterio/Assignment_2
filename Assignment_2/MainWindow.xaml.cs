using Assignment_2.Models;
using Assignment_2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Assignment_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Instantiate a list "contacts" and FileManager class.
        // The list will be saved in a json file "wpf_addressbook" on the desktop.
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
        private FileManager _fileManager = new FileManager();
        private string _filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\wpf_addressbook.json";

        public MainWindow()
        {
            InitializeComponent();

            // When the application starts, the list "contacts" will be showed in the ListView.
            try
            {
                contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(_fileManager.Read(_filePath));
                lv_Contacts.ItemsSource = contacts;
            }
            catch
            { }
        }

        // By clicking "Add to Contact" button, save the object in the json file.
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Contacts.SelectedItem == null)
            {
                // If there is no object in the list, text will be saved as an object in the json file.
                AddToList();
            }

            else
            {
                // Else, the contacts will be showed in the ListView. 
                var item = (Contact)lv_Contacts.SelectedItem;
                item.FirstName = tb_FirstName.Text;
                item.LastName = tb_LastName.Text;
                item.Email = tb_Email.Text;
                _fileManager.Save(_filePath, JsonConvert.SerializeObject(contacts));
            }

            // All objects in the list "contacts" will be showed in the ListView.
            contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(_fileManager.Read(_filePath));
            lv_Contacts.ItemsSource = contacts;

            // After adding the contact, clear the input fields.
            ClearFields();
        }

        // By clicking "Clear Contact", clear the input fields.
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
           ClearFields();
        }

        // The detail of the selected contact will be showed in the input fields.
        // By clicking "Update", the updated contact will be saved.
        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ListView;
            var item = (Contact)obj!.SelectedItem;

            if (item != null)
            {
                tb_FirstName.Text = item.FirstName;
                tb_LastName.Text = item.LastName;
                tb_Email.Text = item.Email; 
            }
            // Changes the content of button "Add to Contact" to "Update".
            btn_Add.Content = "Update";
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(contacts));

            // All objects in the list "contacts" will be showed in the ListView.
            contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(_fileManager.Read(_filePath));
            lv_Contacts.ItemsSource = contacts;

        }

        // By clicking dustbox icon, remove the contact from the list "contacts".
        // Save the change in the json file.
        private void btn_lvItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = sender as Button;
                var item = (Contact)obj!.DataContext;

                // Remove the selected contact and save the change in the list "contacts".
                contacts.Remove(item);

                // After saving the change, clear the input fields.
                ClearFields();
            }
            catch { }
        }

        // Method to clear the input fields.
        private void ClearFields()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Email.Text = "";
            lv_Contacts.SelectedItems.Clear();

            // After clearing the input fields, change the content of button "Update" to "Add to Contact".
            btn_Add.Content = "Add to Contact";
        }

        // Method to add new contact to the list "contacts".
        private void AddToList()
        {
            // If the Email address doesn't exist in the list, add a new contact.
            var contact = contacts.FirstOrDefault(x => x.Email == tb_Email.Text);
            if (contact == null)
            {
                contacts.Add(new Contact
                {
                    FirstName = tb_FirstName.Text,
                    LastName = tb_LastName.Text,
                    Email = tb_Email.Text
                });
                _fileManager.Save(_filePath, JsonConvert.SerializeObject(contacts));
            }
            else
            {
                // Else, show the pop-up message.
                MessageBox.Show("Existing contact : Please check again.");
            }

        }

    }
}
