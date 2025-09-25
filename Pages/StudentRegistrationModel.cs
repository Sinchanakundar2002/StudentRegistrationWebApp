using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstWebApp1.Models;
using MyFirstWebApp1.Helpers;

namespace MyFirstWebApp1.Pages
{
    public class StudentRegistrationModel : PageModel
    {
        [BindProperty]
        public Student Student { get; set; } = new Student();

        public List<Student> StudentList { get; set; } = new();

        private readonly IWebHostEnvironment _env;
        private string FilePath => Path.Combine(_env.WebRootPath, "data", "students.json");

        public StudentRegistrationModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet(string? id)
        {

            StudentList = StudentDataHelper.LoadStudents(FilePath)
                  .Where(s => s.Status == "Active")
                  .ToList();
            // StudentList = StudentDataHelper.LoadStudents(FilePath);

            if (!string.IsNullOrEmpty(id))
            {
                Student = StudentList.FirstOrDefault(s => s.Id == id) ?? new Student();
                if (Student == null)
                {
                    ViewData["SuccessMessage"] = "Student not found.";
                    Student = new Student();
                }
            }
            else
            {
                Student = new Student()
                {
                    DateOfBirth = DateTime.Today

                };
            }
        }

        public IActionResult OnPost()
        {
            StudentList = StudentDataHelper.LoadStudents(FilePath);

            if (!ModelState.IsValid)
                return Page();

            //  Ensure empty or whitespace ID doesn't match existing
            var existingStudent = !string.IsNullOrWhiteSpace(Student.Id)
                ? StudentList.FirstOrDefault(s => s.Id == Student.Id)
                : null;

            if (existingStudent != null)
            {
                // Update existing student
                existingStudent.StudentName = Student.StudentName;
                existingStudent.SchoolName = Student.SchoolName;
                existingStudent.ClassName = Student.ClassName;
                existingStudent.DateOfBirth = Student.DateOfBirth;
                existingStudent.Gender = Student.Gender;
                existingStudent.ContactNumber = Student.ContactNumber;
                existingStudent.Address.Country = Student.Address.Country;
                existingStudent.Address.State = Student.Address.State;
                existingStudent.Address.City = Student.Address.City;

                TempData["SuccessMessage"] = "Student data updated successfully!";
            }
            else
            {
                // Add new student
                Student.Id = GenerateUniqueId(StudentList);
                StudentList.Add(Student);
                TempData["SuccessMessage"] = "New student data saved successfully!";
            }

            StudentDataHelper.SaveStudents(FilePath, StudentList);
            return RedirectToPage();
        }

        private string GenerateUniqueId(List<Student> students)
        {
            var yearStr = DateTime.Now.Year.ToString(); // e.g. "2025"

            var existingIds = students
                .Where(s => !string.IsNullOrEmpty(s.Id) && s.Id.StartsWith(yearStr))
                .Select(s => s.Id)
                .ToList();

            var suffixes = existingIds
                .Select(id =>
                {
                    var suffix = id.Substring(yearStr.Length);
                    return int.TryParse(suffix, out int n) ? n : 0;
                })
                .ToList();

            int nextNumber = suffixes.Any() ? suffixes.Max() + 1 : 1;
            return $"{yearStr}{nextNumber.ToString("D3")}";
        }

        public IActionResult OnPostEdit(string id)
        {
            StudentList = StudentDataHelper.LoadStudents(FilePath);
            Student = StudentList.FirstOrDefault(s => s.Id == id) ?? new Student();
            return Page();
        }

        public IActionResult OnPostDelete(string id)
        {
            StudentList = StudentDataHelper.LoadStudents(FilePath);
            var student = StudentList.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                student.Status = "Deleted"; // Soft-delete
                StudentDataHelper.SaveStudents(FilePath, StudentList);
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }
            return RedirectToPage();
        }

    }
}