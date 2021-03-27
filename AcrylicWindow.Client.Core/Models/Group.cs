using AcrylicWindow.Client.Core.IContract;
using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Core.Models
{
    public class Group : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Employee Teacher { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public int Progress => GetProgress();

        private int GetProgress()
        {
            var totalDay = (End - Begin).Days;
            var now = (DateTime.Now - Begin).Days;

            if (totalDay == 0)
                return 0;

            var progress = now * 100 / totalDay;

            return progress > 100 ? 100 : progress;
        }
    }
}
