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

namespace CityWpfDb
{
    /// <summary>
    /// Interaction logic for UpdateFotm.xaml
    /// </summary>
    public partial class UpdateFotm : Window
    {
        string[] lData;
        public UpdateFotm(string[] listData)
        {
            InitializeComponent();
            CityName.Text = listData[1];
            lData = listData;
        }

        private void Update_city_Click(object sender, RoutedEventArgs e)
        {
            string sqlUpdateCity = $"UPDATE City SET nameCity = '{CityName.Text}' WHERE idRegion = '{lData[0]}'";
            string sqlCheckCity = $"SELECT * FROM City WHERE nameCity = '{CityName.Text}' AND idRegion = '{lData[0]}'";
            if (QuerySQLDB.QueryDB(sqlCheckCity).Rows.Count == 0)
            {
                QuerySQLDB.QueryDB(sqlUpdateCity);
                resultUpCityText.Foreground = Brushes.Green;
                resultUpCityText.Content = "Запись успешно изменена!";
            }
            else
            {
                resultUpCityText.Foreground = Brushes.Red;
                resultUpCityText.Content = "Запись уже существует!";
            }
            
        }
    }
}
