using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UIAWebApp.Data;
using UIAWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using UIAWebApp.Models.FlightViewModels;

namespace UIAWebApp.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public FlightsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<FlightsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .SingleOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,Origin,Destination,DepartureDateTime,ArrivalDateTime,Fare")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,Origin,Destination,DepartureDateTime,ArrivalDateTime,Fare")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .SingleOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FlightId == id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchFlight()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchFlight(SearchFlightViewModel model)
        {
            if (!model.RoundTrip)
            {
                ModelState.Remove("ReturnDate");
            }

            if (ModelState.IsValid)
            {
                var departFlight = from f in _context.Flights
                                   where f.Origin == model.Origin
                                   && (f.Destination == model.Destination)
                                   && (f.DepartureDateTime.Date == model.DepartureDate)
                                   select f;
                SearchFlightResultViewModel result = new SearchFlightResultViewModel
                {
                    Passenger = model.Passenger,
                    Depart = await departFlight.ToListAsync(),
                    Origin = model.Origin,
                    Destination = model.Destination,
                    RoundTrip = model.RoundTrip,
                    DepartDate = model.DepartureDate,
                };

                if (model.RoundTrip)
                {
                    var returnFlight = from f in _context.Flights
                                       where f.Origin == model.Destination
                                       && (f.Destination == model.Origin)
                                       && (f.DepartureDateTime.Date == model.ReturnDate)
                                       select f;
                    result.Return = await returnFlight.ToListAsync();
                    result.ReturnDate = model.ReturnDate;
                }

                return View(nameof(SearchFlightResult), result);
            }

            //has error fallback
            return View(model);
        }

        public IActionResult SearchFlightResult(SearchFlightResultViewModel result)
        {
            return View(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookFlight(int departid, int passenger, int returnid = 0)
        {
            var user = await _userManager.GetUserAsync(User);
            Flight departFlight = await _context.Flights.SingleOrDefaultAsync(m => m.FlightId == departid);

            FlightBooking flightBooking = new FlightBooking
            {
                Passenger = passenger,
                DepartFlightId = departid,
                DepartFlight = departFlight,
                UserName = user.UserName,
                TotalPrice = passenger * departFlight.Fare
            };
            if (returnid > 0)
            {
                Flight returnFlight = await _context.Flights.SingleOrDefaultAsync(m => m.FlightId == returnid);
                flightBooking.TotalPrice = flightBooking.TotalPrice + (passenger * returnFlight.Fare);
                flightBooking.ReturnFlight = returnFlight;
                flightBooking.ReturnFlightId = returnid;
            }
            _logger.LogInformation(user.UserName + " " + flightBooking.TotalPrice);
            _context.Add(flightBooking);
            await _context.SaveChangesAsync();

            return Json(new { result = flightBooking.FlightBookingId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BookedFlights()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = from b in _context.FlightBookings
                           where b.UserName == user.UserName
                           select b;
            return View(await bookings.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BookingDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flightBooking = await _context.FlightBookings
                .SingleOrDefaultAsync(m => m.FlightBookingId == id);
            if (flightBooking == null)
            {
                return NotFound();
            }
            return View(flightBooking);
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}
