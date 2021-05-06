using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using WebApplication1.Models;
using zad2REST;

namespace WebApplication1.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {


        [HttpGet]
        public IEnumerable<Animal> GetAnimals()
        {
            List<Animal> animals = new List<Animal>();
            SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
            string sqlCommand = "SELECT [IdAnimal],[Name],[Description],[Category],[Area] FROM[s18807].[dbo].[Animal] order by name asc";
            using (SqlCommand command = new SqlCommand(sqlCommand, sql))
            {
                sql.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal a = new Animal();
                        a.IdAnimal = (int)reader.GetValue(0);
                        a.Name = reader.GetString(1);
                        a.Description = reader.GetString(2);
                        a.Category = reader.GetString(3);
                        a.Area = reader.GetString(4);
                        animals.Add(a);
                    }
                    return animals;
                }
            }
            return null;
        }



        [HttpGet("{orderBy}")]
        public IEnumerable<Animal> GetAnimals(string orderBy)
        {
            List<Animal> animals = new List<Animal>();
            SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
            string sqlCommand = "SELECT [IdAnimal],[Name],[Description],[Category],[Area] FROM[s18807].[dbo].[Animal] order by " + orderBy + " asc";
            using (SqlCommand command = new SqlCommand(sqlCommand, sql))
            {
                sql.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal a = new Animal();
                        a.IdAnimal = (int)reader.GetValue(0);
                        a.Name = reader.GetString(1);
                        a.Description = reader.GetString(2);
                        a.Category = reader.GetString(3);
                        a.Area = reader.GetString(4);
                        animals.Add(a);
                    }
                    return animals;
                }
            }
            return null;
        }

        [HttpPut("{id}")]
        public HttpResponseMessage PutAnimal(string id, [FromBody] Animal opt)
        {
            var resp = new HttpResponseMessage();
            if (opt?.Name == null || opt?.Category == null || opt?.Area == null)
            {
                resp.StatusCode = (HttpStatusCode.BadRequest);
                return resp;
            }
            string sqlCommand = "Update [s18807].[dbo].[Animal] SET [Name] = '" + opt.Name + "',[Description] = '" + opt.Description + "',[Category] = '" + opt.Category + "',[Area] = '" + opt.Area + "' WHERE IdAnimal="+id;
            SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
            using (SqlCommand command = new SqlCommand(sqlCommand, sql))
            {
                sql.Open();
                command.ExecuteNonQuery();
            }
            resp.StatusCode = (HttpStatusCode.Created);

            return resp;
        }

        [HttpPost]
        public HttpResponseMessage PostAnimal([FromBody] Animal opt)
        {
            var resp = new HttpResponseMessage();
            if (opt?.Name == null|| opt?.Category==null||opt?.Area==null)
            {
                resp.StatusCode = (HttpStatusCode.BadRequest);
                return resp;
            }

            string sqlCommand = "INSERT INTO[s18807].[dbo].[Animal]([Name],[Description],[Category],[Area]) VALUES('" + opt.Name + "','" + opt.Description + "','" + opt.Category + "','" + opt.Area + "')";
            SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
            using (SqlCommand command = new SqlCommand(sqlCommand, sql))
            {
                sql.Open();
                command.ExecuteNonQuery();
            }
            resp.StatusCode = (HttpStatusCode.Created);

            return resp;
        }


        [HttpDelete("{id}")]
        public HttpResponseMessage RemoveAnimal(string id)
        {
            var resp = new HttpResponseMessage();

            string sqlCommand = "DELETE FROM [s18807].[dbo].[Animal] WHERE IdAnimal=" + id;
            SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
            using (SqlCommand command = new SqlCommand(sqlCommand, sql))
            {
                sql.Open();
                command.ExecuteNonQuery();
            }
            resp.StatusCode = (HttpStatusCode.OK);

            return resp;
        }
    }
}