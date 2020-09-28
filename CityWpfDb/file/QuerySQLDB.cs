using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace CityWpfDb
{
    class QuerySQLDB
    {
        public static DataTable QueryDB(string selectSQL) //функция подключения к БД и обрабока запросов
        {
            SqlConnectionStringBuilder connectStringBuild = new SqlConnectionStringBuilder();
            connectStringBuild.DataSource = @"localhost";
            connectStringBuild.InitialCatalog = "testDB_01";
            connectStringBuild.UserID = "userVlad";
            connectStringBuild.Password = "111";
            connectStringBuild.Pooling = true;

            DataTable dataTable = new DataTable("Country");//создаю таблицу в программе и подключаюсь к БД

            using (SqlConnection sqlConnection = new SqlConnection(connectStringBuild.ConnectionString))//подключаюсь к БД
            {
                try
                {
                    sqlConnection.Open();//открываем БД
                    SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection);//создаем команду
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);//создаем обработчик
                    sqlDataAdapter.Fill(dataTable);//возвращаем таблицу с результатом
                    return dataTable;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return dataTable;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
