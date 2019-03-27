using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repository, IMapper mapper)
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.GetUsers();
            
            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repository.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")] // usato per la modifica di risorse
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto) 
        {
            // controllo tramite il token se l'id passato corrisponde all'utente con cui siamo loggati             
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repository.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repository.SaveAll())
                return NoContent(); // non ritorno nulla se ho salvato i dati correttamente

            // altrimenti ritorno un'eccezione
            throw new Exception($"Updating user {id} failed on save");
        }

    }
}