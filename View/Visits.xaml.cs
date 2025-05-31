using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PolMedUMG.View
{
    public class Visit
    {
        public DateTime Date { get; set; }
        public string Doctor { get; set; }       // Zawiera specialistID jako tekst
        public string Description { get; set; }  // Zawiera serviceName
        public string TestType { get; set; }  // Zawiera additionalInfo

        public string FormattedDate => Date.ToString("dd.MM.yyyy");
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

            allVisits = new List<Visit>();

            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT dateOfVisit, serviceName, details, specialistID FROM Visits;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allVisits.Add(new Visit
                            {
                                Date = Convert.ToDateTime(reader["dateOfVisit"]),
                                Doctor = reader["specialistID"].ToString(), // Zawiera specialistID jako tekst
                                TestType = reader["serviceName"].ToString(),
                                Description = reader["details"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd bazy danych: " + ex.Message);
                }
            }

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
                    selectedVisit.Description
                );
                detailsWindow.ShowDialog();

                VisitsListBox.SelectedItem = null;
            }
        }
    }
}
