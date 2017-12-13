using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AuctionProject.Controllers
{
    public class HomeController : Controller
        {
        private UserContext _context;
    
        public HomeController(UserContext context)
        {
            _context = context;
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("CurUser")==null) //Kicks you off of site if you are not logged in
            {
                return RedirectToAction("LoginPage", "Login");
            }
            List<Auction> AllAuctions = _context.auctions.Include(auction=>auction.Creator).Include(auction=>auction.Bidder).OrderBy(a=>a.EndDate).ToList();

            // checking if any listings are expried. Updates wallet amounts, deletes listing, and redirects back to dashboard route to re-quary for all auctions
                foreach(var item in AllAuctions)
                {
                    if((DateTime)item.EndDate < DateTime.Now)
                    {
                        item.Bidder.Wallet-=item.HighestBid;
                        item.Creator.Wallet+=item.HighestBid;
                        _context.Remove(item);
                        _context.SaveChanges();
                        return RedirectToAction("Dashboard");
                    }
                }

            User CurUser = _context.users.SingleOrDefault(user=>user.UserId==(int)HttpContext.Session.GetInt32("CurId"));
            ViewBag.CurUser = CurUser;
            return View(AllAuctions);
        }

        [Route("NewAuction")]
        public IActionResult NewAuction()
        {
            if(HttpContext.Session.GetString("CurUser")==null) //Kicks you off of site if you are not logged in
            {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        [HttpPost]
        [Route("CreateAuction")]
        public IActionResult CreateAuction(AuctionViewModel MyAuction)
        {
            if(ModelState.IsValid){
                Auction NewAuction = new Auction{
                    Name = MyAuction.Name,
                    Description = MyAuction.Description,
                    StartingBid = MyAuction.StartingBid,
                    EndDate = MyAuction.EndDate,
                    CreatorId = (int)HttpContext.Session.GetInt32("CurId"),
                    HighestBid = MyAuction.StartingBid,
                    BidderId = (int)HttpContext.Session.GetInt32("CurId")

                };
                _context.Add(NewAuction);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewAuction");
        }

        [Route("ShowAuction/{Id}")]
        public IActionResult ShowAuction(int Id)
        {
            Auction CurAuction = _context.auctions.Include(auction=>auction.Creator).Include(auction=>auction.Bidder).SingleOrDefault(a=>a.AuctionId==Id);
            ViewBag.Message=TempData["Message"];
            return View(CurAuction);
        }

        [HttpPost]
        [Route("AddBid/{ID}")]
        public IActionResult AddBid(int ID, float NewBid)
        {
            Auction SelectedAuction = _context.auctions.Single(item=>item.AuctionId==ID);
            User CurUser = _context.users.SingleOrDefault(user=>user.UserId==(int)HttpContext.Session.GetInt32("CurId"));
            if(SelectedAuction.CreatorId==CurUser.UserId)
            {
                TempData["Message"] = "You can't place bids on your own listings";
            }
            else if(NewBid < SelectedAuction.HighestBid)
            {
                TempData["Message"] = "Your bid must be higher than the current highest bid.";
            }
            else if(NewBid > CurUser.Wallet){
                TempData["Message"] = "You can't bid more than you currently have in your wallet";
            }
            else
            {
                SelectedAuction.BidderId=CurUser.UserId;
                SelectedAuction.HighestBid=NewBid;
                _context.SaveChanges();
                TempData["Message"] = "Bid successfully placed!";
            }
            return RedirectToAction("ShowAuction", new { Id = ID});
        }

        [Route("Delete/{ID}")]
        public IActionResult Delete(int Id)
        {
            Auction CurItem = _context.auctions.SingleOrDefault(item=>item.AuctionId==Id);
            if((int)HttpContext.Session.GetInt32("CurId")==CurItem.CreatorId){
                _context.Remove(CurItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
