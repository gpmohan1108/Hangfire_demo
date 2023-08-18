using Hangfire.Model;
using System.Data;

namespace Hangfire
{
    public interface IEmployeeMethods
    {
        void AddEmployee(Employee employee);

        List<Employee> GetEmployeeList();

        DataTable SyncData();

        void SendEmail();

    }
}
