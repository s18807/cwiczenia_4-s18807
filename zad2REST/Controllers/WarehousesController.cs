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
    [Route("api/warehouse")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        SqlConnection sql = SQLConnection.GetDBConnection("db-mssql16.pjwstk.edu.pl");
        [HttpPost]
        public HttpResponseMessage PostProduct([FromBody] AddedProduct opt)
        {
            Order order = new Order();
            Product product = new Product();
            Product_Warehouse product_Warehouse = new Product_Warehouse();
            Warehouse warehouse = new Warehouse();


            var resp = new HttpResponseMessage();
            if (opt?.IdProduct != null && opt?.IdWarehouse != null && opt?.Amount >0 && opt?.CreatedAt != null)
            {
                string SQLcommand = "SELECT [IdProduct],[Name],[Description],[Price] FROM[s18807].[dbo].[Product] where[IdProduct] = " + opt?.IdProduct;
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        resp.StatusCode = (HttpStatusCode.BadRequest);
                        return resp;
                    }
                    sql.Close();
                }
                SQLcommand = "SELECT [IdWarehouse],[Name],[Address] FROM[s18807].[dbo].[Warehouse] where[IdWarehouse] = " + opt?.IdWarehouse;
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        resp.StatusCode = (HttpStatusCode.BadRequest);
                        return resp;
                    }
                    else {
                        while (reader.Read()) {
                            warehouse.IdWarehouse = reader.GetInt32(0);
                            warehouse.Name = reader.GetString(1);
                            warehouse.Address = reader.GetString(2);
                        }
                    }
                    sql.Close();
                }
                SQLcommand = "SELECT [IdOrder],[IdProduct],[Amount],[CreatedAt],[FulfilledAt] FROM [s18807].[dbo].[Order] where [IdProduct] = " + opt?.IdProduct + " and [Amount]=" + opt?.Amount;
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if ((DateTime)reader.GetValue(3) < opt?.CreatedAt)
                        {
                            resp.StatusCode = (HttpStatusCode.BadRequest);
                            return resp;
                        }
                        else {
                                order.IdOrder = (int)reader.GetValue(0);
                                order.IdProduct = (int)reader.GetValue(1);
                                order.Amount = (int)reader.GetValue(2);
                                order.CreatedAt = (DateTime)reader.GetValue(3);
                        }
                    }
                    sql.Close();
                }

                SQLcommand = "SELECT [IdProductWarehouse],[IdWarehouse],[IdProduct],[IdOrder],[Amount],[Price],[CreatedAt] FROM [s18807].[dbo].[Product_Warehouse] where [IdOrder]="+order.IdOrder;
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.HasRows)
                    {
                            resp.StatusCode = (HttpStatusCode.BadRequest);
                            return resp;
                    }
                    sql.Close();
                }
                SQLcommand = "UPDATE [s18807].[dbo].[Order] SET [FulfilledAt] = '" + DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + "' WHERE [IdProduct]= "+order.IdProduct+ " AND [Amount]="+order.Amount+ " AND [CreatedAt]='"+order.CreatedAt.ToString("yyyy-MM-dd'T'HH:mm:ss")+"'";
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    sql.Close();
                }

                int id = 0;

                SQLcommand = "INSERT INTO[s18807].[dbo].[Product_Warehouse]([IdWarehouse],[IdProduct],[IdOrder],[Amount],[Price],[CreatedAt]) OUTPUT Inserted.IdProductWarehouse VALUES(" + warehouse.IdWarehouse+","+order.IdProduct+","+order.IdOrder+","+order.Amount+","+order.Amount*product.Price+",'"+DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + "')";
                using (SqlCommand command = new SqlCommand(SQLcommand, sql))
                {
                    sql.Open();
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
            else {
            }
            return null;
        }

    }
}