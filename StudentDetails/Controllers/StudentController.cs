using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentDetails.model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            // Why we need DTO , it is not advisible to link the direct class that linked with the database , so we need DTOs.
            // using link queue we are creating link for the DTO 
            var students = CollegeRepository.students.Select(s => new StudentDTO  //this link queue will help us to convert the students list into StudentDTO list
            {
                Id = s.Id,
                Name = s.Name,
                EmailId = s.EmailId,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
            });
            // ok - 200 - sucess , we have predefined status codes , so we are using it to report about the status of the execution
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetById")]
        [ProducesResponseType(200)]// Documenting the satus code : Ok
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(404)]// Documenting the satus code : NotFound
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest(); // BadRequest - 400 - client error

            // if clent is searching foe the student thats not in the data , then we shouuld use :

            var student = CollegeRepository.students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
                return NotFound($"student with id: {id} is not found"); // NotFound - 404 - client error

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                EmailId = student.EmailId,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
            };

            // ok - 200
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetByName")] // since there is no resolve type as string in route we
                                                    // use alpha bits to make sure it only allows the alpha bits both in upper and loswer case

        [ProducesResponseType(200)]// Documenting the satus code : Ok
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(404)]// Documenting the satus code : NotFound
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError
        public ActionResult<StudentDTO> GetStudentByname(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(); // badrequest - 400 - client error

            var student = CollegeRepository.students.Where(n => n.Name == name).FirstOrDefault();
            if (student == null)
                return NotFound($"The person named {name} is not found"); // NotFound - 404 - clinet error

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                EmailId = student.EmailId,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
            };

            // ok - 200
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("create")]

        [ProducesResponseType(201)]// Documenting the satus code : Sucess code for record creation
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model.AdmissionDate < DateTime.Now)
            {
                ModelState.AddModelError("Admission error", "Admission Date is invalid");
                return BadRequest(ModelState);
            }
            if (model == null)
                return BadRequest();
            int newId = CollegeRepository.students.LastOrDefault().Id + 1; // used to retrive the last student ID from the record and
                                                                           // increment it by 1 to give new id to new student
            Student student = new Student
            {
                Id = newId,
                Name = model.Name,

                EmailId = model.EmailId,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
            };
            CollegeRepository.students.Add(student);

            model.Id = student.Id; // after creation the new student ID will be updated to the model

            // success record creation - 201
            return CreatedAtRoute("GetById", new { id = model.Id }, model);// CreatedAtRout is used to return the 201 status code
                                                                           // and also use to prepare the GET url of the newly created student
                                                                           // using that route we specified and usiing the parameter it creates a link
                                                                           // to get the newly created record
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(200)]// Documenting the satus code : Ok
        [ProducesResponseType(204)]// Documenting the satus code : no content code for record creation
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(404)]// Documenting not found
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError

        public ActionResult<StudentDTO> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest();
            }
            var ExsistingStudent = CollegeRepository.students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (ExsistingStudent == null)
            {
                return NotFound();
            }

            ExsistingStudent.Name = model.Name;
            ExsistingStudent.EmailId = model.EmailId;
            ExsistingStudent.Address = model.Address;

            return Ok();
        }

        [HttpPatch]
        [Route("{id:int}/patch")]
        [ProducesResponseType(200)]// Documenting the satus code : Ok
        [ProducesResponseType(204)]// Documenting the satus code : no content code for record creation
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(404)]// Documenting not found
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError

        public ActionResult UpadateStudentResult(int id , [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();


            var ExsistingStudent = CollegeRepository.students.Where(s => s.Id == id).FirstOrDefault();

            if (ExsistingStudent == null)
            {
                return NotFound();
            }

            var StudentDTO = new StudentDTO
            {
                Id = ExsistingStudent.Id,
                Name = ExsistingStudent.Name,
                EmailId = ExsistingStudent.EmailId,
                Address = ExsistingStudent.Address,
                PhoneNumber = ExsistingStudent.PhoneNumber,
            };

            patchDocument.ApplyTo(StudentDTO,ModelState); // if there is no error it will be mapped to DTO
                                                          // if there is any error then it will bw mapped to ModelState

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExsistingStudent.Name = StudentDTO.Name;
            ExsistingStudent.EmailId = StudentDTO.EmailId;
            ExsistingStudent.Address = StudentDTO.Address;

            return NoContent();

        }

        [HttpDelete]
        [Route("{id:int}",Name = "DeleteById")]

        [ProducesResponseType(200)]// Documenting the satus code : Ok
        [ProducesResponseType(400)]// Documenting the satus code : BadRequesst
        [ProducesResponseType(404)]// Documenting the satus code : NotFound
        [ProducesResponseType(500)]// Documenting the satus code : InternalServerError
        public ActionResult<bool> DeleteStudentById(int id)
        {
            if (id <= 0)
                return BadRequest(); // BadRequest - 400 - client error

            // if clent is searching foe the student thats not in the data , then we shouuld use :

            var student = CollegeRepository.students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
                return NotFound($"student with id: {id} is not found"); // NotFound - 404 - client error

            CollegeRepository.students.Remove(student);

            // ok - 200
            return Ok(true); 

        }



    }
}
