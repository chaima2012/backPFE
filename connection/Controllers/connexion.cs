using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace connection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class connexion : ControllerBase
    {
        public class Connexion
        {
            public string login { get; set; }
            public string password { get; set; }

        }

        public class users
        {
            public string name { get; set; }
            public string email { get; set; }
            public string StatusUserId { get; set; }
            public string role { get; set; }
            public Boolean retour { get; set; }


        }


        [Route("api/connexion"), HttpPost]
        public users getconnexion(Connexion cnx)
        {
            Boolean result = false;

            users usr= new users();
            string login = cnx.login;
            string password = cnx.password;




            string constr = @"Data Source =DESKTOP-VGL0E1R\SQLEXPRESS; Initial Catalog = gestionachats; Trusted_Connection=True; ";
            SqlConnection sqlConnection = new SqlConnection(constr);
            try
            {


                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd.CommandText = @"select * from users where email='" + login + "' and password='" + password + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    usr.retour= true;
                    usr.name=reader["Name"].ToString();
                    usr.email = reader["Email"].ToString();
                    usr.StatusUserId = reader["StatusUserId"].ToString();
                    usr.role = reader["role"].ToString();

                }
                if(usr.StatusUserId=="1" || usr.StatusUserId=="3"){
                    usr.retour=false;
                }
                else if (usr.StatusUserId=="2"){
                    usr.retour=true;
                }
                return usr;

            }
            catch (Exception ex)
            {
                
                usr.retour = false;
                return usr;
            }






        }
    }
}
