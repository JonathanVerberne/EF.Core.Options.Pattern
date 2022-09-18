namespace EF.Core.Options.Pattern.Entities
{
    public class Employee
    {
        public int EmployeeId { get; private set; }

        public string EmployeeName { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime Created { get; private set; } = DateTime.Now;
    }
}