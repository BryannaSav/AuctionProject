using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionProject.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public float Wallet { get; set; }

        [InverseProperty("Bidder")]
        public List<Auction> MyAuctions { get; set; }

        [InverseProperty("Creator")]
        public List<Auction> MyBids { get; set; }
        public User()
        {
            MyAuctions = new List<Auction>();
            MyBids = new List<Auction>();

        }
    }
}