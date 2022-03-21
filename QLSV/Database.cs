using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public class Database
    {
        private string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog = QLSV; User ID = trung; Password = 123456789";
        private SqlConnection conn;
        private DataTable dt;
        private SqlCommand cmd;
        public Database()
        {
            try
            {
                conn = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connected failed: "+ex.Message);
            }
        }

        public DataTable SelectData(string sql, List<CustomParameter> lstPara)
        {
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    //kiểm tra kết nối tới csdl có bị đóng hay không
                    //nếu đang bị đóng thì mở lại
                    conn.Open();
                }
               
                cmd = new SqlCommand(sql, conn);//nội dung sql được truyền vào
                cmd.CommandType = CommandType.StoredProcedure;//set command type cho cmd
                foreach (var para in lstPara)//gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(para.key, para.value);
                }
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                return null;
            }
            finally
            {
                //kiểm tra trạng thái kết nối tới csdl
                //nếu đang mở thì tiến hành đóng kết nối
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Close();
                }
                
            }
        }

        public DataRow Select(string sql)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    //kiểm tra kết nối tới csdl có bị đóng hay không
                    //nếu đang bị đóng thì mở lại
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);//truyền giá trị vào cmd
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());//thực thi câu lệnh
                return dt.Rows[0];//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thông tin chi tiết: " + ex.Message);
                return null;
            }
            finally
            {
                //kiểm tra trạng thái kết nối tới csdl
                //nếu đang mở thì tiến hành đóng kết nối
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public int ExeCute(string sql,List<CustomParameter> lstPara)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    //kiểm tra kết nối tới csdl có bị đóng hay không
                    //nếu đang bị đóng thì mở lại
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);//thực thi câu lệnh sql
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var p in lstPara)// gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(p.key, p.value);
                }
                var rs = cmd.ExecuteNonQuery();//lấy kết quả thực thi truy vấn
                return (int)rs;//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi câu lệnh: " + ex.Message);
                return -100;
            }
            finally
            {
                //kiểm tra trạng thái kết nối tới csdl
                //nếu đang mở thì tiến hành đóng kết nối
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }
    }
}
