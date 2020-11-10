using AcrylicWindow.IContract;

namespace AcrylicWindow.Model
{
    public class Employee : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
