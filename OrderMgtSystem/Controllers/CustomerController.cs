using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgtSystem.Data;
using OrderMgtSystem.Models;

namespace OrderMgtSystem.Controllers
{
    /// <summary>
    /// Provides actions for creating, editing, deleting, and retrieving customer information within the order
    /// management system.
    /// </summary>
    /// <remarks>The CustomerController uses dependency injection to access the application's database context
    /// and implements standard CRUD operations for customer entities. All actions include error handling to ensure a
    /// robust user experience, returning appropriate views or results based on the outcome of each operation. This
    /// controller is intended for use in ASP.NET MVC applications and assumes that model validation and anti-forgery
    /// protection are in place for data-modifying actions.</remarks>
    public class CustomerController : Controller
    {
        #region Variable & Constructor declaration and initialization
        private readonly OrderMgtDBContext _context;
        /// <summary>
        /// Constructor with dependency injection for database context
        /// </summary>
        public CustomerController(OrderMgtDBContext context)
        {
            _context = context;
        }
        #endregion

        #region All the action or non action methods
        /// <summary>
        /// Asynchronously retrieves a list of customers ordered by creation date in descending order and displays them
        /// in the view.
        /// </summary>
        /// <remarks>If an exception is thrown while fetching customer data, the method returns a view
        /// with an empty customer list instead of propagating the error. The returned list is ordered so that the most
        /// recently created customers appear first.</remarks>
        /// <returns>A view that contains a list of customers. If an error occurs during data retrieval, the view contains an
        /// empty list.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _context.Customers
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                return View(new List<Customer>());
            }
        }


        /// <summary>
        /// Displays the view for creating a new customer.
        /// </summary>
        /// <returns>An IActionResult that renders the customer creation view with a new Customer model.</returns>
        public IActionResult CreateCustomer()
        {
            return View(new Customer());
        }

        /// <summary>
        /// Creates a new customer record using the provided customer information.
        /// </summary>
        /// <remarks>If the provided email address is already registered, a model validation error is
        /// added and the view is returned for correction. Any exceptions during the operation result in a general error
        /// message being added to the model state.</remarks>
        /// <param name="customer">The customer entity containing the details to be added. The email address must be unique and valid.</param>
        /// <returns>An IActionResult that redirects to the customer index view if creation succeeds; otherwise, returns the
        /// current view with validation errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind] Customer customer)
        {
            try
            {
                bool emailExists = await _context.Customers
                    .AnyAsync(c => c.Email == customer.Email);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(customer);
                }

                if (!ModelState.IsValid)
                {
                    return View(customer);
                }


                customer.CreatedDate = DateTime.Now;

                _context.Add(customer);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Customer created successfully! (ID: {customer.CustomerId})";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the customer");
                return View(customer);
            }
        }

        /// <summary>
        /// Displays the edit view for a customer specified by the unique identifier.
        /// </summary>
        /// <remarks>If the specified customer does not exist or an error occurs during retrieval, a
        /// NotFound result is returned.</remarks>
        /// <param name="id">The unique identifier of the customer to edit. Must not be null.</param>
        /// <returns>A view displaying the customer details if the customer is found; otherwise, a NotFound result.</returns>
        public async Task<IActionResult> EditCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Updates the details of an existing customer with the specified identifier.  
        /// </summary>
        /// <remarks>Validates the model state before updating the customer. Returns a NotFound result if
        /// the customer does not exist or if the provided identifier does not match the customer object. Handles
        /// concurrency exceptions and general errors by returning the view for user correction.</remarks>
        /// <param name="id">The unique identifier of the customer to update. Must match the CustomerId property of the provided customer
        /// object.</param>
        /// <param name="customer">The customer object containing the updated information. The object must satisfy all model validation
        /// requirements.</param>
        /// <returns>A redirect to the Index action if the update is successful; otherwise, returns the view with the customer
        /// object for correction.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, [Bind] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }

                var existingCustomer = await _context.Customers.FindAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;

                _context.Update(existingCustomer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Customer updated successfully!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the customer");
                return View(customer);
            }
        }

        /// <summary>
        /// Returns a view for deleting the customer with the specified identifier.
        /// </summary>
        /// <remarks>If an exception occurs during the operation, the method returns a NotFound
        /// result.</remarks>
        /// <param name="id">The unique identifier of the customer to delete. If null, the method returns a NotFound result.</param>
        /// <returns>An IActionResult that renders the delete view for the specified customer, or a NotFound result if the
        /// customer does not exist or the identifier is null.</returns>
        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes the customer with the specified identifier from the database.
        /// </summary>
        /// <remarks>If the customer with the specified identifier is not found, the method returns a
        /// NotFound result. If an exception occurs during the deletion process, an error message is stored in TempData
        /// and the user is redirected to the Index view.</remarks>
        /// <param name="id">The unique identifier of the customer to delete. Must correspond to an existing customer.</param>
        /// <returns>A redirect to the Index view if the deletion is successful or if an error occurs. Returns a NotFound result
        /// if the customer does not exist.</returns>
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }



                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Customer deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the customer";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Retrieves the details of a customer with the specified identifier.
        /// </summary>
        /// <remarks>If the customer with the specified identifier does not exist, or if an error occurs
        /// during retrieval, a NotFound result is returned.</remarks>
        /// <param name="id">The unique identifier of the customer to retrieve. Must be a valid integer; if null, a NotFound result is
        /// returned.</param>
        /// <returns>An IActionResult that renders the customer details view if the customer is found; otherwise, a NotFound
        /// result.</returns>
        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
