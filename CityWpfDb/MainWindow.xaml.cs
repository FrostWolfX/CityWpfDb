using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace CityWpfDb
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load1("country", "SELECT * FROM Country");// загрузки стран в listbox, при старте
        }

        public class Country
        {
            public string nameCountry { get; set; }
        }

        public class Region
        {
            public string nameCountry { get; set; }
            public string idRegion { get; set; }
            public string nameRegion { get; set; }
        }

        public class City
        {
            public string idRegion { get; set; }
            public string nameCity { get; set; }
        }

        public void Load1(string viewSelection, string commandText)//функция загрузки данных из таблиц в listbox
        {
            DataTable dataTable = QuerySQLDB.QueryDB(commandText);//получаю данные из таблицы
            for (int i = 0; i < dataTable.Rows.Count; i++) // перебираем данные
            {
                switch (viewSelection)
                {
                    case "country":
                        Country dataCountry = new Country() //создаем экземпляр класса
                        {
                            nameCountry = dataTable.Rows[i][0].ToString()
                        };
                        listCountry.Items.Add(dataCountry);
                        break;

                    case "region":
                        Region dataRegion = new Region()
                        {
                            nameCountry = dataTable.Rows[i][0].ToString(),
                            idRegion = dataTable.Rows[i][1].ToString(),
                            nameRegion = dataTable.Rows[i][2].ToString()
                        };
                        listRegion.Items.Add(dataRegion);
                        break;

                    case "city":
                        City city = new City()
                        {
                            idRegion = dataTable.Rows[i][0].ToString(),
                            nameCity = dataTable.Rows[i][1].ToString()
                        };
                        listCity.Items.Add(city);
                        break;

                    default:
                        break;
                }
            }
        }

        void ClearListView(string nameListForClear)//функция для очистки lisbox при выборе различных стран или регионов
        {
            switch (nameListForClear)
            {
                case "Country"://очищаем listbox региона и города
                    listRegion.SelectedIndex = -1;//снимаю выделение
                    listRegion.Items.Clear();

                    listCity.SelectedIndex = -1;//снимаю выделение
                    listCity.Items.Clear();

                    break;
                case "Region"://очищаем listbox города
                    listCity.SelectedIndex = -1;
                    listCity.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        private void listCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)//вывод регионов
        {
            string selectCountry = (listCountry.SelectedItem as Country).nameCountry;

            ClearListView("Country");

            Load1("region", "SELECT * FROM Region WHERE nameCountry = " + "'" + selectCountry.ToString() + "'");
        }

        private void listRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)//вывод городов
        {
            if (listRegion.SelectedIndex == -1)
            {
                //при очистке listbox переходит сюда, была решена проблема с выделеннием в listbox
            }
            else
            {
                string selectRegion = (listRegion.SelectedItem as Region).idRegion;

                ClearListView("Region");

                Load1("city", "SELECT * FROM City WHERE idRegion = " + selectRegion);
            }
        }

        private void addCountry_Click(object sender, RoutedEventArgs e)//добавление страны
        {
            addData addData = new addData("Country");
            addData.ShowDialog();
            listCountry.Items.Clear();
            Load1("country", "SELECT * FROM Country");
        }

        private void addRegion_Click(object sender, RoutedEventArgs e)//добавление региона
        {
            if (listCountry.SelectedIndex == -1)
            {
                addData addData = new addData("Region");
                addData.ShowDialog();
            }
            else
            {
                addData addData = new addData("Region", (listCountry.SelectedItem as Country).nameCountry);
                addData.ShowDialog();
            }
        }

        private void addCity_Click(object sender, RoutedEventArgs e)//добавление города
        {
            addData addData = new addData("City");
            addData.ShowDialog();
        }
    }
}
