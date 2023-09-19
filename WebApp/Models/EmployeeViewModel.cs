namespace WebApp.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public static class DateTimeExtension
    {
        public static string ConvertDateTime(this DateTime dateTime)
        {
            try
            {
                return dateTime == new DateTime() ? string.Empty : dateTime.ToString("yyyy-MM-dd");
            }
            catch
            {
                return string.Empty;
            }
        }
    }

}