using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestionale.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gestionale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentisController : ControllerBase
    {
        private readonly AgentiContext _context;
        private readonly IConfiguration _configuration;

        public AgentisController(AgentiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Agentis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agenti>>> GetAgentis()
        {
            if (_context.Agentis == null)
            {
                return NotFound();
            }
            return await _context.Agentis.ToListAsync();
        }

        // GET: api/Agentis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agenti>> GetAgenti(short id)
        {
            if (_context.Agentis == null)
            {
                return NotFound();
            }
            var agenti = await _context.Agentis.FindAsync(id);

            if (agenti == null)
            {
                return NotFound();
            }

            return agenti;
        }

        // PUT: api/Agentis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgenti(short id, Agenti agenti)
        {
            if (id != agenti.Id)
            {
                return BadRequest();
            }

            _context.Entry(agenti).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public IActionResult Authenticate(string userId, string password)
        {
            string connectionString = _configuration.GetConnectionString("MainDb");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("RSP_cerca_agente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Ui", userId));
                    command.Parameters.Add(new SqlParameter("@Pw", password));

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        short id = reader.GetInt16(0);
                        string nomeAgente = reader.GetString(1);

                        if (id == 0)
                        {
                            return Unauthorized("L'agente non esiste o è bloccato.");
                        }

                        // Crea un nuovo oggetto Agenti
                        Agenti agente = new Agenti();
                        agente.Id = id;
                        agente.Agente = nomeAgente;

                        return Ok(new { Id = id, NomeAgente = nomeAgente });
                    }
                    else
                    {
                        return Unauthorized("L'agente non esiste o è bloccato.");
                    }
                }
            }
        }

        //[HttpPost]
        //public IActionResult Authenticate(string userId, string password)
        //{
        //    string connectionString = _configuration.GetConnectionString("MainDb");

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand("RSP_cerca_agente", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add(new SqlParameter("@Ui", userId));
        //            command.Parameters.Add(new SqlParameter("@Pw", password));

        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();

        //            if (reader.HasRows)
        //            {
        //                reader.Read();
        //                short id = reader.GetInt16(0);
        //                string nomeAgente = reader.GetString(1);

        //                if (id == 0)
        //                {
        //                    return Unauthorized("L'agente non esiste o è bloccato.");
        //                }

        //                // Genera il token JWT
        //                var tokenHandler = new JwtSecurityTokenHandler();
        //                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        //                var tokenDescriptor = new SecurityTokenDescriptor
        //                {
        //                    Subject = new ClaimsIdentity(new Claim[]
        //                    {
        //                new Claim(ClaimTypes.Name, userId)
        //                    }),
        //                    Expires = DateTime.UtcNow.AddDays(7),
        //                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //                };
        //                var token = tokenHandler.CreateToken(tokenDescriptor);
        //                var tokenString = tokenHandler.WriteToken(token);

        //                return Ok(new { Id = id, NomeAgente = nomeAgente, Token = tokenString });
        //            }
        //            else
        //            {
        //                return Unauthorized("L'agente non esiste o è bloccato.");
        //            }
        //        }
        //    }
        //}


        // DELETE: api/Agentis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenti(short id)
        {
            if (_context.Agentis == null)
            {
                return NotFound();
            }
            var agenti = await _context.Agentis.FindAsync(id);
            if (agenti == null)
            {
                return NotFound();
            }

            _context.Agentis.Remove(agenti);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgentiExists(short id)
        {
            return (_context.Agentis?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
