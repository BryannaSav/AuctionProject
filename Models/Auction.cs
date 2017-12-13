using System;
using System.Collections.Generic;

namespace AuctionProject.Models
{
    public class Auction : BaseEntity
    {
        public int AuctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float StartingBid { get; set; }
        public DateTime EndDate { get; set; }
        public float HighestBid { get; set; }
        
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public int BidderId { get; set; }
        public User Bidder { get; set; }




    }
}