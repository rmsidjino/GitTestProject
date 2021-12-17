using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MenuAuthMgt
{
    public class MenuDAC : IDisposable
    {
        SqlConnection conn;

        public MenuDAC()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["localDB"].ConnectionString);
        }

        public void Dispose()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        public DataTable GetMenuList()
        {
            string sql = @"select menu_id, menu_name, menu_level, pnt_menu_id, program_name, menu_Img, menu_sort
from Menu 
order by pnt_menu_id, menu_sort";

            DataTable dt = new DataTable();
            using(SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAuthList()
        {
            string sql = @"select auth_id, auth_name, auth_desc from Authority";

            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetMenuAuthList()
        {
            string sql = @"select menu_auth_id, menu_id, m.auth_id, auth_name 
from MenuAuth m inner join Authority a on m.auth_id = a.auth_id";

            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public bool SaveMenuAuth(int menu_id, List<int> authList)
        {
            string authStr = string.Join(",", authList);

            string sql = @"delete from MenuAuth where menu_id = @menu_id;
insert into MenuAuth (menu_id, auth_id) 
  select @menu_id, item from SplitString(@authstr, ',')";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@menu_id", menu_id);
                cmd.Parameters.AddWithValue("@authstr", authStr);

                cmd.Connection.Open();
                int iCnt = cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return (iCnt > 0);
            }
        }

        public DataTable GetUserMenuList(string useid)
        {
            string sql = @"select menu_id, menu_name, menu_level, pnt_menu_id, program_name, menu_Img, menu_sort
from Menu
where menu_id in (select menu_id
					from Userinfo U inner join MenuAuth A on U.auth_id = A.auth_id 
					where U.user_id = @useid)
union 
select distinct P.menu_id, P.menu_name, P.menu_level, P.pnt_menu_id, P.program_name, P.menu_Img, P.menu_sort
from Menu P inner join Menu C on P.menu_id = C.pnt_menu_id
where c.menu_id in (select menu_id
					from Userinfo U inner join MenuAuth A on U.auth_id = A.auth_id 
					where U.user_id = @useid)";

            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@useid", useid);

                da.Fill(dt);
            }
            return dt;
        }
    }
}
