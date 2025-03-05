using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using WebAppCRUD.Models;

namespace WebAppCRUD.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=ICT-029\INSTANCE2022;Initial Catalog=MvcCrudDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        [HttpGet]
        // GET: ProductController
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            using(SqlConnection sqlcon=new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("select * from Product", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }



        // GET: ProductController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel productModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Product values(@ProductName,@Price,@Count)";
                SqlCommand sqlcmd=new SqlCommand(query, sqlcon);
                sqlcmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlcmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlcmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "select * from Product where ProductID=@ProductID";
                SqlDataAdapter sqlDa=new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID",id);
                sqlDa.Fill(dtblProduct);

            }
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.ProductId=Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToDecimal(dtblProduct.Rows[0][2].ToString());
                productModel.Count = Convert.ToInt32(dtblProduct.Rows[0][3].ToString());
                return View(productModel);

            }
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "update Product set ProductName=@ProductName,Price=@Price,Count=@Count where ProductID=@ProductID";
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.Parameters.AddWithValue("@ProductID", productModel.ProductId);
                sqlcmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlcmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlcmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlcmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "DELETE from Product where ProductID=@ProductID";
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.Parameters.AddWithValue("@ProductID",id);
                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
