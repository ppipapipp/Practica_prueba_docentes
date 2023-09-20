using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Eval.Models;

namespace Eval.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Docentes()
        {
            List<Docente> _listaDocentes = BD.GetDocentes();
                ViewBag.ListaDocentes = _listaDocentes;
                if (_listaDocentes.Count() > 0)
                {
                    int suma = 0;
                    foreach (Docente docente in _listaDocentes)
                    {
                        suma += docente.AntiguedadDocente;
                    }
                    ViewBag.PromedioDocente = suma / _listaDocentes.Count();
                }
                else
                {
                    ViewBag.PromedioDocente = -1;
                }
                return View();
        }

    [HttpGet] public IActionResult VerDetalleDocente(int IdDocente)
    {
        ViewBag.Empleado = BD.GetDocenteById(IdDocente);
        return View();
    }


        public IActionResult CrearDocente()
        {
            ViewBag.ListaDeDocentes = BD.GetDocentes();
            return View();
        }

[HttpPost]
    public IActionResult GuardarDocente(string NombreDocente, string FotoDocente, int IdMateria, int AntiguedadDocente)
    {
        int cont = 0;
        foreach (Docente docente in BD.GetDocentes())
        {
            if (docente.IdMateria == IdMateria)
            {
                cont++;
            }
        }
        if (cont >= 3)
        {
            ViewBag.ListaMaterias = BD.GetMaterias();
            ViewBag.Error = "Hay más de 3 docentes";
            return View("CrearDocente", "Home");
        }
        else
        {
            string NombreMateria = "";
            foreach (Materia materia in BD.GetMaterias())
            {
                if (materia.IdMateria == IdMateria)
                {
                    NombreMateria = materia.NombreMateria;
                }
            }

            // Creamos el objeto y se lo mandamos a la base de datos
            Docente nuevoDocente = new Docente(NombreDocente, FotoDocente, IdMateria, NombreMateria, AntiguedadDocente);
            BD.InsertDocente(nuevoDocente);
        }

        // Redireccionamos a Empleados
        return RedirectToAction("Docente", "Home");
    }



    [HttpGet]
        public IActionResult EliminarDocente(int IdDocente)
        {
            BD.DeleteDocenteById(IdDocente);
            return RedirectToAction("Docentes", "Home");
        }
    }
}