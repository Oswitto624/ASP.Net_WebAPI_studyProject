using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    //[Route("api/[controller]")] // http://localhost:5209/api/values
    [Route("api/values")] // http://localhost:5209/api/values
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _Logger;

        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

        [HttpGet]
        public IEnumerable<string> GetAll() => _Values.Values;

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //if(!_Values.ContainsKey(id))
            //    return NotFound();

            //return Ok(_Values[id]);

            if(_Values.TryGetValue(id, out var value))
                return Ok(value);
            return NotFound();
        }

        [HttpGet("count")]
        public int Count() => _Values.Count;

        [HttpPost] // POST -> http://localhost:5209/api/values
        [HttpPost("add")]  // POST -> http://localhost:5209/api/values/add
        public IActionResult Add([FromBody] string Value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            _Values[id] = Value;
            return CreatedAtAction(nameof(GetById), new { id }, Value);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int Id, [FromBody] string Value)
        {
            if(!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok();
        }

        [HttpDelete("{Id}")]  // POST -> http://localhost:5209/api/values/42
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values.Remove(Id);

            return Ok();
        }
    }
}
