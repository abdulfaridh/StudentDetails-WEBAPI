namespace StudentDetails.model
{
    public class CollegeRepository
    {
        public static List<Student> students { get; set; } = new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "Abdul",
                    EmailId = "abdul@gmail.com",
                    PhoneNumber = "1234567890",
                    Address = "chennai , INDIA"
                },
                new Student
                {
                    Id = 2,
                    Name = "Ajay",
                    EmailId = "ajay@gmail.com",
                    PhoneNumber="0987654321",
                }
            };
    }
}
