using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentDetails.Data.Configuration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);// setting ID as the primary key
            builder.Property(x => x.Id).UseIdentityColumn(); // this makes id as the auto generated column

            builder.Property(n => n.Name).IsRequired().HasMaxLength(250); //we are setting the feild as IsRequired and
                                                                         //setting is lengnth to 250
            builder.Property(n => n.EmailId).IsRequired().HasMaxLength(250); // since the IsRequired is false that means email
                                                                            // is not a compulsory feild
            builder.Property(n => n.PhoneNumber).IsRequired().HasMaxLength(100);

            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);

            builder.Property(n => n.DOB).IsRequired();

            builder.HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "abdul",
                    EmailId = "a@gmail.com",
                    Address = "chennai",
                    PhoneNumber = "1234567890",
                    DOB = new DateTime(2003,05,07)
                },
                new Student
                {
                    Id = 2,
                    Name = "faridh",
                    EmailId = "f@gmail.com",
                    Address = "banglore",
                    PhoneNumber = "0123456789",
                    DOB = new DateTime(2002,06,19)
                }

            });

        }
    }
}
