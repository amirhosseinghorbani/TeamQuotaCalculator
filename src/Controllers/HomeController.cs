using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamQuotaCalculator.Data;
using TeamQuotaCalculator.Models;

namespace TeamQuotaCalculator.Controllers
{
    public class HomeController : Controller
    {
        IAppDbContext _appDbContext;
        public HomeController(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ActionResult> Index()
        {
            await CalculationAsync();
            List<Manager> managers = await _appDbContext.Managers.ToListAsync();
            return View(managers);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Manager model)
        {
            if(!ModelState.IsValid)
                return View();
            model.Biz = model.Ceo = model.Dev = 0;
            applyCalculation(ref model);
            _appDbContext.Managers.Add(model);
            _appDbContext.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var manager = await _appDbContext.Managers.FindAsync(id);
            if( manager != null)
            {
                _appDbContext.Managers.Remove(manager);
                _appDbContext.Save();
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var manager = await _appDbContext.Managers.FindAsync(id);
            if( manager != null)
            {
                return View(nameof(Add), manager);
            }
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Manager model)
        {
            var manager = await _appDbContext.Managers.FindAsync(model.Id);
            if( manager != null)
            {
                manager.Name = model.Name;
                manager.q1  =  model.q1;
                manager.q2  =  model.q2;
                manager.q3  =  model.q3;
                manager.q4  =  model.q4;
                manager.q5  =  model.q5;
                manager.q6  =  model.q6;
                manager.q7  =  model.q7;
                manager.q8  =  model.q8;
                manager.q9  =  model.q9;
                manager.q10 =  model.q10;
                manager.q11 =  model.q11;
                manager.q12 =  model.q12;
                manager.q13 =  model.q13;
                manager.q14 =  model.q14;
                manager.q15 =  model.q15;
                applyCalculation(ref manager);
                _appDbContext.Managers.Update(manager);
                _appDbContext.Save();
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public void applyCalculation(ref Manager model)
        {
            model.Biz = model.Dev = model.Ceo = 0;

            if(model.q1)
            {
                model.Ceo += 10;
            }
            if(model.q2)
            {
                model.Dev += 10;
            }
            if(model.q3)
            {
                model.Ceo += 5;
                model.Biz += 3;
                model.Dev += 3;
            }
            if(model.q4)
            {
                model.Ceo += 1;
                model.Dev += 10;
            }
            if(model.q5)
            {
                model.Ratio = 1.3;
            } else
            {
                model.Ratio = 1;
            }
            if(model.q6)
            {
                model.Ceo += 10;
            }
            if(model.q7)
            {
                model.Ceo += 1;
                model.Dev += 10;
            }
            if(model.q8)
            {
                model.Ceo += 3;
                model.Biz += 3;
                model.Dev += 3;
            }
            if(model.q9)
            {
                model.Ceo += 2;
                model.Biz += 10;
            }
            if(model.q10)
            {
                model.Biz += 3;
            }
            if(model.q11)
            {
                model.Ceo += 5;
                model.Biz += 2;
                model.Dev += 2;
            }
            if(model.q12)
            {
                model.Ceo += 3;
                model.Biz += 5;
            }
            if(model.q13)
            {
                model.Ceo += 2;
                model.Biz += 2;
            }
            if(model.q14)
            {
                model.Ceo += 10;
                model.Biz += 1;
            }
            if(model.q15)
            {
                model.Ceo += 3;
                model.Biz += 10;
            }
        }

        public async Task CalculationAsync()
        {
            var managers = _appDbContext.Managers.ToList();
            double totalCeo = 0 , totalDev = 0,  totalBiz = 0;

            foreach(var manager in managers)
            {
                manager.Ceo = Math.Max((manager.Ceo / manager.Ratio), 0);
                manager.Dev = Math.Max((manager.Dev / manager.Ratio), 0);
                manager.Biz = Math.Max((manager.Biz / manager.Ratio), 0);
                await Edit(manager);

                totalBiz = managers.Sum(x=> x.Biz);
                totalDev = managers.Sum(x=> x.Dev);
                totalCeo = managers.Sum(x=> x.Ceo);
                var theCeos = managers.Where(x=>x.q1);
                double avgCeo = 0;
                if(theCeos.Any())
                    avgCeo = theCeos.Average(x=>x.Ceo);
                    
                if(totalBiz < 20)
                {
                    ViewBag.IsBizError = true;
                    ViewBag.BizError = "پیشنهاد می شود تیم بازاریابی و توسعه کسب و کار تیم تان را تقویت کنید.";
                }
                if(totalDev < 25)
                {
                    ViewBag.IsDevError = true;
                    ViewBag.DevError = "پییشنهاد می شود دنبال یک مسئول فنی برای استارتاپ تان بگردید";
                }
                if(avgCeo < 35)
                {
                    ViewBag.IsCeoError = true;
                    ViewBag.CeoError = "به نظر می آید تیم شما یک مدیر قوی تر نیاز دارد!";
                }
                
                var total = (totalCeo * 1.4) +
                             (totalDev * 1.2) +
                             (totalBiz);
                var managerShare = (manager.Ceo * 1.4) +
                                   (manager.Dev * 1.2) +
                                   (manager.Biz);
                manager.Share = managerShare / total;
                await Edit(manager);
            }
        }




        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
