using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using DevMeeting.API.Models;

namespace DevMeeting.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController: ControllerBase
    {
        private static readonly IList<User> UserCollection = new List<User>();
        
        /// <summary>
        /// Select all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public IActionResult GetAll()
        {
            if (UserCollection.Any())
                return Ok(UserCollection);
            
            return NoContent();
        }

        
        /// <summary>
        /// Select a single user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetById([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id)) 
                return BadRequest();            
            
            var user = UserCollection.SingleOrDefault(usr => usr.Id == id);
            if (user != null)
                return Ok(user);

            return NotFound();
        }
        
        /// <summary>
        /// Creates a single user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult Register([FromBody] User user)
        {
            UserCollection.Add(user);
            return Created(nameof(GetById), user);
        }

        /// <summary>
        /// Updates a single user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromRoute]string id, [FromBody] User user)
        {
            if (string.IsNullOrEmpty(id)) 
                return BadRequest();
            
            if (UserCollection.All(usr => usr.Id != id))
                return NotFound();

            var index = UserCollection.IndexOf(UserCollection.SingleOrDefault(usr => usr.Id == id));
            UserCollection[index] = user.WithId(id);
            return Accepted(user);
        }
        
                
        /// <summary>
        /// Removes a single user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Delete([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id)) 
                return BadRequest();
            
            var user = UserCollection.SingleOrDefault(usr => usr.Id == id);
            if (user is null) 
                return NotFound();
            
            UserCollection.Remove(user);
            return Ok();
        }
    }
}