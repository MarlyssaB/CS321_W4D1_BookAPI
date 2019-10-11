using CS321_W4D1_BookAPI.ApiModels;
using CS321_W4D1_BookAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CS321_W4D1_BookAPI.Services;
using CS321_W4D1_BookAPI.ApiModels;

namespace CS321_W4D1_BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
       private readonly IPublisherService _publisherService;
       public PublishersController (IPublishersService publisherService)
        {
            _publisherService = publisherService;
        }
        // GET api/books
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_publisherService.GetAll().ToApiModels());
        }

        // get specific book by id
        // GET api/books/:id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var publisher = _publisherService.Get(id).ToApiModel();
            if (publisher == null) return NotFound();
            return Ok(publisher);

        }

        // create a new book
        // POST api/books
        [HttpPost]
        public IActionResult Post([FromBody] BookModel newBook)
        {
            try
            {
                // TODO: convert apimodel to domain model
                // add the new book
                _bookService.Add(newBook.ToDomainModel());
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("AddBook", ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }

            return CreatedAtAction("Get", new { Id = newBook.Id }, newBook);
        }

        // PUT api/books/:id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] BookModel updatedBook)
        {
            var book = _bookService.Update(updatedBook.ToDomainModel());
            if (book == null) return NotFound();
            return Ok(book.ToApiModel());
        }

        // DELETE api/books/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookService.Get(id);
            if (book == null) return NotFound();
            _bookService.Remove(book);
            return NoContent();
        }

        // GET api/author/{authorId}/books
        [HttpGet("/api/authors/{authorId}/books")]
        public IActionResult GetBooksForAuthor(int authorId)
        {
            var bookModels = _bookService
                .GetBooksForAuthor(authorId)
                .ToApiModels();

            return Ok(bookModels);
        }

        public IEnumerable<Book> GetBooksForPublisher(int publisherId)
        {
            return _bookContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Where(b => b.PublisherId == publisherId)
                .ToList();
        }
        // GET api/author/{authorId}/books
        // NOTE that the route specified in HttpGet begins with a forward slash.
        // This overrides the Route("/api/[controller]") specified on the BooksController
        // class.
        [HttpGet("/api/authors/{authorId}/books")]
        public IActionResult GetBooksForAuthor(int authorId)
        {
            var bookModels = _bookService
                .GetBooksForAuthor(authorId)
                .ToApiModels();

            return Ok(bookModels);
        }
    }
}