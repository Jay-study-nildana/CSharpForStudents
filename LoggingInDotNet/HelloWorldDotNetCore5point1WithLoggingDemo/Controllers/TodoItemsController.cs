using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloWorldDotNetCore5point1.DatabaseContext;
using HelloWorldDotNetCore5point1.Models;
//for logging support
using Microsoft.Extensions.Logging;

//command for creating controllers via scaffolding.
//    dotnet-aspnet-codegenerator controller -name TodoItemsController -async -api 
//    -m TodoItem -dc TodoContext -outDir Controllers

namespace HelloWorldDotNetCore5point1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        //let's add a logger
        //private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILogger<TodoItemsController> _logger;
        private string tempmessage;

        public TodoItemsController(TodoContext context, ILogger<TodoItemsController> logger)
        {
            _context = context;
            tempmessage = "This is a Log Message. ";
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            _logger.LogInformation(tempmessage + "List of all todo items returned");
            return await _context.TodoItems.ToListAsync();
        }


        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _logger.LogInformation(tempmessage + "Specific Todo Item with id:" + id + " has been returned");
            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation(tempmessage + "Specific Todo Item with id:" + id + " has been updated");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    _logger.LogWarning(tempmessage + "Specific Todo Item with id:" + id + " is not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogCritical(tempmessage + "Specific Todo Item with id:" + id + " has a problem: DbUpdateConcurrencyException");
                    // throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation(tempmessage + " Todo Item has been created");

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                _logger.LogWarning(tempmessage + "Specific Todo Item with id:" + id + " is not found");
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation(tempmessage + "Specific Todo Item with id:" + id + " has been deleted");

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
