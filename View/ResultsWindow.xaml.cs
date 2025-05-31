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
using MySqlX.XDevAPI.Common;

namespace PolMedUMG.View
{
    /// <summary>
    /// Logika interakcji dla klasy ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow(Result selectedResult)
        {
            InitializeComponent();


            DescriptionText.Text = selectedResult.Description.Replace("\\n", Environment.NewLine);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
//private string GetTestDetails(Result result)
//{
//    // Domyślne wartości jeśli czegoś brakuje
//    string sampleDate = result.SampleDate ?? "brak danych";
//    string resultDate = result.ResultDate ?? "brak danych";
//    string orderNumber = result.OrderNumber ?? "brak danych";

//    string header =
//        $"Data pobrania: {sampleDate}\n" +
//        $"Data wykonania: {resultDate}\n" +
//        $"Numer zlecenia: {orderNumber}\n";

//    switch (result.TestType)
//    {
//        case "Morfologia krwi":
//            return header +
//                   "Leukocyty (WBC): 6,2 x10^3/μl\n" +
//                   "Erytrocyty (RBC): 4,5 mln/μl\n" +
//                   "Hemoglobina (HGB): 13,5 g/dl\n" +
//                   "Hematokryt (HCT): 40,2%\n" +
//                   "Płytki krwi (PLT): 230 tys./μl";

//        case "Lipidogram":
//            return header +
//                   "Cholesterol całkowity: 195 mg/dl\n" +
//                   "LDL (zły cholesterol): 125 mg/dl\n" +
//                   "HDL (dobry cholesterol): 52 mg/dl\n" +
//                   "Triglicerydy: 135 mg/dl";

//        case "Mocz – badania ogólne":
//            return header +
//                   "Barwa: żółta\n" +
//                   "Przejrzystość: przejrzysty\n" +
//                   "Ciężar właściwy: 1,020\n" +
//                   "pH: 6,0\n" +
//                   "Białko: nieobecne\n" +
//                   "Glukoza: nieobecna\n" +
//                   "Ciała ketonowe: nieobecne\n" +
//                   "Leukocyty: pojedyncze";

//        case "Przeciwciała":
//            return header +
//                   "Przeciwciała IgG: obecne\n" +
//                   "Przeciwciała IgM: nieobecne\n" +
//                   "Interpretacja: wynik świadczy o przebytej infekcji i odporności nabytej";

//        default:
//            return "Brak szczegółów dla tego typu badania.";
//    }








