using BuildingsWebApi.Models;
using BuildingsWebApi.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace BuildingsWebApi.Controllers
{
    
    [Route("/api/[controller]")]
    public class FlatsController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult Index() { return View(); }
        [HttpGet]
        public async Task<IEnumerable<FlatWithPrice>> Get([FromQuery] Filter filter = null)
        {
            var query = $"SELECT f.ID, CountRooms, Link, Area, Price, Date FROM Flat AS f JOIN PriceHistory AS ph ON ph.FlatId = f.ID" +
                $" WHERE ph.Date = (SELECT MAX(Date) FROM PriceHistory WHERE FlatId = f.ID)";
            if(filter != null && !filter.IsEmpty)
            {
                var queryParams = new List<string>();
                if (filter.Id.HasValue)
                    queryParams.Add($"f.ID = {filter.Id}");
                if (filter.CountRooms.HasValue)
                    queryParams.Add($"CountRooms = {filter.CountRooms}");
                if (!string.IsNullOrEmpty(filter.Link))
                    queryParams.Add($"Link = '{filter.Link}'");
                if (filter.Area.HasValue)
                    queryParams.Add($"Area = {filter.Area.Value.ToString().Replace(',', '.')}");
                query += " AND " + string.Join(" AND ", queryParams);
            }
            List<FlatWithPrice> flats = new();
            await DbConnection.ExecuteQueryReadAsync(query, (reader) =>
            {
                try
                {
                    while (reader.Read())
                    {
                        flats.Add(new FlatWithPrice
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            CountRooms = reader.GetInt32(reader.GetOrdinal("CountRooms")),
                            Link = reader.GetString(reader.GetOrdinal("Link")),
                            Area = reader.GetFloat(reader.GetOrdinal("Area")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                        });
                    }
                }
                catch { throw new Exception("Не удалось считать данные!"); }
            });

            return flats;
        }
        [HttpGet("{id}")]
        public async Task<FlatWithPrice> Get(int id)
        {
            var query = $"SELECT f.ID, CountRooms, Link, Area, Price, Date FROM Flat AS f JOIN PriceHistory AS ph ON ph.FlatId = f.ID" +
                $" WHERE ph.Date = (SELECT MAX(Date) FROM PriceHistory WHERE FlatId = f.ID) AND f.ID = {id}";
            FlatWithPrice flat = null;
            await DbConnection.ExecuteQueryReadAsync(query, (reader) =>
            {
                try
                {
                    if (reader.Read())
                    {
                        flat = new FlatWithPrice
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            CountRooms = reader.GetInt32(reader.GetOrdinal("CountRooms")),
                            Link = reader.GetString(reader.GetOrdinal("Link")),
                            Area = reader.GetFloat(reader.GetOrdinal("Area")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                        };
                    }
                }
                catch { throw new Exception("Не удалось считать данные!"); }
            });

            return flat;
        }

        [HttpPost]
        public async Task<IActionResult> Post(FlatWithPrice newFlat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var query = $"INSERT INTO Flat (CountRooms, Link, Area) VALUES ({newFlat.CountRooms}, '{newFlat.Link}', {newFlat.Area?.ToString().Replace(',', '.')})\n";
                query += $"INSERT INTO PriceHistory (FlatId, Price, Date) VALUES (SCOPE_IDENTITY(), '{newFlat.Price?.ToString().Replace(',', '.')}', '{DateTime.Now}')";
                await DbConnection.ExecuteQueryAsync(query);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put(FlatWithPrice newFlat)
        {
            try
            {
                if (!ModelState.IsValid)
                { 
                    return BadRequest();
                }
                var query = $"UPDATE Flat SET CountRooms = {newFlat.CountRooms}, Link = '{newFlat.Link}', Area = {newFlat.Area?.ToString().Replace(',','.')} WHERE ID = {newFlat.Id}\n";
                var oldFlat = await Get(new Filter { Id = newFlat.Id });
                if(oldFlat.First().Price != newFlat.Price)
                    query += $"INSERT INTO PriceHistory (FlatId, Price, Date) VALUES ({newFlat.Id}, '{newFlat.Price?.ToString().Replace(',', '.')}', '{DateTime.Now}')";
                await DbConnection.ExecuteQueryAsync(query);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var query = $"DELETE FROM Flat WHERE ID = {id}";
                await DbConnection.ExecuteQueryAsync(query);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
