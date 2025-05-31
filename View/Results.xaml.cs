using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PolMedUMG.View
{
    public class Result
    {
        public DateTime Date { get; set; }
        public string TestType { get; set; } // np. Morfologia krwi
        public string Status { get; set; }   // np. odczytane / nieodczytane
        public string Description { get; set; } // np. szczegóły badania

        public bool IsRead => Status == "odczytane";

        public string FormattedDate => Date.ToString("dd.MM.yyyy");
    }

    public partial class Results : UserControl
    {
        private List<Result> allResults;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalPages => (int)Math.Ceiling((double)allResults.Count / pageSize);

        public Results()
        {
            InitializeComponent();

            allResults = new List<Result>();

            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT dateOfVisit, serviceName, additionalInfo, status FROM Visits;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allResults.Add(new Result
                            {
                                Date = Convert.ToDateTime(reader["dateOfVisit"]),
                                TestType = reader["serviceName"].ToString(),
                                Description = reader["additionalInfo"].ToString(),
                                Status = reader["status"].ToString()
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
            var pageResults = allResults
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ResultsListBox.ItemsSource = null;
            ResultsListBox.Items.Clear();
            ResultsListBox.ItemsSource = pageResults;

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

        private void ResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsListBox.SelectedItem is Result selectedResult)
            {
                if (selectedResult.Status == "nieodczytane")
                {
                    using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
                    {
                        try
                        {
                            conn.Open();
                            string sql = "UPDATE Visits SET status = 'odczytane' WHERE dateOfVisit = @date AND serviceName = @type";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@date", selectedResult.Date.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@type", selectedResult.TestType);
                                cmd.ExecuteNonQuery();
                            }

                            selectedResult.Status = "odczytane";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Błąd przy aktualizacji statusu: " + ex.Message);
                        }
                    }
                }

                var detailsWindow = new ResultsWindow(selectedResult);
                detailsWindow.ShowDialog();

                ResultsListBox.SelectedItem = null;
                LoadCurrentPage();
            }
        }
    }
}
