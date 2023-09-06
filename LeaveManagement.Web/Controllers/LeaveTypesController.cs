using AutoMapper;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Dto;
using LeaveManagement.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    [Authorize]
    public class LeaveTypesController : BaseApiController
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;
        private readonly IMapper _mapper;
        public LeaveTypesController(ILeaveTypesRepository leaveTypesRepository,IMapper mapper)
        {
            _mapper = mapper;
            _leaveTypesRepository = leaveTypesRepository;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = _mapper.Map<List<LeaveTypesVM>>(await _leaveTypesRepository.GetAllAsync()); 
            return Ok(leaveTypes);
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
                return NotFound();
            }
            var leaveTypeVM =_mapper.Map<LeaveTypesVM>(leavetype);
            return Ok(leaveTypeVM);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(string id,LeaveTypesVM leaveTypesVM)
        {  
            if(id != leaveTypesVM.Id)
            {
                return NotFound();
            }
            var leaveType = await _leaveTypesRepository.GetAsync(id);
            if(leaveType == null)
            {
                return NotFound();
            }
            _mapper.Map(leaveTypesVM,leaveType);
            await _leaveTypesRepository.UpdateAsync(leaveType);
            return Ok(leaveTypesVM);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveType(string id)
        {
            var leaveType = await _leaveTypesRepository.GetAsync(id);
            if(leaveType==null)
            {
                return NotFound();
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