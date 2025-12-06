using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using System.Text.RegularExpressions;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> Users = new List<User>();

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound(new { message = "User not found" });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Name))
                    return BadRequest(new { message = "Name is required" });

                if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return BadRequest(new { message = "Valid email is required" });

                user.Id = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1;
                Users.Add(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding user", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound(new { message = "User not found" });

                if (string.IsNullOrWhiteSpace(updatedUser.Name))
                    return BadRequest(new { message = "Name is required" });

                if (string.IsNullOrWhiteSpace(updatedUser.Email) || !Regex.IsMatch(updatedUser.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return BadRequest(new { message = "Valid email is required" });

                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating user", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound(new { message = "User not found" });

                Users.Remove(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting user", error = ex.Message });
            }
        }
    }
}
