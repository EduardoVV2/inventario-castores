package com.grupocastores.inventario_castores.controller;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.grupocastores.inventario_castores.model.Producto;
import com.grupocastores.inventario_castores.service.IProductoService;



@Controller
public class HomeController {

    @Autowired
    private IProductoService serviceProducto;

    @PostMapping("/login")
    public String login(@RequestParam("correo") String correo, Model model) {
        model.addAttribute("title", correo);
        return "home";
    }
    

    @GetMapping("/productos")
    public String mostrarProductos(Model model) {
        List<Producto> productos = serviceProducto.buscaProductos();
        model.addAttribute("productos", productos);
        return "productos"; // Retorna la vista de productos
    }

    @GetMapping("/")
    public String home(Model model) {
        model.addAttribute("title", "Inicio");
        model.addAttribute("fecha", new Date());
        return "home";
    }

}
