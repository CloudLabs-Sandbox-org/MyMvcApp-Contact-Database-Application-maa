using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class UserController : Controller
{
    public static List<User> userlist = new List<User>();

    // GET: User
    public ActionResult Index(string? searchString = null)
    {
        IEnumerable<User> users = userlist;
        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u =>
                (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            );
        }
        ViewBag.SearchString = searchString;
        return View(users);
    }

    // GET: User/Details/5
    public ActionResult Details(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // GET: User/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            if (userlist.Count > 0)
            {
                user.Id = userlist.Max(u => u.Id) + 1;
            }
            else
            {
                user.Id = 1;
            }
            userlist.Add(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: User/Edit/5
    public ActionResult Edit(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingUser = userlist.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: User/Delete/5
    public ActionResult Delete(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        userlist.Remove(user);
        return RedirectToAction(nameof(Index));
    }
}
