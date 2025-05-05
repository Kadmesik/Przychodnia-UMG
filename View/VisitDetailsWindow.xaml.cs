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

namespace PolMedUMG.View
{
    /// <summary>
    /// Logika interakcji dla klasy VisitDetailsWindow.xaml
    /// </summary>
    public partial class VisitDetailsWindow : Window
    {
        private string date;
        private string doctor;
        private string description;

        public VisitDetailsWindow(string date, string doctor, Visit visit)
        {
            InitializeComponent();

            VisitDateText.Text = $"Data: {visit.Date}";
            DoctorText.Text = $"Lekarz: {visit.Doctor}";
            DescriptionText.Text = $"Opis: {visit.Description}";
        }


        public VisitDetailsWindow(string date, string doctor, string description)
        {
            this.date = date;
            this.doctor = doctor;
            this.description = description;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}