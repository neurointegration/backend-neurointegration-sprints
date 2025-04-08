using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class SprintResponse
    {
        public long id {  get; set; } // TODO удалить
        public long Number { get; set; }
        public long UserId { get; set; }
        public int WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Dictionary<int, SprintWeek> Weeks { get; set; } = default!;
    }
}
