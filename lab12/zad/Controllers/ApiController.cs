using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zad.Data;

namespace zad.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private readonly ContextDb _context;

    public ApiController(ContextDb context)
    {
        _context = context;
    }

    // GET: Api/Info
    [HttpGet("Info")]
    public async Task<ActionResult<IEnumerable<MySchemaRecord>>> GetInfo()
    {
        if (_context.MySchema == null)
        {
            return NotFound();
        }
        return await _context.MySchema.ToListAsync();
    }

    // GET: Api/Info/5
    [HttpGet("Info/{id}")]
    public async Task<ActionResult<MySchemaRecord>> GetInfo(int id)
    {
        if (_context.MySchema == null)
        {
            return NotFound();
        }
        var mySchema = await _context.MySchema.FindAsync(id);

        if (mySchema == null)
        {
            return NotFound();
        }

        return mySchema;
    }

    private bool MySchemaExists(int id)
    {
        return (_context.MySchema?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // PUT: Api/PutInfo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("PutInfo/{id}")]
    public async Task<ActionResult<MySchemaRecord>> PutMySchema(int id, MySchemaRecord mySchemaRecord)
    {
        if (id != mySchemaRecord.Id)
        {
            return BadRequest();
        }

        var existing = await _context.MySchema.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        if (mySchemaRecord.apparatusId != null)
            existing.apparatusId = mySchemaRecord.apparatusId;
        if (mySchemaRecord.apparatusSensorType != null)
            existing.apparatusSensorType = mySchemaRecord.apparatusSensorType;
        if (mySchemaRecord.apparatusTubeType != null)
            existing.apparatusTubeType = mySchemaRecord.apparatusTubeType;
        if (mySchemaRecord.apparatusVersion != null)
            existing.apparatusVersion = mySchemaRecord.apparatusVersion;
        if (mySchemaRecord.calibrationFunction != null)
            existing.calibrationFunction = mySchemaRecord.calibrationFunction;
        if (mySchemaRecord.endTime != null)
            existing.endTime = mySchemaRecord.endTime;
        if (mySchemaRecord.hitsNumber != null)
            existing.hitsNumber = mySchemaRecord.hitsNumber;
        if (mySchemaRecord.startTime != null)
            existing.startTime = mySchemaRecord.startTime;
        if (mySchemaRecord.temperature != null)
            existing.temperature = mySchemaRecord.temperature;
        if (mySchemaRecord.value != null)
            existing.value = mySchemaRecord.value;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MySchemaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return existing;
    }

    // POST: Api/PostInfo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("PostInfo")]
    public async Task<ActionResult<MySchemaRecord>> PostInfo(MySchemaRecord mySchemaRecord)
    {
        if (_context.MySchema == null)
        {
            return Problem("Entity set 'ContextDb.MySchema' is null.");
        }
        _context.MySchema.Add(mySchemaRecord);
        await _context.SaveChangesAsync();

        return mySchemaRecord;
    }

    // DELETE: Api/DelInfo/5
    [HttpDelete("DelInfo/{id}")]
    public async Task<ActionResult<MySchemaRecord>>  DeleteInfo(int id)
    {
        if (_context.MySchema == null)
        {
            return NotFound();
        }
        var mySchema = await _context.MySchema.FindAsync(id);
        if (mySchema == null)
        {
            return NotFound();
        }
        _context.MySchema.Remove(mySchema);
        await _context.SaveChangesAsync();

        return mySchema;
    }
}