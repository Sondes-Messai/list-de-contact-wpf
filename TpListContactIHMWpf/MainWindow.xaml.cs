using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
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
using TpListContactBaseClass.Class;
using TpListContactBaseClass.DAO;
using TpListContactBaseClass.Tools;

namespace TpListContact
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Contact> contacts;
        ContactDAO daoContact = new ContactDAO();

        public MainWindow()
        {
            InitializeComponent();

            contacts_list.ItemsSource = daoContact.FindAll();
            
        }

        private void Ajouter_un_contact_Click(object sender, RoutedEventArgs e)
        {
            int id ,number = 0, zipcode = 0;
            string firstname = "", lastname = "", roadName = "", city = "", country = "", phone = "", email = "";
            DateTime dateOfBirth;

            id = Convert.ToInt32(contact_id.Text);
            firstname = contact_prenom.Text;
            lastname = contact_nom.Text;
            dateOfBirth = contact_naissance.SelectedDate.GetValueOrDefault();

            bool isValid = true;

            if (!MyRegex.IsName(firstname))
            {
                MessageBox.Show("Veuillez saisir un prenom de contact valide");
                return;
            }
            if (!MyRegex.IsName(lastname))
            {
                MessageBox.Show("Veuillez saisir un nom de contact valide");
                return;
            }

            number = Convert.ToInt32(contact_ad_n.Text);
            roadName = contact_ad_road.Text;
            zipcode = Convert.ToInt32(contact_ad_cp.Text);
            city = contact_ad_city.Text;
            country = contact_ad_country.Text;

            if (!MyRegex.IsAlphabetic(roadName))
            {
                MessageBox.Show("Veuillez saisir le nom de la rue");
                return;
            }
            if (!MyRegex.IsName(city))
            {
                MessageBox.Show("Veuillez saisir le nom de la Ville");
                return;
            }
            if (!MyRegex.IsName(country))
            {
                MessageBox.Show("Veuillez saisir le nom du pays");
                return;
            }
            
            email = contact_email.Text;
            phone = contact_tel.Text;
            if (!MyRegex.IsPhone(phone))
            {
                MessageBox.Show("Veuillez saisir le numéro de téléphone");
                return;
            }
            
            if (!MyRegex.IsEmail(email))
            {
                MessageBox.Show("Veuillez saisir l'email");
                return;
            }
            // Création du contact
            if (isValid)
            {

                if(id > 0)
                {
                    var contact = (Contact)contacts_list.SelectedItem;
                    contact.Firstname = firstname;
                    contact.Lastname = lastname;
                    contact.DateOfBirth = dateOfBirth;
                    contact.ContactAddress.Number = number;
                    contact.ContactAddress.RoadName = roadName;
                    contact.ContactAddress.ZipCode = zipcode;
                    contact.ContactAddress.City = city;
                    contact.ContactAddress.Country = country;

                    contact.Email = email;
                    contact.Phone = phone;
                    new ContactDAO().Update(contact);
                }
                else
                {

                    Address contactAdress = new Address(number, roadName, zipcode, city, country);

                    Contact c = new(firstname, lastname, dateOfBirth, contactAdress, phone, email);
                    c.ContactId = new ContactDAO().Create(c);
                    if (c.ContactId > 0)
                    {
                        MessageBox.Show($"\nContact ajouté avec l'id n°{c.ContactId}!\n");
                    }
                    else
                        MessageBox.Show("\nErreur lors de l'ajout du contact!\n");
                }
            }
            contact_prenom.Text = null;
            contact_nom.Text = null;
            contact_naissance.SelectedDate = null;

            contact_ad_n.Text = null;
            contact_ad_road.Text = null;
            contact_ad_cp.Text = null;
            contact_ad_city.Text = null;
            contact_ad_country.Text = null;

            contact_email.Text = null;
            contact_tel.Text = null;
            contacts_list.ItemsSource = daoContact.FindAll();
        }

        private void Modifier_un_contact_Click(object sender, RoutedEventArgs e)
        {
            var contact = (Contact)contacts_list.SelectedItem;

            if (contact != null)
            {
                contact_id.Text = contact.ContactId.ToString();
                contact_prenom.Text = contact.Firstname;
                contact_nom.Text = contact.Lastname;
                contact_naissance.SelectedDate = contact.DateOfBirth;

                contact_ad_n.Text = contact.ContactAddress.Number.ToString();
                contact_ad_road.Text = contact.ContactAddress.RoadName;
                contact_ad_cp.Text = contact.ContactAddress.ZipCode.ToString();
                contact_ad_city.Text = contact.ContactAddress.City.ToString();
                contact_ad_country.Text = contact.ContactAddress.Country.ToString();

                contact_email.Text = contact.Email;
                contact_tel.Text = contact.Phone;
            }
        }

        private void Supprimer_un_contact_Click(object sender, RoutedEventArgs e)
        {
            var contact = (Contact)contacts_list.SelectedItem;
            if (contact != null)
            {
                new ContactDAO().Delete(contact);
            }
        }


        private void contacts_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var contact = (Contact)contacts_list.SelectedItem;
            if (contact != null)
            {
                contact_id.Text = contact.ContactId.ToString();
            }
        }
    }
}
