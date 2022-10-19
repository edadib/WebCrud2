using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebCrud2.Models;

namespace WebCrud2.Controllers
{
    public class RoleController : Controller
    {
        private readonly IConfiguration config;

        public RoleController(IConfiguration configuration)
        {
            this.config = configuration;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(log_in));
        }

        public IActionResult list_user()
        {
            List<RoleModel> result = new List<RoleModel>();

            using (SqlConnection con = new SqlConnection(config.GetConnectionString("UserDB")))
            {
                string sql = "SELECT * FROM RoleUser";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        RoleModel user = new RoleModel();

                        user.id = dr.GetInt32(0);
                        user.email = dr.GetString(1);
                        user.password = dr.GetString(2);
                        user.create_date = dr.GetDateTime(3);
                        user.update_date = dr.IsDBNull(4) ? null : dr.GetDateTime(4);

                        result.Add(user);

                    }
                }
            }

            ViewData["user"] = result;

            return View();
        }

        public IActionResult create_user()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult add_user()
        {
            using (SqlConnection con = new SqlConnection(config.GetConnectionString("UserDB")))
            {
                string sql = "INSERT INTO RoleUser (email,password,create_date,update_date)  VALUES (@email,@password,@create_date,@update_date)";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 255).Value = Request.Form["email"].ToString();
                cmd.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 255).Value = Request.Form["password"].ToString();
                cmd.Parameters.Add("@create_date", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@update_date", System.Data.SqlDbType.DateTime).Value = DateTime.Now;

                con.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(list_user));
        }

        public IActionResult log_in()
        {
            return View();
        }

        public IActionResult login_user()
        {

            String user_email = Request.Form["email"].ToString();
            String user_password = Request.Form["password"].ToString();

            bool status_wujud = false;

            using (SqlConnection con = new SqlConnection(config.GetConnectionString("UserDB")))
            {
                string sql = "SELECT * FROM RoleUser";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        RoleModel user = new RoleModel();

                        user.id = dr.GetInt32(0);
                        user.email = dr.GetString(1);
                        user.password = dr.GetString(2);

                        if(user_email == user.email && user_password == user.password)
                        {
                            status_wujud = true;
                            break;
                        }
                    }
                }

                if(status_wujud)
                {
                    return RedirectToAction(nameof(list_user));
                }
                else
                {
                    String false_user = "display:block;";
                    return RedirectToAction(nameof(log_in));
                }

            }


            
        }
    }
}
