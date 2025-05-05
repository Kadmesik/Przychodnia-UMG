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
    /// Logika interakcji dla klasy Visits.xaml
    /// </summary>
    public class Visit
    {
        public string Doctor { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }


    public partial class Visits : UserControl
    {
        private List<Visit> allVisits;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalPages => (int)Math.Ceiling((double)allVisits.Count / pageSize);

        public Visits()
        {
            InitializeComponent();

            allVisits = new List<Visit>
        {
            new Visit { Date = "2024-04-01", Doctor = "dr Anna Kowalska", Description = "Wizyta kontrolna" },
            new Visit { Date = "2024-03-15", Doctor = "dr Jan Nowak", Description = "Badania okresowe" },
            new Visit { Date = "2024-02-10", Doctor = "dr Maria Nowicka", Description = "Konsultacja specjalistyczna" },
            new Visit { Date = "2024-01-22", Doctor = "dr Tomasz Wiśniewski", Description = "Recepta" },
            new Visit { Date = "2023-12-30", Doctor = "dr Ewa Zielińska", Description = "Szczepienie" },
            new Visit { Date = "2024-04-01", Doctor = "dr Anna Kowalska", Description = "Wizyta kontrolna" },
            new Visit { Date = "2024-03-15", Doctor = "dr Jan Nowak", Description = "Badania okresowe" },
            new Visit { Date = "2024-02-10", Doctor = "dr Maria Nowicka", Description = "Konsultacja specjalistyczna" },
            new Visit { Date = "2024-01-22", Doctor = "dr Tomasz Wiśniewski", Description = "Recepta" },
            new Visit { Date = "2023-12-30", Doctor = "dr Ewa Zielińska", Description = "Szczepienie" },
            new Visit { Date = "2024-01-22", Doctor = "dr Tomasz Wiśniewski", Description = "Recepta" },
            new Visit { Date = "2024-01-22", Doctor = "dr Tomasz Wiśniewski", Description = "Recepta" },


        };

            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            var pageVisits = allVisits
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            VisitsListBox.ItemsSource = null;
            VisitsListBox.Items.Clear();
            VisitsListBox.ItemsSource = pageVisits;

            PageCounterText.Text = $"{currentPage}/{totalPages}";
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadCurrentPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadCurrentPage();
            }
        }

        private void VisitsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisitsListBox.SelectedItem is Visit selectedVisit)
            {
                var detailsWindow = new VisitDetailsWindow(
                    selectedVisit.Date,
                    selectedVisit.Doctor,
                    selectedVisit // Przekaż obiekt Visit
                );
                detailsWindow.ShowDialog();

                VisitsListBox.SelectedItem = null;
            }
        }




    }




}

