using System.Linq;
using System.Web.Mvc;
using BranchManagementApp.Models;
using System.Data.Entity;
using ClosedXML.Excel;
using System.IO;
using System.Configuration;

[BasicAuthentication]   
public class HomeController : Controller
{
    private BranchDbContext db = new BranchDbContext();

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string username, string password)
    {
        string adminUsername = ConfigurationManager.AppSettings["AdminUsername"];
        string adminPassword = ConfigurationManager.AppSettings["AdminPassword"];

        if (username == adminUsername && password == adminPassword)
        {
            Session["IsAdmin"] = true; // Store admin session
            return RedirectToAction("Index");
        }

        ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
        return View();
    }

    public ActionResult Logout()
    {
        Session.Clear(); // Clear session
        return RedirectToAction("Login");
    }

    public ActionResult Index()
    {
        var branches = db.Branches.ToList();

        // Get unique branch names
        var uniqueBranches = branches
            .GroupBy(b => b.BranchName) // Group by BranchName
            .Select(g => g.First())    // Take the first branch in each group
            .ToList();

        // Populate the dropdown with branch names
        ViewBag.BranchDropdown = uniqueBranches.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.BranchName
        }).ToList();

        // Pass the branches to the view
        return View(branches);
    }

    [HttpPost]
    public JsonResult GetBranchDetails(string branchName)
    {
        var branches = db.Branches.Where(b => b.BranchName == branchName).ToList();
        if (branches == null)
        {
            return Json(new { success = false, message = "Branch not found" });
        }

        var result = branches.Select(b => new
        {
            b.Id,
            b.BranchName,
            b.PersonName,
            b.Age,
            b.MobileNumber,
            b.CompleteAddress,
            b.Profession
        });

        return Json(new { success = true, data = result });
    }




    // GET: Edit
    public ActionResult Edit(int id)
    {
        var branch = db.Branches.FirstOrDefault(b => b.Id == id);
        if (branch == null)
        {
            return HttpNotFound();
        }
        return View(branch);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Branch branch)
    {
        if (ModelState.IsValid)
        {
            db.Entry(branch).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(branch);
    }

    // GET: Delete
    public ActionResult Delete(int id)
    {
        var branch = db.Branches.FirstOrDefault(b => b.Id == id);
        if (branch == null)
        {
            return HttpNotFound();
        }
        return View(branch);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        var branch = db.Branches.Find(id);
        if (branch != null)
        {
            db.Branches.Remove(branch);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    // GET: Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Branch branch)
    {
        if (ModelState.IsValid)
        {
            db.Branches.Add(branch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(branch);
    }


    public ActionResult ExportToExcel(string branchName)
    {
        var branches = db.Branches.Where(b => b.BranchName == branchName).ToList();

        if (!branches.Any())
        {
            // If no data is found, return an empty file
            return new HttpStatusCodeResult(404, "No data available to export.");
        }


        // Create a new Excel workbook
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Branches");
            int currentRow = 1;

            // Add headers
            worksheet.Cell(currentRow, 1).Value = "Branch";
            worksheet.Cell(currentRow, 2).Value = "Person Name";
            worksheet.Cell(currentRow, 3).Value = "Age";
            worksheet.Cell(currentRow, 4).Value = "Mobile Number";
            worksheet.Cell(currentRow, 5).Value = "Complete Address";
            worksheet.Cell(currentRow, 6).Value = "Profession";

            // Add data
            foreach (var branch in branches)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = branch.BranchName;
                worksheet.Cell(currentRow, 2).Value = branch.PersonName;
                worksheet.Cell(currentRow, 3).Value = branch.Age;
                worksheet.Cell(currentRow, 4).Value = branch.MobileNumber;
                worksheet.Cell(currentRow, 5).Value = branch.CompleteAddress;
                worksheet.Cell(currentRow, 6).Value = branch.Profession;
            }

            // Return the Excel file
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Branches.xlsx");
            }
        }
    }



}
