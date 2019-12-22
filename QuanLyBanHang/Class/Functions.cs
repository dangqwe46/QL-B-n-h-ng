﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang.Class
{
    class Functions
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            Con = new SqlConnection();   //Khởi tạo đối tượng
            Con.ConnectionString = @"Data Source=DANGPC;Initial Catalog=QuanLyBanHang;User ID=sa;Password=123;Integrated Security=True";
            Con.Open();                  //Mở kết nối
            //Kiểm tra kết nối
            if (Con.State == ConnectionState.Open)
                MessageBox.Show("Kết nối thành công");
            else MessageBox.Show("Không thể kết nối với dữ liệu");

        }
        public static void Disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();   	//Đóng kết nối
                Con.Dispose(); 	//Giải phóng tài nguyên
                Con = null;
            }
        }
        //Lấy dữ liệu vào bảng cách 1
        //public static DataTable GetDataToTable(string sql)
        //{
        //    SqlDataAdapter MyData = new SqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
        //    //Tạo đối tượng thuộc lớp SqlCommand
        //    MyData.SelectCommand = new SqlCommand();
        //    MyData.SelectCommand.Connection = Functions.Con; //Kết nối cơ sở dữ liệu
        //    MyData.SelectCommand.CommandText = sql; //Lệnh SQL
        //    //Khai báo đối tượng table thuộc lớp DataTable
        //    DataTable table = new DataTable();
        //    MyData.Fill(table);
        //    return table;

        //}
        //Lấy dữ liệu vào bảng cách 2 (gọn hơn, nên dùng)
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter MyData = new SqlDataAdapter(sql, Con); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            MyData.Fill(table); //Đổ kết quả từ câu lệnh sql vào table
            return table;
        }

        //Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter MyData = new SqlDataAdapter(sql, Con);
            DataTable table = new DataTable();
            MyData.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        //Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }
        //Phương thức RunSQLDel tương tự như RunSQL nhưng 
        // trong trường hợp xóa dữ liệu nếu dữ liệu đang được dùng bởi một đối tượng khác 
        // thì không được phép xóa.
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Functions.Con;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Dữ liệu đang được dùng, không thể xoá...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }
        //IsDate có tác dụng kiểm tra một biến có ở dạng ngày tháng không
        public static bool IsDate(string date)
        {
            string[] elements = date.Split('/');
            if ((Convert.ToInt32(elements[0]) >= 1) && (Convert.ToInt32(elements[0]) <= 31) && (Convert.ToInt32(elements[1]) >= 1) && (Convert.ToInt32(elements[1]) <= 12) && (Convert.ToInt32(elements[2]) >= 1900))
                return true;
            else return false;
        }
        //ConvertDateTime có tác dụng đổi một chuỗi ngày tháng do người dùng nhập có dạng dd/mm/yyyy thành chuỗi ngày tháng có dạng mm/dd/yyyy để lưu vào CSDL.
        public static string ConvertDateTime(string date)
        {
            string[] elements = date.Split('/');
            string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
            return dt;
        }


    }
}
