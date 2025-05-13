using GWWS60Beadando.Data;
using GWWS60Beadando.Models;

namespace GWWS60Beadando.Controllers;

// Controllers/ShipController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

public class ShipController : Controller
{
    private readonly DockyardContext _context;

    public ShipController(DockyardContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Ships.ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Ship ship)
    {
        if (!ModelState.IsValid) return View(ship);
        _context.Ships.Add(ship);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        return ship == null ? NotFound() : View(ship);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Ship ship)
    {
        if (id != ship.Id || !ModelState.IsValid) return View(ship);
        _context.Update(ship);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        return ship == null ? NotFound() : View(ship);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        if (ship != null)
        {
            _context.Ships.Remove(ship);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Manifest(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        if (ship == null) return NotFound();

        using var stream = new MemoryStream();
        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Verdana", 12);

        gfx.DrawString($"Ship Manifest for: {ship.Name}", font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

        gfx.DrawString(
            $"Name: {ship.Name}\n" +
            $"Cargo: {ship.Cargo}\n" +
            $"Weight: {ship.Weight} tons\n" +
            $"Arrival Date: {ship.ArrivalDate:yyyy-MM-dd}",
            font, XBrushes.Black, new XRect(40, 100, page.Width, page.Height), XStringFormats.TopLeft
        );

        document.Save(stream, false);
        return File(stream.ToArray(), "application/pdf", $"Manifest_{ship.Name}.pdf");
    }

}
