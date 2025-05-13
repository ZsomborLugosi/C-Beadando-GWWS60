using GWWS60Beadando.Data;
using GWWS60Beadando.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace GWWS60Beadando.Controllers;

public class ShipController : Controller
{
    private readonly DockyardContext _context;
    private readonly IWebHostEnvironment _environment;

    public ShipController(DockyardContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Ships.ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Ship ship)
    {
        if (!ModelState.IsValid) return View(ship);
        _context.Ships.Add(ship);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Ship '{ship.Name}' has been added successfully!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        return ship == null ? NotFound() : View(ship);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Ship ship)
    {
        if (id != ship.Id) return NotFound();
        
        if (!ModelState.IsValid) return View(ship);
        
        try
        {
            _context.Update(ship);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Ship '{ship.Name}' has been updated successfully!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShipExists(ship.Id))
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

    public async Task<IActionResult> Delete(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        return ship == null ? NotFound() : View(ship);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ship = await _context.Ships.FindAsync(id);
        if (ship != null)
        {
            _context.Ships.Remove(ship);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Ship '{ship.Name}' has been deleted successfully!";
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
        
        // Define fonts
        var titleFont = new XFont("Arial", 18, XFontStyle.Bold);
        var headerFont = new XFont("Arial", 14, XFontStyle.Bold);
        var normalFont = new XFont("Arial", 12);
        
        // Draw company header/logo section
        gfx.DrawString("GLOBAL DOCKYARD MANAGEMENT", titleFont, XBrushes.DarkBlue,
            new XRect(0, 40, page.Width, 30), XStringFormats.TopCenter);
            
        gfx.DrawString("OFFICIAL SHIP MANIFEST", headerFont, XBrushes.DarkBlue,
            new XRect(0, 70, page.Width, 30), XStringFormats.TopCenter);
            
        // Draw horizontal line
        var pen = new XPen(XColors.DarkBlue, 1);
        gfx.DrawLine(pen, 50, 100, page.Width - 50, 100);
        
        // Ship details header
        gfx.DrawString("SHIP DETAILS", headerFont, XBrushes.Black,
            new XRect(50, 120, page.Width - 100, 30), XStringFormats.TopLeft);
        
        // Ship details content
        var yPos = 160;
        DrawInfoRow(gfx, normalFont, "Ship Name:", ship.Name, 50, yPos);
        yPos += 30;
        DrawInfoRow(gfx, normalFont, "Cargo Type:", ship.Cargo, 50, yPos);
        yPos += 30;
        DrawInfoRow(gfx, normalFont, "Weight:", $"{ship.Weight:N2} tons", 50, yPos);
        yPos += 30;
        DrawInfoRow(gfx, normalFont, "Arrival Date:", $"{ship.ArrivalDate:yyyy-MM-dd}", 50, yPos);
        yPos += 30;
        DrawInfoRow(gfx, normalFont, "Document ID:", $"SHIP-{ship.Id:D4}", 50, yPos);
        
        // Draw border around the manifest
        pen = new XPen(XColors.Black, 1.5);
        gfx.DrawRectangle(pen, 40, 30, page.Width - 80, page.Height - 80);
        
        // Footer with generated date
        gfx.DrawString($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", normalFont, XBrushes.Gray,
            new XRect(0, page.Height - 50, page.Width, 30), XStringFormats.TopCenter);

        document.Save(stream, false);
        return File(stream.ToArray(), "application/pdf", $"Manifest_{ship.Name}_{DateTime.Now:yyyyMMdd}.pdf");
    }
    
    private void DrawInfoRow(XGraphics gfx, XFont font, string label, string value, double x, double y)
    {
        var labelFont = new XFont(font.Name, font.Size, XFontStyle.Bold);
        
        gfx.DrawString(label, labelFont, XBrushes.Black, new XPoint(x, y));
        gfx.DrawString(value, font, XBrushes.Black, new XPoint(x + 150, y));
    }
    
    private bool ShipExists(int id)
    {
        return _context.Ships.Any(e => e.Id == id);
    }
}