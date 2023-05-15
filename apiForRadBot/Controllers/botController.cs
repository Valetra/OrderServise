using Microsoft.AspNetCore.Mvc;

namespace apiForRadBot.Data;

[Route("api/[controller]")]
[ApiController]
public class botController : ControllerBase
{
    // GET: api/bot
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/bot/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/bot
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/bot/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/bot/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
