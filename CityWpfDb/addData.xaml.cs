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
        public addData(string countryName, string regionId, string regionName)
        {
            InitializeComponent();

            if (countryName != "")
            {
                addCountryName.Items.Add(countryName);
                addCountryName.SelectedIndex = addCountryName.Items.Count - 1;
            }
            if (regionId != "")
            {
                addRegionId.Items.Add(regionId);
                addRegionId.SelectedIndex = addRegionId.Items.Count - 1;
            }
            if (regionName != "")
            {
                addRegionName.Text = regionName;
            }
        }

        private void addContentRegion_Click(object sender, RoutedEventArgs e)
        {
            if (addCityName.Text == "" && addRegionName.Text == "" && addRegionId.Text == "" && addCountryName.Text != "")
            {
                string strAddCountry = $"INSERT INTO Country (nameCountry) VALUES ('{addCountryName.Text}')";
                string strSearchCountry = $"SELECT nameCountry FROM Country WHERE nameCountry ='{addCountryName.Text}'";
                if (QuerySQLDB.QueryDB(strSearchCountry).Rows.Count == 0)//проверка существует ли запись страны в таблице
                {
                    QuerySQLDB.QueryDB(strAddCountry);//записываем страну в таблицу
                    resultAdd.Foreground = Brushes.Green;
                    resultAdd.Content = $"{addCountryName.Text} успешно добавлена";
                }
                else
                {
                    resultAdd.Foreground = Brushes.Red;
                    resultAdd.Content = $"{addCountryName.Text} уже существует";
                }
            }
            else if (addCityName.Text == "" && (addRegionName.Text != "" && addRegionId.Text != "") && addCountryName.Text != "")
            {
                insertRegion();
            }
            else if (addCityName.Text != "" && (addRegionName.Text != "" && addRegionId.Text != "") && addCountryName.Text != "")
            {
                string sqlExistCity = $"SELECT * FROM City WHERE idRegion = {addRegionId.Text} AND nameCity = '{addCityName.Text}'";
                string sqlAddCity = $"INSERT INTO City (idRegion, nameCity) VALUES ('{addRegionId.Text}', '{addCityName.Text}')";
                if (insertRegion())
                {
                    //проверяем существует ли город
                    if (QuerySQLDB.QueryDB(sqlExistCity).Rows.Count == 0)
                    {
                        //добавляем город
                        QuerySQLDB.QueryDB(sqlAddCity);
                        resultAdd.Foreground = Brushes.Green;
                        resultAdd.Content = "Город успешно добавлен";
                    }
                    else
                    {
                        resultAdd.Foreground = Brushes.Red;
                        resultAdd.Content = "Город уже существует";
                    }
                }
            }
            else
            {
                resultAdd.Foreground = Brushes.Red;
                resultAdd.Content = "Некоторые поля заполненны некоректно";
            }

            bool insertRegion()
            {
                if (Int32.TryParse(addRegionId.Text, out int correctNumberId2))
                {
                    string sqlExistCountryAndRegion = $"SELECT * FROM Region WHERE nameCountry = '{addCountryName.Text}' AND " +
                $"nameRegion = '{addRegionName.Text}'";
                    string sqlExistId = $"SELECT * FROM Region WHERE idRegion = {addRegionId.Text}";
                    string sqlExistCounrty = $"SELECT * FROM Country WHERE nameCountry = '{addCountryName.Text}'";

                    string sqlAddCountry = $"INSERT INTO Country (nameCountry) VALUES ('{addCountryName.Text}')";
                    string sqlAddRegion = $"INSERT INTO Region (nameCountry, idRegion, nameRegion) " +
                        $"VALUES ('{addCountryName.Text}', {addRegionId.Text}, '{addRegionName.Text}')";

                    if (QuerySQLDB.QueryDB(sqlExistCountryAndRegion).Rows.Count == 0)//проверка существования страны с вписанным регионом
                    {
                        if (QuerySQLDB.QueryDB(sqlExistId).Rows.Count == 0)//проверка свободен ли id
                        {
                            if (QuerySQLDB.QueryDB(sqlExistCounrty).Rows.Count == 0)//проверка существования страны в таблице Страны, 
                                                                                    //т.к. БД реляционная и это критично
                            {
                                QuerySQLDB.QueryDB(sqlAddCountry);//добавляем страну в таблицу Country
                                QuerySQLDB.QueryDB(sqlAddRegion);//добавляю запись в таблицу Region

                                resultAdd.Foreground = Brushes.Green;
                                resultAdd.Content = "Запись успешно добавлена";
                            }
                            else
                            {
                                QuerySQLDB.QueryDB(sqlAddRegion);//добавляю запись в таблицу Region
                                resultAdd.Foreground = Brushes.Green;
                                resultAdd.Content = "Запись успешно добавлена";
                            }
                        }
                        else
                        {
                            resultAdd.Foreground = Brushes.Red;
                            resultAdd.Content = "Id уже сущестует, введите новый Id";
                        }
                    }
                    else
                    {
                        resultAdd.Foreground = Brushes.Red;
                        resultAdd.Content = "Запись уже существует";
                    }
                    return true;
                }
                else
                {
                    resultAdd.Foreground = Brushes.Red;
                    resultAdd.Content = "Некорректный индекс региона";
                    return false;
                }
            }
        }
    }
}
