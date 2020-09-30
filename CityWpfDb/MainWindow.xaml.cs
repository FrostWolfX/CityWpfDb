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

            //загрузка городов в Combo_search
            string sqlShowCity = $"SELECT * FROM City";
            DataTable dataCity = QuerySQLDB.QueryDB(sqlShowCity);
            for (int i = 0; i < dataCity.Rows.Count; i++)
            {
                Combo_search.Items.Add(dataCity.Rows[i][1]);
            }
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
            switch (nameListForClear.ToLower())
            {
                case "country"://очищаем listbox региона и города
                    listRegion.SelectedIndex = -1;//снимаю выделение
                    listRegion.Items.Clear();
                    listCity.SelectedIndex = -1;//снимаю выделение
                    listCity.Items.Clear();
                    break;

                case "region"://очищаем listbox города
                    listCity.SelectedIndex = -1;
                    listCity.Items.Clear();
                    break;

                case "city":
                    listRegion.SelectedIndex = -1;//снимаю выделение
                    listRegion.Items.Clear();
                    listCity.SelectedIndex = -1;//снимаю выделение
                    listCity.Items.Clear();
                    listCountry.SelectedIndex = -1;//снимаю выделение
                    listCountry.Items.Clear();
                    break;

                default:
                    break;
            }
        }

        private void listCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)//вывод регионов
        {
            if (listCountry.SelectedIndex == -1)
            {
                //при очистке listbox переходит сюда, была решена проблема с выделеннием в listbox
            }
            else
            {
                string selectCountry = (listCountry.SelectedItem as Country).nameCountry;

                ClearListView("Country");

                Load1("region", "SELECT * FROM Region WHERE nameCountry = " + "'" + selectCountry.ToString() + "'");
            }            
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

        private void addRegion_Click(object sender, RoutedEventArgs e)//добавление региона
        {
            string nCountry, iRegion, nRegion;
            if (listCountry.SelectedIndex == -1)
            {
                nCountry = "";
            }
            else
            {
                nCountry = (listCountry.SelectedItem as Country).nameCountry;
            }
            if (listRegion.SelectedIndex == -1)
            {
                iRegion = "";
                nRegion = "";
            }
            else
            {
                iRegion = (listRegion.SelectedItem as Region).idRegion;
                nRegion = (listRegion.SelectedItem as Region).nameRegion;
            }

            addData addData = new addData(nCountry, iRegion, nRegion);
            addData.ShowDialog();
        }

        //кнопка очистки списка регионов и городов
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            listCountry.Items.Clear();
            listRegion.Items.Clear();
            listCity.Items.Clear();
            Load1("country", "SELECT * FROM Country");
        }

        private void Search_button_Click(object sender, RoutedEventArgs e)
        {
            string sqlExistCity = $"SELECT * FROM City WHERE nameCity = '{Combo_search.Text}'";
            ClearListView("city");
            

            DataTable dataCity = QuerySQLDB.QueryDB(sqlExistCity);
            for (int i = 0; i < dataCity.Rows.Count; i++)
            {
                City city = new City()
                {
                    idRegion = dataCity.Rows[i][0].ToString(),
                    nameCity = dataCity.Rows[i][1].ToString()
                };
                listCity.Items.Add(city);


                string sqlExitsRegion = $"SELECT * FROM Region WHERE idRegion = {dataCity.Rows[i][0]}";
                DataTable dataRegion = QuerySQLDB.QueryDB(sqlExitsRegion);
                for (int j = 0; j < dataRegion.Rows.Count; j++)
                {
                    Region region = new Region()
                    {
                        nameCountry = dataRegion.Rows[j][0].ToString(),
                        idRegion = dataRegion.Rows[j][1].ToString(),
                        nameRegion = dataRegion.Rows[j][2].ToString()
                    };
                    listRegion.Items.Add(region);
                    listCountry.Items.Add(new Country { nameCountry = dataRegion.Rows[j][0].ToString() });
                }
            }
        }

        private void updateButtonCity_Click(object sender, RoutedEventArgs e)
        {
            //изменение выбранного города
            if (listCity.Items.Count != 0 && listCity.SelectedIndex != -1)
            {
                string[] selectCity = {
                (listCity.SelectedItem as City).idRegion,
                (listCity.SelectedItem as City).nameCity
                };

                UpdateFotm updateFotm = new UpdateFotm(selectCity);
                updateFotm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбран город для редактирования");
            }
        }

        private void deleteButtonCity_Click(object sender, RoutedEventArgs e)
        {
            //удаление выбранного города
            if (listCity.Items.Count != 0 && listCity.SelectedIndex != -1)
            {
                string sqlDeleteCity = $"DELETE FROM City WHERE idRegion = {(listCity.SelectedItem as City).idRegion} " +
                    $"AND nameCity = '{(listCity.SelectedItem as City).nameCity}'";
                QuerySQLDB.QueryDB(sqlDeleteCity);
                MessageBox.Show("Запись успешно удалена!");
                listCity.Items.RemoveAt(listCity.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Не выбран город для удаления");
            }
        }
    }
}
