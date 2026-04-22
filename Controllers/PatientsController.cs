using API_LM.Data;
using API_LM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_LM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/patient?page=1&pageSize=10&name=carlos&documentNumber=123
        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] string? documentNumber = null)
        {
            var query = _context.Patients.AsQueryable();

            // Filtros opcionales
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p =>
                    p.FirstName.Contains(name) ||
                    p.LastName.Contains(name));

            if (!string.IsNullOrWhiteSpace(documentNumber))
                query = query.Where(p => p.DocumentNumber.Contains(documentNumber));

            // Total de registros antes de paginar
            var total = await query.CountAsync();

            // Paginación
            var patients = await query
                .OrderBy(p => p.PatientId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                total,
                totalPages = (int)Math.Ceiling((double)total / pageSize),
                data = patients
            });
        }

        // GET: api/patient
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        //{
        //    var patients = await _context.Patients.ToListAsync();
        //    return Ok(patients);
        //}

        // GET: api/patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        // POST: api/patient
        [HttpPost]
        public async Task<ActionResult<Patient>> Create(Patient patient)
        {
            var exists = await _context.Patients.AnyAsync(p =>
                p.DocumentType == patient.DocumentType &&
                p.DocumentNumber == patient.DocumentNumber);

            if (exists)
                return Conflict(new { message = $"Ya existe un paciente con el documento {patient.DocumentType} - {patient.DocumentNumber}" });

            patient.CreatedAt = DateTime.UtcNow;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
        }

        // PUT: api/patient/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            if (id != patient.PatientId) return BadRequest();
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // GET: api/patient/created-after?fromDateCreation=2026-04-21
        [HttpGet("created-after")]
        public async Task<ActionResult> GetPatientsCreatedAfter([FromQuery] DateTime fromDate)
        {
            var patients = await _context.Set<PatientFieldsSP>()
            .FromSqlRaw("EXEC sp_GetPatientsByFieldCreatedAt @FromDate = {0}", fromDate)
            .ToListAsync();

            if (!patients.Any())
                return NotFound(new { message = $"No se encontraron pacientes creados después de la fecha: {fromDate:yyyy-MM-dd}" });

            return Ok(new
            {
                fromDate,
                total = patients.Count,
                data = patients
            });
        }

    }
}