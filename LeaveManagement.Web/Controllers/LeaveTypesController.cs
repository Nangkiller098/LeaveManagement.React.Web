using AutoMapper;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Domain.Dto;
using LeaveManagement.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    [AllowAnonymous]
    public class LeaveTypesController : BaseApiController
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public LeaveTypesController(ILeaveTypesRepository leaveTypesRepository,IMapper mapper,ILogger<LeaveTypesController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _leaveTypesRepository = leaveTypesRepository;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = await _leaveTypesRepository.GetAllAsync();
            var record = _mapper.Map<List<LeaveTypesVM>>(leaveTypes); 
            return Ok(record);
        }
        [HttpGet("GetAll")]
    public async Task<ActionResult<PageResult<LeaveTypesVM>>> GetPageLeaveTypes([FromQuery] QueryParameters queryParameters)
        {
            var leavetypes = await _leaveTypesRepository.GetAllAsync<LeaveTypesVM>(queryParameters);
            return Ok(leavetypes);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypesVM leaveTypesVM)
        {
           var leavetype = _mapper.Map<LeaveTypes>(leaveTypesVM);
            await _leaveTypesRepository.AddAsync(leavetype);
            return Ok(leavetype);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveType(string id)
        {
            var leavetype= await _leaveTypesRepository.GetAsync(id);
            if (leavetype == null)
            {
                throw new NotFoundException(nameof(GetLeaveType),id);
            }
            var leaveTypeVM =_mapper.Map<LeaveTypesVM>(leavetype);
            return Ok(leaveTypeVM);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(string id,LeaveTypesVM leaveTypesVM)
        {  
            if(id != leaveTypesVM.Id)
            {
                return BadRequest("Invalid Record Id");
            }
            var leaveType = await _leaveTypesRepository.GetAsync(id);
            if(leaveType == null)
            {
                throw new NotFoundException(nameof(PutLeaveType),id);
            }
            _mapper.Map(leaveTypesVM,leaveType);
            await _leaveTypesRepository.UpdateAsync(leaveType);
            return Ok(leaveTypesVM);

        }
        [HttpDelete]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteLeaveType(string id)
        {
            var leaveType = await _leaveTypesRepository.GetAsync(id);
            if(leaveType==null)
            {
                throw new NotFoundException(nameof(DeleteLeaveType),id);
            }
            await _leaveTypesRepository.DeleteAsync(id);
            return Ok();
        }
        private async Task<bool> LeaveTypeExists(string id)
        {
            return await _leaveTypesRepository.Exits(id);
        }

    }
}