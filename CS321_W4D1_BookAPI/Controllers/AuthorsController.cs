﻿using CS321_W4D1_BookAPI.Models;
using CS321_W4D1_BookAPI.ApiModels;
using CS321_W4D1_BookAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CS321_W4D1_BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        // Constructor
        public PublishersController(IPublishersService publisherService)
        {
            // TODO: keep a reference to the service so we can use below
            _publisherService = publisherService;
        }

        // TODO: get all authors
        // GET api/authors
        [HttpGet]
        public IActionResult Get()
        {
            // TODO: return ApiModels instead of domain models
            var publisherModels = _publisherService
                .GetAll()
                .ToApiModels();
            return Ok(publisherModels);
        }

        // get specific author by id
        // GET api/authors/:id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // TODO: return ApiModel instead of domain model
            var publisher = _publisherService
                .Get(id)
                .ToApiModel();
            if (publisher == null) return NotFound();
            return Ok(publisher);
        }

        // create a new author
        // POST api/authors
        [HttpPost]
        public IActionResult Post([FromBody] PublisherModel newPublisher)
        {
            try
            {
                // TODO: convert the newAuthor to a domain model
                // add the new author
                _publisherService.Add(newPublisher.ToDomainModel());
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("AddPublisher", ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }

            // return a 201 Created status. This will also add a "location" header
            // with the URI of the new author. E.g., /api/authors/99, if the new is 99
            return CreatedAtAction("Get", new { Id = newPublisher.Id }, newPublisher);
        }

        // TODO: update an existing author
        // PUT api/authors/:id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PublisherModel updatedPublisher)
        {
            // TODO: convert updatedAuthor to a domain model
            var publisher = _publisherService.Update(updatedPublisher.ToDomainModel());
            if (publisher == null) return NotFound();
            return Ok(publisher);
        }

        // TODO: delete an existing author
        // DELETE api/authors/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisher = _apublisherService.Get(id);
            if (publisher == null) return NotFound();
            _publisherService.Remove(publisher);
            return NoContent();
        }
    }
}