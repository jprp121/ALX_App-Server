using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserController (DataContext context): BaseController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll(){
            return await context.Users.ToListAsync();
        }
        
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User?>> Get(int id)
        {
            return await context.Users.FindAsync(id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Post(User user){
             context.Add(user);
             await context.SaveChangesAsync();
             return user.Id;
        }
    }
}
