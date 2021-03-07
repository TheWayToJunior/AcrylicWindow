using System;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class ClientEntity : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }

        //public ICollection<Schedule> VisitingDays { get; set; }
    }

    //public class ScheduleEntity : EntityBase<Guid>
    //{
    //    public DayOfWeek DayOfWeek { get; set; }

    //    public TimeSpan From { get; set; }

    //    public TimeSpan To { get; set; }
    //}
}
