using System.ComponentModel.DataAnnotations;

namespace Project4
{
    class Airplane
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int MaxCountPassangers { get; set; }




        public ICollection<Flight> Flights { get; set; }
    }
}
