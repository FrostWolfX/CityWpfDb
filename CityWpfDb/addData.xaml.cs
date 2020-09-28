using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for addData.xaml
    /// </summary>
    public partial class addData : Window
    {
        public addData()
        {
            InitializeComponent();
        }
        public addData(string nameTable)
        {
            InitializeComponent();
            сhoiceTab(nameTable);
        }
        public addData(string nameTable, string nameCountry)
        {
            InitializeComponent();
            сhoiceTab(nameTable);

            addNameCountry.Items.Add(nameCountry);
            addNameCountry.SelectedIndex = addNameCountry.Items.Count - 1;
        }

        void сhoiceTab(string nameTable)
        {
            //выбираю нужное добавление на новой форме, деактивируя другие вкладки
            switch (nameTable)
            {
                case "Country":
                    CountryTab.IsSelected = true;
                    RegionTab.IsEnabled = false;
                    CityTab.IsEnabled = false;
                    break;
                case "Region":
                    RegionTab.IsSelected = true;
                    CountryTab.IsEnabled = false;
                    CityTab.IsEnabled = false;
                    break;
                case "City":
                    CityTab.IsSelected = true;
                    RegionTab.IsEnabled = false;
                    CountryTab.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void addContent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addCountryText.Text != "")
                {
                    string strAddCountry = $"INSERT INTO Country (nameCountry) VALUES ('{addCountryText.Text}')";
                    string strSearchCountry = $"SELECT nameCountry FROM Country WHERE nameCountry ='{addCountryText.Text}'";
                    if (QuerySQLDB.QueryDB(strSearchCountry).Rows.Count == 0)//проверка существует ли запись страны в таблице
                    {
                        QuerySQLDB.QueryDB(strAddCountry);//записываем страну в таблицу
                        resultAddCountry.Foreground = Brushes.Green;
                        resultAddCountry.Content = "Запись успешно добавлена";
                    }
                    else
                    {
                        resultAddCountry.Foreground = Brushes.Red;
                        resultAddCountry.Content = "Запись уже существует";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addContentRegion_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(addRegionId.Text, out int correctNumberId))
            {
                string sqlExistCountryAndRegion = $"SELECT * FROM Region WHERE nameCountry = '{addNameCountry.Text}' AND " +
                $"nameRegion = '{addRegionName.Text}'";
                string sqlExistId = $"SELECT * FROM Region WHERE idRegion = {addRegionId.Text}";
                string sqlExistCounrty = $"SELECT * FROM Country WHERE nameCountry = '{addNameCountry.Text}'";

                string sqlAddCountry = $"INSERT INTO Country (nameCountry) VALUES ('{addNameCountry.Text}')";
                string sqlAddRegion = $"INSERT INTO Region (nameCountry, idRegion, nameRegion) " +
                    $"VALUES ('{addNameCountry.Text}', {addRegionId.Text}, '{addRegionName.Text}')";

                if (QuerySQLDB.QueryDB(sqlExistCountryAndRegion).Rows.Count == 0)//проверка существования страны с регионом
                {
                    if (QuerySQLDB.QueryDB(sqlExistId).Rows.Count == 0)//проверка свободен ли id
                    {
                        if (QuerySQLDB.QueryDB(sqlExistCounrty).Rows.Count == 0)//проверка существования страны в таблице Страны, 
                                                                                //т.к. БД реляционная и это критично
                        {
                            QuerySQLDB.QueryDB(sqlAddCountry);//добавляем страну в таблицу Country
                            QuerySQLDB.QueryDB(sqlAddRegion);//добавляю запись в таблицу Region

                            resultAddRegion.Foreground = Brushes.Green;
                            resultAddRegion.Content = "Запись успешно добавлена";
                        }
                        else
                        {
                            QuerySQLDB.QueryDB(sqlAddRegion);//добавляю запись в таблицу Region
                            resultAddRegion.Foreground = Brushes.Green;
                            resultAddRegion.Content = "Запись успешно добавлена";
                        }
                    }
                    else
                    {
                        resultAddRegion.Foreground = Brushes.Red;
                        resultAddRegion.Content = "Id уже сущестует, введите новый Id";
                    }
                }
                else
                {
                    resultAddRegion.Foreground = Brushes.Red;
                    resultAddRegion.Content = "Запись уже существует";
                }
            }
            else
            {
                resultAddRegion.Foreground = Brushes.Red;
                resultAddRegion.Content = "Некорректный индекс региона";
            }
        }

        private void addContentCity_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
