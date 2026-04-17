using Microsoft.AspNetCore.Mvc;
using BuggyBackend.Models;
using BuggyBackend.Services;

namespace BuggyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult<List<Member>> GetAll()
        {
            var members = _memberService.GetAllMembers();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public ActionResult<Member> GetById(int id)
        {
            try
            {
                var member = _memberService.GetMemberById(id);
                if (member == null)
                {
                    return NotFound($"Member with ID {id} not found");
                }
                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public ActionResult<Member> GetByEmail(string email)
        {
            try
            {
                var member = _memberService.GetMemberByEmail(email);
                if (member == null)
                {
                    return NotFound($"Member with email {email} not found");
                }
                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Member> Create([FromBody] Member member)
        {
            try
            {
                var createdMember = _memberService.AddMember(member);
                return CreatedAtAction(nameof(GetById), new { id = createdMember.Id }, createdMember);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Member> Update(int id, [FromBody] Member member)
        {
            try
            {
                var updatedMember = _memberService.UpdateMember(id, member);
                return Ok(updatedMember);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _memberService.DeleteMember(id);
                if (!result)
                {
                    return NotFound($"Member with ID {id} not found");
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
