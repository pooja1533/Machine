using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IMachineRepository
    {
         public Task<bool> PostMachine(MachineDetail machine);
        public Task<List<MachineDetail>> GetMachine();
        public Task<string> DeleteMachine(long Id);
        public Task<MachineDetail> GetMachineById(long Id);
        public Task<bool> UpdateMachine(MachineDetail machine);
        public Task<bool> PostComment(MachineComment machineComment);
    }
}
