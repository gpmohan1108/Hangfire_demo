using Hangfire.Model;
using System.Data;
using System.Data.SqlClient;

namespace Hangfire
{
    public class Employeemethods : IEmployeeMethods
    {
        private readonly IConfiguration _configuration;
        public Employeemethods(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void AddEmployee(Employee employee)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            var query = "Insert into Employee(Id,Name,ContactNo) values(@Id,@Name,@ContactNo)";
            SqlConnection cn = new SqlConnection(connection);
            cn.Open();
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("Id", employee.Id);
                cmd.Parameters.AddWithValue("Name", employee.Name);
                cmd.Parameters.AddWithValue("ContactNo", employee.ContactNo);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

        }

        public List<Employee> GetEmployeeList()
        {
            DataTable employees = SyncData();
            List<Employee> emplist = (from DataRow dr in employees.Rows
                                      select new Employee()
                                      {
                                          Id = Convert.ToInt32(dr["Id"]),
                                          Name = dr["Name"].ToString(),
                                          ContactNo = dr["ContactNo"].ToString()
                                      }).ToList();
            return emplist;
        }

        public void SendEmail()
        {
            Console.WriteLine($"SendEmail :Sending email is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            var query = "Select * from Employee";
            SqlConnection cn = new SqlConnection(connection);
            cn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, cn);
            adapter.Fill(ds);
            cn.Close();

            Console.WriteLine($"SyncData :sync is going on..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return ds.Tables[0];
        }
    }
}
