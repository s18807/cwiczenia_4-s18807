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
    [Route("api/warehouse2")]
    [ApiController]
    public class WarehousesController2 : ControllerBase
    {
        SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
        [HttpPost]
        public HttpResponseMessage PostProduct([FromBody] AddedProduct opt)
        {
            int id = 0;
            var resp = new HttpResponseMessage();
            string SQLcommand = "exec [s18807].[dbo].[AddProductToWarehouse] @IdProduct=" + opt?.IdProduct + ", @IdWarehouse=" + opt?.IdWarehouse + ", @Amount=" + opt?.Amount + ", @CreatedAt='" + opt?.CreatedAt.ToString("yyyy-MM-dd'T'HH:mm:ss") + "'";
            using (SqlCommand command = new SqlCommand(SQLcommand, sql))
            {
                sql.Open();
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader.GetValue(0);
                }
                sql.Close();
            }
            resp.StatusCode = (HttpStatusCode.Created);
            resp.Content = new StringContent(id.ToString());
            return resp;
        }


    }

}