using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MyFirstWebApp1.Models;

namespace MyFirstWebApp1.Helpers
{
    public static class StudentDataHelper
    {
        public static List<Student> LoadStudents(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Student>();

            var content = File.ReadAllText(filePath);
            return string.IsNullOrWhiteSpace(content)
                ? new List<Student>()
                : JsonSerializer.Deserialize<List<Student>>(content) ?? new List<Student>();
        }

        public static void SaveStudents(string filePath, List<Student> students)
        {
            var jsonData = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
    }
}

