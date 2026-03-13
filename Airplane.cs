using System.ComponentModel.DataAnnotations;

namespace Project4
{
    //Entity - Airplane
    class Airplane
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Model { get; set; }
        //Relationship type : one to many (1...*)
        public int MaxCountPassangers { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }
}
