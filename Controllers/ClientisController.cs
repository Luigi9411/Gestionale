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

namespace Gestionale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientisController : ControllerBase
    {
        private readonly AgentiContext _context;
        private readonly IConfiguration _configuration;

        public ClientisController(AgentiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Clientis
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Clienti>>> GetClientis()
        //{
        //  if (_context.Clientis == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Clientis.ToListAsync();
        //}



        [HttpGet]
        public IActionResult GetClienti(string ricercato, string id_agente)
        {
            string connectionString = _configuration.GetConnectionString("MainDb");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RSP_cerca_cliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ricercato", SqlDbType.VarChar, 100).Value = ricercato;
                    cmd.Parameters.Add("@id_agente", SqlDbType.SmallInt).Value = id_agente;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    List<Clienti> clientiList = new List<Clienti>();
                    while (rdr.Read())
                    {
                        Clienti clienti = new Clienti();
                        clienti.Ragionesociale = rdr["Nominativo"].ToString();
                        clienti.IdAgente = rdr["Id"] as short?;

                        clientiList.Add(clienti);
                    }

                    return Ok(clientiList);
                }
            }
        }


        // GET: api/Clientis/Details/5
        [HttpGet("Details/{id}")]
        public IActionResult GetClientDetails(short id)
        {
            string connectionString = _configuration.GetConnectionString("MainDb");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RSP_carica_cliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_cliente", SqlDbType.SmallInt).Value = id;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    Clienti clienti = null;
                    if (rdr.Read())
                    {
                        clienti = new Clienti();
                        clienti.Ragionesociale = rdr["ragionesociale"].ToString();
                        clienti.Indirizzo = rdr["indirizzo"].ToString();
                        clienti.Cap = rdr["cap"].ToString();
                        clienti.Citta = rdr["citta"].ToString();
                        clienti.Prov = rdr["prov"].ToString();
                        clienti.IdAgente = rdr["id_agente"] as short?;
                    }

                    if (clienti == null)
                    {
                        return NotFound();
                    }

                    return Ok(clienti);
                }
            }
        }


        // GET: api/Clientis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clienti>> GetClienti(short id)
        {
          if (_context.Clientis == null)
          {
              return NotFound();
          }
            var clienti = await _context.Clientis.FindAsync(id);

            if (clienti == null)
            {
                return NotFound();
            }

            return clienti;
        }

        // PUT: api/Clientis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClienti(short id, Clienti clienti)
        {
            if (id != clienti.Id)
            {
                return BadRequest();
            }

            _context.Entry(clienti).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientiExists(id))
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

        // POST: api/Clientis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Clienti>> PostClienti(Clienti clienti)
        {
          if (_context.Clientis == null)
          {
              return Problem("Entity set 'AgentiContext.Clientis'  is null.");
          }
            _context.Clientis.Add(clienti);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClienti", new { id = clienti.Id }, clienti);
        }

        // DELETE: api/Clientis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClienti(short id)
        {
            if (_context.Clientis == null)
            {
                return NotFound();
            }
            var clienti = await _context.Clientis.FindAsync(id);
            if (clienti == null)
            {
                return NotFound();
            }

            _context.Clientis.Remove(clienti);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientiExists(short id)
        {
            return (_context.Clientis?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
